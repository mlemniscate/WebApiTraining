namespace Contracts.Repository;

public interface IRepositoryManager
{
    ICompanyRepository CompanyRepository { get; }
    IEmployeeRepository EmployeeRepository { get; }
    void Save();
}