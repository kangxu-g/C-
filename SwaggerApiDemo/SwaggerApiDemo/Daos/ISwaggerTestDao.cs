using SwaggerApiDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwaggerApiDemo.Daos
{
    public interface ISwaggerTestDao
    {
        /// <summary>
        /// 获取所有测试数据
        /// </summary>
        /// <returns></returns>
        List<NetTest> getNetTestAll();

        void add();
    }
}
