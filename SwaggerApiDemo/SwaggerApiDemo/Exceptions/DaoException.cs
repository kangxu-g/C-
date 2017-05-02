using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SwaggerApiDemo.Exceptions
{
    /// <summary>
    /// Dao 异常
    /// </summary>
    public class DaoException : Exception
    {
        public DaoException()
            : base()
        {

        }

        public DaoException(String message)
            : base(message)
        {

        }

    }
}