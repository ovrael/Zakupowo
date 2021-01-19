using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Security;
using Newtonsoft.Json;
using ShopApp.DAL;
using ShopApp.Models;
using ShopApp.Utility;

namespace ShopApp.Controllers
{
    [RoutePrefix("api/Users")]


    public class UsersController : ApiController
    {
        private ShopContext db = new ShopContext();

        public IHttpActionResult GetUser(int userID)
        {
            User user = null;
            try
            {
                user = db.Users.Where(i => i.UserID == userID).First();
            }
            catch (Exception e)
            {

            }


            if (user == null)
            {
                return BadRequest("Couldn't find user!");
            }

          
            return Ok(user);
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

        [Route("ChangeData")]
        public IHttpActionResult ChangePersonalData(PersonalDataBindingModel model)
        {
            string name = model.FirstName;
            string surname = model.LastName;
            string email = model.Email;
            string phone = model.Phone;

            User editUser = db.Users.Where(u => u.Login == model.Login).FirstOrDefault();

            if (editUser != null)
            {
                string changedFirstName = name;
                string changedLastName = surname;
                string changedEmail = email;
                string changedPhoneNumber = phone;


                if (changedFirstName != null)
                    editUser.FirstName = changedFirstName;

                if (changedLastName != null)
                    editUser.LastName = changedLastName;

                if (changedEmail != null)
                    editUser.Email = changedEmail;

                if (changedPhoneNumber != null)
                    editUser.Phone = changedPhoneNumber;

                db.Entry(editUser).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            if (editUser != null)
            {
                return Ok(editUser);
            }
            else
            {
                return Ok();
            }

        }


        [HttpPost]
        [Route("DeleteAddress")]
        public IHttpActionResult DeleteAddress(ShippingAdressBindingModel model)
        {

            ShippingAdress editAddress = db.ShippingAdresses.Where(u => u.AdressID == model.AdressID).FirstOrDefault();
            int userId = editAddress.User.UserID;
            User editUser = db.Users.Where(u => u.UserID == userId).FirstOrDefault();

            if (editUser != null && editAddress != null)
            {

                editUser.ShippingAdresses.Remove(editAddress);
                db.ShippingAdresses.Remove(editAddress);
                db.SaveChanges();
            }
            if (editUser != null)
            {
                return Ok(editUser);
            }
            else
            {
                return Ok();
            }

        }
        [HttpPost]
        [Route("ChangeAddressData")]
        public IHttpActionResult ChangeAddressData(ShippingAdressBindingModel model)
        {
            string country = model.Country;
            string city = model.City;
            string street = model.Street;
            string postal = model.PostalCode;
            string premises = model.PremisesNumber;

            int userId = db.ShippingAdresses.Where(u => u.AdressID == model.AdressID).FirstOrDefault().User.UserID;
            ShippingAdress editAddress = db.ShippingAdresses.Where(u => u.AdressID == model.AdressID).FirstOrDefault();
            User editUser = db.Users.Where(u => u.UserID == userId).FirstOrDefault();

            if (editAddress != null)
            {

                if (country != null)
                    editAddress.Country = country;
                if (city != null)
                    editAddress.City = city;
                if (street != null)
                    editAddress.Street = street;
                if (postal != null)
                    editAddress.PostalCode = postal;
                if (premises != null)
                    editAddress.PremisesNumber = premises;

                db.Entry(editAddress).State = EntityState.Modified;
                db.SaveChanges();
            }
            if (editUser != null)
            {
                return Ok(editUser);
            }
            else
            {
                return Ok();
            }

        }
        [HttpPost]
        [Route("AddAddress")]
        public IHttpActionResult AddAddress(ShippingAdressBindingModel model)
        {
            string country = model.Country;
            string city = model.City;
            string street = model.Street;
            string postal = model.PostalCode;
            string premises = model.PremisesNumber;

            User editUser = db.Users.Where(u => u.UserID == model.AdressID).FirstOrDefault();
            ShippingAdress shippingAdress = new ShippingAdress
            {
                Country = country,
                City = city,
                Street = street,
                PostalCode = postal,
                PremisesNumber = premises,
                User = editUser
            };

            db.ShippingAdresses.Add(shippingAdress);
            db.SaveChanges();
            editUser.ShippingAdresses.Add(shippingAdress);
            db.SaveChanges();

            if (editUser != null)
            {
                return Ok(editUser);
            }
            else
            {
                return Ok();
            }

        }


        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePasswordAsync()
        {
            string json = await Request.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            string login = data["login"];
            string newPassword = data["newPassword"];
            string oldPassword = data["oldPassword"];

            User editUser = db.Users.Where(i => i.Login == login).FirstOrDefault();


            if (editUser != null)
            {
                string encryptedOldPassword = Cryptographing.Encrypt(oldPassword.Trim());
                string encryptedNewPassword = Cryptographing.Encrypt(newPassword.Trim());


                // If written current password is the same as current password AND written current and new passwords are not NULLs
                if (encryptedOldPassword.Equals(editUser.EncryptedPassword) && encryptedOldPassword != null && encryptedNewPassword != null)
                {
                    // If new password validates and is different from old one => CHANGE PASSWORD
                    if (!encryptedNewPassword.Equals(encryptedOldPassword))
                    {
                        await Task.Run(() => EmailManager.SendEmailAsync(EmailManager.EmailType.ChangePassword, editUser.FirstName, editUser.LastName, editUser.Email, encryptedNewPassword));
                        return Ok("Wysłano email z potwierdzeniem zmiany hasła!");
                    }
                }
            }

            return BadRequest("Nie udało się zmienić hasła");
        }
        [HttpPost]
        [Route("ChangeAvatar")]
        public async Task<IHttpActionResult> ChangeAvatar()
        {
            var uploadPath = HostingEnvironment.MapPath("/") + @"/Uploads";
            Directory.CreateDirectory(uploadPath);
            var provider = new MultipartFormDataStreamProvider(uploadPath);
            await Request.Content.ReadAsMultipartAsync(provider);


            string serializedModel = "";
            foreach (var key in provider.FormData.AllKeys)
            {
                foreach (var val in provider.FormData.GetValues(key))
                {
                    if (val != "")
                    {
                        serializedModel = val;
                    }
                }
            }

            Dictionary<string,string> model = JsonConvert.DeserializeObject<Dictionary<string,string>>(serializedModel);
            string value = model.FirstOrDefault().Value;

            try
            {
                User editUser = db.Users.Where(i => i.Login == value.ToString()).First();
                if (editUser != null)
                {
                    var filesData = HttpContext.Current.Request.Files.Count > 0 ?
                    HttpContext.Current.Request.Files : null;

                    HttpPostedFile file = null;
                    if (filesData != null) file = filesData[0];
                    HttpPostedFileBase filebase = new HttpPostedFileWrapper(file);
                    var imageUrl = await FileManager.UploadAvatar(filebase, editUser.UserID);

                    if (imageUrl != null)
                    {
                        if (editUser.AvatarImage == null)
                        {
                            AvatarImage newAvatar = new AvatarImage() { PathToFile = imageUrl, User = editUser };
                            db.Entry(newAvatar).State = System.Data.Entity.EntityState.Added;
                        }
                        else
                        {
                            editUser.AvatarImage.PathToFile = imageUrl;
                            db.Entry(editUser).State = System.Data.Entity.EntityState.Modified;
                        }

                        System.IO.DirectoryInfo di = new DirectoryInfo(uploadPath);

                        foreach (FileInfo data in di.GetFiles())
                        {
                            data.Delete();
                        }
                        db.SaveChanges();

                        return Ok(editUser);

                    }

                    return Ok(editUser);


                }

            }
            catch(Exception e )
            {
                Debug.WriteLine(e.Message);
            }


           

            return BadRequest();
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("AddOfferToBucket")]
        public async Task<IHttpActionResult> AddOfferToBucketAsync()
        {
            string json = await Request.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            string login = data["login"];
            int offerId = Convert.ToInt32(data["offerId"]);
            string quantity = data["quantity"];

            User user = db.Users.Where(i => i.Login == login).FirstOrDefault();
            BucketItem NewBucketItem = new BucketItem();


            var Offer = db.Offers.Where(i => i.OfferID == offerId && i.IsActive).FirstOrDefault();
            if (Offer != null)//We chceck if user called for existing and active offer
            {
                bool? AlreadyInBucket = user.Bucket?.BucketItems?.Where(i => i.Offer != null && i.Offer.OfferID == offerId).Any();
                if (AlreadyInBucket == false)
                {
                    bool? IsOwner = user.Offers?.Where(i => i.OfferID == offerId).Any();
                    if (IsOwner == false)
                    {
                        if (int.TryParse(quantity, out int QuantityAsInt))
                        {
                            if (Offer.InStockNow < QuantityAsInt || QuantityAsInt < 1)
                            {
                                return BadRequest("Przekroczono dostępną ilość danego produktu");
                            }
                            else
                            {
                                NewBucketItem.Offer = Offer;
                                NewBucketItem.Quantity = QuantityAsInt;
                                NewBucketItem.TotalPrice = QuantityAsInt * Offer.Price;
                                db.BucketItems.Add(NewBucketItem);

                                user.Bucket.BucketItems.Add(NewBucketItem);
                                NewBucketItem.Bucket = user.Bucket;
                                Offer.BucketItems.Add(NewBucketItem);
                                ConcurencyHandling.SaveChangesWithConcurencyHandling(db);
                                return Ok("true");
                            }
                        }
                    }
                    else
                        return BadRequest("Już posiadasz tę ofertę");
                }
                return BadRequest("Oferta znajduje sie juz w koszyku");

            }
            return BadRequest("Nie udalo sie dodac oferty");
        }

    }

}




  
public class ShippingAdressBindingModel
    {
       
        public int AdressID { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PremisesNumber { get; set; }
        public string PostalCode { get; set; }
    }

    public class PersonalDataBindingModel
    {
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
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


