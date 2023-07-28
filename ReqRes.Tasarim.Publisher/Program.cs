
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

// Bağlantı Oluşturma
ConnectionFactory factory = new();
factory.HostName = "localhost";
factory.Port = 5672;
factory.UserName = "musti";
factory.Password = "gopher";

//Bağlantı Aktifleştirme
using IConnection connection = factory.CreateConnection();

// Kanal Açma
using IModel channel = connection.CreateModel();

#region  tasarım
// request kuyrugu
string requestQueueName = "request_queue_1";
channel.QueueDeclare(queue: requestQueueName, durable: true, exclusive: false, autoDelete: false);

// response kuyruğu
string responseQueueName = "response_queue_1";
channel.QueueDeclare(queue: responseQueueName, durable: true, exclusive: false, autoDelete: false);

// correlation id
string correlationId = Guid.NewGuid().ToString();

#endregion

#region request mesajını oluşturma ve gönderme

IBasicProperties properties = channel.CreateBasicProperties();
properties.Persistent = true;
properties.CorrelationId = correlationId; // korelasyon id
properties.ReplyTo = responseQueueName;  // yanıt mesajı hangi kuyruktan gelecek

int i = 0;
while (i < 5)
{
    i++;
    Console.Write("Mesaj giriniz:");
    byte[] byteMesaj = Encoding.UTF8.GetBytes(Console.ReadLine() ?? "mesaj girilmedi");
    channel.BasicPublish(exchange: string.Empty, routingKey: requestQueueName, basicProperties: properties, body: byteMesaj);
}
#endregion

#region response kuyruğunu dinleme
// consomer oluşturma
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: responseQueueName, autoAck: false, consumer: consumer);

consumer.Received += (sender, e) =>
{
    //korelasyonu uyan mesajlar işlenir
    if(e.BasicProperties.CorrelationId == correlationId)
    {
        string mesaj = Encoding.UTF8.GetString(e.Body.Span);
        Console.WriteLine($"Response : {mesaj}");
        channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
    }
    else
    {
        Console.WriteLine("mesaj başka yerden");
        channel.BasicNack(deliveryTag: e.DeliveryTag, multiple: false, requeue: false);
    }
};

Console.Read();

#endregion
