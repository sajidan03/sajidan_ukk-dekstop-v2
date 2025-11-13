using System;
using System.Collections.Generic;
//using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace sajidan_ukk_dekstop
{
    internal class Connections
    {
        //public static string url = "Data Source=PONGO\\SQLEXPRESS;Initial Catalog=sajidan_ukk;Integrated Security=True;TrustServerCertificate=True";
        public static string url = "Server=sajidan-rifansyahsajidan03-b465.i.aivencloud.com;" +
                                   "Port=20922;" +
                                   "Database=sajidan_ukk-dekstop;" +
                                   "User ID=avnadmin;" +
                                   "Password=AVNS_kVVY-guj4jiDvlmF7aP;" +
                                   "SslMode=Required;";
        public static MySqlConnection koneksi;
        public static MySqlConnection Connect()
        {
            if (koneksi == null)
            {
                koneksi = new MySqlConnection(url);
            }
            return koneksi;
        }
    }
}
