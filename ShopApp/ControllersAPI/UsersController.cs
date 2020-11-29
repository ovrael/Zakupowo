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
            var email = model.Email;
            var password = Cryptographing.Encrypt(model.EncryptedPassword);

            var user = db.Users.Where(x => x.Email == email && x.EncryptedPassword == password).SingleOrDefault();

            if (user != null)
            {
                return Ok(model);
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
        public string Email { get; set; }
        [Required]
        public string EncryptedPassword { get; set; }

    }




}