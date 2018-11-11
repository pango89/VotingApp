using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Worker.Data;
using Worker.Entities;

namespace worker
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var loggerFactory = new LoggerFactory()
                .AddConsole();

            var services = new ServiceCollection()
                .AddSingleton(loggerFactory)
                .AddLogging()
                .AddSingleton<IConfiguration>(config)
                .AddSingleton<IRedisStore, RedisStore>()
                .AddTransient<IVoteData, MySqlVoteData>()
                .AddSingleton<QueueWorker>()
                .AddDbContext<VoteContext>(builder => builder.UseMySQL(config.GetConnectionString("VoteData")));

            var provider = services.BuildServiceProvider();
            var worker = provider.GetService<QueueWorker>();
            worker.Start();
        }
    }
}
