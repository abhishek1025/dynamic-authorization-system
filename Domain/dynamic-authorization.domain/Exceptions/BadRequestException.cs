namespace dynamic_authorization.domain.Exceptions;

public class BadRequestException:Exception
{
    public BadRequestException(string message)
        : base(message)
    {
    }
}