namespace BookSeat.Settings;

public class ProducerKafkaSettings
{
    public string BootstrapServers { get; init; }
    public string BookTopic { get; init; }
}