﻿namespace TaskManager.Application.DTOs
{
    //shape of the data the client will send
    public class CreateTaskDTO
    {
        public string taskName { get; set; }
        public string taskDescription { get; set; }
        public DateTime taskDeadline { get; set; }
        public int usrId { get; set; }
        public List<string> Tags { get; set; } = new();
    }
}
