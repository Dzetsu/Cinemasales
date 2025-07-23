namespace Cinemasales.Settings;

public class ProducerKafkaSettings
{
    public string BootstrapServers { get; init; }
    public string PayTopic { get; init; }
    public string BookTopic { get; init; }
    public string PayRollbackTopic { get; init; }
    public string BookRollbackTopic { get; init; }
}