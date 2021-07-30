using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FileServiceApi.Entities;
using FileServiceApi.Exceptions;
using FileServiceApi.Models;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FileServiceApi.Services
{
    public interface IUserService
    {
        public void Register(RegisterUserDto dto);
        void Upload(IFormFile file);
        string Login(LoginUserDto dto);
        OpenFileDto GetFile(string fileName);
    }

    public class UserService : IUserService
    {
        private readonly IPasswordHasher<User> _hasher;
        private readonly UserServiceDbContext _dbContext;
        private readonly Authentication _authentication;
        private readonly IUserContextService _userContextService;

        public UserService(IPasswordHasher<User> hasher, UserServiceDbContext dbContext, Authentication authentication, IUserContextService userContextService)
        {
            _hasher = hasher;
            _dbContext = dbContext;
            _authentication = authentication;
            _userContextService = userContextService;
        }

        public void Register(RegisterUserDto dto)
        {
            var email = dto.Email;
            var newUser = new User()
            {
                Email = email
            };

            var passwordHash = _hasher.HashPassword(newUser, dto.Password);

            var pathGenerator = PathGenerator(email);
            newUser.FilePath = new FilePath()
            {
                Path = $"/{pathGenerator}"
            };
            var currentPath = Directory.GetCurrentDirectory();
            Directory.CreateDirectory($"{currentPath}/UserFiles/{pathGenerator}");

            newUser.Password = passwordHash;

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();
        }



        public void Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var path = Directory.GetCurrentDirectory();
                var userId = _userContextService.GetId;
                if (userId is null)
                {
                    throw new ForbidException();
                }

                var userFilesPath = _dbContext.Users
                    .Include(u => u.FilePath)
                    .Include(u => u.FilesSet)
                    .FirstOrDefault(u => u.Id == userId);
                var fileName = file.FileName;

                var fullPath = $"{path}/UserFiles{userFilesPath?.FilePath.Path}/{fileName}";

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var user = _dbContext.Users.Include(u => u.FilesSet).FirstOrDefault(u => u.Id == userId);
                user.FilesSet.Add(new Files()
                {
                    FileName = fileName
                });
                _dbContext.SaveChanges();
            };

            
            

        }

        public string Login(LoginUserDto dto)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == dto.Email);

            if (user is null)
            {
                throw new NotFoundException("Incorrect email or password!");
            }

            var passwordHash = _hasher.VerifyHashedPassword(user, user.Password, dto.Password);

            if (passwordHash == PasswordVerificationResult.Failed)
            {
                throw new NotFoundException("Incorrect email or password!");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authentication.JwtIssuer));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authentication.JwtExpire);


            var token = new JwtSecurityToken(_authentication.JwtIssuer,
                _authentication.JwtIssuer,
                claims: claims,
                expires: expires,
                signingCredentials: cred
            );

            var tokenHandler = new JwtSecurityTokenHandler(); 
            return tokenHandler.WriteToken(token);

        }

        public OpenFileDto GetFile(string fileName)
        {
            var path = Directory.GetCurrentDirectory();

            var userId = _userContextService.GetId;

            var user = _dbContext.Users
                .Include(u => u.FilesSet)
                .Include(u => u.FilePath)
                .FirstOrDefault(c => c.Id == userId);

            if (user is null) throw new ForbidException();
            

            var file = user.FilesSet.FirstOrDefault(f => f.FileName.Contains(fileName));

            if (file is null) throw new NotFoundException("You don't have file of this name!");

            var fullPath = $"{path}/UserFiles{user.FilePath.Path}/{fileName}";

            var result = File.Exists(fullPath);
            if (!result) throw new NotFoundException("Bad implements!");

            var fileExtension = new FileExtensionContentTypeProvider();
            fileExtension.TryGetContentType(fullPath, out string contentType);

            return new OpenFileDto()
            {
                FilePath = fullPath,
                ContentType = contentType,
                FileName = fileName
            };

        }

        private string PathGenerator(string email)
        {
            char[] chars  = {'@', '.'};

            var path = email.ToLower().Replace('@', '_').Replace('.', '_');

            return path;
        }
    }
}
