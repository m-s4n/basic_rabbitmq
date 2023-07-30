using MassTransit;
using ESB.MassTransit.ReqResPattern.Shared.RequestResponseMessages;

namespace ESB.MassTransit.ReqResPattern.Consumer.Consumers;
public class RequestMessageConsumer : IConsumer<RequestMessage>
{
    public async Task Consume(ConsumeContext<RequestMessage> context)
    {
        // mesaj alınır
        RequestMessage receivedMesaj = context.Message;
        // işlenir
        await Console.Out.WriteLineAsync(receivedMesaj.Text);

        // yanıt gönderilir
        ResponseMessage responseMesaj = new(){Text = $"{receivedMesaj}'ın yanıtı"};
        await context.RespondAsync<ResponseMessage>(message: responseMesaj);
    }
}