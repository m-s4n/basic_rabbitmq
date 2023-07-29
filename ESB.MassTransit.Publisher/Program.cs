using ESB.MassTransit.Shared.Messages;
using MassTransit;

string rabbitMQUri = "amqp://musti:gopher@localhost:5672";

string queueName = "kuyruk_1";

// masstransit config
// butun işlemler bus üzerinden yürütülür
IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory => {
    factory.Host(rabbitMQUri);
});

// mesajı göndereceğiz endpoint tasarlanır
ISendEndpoint sendEndpoint = await bus.GetSendEndpoint(new($"{rabbitMQUri}/{queueName}"));
 
while(true)
{
    Console.Write("Gönderilecek mesaj : ");
    string mesaj = Console.ReadLine() ?? "mesaj girilmedi";

    // mesaj gönderilir
    await sendEndpoint.Send<IMessage>(message: new ExampleMessage (){Text = mesaj });
}




