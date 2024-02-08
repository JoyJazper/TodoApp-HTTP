using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoDB.Todos
{
    public class Todos
    {
        public Todos() { }
        public Todos(string Todo) {
            Task = Todo;
            IsDone = false;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)][BsonElement("Id")]
        public string Id { get; set; }

        [BsonElement("Task")]
        public string Task { get; set; } = "";

        [BsonElement("IsDone")]
        public bool IsDone { get; set; } = false;
    }
}

