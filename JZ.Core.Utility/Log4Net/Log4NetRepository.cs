using log4net.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.Core.Utility.Log4Net
{
    public class Log4NetRepository
    {
        public static ILoggerRepository loggerRepository { get; set; }
    }
}
