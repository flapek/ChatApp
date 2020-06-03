using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Helpers
{
    public sealed class User
    {
        private static User instance = null;

        public List<string> Users { get; set; }
        private User()
        {
            Users = new List<string>();
        }

        public static User Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new User();
                }
                return instance;
            }
        }
    }
}
