namespace Entities.Exceptions;

public class MaxAgeRangeBadRequestException : BadRequestException
{
    public MaxAgeRangeBadRequestException() : base("MaxAge can't be less then min age.")
    {
    }
}