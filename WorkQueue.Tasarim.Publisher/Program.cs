﻿
using System.Text;
using RabbitMQ.Client;

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

//Work queue
// mesaj direkt olarak kuyruğa gönderileceği için direkt kuyruk oluşturulur.
channel.QueueDeclare(queue: "work_queue_1", durable: true, exclusive: false, autoDelete: false);
IBasicProperties properties = channel.CreateBasicProperties();
properties.Persistent = true;

while (true)
{
    Console.Write("Mesaj giriniz: ");
    string mesaj = Console.ReadLine() ?? "Mesaj girilmedi";
    byte[] byteMesaj = Encoding.UTF8.GetBytes(mesaj);
    channel.BasicPublish(exchange: string.Empty, routingKey: "work_queue_1", body: byteMesaj, basicProperties: properties);
}