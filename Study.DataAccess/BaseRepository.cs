using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace  Study.DataAccess
{
    public abstract class BaseRepository
    {
        protected DBUtil _dbDapper = DBUtil.CreateInstance();
        protected SqlConnection _conn = DBUtil.Connection();
    }
}
