namespace DDD.Api.Domain.Models.BusnModel.Exception;
public class BusinessException : System.Exception
{
    public BusinessException()
    {
        MensagensErro = new List<string>();
    }
    public int StatusCode { get; set; }
    public List<string> MensagensErro { get; set; }
}