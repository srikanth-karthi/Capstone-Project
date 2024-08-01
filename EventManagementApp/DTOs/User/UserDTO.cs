﻿using System.ComponentModel.DataAnnotations;

namespace EventManagementApp.DTOs.User
{
    public class UserDTO
    {

        public int UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string ProfileUrl { get; set; }
    }
}
