using System;

namespace MiniTwit.Library.Database
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PwHash { get; set; }

        //can't remember how many-to-many relations are handled in EFC

    }
}