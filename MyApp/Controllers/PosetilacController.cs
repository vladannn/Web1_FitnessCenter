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
    public class PosetilacController : ApiController
    {
        [HttpGet, Route("api/posetilac/Korisnik")]
        public User Korisnik()
        {
            var korIme = HttpContext.Current.Request.Params["ime"];
            List<User> lista = new List<User>();

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/Users.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<User>>(jsonData);

            foreach(var u in lista)
            {
                if(u.Username==korIme)
                {
                    return u;
                }
            }

            return null;
        }

        [HttpPost, Route("api/posetilac/Prijava")]
        public bool Prijava()
        {
            var id = HttpContext.Current.Request.Params["id"];
            var korisnickoIme = HttpContext.Current.Request.Params["korIme"];
            Int32.TryParse(id, out int idd);
            List<User> lista = new List<User>();
            List<GroupTraining> grupni = new List<GroupTraining>();

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/Users.json");
            string path1 = Path.Combine(directory, "App_Data/GroupTrainings.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<User>>(jsonData);

            var jsonData1 = System.IO.File.ReadAllText(path1);
            grupni = JsonConvert.DeserializeObject<List<GroupTraining>>(jsonData1);

            var niz =new List<string>();
            string korisnik = "";
            var broj = -1;
            var provera = new List<GroupTraining>();
            bool oki = false;

            foreach(var g in grupni)
            {
                if(g.Id==idd)
                {
                    //niz = g.Users;
                    if(g.Users==null)
                    {
                        broj = 0;
                    }
                    else
                    {
                        broj = g.Users.Count;
                    }
                    if (g.Users != null)
                    {
                        foreach (var m in g.Users)
                        {
                            if (m == korisnickoIme)
                            {
                                korisnik = m;
                            }
                        }
                    }
                    
                    if(korisnik=="" && broj< g.MaxNumber)
                    {
                        //niz.Add(korisnickoIme);
                        //g.Users = niz;
                        if(g.Users==null)
                        {
                            niz.Add(korisnickoIme);
                            g.Users = niz;
                        }
                        else
                        {
                            g.Users.Add(korisnickoIme);
                        }
                        foreach(var u in lista)
                        {
                            if(u.Username==korisnickoIme)
                            {
                                if (u.VisitorGroupTrainings == null)
                                {
                                    provera.Add(g);
                                    u.VisitorGroupTrainings = provera;
                                    
                                }
                                else
                                {
                                    u.VisitorGroupTrainings.Add(g);
                                }
                                oki = true;
                                
                                
                            }
                            else if(u.CoachGroupTrainings!=null && u.Role==UserType.Coach)
                            {
                                foreach(var k in u.CoachGroupTrainings)
                                {
                                    if(k.Id == idd && k.Deleted == false && k.Users==null)
                                    {
                                        k.Users = new List<string>();
                                        k.Users.Add(korisnickoIme);
                                        oki = true;
                                    }
                                    else if(k.Id==idd && k.Deleted==false && !k.Users.Contains(korisnickoIme))
                                    {
                                        k.Users.Add(korisnickoIme);
                                        oki = true;
                                    }
                                }
                            }
                            else if (u.VisitorGroupTrainings != null && u.Role==UserType.Visitor)
                            {
                                foreach (var p in u.VisitorGroupTrainings)
                                {
                                    if(p.Id == idd && p.Deleted == false && p.Users==null)
                                    {
                                        p.Users = new List<string>();
                                        p.Users.Add(korisnickoIme);
                                        oki = true;
                                    }
                                    else if (p.Id == idd && p.Deleted == false && !p.Users.Contains(korisnickoIme))
                                    {
                                        p.Users.Add(korisnickoIme);
                                        oki = true;
                                    }
                                }
                            }
                        }
                        if(oki==true)
                        {
                            using (StreamWriter streamWriter = new StreamWriter(path))
                            {
                                JsonSerializer serializer = new JsonSerializer();
                                serializer.Serialize(streamWriter, lista);
                            }
                            using (StreamWriter streamWriter = new StreamWriter(path1))
                            {
                                JsonSerializer serializer = new JsonSerializer();
                                serializer.Serialize(streamWriter, grupni);
                            }
                            return true;
                        }
                    }
                    
                }
            }

            return false;
        }

        [HttpGet, Route("api/posetilac/RanijiTreninzi")]
        public List<GroupTraining> RanijiTreninzi()
        {
            List<User> lista2 = new List<User>();
            var korisnickoIme = HttpContext.Current.Request.Params["koIme"];

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/Users.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista2 = JsonConvert.DeserializeObject<List<User>>(jsonData);

            var meh = false;
            var lista = new List<GroupTraining>();

            foreach (var i in lista2)
            {
                if(i.Username==korisnickoIme)
                {
                    if(i.VisitorGroupTrainings!=null)
                    {
                        foreach(var u in i.VisitorGroupTrainings)
                        {
                            if(u.DateTime<DateTime.Now && u.Deleted==false)
                            {
                                lista.Add(u);
                                meh = true;
                            }
                        }
                        
                    }
                }
            }
            if(meh)
            {
                return lista;
            }
            return null;
        }
        [HttpGet, Route("api/posetilac/Pretraga")]
        public List<GroupTraining> Pretraga()
        {
            string ime = HttpContext.Current.Request.Params["ime"];
            var fitnes = HttpContext.Current.Request.Params["fitnes"];
            var tip = HttpContext.Current.Request.Params["tip"];
            var korisnickoIme = HttpContext.Current.Request.Params["koIme"];
            var obj = tip.Replace(" ", "");
            List<User> lista = new List<User>();
            List<GroupTraining> grupni = new List<GroupTraining>();

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/Users.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<User>>(jsonData);
            
            foreach(var u in lista)
            {
                if(u.Username==korisnickoIme && u.VisitorGroupTrainings!=null)
                {
                    
                    foreach(var g in u.VisitorGroupTrainings)
                    {
                        if (g.Deleted == false)
                        {
                            if (g.Name.ToLower() == ime.ToLower() && g.Place.Name.ToLower() == fitnes.ToLower() && g.TrainingType.ToString().ToLower() == obj.ToLower() && g.DateTime < DateTime.Now)
                            {
                                if (!grupni.Contains(g))
                                {
                                    grupni.Add(g);
                                }
                            }
                            else if (g.Name.ToLower() == ime.ToLower() && g.Place.Name.ToLower() == fitnes.ToLower() && (obj == null || obj == "(empty)") && g.DateTime < DateTime.Now)
                            {
                                if (!grupni.Contains(g))
                                {
                                    grupni.Add(g);
                                }
                            }
                            else if (g.Name.ToLower() == ime.ToLower() && (fitnes == null || fitnes == "") && g.TrainingType.ToString().ToLower() == obj.ToLower() && g.DateTime < DateTime.Now)
                            {
                                if (!grupni.Contains(g))
                                {
                                    grupni.Add(g);
                                }
                            }
                            else if ((ime == null || ime == "") && g.Place.Name.ToLower() == fitnes.ToLower() && g.TrainingType.ToString().ToLower() == obj.ToLower() && g.DateTime < DateTime.Now)
                            {
                                if (!grupni.Contains(g))
                                {
                                    grupni.Add(g);
                                }
                            }
                            else if ((ime == null || ime == "") && (fitnes == null || fitnes == "") && g.TrainingType.ToString().ToLower() == obj.ToLower() && g.DateTime < DateTime.Now)
                            {
                                if (!grupni.Contains(g))
                                {
                                    grupni.Add(g);
                                }
                            }
                            else if (g.Name.ToLower() == ime.ToLower() && (fitnes == null || fitnes == "") && (obj == null || obj == "(empty)") && g.DateTime < DateTime.Now)
                            {
                                if (!grupni.Contains(g))
                                {
                                    grupni.Add(g);
                                }
                            }
                            else if ((ime == null || ime == "") && g.Place.Name.ToLower() == fitnes.ToLower() && (obj == null || obj == "(empty)") && g.DateTime < DateTime.Now)
                            {
                                if (!grupni.Contains(g))
                                {
                                    grupni.Add(g);
                                }
                            }
                        }
                    }
                }
            }
            if (grupni.Count == 0)
            {
                return null;
            }

            return grupni;
        }

        [HttpGet, Route("api/posetilac/NazivRastuce")]
        public List<GroupTraining> NazivRastuce()
        {
            List<GroupTraining> lista1 = new List<GroupTraining>();
            List<User> posetioci = new List<User>();
            var korIme = HttpContext.Current.Request.Params["koIme"];

            string directory = HttpRuntime.AppDomainAppPath;
            string path1 = Path.Combine(directory, "App_Data/Users.json");
            

            var jsonData1 = System.IO.File.ReadAllText(path1);
            posetioci = JsonConvert.DeserializeObject<List<User>>(jsonData1);

            foreach (var u in posetioci)
            {
                if (korIme == u.Username && u.Role == UserType.Visitor && u.VisitorGroupTrainings!=null)
                {
                    lista1 = u.VisitorGroupTrainings.FindAll(x => x.Deleted != true && x.DateTime < DateTime.Now);
                    if (lista1.Count == 0)
                    {
                        return null;
                    }
                    else
                    {
                        return lista1.OrderBy(o => o.Name).ToList();
                    }
                }
            }
            return null;
        }

        [HttpGet, Route("api/posetilac/NazivOpadajuce")]
        public List<GroupTraining> NazivOpadajuce()
        {
            List<GroupTraining> lista1 = new List<GroupTraining>();
            List<User> posetioci = new List<User>();
            var korIme = HttpContext.Current.Request.Params["koIme"];

            string directory = HttpRuntime.AppDomainAppPath;
            string path1 = Path.Combine(directory, "App_Data/Users.json");


            var jsonData1 = System.IO.File.ReadAllText(path1);
            posetioci = JsonConvert.DeserializeObject<List<User>>(jsonData1);

            foreach (var u in posetioci)
            {
                if (korIme == u.Username && u.Role == UserType.Visitor && u.VisitorGroupTrainings!=null)
                {
                    lista1 = u.VisitorGroupTrainings.FindAll(x => x.Deleted != true && x.DateTime < DateTime.Now);
                    if (lista1.Count == 0)
                    {
                        return null;
                    }
                    else
                    {
                        return lista1.OrderByDescending(o => o.Name).ToList();
                    }
                }
            }
            return null;
        }

        [HttpGet, Route("api/posetilac/TipRastuce")]
        public List<GroupTraining> TipRastuce()
        {
            List<GroupTraining> lista1 = new List<GroupTraining>();
            List<User> posetioci = new List<User>();
            var korIme = HttpContext.Current.Request.Params["koIme"];

            string directory = HttpRuntime.AppDomainAppPath;
            string path1 = Path.Combine(directory, "App_Data/Users.json");


            var jsonData1 = System.IO.File.ReadAllText(path1);
            posetioci = JsonConvert.DeserializeObject<List<User>>(jsonData1);

            foreach (var u in posetioci)
            {
                if (korIme == u.Username && u.Role == UserType.Visitor && u.VisitorGroupTrainings!=null)
                {
                    lista1 = u.VisitorGroupTrainings.FindAll(x => x.Deleted != true && x.DateTime < DateTime.Now);
                    if (lista1.Count == 0)
                    {
                        return null;
                    }
                    else
                    {
                        return lista1.OrderBy(o => o.TrainingType.ToString()).ToList();
                    }
                }
            }
            return null;
        }

        [HttpGet, Route("api/posetilac/TipOpadajuce")]
        public List<GroupTraining> TipOpadajuce()
        {
            List<GroupTraining> lista1 = new List<GroupTraining>();
            List<User> posetioci = new List<User>();
            var korIme = HttpContext.Current.Request.Params["koIme"];

            string directory = HttpRuntime.AppDomainAppPath;
            string path1 = Path.Combine(directory, "App_Data/Users.json");


            var jsonData1 = System.IO.File.ReadAllText(path1);
            posetioci = JsonConvert.DeserializeObject<List<User>>(jsonData1);

            foreach (var u in posetioci)
            {
                if (korIme == u.Username && u.Role == UserType.Visitor && u.VisitorGroupTrainings!=null)
                {
                    lista1 = u.VisitorGroupTrainings.FindAll(x => x.Deleted != true && x.DateTime < DateTime.Now);
                    if (lista1.Count == 0)
                    {
                        return null;
                    }
                    else
                    {
                        return lista1.OrderByDescending(o => o.TrainingType.ToString()).ToList();
                    }
                }
            }
            return null;
        }

        [HttpGet, Route("api/posetilac/DatumRastuce")]
        public List<GroupTraining> DatumRastuce()
        {
            List<GroupTraining> lista1 = new List<GroupTraining>();
            List<User> posetioci = new List<User>();
            var korIme = HttpContext.Current.Request.Params["koIme"];

            string directory = HttpRuntime.AppDomainAppPath;
            string path1 = Path.Combine(directory, "App_Data/Users.json");


            var jsonData1 = System.IO.File.ReadAllText(path1);
            posetioci = JsonConvert.DeserializeObject<List<User>>(jsonData1);

            foreach (var u in posetioci)
            {
                if (korIme == u.Username && u.Role == UserType.Visitor && u.VisitorGroupTrainings!=null)
                {
                    lista1 = u.VisitorGroupTrainings.FindAll(x => x.Deleted != true && x.DateTime < DateTime.Now);
                    if (lista1.Count == 0)
                    {
                        return null;
                    }
                    else
                    {
                        return lista1.OrderBy(o => o.DateTime).ToList();
                    }
                }
            }
            return null;
        }

        [HttpGet, Route("api/posetilac/DatumOpadajuce")]
        public List<GroupTraining> DatumOpadajuce()
        {
            List<GroupTraining> lista1 = new List<GroupTraining>();
            List<User> posetioci = new List<User>();
            var korIme = HttpContext.Current.Request.Params["koIme"];

            string directory = HttpRuntime.AppDomainAppPath;
            string path1 = Path.Combine(directory, "App_Data/Users.json");


            var jsonData1 = System.IO.File.ReadAllText(path1);
            posetioci = JsonConvert.DeserializeObject<List<User>>(jsonData1);

            foreach (var u in posetioci)
            {
                if (korIme == u.Username && u.Role == UserType.Visitor && u.VisitorGroupTrainings!=null)
                {
                    lista1 = u.VisitorGroupTrainings.FindAll(x => x.Deleted != true && x.DateTime < DateTime.Now);
                    if (lista1.Count == 0)
                    {
                        return null;
                    }
                    else
                    {
                        return lista1.OrderByDescending(o => o.DateTime).ToList();
                    }
                }
            }
            return null;
        }
    }
}
