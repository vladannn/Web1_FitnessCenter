using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApp.Models
{
    public class User
    {
        public User(string username, string password, string firstName, string lastName, GenderType gender, string email, DateTime dateTime, UserType role, List<GroupTraining> visitorGroupTrainings, List<GroupTraining> coachGroupTrainings, int fCEngagement, List<int> fCOwner, bool deleted)
        {
            Username = username;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            Email = email;
            DateTime = dateTime;
            Role = role;
            VisitorGroupTrainings = visitorGroupTrainings;
            CoachGroupTrainings = coachGroupTrainings;
            FCEngagement = fCEngagement;
            FCOwner = fCOwner;
            Deleted = deleted;
        }

        public User(string username, string password, string firstName, string lastName, GenderType gender, string email, DateTime dateTime, UserType role, bool deleted)
        {
            Username = username;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            Email = email;
            DateTime = dateTime;
            Role = role;
            Deleted = deleted;
        }

        public User() { }

        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GenderType Gender { get; set; }
        public string Email { get; set; }
        public DateTime DateTime { get; set; }
        public UserType Role { get; set; }
        public List<GroupTraining> VisitorGroupTrainings { get; set; }
        public List<GroupTraining> CoachGroupTrainings { get; set; }
        public int FCEngagement { get; set; }
        public List<int> FCOwner { get; set; }
        public bool Deleted { get; set; }
    }
}