using LeaveManagement.Core.Domain.IdentityEntities;
using LeaveManagement.Infrastructure.DatabaseContext;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LeaveManagement.Core.Domain.RepositoryContracts;
using LeaveManagement.Core.ServiceContracts;
using LeaveManagement.Core.Services;
using LeaveManagement.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ILeaveTypeService, LeaveTypeService>();
builder.Services.AddScoped<ILeaveService, LeaveService>();

builder.Services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
builder.Services.AddScoped<ILeaveRepository, LeaveRepository>();

builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

builder.Services
    .AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireDigit = true;
        options.Password.RequiredUniqueChars = 5;    //Eg: Abcbc1!#
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
    .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

builder.Services
    .AddAuthorization(options =>
    {
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()             //User must be authenticated for all action method
        .Build();
    });

builder.Services
    .ConfigureApplicationCookie(options =>
    {
        options.LoginPath = "/Account/Login";
    });


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();    //Read identity cookie
app.UseAuthorization();     //Validate access permission of the user
app.MapControllers();
app.Run();

