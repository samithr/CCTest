using CCTest.API.Middlewares;
using CCTest.API.SignalRHubs;
using CCTest.Database.Common;
using CCTest.Repository.Common;
using CCTest.Repository.Contracts;
using CCTest.Repository.Repos;
using CCTest.Service.Contracts;
using CCTest.Service.Services;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

// Used PostgreSQL for the database
string? connectionString = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("POSTGRESQLCONNSTR_TestDbConnection")) ? builder.Configuration.GetConnectionString("CCTestDbConnection") : Environment.GetEnvironmentVariable("POSTGRESQLCONNSTR_TestDbConnection");
builder.Services.AddDbContext<CCTestDbContext>(options => options.UseNpgsql(connectionString));

#region Register hangfire

builder.Services.AddHangfire(config => config.UsePostgreSqlStorage(connectionString));
builder.Services.AddHangfireServer();
GlobalConfiguration.Configuration.UsePostgreSqlStorage(connectionString);

#endregion

builder.Services.AddSignalR();

//builder.Services.AddScoped<IEntityMapper, EntityMapper>();

#region Repository DI

builder.Services.AddScoped<IAgentRepository, AgentRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();

#endregion

#region Services DI

builder.Services.AddScoped<IAgentService, AgentService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ISupportService, SupportService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IAgentService, AgentService>();
builder.Services.AddScoped<ISessionManagerService, SessionManagerService>();

#endregion

var app = builder.Build();

// Monitor session queue with hangfire recuring job
RecurringJob.AddOrUpdate<ISessionManagerService>("Manage session queue", o => o.MonitorAndProcessChatQueue(), Cron.Minutely);

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseHttpsRedirection();
app.UseMiddleware(typeof(ExceptionHandlingMiddleware));
app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(ep =>
{
    // Register SignalR hub
    ep.MapHub<ChatManagementHub>("/chatHub");
});

app.Run();
