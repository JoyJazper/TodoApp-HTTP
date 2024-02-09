namespace RPS.Network.Models
{
    public class RPSDBSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string RPSServerInstance { get; set; } = null!;
    }
}
