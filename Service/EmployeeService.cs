using AutoMapper;
using Contracts;
using Contracts.Repository;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service;

public class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager repository;
    private readonly ILoggerManager logger;
    private readonly IMapper mapper;
    private readonly IDataShaper<EmployeeDto> dataShaper;

    public EmployeeService(IRepositoryManager repository,
        ILoggerManager logger,
        IMapper mapper, 
        IDataShaper<EmployeeDto> dataShaper)
    {
        this.repository = repository;
        this.logger = logger;
        this.mapper = mapper;
        this.dataShaper = dataShaper;
    }

    public async Task<(IEnumerable<EmployeeDto> employees, MetaData metaData)> GetEmployeesAsync(Guid companyId,
        EmployeeParameters employeeParameters, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges);
        
        var employeesWithMetaData = await repository.Employee.GetEmployeesAsync(companyId,
            employeeParameters, trackChanges);

        var employeesDto = mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);
        var shapedData = dataShaper.ShapeData(employeesDto, employeeParameters.Fields);

        return (employees: shapedData, metaData: employeesWithMetaData.MetaData);

    }

    public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges);

        var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges);

        var employeeDto = mapper.Map<EmployeeDto>(employeeDb);
        return employeeDto;
    }

    public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeCreateDto employee, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges);

        var employeeEntity = mapper.Map<Employee>(employee);

        repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
        await repository.SaveAsync();

        return mapper.Map<EmployeeDto>(employeeEntity);
    }

    public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges);

        var employeeForCompany = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges);

        repository.Employee.DeleteEmployee(employeeForCompany);
        await repository.SaveAsync();
    }

    public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id,
        EmployeeUpdateDto employeeUpdateDto , bool compTrackChanges, bool empTrackChanges)
    {
        await CheckIfCompanyExists(companyId, compTrackChanges);

        var employeeEntity = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, empTrackChanges);

        mapper.Map(employeeUpdateDto, employeeEntity);
        await repository.SaveAsync();
    }

    public async Task<(EmployeeUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(
        Guid companyId, Guid id,
        bool compTrackChanges, bool empTrackChanges)
    {
        await CheckIfCompanyExists(companyId, compTrackChanges);

        var employeeEntity = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, empTrackChanges);

        var employeeToPatch = mapper.Map<EmployeeUpdateDto>(employeeEntity);

        return (employeeToPatch: employeeToPatch, employeeEntity: employeeEntity);
    }

    public async Task SaveChangesForPatchAsync(EmployeeUpdateDto employeeToPatch, Employee employeeEntity)
    {
        mapper.Map(employeeToPatch, employeeEntity);
        await repository.SaveAsync();
    }

    private async Task<Employee?> GetEmployeeForCompanyAndCheckIfItExists(Guid companyId, Guid id, bool trackChanges)
    {
        var employeeDb = await repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);
        if (employeeDb == null)
            throw new EmployeeNotFoundException(id);
        return employeeDb;
    }

    private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges)
    {
        var company = await repository.Company.GetCompanyAsync(companyId, trackChanges);
        if (company == null)
            throw new CompanyNotFoundException(companyId);
    }
}