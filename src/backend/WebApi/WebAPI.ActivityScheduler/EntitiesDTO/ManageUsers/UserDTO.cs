﻿using System;


namespace WebAPI.ActivityScheduler.EntitiesDTO.ManageUsers
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
