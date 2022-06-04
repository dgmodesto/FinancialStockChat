using FinancialChat.Consumer.Configuration;
using FinancialChat.Consumer.Consumers;
using FinancialChat.IoC;
using FinancialChatConsumer.NewFolder.Configuration;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var host = CreateHostBuilder(args).Build();
//host.StartAsync();

static IHostBuilder CreateHostBuilder(string[] args)
{
    var hostBuilder = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((context, builder) =>
        {
            builder.SetBasePath(Directory.GetCurrentDirectory());
        })
        .ConfigureServices((context, services) =>
        {
            //add your service registrations
            services.AddMassTransitConfiguration(context.Configuration);

            //Dependencies Injection
            services.BuildConfiguration();
        });

    return hostBuilder;
}

//var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
//{
//    cfg.ReceiveEndpoint(Environment.GetEnvironmentVariable("FINANCIAL_CHAT_STOCK_QUEUE_REQUEST"), e =>
//    {
//        e.PrefetchCount = 10;
//        e.UseMessageRetry(p => p.Interval(3, 100));
//        e.Consumer<FinancialChatStockConsumer>();
//    });
//});
//var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
//await busControl.StartAsync(source.Token);

Console.WriteLine("Waiting for new messages.");
host.Run();
while (true) ;
