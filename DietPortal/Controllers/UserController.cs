using BL;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace DietPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        IUserBl ubl;
        IUserInGroupBl uigbl;
        public UserController(IUserBl ubl,IUserInGroupBl uigbl)
        {

            this.ubl = ubl;
            this.uigbl = uigbl;
        }


        [HttpGet("{userId}")]
        public async Task<User> Get(int userId)
        {
          
            return await ubl.GetUser(userId);
        }
        [HttpGet ("{name}/{password}")]

        public async Task<User>LogIn(string name,string password)
        {
           
            return await ubl.LogIn(name,password);                                                      
                
        }

        [HttpGet("{id}/Group")]
        
        public async Task<List<User>> GetAllUsers(int id) 
        {
            return await uigbl.GetAllUsers(id);
        }
        [HttpGet]
        
        public async Task<List<User>> GetAllUsers()
        {
            return await ubl.GetAllUsers();
        }
        
        
       

        [HttpPost("image/{userId}"), DisableRequestSizeLimit]
       // [Route("{userId}")]
        public async Task PostImage(int userId, [FromForm] IFormFile image)
        {
            // int eventId= await ieventbl.PostBL(e, userId);
            var folderName = Path.Combine("Resources", "Images", userId.ToString());
            var directory = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            Directory.CreateDirectory(directory);
            string ImageFullPath = Path.Combine(folderName, image.FileName);
            string filePath = Path.Combine(directory, image.FileName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(fileStream);
            }
            User u = await ubl.GetUser(userId);
            User newUser = new User
            {
                Id = userId,
                IdentityNumber = u.IdentityNumber,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                DateOfBirth = u.DateOfBirth,
                Gender = u.Gender,
                Weight = u.Weight,
                Password = u.Password,
                //ProfileImageName=image.FileName,
                //ProfileImagePath= ImageFullPath                
            };
            await ubl.UpdateUser(newUser);


        }
        [HttpPost]
        public Task<int> AddUser([FromBody] User user)
        {
            return ubl.AddUser(user);
        }
        
        [HttpPost("/Group")]

        public async Task<int> AddUserInGroup([FromBody] UserInGroup userInGroup, [FromQuery] string? password = null)
        {
            return await uigbl.AddUserInGroup(userInGroup, password);
        }

        [HttpDelete("/UserInGroup")]

        public async Task DeleteUserInGroup([FromBody] UserInGroup userInGroup)
        {
            await uigbl.DeleteUserInGroup(userInGroup);
        }

        [HttpPut]
     
        public async Task<User> UpdateUser([FromBody] User user)
        {
            return await ubl.UpdateUser(user);
        }

       


    }
}
