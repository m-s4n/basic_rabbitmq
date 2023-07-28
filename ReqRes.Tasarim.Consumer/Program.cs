
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
// response kuyrugu
string receiverQueueName = "request_queue_1";
channel.QueueDeclare(queue: receiverQueueName, durable: true, exclusive: false, autoDelete: false);

// tüketici
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: receiverQueueName,autoAck: false, consumer: consumer );


// mesaj okuma

consumer.Received += (sender, e) => 
{
    string mesaj = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine($"Gelen mesaj: {mesaj}");
    // mesaj işlenir

    // Bu mesaj için response gönderilir
    byte[] responseMesaj = Encoding.UTF8.GetBytes($"işlem tamamlandi : {mesaj}" );
    IBasicProperties properties = channel.CreateBasicProperties();
    properties.Persistent = true;
    properties.CorrelationId = e.BasicProperties.CorrelationId; // response kuyruğunun adı
    
    // cevap gönder
    channel.BasicPublish(exchange: string.Empty, routingKey: e.BasicProperties.ReplyTo, basicProperties: properties, body: responseMesaj);
    // onay gönder
    channel.BasicAck(deliveryTag:e.DeliveryTag, multiple: false);
};

Console.Read();

#endregion




