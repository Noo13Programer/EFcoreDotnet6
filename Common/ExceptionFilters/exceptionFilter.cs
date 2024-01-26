using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ExceptionFilters
{
    public class exceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            string message = "";
            string developMessage = "";

            developMessage = context.Exception.StackTrace;
            message=context.Exception.Message;

            //初始化错误信息
            ObjectResult result = new ObjectResult(new ExceptionModel { code = 500, message = message, developMessage = developMessage });

            //区分环境


            //返回错误信息
            context.Result = result;

            //日志记录

            //是否处理错误信息，如果处理，则不会继续向下一个过滤器执行
            context.ExceptionHandled = true;

            return Task.CompletedTask;
        }

        private class ExceptionModel
        {
            public string message { get; set; }

            public string developMessage { get; set; }

            public int code { get; set; }
        }
    }
}
