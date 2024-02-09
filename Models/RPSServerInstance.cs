using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace RPS.Network.Models
{
    public class RPSServerInstance
    {
        public RPSServerInstance() { }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("Id")]
        public string Id { get; set; }

        [BsonElement("LobbyID")]
        public string LobbyID { get; set; } = "AAAA";

        [BsonElement("P1Role")]
        public string P1Role { get; set; } = "";

        [BsonElement("P2Role")]
        public string P2Role { get; set; } = "";

        [BsonElement("IsP1Ready")]
        public bool IsP1Ready { get; set; } = false;

        [BsonElement("IsP2Ready")]
        public bool IsP2Ready { get; set; } = false;

        [BsonElement("InResult")]
        public bool InResult { get; set; } = false;

        public void ResetContent()
        {
            P1Role = "";
            P2Role = "";
            IsP1Ready = false;
            IsP2Ready = false;
            InResult = false;
        }
    }
}
