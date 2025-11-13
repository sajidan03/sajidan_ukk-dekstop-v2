using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sajidan_ukk_dekstop
{
    internal class Sessions
    {
        public static string username;
        public static int id;
        public static void start(string username, int id)
        {
            Sessions.username = username;
            Sessions.id = id;
        }
    }
}
