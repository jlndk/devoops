using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniTwit.Entities;
using MiniTwit.Models;
using MiniTwit.Web.App.Models;

namespace MiniTwit.Web.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;

        private const int DefaultMessagesPerPage = 20;

        public HomeController(ILogger<HomeController> logger, IMessageRepository messageRepository,
            IUserRepository userRepository)
        {
            _logger = logger;
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }

		[Route("/my")]
        public async Task<IActionResult> My_Timeline(DateTime? dateOlderThan = null, DateTime? dateNewerThan = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            Func<DateTime?, DateTime?, int, Task<IEnumerable<Message>>> getMessages =
                (innerDateOlderThan, innerDateNewerThan, count) => _messageRepository.ReadMessagesFromFollowedWithinTimeAsync(
                    int.Parse(userId),
                    dateNewerThan: innerDateNewerThan,
                    dateOlderThan: innerDateOlderThan
                );
            await SetMessageViewData(dateOlderThan, dateNewerThan, getMessages);
            return View();
        }

		[Route("/")]
        public async Task<IActionResult> Index(DateTime? dateOlderThan = null, DateTime? dateNewerThan = null)
        {
            Func<DateTime?, DateTime?, int, Task<IEnumerable<Message>>> getMessages =
                (innerDateOlderThan, innerDateNewerThan, count) => _messageRepository.ReadManyWithinTimeAsync(
                    count,
                    dateNewerThan: innerDateNewerThan,
                    dateOlderThan: innerDateOlderThan
                );
            await SetMessageViewData(dateOlderThan, dateNewerThan, getMessages);
            return View();
        }

		[Route("/user/{username}")]
        public async Task<IActionResult> User_Timeline(
            string username,
            DateTime? dateOlderThan = null,
            DateTime? dateNewerThan = null
        )
        {
            if (username == null)
            {
                return Error();
            }

            var user = await _userRepository.ReadAsyncByUsername(username);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["ViewedUserName"] = username;
            Func<DateTime?, DateTime?, int, Task<IEnumerable<Message>>> getMessages =
                (innerDateOlderThan, innerDateNewerThan, count) => _messageRepository.ReadManyFromUserWithinTimeAsync(
                    user.Id,
                    count,
                    dateNewerThan: innerDateNewerThan,
                    dateOlderThan: innerDateOlderThan
                );
            await SetMessageViewData(dateOlderThan, dateNewerThan, getMessages);
            ViewData["ViewedUserId"] = user.Id;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userId, out var followerId))
            {
                ViewData["IsFollowingUser"] = await _userRepository.IsUserFollowing(followerId, user.Id);
            }
            return View();
        }

		[Route("/post")]
        [HttpPost]
        public async Task<IActionResult> PostMessage(Message message, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userId, out var actualId))
            {
                return View("Index");
            }
            message.AuthorId = actualId;
            message.Author = await _userRepository.ReadAsync(actualId);
            message.PubDate = DateTime.Now;
            await _messageRepository.CreateAsync(message);
            return RedirectToAction(nameof(Index), "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }

        public async Task<IActionResult> Follow(int followeeId, string viewedUserName)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userId, out var followerId))
            {
                return View("Index");
            }
            var result = await _userRepository.AddFollowerAsync(followerId, followeeId);
            if (result != MiniTwit.Models.Response.Created)
            {
                return View("Index");
            }
            _logger.LogInformation($"{userId} Followed {followeeId}");
            return RedirectToAction("User_Timeline", new {username = viewedUserName});
        }

        public async Task<IActionResult> Unfollow(int followeeId, string viewedUserName)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userId, out var followerId))
            {
                return View("Index");
            }
            var result = await _userRepository.RemoveFollowerAsync(followerId, followeeId);
            if (result != MiniTwit.Models.Response.Deleted)
            {
                return View("Index");
            }
            return RedirectToAction("User_Timeline", new {username = viewedUserName});
        }

        private async Task SetMessageViewData(DateTime? dateOlderThan, DateTime? dateNewerThan, Func<DateTime?, DateTime?, int, Task<IEnumerable<Message>>> getMessages)
        {
            var messages = await getMessages(dateOlderThan, dateNewerThan, DefaultMessagesPerPage);
            var anyMessages = messages != null && messages.Any();
            DateTime? newestDate = null;
            DateTime? oldestDate = null;
            var anyNewer = false;
            var anyOlder = false;
            if (anyMessages)
            {
                newestDate = messages.Max(t => t.PubDate);
                oldestDate = messages.Min(t => t.PubDate);
                anyNewer = (await getMessages(null, newestDate, 1)).Any();
                anyOlder = (await getMessages(oldestDate, null, 1)).Any();
            }
            ViewData["Messages"] = messages;
            ViewData["NewestDate"] = newestDate;
            ViewData["OldestDate"] = oldestDate;
            ViewData["ThereIsNextPage"] = anyOlder;
            ViewData["ThereIsPreviousPage"] = anyNewer;
        }
    }
}
