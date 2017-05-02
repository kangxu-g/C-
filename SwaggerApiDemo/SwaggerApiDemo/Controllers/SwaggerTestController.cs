using CommonLibs.Utilities;
using SwaggerApiDemo.Daos;
using SwaggerApiDemo.Daos.Impl;
using SwaggerApiDemo.Models;
using SwaggerApiDemo.BLL;
using SwaggerApiDemo.BLL.Impl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SwaggerApiDemo.Exceptions;
using Dev_Log4Net.Utilities;
using SwaggerApiDemo.Utils;

namespace SwaggerApiDemo.Controllers
{
    public class SwaggerTestController : ApiController
    {
        private ISwaggerTestBLL swaggerTestServiceImpl;

        public SwaggerTestController()
        {
            swaggerTestServiceImpl = new SwaggerTestBLLImpl();
        }



        /// <summary>
        /// 获取所有的net_test
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("swaggerTest/getNetTestAll")]
        public List<NetTest> getNetTestAll()
        {

            List<NetTest> netTests = null;
            try
            {
                netTests = swaggerTestServiceImpl.getNetTestAll();
            }
            catch (ServiceException e)
            {
                LogHelper.CurrentLogger.Error(e.Message, e);
            }

            return netTests;
        }

        /// <summary>
        /// 添加测试数据
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("swaggerTest/add")]
        public string add()
        {
            try
            {
                swaggerTestServiceImpl.add();
            }
            catch (ServiceException e)
            {
                LogHelper.CurrentLogger.Error(e.Message, e);
            }
            return "kangxu";
        }

    }
}