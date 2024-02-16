using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Models
{
    public partial class ClinicSysContext : DbContext
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        IConfigurationRoot configuration = new ConfigurationBuilder()
        //            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) //找到根目錄
        //            .AddJsonFile("appsettings.json")  //找到設定檔
        //            .Build();
        //        optionsBuilder.UseSqlServer(configuration.GetConnectionString("ClinicSys"));  //找到連接字串的某個Key
        //    }
        //}
    }
}
