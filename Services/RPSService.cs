using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RPS.Network.Models;

namespace RPS.Network.Services
{
    public class RPSService
    {
        private readonly IMongoCollection<RPSServerInstance> _RPSServers;

        public RPSService(IOptions<RPSDBSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _RPSServers = mongoDatabase.GetCollection<RPSServerInstance>(
                bookStoreDatabaseSettings.Value.RPSServerInstance);
        }

        //public List<RPSServerInstance> GetAsync()
        //{
        //    List<RPSServerInstance> temp = new List<RPSServerInstance> ();
        //    RPSServerInstance todo = new RPSServerInstance("Mock test Text");
        //    temp.Add(todo);
        //    return temp;
        //}

        public async Task<List<RPSServerInstance>> GetAsync()
        {
            return await _RPSServers.Find(x => true).ToListAsync();
        }
        //await _RPSServers.Find(_ => true).ToListAsync();

        public async Task<RPSServerInstance?> GetAsync(string id) =>
            await _RPSServers.Find(x => x.Id == id).FirstOrDefaultAsync();
        public async Task CreateAsync(RPSServerInstance newRPSServerInstance) =>
            await _RPSServers.InsertOneAsync(newRPSServerInstance);
        public async Task UpdateAsync(string id, RPSServerInstance updatedRPSServerInstance) =>
            await _RPSServers.ReplaceOneAsync(x => x.Id == id, updatedRPSServerInstance);
        public async Task RemoveAsync(string id) =>
            await _RPSServers.DeleteOneAsync(x => x.Id == id);
    }
}
