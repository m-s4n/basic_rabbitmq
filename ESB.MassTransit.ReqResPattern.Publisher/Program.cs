using MassTransit;
using ESB.MassTransit.ReqResPattern.Shared.RequestResponseMessages;

string rabbitMQUri = "amqp://musti:gopher@localhost:5672";
string requestQueueName = "esb_request_queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);
});

await bus.StartAsync();

// req-res için request nesnesi oluşturulur türü RequestMessage ve dinleyeceği kuyruk
IRequestClient<RequestMessage> request = bus.CreateRequestClient<RequestMessage>(new Uri($"{rabbitMQUri}/{requestQueueName}"));

int i = 0;
while (true)
{
    i++;
    Console.Write("Mesaj giriniz :");
    string mesaj = Console.ReadLine() ?? "Mesaj girilmedi";
    RequestMessage requestMesaj = new() { MessageNo = i, Text = mesaj };
    // hem mesajı gönderir hemde response beklenir
    // GetResponse ile request işlemi gerçekleştirilir
    // RequestMessage gönderilir ResponseMessage alınır
    var response = await request.GetResponse<ResponseMessage>(message: requestMesaj);

    Console.WriteLine($"Response receive:{response.Message.Text}");
}