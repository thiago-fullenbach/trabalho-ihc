using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DDD.Api.Domain.Models.RepoModel;
public class Acesso
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? usuario_id { get; set; }
    public int recurso_cod_en { get; set; }
    public bool? eh_habilitado { get; set; }
}