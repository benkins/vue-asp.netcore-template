using System;

namespace Service.Models
{
    public class UserModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public DateTime DOB { get; set; }
    }
}
