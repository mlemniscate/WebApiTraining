namespace Entities.Exceptions;

public class CompanyNotFoundException : NotFoundException
{
    public CompanyNotFoundException(Guid companyId) 
        : base($"The company with id: {companyId} doesn't exist int the database.")
    {
    }
}