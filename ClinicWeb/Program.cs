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


//db switcher
/// 0: nick's db server -- Nick�a���A��
/// 1: localhost db server -- localhost������Ʈw���A��
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
