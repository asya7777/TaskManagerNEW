﻿namespace TaskManager.Application.DTOs
{
    public record LoginDTO
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}
