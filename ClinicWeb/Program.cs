using ClinicWeb.Data;
using ClinicWeb.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

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


//db switcher
/// 0: nick's db server -- Nick�a���A��
/// 1: localhost db server -- localhost������Ʈw���A��
/// 2: ispan classroom 201 db server cat -- 201�Ы�ip��m14��(nkj)
int db_source_switcher = 1;

var ConnString = "";
switch (db_source_switcher)
{
    case 0:
        //nick's db server
        var ClinicDb = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("ClinicSysWAN"))
        {
            DataSource = Server,
            UserID = User,
            Password = Pass
        };
        ConnString = ClinicDb.ConnectionString;
        break;
    case 1:
        //localhost db server
        ConnString = builder.Configuration.GetConnectionString("ClinicSys");
        break;
    case 2:
        //class 201 db server cat
        ConnString = builder.Configuration.GetConnectionString("ClinicSysClass201");
        break;
    default:
        break;
}

//Areas model DI
builder.Services.AddDbContext<ClinicWeb.Areas.Member.Models.ClinicSysContext>(options => options.UseSqlServer(ConnString));
builder.Services.AddDbContext<ClinicWeb.Areas.Drugs.Models.ClinicSysContext>(options => options.UseSqlServer(ConnString));
builder.Services.AddDbContext<ClinicWeb.Areas.Schedule.Models.ClinicSysContext>(options => options.UseSqlServer(ConnString));
builder.Services.AddDbContext<ClinicWeb.Areas.Appointment.Models.ClinicSysContext>(options => options.UseSqlServer(ConnString));
builder.Services.AddDbContext<ClinicSysContext>(options => options.UseSqlServer(ConnString));


//NewtonsoftJson
builder.Services.AddControllers().AddNewtonsoftJson();
//可在自訂元件取得Httpcontext
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    //未登入時會自動導到這個網址
    option.LoginPath = new PathString("/Employee/Main/Login");
    //看要不要加上access被拒絕的頁面 還是就是單純提醒
    option.AccessDeniedPath = new PathString("/Employee/Main/noAccess");
    option.ExpireTimeSpan = TimeSpan.FromMinutes(30);
});

////預設全部api都套用驗證 (不要的話在該 action加上[AllowAnonymous] 好比login
//builder.Services.AddMvc(options =>
//{
//    options.Filters.Add(new AuthorizeFilter());
//});

//允許angular4200
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AngularAccess",
                      policy =>
                      {
                          policy
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .WithOrigins("http://localhost:4200");
                      });
});

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

//允許angular4200
app.UseCors("AngularAccess");

//順序要一樣
app.UseCookiePolicy();
app.UseAuthentication();
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
