using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace CommonLibs.Utilities
{
    //通用的数据访问辅助类:执行增删改查的方法
    public class DBHelper
    {

        public static string ConnString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            
        /// <summary>
        /// 通用的执行增删改的方法
        /// </summary>
        /// <param name="cmdtxt">sql语句</param>
        /// <returns>返回一个BOOL值，成功为true，失败为false</returns>
        public bool RunIUD(string cmdtxt)
        {
            bool result = false;
            MySqlConnection conn = new MySqlConnection(ConnString);
            try
            {
                MySqlCommand cmd = new MySqlCommand(cmdtxt,conn);
                cmd.Connection.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows >= 0)
                {
                    result = true;
                }
            }
            finally
            {
                conn.Close();
            }
            return result;
        }
        
        /// <summary>
        /// 通用的查询方法（DataSet)
        /// </summary>
        /// <param name="selectSQL">查询语句</param>
        /// <param name="tableName">虚拟表名</param>
        /// <returns>返回一个dataset</returns>
        public DataSet GetDataSet(string selectSQL,string tableName)
        {
            MySqlConnection conn = new MySqlConnection(ConnString);
            MySqlDataAdapter da = new MySqlDataAdapter(selectSQL, conn);
            DataSet ds = new DataSet();
            da.Fill(ds, tableName);
            conn.Close();
            return ds;
        }

        /// <summary>
        /// 通用查询（datareader）
        /// </summary>
        /// <param name="cmdtxt"></param>
        /// <returns></returns>
        public MySqlDataReader GetReader(string cmdtxt)
        {
            MySqlConnection conn = new MySqlConnection(ConnString);
            MySqlCommand cmd = new MySqlCommand(cmdtxt,conn);
            cmd.Connection.Open();
            MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }

        /// <summary>
        /// 是否有值
        /// </summary>
        /// <param name="cmdtxt"></param>
        /// <returns></returns>
         public bool Istrue(string cmdtxt)
        {
            bool result = false;
            MySqlConnection conn = new MySqlConnection(ConnString);
            MySqlCommand cmd = new MySqlCommand(cmdtxt,conn);
            cmd.Connection.Open();
            if(Convert.ToInt32(cmd.ExecuteScalar())>0)
            {
                result = true;
            }
            conn.Close();
            return result;
        }

         /// <summary>
         /// 判断是否执行
         /// </summary>
         /// <param name="cmdtxt"></param>
         /// <returns></returns>
         public bool IsExist(string cmdtxt)
         {
             bool result = false;
             MySqlConnection conn = new MySqlConnection(ConnString);
             MySqlDataAdapter da = new MySqlDataAdapter(cmdtxt,conn);
             DataSet ds = new DataSet();
             da.Fill(ds);
             if (ds.Tables[0].Rows.Count > 0)
             {
                 result = true;
             }
             conn.Close();
             return result;
         }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object GetOneData(string sql)
         {
             MySqlConnection conn = new MySqlConnection(ConnString);
             MySqlCommand cmd = new MySqlCommand(sql, conn);
             cmd.Connection.Open();
             object obj= cmd.ExecuteScalar();
             conn.Close();
             return obj;
             
         }

        /// <summary>
        /// 执行带参数的增删改SQL语句
        /// </summary>
        /// <param name="cmdtxt">sql语句</param>
        /// <param name="paramsValues">参数数组</param>
        /// <returns></returns>
        public bool ExecuteNonQuery(string cmdtxt,params object[] paramsValues)
        {
            bool rel = false;
            MySqlConnection conn = new MySqlConnection(ConnString);
            try
            {
                MySqlCommand cmd = new MySqlCommand(cmdtxt,conn);
                cmd.Connection.Open();
                cmd.Parameters.AddRange(paramsValues);
                int rows = cmd.ExecuteNonQuery();
                if (rows>0)
                {
                    rel = true;
                }
            }
            finally
            {
                conn.Close();
            }
            return rel;
        }

    }
    
}
