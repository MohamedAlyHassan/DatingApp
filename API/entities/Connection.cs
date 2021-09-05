namespace API.entities
{
    public class Connection
    {
        public Connection()
        {
        }

        public Connection(string connectionId, string usename)
        {
            ConnectionId = connectionId;
            Usename = usename;
        }

        public string ConnectionId { get; set; }
        public string Usename { get; set; }
    }
}