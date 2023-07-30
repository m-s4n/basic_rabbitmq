using MassTransit;
using ESB.MassTransit.Shared.Messages;

namespace ESB.MassTransit.WorkerService.Consumer.Consumers;

public class ExampleMessageConsumer : IConsumer<IMessage>
{
    public Task Consume(ConsumeContext<IMessage> context)
    {
        IMessage message = context.Message;
        Console.WriteLine($"Gelen mesaj : {message.Text}");
        return Task.CompletedTask;
    }
}