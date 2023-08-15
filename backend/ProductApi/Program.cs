using ProductApi.Models;
using ProductApi.Services;
using ProductApi.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});

// Configure database settings
builder.Services.Configure<ProductDatabaseSettings>(
    builder.Configuration.GetSection("ProductDatabase"));
builder.Services.AddSingleton<ProductDbContext>();

builder.Services.AddIdentityCore<AppUser>(options => 
{
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddRoles<AppRole>()
    .AddUserStore<AppUserStore>()
    .AddRoleStore<AppRoleStore>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddSingleton<ProductsService>();

// Add JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["JwtDevelopment:Issuer"],
        ValidAudience = builder.Configuration["JwtDevelopment:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtDevelopment:Key"])),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.Configure<AppJwtConfig>(
    builder.Configuration.GetSection("JwtDevelopment"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // development error handler
    app.UseExceptionHandler("/error-dev");

    using (var serviceScope = app.Services.CreateScope())
    {
        var services = serviceScope.ServiceProvider;
        SeedData.CreateInitialRoles(services.GetRequiredService<RoleService>());
        SeedData.CreateInitialUsers(
            services.GetRequiredService<RoleService>(),
            services.GetRequiredService<UserService>());
        SeedData.CreateInitialProducts(services.GetRequiredService<ProductsService>());
    }
}
else
{
    // production error handler
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
