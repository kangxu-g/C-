using Dev_Log4Net.Utilities;
using SwaggerApiDemo.BLL;
using SwaggerApiDemo.Daos;
using SwaggerApiDemo.Daos.Impl;
using SwaggerApiDemo.Exceptions;
using SwaggerApiDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

namespace SwaggerApiDemo.BLL.Impl
{
    public class SwaggerTestBLLImpl : ISwaggerTestBLL
    {

        private ISwaggerTestDao swaggerTestDaoImpl;

        public SwaggerTestBLLImpl()
        {
            swaggerTestDaoImpl = new SwaggerTestDaoImpl();
        }

        public List<NetTest> getNetTestAll()
        {
            List<NetTest> netTests = null;
            try
            {
                netTests = swaggerTestDaoImpl.getNetTestAll();
            }
            catch (DaoException e)
            {
                LogHelper.CurrentLogger.Error(e.Message, e);
                throw new ServiceException("获取全部测试数据失败");
            }

            return netTests;
        }

        public void add()
        {
            try
            {
                // 事务处理
                using (TransactionScope ts = new TransactionScope())
                {
                    swaggerTestDaoImpl.add();
                    // 事务提交
                    ts.Complete();
                }
            }
            catch (DaoException e)
            {
                LogHelper.CurrentLogger.Error(e.Message, e);
                throw new ServiceException("添加数据失败");
            }
        }
    }
}