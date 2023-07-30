using MassTransit;
using ESB.MassTransit.ReqResPattern.Consumer.Consumers;

string rabbitMQUri = "amqp://musti:gopher@localhost:5672";
string requestQueueName = "esb_request_queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);
    // kuyruk ve kuyruğu dinleyen consumer tanımlanır
    factory.ReceiveEndpoint(requestQueueName,endPoint=>
    {
        endPoint.Consumer<RequestMessageConsumer>();
    });
});

await bus.StartAsync();

Console.Read();