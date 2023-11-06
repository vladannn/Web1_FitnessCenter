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
    public class VlasnikController : ApiController
    {
        [HttpGet, Route("api/vlasnik/Uloga")]
        public User Uloga()
        {
            User u = new User();
            List<User> lista = new List<User>();
            var username = HttpContext.Current.Request.Params["koIme"];
            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/Users.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<User>>(jsonData);

            foreach (var item in lista)
            {
                if (item.Username == username && item.Deleted==false)
                {
                    u = item;
                    return u;
                }
            }
            return null;
        }

        [HttpPost, Route("api/vlasnik/DodavanjeTrenera")]
        public bool DodavanjeTrenera()
        {
            List<User> lista = new List<User>();
            List<FitnessCenter> listaCentara = new List<FitnessCenter>();
            //FitnessCenter f = new FitnessCenter();
            User u = new User();
            var idCentra = HttpContext.Current.Request.Params["id"];
            var name = HttpContext.Current.Request.Params["name"];
            var lastname = HttpContext.Current.Request.Params["lastname"];
            var username = HttpContext.Current.Request.Params["username"];
            var email = HttpContext.Current.Request.Params["email"];
            var password = HttpContext.Current.Request.Params["password"];
            var datetime = HttpContext.Current.Request.Params["datetime"];
            var gender = HttpContext.Current.Request.Params["gender"];
            var korIme = HttpContext.Current.Request.Params["korIme"];
            Int32.TryParse(idCentra, out int idd);

            DateTime.TryParse(datetime, out DateTime dateTime);
            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/Users.json");
            string path1 = Path.Combine(directory, "App_Data/FitnessCenters.json");

            var jsonData1 = System.IO.File.ReadAllText(path1);
            listaCentara = JsonConvert.DeserializeObject<List<FitnessCenter>>(jsonData1);


            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<User>>(jsonData);


            u.FirstName = name;
            u.LastName = lastname;
            u.Username = username;
            u.Email = email;
            u.Password = password;
            if (gender == "Muski")
            {
                u.Gender = GenderType.Muski;
            }
            else
            {
                u.Gender = GenderType.Zenski;
            }
            u.DateTime = dateTime;
            u.Role = UserType.Coach;
            u.Deleted = false;

            foreach (var item in lista)
            {

                if (item.Username == username && item.Role==UserType.Coach && item.FCEngagement==idd)
                {
                    return false;
                }
                else if (item.Email == email && item.Role == UserType.Coach && item.FCEngagement == idd)
                {
                    return false;
                }
                else if(item.Username == username)
                {
                    return false;
                }
            }

            foreach(var a in listaCentara)
            {
                //var meh = a.Id;
                if(idd==a.Id && a.Deleted==false)
                {

                    u.FCEngagement = idd;

                }
                
            }
            if (lista == null)
            {
                lista = new List<User>();
                lista.Add(u);
            }
            else
            {
                lista.Add(u);
            }
            using (StreamWriter streamWriter = new StreamWriter(path/*, true*/))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(streamWriter, lista);
            }

            return true;
        }
        [HttpPost, Route("api/vlasnik/SpisakCentara")]
        public List<FitnessCenter> SpisakCentara()
        {
            List<FitnessCenter> lista = new List<FitnessCenter>();
            List<FitnessCenter> povratna = new List<FitnessCenter>();

            var username = HttpContext.Current.Request.Params["ime"];
            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/FitnessCenters.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<FitnessCenter>>(jsonData);

            foreach(var i in lista)
            {
                if(i.Owner==username && i.Deleted==false)
                {
                    povratna.Add(i);
                }
            }

            return povratna;
        }

        [HttpPost, Route("api/vlasnik/DodavanjeCentra")]
        public IHttpActionResult DodavanjeCentra()
        {
            List<FitnessCenter> lista = new List<FitnessCenter>();
            List<User> lista2 = new List<User>();
            FitnessCenter f = new FitnessCenter();
            Address a = new Address();


            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/FitnessCenters.json");
            string path1 = Path.Combine(directory, "App_Data/Users.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<FitnessCenter>>(jsonData);

            var jsonData1 = System.IO.File.ReadAllText(path1);
            lista2 = JsonConvert.DeserializeObject<List<User>>(jsonData1);

            var korisnickoIme = HttpContext.Current.Request.Params["koIme"];
            var ime = HttpContext.Current.Request.Params["ime"];
            var adresa = HttpContext.Current.Request.Params["adresa"];
            var godina = HttpContext.Current.Request.Params["godina"];
            var mesecna = HttpContext.Current.Request.Params["mesecna"];
            var godisnja = HttpContext.Current.Request.Params["godisnja"];
            var jednodnevni = HttpContext.Current.Request.Params["jednodnevni"];
            var grupni = HttpContext.Current.Request.Params["grupni"];
            var personalni = HttpContext.Current.Request.Params["personalni"];

            var niz = adresa.Split(',');
            var ulica = niz[0];
            var broj = niz[1];
            var grad = niz[2];
            var posBroj = niz[3];

            a.Ulica = ulica;
            a.Broj = broj;
            a.Mesto = grad;
            a.PostanskiBroj = posBroj;
            f.Name = ime;
            f.Address = a;
            f.YMembership = Int32.Parse(godisnja);
            f.Owner = korisnickoIme;
            f.MonMembership = Int32.Parse(mesecna);
            f.OpeningYear= Int32.Parse(godina);
            f.OneDayPrice = Int32.Parse(jednodnevni);
            f.GroupTrPrice = Int32.Parse(grupni);
            f.OneTrWCoach = Int32.Parse(personalni);
            f.Deleted = false;

            if (lista == null)
            {
                lista = new List<FitnessCenter>();
                lista.Add(new FitnessCenter(ime, a, Int32.Parse(godina), korisnickoIme, Int32.Parse(mesecna), Int32.Parse(godisnja), Int32.Parse(jednodnevni), Int32.Parse(grupni), Int32.Parse(personalni), false));
                
            }
            else
            {
                lista.Add(new FitnessCenter(ime, a, Int32.Parse(godina), korisnickoIme, Int32.Parse(mesecna), Int32.Parse(godisnja), Int32.Parse(jednodnevni), Int32.Parse(grupni), Int32.Parse(personalni), false));
            }
            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(streamWriter, lista);
            }

            foreach (var u in lista2)
            {
                if (korisnickoIme == u.Username)
                {
                    if (u.FCOwner == null)
                    {
                        u.FCOwner = new List<int>();
                        foreach(var s in lista)
                        {
                            if(s.Owner==korisnickoIme)
                            {
                                u.FCOwner.Add(s.Id);
                                using (StreamWriter streamWriter = new StreamWriter(path1))
                                {
                                    JsonSerializer serializer = new JsonSerializer();
                                    serializer.Serialize(streamWriter, lista2);
                                }
                            }
                        }
                    }
                    else
                    {
                        u.FCOwner = null;
                        u.FCOwner = new List<int>();
                        foreach (var s in lista)
                        {
                            if (s.Owner == korisnickoIme)
                            {
                                u.FCOwner.Add(s.Id);
                                
                            }
                        }
                        using (StreamWriter streamWriter = new StreamWriter(path1))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            serializer.Serialize(streamWriter, lista2);
                        }
                    }
                }
            }

            return Ok();
        }
        [HttpGet, Route("api/vlasnik/Lista")]
        public FitnessCenter Lista()
        {
            List<FitnessCenter> lista = new List<FitnessCenter>();
            FitnessCenter f = new FitnessCenter();

            var id = HttpContext.Current.Request.Params["id"];
            var idd =Int32.Parse(id);
            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/FitnessCenters.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<FitnessCenter>>(jsonData);

            foreach(var v in lista)
            {
                if(v.Id==idd && v.Deleted==false)
                {
                    f = v;
                }
            }

            return f;

        }
        
        [HttpPost, Route("api/vlasnik/IzmenaCentra")]
        public FitnessCenter IzmenaCentra()
        {
            List<FitnessCenter> lista = new List<FitnessCenter>();
            FitnessCenter f = new FitnessCenter();
            List<User> korisnici = new List<User>();
            Address a = new Address();
            List<GroupTraining> lista2 = new List<GroupTraining>();


            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/FitnessCenters.json");
            string path1 = Path.Combine(directory, "App_Data/GroupTrainings.json");
            string path2 = Path.Combine(directory, "App_Data/Users.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<FitnessCenter>>(jsonData);

            var jsonData1 = System.IO.File.ReadAllText(path1);
            lista2 = JsonConvert.DeserializeObject<List<GroupTraining>>(jsonData1);

            var jsonData2 = System.IO.File.ReadAllText(path2);
            korisnici = JsonConvert.DeserializeObject<List<User>>(jsonData2);

            var id = HttpContext.Current.Request.Params["id"];
            var koIme= HttpContext.Current.Request.Params["koIme"];
            var ime = HttpContext.Current.Request.Params["ime"];
            var adresa = HttpContext.Current.Request.Params["adresa"];
            var godina = HttpContext.Current.Request.Params["godina"];
            var mesecna = HttpContext.Current.Request.Params["mesecna"];
            var godisnja = HttpContext.Current.Request.Params["godisnja"];
            var jednodnevni = HttpContext.Current.Request.Params["jednodnevni"];
            var grupni = HttpContext.Current.Request.Params["grupni"];
            var personalni = HttpContext.Current.Request.Params["personalni"];
            var idd = Int32.Parse(id);

            var niz = adresa.Split(',');
            var ulica = niz[0];
            var broj = niz[1];
            var grad = niz[2];
            var posBroj = niz[3];

            foreach(var fc in lista)
            {
                if(idd==fc.Id && fc.Deleted==false)
                {
                    f = fc;
                    fc.Name = ime;
                    fc.Address.Ulica = ulica;
                    fc.Address.Broj = broj;
                    fc.Address.Mesto = grad;
                    fc.Address.PostanskiBroj = posBroj;
                    fc.OpeningYear = Int32.Parse(godina);
                    fc.MonMembership = Int32.Parse(mesecna);
                    fc.YMembership = Int32.Parse(godisnja);
                    fc.OneDayPrice = Int32.Parse(jednodnevni);
                    fc.GroupTrPrice = Int32.Parse(grupni);
                    fc.OneTrWCoach = Int32.Parse(personalni);
                    if(lista2!=null)
                    {
                        foreach(var gtr in lista2)
                        {
                            if(gtr.Place.Id==idd)
                            {
                                gtr.Place.Name =ime;
                                gtr.Place.Address.Ulica = ulica;
                                gtr.Place.Address.Broj = broj;
                                gtr.Place.Address.Mesto = grad;
                                gtr.Place.Address.PostanskiBroj = posBroj;
                                gtr.Place.OpeningYear= Int32.Parse(godina);
                                gtr.Place.MonMembership = Int32.Parse(mesecna);
                                gtr.Place.YMembership = Int32.Parse(godisnja);
                                gtr.Place.OneDayPrice = Int32.Parse(jednodnevni);
                                gtr.Place.GroupTrPrice = Int32.Parse(grupni);
                                gtr.Place.OneTrWCoach = Int32.Parse(personalni);
                                
                            }
                        }
                        using (StreamWriter streamWriter = new StreamWriter(path1))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            serializer.Serialize(streamWriter, lista2);
                        }
                    }
                    if(korisnici!=null)
                    {
                        foreach(var k in korisnici)
                        {
                            if(k.VisitorGroupTrainings!=null)
                            {
                                foreach(var tr in k.VisitorGroupTrainings)
                                {
                                    if(tr.Place.Id==idd)
                                    {
                                        tr.Place.Name = ime;
                                        tr.Place.Address.Ulica = ulica;
                                        tr.Place.Address.Broj = broj;
                                        tr.Place.Address.Mesto = grad;
                                        tr.Place.Address.PostanskiBroj = posBroj;
                                        tr.Place.OpeningYear = Int32.Parse(godina);
                                        tr.Place.MonMembership = Int32.Parse(mesecna);
                                        tr.Place.YMembership = Int32.Parse(godisnja);
                                        tr.Place.OneDayPrice = Int32.Parse(jednodnevni);
                                        tr.Place.GroupTrPrice = Int32.Parse(grupni);
                                        tr.Place.OneTrWCoach = Int32.Parse(personalni);
                                    }
                                }
                            }
                            else if(k.CoachGroupTrainings!=null)
                            {
                                foreach(var mr in k.CoachGroupTrainings)
                                {
                                    if (mr.Place.Id == idd)
                                    {
                                        mr.Place.Name = ime;
                                        mr.Place.Address.Ulica = ulica;
                                        mr.Place.Address.Broj = broj;
                                        mr.Place.Address.Mesto = grad;
                                        mr.Place.Address.PostanskiBroj = posBroj;
                                        mr.Place.OpeningYear = Int32.Parse(godina);
                                        mr.Place.MonMembership = Int32.Parse(mesecna);
                                        mr.Place.YMembership = Int32.Parse(godisnja);
                                        mr.Place.OneDayPrice = Int32.Parse(jednodnevni);
                                        mr.Place.GroupTrPrice = Int32.Parse(grupni);
                                        mr.Place.OneTrWCoach = Int32.Parse(personalni);
                                    }
                                }
                            }
                            
                        }
                        using (StreamWriter streamWriter = new StreamWriter(path2))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            serializer.Serialize(streamWriter, korisnici);
                        }
                    }
                }
            }

            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(streamWriter, lista);
            }

            if(f==null)
            {
                return null;
            }

            return f;
        }

        [HttpDelete, Route("api/vlasnik/Blokiranje")]
        public bool Blokiranje()
        {
            var username = HttpContext.Current.Request.Params["polje"];
            var fom = HttpContext.Current.Request.Params["fom"];
            List<GroupTraining> grupni = new List<GroupTraining>();
            List<User> treneri = new List<User>();
            List<int> brojevi = new List<int>();

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/Users.json");
            string path1 = Path.Combine(directory, "App_Data/GroupTrainings.json");

            var jsonData = System.IO.File.ReadAllText(path);
            treneri = JsonConvert.DeserializeObject<List<User>>(jsonData);

            var jsonData1 = System.IO.File.ReadAllText(path1);
            grupni = JsonConvert.DeserializeObject<List<GroupTraining>>(jsonData1);
            var prom = false;

            foreach(var b in treneri)
            {
                if(fom==b.Username)
                {
                    brojevi = b.FCOwner;
                }
            }

            foreach (var t in treneri)
            {
                if(t.Deleted==false && t.Username==username && t.Role==UserType.Coach && brojevi.Contains(t.FCEngagement))
                {
                    t.Deleted = true;
                    t.FCEngagement = -1;
                    //t.CoachGroupTrainings = null; ubaciti unutra kasnije
                    if (grupni.Count != 0)
                    {
                        foreach (var g in grupni)
                        {
                            /*if (t.FCEngagement == g.Place.Id)
                            {
                                g.Deleted = true;
                                using (StreamWriter streamWriter = new StreamWriter(path1))
                                {
                                    JsonSerializer serializer = new JsonSerializer();
                                    serializer.Serialize(streamWriter, grupni);
                                }
                            }*/
                            if(t.CoachGroupTrainings.Count!=0)
                            {
                                foreach(var q in t.CoachGroupTrainings)
                                {
                                    if(q.Id==g.Id && q.DateTime>=DateTime.Now)
                                    {
                                        q.Deleted = true;
                                        prom = true;
                                        q.Users = null;
                                        foreach (var r in treneri)
                                        {
                                            if (r.Role == UserType.Visitor && r.VisitorGroupTrainings != null && r.Deleted == false)
                                            {
                                                foreach (var i in r.VisitorGroupTrainings)
                                                {
                                                    if (g.Id==i.Id)
                                                    {
                                                        i.Deleted = true;
                                                        i.Users = null;
                                                    }
                            }
                                            }
                                        }
                                    }
                                }
                            }
                            if(prom)
                            {
                                g.Deleted = true;
                                g.Users = null;
                                prom = false;
                            }
                        }
                    }

                    //t.CoachGroupTrainings = null;
                    /* foreach(var r in treneri)
                     {
                         if (r.Role == UserType.Visitor && r.VisitorGroupTrainings != null && r.Deleted==false)
                         {
                             foreach(var i in r.VisitorGroupTrainings)
                             {
                                 if(t.FCEngagement==i.Place.Id)
                             }
                         }
                     }*/
                    using (StreamWriter streamWriter = new StreamWriter(path1))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(streamWriter, grupni);
                    }

                    using (StreamWriter streamWriter = new StreamWriter(path))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(streamWriter, treneri);
                    }
                    return true;
                }
            }

            return false;
        }

        [HttpDelete, Route("api/vlasnik/Brisanje")]
        public bool Brisanje()
        {
            var id = HttpContext.Current.Request.Params["id"];
            var ime = HttpContext.Current.Request.Params["ime"];
            var idd = Int32.Parse(id);
            bool provera= false;

            List<User> treneri = new List<User>();
            List<GroupTraining> lista = new List<GroupTraining>();
            List <GroupTraining> probna = new List<GroupTraining>();
            List<User> probna1 = new List<User>();
            List<FitnessCenter> fitmesi = new List<FitnessCenter>();
            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/FitnessCenters.json");
            string path1 = Path.Combine(directory, "App_Data/GroupTrainings.json");
            string path2 = Path.Combine(directory, "App_Data/Users.json");

            var jsonData2 = System.IO.File.ReadAllText(path2);
            treneri = JsonConvert.DeserializeObject<List<User>>(jsonData2);

            var jsonData1 = System.IO.File.ReadAllText(path);
            fitmesi = JsonConvert.DeserializeObject<List<FitnessCenter>>(jsonData1);

            var jsonData = System.IO.File.ReadAllText(path1);
            lista = JsonConvert.DeserializeObject<List<GroupTraining>>(jsonData);

            var broj = -1;

            probna = lista.FindAll(m=>m.Place.Id==idd && m.DateTime>DateTime.Now && m.Deleted==false);
            probna1 = treneri.FindAll(v=>v.FCEngagement==idd);

            foreach (var u in treneri)
            {
                if (u.Role == UserType.Coach && probna.Count == 0 && u.FCEngagement == idd && u.Deleted == false)
                {
                    foreach (var g in lista)
                    {
                        if (g.Deleted == false && g.DateTime < DateTime.Now && g.Place.Id == idd)
                        {
                            broj = g.Id;
                            foreach (var f in fitmesi)
                            {
                                if (f.Id == idd && f.Id == g.Place.Id)
                                {
                                    f.Deleted = true;
                                    u.Deleted = true;
                                    g.Place.Deleted = true;
                                    u.FCEngagement = -1;
                                    if (u.CoachGroupTrainings != null)
                                    {
                                        u.CoachGroupTrainings = lista.FindAll(x => x.Id == broj);
                                        foreach (var meh in u.CoachGroupTrainings)
                                        {
                                            meh.Deleted = true;
                                            meh.Place.Deleted = true;
                                            provera = true;
                                            meh.Users = null;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    foreach (var z in treneri)
                    {
                        if (z.Role == UserType.Visitor && z.VisitorGroupTrainings != null)
                        {
                            foreach (var h in z.VisitorGroupTrainings)
                            {
                                if (h.Place.Id == idd)
                                {
                                    h.Place.Deleted = true;
                                    h.Deleted = true;
                                    h.Users = null;
                                }
                            }
                        }
                    }
                }
            }
            foreach (var g in treneri)
            {
                if (g.Role == UserType.Coach && g.CoachGroupTrainings == null && g.FCEngagement == idd)
                {
                    g.Deleted = true;
                    g.FCEngagement = -1;
                    foreach (var fci in fitmesi)
                    {
                        if (fci.Id == idd)
                        {
                            fci.Deleted = true;
                            provera = true;
                        }
                    }
                }
            }

            foreach (var c in treneri)
            {
                if (probna1.Count == 0)
                {
                    foreach (var f in fitmesi)
                    {
                        if (f.Id == idd)
                        {
                            f.Deleted = true;
                            provera = true;
                        }
                    }
                }
            }


            if (provera)
            {
                foreach(var u in treneri)
                {
                    if(u.Username==ime && u.FCOwner!=null)
                    {
                        /*foreach(var h in u.FCOwner)
                        {
                            if(h==idd)
                            {
                                u.FCOwner.Remove(h);
                            }
                        }*/
                        u.FCOwner.RemoveAll(x=>x==idd);
                    }
                }
                using (StreamWriter streamWriter = new StreamWriter(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(streamWriter, fitmesi);
                }
                using (StreamWriter streamWriter = new StreamWriter(path1))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(streamWriter, lista);
                }
                using (StreamWriter streamWriter = new StreamWriter(path2))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(streamWriter, treneri);
                }

                return true;
            }


            return false;
        }
        
    }
}
