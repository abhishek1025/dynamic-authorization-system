using System.Text;
using dynamic_authorization.api;
using dynamic_authorization.api.Authorization;
using dynamic_authorization.api.Utils.Middleware;
using dynamic_authorization.application.Services;
using dynamic_authorization.application.Services.Interfaces;
using dynamic_authorization.domain.Enums;
using dynamic_authorization.domain.Interfaces;
using dynamic_authorization.infrastructure.Data;
using dynamic_authorization.infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DapperDbContext>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserPermissionRepository, UserPermssionRepository>();

builder.Services.AddScoped<IPermissionServices, PermissionServices>();
builder.Services.AddScoped<IAuthServices, AuthServices>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor(); 

var secretKey = builder.Configuration.GetConnectionString("SecretKey");
var issuer = builder.Configuration.GetConnectionString("Issuer");


// Authentication
builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme
).AddJwtBearer(
    x =>
    {
      
        x.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.
                GetBytes(secretKey))
        };
        x.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerHandler>>();

                logger.LogError("Authentication failed: {Message}", context.Exception.Message);

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                
                var response = new ApiErrorResponse()
                {
                    Success = false,
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = "Invalid or expired token. Please login again."
                };

                return context.Response.WriteAsJsonAsync(response);
            }
        };
    }
);

builder.Services.AddAuthorization(options =>
{
    var resources = Enum.GetValues(typeof(ResourceEnum)).Cast<ResourceEnum>();
    var permissions = Enum.GetValues(typeof(PermissionOperationEnum)).Cast<PermissionOperationEnum>();

    foreach (var resource in resources)
    {
        foreach (var permission in permissions)
        {
            string policyName = $"{resource.ToString()}:{permission.ToString()}";
            
            options.AddPolicy(policyName, policy =>
            {
                policy.Requirements.Add(new PermissionRequirement(resource, permission));
            });
        }
    }
});

builder.Services.AddCors(
    cors => cors.AddPolicy(
        "AllowAll", policyBuilder =>
        {
            policyBuilder.WithOrigins("*");
            policyBuilder.AllowAnyHeader();
            policyBuilder.AllowAnyMethod();
        }
    )
);


var app = builder.Build();

app.UseCors("AllowAll"); // Apply CORS policy

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();