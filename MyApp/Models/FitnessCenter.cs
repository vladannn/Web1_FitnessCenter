using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApp.Models
{
    public class FitnessCenter
    {
        public FitnessCenter(string name, Address address, int openingYear, string owner, int monMembership, int yMembership, int oneDayPrice, int groupTrPrice, int oneTrWCoach, bool deleted)
        {
            Id = GenerateId();
            Name = name;
            Address = address;
            OpeningYear = openingYear;
            Owner = owner;
            MonMembership = monMembership;
            YMembership = yMembership;
            OneDayPrice = oneDayPrice;
            GroupTrPrice = groupTrPrice;
            OneTrWCoach = oneTrWCoach;
            Deleted = deleted;
        }

        public FitnessCenter(int id, string name, Address address, int openingYear, string owner, int monMembership, int yMembership, int oneDayPrice, int groupTrPrice, int oneTrWCoach, bool deleted)
        {
            Id = id;
            Name = name;
            Address = address;
            OpeningYear = openingYear;
            Owner = owner;
            MonMembership = monMembership;
            YMembership = yMembership;
            OneDayPrice = oneDayPrice;
            GroupTrPrice = groupTrPrice;
            OneTrWCoach = oneTrWCoach;
            Deleted = deleted;
        }

        public FitnessCenter(FitnessCenter fc)
        {
            Id = GenerateId();
        }

        public FitnessCenter() { }

        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public int OpeningYear { get; set; }
        public string Owner { get; set; }
        public int MonMembership { get; set; }
        public int YMembership { get; set; }
        public int OneDayPrice { get; set; }
        public int GroupTrPrice { get; set; }
        public int OneTrWCoach { get; set; }
        public bool Deleted { get; set; }

        private static int GenerateId()
        {
            return Math.Abs(Guid.NewGuid().GetHashCode());
        }
    }
}