using ESB.MassTransit.WorkerService.Publisher.Services;
using MassTransit;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(config => 
        {
            // message broker ile ilgili yapılanma verilir
            config.UsingRabbitMq((context, _config) => 
            {
                _config.Host("amqp://musti:gopher@localhost:5672");
            });
        });

        // ilgisi servis worker servise tanımlanır
        services.AddHostedService<PublishMessageService>(provider => {
            // IoC container'dan manual alınıp constructor'a gönderilir
            using IServiceScope scope = provider.CreateScope();
            IPublishEndpoint publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
            return new(publishEndpoint);
        });
    })
    .Build();

host.Run();

//mesajı gönderecek servis oluşturulur
// background servis olarak çalışacak servis oluşturacağız


