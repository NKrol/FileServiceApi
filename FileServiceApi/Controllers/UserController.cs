using System.IO;
using System.Net;
using FileServiceApi.Models;
using FileServiceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FileServiceApi.Controllers
{
    [Route("/api/account")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        [Route("register")]
        public ActionResult Register([FromBody] RegisterUserDto dto)
        {
            _userService.Register(dto);


            return Ok();
        }

        [HttpPost]
        [Route("upload")]
        [Authorize]
        public ActionResult Upload([FromForm] IFormFile file)
        {
            _userService.Upload(file);

            return Ok();
        }
        [HttpPost]
        [Route("login")]
        public ActionResult Login([FromBody] LoginUserDto dto)
        {
            var token = _userService.Login(dto);

            return Ok(token);
        }
        [HttpGet]
        [Route("getFile")]
        [ResponseCache(Duration = 1200, VaryByQueryKeys = new []{"fileName"})]
        public ActionResult GetFile([FromQuery] string fileName)
        {
            var openFile = _userService.GetFile(fileName);
            if (openFile is null)
            {
                return NoContent();
            }
            var readAllBytes = System.IO.File.ReadAllBytes(openFile.FilePath);


            return File(readAllBytes, openFile.ContentType, openFile.FileName);
        } 
        
    }
}