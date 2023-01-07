using System.Dynamic;
using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service.Contracts;

public interface IEmployeeService
{
    Task<(IEnumerable<Entity> employees, MetaData metaData)> GetEmployeesAsync(Guid companyId,
        EmployeeParameters employeeParameters, bool trackChanges);
    Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges);
    Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeCreateDto employee, 
        bool trackChanges);

    Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges);
    Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id,
        EmployeeUpdateDto employeeUpdateDto, bool compTrackChanges,bool empTrackChanges);

    Task<(EmployeeUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid id,
        bool compTrackChanges, bool empTrackChanges);

    Task SaveChangesForPatchAsync(EmployeeUpdateDto employeeToPatch, Employee employeeEntity);
}