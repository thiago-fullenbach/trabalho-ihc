using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DDD.Api.Domain.Models.RepoModel;
public class Presenca
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? usuario_id { get; set; }
    public DateTime datahora_presenca { get; set; }
    public string? local_id { get; set; }
    public bool? eh_entrada { get; set; }
    public bool? foi_aprovada { get; set; }
}