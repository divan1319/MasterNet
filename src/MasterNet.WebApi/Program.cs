using MasterNet.Application;
using MasterNet.Application.Interfaces;
using MasterNet.Infrastucture.Reports;
using MasterNet.Persistence;
using MasterNet.Persistence.Models;
using MasterNet.WebApi.Extension;
using MasterNet.WebApi.Middleware;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddOpenApi();
//builder.Services.AddScoped<IReportService,ReportService>();
builder.Services.AddScoped(typeof(IReportService<>),typeof(ReportService<>));
builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddIdentityCore<AppUser>(opt=>{
    opt.Password.RequireNonAlphanumeric = false;
    opt.User.RequireUniqueEmail = true;
}).AddRoles<IdentityRole>().AddEntityFrameworkStores<MasterNetDbContext>();

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

await app.SeedDataAuthentication();

app.MapControllers();
app.Run();
