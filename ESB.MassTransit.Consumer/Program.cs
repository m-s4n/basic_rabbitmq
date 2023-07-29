using MassTransit;
using ESB.MassTransit.Consumer.Consumer;

string rabbitMQUri = "amqp://musti:gopher@localhost:5672";

string queueName = "kuyruk_1";

// bus oluşturulur
IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory => {
    factory.Host(rabbitMQUri);
    // consumer kuyruğa gelenleri tüketecek
    // bu endpoint'e gelenleri receive edeceksin
    factory.ReceiveEndpoint(queueName: queueName, endpoint => 
    {
        // Consumer belirlenir
        endpoint.Consumer<ExampleMessageConsumer>();
    });
});

// consumer'ların çalışması için bus start edilir

await bus.StartAsync();

Console.Read(); // console ayakta kalması için

