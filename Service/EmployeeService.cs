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

    public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid companyId, bool trackChanges)
    {
        var company = await repository.Company.GetCompanyAsync(companyId, trackChanges);
        if (company == null)
            throw new CompanyNotFoundException(companyId);
        
        var employeesFromDb = await repository.Employee.GetEmployeesAsync(companyId, trackChanges);

        var employeesDto = mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);

        return employeesDto;
    }

    public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
    {
        var company = await repository.Company.GetCompanyAsync(companyId, trackChanges);
        if(company == null)
            throw new CompanyNotFoundException(companyId);

        var employeeDb = await repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);
        if (employeeDb == null)
            throw new EmployeeNotFoundException(id);

        var employeeDto = mapper.Map<EmployeeDto>(employeeDb);
        return employeeDto;
    }

    public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeCreateDto employee, bool trackChanges)
    {
        var company = await repository.Company.GetCompanyAsync(companyId, trackChanges);
        if(company is null) 
            throw new CompanyNotFoundException(companyId);

        var employeeEntity = mapper.Map<Employee>(employee);

        repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
        await repository.SaveAsync();

        return mapper.Map<EmployeeDto>(employeeEntity);
    }

    public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges)
    {
        var company = await repository.Company.GetCompanyAsync(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employeeForCompany = await repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);
        if(employeeForCompany is null)
            throw new EmployeeNotFoundException(id);

        repository.Employee.DeleteEmployee(employeeForCompany);
        await repository.SaveAsync();
    }

    public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id,
        EmployeeUpdateDto employeeUpdateDto , bool compTrackChanges, bool empTrackChanges)
    {
        var company = await repository.Company.GetCompanyAsync(companyId, compTrackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employeeEntity = await repository.Employee.GetEmployeeAsync(companyId, id, empTrackChanges);
        if(employeeEntity is null)
            throw new EmployeeNotFoundException(id);

        mapper.Map(employeeUpdateDto, employeeEntity);
        await repository.SaveAsync();
    }

    public async Task<(EmployeeUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid id,
        bool compTrackChanges, bool empTrackChanges)
    {
        var company = await repository.Company.GetCompanyAsync(companyId, compTrackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employeeEntity = await repository.Employee.GetEmployeeAsync(companyId, id, empTrackChanges);
        if(employeeEntity is null)
            throw new EmployeeNotFoundException(id);

        var employeeToPatch = mapper.Map<EmployeeUpdateDto>(employeeEntity);

        return (employeeToPatch, employeeEntity);
    }

    public async Task SaveChangesForPatchAsync(EmployeeUpdateDto employeeToPatch, Employee employeeEntity)
    {
        mapper.Map(employeeToPatch, employeeEntity);
        await repository.SaveAsync();
    }
}