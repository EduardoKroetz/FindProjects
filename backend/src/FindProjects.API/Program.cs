using FindProjects.Application.DTOs;
using FindProjects.Application.Services;
using FindProjects.Application.Services.Interfaces;
using FindProjects.Core.Entities;
using FindProjects.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            var result = ResultDto<object>.BadResult(errors);

            return new BadRequestObjectResult(result);
        };
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<FindProjectsDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
});

var sqlServerConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<FindProjectsDbContext>(options => 
    options.UseSqlServer(
        sqlServerConnectionString, 
        optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(FindProjectsDbContext).Assembly.GetName().Name)));

builder.Services.AddLogging();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();    
app.UseAuthorization();

app.MapControllers();

app.Run();