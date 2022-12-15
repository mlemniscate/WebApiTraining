using AutoMapper;
using Contracts;
using Contracts.Repository;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

public class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager repository;
    private readonly ILoggerManager logger;
    private readonly IMapper mapper;

    public EmployeeService(IRepositoryManager repository, 
        ILoggerManager logger,
        IMapper mapper)
    {
        this.repository = repository;
        this.logger = logger;
        this.mapper = mapper;
    }

    public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
    {
        var company = repository.Company.GetCompany(companyId, trackChanges);
        if (company == null)
            throw new CompanyNotFoundException(companyId);
        
        var employeesFromDb = repository.Employee.GetEmployees(companyId, trackChanges);

        var employeesDto = mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);

        return employeesDto;
    }

    public EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges)
    {
        var company = repository.Company.GetCompany(companyId, trackChanges);
        if(company == null)
            throw new CompanyNotFoundException(companyId);

        var employeeDb = repository.Employee.GetEmployee(companyId, id, trackChanges);
        if (employeeDb == null)
            throw new EmployeeNotFoundException(id);

        var employeeDto = mapper.Map<EmployeeDto>(employeeDb);
        return employeeDto;
    }

    public EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeCreateDto employee, bool trackChanges)
    {
        var company = repository.Company.GetCompany(companyId, trackChanges);
        if(company is null) 
            throw new CompanyNotFoundException(companyId);

        var employeeEntity = mapper.Map<Employee>(employee);

        repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
        repository.Save();

        return mapper.Map<EmployeeDto>(employeeEntity);
    }
}