using SwaggerApiDemo.Daos;
using SwaggerApiDemo.Daos.Impl;
using SwaggerApiDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwaggerApiDemo.BLL
{
    public interface ISwaggerTestBLL
    {
        List<NetTest> getNetTestAll();

        void add();
    }
}
