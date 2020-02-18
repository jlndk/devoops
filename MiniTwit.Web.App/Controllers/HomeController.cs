using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniTwit.Entities;
using MiniTwit.Web.App.Models;

namespace MiniTwit.Web.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMiniTwitContext _context;

        public HomeController(ILogger<HomeController> logger, MiniTwitContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["Messages"] = new List<Message> { //todo: Figure out how to import messages from context.
                new Message
                {
                    Author = new User
                    {
                        UserName = "Jakobis",
                        Email = "yo@simongreen.ninja"
                    },
                    Text = "YO waddup!?",
                    PubDate = 2
                }
            };
            return View();
        }
        
        public IActionResult LogIn()
        {
            return View();
        }
        
        public IActionResult Register()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
