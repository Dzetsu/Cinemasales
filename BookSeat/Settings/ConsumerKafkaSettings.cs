namespace BookSeat.Settings;

public class ConsumerKafkaSettings
{
    public string BootstrapServers { get; init; }
    public string BookTopic { get; init; }
    public string BookRollbackTopic { get; init; }
    public string BookGroup { get; init; }
    public string BookRollbackGroup { get; init; }
}