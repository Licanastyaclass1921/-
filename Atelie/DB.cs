using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
namespace Atelie
{
    public class DB
    {
        SqlConnection connect = new
            SqlConnection(ConfigurationManager.ConnectionStrings["Atelie.Properties.Settings.AtelieConnectionString"].ConnectionString);
        public void ConnectOpen()
        {
            connect.Open();
        }
        public SqlConnection GetConnect()
        {
            return connect;
        }
    }
}
