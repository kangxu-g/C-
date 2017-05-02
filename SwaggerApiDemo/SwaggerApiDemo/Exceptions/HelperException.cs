using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SwaggerApiDemo.Exceptions
{
    /// <summary>
    /// Helper 异常
    /// </summary>
    public class HelperException : Exception
    {

        public HelperException()
            : base()
        {
        }

        public HelperException(string message)
            : base(message)
        { 
        }
    }
}