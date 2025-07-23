namespace WalletPay.Settings;

public class ProducerKafkaSettings
{
    public string BootstrapServers { get; init; }
    public string PayTopic { get; init; }
}