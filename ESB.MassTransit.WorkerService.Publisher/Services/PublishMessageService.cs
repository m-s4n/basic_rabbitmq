using MassTransit;
using ESB.MassTransit.Shared.Messages;

namespace ESB.MassTransit.WorkerService.Publisher.Services;
//Background Servis olacak
public class PublishMessageService : BackgroundService
{
    // IoC container'dan taleple bulunacaklar için program.cs den gönderilir
    readonly IPublishEndpoint _publishEndpoint;
    // readonly ISendEndpointProvider _sendEndpointProvider;

    public PublishMessageService(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    // kuyruk bildirmeden ilgili mesaj publis edildi
    // publish genel davranışı bu exchange'e bind edilmiş tüm kuyruklara mesaj gönderir.
    // masstransit arka planda exchange yönetimini üstleniyor
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
       int i = 0;
       while(i< 50)
       {
            ExampleMessage message = new(){Text = $"{++i}. mesaj"};
            await _publishEndpoint.Publish(message);
            Console.WriteLine("mesaj gönderildi");
       }


    }
}