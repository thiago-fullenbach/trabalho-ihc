using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DDD.Api.Domain.Models.RepoModel;
public class Local
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string logradouro { get; set; }
    public string numero { get; set; }
    public string complemento { get; set; }
    public string bairro { get; set; }
    public string cidade { get; set; }
    public string uf { get; set; }
}