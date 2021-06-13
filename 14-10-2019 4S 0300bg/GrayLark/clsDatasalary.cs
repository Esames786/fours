using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reddot_Express_Inventory
{
    public static class clsSqlConnection2
    {
      
        static SqlConnection cn = null;
        public static SqlConnection getConnection()
        {
            if (cn == null)
            {
       //  String c1 = "Data Source=DESKTOP-U44SVM0;Initial Catalog=deliziasalary;User ID=sa;Password=sa123";
         String c1 = "Data Source=HP-PC\\SQLEXPRESS;Initial Catalog=deliziasalary;User ID=sa;Password=sa9";
             cn = new SqlConnection(c1);
             //if (cn.State != ConnectionState.Open)
             //{
                 cn.Open();
             //   } 
            }
            return cn;
        }
    }

    public static class clsDataLayer2
    {
        static SqlCommand cmd = new SqlCommand();
        static SqlDataAdapter da = new SqlDataAdapter();

        public static int ExecuteQuery(string sql)
        {
            cmd.Connection = clsSqlConnection2.getConnection();
            cmd.CommandText = sql;
            return cmd.ExecuteNonQuery();
        }

        public static DataTable RetreiveQuery(string sql)
        {
            cmd.Connection = clsSqlConnection2.getConnection();
            cmd.CommandText = sql;
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }


    }


}

