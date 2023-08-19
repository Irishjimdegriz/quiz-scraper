using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Quartz.Impl;
using Quartz.Logging;
using Quartz;
using QuizDb.Module.Services;
using QuizDb.Module.Services.Interfaces;
using QuizDb.Scraper;
using QuizDB.DAL;
using QuizDB.DAL.Services;
using QuizDB.DAL.Services.Interfaces;
using Microsoft.Extensions.Hosting;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();

//LogProvider.SetCurrentLogProvider(new ConsoleLogProvider()); // Configure logging

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddTransient(provider => new DalStartup(configuration))
            .AddScoped<ApplicationDbContext>()
            .AddSingleton<IQuizDbService, QuizDbService>()
            .AddSingleton<IDatabaseService, DatabaseService>()
            .AddSingleton<ITournamentParser, TournamentParser>()
            .AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")),
                ServiceLifetime.Transient,
                ServiceLifetime.Transient)
            .AddQuartz(options =>
            {
                var jobKey = JobKey.Create(nameof(ScraperJob));

                options
                    .AddJob<ScraperJob>(jobKey)
                    .AddTrigger(trigger =>
                        trigger
                            .ForJob(jobKey)
                            .WithDailyTimeIntervalSchedule(x => x.StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(3, 0))));
            })
            .AddQuartzHostedService()
            .AddSingleton<ScraperJob>();
    })
    .Build();

await host.RunAsync();