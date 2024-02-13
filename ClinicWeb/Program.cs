using ClinicWeb.Data;
using ClinicWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var Server = builder.Configuration["ClinicSys:Server"];
var User = builder.Configuration["ClinicSys:USER"];
var Pass = builder.Configuration["ClinicSys:PASS"];

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

//�]�w�s���r��
builder.Services.AddDbContext<ClinicSysContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ClinicSys"))); //�s�����a�ݸ�Ʈw
//var ClinicDb = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("ClinicSysWAN")) //�s�����ճs�u��Ʈw
//{
//    ServerSPN = Server,
//    UserID = User,
//    Password = Pass
//};
//var ConnString = ClinicDb.ConnectionString;
//builder.Services.AddDbContext<ClinicSysContext>(options => options.UseSqlServer(ConnString));

//NewtonsoftJson
builder.Services.AddControllers().AddNewtonsoftJson();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();



app.Run();
