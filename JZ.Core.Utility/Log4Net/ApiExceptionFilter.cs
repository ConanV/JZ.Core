using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.Core.Utility.Log4Net
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            LogHelper.Error("API异常", context.Exception);

            base.OnException(context);

            //context.ExceptionHandled = true;
        }
    }
}
