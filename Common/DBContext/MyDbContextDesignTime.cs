using Common.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DBContext
{
    public class MyDbContextDesignTime : IDesignTimeDbContextFactory<MyDBContext>
    {
        public MyDBContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();
            string ConnectStr = CommonHelper.GetConnecttionStr();
            var severVersion = ServerVersion.AutoDetect(ConnectStr);
            optionsBuilder.UseMySql(ConnectStr, severVersion);
            return new MyDBContext(optionsBuilder.Options);
        }
    }
}
