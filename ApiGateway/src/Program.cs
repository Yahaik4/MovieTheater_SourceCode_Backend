using ApiGateway.Helper;
using ApiGateway.ServiceConnector.AuthenticationService;
using ApiGateway.ServiceConnector.CinemaService;
using ApiGateway.ServiceConnector.MovieService;
using ApiGateway.ServiceConnector.OTPService;
using ApiGateway.ServiceConnector.PaymentService;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:AccessTokenKey"]))
        };
    });

builder.Services.AddControllers();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireAssertion(ctx =>
        {
            var role = ctx.User.FindFirst(ClaimTypes.Role)?.Value;
            return role == "admin"; 
        }));


    options.AddPolicy("OperationsManagerOnly", policy =>
        policy.RequireAssertion(ctx =>
        {
            var role = ctx.User.FindFirst(ClaimTypes.Role)?.Value;
            var position = ctx.User.FindFirst("position")?.Value;

            // (2) Admin override quyền
            if (role == "admin")
                return true;

            return role == "staff" && position == "operations_manager";
        }));

    options.AddPolicy("CinemaManagerOrHigher", policy =>
        policy.RequireAssertion(ctx =>
        {
            var role = ctx.User.FindFirst(ClaimTypes.Role)?.Value;
            var position = ctx.User.FindFirst("position")?.Value;

            if (role == "admin")
                return true;

            if (role != "staff" || position == null)
                return false;

            return position == "cinema_manager" || position == "operations_manager";
        }));

    options.AddPolicy("StaffOrHigher", policy =>
    policy.RequireAssertion(ctx =>
    {
        var role = ctx.User.FindFirst(ClaimTypes.Role)?.Value;
        var position = ctx.User.FindFirst("position")?.Value;

        if (role == "admin")
            return true;

        if (role != "staff")
            return false;

        if (string.IsNullOrWhiteSpace(position))
            return false;

        return position == "cinema_manager" 
            || position == "operations_manager"
            || position == "staff";
    }));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "SFA API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                .AllowCredentials()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddScoped<AuthenticationServiceConnector>();
builder.Services.AddScoped<CinemaServiceConnector>();
builder.Services.AddScoped<MovieServiceConnector>();
builder.Services.AddScoped<OTPServiceConnector>();
builder.Services.AddScoped<PaymentServiceConnector>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
