using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApp.Models
{
    public class Comment
    {
        
        public Comment(string name, int fitnessCenter, string text, double grade, bool available)
        {
            Id = GenerateId();
            Name = name;
            FitnessCenter = fitnessCenter;
            Text = text;
            Grade = grade;
            Available = available;
        }

        public int Id { get; set; }
        public string Name { get; set; } 
        public int FitnessCenter { get; set; }
        public string Text { get; set; }
        public double Grade { get; set; } //od 1 do 5
        public bool Available { get; set; }

        private static int GenerateId()
        {
            return Math.Abs(Guid.NewGuid().GetHashCode());
        }
    }
}