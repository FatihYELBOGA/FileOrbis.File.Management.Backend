using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using FileOrbis.File.Management.Backend.Configurations.Database;
using FileOrbis.File.Management.Backend.Services;
using FileOrbis.File.Management.Backend.Repositories;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog.Events;
using Serilog;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//  limit of the file
builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = int.MaxValue;
    x.MultipartHeadersLengthLimit = int.MaxValue;
});

builder.WebHost.UseKestrel(x => x.Limits.MaxRequestBodySize = null);

// swagger oauth2 configuration
builder.Services.AddSwaggerGen(
    options =>
    {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Description = "Standart Authorization header using the Bearer scheme",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });

        options.OperationFilter<SecurityRequirementsOperationFilter>();
    }
);

// logging configuration
string logging = builder.Configuration.GetSection("loggingTo").Value;
if (logging.Equals("file"))
{
    string loggingPath = builder.Configuration.GetSection("loggingPath").Value;
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File(loggingPath + "/log.txt",
            rollingInterval: RollingInterval.Day)
        .CreateLogger();

    builder.Services.AddLogging(loggingBuilder =>
        loggingBuilder.AddSerilog(dispose: true));
}
if (logging.Equals("database"))
{
    string connectionString = builder.Configuration.GetConnectionString("defaultConnection");

    // Log to database configuration
    Log.Logger = new LoggerConfiguration()
           .WriteTo.MSSqlServer(connectionString: connectionString,
                                 tableName: "Logs",
                                 autoCreateSqlTable: true) // Tabloyu otomatik oluþtur
           .CreateLogger();

    builder.Services.AddLogging(loggingBuilder =>
        loggingBuilder.AddSerilog(dispose: true));
}

// database configuration
builder.Services.AddDbContext<Database>(
    options => {
        options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"));
        options.ConfigureWarnings(warnings =>
            warnings.Ignore(CoreEventId.NavigationBaseIncludeIgnored));
    }
);

// enum converter and ignore cycle
builder.Services.AddControllers().AddJsonOptions(
    options =>
    {
        var enumConverter = new JsonStringEnumConverter();
        options.JsonSerializerOptions.Converters.Add(enumConverter);
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    }
);

// authentication configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(
        options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWTConfig:key").Value)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        }
    );

// dependencies or containers
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserRepository,  UserRepository>();
builder.Services.AddTransient<IFolderService, FolderService>();
builder.Services.AddTransient<IFolderRepository, FolderRepository>();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddTransient<IFileRepository, FileRepository>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IAuthRepository, AuthRepository>();

var app = builder.Build();

Database.Seed(
    app.Services.CreateScope().ServiceProvider.GetRequiredService<Database>(), 
    app.Services.CreateScope().ServiceProvider.GetRequiredService<IConfiguration>()
);

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

// cors configuration
app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
