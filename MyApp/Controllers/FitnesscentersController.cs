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
    public class FitnesscentersController : ApiController
    {
        //static List<FitnessCenter> Staticka { get; set; } = new List<FitnessCenter>();
        static FitnessCenter Fc = new FitnessCenter();

        [HttpGet, Route("api/fitnesscenters/Vrati")]
        public List<FitnessCenter> Vrati()
        {
            List<FitnessCenter> lista = new List<FitnessCenter>();
            List<FitnessCenter> FitnessCenterss = new List<FitnessCenter>();

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/FitnessCenters.json");

            var jsonData = System.IO.File.ReadAllText(path);
            FitnessCenterss = JsonConvert.DeserializeObject<List<FitnessCenter>>(jsonData);

            foreach(var i in FitnessCenterss)
            {
                if(!i.Deleted)
                {
                    lista.Add(i);
                }
            }

            lista.Sort((x, y) => string.Compare(x.Name, y.Name));
            return lista;
        }
        [HttpGet, Route("api/fitnesscenters/OpadajuceNaziv")]
        public List<FitnessCenter> OpadajuceNaziv()
        {
            List<FitnessCenter> lista1 = new List<FitnessCenter>();
            List<FitnessCenter> lista = new List<FitnessCenter>();


            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/FitnessCenters.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<FitnessCenter>>(jsonData);

            foreach (var i in lista)
            {
                if (!i.Deleted)
                {
                    lista1.Add(i);
                }
            }
            var opadajuce = lista1.OrderByDescending(o=>o.Name).ToList();
            
            
            return opadajuce;
        }

        [HttpGet, Route("api/fitnesscenters/RastuceAdresa")]
        public List<FitnessCenter> RastuceAdresa()
        {
            List<FitnessCenter> lista1 = new List<FitnessCenter>();
            List<FitnessCenter> lista = new List<FitnessCenter>();

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/FitnessCenters.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<FitnessCenter>>(jsonData);
            
            foreach (var i in lista)
            {
                if (!i.Deleted)
                {
                    lista1.Add(i);
                }
            }

            var rastuce = lista1.OrderBy(o => o.Address.Ulica).ThenBy(o => o.Address.Broj).ThenBy(o=>o.Address.Mesto).ThenBy(o=>o.Address.PostanskiBroj).ToList();


            return rastuce;
        }

        [HttpGet, Route("api/fitnesscenters/OpadajuceAdresa")]
        public List<FitnessCenter> OpadajuceAdresa()
        {
            List<FitnessCenter> lista1 = new List<FitnessCenter>();
            List<FitnessCenter> lista = new List<FitnessCenter>();

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/FitnessCenters.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<FitnessCenter>>(jsonData);

            foreach (var i in lista)
            {
                if (!i.Deleted)
                {
                    lista1.Add(i);
                }
            }

            var opadajuca = lista1.OrderByDescending(o => o.Address.Ulica).ThenBy(o => o.Address.Broj).ThenBy(o => o.Address.Mesto).ThenBy(o => o.Address.PostanskiBroj).ToList();


            return opadajuca;
        }

        [HttpGet, Route("api/fitnesscenters/RastuceGodina")]
        public List<FitnessCenter> RastuceGodina()
        {
            List<FitnessCenter> lista1 = new List<FitnessCenter>();
            List<FitnessCenter> lista = new List<FitnessCenter>();

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/FitnessCenters.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<FitnessCenter>>(jsonData);

            foreach (var i in lista)
            {
                if (!i.Deleted)
                {
                    lista1.Add(i);
                }
            }

            var rastuce = lista1.OrderBy(o => o.OpeningYear).ToList();


            return rastuce;
        }

        [HttpGet, Route("api/fitnesscenters/OpadajuceGodina")]
        public List<FitnessCenter> OpadajuceGodina()
        {
            List<FitnessCenter> lista1 = new List<FitnessCenter>();
            List<FitnessCenter> lista = new List<FitnessCenter>();

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/FitnessCenters.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<FitnessCenter>>(jsonData);
            
            foreach (var i in lista)
            {
                if (!i.Deleted)
                {
                    lista1.Add(i);
                }
            }

            var opadajuca = lista1.OrderByDescending(o => o.OpeningYear).ToList();


            return opadajuca;
        }
        [HttpPost, Route("api/fitnesscenters/Detalji")]
        public FitnessCenter Detalji()
        {
            List<FitnessCenter> lista = new List<FitnessCenter>();
            var id = HttpContext.Current.Request.Params["name"];
            Int32.TryParse(id, out int idd);

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/FitnessCenters.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<FitnessCenter>>(jsonData);
            Fc = null;

            Fc = lista.Find(item => item.Id == idd);
            return Fc;
        }
        [HttpGet, Route("api/fitnesscenters/SpisakGT")]
        public List<GroupTraining> SpisakGT()
        {
            List<GroupTraining> lista2 = new List<GroupTraining>();

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/GroupTrainings.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista2 = JsonConvert.DeserializeObject<List<GroupTraining>>(jsonData);

            var lista = new List<GroupTraining>();

            foreach(var i in lista2)
            {
                if(Fc.Id==i.Place.Id && i.Deleted==false)
                {
                    lista.Add(i);
                }
            }

            if (lista.Count != 0)
            {
                return lista;
            }

            return null;
        }

        [HttpGet, Route("api/fitnesscenters/SpisakK")]
        public List<Comment> SpisakK()
        {
            List<Comment> lista2 = new List<Comment>();

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/Comments.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista2 = JsonConvert.DeserializeObject<List<Comment>>(jsonData);
            

            var lista = new List<Comment>();

            foreach (var i in lista2)
            {
                if (Fc.Id == i.FitnessCenter && i.Available==true)
                {
                    lista.Add(i);
                }
            }
            return lista;
        }

        [HttpPost, Route("api/fitnesscenters/Pretraga")]
        public List<FitnessCenter> Pretraga()
        {
            List<FitnessCenter> lista = new List<FitnessCenter>();

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/FitnessCenters.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<FitnessCenter>>(jsonData);
            
            List<FitnessCenter> empty = new List<FitnessCenter>();
            

            var name = HttpContext.Current.Request.Params["name"];
            var adresa = HttpContext.Current.Request.Params["adresa"];
            var godina11 = HttpContext.Current.Request.Params["godina1"];
            var godina12 = HttpContext.Current.Request.Params["godina2"];

            Int32.TryParse(godina11, out int godina1);
            Int32.TryParse(godina12, out int godina2);

            //Address adresaa = new Address();
            //string meh = adresaa.Ulica + ", " + adresaa.Broj + ", " + adresaa.Mesto + ", " + adresaa.PostanskiBroj;

            string[] adresica = adresa.Split(',');

            foreach(var i in lista)
            {
                if(i.Name.ToLower()==name.ToLower() && i.Address.Ulica.ToLower()==adresica[0].ToLower() && i.Address.Broj.ToLower() == adresica[1].ToLower() && i.Address.Mesto.ToLower() == adresica[2].ToLower() && i.Address.PostanskiBroj.ToLower() == adresica[3].ToLower() && godina1<i.OpeningYear && godina2>i.OpeningYear && i.Deleted==false)
                {
                    if (!empty.Contains(i))
                    {
                        empty.Add(i);
                    }
                }
                else if (i.Name.ToLower() == name.ToLower() && (adresa==null || adresa=="") && (godina11==null || godina11=="") && (godina12==null || godina12=="") && i.Deleted == false)
                {
                    if (!empty.Contains(i))
                    {
                        empty.Add(i);
                    }
                }
                else if(i.Name.ToLower()==name.ToLower() && i.Address.Ulica.ToLower() == adresica[0].ToLower() && i.Address.Broj.ToLower() == adresica[1].ToLower() && i.Address.Mesto.ToLower() == adresica[2].ToLower() && i.Address.PostanskiBroj.ToLower() == adresica[3].ToLower() && (godina11 == null || godina11 == "") && (godina12 == null || godina12 == "") && i.Deleted == false)
                {
                    if (!empty.Contains(i))
                    {
                        empty.Add(i);
                    }
                }
                else if(i.Name.ToLower() == name.ToLower() && godina1 < i.OpeningYear && godina2 > i.OpeningYear && (adresa == null || adresa == "") && i.Deleted == false)
                {
                    if (!empty.Contains(i))
                    {
                        empty.Add(i);
                    }
                }
                else if(i.Name.ToLower() == name.ToLower() && godina1 < i.OpeningYear && (adresa == null || adresa == "") && (godina12 == null || godina12 == "") && i.Deleted == false)
                {
                    if (!empty.Contains(i))
                    {
                        empty.Add(i);
                    }
                }
                else if(i.Name.ToLower() == name.ToLower() && godina2 > i.OpeningYear && (adresa == null || adresa == "") && (godina11 == null || godina11 == "") && i.Deleted == false)
                {
                    if (!empty.Contains(i))
                    {
                        empty.Add(i);
                    }
                }
                else if(i.Address.Ulica.ToLower() == adresica[0].ToLower() && i.Address.Broj.ToLower() == adresica[1].ToLower() && i.Address.Mesto.ToLower() == adresica[2].ToLower() && i.Address.PostanskiBroj.ToLower() == adresica[3].ToLower() && godina1 < i.OpeningYear && godina2 > i.OpeningYear && (name == null || name=="") && i.Deleted == false)
                {
                    if (!empty.Contains(i))
                    {
                        empty.Add(i);
                    }
                }
                else if(i.Address.Ulica.ToLower() == adresica[0].ToLower() && i.Address.Broj.ToLower() == adresica[1].ToLower() && i.Address.Mesto.ToLower() == adresica[2].ToLower() && i.Address.PostanskiBroj.ToLower() == adresica[3].ToLower() && (name == null || name == "") && (godina11 == null || godina11 == "") && (godina12 == null || godina12 == "") && i.Deleted == false)
                {
                    if (!empty.Contains(i))
                    {
                        empty.Add(i);
                    }
                }
                else if(i.Address.Ulica.ToLower() == adresica[0].ToLower() && i.Address.Broj.ToLower() == adresica[1].ToLower() && i.Address.Mesto.ToLower() == adresica[2].ToLower() && i.Address.PostanskiBroj.ToLower() == adresica[3].ToLower() && godina1 < i.OpeningYear && (godina12 == null || godina12 == "") && (name == null || name == "") && i.Deleted == false)
                {
                    if (!empty.Contains(i))
                    {
                        empty.Add(i);
                    }
                }
                else if(i.Address.Ulica.ToLower() == adresica[0].ToLower() && i.Address.Broj.ToLower() == adresica[1].ToLower() && i.Address.Mesto.ToLower() == adresica[2].ToLower() && i.Address.PostanskiBroj.ToLower() == adresica[3].ToLower() && godina2 > i.OpeningYear && (name == null || name == "") && (godina11 == null || godina11 == "") && i.Deleted == false)
                {
                    if (!empty.Contains(i))
                    {
                        empty.Add(i);
                    }
                }
                else if(godina1 < i.OpeningYear && (adresa == null || adresa == "") && (godina12 == null || godina12 == "") && (name == null || name == "") && i.Deleted == false)
                {
                    if (!empty.Contains(i))
                    {
                        empty.Add(i);
                    }
                }
                else if(godina2 > i.OpeningYear && (adresa == null || adresa == "") && (godina11 == null || godina11 == "") && (name == null || name == "") && i.Deleted == false)
                {
                    if (!empty.Contains(i))
                    {
                        empty.Add(i);
                    }
                }
                else if(godina1 < i.OpeningYear && godina2 > i.OpeningYear && (adresa == null || adresa == "") && (name == null || name == "") && i.Deleted == false)
                {
                    if (!empty.Contains(i))
                    {
                        empty.Add(i);
                    }
                }
                else if(i.Name.ToLower() == name.ToLower() && i.Address.Ulica.ToLower() == adresica[0].ToLower() && i.Address.Broj.ToLower() == adresica[1].ToLower() && i.Address.Mesto.ToLower() == adresica[2].ToLower() && i.Address.PostanskiBroj.ToLower() == adresica[3].ToLower() && godina1 < i.OpeningYear && (godina12 == null || godina12 == "") && i.Deleted == false)
                {
                    if (!empty.Contains(i))
                    {
                        empty.Add(i);
                    }
                }
                else if(i.Name.ToLower() == name.ToLower() && i.Address.Ulica.ToLower() == adresica[0].ToLower() && i.Address.Broj.ToLower() == adresica[1].ToLower() && i.Address.Mesto.ToLower() == adresica[2].ToLower() && i.Address.PostanskiBroj.ToLower() == adresica[3].ToLower() && godina2 > i.OpeningYear && (godina11 == null || godina11 == "") && i.Deleted == false)
                {
                    if(!empty.Contains(i))
                    {
                        empty.Add(i);
                    }
                }
            }
            
            if(empty.Count==0)
            {
                return null;
            }

            return empty;
        }

        [HttpGet, Route("api/fitnesscenters/Lista")]
        public FitnessCenter Lista()
        {
            return Fc;
        }

        [HttpGet, Route("api/fitnesscenters/SpisakGrupnih")]
        public List<GroupTraining>SpisakGrupnih()
        {
            List<GroupTraining> lista2 = new List<GroupTraining>();

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/GroupTrainings.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista2 = JsonConvert.DeserializeObject<List<GroupTraining>>(jsonData);

            var lista = new List<GroupTraining>();

            foreach (var i in lista2)
            {
                if (Fc.Id == i.Place.Id && i.Deleted == false && i.DateTime>DateTime.Now)
                {
                    lista.Add(i);
                }
            }
            if (lista.Count != 0)
            {
                return lista;
            }

            return null;
        }

        [HttpPost, Route("api/fitnesscenters/OstaviKomentar")]
        public bool OstaviKomentar()
        {
            List<Comment> lista2 = new List<Comment>();
            List<User> lista = new List<User>();
            var korIme = HttpContext.Current.Request.Params["korIme"];
            var ocena = HttpContext.Current.Request.Params["ocena"];
            var komentar = HttpContext.Current.Request.Params["kom"];
            Int32.TryParse(ocena, out int ocenaa);

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/Comments.json");
            string path1 = Path.Combine(directory, "App_Data/Users.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista2 = JsonConvert.DeserializeObject<List<Comment>>(jsonData);

            var jsonData1 = System.IO.File.ReadAllText(path1);
            lista = JsonConvert.DeserializeObject<List<User>>(jsonData1);

            foreach(var u in lista)
            {
                if(u.Role==UserType.Visitor && u.Username==korIme && u.VisitorGroupTrainings!=null && u.Deleted==false)
                {
                    foreach(var t in u.VisitorGroupTrainings)
                    {
                        if(Fc.Id==t.Place.Id && t.DateTime<DateTime.Now)
                        {
                            lista2.Add(new Comment(korIme, Fc.Id, komentar, ocenaa, false));
                            using (StreamWriter streamWriter = new StreamWriter(path))
                            {
                                JsonSerializer serializer = new JsonSerializer();
                                serializer.Serialize(streamWriter, lista2);
                            }
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        [HttpGet, Route("api/fitnesscenters/SpisakSvih")]
        public List<Comment> SpisakSvih()
        {
            List<Comment> lista2 = new List<Comment>();

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/Comments.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista2 = JsonConvert.DeserializeObject<List<Comment>>(jsonData);

            if (lista2.Count == 0)
            {
                lista2 = new List<Comment>();
            }

            var lista = new List<Comment>();

            foreach (var i in lista2)
            {
                if (Fc.Id == i.FitnessCenter)
                {
                    lista.Add(i);
                }
            }
            return lista;
        }

        [HttpGet, Route("api/fitnesscenters/PraviVlasnik")]
        public bool PraviVlasnik()
        {
            var korIme = HttpContext.Current.Request.Params["ime"];
            List<User> lista = new List<User>();

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/Users.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista = JsonConvert.DeserializeObject<List<User>>(jsonData);

            foreach (var u in lista)
            {
                if (u.Username == korIme && u.FCOwner!=null)
                {
                    foreach(var broj in u.FCOwner)
                    {
                        if(Fc.Id==broj)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        [HttpPut, Route("api/fitnesscenters/Potvrda")]
        public bool Potvrda()
        {
            List<Comment> lista2 = new List<Comment>();
            var id = HttpContext.Current.Request.Params["id"];
            Int32.TryParse(id, out int idd);

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/Comments.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista2 = JsonConvert.DeserializeObject<List<Comment>>(jsonData);

            if(lista2.Count==0)
            {
                lista2 = new List<Comment>();
            }

            
            foreach (var i in lista2)
            {
                if (idd==i.Id)
                {
                    i.Available = true;
                    using (StreamWriter streamWriter = new StreamWriter(path))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(streamWriter, lista2);
                    }
                    return true;
                }
            }
            return false;
        }

        [HttpPut, Route("api/fitnesscenters/Odbij")]
        public bool Odbij()
        {
            List<Comment> lista2 = new List<Comment>();
            var id = HttpContext.Current.Request.Params["id"];
            Int32.TryParse(id, out int idd);

            string directory = HttpRuntime.AppDomainAppPath;
            string path = Path.Combine(directory, "App_Data/Comments.json");

            var jsonData = System.IO.File.ReadAllText(path);
            lista2 = JsonConvert.DeserializeObject<List<Comment>>(jsonData);

            if (lista2.Count == 0)
            {
                lista2 = new List<Comment>();
            }

            foreach (var i in lista2)
            {
                if (idd == i.Id)
                {
                    i.Available = false;
                    using (StreamWriter streamWriter = new StreamWriter(path))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(streamWriter, lista2);
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
