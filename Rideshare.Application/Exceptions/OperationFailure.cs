namespace Rideshare.Application.Exceptions;

public class OperationFailure: Exception
{
    public OperationFailure(string message): base(message)
    {      
    }
}
