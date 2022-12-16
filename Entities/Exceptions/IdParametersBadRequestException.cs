namespace Entities.Exceptions;

public class IdParametersBadRequestException : BadRequestException
{
    public IdParametersBadRequestException() : 
        base("Parameter ids is null")
    {
    }
}