using MyApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace MyApp.Controllers
{
    public class RegisterController : ApiController
    {
        [HttpPost, Route("api/register/Upis")]
        public bool Upis()
        {
            List<User> lista = new List<User>();
            User u = new User();
            var name = HttpContext.Current.Request.Params["name"];
            var lastname = HttpContext.Current.Request.Params["lastname"];
            var username = HttpContext.Current.Request.Params["username"];
            var email = HttpContext.Current.Request.Params["email"];
            var password = HttpContext.Current.Request.Params["password"];
            var datetime = HttpContext.Current.Request.Params["datetime"];
            var gender = HttpContext.Current.Request.Params["gender"];
            //var vreme = datetime + "T00:00:00";
            DateTime.TryParse(datetime, out DateTime dateTime);
            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/Users.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<User>>(jsonData);


            u.FirstName = name;
            u.LastName = lastname;
            u.Username = username;
            u.Email = email;
            u.Password = password;
            if(gender=="Muski")
            {
                u.Gender = GenderType.Muski;
            }
            else
            {
                u.Gender = GenderType.Zenski;
            }
            u.DateTime = dateTime;
            u.Role = UserType.Visitor;
            u.Deleted = false;
            u.FCEngagement = -1;

            foreach(var item in lista)
            {
                
                if(item.Username==username)
                {
                    return false;
                }
                else if(item.Email==email)
                {
                    return false;
                }
            }

            lista.Add(u);
            //string filename = "AppData/Users.json";
            
            //File.WriteAllText(path, JsonConvert.SerializeObject(lista));
            using (StreamWriter streamWriter = new StreamWriter(path/*, true*/))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(streamWriter, lista);
            }

            return true;
        }
        [HttpPost, Route("api/register/Logovanje")]
        public User Logovanje()
        {
            User u = new User();
            List<User> lista = new List<User>();
            var username = HttpContext.Current.Request.Params["name"];
            var password = HttpContext.Current.Request.Params["password"];
            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/Users.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<User>>(jsonData);

            foreach(var item in lista)
            {
                if(item.Password==password && item.Username==username && item.Deleted==false)
                {
                    u = item;
                    return u;
                }
            }
            return null;
        }
        
    }
}
