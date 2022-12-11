using Contracts.Repository;

namespace Repository;

public class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext context;
    private readonly Lazy<ICompanyRepository> companyRepository;
    private readonly Lazy<IEmployeeRepository> employeeRepository;

    public RepositoryManager(RepositoryContext context, Lazy<IEmployeeRepository> employeeRepository, Lazy<ICompanyRepository> companyRepository)
    {
        this.context = context;
        this.employeeRepository = new Lazy<IEmployeeRepository>(() => new
            EmployeeRepository(context));
        this.companyRepository = new Lazy<ICompanyRepository>(() => new 
            CompanyRepository(context));
    }

    public void Save() => context.SaveChanges();

    public ICompanyRepository CompanyRepository => companyRepository.Value;
    public IEmployeeRepository EmployeeRepository => employeeRepository.Value;
}