using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public enum EConnectionString
    {
        MainConnection
    }
    class ConnectionStringHelper
    {
        public static string GetConnectionString(EConnectionString ec)
        {
            return ConfigurationManager.ConnectionStrings[ec.ToString()]?.ConnectionString;
        }
    }
}
