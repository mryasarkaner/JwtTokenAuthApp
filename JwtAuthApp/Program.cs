using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Auth Bölümü
var issuer=builder.Configuration["JwtConfig:Issuer"];

var audience= builder.Configuration["JwtConfig:Audience"];
var secKey =builder.Configuration["JwtConfig:mKey"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options=>
{

options.TokenValidationParameters = new TokenValidationParameters{
ValidateIssuer=true,
ValidateAudience=true,
ValidateLifetime=true,
ValidateIssuerSigningKey=true,
ValidIssuer= issuer,
ValidAudience=audience,
IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secKey))

};

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
