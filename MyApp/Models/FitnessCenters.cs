using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MyApp.Models
{
    public class FitnessCenters
    {
        public static List<FitnessCenter> FitnessCenterss { get; set; } = new List<FitnessCenter>();

        public  List<FitnessCenter> AllUsers()
        {
            /*string json = File.ReadAllText("C:/Users/ZBOOK/Desktop/Veb/VebProjekat/MyApp/App_Data/FitnessCenters.json");
            FitnessCenterss = JsonConvert.DeserializeObject<List<FitnessCenter>>(json);*/
            using (StreamReader r = new StreamReader("C:/Users/ZBOOK/Desktop/Veb/VebProjekat/MyApp/App_Data/FitnessCenters.json"))
            {
                string json = r.ReadToEnd();
                FitnessCenterss = JsonConvert.DeserializeObject<List<FitnessCenter>>(json);
            }
            FitnessCenterss.Sort((x,y)=>string.Compare(x.Name, y.Name));
            return FitnessCenterss;
        }
        



    }
    
}