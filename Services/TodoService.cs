namespace TodoApp.Services
{
    using TodoDB.Todos;
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;

    public class TodosService
    {
        private readonly IMongoCollection<Todos> _todos;

        public TodosService(
            IOptions<TodoDbSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _todos = mongoDatabase.GetCollection<Todos>(
                bookStoreDatabaseSettings.Value.Todo);
        }

        public async Task<List<Todos>> GetAsync() =>
            await _todos.Find(_ => true).ToListAsync();

        public async Task<Todos?> GetAsync(string id) =>
            await _todos.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Todos newTodo) =>
            await _todos.InsertOneAsync(newTodo);

        public async Task UpdateAsync(string id, Todos updatedBook) =>
            await _todos.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveAsync(string id) =>
            await _todos.DeleteOneAsync(x => x.Id == id);
    }
}
