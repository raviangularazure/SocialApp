using System;

namespace DatingApp.API.Models
{
    public class Photo
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool isMain { get; set; }
        public User User { get; set; } // These 2 properties are added so EF will create a cascade delete in migration
        public int UserId { get; set; }
        public string PublicId { get; set; }

    }
}