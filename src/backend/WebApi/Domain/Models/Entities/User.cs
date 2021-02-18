using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;


namespace ActivityScheduler.Domain.Entities 
{ 
    public class User : IdentityUser
    {
        public User() { }


        public string LastName { get; set; }
        public bool IsAdmin { get; set; }

        public IEnumerable<Activity> Activities { get; set; } = new List<Activity>();
    }
}
