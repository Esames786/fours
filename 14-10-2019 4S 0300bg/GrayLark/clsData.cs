using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrayLark
{
    public static class clsSqlConnection
    {
      
        static SqlConnection cn = null;
        public static SqlConnection getConnection()
        {
            if (cn == null)
            {
                cn = new SqlConnection(ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString);
                //if (cn.State != ConnectionState.Open)
                //{
                    cn.Open();
             //   } 
            }
            return cn;
        }
    }

    public static class clsDataLayer
    {
        static SqlCommand cmd = new SqlCommand();
        static SqlDataAdapter da = new SqlDataAdapter();

        public static int ExecuteQuery(string sql)
        {
            cmd.Connection = clsSqlConnection.getConnection();
            cmd.CommandText = sql;
            return cmd.ExecuteNonQuery();
        }

        public static DataTable RetreiveQuery(string sql)
        {
            cmd.Connection = clsSqlConnection.getConnection();
            cmd.CommandText = sql;
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }


    }


}

