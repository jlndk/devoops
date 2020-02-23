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
        

        public HomeController(ILogger<HomeController> logger, IMessageRepository messageRepository, IUserRepository userRepository)
        {
            _logger = logger;
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["Messages"] = await _messageRepository.ReadAsync();
            return View();
        }

        [Route("/user/{id}")]
        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            ViewData["ViewedUserId"] = id.ToString();
            ViewData["ViewedUserName"] = (await _userRepository.ReadAsync(id)).UserName;
            ViewData["Messages"] = await _messageRepository.ReadAsync(id);
            return View();
        }

        // THIS SHOULD PROBALY NOT BE HERE. [Route("/msgs/{id}")] 
        [HttpPost]
        public async Task<IActionResult> PostMessage(Message message, string returnUrl = null) 
        {
            ViewData["ReturnUrl"] = returnUrl;
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Int32.TryParse(userId, out var actualId)) return View("Index");
            message.AuthorId = actualId;
            message.Author = await _userRepository.ReadAsync(actualId);
            message.PubDate = DateTime.Now;

            var (result, messageId) = await _messageRepository.CreateAsync(message);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
