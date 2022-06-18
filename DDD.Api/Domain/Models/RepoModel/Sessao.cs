using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DDD.Api.Domain.Models.RepoModel;
public class Sessao
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string hex_verificacao { get; set; }
    public string? usuario_id { get; set; }
    public DateTime datahora_ultima_autenticacao { get; set; }
}