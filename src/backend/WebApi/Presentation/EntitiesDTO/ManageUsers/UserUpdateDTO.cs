using System;


namespace ActivityScheduler.Presentation.EntitiesDTO
{
    public class UserUpdateDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}
