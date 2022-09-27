using DocumentStorage.BLL;
using DocumentStorage.DAL.Context;
using DocumentStorage.DAL.Repository;

using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using NLog;
using NLog.Web;

using NLogFastCore;
using NLogFastCore.Enums;

var logger = new LoggerRegistrar().AddInternal()
    .SetConnectionStringByName("LogerProvider")
    .SetDbProvider(DbProviderType.PostgreSQL)
    .AddDbLogger(LoggerType.ErrorLogger,"Log", NLog.LogLevel.Warn, NLog.LogLevel.Fatal).Setup()
    .GetCurrentClassLogger();

try
{
    #region ASPNETCORE_ENVIRONMENT
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    if(environment == null)
    {
        throw new Exception("Не установлено ASPNETCORE_ENVIRONMENT");
    }
    #endregion

    var builder = WebApplication.CreateBuilder(args);

    #region Logger 
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);
    builder.Host.UseNLog();
    #endregion

    #region Service Const from appsettings
    var maximumFileSize = new MaximumFileSize(builder.Configuration.GetValue<long>("MaximumFileSize"));
    builder.Services.AddSingleton<MaximumFileSize>(maximumFileSize);
    #endregion

    #region DB Settings
    var sqlConnectionString = builder.Configuration.GetConnectionString("DocumentPostgresSqlProvider");
    builder.Services.AddDbContext<AppPostgreContext>(options => options.UseNpgsql(sqlConnectionString));
    builder.Services.AddTransient<DocumentRepository>();
    #endregion

    #region Other
    builder.Services.Configure<KestrelServerOptions>(builder.Configuration.GetSection("Kestrel"));

    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddSession();

    builder.Services.AddControllers();
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddCors();
    #endregion

    #region Swagger
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "DocumentsStorage", Version = "v1.0.3" });
        c.IncludeXmlComments("DocumentsStorage.xml");
    });
    #endregion

    #region Secure
    builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://signinoidc.demo.ibzkh.ru/";

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

    #endregion

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if(app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseDeveloperExceptionPage();
        Console.WriteLine(environment);
    }

    app.UseHttpsRedirection();
    app.UseRouting();

    app.UseSession();
    app.UseCors(builder => builder.WithOrigins(app.Configuration.GetSection("Cors").GetChildren().Select(m => m.Value).ToArray())
                                  .AllowAnyMethod()
                                  .AllowAnyHeader());

    app.UseAuthorization();
    app.UseAuthentication();

    app.MapControllers();

    app.Run();
}
catch(Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}