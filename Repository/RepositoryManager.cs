using Contracts.Repository;

namespace Repository;

public class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext context;
    private readonly Lazy<ICompanyRepository> companyRepository;
    private readonly Lazy<IEmployeeRepository> employeeRepository;

    public RepositoryManager(RepositoryContext context)
    {
        this.context = context;
        this.employeeRepository = new Lazy<IEmployeeRepository>(() => new
            EmployeeRepository(context));
        this.companyRepository = new Lazy<ICompanyRepository>(() => new 
            CompanyRepository(context));
    }

    public async Task SaveAsync() => await context.SaveChangesAsync();

    public ICompanyRepository Company => companyRepository.Value;
    public IEmployeeRepository Employee => employeeRepository.Value;
}