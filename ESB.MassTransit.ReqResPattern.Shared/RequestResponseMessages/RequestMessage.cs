
namespace ESB.MassTransit.ReqResPattern.Shared.RequestResponseMessages;

// kontratların record olarak tasarlanması performanslıdır
public record RequestMessage
{
    public int MessageNo { get; set; }
    public string Text { get; set; }
}