
namespace ESB.MassTransit.ReqResPattern.Shared.RequestResponseMessages;

// kontratların record olarak tasarlanması performanslıdır
public record ResponseMessage
{
    public string Text { get; set; }
}