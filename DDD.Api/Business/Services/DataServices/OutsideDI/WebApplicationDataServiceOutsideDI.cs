namespace DDD.Api.Business.Services.DataServices.OutsideDI;
public class WebApplicationDataServiceOutsideDI
{
    private static WebApplicationDataServiceOutsideDI? _instance;
    public static WebApplicationDataServiceOutsideDI GetInstance()
    {
        if (_instance == null)
        {
            _instance = new WebApplicationDataServiceOutsideDI();
        }
        return _instance;
    }
    private WebApplicationDataServiceOutsideDI()
    {
    }
    private WebApplication? _app;
    public WebApplication? GetWebApplication()
    {
        return _app;
    }

    public void SetWebApplication(WebApplication app)
    {
        _app = app;
    }
    
}