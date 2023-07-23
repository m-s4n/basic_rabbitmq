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

// Queue Oluşturma - producer ile aynı
channel.QueueDeclare(queue: "gs", exclusive: false, durable: true, autoDelete: false);

// Mesaj Okuma
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: "gs", autoAck: true, consumer: consumer);
consumer.Received += (sender, e) => {
    // Mesajın işlendiği alan
    string mesaj = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(mesaj);
};

Console.ReadLine();
  