using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
    public static class CommonHelper
    {
        public static string GetConnecttionStr()
        {
            var Buider = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            var cfg = Buider.Build();
            string ConnectionStr = cfg.GetConnectionString("default");
            return ConnectionStr;
        }

        public static bool IsValuable(this IEnumerable<object> value)
        {
            if (value == null || value.Count() == 0)
                return false;
            return true;
        }

        public static bool IsValuable(this object thisValue)
        {
            if (thisValue == null)
            {
                return false;
            }
            return thisValue.ToString() != "";
        }

        public static DateTime OtoDate(this object value)
        {
            try
            {
                return Convert.ToDateTime(value);
            }
            catch(Exception e)
            {
                throw new Exception(e.ToString());
            }
        }


        //Linq拓展方法

        public static IEnumerable<T> whereIF<T>(this IEnumerable<T> value,Boolean Condition,Func<T, bool> expression)
        {
            return Condition ? value.Where(expression) : value;
        }

        public static IQueryable<T> whereIF<T>(this IQueryable<T> value, Boolean Condition, Expression<Func<T, bool>> expression)
        {
            return Condition ? value.Where(expression) : value;
        }
    }
}
