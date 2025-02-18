using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TourNest.Services.Flights.Book_Flights;
using Razorpay.Api;
using RazorpayIntegration;
using RazorpayIntegration.Services;
using System.Text;
using TourNest.Automapping;
using TourNest.Models;
using TourNest.Models.Chatbot;
using TourNest.Services.JWT;
using TourNest.Services.Mail;
using TourNest.Services.MemoryCache;
using TourNest.Services.Otp;
using TourNest.Services.User.ProfilePicture;
using TourNest.Services.Flights.Search_Flights_Locations;
using TourNest.Services.Flights.Search_Flights;
using TourNest.Services.Location;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<RazorpayClient>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    string key = configuration["Razorpay:Key"];
    string secret = configuration["Razorpay:Secret"];
    return new RazorpayClient(key, secret);
});

builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<InstagramScraperService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddSingleton<Random>();
builder.Services.AddScoped<IEmailServices, EmailServices>();
builder.Services.AddScoped<IOtpServices, OtpServices>();
builder.Services.AddScoped<IMemoryCacheService, MemoryCacheService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<JwtHelper>();
builder.Services.AddHttpClient();
builder.Services.Configure<RapidApiSettings>(builder.Configuration.GetSection("RapidApiSettings"));
builder.Services.AddHttpClient<SearchFlightService>();
builder.Services.AddScoped<FlightBookingService>();
builder.Services.AddScoped<FlightService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<RazorpayService>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var key = config["Razorpay:Key"];
    var secret = config["Razorpay:Secret"];
    return new RazorpayService(key, secret);
});

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Register DBContext 
var provider = builder.Services.BuildServiceProvider();
var config = provider.GetRequiredService<IConfiguration>();
builder.Services.AddDbContext<TourNestContext>(item => item.UseSqlServer(config.GetConnectionString("dbcs")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
    };
});


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "JWT API", Version = "v1" });

    // Add JWT Authentication to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid JWT token in the text input below.\n\nExample: `Bearer eyJhb...`"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
