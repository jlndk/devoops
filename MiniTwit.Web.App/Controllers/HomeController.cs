using System;
using System.Diagnostics;
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
        
        public HomeController(ILogger<HomeController> logger, IMessageRepository messageRepository,
            IUserRepository userRepository)
        {
            _logger = logger;
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> My_Timeline()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["Messages"] = await _messageRepository.ReadAllMessagesFromFollowedAsync(int.Parse(userId));
            return View();
        }
        
        public async Task<IActionResult> Index()
        {
            ViewData["Messages"] = await _messageRepository.ReadCountAsync();
            return View();
        }
        
        public async Task<IActionResult> User_Timeline(int? id)
        {
            if (id == null)
            {
                return Error();
            }
            ViewData["Messages"] = await _messageRepository.ReadAllMessagesFromUserAsync(id.Value);
            ViewData["ViewedUserId"] = id.Value;
            var user = await _userRepository.ReadAsync(id.Value);
            ViewData["ViewedUserName"] = user.UserName;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userId, out var followerId))
            {
                ViewData["IsFollowingUser"] = await _userRepository.IsUserFollowing(followerId, (int) id);
            }
            return View();
        }
        
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

        public async Task<IActionResult> Follow(int followeeId)
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
            return RedirectToAction("User_Timeline", new {id = followeeId});
        }

        public async Task<IActionResult> Unfollow(int followeeId)
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
            return RedirectToAction("User_Timeline", new {id = followeeId});
        }
    }
}