
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

//Pub-Sub
// exchange tanimlanir
channel.ExchangeDeclare(exchange: "pub_sub_exchange_1", type: ExchangeType.Fanout, durable: true, autoDelete: false);

// kuyruk oluşturulur
string queueName = channel.QueueDeclare(durable:true, exclusive: false, autoDelete: false).QueueName;

//kuyruk exchange'e bind edilir
channel.QueueBind(queue: queueName, exchange: "pub_sub_exchange_1", routingKey: string.Empty);

// ölçekleme --> tüm tüketiciler o anda 1 mesaj işlesin
channel.BasicQos(prefetchCount: 1, prefetchSize: 0, global: false);

// consumer oluşturulur
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: queueName, autoAck: false,consumer: consumer);

// mesajlar yakalanır
consumer.Received += (sender, e) => 
{
    string mesaj = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(mesaj);
    channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
};

Console.Read();


