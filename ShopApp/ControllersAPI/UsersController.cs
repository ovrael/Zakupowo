using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Security;
using ShopApp.DAL;
using ShopApp.Models;
using ShopApp.Utility;

namespace ShopApp.Controllers
{
    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        private ShopContext db = new ShopContext();


        // GET: api/Users/Avatar/5
        [Route("Avatar")]
        
        public IHttpActionResult GetAvatarURI(int userID)
        {
            User user = null;
            try
            {
                user = db.Users.Where(i => i.UserID == userID).First();
            }catch(Exception e)
            {

            }
           
               
            if (user == null)
            {
                return BadRequest("Couldn't find user!");
            }
        
            var avatarImage = user.AvatarImage;
            var uriBase = "http://192.168.0.103:45455/../";
            var uri = uriBase + "App_Files/Images/UserAvatars/DefaultAvatar.jpg";
            if (avatarImage != null) uri = uriBase  +  avatarImage.PathToFile;
            return Ok(uri);
        }
       
        //POST api/Users/Register
        [AllowAnonymous]
        [Route("Register")]
        public IHttpActionResult Register(RegisterBindingModel model)
        {
            DateTime birthdate = DateTime.Parse(model.BirthDate);
            User user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Login = model.Login,
                EncryptedPassword = Cryptographing.Encrypt(model.Password),
                Email = model.Email,
                BirthDate = birthdate,
                CreationDate = DateTime.Now,
                IsActivated = false

            };

            db.Users.Add(user);
            db.SaveChanges();
            var bucket = new Bucket
            {
                User = db.Users.Where(i => i.Login == user.Login).First()
            };

            db.Buckets.Add(bucket);
            db.SaveChanges();

            Task.Run(() => EmailManager.SendEmailAsync(EmailManager.EmailType.Registration, user.FirstName, user.LastName, user.Email));

            if (user != null)
            {
                return Ok();
            }
            else return BadRequest("Cannot register user");

        }

        [AllowAnonymous]
        [Route("Login")]
        public IHttpActionResult Login(LoginBindingModel model)
        {
            var login = model.Login;
            var password = Cryptographing.Encrypt(model.Password);

            var user = db.Users.Where(x => x.Login == login && x.EncryptedPassword == password).SingleOrDefault();


            if (user != null)
            {
                return Ok(user);
            }
            return BadRequest("User not found");

        }
      
}

    public class RegisterBindingModel
    {
        [Required]
        public string Email
        {
            get; set;
        }
        [Required]
        public string Password
        {
            get; set;
        }
        [Required]
        public string Login
        {
            get; set;
        }
     
        [Required]
        public string FirstName
        {
            get; set;
        }
        [Required]
        public string LastName
        {
            get; set;
        }
        [Required]
        public string BirthDate
        {
            get; set;
        }
       
        public string Phone
        {
            get; set;
        }
    }

    public class LoginBindingModel
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }

    }


}