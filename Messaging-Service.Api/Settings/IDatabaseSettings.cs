namespace Messaging_Service.Api.Settings
{
    public interface IDatabaseSettings
    {
        public string UserCollections { get; set; }
        public string MessageCollections { get; set; }
        public string BlockUserCollections { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
