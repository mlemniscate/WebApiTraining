namespace Entities.Exceptions;

public class CollectionByIdsBadRequestException : BadRequestException
{
    protected CollectionByIdsBadRequestException() : base("Collection count mismatch comparing to ids.")
    {
    }
}