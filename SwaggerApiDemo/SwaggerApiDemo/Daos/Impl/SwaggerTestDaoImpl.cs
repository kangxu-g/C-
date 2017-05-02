using CommonLibs.Utilities;
using Dev_Log4Net.Utilities;
using MySql.Data.MySqlClient;
using SwaggerApiDemo.Exceptions;
using SwaggerApiDemo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;

namespace SwaggerApiDemo.Daos.Impl
{
    public class SwaggerTestDaoImpl : ISwaggerTestDao
    {
        public List<NetTest> getNetTestAll()
        {
            List<NetTest> netTests = null;
            try
            {
                String sql = "select id,name from net_test";

                netTests = SqlHelper.ExecuteReader<NetTest>(System.Data.CommandType.Text, sql, null);
            }
            catch (HelperException e)
            {
                LogHelper.CurrentLogger.Error(e.Message, e);
                throw new DaoException("获取全部测试数据失败");
            }

            return netTests;
        }

        public void add()
        {
            try
            {

                Guid guid = Guid.NewGuid();
                string id = guid.ToString().Replace("-", "");

                string sql = "insert into net_test(id,name) values(@id,@name)";
                MySqlParameter[] param = new MySqlParameter[]{ 
                    new MySqlParameter("@id",id),
                    new MySqlParameter("@name","kangxu1")
                };
                SqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, sql, param);

            }
            catch (HelperException e)
            {
                LogHelper.CurrentLogger.Error(e.Message, e);
                throw new DaoException("添加数据失败");
            }


        }

    }
}