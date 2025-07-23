namespace Cinemasales.Settings;

public class ConsumerKafkaSettings
{
    public string PayGroup { get; init; }
    public string BookGroup { get; init; }
    public string BootstrapServers { get; init; }
    public string BookTopic { get; init; }
    public string PayTopic { get; init; }
}