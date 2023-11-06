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
    public class KorisniciController : ApiController
    {
        static User user = new User();

        [HttpPost, Route("api/korisnici/DetaljiKorisnik")]
        public User DetaljiKorisnik()
        {
            User u = new User();
            List<User> lista = new List<User>();
            var username = HttpContext.Current.Request.Params["ime"];
            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/Users.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<User>>(jsonData);

            

            foreach (var item in lista)
            {
                if (item.Username == username)
                {
                    u = item;
                    return u;
                }
            }
            return null;
        }
        
        [HttpPost, Route("api/korisnici/Izmena")]
        public User Izmena()
        {
            List<User> lista = new List<User>();
            List<FitnessCenter> fitnesi = new List<FitnessCenter>();
            List<Comment> komentari = new List<Comment>();
            List<GroupTraining> grupni = new List<GroupTraining>();
            User u = new User();

            var name = HttpContext.Current.Request.Params["ime"];
            var lastname = HttpContext.Current.Request.Params["prezime"];
            var username = HttpContext.Current.Request.Params["korisnickoIme"];
            var email = HttpContext.Current.Request.Params["email"];
            var password = HttpContext.Current.Request.Params["password"];
            var datetime = HttpContext.Current.Request.Params["datetime"];
            var gender = HttpContext.Current.Request.Params["gender"];
            var username1= HttpContext.Current.Request.Params["korIme"];
            DateTime.TryParse(datetime, out DateTime dateTime);

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/Users.json");
            string path1 = Path.Combine(directory, "App_Data/FitnessCenters.json");
            string path2 = Path.Combine(directory, "App_Data/Comments.json");
            string path3 = Path.Combine(directory, "App_Data/GroupTrainings.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<User>>(jsonData);

            var jsonData1 = System.IO.File.ReadAllText(path1);
            fitnesi = JsonConvert.DeserializeObject<List<FitnessCenter>>(jsonData1);

            var jsonData2 = System.IO.File.ReadAllText(path2);
            komentari = JsonConvert.DeserializeObject<List<Comment>>(jsonData2);

            var jsonData3 = System.IO.File.ReadAllText(path3);
            grupni= JsonConvert.DeserializeObject<List<GroupTraining>>(jsonData3);


            foreach (var i in lista)
            {
                if(i.Username==username1)
                {
                    u = i;
                    if(i.FirstName!=name)
                    {
                        user.FirstName = name;
                    }
                    else
                    {
                        user.FirstName = i.FirstName;
                    }
                    if(i.LastName!=lastname)
                    {
                        user.LastName = lastname;
                    }
                    else
                    {
                        user.LastName = i.LastName;
                    }
                    if (i.Username!=username)
                    {
                        foreach(var a in lista)
                        {
                            if(a.Username!=username && i.Username!=username)
                            {
                                user.Username = username; //treba da proveri da li imamo istog sa novim imenom tj da ne unesemo korisnicko ime koje vec postoji
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                    else
                    {
                        user.Username = i.Username;
                    }
                    if (i.Password!=password)
                    {
                        user.Password = password;
                    }
                    else
                    {
                        user.Password = i.Password;
                    }
                    if (i.Gender.ToString() != gender)
                    {
                        if(i.Gender.ToString()=="Muski")
                        {
                            user.Gender = GenderType.Zenski;
                        }
                        else
                        {
                            user.Gender = GenderType.Muski;
                        }
                    }
                    else
                    {
                        user.Gender = i.Gender;
                    }
                    user.Role = i.Role;
                    if(i.DateTime!=dateTime)
                    {
                        user.DateTime =dateTime;
                    }
                    else
                    {
                        user.DateTime = i.DateTime;
                    }
                    if (i.Email != email)
                    {
                        user.Email = email;
                    }
                    else
                    {
                        user.Email = i.Email;
                    }
                    
                    user.CoachGroupTrainings = i.CoachGroupTrainings;
                    user.FCEngagement = i.FCEngagement;
                    user.FCOwner = i.FCOwner;
                    user.VisitorGroupTrainings = i.VisitorGroupTrainings;
                    user.Deleted = i.Deleted;
                }
               
                

                
            }
            //lista.Remove(u);
            //lista.Add(user);

            if(username!=username1)
            {
                if(lista.Count!=0)
                {
                    foreach(var j in lista)
                    {
                        if(j.CoachGroupTrainings!=null)
                        {
                            foreach(var k in j.CoachGroupTrainings)
                            {
                                if(k.Place.Owner==username1)
                                {
                                    k.Place.Owner = username;
                                }
                                else if(k.Users!=null && user.Role==UserType.Visitor && k.Users.Contains(username1))
                                {
                                    /*foreach(var p in k.Users)
                                    {
                                        if(p==username1)
                                        {
                                            k.Users.Remove(p);
                                            k.Users.Add(username);
                                        }
                                    }*/
                                    k.Users.RemoveAll(v=>v.Contains(username1));
                                    k.Users.Add(username);
                                }
                            }
                        }
                        else if(j.VisitorGroupTrainings!=null)
                        {
                            foreach(var z in j.VisitorGroupTrainings)
                            {
                                if(z.Place.Owner==username1 && user.Role==UserType.Owner)
                                {
                                    z.Place.Owner = username;
                                }
                                else if(z.Users!=null && user.Role==UserType.Visitor && z.Users.Contains(username1))
                                {
                                    /*foreach(var f in z.Users)
                                    {
                                        if(f==username1)
                                        {
                                            z.Users.Remove(f);
                                            z.Users.Add(username);
                                        }
                                    }*/
                                    z.Users.RemoveAll(v => v.Contains(username1));
                                    z.Users.Add(username);
                                }
                            }
                        }
                    }
                }
            }

            if(username!=username1)
            {
                if (fitnesi.Count != 0)
                {
                    foreach (var fc in fitnesi)
                    {
                        if (fc.Owner == username1)
                        {
                            fc.Owner = username;
                        }
                    }
                }
            }

            if(username!=username1)
            {
                if (komentari.Count != 0)
                {
                    foreach (var c in komentari)
                    {
                        if (c.Name == username1)
                        {
                            c.Name = username;
                        }
                    }
                }
            }

            if(username!=username1)
            {
                if(grupni.Count!=0)
                {
                    foreach(var g in grupni)
                    {
                        if(g.Place.Owner==username1)
                        {
                            g.Place.Owner = username;
                        }
                        else if(g.Users!=null && user.Role==UserType.Visitor && g.Users.Contains(username1))
                        {
                            /*foreach(var y in g.Users)
                            {
                                if(y==username1)
                                {
                                    .Users.RemoveAll(v => v.Contains(username1));
                                    k.Users.Add(username);
                                }
                            }*/
                            g.Users.RemoveAll(v => v.Contains(username1));
                            g.Users.Add(username);
                        }
                    }
                }
            }

            lista.Remove(u);
            lista.Add(user);

            using (StreamWriter streamWriter = new StreamWriter(path3))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(streamWriter, grupni);
            }

            using (StreamWriter streamWriter = new StreamWriter(path2))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(streamWriter, komentari);
            }

            using (StreamWriter streamWriter = new StreamWriter(path1))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(streamWriter, fitnesi);
            }

            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(streamWriter, lista);
            }

            return user;
        }
    }
}
