using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApp.Models
{
    public class GroupTraining
    {
        public GroupTraining(string name, TrainingType trainingType, FitnessCenter place, int duration, DateTime dateTime, int maxNumber, List<string> users, bool deleted)
        {
            Name = name;
            TrainingType = trainingType;
            Place = place;
            Duration = duration;
            DateTime = dateTime;
            MaxNumber = maxNumber;
            Id = GenerateId();
            Users = users;
            Deleted = deleted;
        }
        public GroupTraining()
        { }

        public string Name { get; set; }
        public TrainingType TrainingType { get; set; }
        public FitnessCenter Place { get; set; }// id od grupnog treninga koji predstavlja referencu na fitnes centar
        public int Duration { get; set; }
        public DateTime DateTime { get; set; }
        public int MaxNumber { get; set; }
        public int Id { get; set; }
        public List<string> Users { get; set; }
        public bool Deleted { get; set; }

        private static int GenerateId()
        {
            return Math.Abs(Guid.NewGuid().GetHashCode());
        }
    }
}