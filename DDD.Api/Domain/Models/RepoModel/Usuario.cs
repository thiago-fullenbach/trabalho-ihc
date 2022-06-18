using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DDD.Api.Domain.Models.RepoModel;
public class Usuario
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string nome { get; set; }
    public string cpf { get; set; }
    public DateTime data_nascimento { get; set; }
    public int horas_diarias { get; set; }
    public string login { get; set; }
    public string senha { get; set; }
    public string url_hasheia_senha_sem_parametros { get; set; }
}