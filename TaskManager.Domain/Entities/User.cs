namespace TaskManager.Domain.Entities
{
    public class User
    {
        public int usrId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public byte[] passwordHash { get; set; } //hashed password
        public byte[] passwordSalt { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
        public bool verified { get; set; } = false; //default value is false, user needs to verify their email
        public DateTime? verifiedAt { get; set; }
        public string? verificationToken { get; set; }
        public DateTime? verificationTokenExpiration{ get; set; }
        public string userRole { get; set; } 
        public ICollection<Task> Tasks { get; set; } = new List<Task>();//bir user ın birden fazla task ı olabilir
    }
}
