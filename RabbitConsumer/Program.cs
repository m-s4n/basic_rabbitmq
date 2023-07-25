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
// autoAck: Message Ack özelliği false ile aktif edilir
var consumerTag = channel.BasicConsume(queue: "gs", autoAck: false, consumer: consumer);
// fair dispatch
channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
// Tüm mesajlar reddedilerek işlenmez
// channel.BasicCancel(consumerTag: consumerTag);
consumer.Received += (sender, e) =>
{
    // Mesajın işlendiği alan
    string mesaj = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(mesaj);
    // e.DeliveryTag = mesaj id
    // multiple false --> ilgili mesaja dair onay bildirimi
    // multiple true --> ilgili mesaj ve bundan önceki mesajlar için onay bildirimi
    channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false); // mesaj işlendi onayı
};

Console.ReadLine();
