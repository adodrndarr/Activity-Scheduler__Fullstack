using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;


namespace WebAPI.ActivityScheduler.Entities
{
    public class User : IdentityUser
    {
        public User() { }
        public User(string firstName, string lastName, string email, string password)
        {
            this.UserName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.PasswordHash = password;
        }


        public string LastName { get; set; }

        public IEnumerable<Activity> Activities { get; set; } = new List<Activity>();
    }
}
