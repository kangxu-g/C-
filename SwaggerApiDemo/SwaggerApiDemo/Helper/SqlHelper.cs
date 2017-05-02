using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using MySql.Data.MySqlClient;
using Dev_Log4Net.Utilities;
using SwaggerApiDemo.Exceptions;
using SwaggerApiDemo.Utils;

namespace CommonLibs.Utilities
{
    /// <summary>
    /// 辅助类
    /// </summary>
    public class SqlHelper
    {
        /// <summary>
        /// 连接字符串
        /// Server=121.42.33.174;Port=3306;Database=bysj_king_music;Uid=root;Pwd=kangxu;Max Pool Size=512;
        /// </summary>
        public static string ConnString = DESUtil.DesDecrypt(System.Configuration.ConfigurationManager.ConnectionStrings["connStr"].ToString());

        /// <summary>
        /// 执行insert、update和delete等sql语句的ADO.NET操作
        /// </summary>
        /// <param name="cmdType">命令的类型</param>
        /// <param name="cmdText">执行的SQL脚本或存储过程的名称</param>
        /// <param name="cmdPrams">执行的SQL脚本的参数列表，没有则为NULL</param>
        /// <returns>返回的成功与否</returns>
        public static bool ExecuteNonQuery(CommandType cmdType, string cmdText, MySqlParameter[] cmdPrams)
        {

            bool flag = false;

            // 创建连接对象，通往底层的物理数据库通道
            MySqlConnection conn = null;

            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = ConnString;
                // 打开连接通道
                conn.Open();
                // 命令的发布和处理
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = cmdType;
                cmd.CommandText = cmdText;
                // 添加参数列表
                cmd.Parameters.Clear();
                if (cmdPrams != null)
                {
                    foreach (MySqlParameter parm in cmdPrams)
                    {
                        cmd.Parameters.Add(parm);
                    }
                }
                // 执行命令
                int i = cmd.ExecuteNonQuery();  // insert  / update / delete 等命令
                // 执行结果
                flag = i > 0 ? true : false;

            }
            catch (Exception ex)
            {
                LogHelper.CurrentLogger.Error(ex.Message, ex);
                throw new HelperException("[ExecuteNonQuery] failure");
                //Console.WriteLine("数据库连接异常，异常信息：{0}", ex.Message);
                //return false;
            }
            finally
            {
                // 关闭连接通道
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return flag;
        }

        /// <summary>
        /// 执行包含count(*) \ max(..) 等 select 语句
        /// </summary>
        /// <param name="cmdType">命令的类型</param>
        /// <param name="cmdText">执行的SQL脚本或存储过程的名称</param>
        /// <param name="cmdPrams">执行的SQL脚本的参数列表，没有则为NULL</param>
        /// <returns>返回的是第一行第一列的值</returns>
        public static object ExecuteScalar(CommandType cmdType, string cmdText, MySqlParameter[] cmdPrams)
        {
            object obj = null;

            // 创建连接对象，通往底层的物理数据库通道
            MySqlConnection conn = null;
            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = ConnString;
                // 打开连接通道
                conn.Open();
                // 命令的发布和处理
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = cmdType;
                cmd.CommandText = cmdText;
                // 添加参数列表
                cmd.Parameters.Clear();
                if (cmdPrams != null)
                {
                    foreach (MySqlParameter parm in cmdPrams)
                    {
                        cmd.Parameters.Add(parm);
                    }
                }
                // 执行命令
                obj = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                LogHelper.CurrentLogger.Error(ex.Message, ex);
                throw new HelperException("[ExecuteScalar] failure");
                //Console.WriteLine("数据库连接异常，异常信息：{0}", ex.Message);
                //return null;
            }
            finally
            {
                // 关闭连接通道
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return obj;
        }

        /// <summary>
        /// 执行select 语句
        /// </summary>
        /// <param name="cmdType">命令的类型</param>
        /// <param name="cmdText">执行的SQL脚本或存储过程的名称</param>
        /// <param name="cmdPrams">执行的SQL脚本的参数列表，没有则为NULL</param>
        /// <returns>返回的数据集合</returns>
        public static List<T> ExecuteReader<T>(CommandType cmdType, string cmdText, MySqlParameter[] cmdPrams) where T : new()
        {
            // 定义一个返回结果
            List<T> result = new List<T>();

            // 创建连接对象，通往底层的物理数据库通道
            MySqlConnection conn = null;
            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = ConnString;

                // 打开连接通道
                conn.Open();
                // 命令的发布和处理
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = cmdType;
                cmd.CommandText = cmdText;
                // 添加参数列表
                cmd.Parameters.Clear();
                if (cmdPrams != null)
                {
                    foreach (MySqlParameter parm in cmdPrams)
                    {
                        cmd.Parameters.Add(parm);
                    }
                }
                // 执行命令
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    #region 构建自动映射器（使用的技术包含泛型和反射）

                    // 实例化泛型类
                    T t = new T();
                    // 获取该泛型对象的属性
                    PropertyInfo[] properties = t.GetType().GetProperties();
                    // 遍历每个属性值
                    foreach (PropertyInfo pi in properties)
                    {
                        // 判断只读器reader中是否包含同名属性值，有则取其值，否则为NULL
                        object val = null;
                        try { val = reader[pi.Name]; }
                        catch { }
                        // 将reader中的属性值 赋值给泛型对象的属性中
                        if (pi.CanWrite && val != null && val != DBNull.Value)
                        {
                            pi.SetValue(t, val, null);
                        }
                    }
                    #endregion

                    result.Add(t);
                }
                reader.Close();
                // 返回
                //return result;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("数据库连接异常，异常信息：{0}", ex.Message);
                //return null;
                LogHelper.CurrentLogger.Error(ex.Message, ex);
                throw new HelperException("[ExecuteReader] failure");
            }
            finally
            {
                // 关闭连接通道
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// 执行分页存储过程pagequery
        /// </summary>
        /// <typeparam name="T">泛型实体</typeparam>
        /// <param name="sqlTable">表名</param>
        /// <param name="sqlColumns">表列</param>
        /// <param name="sqlWhere">表条件</param>
        /// <param name="sqlSort">表排序</param>
        /// <param name="pageIndex">查询的页索引（从0开始）</param>
        /// <param name="pageSize">每页显示的数量</param>
        /// <returns>分页对象</returns>
        public static Pager<T> ExecutePager<T>(string sqlTable, string sqlColumns, string sqlWhere, string sqlSort, int pageIndex, int pageSize) where T : new()
        {
            // 初始化一个返回结果
            Pager<T> result = new Pager<T>();
            result.rows = new List<T>();
            result.total = 0;

            // 创建连接对象，通往底层的物理数据库通道
            MySqlConnection conn = null;

            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = ConnString;

                // 打开连接通道
                conn.Open();
                // 基于连接通道创建命令
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_paged_data";
                // 添加命令参数
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@sqlTable", sqlTable);
                cmd.Parameters.AddWithValue("@sqlColumns", sqlColumns);
                cmd.Parameters.AddWithValue("@sqlWhere", sqlWhere);
                cmd.Parameters.AddWithValue("@sqlSort", sqlSort);
                cmd.Parameters.AddWithValue("@pageIndex", pageIndex);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.Add(new MySqlParameter("@rowTotal", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                });
                // 执行命令
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    #region 关系列 自动映射到 对象属性

                    // 实例化泛型类
                    T t = new T();
                    // 获取该泛型对象的属性
                    PropertyInfo[] properties = t.GetType().GetProperties();
                    // 遍历每个属性值
                    foreach (PropertyInfo pi in properties)
                    {
                        // 判断只读器reader中是否包含同名属性值，有则取其值，否则为NULL
                        object val = null;
                        try { val = reader[pi.Name]; }
                        catch { }
                        // 将reader中的属性值 赋值给泛型对象的属性中
                        if (pi.CanWrite && val != null && val != DBNull.Value)
                        {
                            pi.SetValue(t, val, null);
                        }
                    }
                    #endregion

                    result.rows.Add(t);
                }
                reader.NextResult();
                result.total = Convert.ToInt32(cmd.Parameters["@rowTotal"].Value);
                reader.Close();
                // 返回结果
                //return result;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("数据库连接异常，异常信息：{0}", ex.Message);
                //return result;
                LogHelper.CurrentLogger.Error(ex.Message, ex);
                throw new HelperException("[ExecutePager] failure");
            }
            finally
            {
                // 关闭连接通道
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return result;
        }

        /* 上述分页方法的存储过程如下
        create procedure sp_paged_data
        (
            @sqlTable nvarchar(200),              ----待查询表名
            @sqlColumns nvarchar(500) ,    ----待显示字段
            @sqlWhere nvarchar(1000) ,  ----查询条件,不需where 
            @sqlSort nvarchar(500) ,        ----排序字段，不需order by 
            @pageIndex int,                       ----当前页
            @pageSize int,                        ----每页显示的记录数
            @rowTotal int = 1 output	  ----返回总记录数
        )
        as
        begin
            set nocount on;
            -- 获取记录总数
            declare @sqlcount nvarchar(1000)  ;
            set @sqlcount = N' select @rowTotal=count(*) from '+@sqlTable +' where 1=1 ' +@sqlWhere;
            exec sp_executesql @sqlcount,N'@rowTotal int out ',@rowTotal out ;
            -- 返回数据查询
            declare @sqldata nvarchar(1000) ;
            if( @pageIndex =0)
            begin
                set @sqldata=' select top '+cast(@pageSize as varchar(10))+' '+@sqlColumns + ' from '+@sqlTable +' where 1=1 '+ @sqlWhere+' order by ' +@sqlSort;
            end
            else
            begin
                set @sqldata=' select '+ @sqlColumns + ' from (select *,Row_number() over(order by '+ @sqlSort +' ) as RN from '+ @sqlTable +' where 1=1 '+ @sqlWhere+') as TR where RN>'+ cast(@pageSize*@pageIndex as varchar(20))+' and RN<'+ cast((@pageSize*(@pageIndex+1)+1) as varchar(20));
            end
            exec sp_executesql @sqldata ;
        end
        */
    }

    /// <summary>
    /// 分页实体类
    /// </summary>
    /// <typeparam name="T">泛型实体</typeparam>
    public class Pager<T>
    {
        /// <summary>
        /// 当前索引页的数据集合
        /// </summary>
        public List<T> rows;
        /// <summary>
        /// 总的数据数量
        /// </summary>
        public Int32 total;
    }
}
