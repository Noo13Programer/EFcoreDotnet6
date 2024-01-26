using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Filter.UnitOfWork
{
    [AttributeUsage(AttributeTargets.Class
 | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class UnitOfWork:Attribute
    {
        public Type[] DBContextTypes { get; init; }

        public UnitOfWork(Type[] dBContextTypes)
        {
            this.DBContextTypes = dBContextTypes;
        }
    }
}
