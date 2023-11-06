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
    public class TrenerController : ApiController
    {
        [HttpPost, Route("api/trener/Dodaj")]
        public bool Dodaj()
        {
            List<GroupTraining> lista = new List<GroupTraining>();
            List<User> lista1 = new List<User>();
            List<FitnessCenter> lista2 = new List<FitnessCenter>();
            GroupTraining g = new GroupTraining();
            //FitnessCenter fc = new FitnessCenter();

            var ime = HttpContext.Current.Request.Params["ime"];
            var tip = HttpContext.Current.Request.Params["tip"];
            var trajanje = HttpContext.Current.Request.Params["trajanje"];
            var korIme = HttpContext.Current.Request.Params["koIme"];
            var max = HttpContext.Current.Request.Params["max"];
            var vreme = HttpContext.Current.Request.Params["vreme"];
            var obj = tip.Replace(" ","");
            var zec = false;

            DateTime.TryParse(vreme, out DateTime dateTime);

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/GroupTrainings.json");
            string path1 = Path.Combine(directory, "App_Data/Users.json");
            string path2 = Path.Combine(directory, "App_Data/FitnessCenters.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<GroupTraining>>(jsonData);

            var jsonData1 = System.IO.File.ReadAllText(path1);
            lista1 = JsonConvert.DeserializeObject<List<User>>(jsonData1);

            var jsonData2 = System.IO.File.ReadAllText(path2);
            lista2 = JsonConvert.DeserializeObject<List<FitnessCenter>>(jsonData2);

            if (dateTime>=DateTime.Now.Date.AddDays(3))
            {
                g.Name = ime;
                if (obj == "Yoga")
                {
                    g.TrainingType = TrainingType.Yoga;
                }
                else if (obj == "LessMillsTone")
                {
                    g.TrainingType = TrainingType.LessMillsTone;
                }
                else if (obj == "BodyPump")
                {
                    g.TrainingType = TrainingType.BodyPump;
                }
                g.Duration = Int32.Parse(trajanje);
                g.MaxNumber = Int32.Parse(max);
                g.DateTime = dateTime;
                g.Deleted = false;
                g.Users = null;
                
                foreach(var um in lista1)
                {
                    if (um.Role == UserType.Coach && korIme==um.Username)
                    {
                        foreach (var f in lista2)
                        {
                            if (f.Id == um.FCEngagement && f.Deleted==false)
                            {
                                g.Place = f;
                                zec = true;
                            }
                        }
                    }
                }

                

                var objekat = new GroupTraining(g.Name, g.TrainingType, g.Place, g.Duration, g.DateTime, g.MaxNumber, g.Users, g.Deleted);
                foreach (var u in lista1)
                {
                    if(korIme==u.Username)
                    {
                        if (u.CoachGroupTrainings == null)
                        {
                            var primer = new List<GroupTraining>();
                            u.CoachGroupTrainings = primer;
                            u.CoachGroupTrainings.Add(objekat);
                        }else
                        {
                            u.CoachGroupTrainings.Add(objekat);
                        }
                    }
                }

                if (lista == null)
                {
                    lista = new List<GroupTraining>();
                    lista.Add(objekat);
                }
                else
                {
                    lista.Add(objekat);
                }

                using (StreamWriter streamWriter = new StreamWriter(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(streamWriter, lista);
                }
                using (StreamWriter streamWriter = new StreamWriter(path1))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(streamWriter, lista1);
                }
                
            }
            else
            {
                return false;
            }

            return true;
        }

        [HttpGet, Route("api/trener/SpisakGT")]
        public List<GroupTraining> SpisakGT()
        {
            List<GroupTraining> lista = new List<GroupTraining>();
            List<User> treneri = new List<User>();
            var korIme = HttpContext.Current.Request.Params["ime"];
            List<GroupTraining> lista1 = new List<GroupTraining>();

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/GroupTrainings.json");
            string path1 = Path.Combine(directory, "App_Data/Users.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<GroupTraining>>(jsonData);

            var jsonData1 = System.IO.File.ReadAllText(path1);
            treneri = JsonConvert.DeserializeObject<List<User>>(jsonData1);

            foreach (var u in treneri)
            {
                if(korIme==u.Username && u.Role==UserType.Coach)
                {
                    if (u.CoachGroupTrainings != null)
                    {
                        lista1 = u.CoachGroupTrainings.FindAll(x => x.Deleted != true && x.DateTime >= DateTime.Now);
                        //return lista1;
                    }
                    /*if (u.CoachGroupTrainings == null)
                    {
                        return null;
                    }
                    else
                    {
                        return lista1;
                    }*/
                }
            }

            if(lista1.Count!=0)
            {
                return lista1;
            }

            return null;
        }

        [HttpGet, Route("api/trener/ZasebanTrening")]
        public GroupTraining ZasebanTrening()
        {
            GroupTraining g = new GroupTraining();
            List<GroupTraining> lista = new List<GroupTraining>();
            var id = HttpContext.Current.Request.Params["id"];
            Int32.TryParse(id, out int idd);

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/GroupTrainings.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<GroupTraining>>(jsonData);

            foreach(var gt in lista)
            {
                if(gt.Id==idd && gt.Deleted==false)
                {
                    g = gt;
                    return g;
                }
            }

            return null;
        }

        [HttpPost, Route("api/trener/IzmeniTrening")]
        public bool IzmeniTrening()
        {
            List<GroupTraining> lista = new List<GroupTraining>();
            List<User> lista1 = new List<User>();

            var ime = HttpContext.Current.Request.Params["ime"];
            var tip = HttpContext.Current.Request.Params["tip"];
            var trajanje = HttpContext.Current.Request.Params["trajanje"];
            var korIme = HttpContext.Current.Request.Params["koIme"];
            var max = HttpContext.Current.Request.Params["max"];
            var vreme = HttpContext.Current.Request.Params["vreme"];
            var id = HttpContext.Current.Request.Params["id"];
            var obj = tip.Replace(" ","");
            var provera = false;
            DateTime.TryParse(vreme, out DateTime format);
            Int32.TryParse(id, out int idd);

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/GroupTrainings.json");
            string path1 = Path.Combine(directory, "App_Data/Users.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<GroupTraining>>(jsonData);

            var jsonData1 = System.IO.File.ReadAllText(path1);
            lista1 = JsonConvert.DeserializeObject<List<User>>(jsonData1);

            foreach(var g in lista)
            {
                if(g.Id==idd && g.Deleted==false)
                {
                    provera = true;
                    g.MaxNumber = Int32.Parse(max);
                    if(DateTime.Now.Date.AddDays(3) > format)
                    {
                        return false;
                    }
                    else
                    {
                        g.DateTime = format;
                    }
                    g.Duration = Int32.Parse(trajanje);
                    g.Name = ime;
                    if(obj==TrainingType.Yoga.ToString())
                    {
                        g.TrainingType = TrainingType.Yoga;
                    }
                    else if (obj == TrainingType.LessMillsTone.ToString())
                    {
                        g.TrainingType = TrainingType.LessMillsTone;
                    }
                    else if (obj == TrainingType.BodyPump.ToString())
                    {
                        g.TrainingType = TrainingType.BodyPump;
                    }

                    using (StreamWriter streamWriter = new StreamWriter(path))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(streamWriter, lista);
                    }
                }
            }
            if (provera)
            {
                foreach (var t in lista1)
                {
                    if (t.Username == korIme && t.Role == UserType.Coach && t.Deleted == false && t.CoachGroupTrainings != null)
                    {
                        foreach (var gtr in t.CoachGroupTrainings)
                        {
                            if (gtr.Id == idd)
                            {
                                gtr.MaxNumber = Int32.Parse(max);
                                if (DateTime.Now.Date.AddDays(3) > format)
                                {
                                    return false;
                                }
                                else
                                {
                                    gtr.DateTime = format;
                                }
                                gtr.Duration = Int32.Parse(trajanje);
                                gtr.Name = ime;
                                if (obj == TrainingType.Yoga.ToString())
                                {
                                    gtr.TrainingType = TrainingType.Yoga;
                                }
                                else if (obj == TrainingType.LessMillsTone.ToString())
                                {
                                    gtr.TrainingType = TrainingType.LessMillsTone;
                                }
                                else if (obj == TrainingType.BodyPump.ToString())
                                {
                                    gtr.TrainingType = TrainingType.BodyPump;
                                }

                                using (StreamWriter streamWriter = new StreamWriter(path1))
                                {
                                    JsonSerializer serializer = new JsonSerializer();
                                    serializer.Serialize(streamWriter, lista1);
                                }
                            }
                        }
                    }
                }

                foreach (var l in lista1)
                {
                    if (l.VisitorGroupTrainings != null)
                    {
                        foreach (var c in l.VisitorGroupTrainings)
                        {
                            if (c.Id == idd)
                            {
                                c.MaxNumber = Int32.Parse(max);
                                if (DateTime.Now > format)
                                {
                                    return false;
                                }
                                else
                                {
                                    c.DateTime = format;
                                }
                                c.Duration = Int32.Parse(trajanje);
                                c.Name = ime;
                                if (obj == TrainingType.Yoga.ToString())
                                {
                                    c.TrainingType = TrainingType.Yoga;
                                }
                                else if (obj == TrainingType.LessMillsTone.ToString())
                                {
                                    c.TrainingType = TrainingType.LessMillsTone;
                                }
                                else if (obj == TrainingType.BodyPump.ToString())
                                {
                                    c.TrainingType = TrainingType.BodyPump;
                                }

                                using (StreamWriter streamWriter = new StreamWriter(path1))
                                {
                                    JsonSerializer serializer = new JsonSerializer();
                                    serializer.Serialize(streamWriter, lista1);
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
        [HttpDelete, Route("api/trener/Brisanje")]
        public bool Brisanje()
        {
            List<GroupTraining> lista = new List<GroupTraining>();
            List<User> lista1 = new List<User>();

            var korIme = HttpContext.Current.Request.Params["ime"];
            var id = HttpContext.Current.Request.Params["id"];
            Int32.TryParse(id, out int idd);

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/GroupTrainings.json");
            string path1 = Path.Combine(directory, "App_Data/Users.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<GroupTraining>>(jsonData);

            var jsonData1 = System.IO.File.ReadAllText(path1);
            lista1 = JsonConvert.DeserializeObject<List<User>>(jsonData1);

            //var obj = new List<GroupTraining>();

            foreach(var k in lista1)
            {
                if(k.Role==UserType.Coach && k.Username==korIme && k.CoachGroupTrainings!=null)
                {
                    //k.CoachGroupTrainings = lista1.FindAll(c=>c.CoachGroupTrainings.FindAll(m=>m.Id==idd));
                    //obj = k.CoachGroupTrainings;
                    foreach(var unutra in k.CoachGroupTrainings)
                    {
                        if(unutra.Id==idd && unutra.Deleted==false && unutra.Users==null)
                        {
                            unutra.Deleted = true;
                            
                            foreach (var g in lista)
                            {
                                if(g.Id==idd && g.Deleted==false)
                                {
                                    g.Deleted = true;
                                    using (StreamWriter streamWriter = new StreamWriter(path))
                                    {
                                        JsonSerializer serializer = new JsonSerializer();
                                        serializer.Serialize(streamWriter, lista);
                                    }
                                    using (StreamWriter streamWriter = new StreamWriter(path1))
                                    {
                                        JsonSerializer serializer = new JsonSerializer();
                                        serializer.Serialize(streamWriter, lista1);
                                    }
                                    return true;
                                }
                            }
                            
                        }
                    }
                }
            }

            return false;
        }

        [HttpGet, Route("api/trener/Spisak")]
        public List<GroupTraining> Spisak()
        {
            List<GroupTraining> lista = new List<GroupTraining>();
            List<User> treneri = new List<User>();
            var korIme = HttpContext.Current.Request.Params["ime"];
            List<GroupTraining> lista1 = new List<GroupTraining>();

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/GroupTrainings.json");
            string path1 = Path.Combine(directory, "App_Data/Users.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<GroupTraining>>(jsonData);

            var jsonData1 = System.IO.File.ReadAllText(path1);
            treneri = JsonConvert.DeserializeObject<List<User>>(jsonData1);

            foreach (var u in treneri)
            {
                if (korIme == u.Username && u.Role == UserType.Coach)
                {
                    if (u.CoachGroupTrainings != null)
                    {
                        lista1 = u.CoachGroupTrainings.FindAll(x => x.Deleted != true && x.DateTime < DateTime.Now);
                    }
                    /*if(u.CoachGroupTrainings==null)
                    {
                        return null; 
                    }
                    else
                    {
                        return lista1;
                    }*/
                }
            }

            if(lista1.Count!=0)
            {
                return lista1;
            }

            return null;
        }
        [HttpGet, Route("api/trener/NazivRastuce")]
        public List<GroupTraining> NazivRastuce()
        {
            List<GroupTraining> lista = new List<GroupTraining>();
            List<User> treneri = new List<User>();
            var korIme = HttpContext.Current.Request.Params["koIme"];
            List<GroupTraining> lista1 = new List<GroupTraining>();

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/GroupTrainings.json");
            string path1 = Path.Combine(directory, "App_Data/Users.json");

            /*var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<GroupTraining>>(jsonData);*/

            var jsonData1 = System.IO.File.ReadAllText(path1);
            treneri = JsonConvert.DeserializeObject<List<User>>(jsonData1);

            foreach (var u in treneri)
            {
                if (korIme == u.Username && u.Role == UserType.Coach)
                {
                    if (u.CoachGroupTrainings != null)
                    {
                        lista1 = u.CoachGroupTrainings.FindAll(x => x.Deleted != true && x.DateTime < DateTime.Now);
                    }
                    /*if (u.CoachGroupTrainings.Count==0)
                    {
                        return null;
                    }
                    else
                    {
                        return lista1.OrderBy(o=>o.Name).ToList();
                    }*/
                }
            }

            if (lista1.Count != 0)
            {
                return lista1.OrderBy(o => o.Name).ToList();
            }

            return null;
        }

        [HttpGet, Route("api/trener/NazivOpadajuce")]
        public List<GroupTraining> NazivOpadajuce()
        {
            List<GroupTraining> lista = new List<GroupTraining>();
            List<User> treneri = new List<User>();
            var korIme = HttpContext.Current.Request.Params["koIme"];
            List<GroupTraining> lista1 = new List<GroupTraining>();

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/GroupTrainings.json");
            string path1 = Path.Combine(directory, "App_Data/Users.json");
            

            var jsonData1 = System.IO.File.ReadAllText(path1);
            treneri = JsonConvert.DeserializeObject<List<User>>(jsonData1);

            foreach (var u in treneri)
            {
                if (korIme == u.Username && u.Role == UserType.Coach)
                {
                    if (u.CoachGroupTrainings != null)
                    {
                        lista1 = u.CoachGroupTrainings.FindAll(x => x.Deleted != true && x.DateTime < DateTime.Now);
                    }
                    /*if (u.CoachGroupTrainings.Count==0)
                    {
                        return null;
                    }
                    else
                    {
                        return lista1.OrderByDescending(o => o.Name).ToList();
                    }*/
                }
            }

            if (lista1.Count != 0)
            {
                return lista1.OrderByDescending(o => o.Name).ToList();
            }

            return null;
        }

        [HttpGet, Route("api/trener/TipRastuce")]
        public List<GroupTraining> TipRastuce()
        {
            List<GroupTraining> lista = new List<GroupTraining>();
            List<User> treneri = new List<User>();
            var korIme = HttpContext.Current.Request.Params["koIme"];
            List<GroupTraining> lista1 = new List<GroupTraining>();

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/GroupTrainings.json");
            string path1 = Path.Combine(directory, "App_Data/Users.json");


            var jsonData1 = System.IO.File.ReadAllText(path1);
            treneri = JsonConvert.DeserializeObject<List<User>>(jsonData1);

            foreach (var u in treneri)
            {
                if (korIme == u.Username && u.Role == UserType.Coach)
                {
                    if (u.CoachGroupTrainings != null)
                    {
                        lista1 = u.CoachGroupTrainings.FindAll(x => x.Deleted != true && x.DateTime < DateTime.Now);
                    }
                    /*if (u.CoachGroupTrainings.Count == 0)
                    {
                        return null;
                    }
                    else
                    {
                        return lista1.OrderBy(o => o.TrainingType.ToString()).ToList();
                    }*/
                }
            }
            if (lista1.Count != 0)
            {
                return lista1.OrderBy(o => o.TrainingType.ToString()).ToList();
            }

            return null;
        }

        [HttpGet, Route("api/trener/TipOpadajuce")]
        public List<GroupTraining> TipOpadajuce()
        {
            List<GroupTraining> lista = new List<GroupTraining>();
            List<User> treneri = new List<User>();
            var korIme = HttpContext.Current.Request.Params["koIme"];
            List<GroupTraining> lista1 = new List<GroupTraining>();

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/GroupTrainings.json");
            string path1 = Path.Combine(directory, "App_Data/Users.json");


            var jsonData1 = System.IO.File.ReadAllText(path1);
            treneri = JsonConvert.DeserializeObject<List<User>>(jsonData1);

            foreach (var u in treneri)
            {
                if (korIme == u.Username && u.Role == UserType.Coach)
                {
                    if (u.CoachGroupTrainings != null)
                    {
                        lista1 = u.CoachGroupTrainings.FindAll(x => x.Deleted != true && x.DateTime < DateTime.Now);
                    }
                    /*if (u.CoachGroupTrainings.Count == 0)
                    {
                        return null;
                    }
                    else
                    {
                        return lista1.OrderByDescending(o => o.TrainingType.ToString()).ToList();
                    }*/
                }
            }

            if (lista1.Count != 0)
            {
                return lista1.OrderByDescending(o => o.TrainingType.ToString()).ToList();
            }

            return null;
        }

        [HttpGet, Route("api/trener/DatumRastuce")]
        public List<GroupTraining> DatumRastuce()
        {
            List<GroupTraining> lista = new List<GroupTraining>();
            List<User> treneri = new List<User>();
            var korIme = HttpContext.Current.Request.Params["koIme"];
            List<GroupTraining> lista1 = new List<GroupTraining>();

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/GroupTrainings.json");
            string path1 = Path.Combine(directory, "App_Data/Users.json");


            var jsonData1 = System.IO.File.ReadAllText(path1);
            treneri = JsonConvert.DeserializeObject<List<User>>(jsonData1);

            foreach (var u in treneri)
            {
                if (korIme == u.Username && u.Role == UserType.Coach)
                {
                    if (u.CoachGroupTrainings != null)
                    {
                        lista1 = u.CoachGroupTrainings.FindAll(x => x.Deleted != true && x.DateTime < DateTime.Now);
                    }
                    /*if (u.CoachGroupTrainings.Count == 0)
                    {
                        return null;
                    }
                    else
                    {
                        return lista1.OrderBy(o => o.DateTime).ToList();
                    }*/
                }
            }

            if (lista1.Count != 0)
            {
                return lista1.OrderBy(o => o.DateTime).ToList();
            }

            return null;
        }

        [HttpGet, Route("api/trener/DatumOpadajuce")]
        public List<GroupTraining> DatumOpadajuce()
        {
            List<GroupTraining> lista = new List<GroupTraining>();
            List<User> treneri = new List<User>();
            var korIme = HttpContext.Current.Request.Params["koIme"];
            List<GroupTraining> lista1 = new List<GroupTraining>();

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/GroupTrainings.json");
            string path1 = Path.Combine(directory, "App_Data/Users.json");


            var jsonData1 = System.IO.File.ReadAllText(path1);
            treneri = JsonConvert.DeserializeObject<List<User>>(jsonData1);

            foreach (var u in treneri)
            {
                if (korIme == u.Username && u.Role == UserType.Coach)
                {
                    if (u.CoachGroupTrainings != null)
                    {
                        lista1 = u.CoachGroupTrainings.FindAll(x => x.Deleted != true && x.DateTime < DateTime.Now);
                    }
                    /*if (u.CoachGroupTrainings.Count == 0)
                    {
                        return null;
                    }
                    else
                    {
                        return lista1.OrderByDescending(o => o.DateTime).ToList();
                    }*/
                }
            }

            if(lista1.Count!=0)
            {
                return lista1.OrderByDescending(o => o.DateTime).ToList();
            }

            return null;
        }

        [HttpGet, Route("api/trener/Posetioci")]
        public List<string> Posetioci()
        {
            var korIme = HttpContext.Current.Request.Params["ime"];
            var id = HttpContext.Current.Request.Params["id"];
            Int32.TryParse(id, out int idd);
            List<GroupTraining> lista = new List<GroupTraining>();

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/GroupTrainings.json");
            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<GroupTraining>>(jsonData);

            foreach(var v in lista)
            {
                if(v.Id==idd && v.Deleted==false)
                {
                    return v.Users;
                }
            }

            return null;
        }

        [HttpGet, Route("api/trener/Godina")]
        public FitnessCenter Godina()
        {
            List<FitnessCenter> lista = new List<FitnessCenter>();
            List<User> treneri = new List<User>();
            var korIme = HttpContext.Current.Request.Params["koIme"];
            var ime = HttpContext.Current.Request.Params["ime"];
            var tip = HttpContext.Current.Request.Params["tip"];
            var vreme = HttpContext.Current.Request.Params["vreme1"];
            var vreme1 = HttpContext.Current.Request.Params["vreme2"];
            Int32.TryParse(vreme, out int rez1);
            Int32.TryParse(vreme1, out int rez2);

            //var vrednost = -1;

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/FitnessCenters.json");
            string path1 = Path.Combine(directory, "App_Data/Users.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<FitnessCenter>>(jsonData);

            var jsonData1 = System.IO.File.ReadAllText(path1);
            treneri = JsonConvert.DeserializeObject<List<User>>(jsonData1);

            foreach(var u in treneri)
            {
                if (u.Role == UserType.Coach && u.Username==korIme)
                {
                    foreach (var fc in lista)
                    {
                        if (fc.Id == u.FCEngagement)
                        {
                            return fc;
                        }
                    }
                }
            }

            
            return null;
        }

        [HttpPost, Route("api/trener/Pretraga")]
        public List<GroupTraining> Pretraga()
        {
            List<User> lista = new List<User>();
            List<GroupTraining> empty = new List<GroupTraining>();

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/Users.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<User>>(jsonData);


            var ime = HttpContext.Current.Request.Params["ime"];
            var tip = HttpContext.Current.Request.Params["tip"];
            var vreme = HttpContext.Current.Request.Params["vreme"];
            var vreme1 = HttpContext.Current.Request.Params["vreme1"];
            var jedan = HttpContext.Current.Request.Params["jedan"];
            var dva = HttpContext.Current.Request.Params["dva"];
            var koIme = HttpContext.Current.Request.Params["koIme"];
            var obj = tip.Replace(" ","");
            Int32.TryParse(vreme, out int prva);
            Int32.TryParse(vreme1, out int druga);
            DateTime.TryParse(jedan, out DateTime date1);
            DateTime.TryParse(dva, out DateTime date2);


            foreach (var niz in lista)
            {
                if(koIme==niz.Username && niz.Role==UserType.Coach && niz.CoachGroupTrainings!=null)
                {
                    foreach(var g in niz.CoachGroupTrainings)
                    {
                        if (g.Deleted == false && g.DateTime<DateTime.Now /*&& (g.DateTime<=date2 || g.DateTime>=date1)*/)
                        {
                            if (g.Name.ToLower() == ime.ToLower() && obj == g.TrainingType.ToString() && date1 <= g.DateTime && date2 >= g.DateTime)
                            {
                                if (!empty.Contains(g))
                                {
                                    empty.Add(g);
                                }
                            }
                            else if (g.Name.ToLower() == ime.ToLower() && obj == "(empty)" && (jedan == null || jedan=="") && (dva == null || dva == ""))
                            {
                                if (!empty.Contains(g))
                                {
                                    empty.Add(g);
                                }
                            }
                            else if ((ime == null || ime == "") && obj == g.TrainingType.ToString() && (jedan == null || jedan == "") && (dva == null || dva == ""))
                            {
                                if (!empty.Contains(g))
                                {
                                    empty.Add(g);
                                }
                            }
                            else if ((ime == null || ime == "") && obj == "(empty)" && date1 <= g.DateTime && (dva == null || dva == ""))
                            {
                                if (!empty.Contains(g))
                                {
                                    empty.Add(g);
                                }
                            }
                            else if ((ime == null || ime == "") && obj == "(empty)" && (jedan == null || jedan == "") && date2 >= g.DateTime)
                            {
                                if (!empty.Contains(g))
                                {
                                    empty.Add(g);
                                }
                            }
                            else if (g.Name.ToLower() == ime.ToLower() && obj == g.TrainingType.ToString() && (jedan == null || jedan == "") && (dva == null || dva == ""))
                            {
                                if (!empty.Contains(g))
                                {
                                    empty.Add(g);
                                }
                            }
                            else if (g.Name.ToLower() == ime.ToLower() && obj == "(empty)" && date1 <= g.DateTime && (dva == null || dva == ""))
                            {
                                if (!empty.Contains(g))
                                {
                                    empty.Add(g);
                                }
                            }
                            else if (g.Name.ToLower() == ime.ToLower() && obj == "(empty)" && (jedan == null || jedan == "") && date2 >= g.DateTime)
                            {
                                if (!empty.Contains(g))
                                {
                                    empty.Add(g);
                                }
                            }
                            else if ((ime == null || ime == "") && obj == g.TrainingType.ToString() && date1 <= g.DateTime && (dva == null || dva == ""))
                            {
                                if (!empty.Contains(g))
                                {
                                    empty.Add(g);
                                }
                            }
                            else if ((ime == null || ime == "") && obj == g.TrainingType.ToString() && (jedan == null || jedan == "") && date2 >= g.DateTime)
                            {
                                if (!empty.Contains(g))
                                {
                                    empty.Add(g);
                                }
                            }
                            else if ((ime == null || ime == "") && obj == "(empty)" && date1 <= g.DateTime && date2 >= g.DateTime)
                            {
                                if (!empty.Contains(g))
                                {
                                    empty.Add(g);
                                }
                            }
                            else if (g.Name.ToLower() == ime.ToLower() && obj == g.TrainingType.ToString() && date1 <= g.DateTime && (dva == null || dva == ""))
                            {
                                if (!empty.Contains(g))
                                {
                                    empty.Add(g);
                                }
                            }
                            else if (g.Name.ToLower() == ime.ToLower() && obj == g.TrainingType.ToString() && (jedan == null || jedan == "") && date2 >= g.DateTime)
                            {
                                if (!empty.Contains(g))
                                {
                                    empty.Add(g);
                                }
                            }
                            else if (g.Name.ToLower() == ime.ToLower() && obj == "(empty)" && date1 <= g.DateTime && date2 >= g.DateTime)
                            {
                                if (!empty.Contains(g))
                                {
                                    empty.Add(g);
                                }
                            }
                            else if ((ime == null && ime=="") && obj == g.TrainingType.ToString() && date1 <= g.DateTime && date2 >= g.DateTime)
                            {
                                if (!empty.Contains(g))
                                {
                                    empty.Add(g);
                                }
                            }
                        }
                    }
                }
            }

            if(empty.Count==0)
            {
                return null;
            }

            return empty;
        }
    }
}
