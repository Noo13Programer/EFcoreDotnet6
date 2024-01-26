using Common.DBContext;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Common.Filter.UnitOfWork
{
    public class UnitOfWorkFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var ContrillerDesc = context.ActionDescriptor as ControllerActionDescriptor;
            if (ContrillerDesc != null)
            {
                await next();
                return;
            }
            UnitOfWork methodsInfo= ContrillerDesc.MethodInfo.GetCustomAttribute<UnitOfWork>();
            if (methodsInfo != null)
            {
                await next();
                return;
            }
            using (TransactionScope trans=new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                List<MyDBContext> dBContexts=new List<MyDBContext>();
                foreach(var dbctx in methodsInfo.DBContextTypes)
                {
                    //获取实例方法
                    var sp = context.HttpContext.RequestServices;
                    var myctx = (MyDBContext)sp.GetRequiredService<MyDBContext>();
                    dBContexts.Add(myctx);
                }
                var res=await next();
                if(res.Exception != null)
                {
                    throw new Exception(res.Exception.ToString());
                }
                else
                {
                    foreach(var dbctx in dBContexts)
                    {
                        await dbctx.SaveChangesAsync();
                    }
                }
                trans.Complete();
            }
        }
    }
}
