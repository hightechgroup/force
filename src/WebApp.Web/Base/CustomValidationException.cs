namespace WebApp.Web.Base;

public class CustomValidationException: Exception
{
    public Dictionary<string, List<string>> ValidationMessages { get;}

    public CustomValidationException(Dictionary<string, List<string>> validationMessages)
    {
        ValidationMessages = validationMessages;
    }
}