using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SwaggerApiDemo.Exceptions
{
    /// <summary>
    /// Service 异常
    /// </summary>
    public class ServiceException : Exception
    {

        public ServiceException()
            : base()
        {
        }

        public ServiceException(string message) 
            : base(message) 
        { 
        
        }
    }
}