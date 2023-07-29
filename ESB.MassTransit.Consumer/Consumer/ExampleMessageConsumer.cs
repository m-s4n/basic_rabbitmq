using ESB.MassTransit.Shared.Messages;
using MassTransit;

namespace ESB.MassTransit.Consumer.Consumer;
// hangi mesaji tüketicekse türü verilir
public class ExampleMessageConsumer : IConsumer<IMessage>
{
    // ne zaman ilgili kuyruga IMessage türünde mesaj geldi 
    // o zaman bu consumer consume metodunu tetikleyecektir
    // context üzerinden mesaj elde edilecektir
    public Task Consume(ConsumeContext<IMessage> context)
    {
        IMessage mesaj = context.Message;
        Console.WriteLine($"Gelen mesaj : {mesaj.Text}");
        return Task.CompletedTask;
    }
}