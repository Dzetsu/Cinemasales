namespace WalletPay.Settings;

public class ConsumerKafkaSettings
{
    public string BootstrapServers { get; init; }
    public string PayTopic { get; init; }
    public string PayRollbackTopic { get; init; }
    public string PayGroup { get; init; }
    public string PayRollbackGroup { get; init; }
}