namespace Application.Interfaces.Infrastructure.Mqtt;

public interface IMqttPublisher<T>
{
    Task Publish(T dto);
}