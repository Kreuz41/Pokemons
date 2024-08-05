namespace PokemonsDomain.MessageBroker.Sender;

public interface IBrokerSender
{
    Task Send(object obj);
    Task Send(byte[] bytes);
    Task<byte[]> RpsCaller(byte[]? content);
}