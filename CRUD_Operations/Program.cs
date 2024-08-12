using CRUD_Operations;
using CRUD_Operations.ActionFilter;
using CRUD_Operations.Authentication;
using CRUD_Operations.Data;
using CRUD_Operations.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("Config.json");

builder.Services.Configure<AttachmentOptions>(builder.Configuration.GetSection("Attachments"));

// Add services to the container.

builder.Services.AddControllers(options => {
    options.Filters.Add<LogActivityFilter>();
    //this is used as local ,but you can make it as global by enable it here
    //options.Filters.Add<LogSensitiveActionAttribute>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(conf =>
    conf.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"])
    );

var JwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();

builder.Services.AddSingleton(JwtOptions);

builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => 
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = JwtOptions.Isuuer,
        ValidateAudience = true,
        ValidAudience = JwtOptions.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SigningKey))
    };
});
    //.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic",null);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RateLimitingMiddleware>();

app.UseMiddleware<ProfilingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
