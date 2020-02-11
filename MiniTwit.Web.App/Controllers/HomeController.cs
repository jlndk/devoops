﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniTwit.Web.App.Models;

namespace MiniTwit.Web.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["Messages"] = new List<Message> {
                new Message
                {
                    Author = new User
                    {
                        Username = "Jakobis",
                        Email = "yo@simongreen.ninja"
                    },
                    Text = "YO waddup!?",
                    PubDate = 2
                }
            };
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Hola()
        {
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