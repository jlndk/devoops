using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniTwit.Entities;
using MiniTwit.Models;
using MiniTwit.Web.App.Models.Api;
using Newtonsoft.Json;

namespace MiniTwit.Web.App.Controllers
{
    public class ApiController : Controller
    {
        private readonly ILogger<ApiController> _logger;
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private static int _latest = 0;

        public ApiController(
            UserManager<User> userManager, 
            ILogger<ApiController> logger,
            IMessageRepository messageRepository,
            IUserRepository userRepository
        )
        {
            _userManager = userManager;
            _logger = logger;
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }

        private void LogInfo (string message)
        {
            _logger.LogInformation($"Latest:{_latest}, {message}");
        }

        //This function is defined, dont know if we need to use it.
        //The tests doesnt test for it.
        private bool RequestFromSimulator()
        {
            return (HttpContext.Request.Headers["Authorization"].Equals("Basic c2ltdWxhdG9yOnN1cGVyX3NhZmUh"));
        }

        private IActionResult NotAuthorizedError()
        {
            return StatusCode(403, new {status = 403, error_msg = "You are not authorized to use this resource!"});
        }

        [Route("[controller]/latest/")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetLatest()
        {
            return Json(new {latest = _latest});
        }

        private void UpdateLatest(int? latestMessage)
        {
            if (latestMessage != null)
            {
                _latest = latestMessage.Value;
            }
        }
        

        [Route("[controller]/msgs/")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetMessages([FromQuery(Name = "latest")] int? latestMessage,
            [FromQuery(Name = "no")] int number = 100)
        {
            UpdateLatest(latestMessage);
            if (!RequestFromSimulator())
            {
                return NotAuthorizedError();
            }
            var messages = (await _messageRepository.ReadCountAsync(number))
                .Select(m => new {content = m.Text, pub_date = m.PubDate, user = m.Author.UserName});
            
            return Json(messages);
        }

        [Route("[controller]/msgs/{username}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> UserGetMessages(string username,
            [FromQuery(Name = "latest")] int? latestMessage, [FromQuery(Name = "no")] int number = 100)
        {
            UpdateLatest(latestMessage);
            if (!RequestFromSimulator())
            {
                return NotAuthorizedError();
            }
            var user = await _userRepository.ReadAsyncByUsername(username);
            if (user == null)
            {
                LogInfo($"Invalid username '{username}' to get messages for");
                // TODO: This has to be another error, likely 500???
                return NotFound();
            }    
            var messages = (await _messageRepository.ReadCountFromUserAsync(user.Id,number))
                .Select(m => new {content = m.Text, pub_date = m.PubDate, user = m.Author.UserName});
            return Json(messages);
        }
        

        [Route("[controller]/msgs/{username}")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UserPostMessages(string username,
            [FromQuery(Name = "latest")] int? latestMessage, [FromBody] MessagePost messagePost)
        {
            UpdateLatest(latestMessage);
            UpdateLatest(messagePost.Latest);
            if (!RequestFromSimulator())
            {
                return NotAuthorizedError();
            }
            var user = await _userRepository.ReadAsyncByUsername(username);
            if (user == null || messagePost?.Content == null)
            {
                return NotFound();
            }

            var message = new Message
            {
                Author = user,
                AuthorId = user.Id,
                Flagged = 0,
                PubDate = DateTime.Now,
                Text = messagePost.Content
            };
            await _messageRepository.CreateAsync(message);
            return StatusCode(204, "");
        }

        public class RegisterPost
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Pwd { get; set; }
            public int? Latest { get; set; }
        }

        [Route("[controller]/register")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterPost registerPost,
            [FromQuery(Name = "latest")] int? latestMessage)
        {
            UpdateLatest(latestMessage);
            UpdateLatest(registerPost.Latest);
            if (!RequestFromSimulator())
            {
                return NotAuthorizedError();
            }
            if (registerPost?.Username == null)
            {
                return StatusCode(400, new {status = 400, error_msg = "You have to enter a username"});
            }

            if (registerPost.Email == null || !registerPost.Email.Contains("@"))
            {
                return StatusCode(400,new {status = 400, error_msg = "You have to enter a valid email address"});
            }

            if (registerPost.Pwd == null)
            {
                return StatusCode(400,new {status = 400, error_msg = "You have to enter a password"});
            }

            if ((await _userRepository.ReadAsyncByUsername(registerPost.Username)) != null)
            {
                return StatusCode(400,new {status = 400, error_msg = "The username is already taken"});
            }

            var user = new User
            {
                UserName = registerPost.Username,
                Email = registerPost.Email
            };
            var result = await _userManager.CreateAsync(user, registerPost.Pwd);
            if (!result.Succeeded)
            {
                LogInfo(registerPost.Username + " failed at creation, with result: " + result);
                return BadRequest(result.Errors);
            }
            LogInfo(registerPost.Username + " created a new account");
            return StatusCode(204, "");
        }

        [Route("[controller]/fllws/{username}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> UserGetFollows(string username,
            [FromQuery(Name = "latest")] int? latestMessage, [FromQuery(Name = "no")] int number = 100)
        {
            UpdateLatest(latestMessage);
            if (!RequestFromSimulator())
            {
                return NotAuthorizedError();
            }
            var user = await _userRepository.ReadAsyncByUsername(username);
            var followers = (await _userRepository.GetFollows(user.Id))?
                .Take(number)
                .Select(u => u.UserName);
            return Json(new {follows = followers});
        }

        

        [Route("[controller]/fllws/{username}")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UserFollow(string username, [FromQuery(Name = "latest")] int? latestMessage,
            [FromBody] PostFollow postFollow)
        {
            UpdateLatest(latestMessage);
            UpdateLatest(postFollow.Latest);
            if (!RequestFromSimulator())
            {
                return NotAuthorizedError();
            }
            var follower = await _userRepository.ReadAsyncByUsername(username);
            
            if (follower == null)
            {
                LogInfo($"Invalid follower username '{username}' in follow/unfollow action");
                // TODO: This has to be another error, likely 500???
                return StatusCode(404, "");
            }

            if (postFollow?.Follow != null)
            {
                var followee = await _userRepository.ReadAsyncByUsername(postFollow.Follow);
                if (followee == null)
                {
                    LogInfo($"Invalid followee username '{postFollow.Follow}' in follow operation");
                    // TODO: This has to be another error, likely 500???
                    return StatusCode(404, "");
                }
                LogInfo($"'{username}' followed '{postFollow.Follow}'");
                await _userRepository.AddFollowerAsync(followerId: follower.Id, followeeId: followee.Id);
            }
            else if (postFollow?.UnFollow != null)
            {
                var followee = await _userRepository.ReadAsyncByUsername(postFollow.UnFollow);
                if (followee == null)
                {
                    LogInfo($"Invalid followee username '{postFollow.UnFollow}' in unfollow operation");
                    // TODO: This has to be another error, likely 500???
                    return StatusCode(404, "");
                }
                LogInfo($"'{username}' unfollowed '{postFollow.UnFollow}'");
                await _userRepository.RemoveFollowerAsync(followerId: follower.Id, followeeId: followee.Id);
            }

            return StatusCode(204, "");
        }
    }
}