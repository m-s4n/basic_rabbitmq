using MassTransit;
using ESB.MassTransit.WorkerService.Consumer.Consumers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // masstransit configurasyonu
        // consumer olduğu içi consumer'larıda tanımlanamalıyız
        services.AddMassTransit(config => 
        {
            // Masstransite consumer olacağını bildiriyoruz
            config.AddConsumer<ExampleMessageConsumer>();
            // Bu consumer hangi kuyruğu consume edecek aşağıda bildirilir
            config.UsingRabbitMq((context, _config) => 
            {
                _config.Host("amqp://musti:gopher@localhost:5672");

                // hangi consumer'in hangi kuyruğu consume edeceği bilgisi verilir.
                _config.ReceiveEndpoint("esb_kuyruk_1", e => e.ConfigureConsumer<ExampleMessageConsumer>(context));
            });
            
        });
    })
    .Build();

host.Run();
