using System.Dynamic;
using System.Reflection;
using Contracts;

namespace Service.DataShapping;

public class DataShaper<T> : IDataShaper<T> where T : class
{
    public DataShaper()
    {
        Properties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }

    public PropertyInfo[] Properties { get; set; }


    public IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string fieldString)
    {
        var requiredProperties = GetRequiredProperties(fieldString);

        return FetchData(entities, requiredProperties);
    }

    public ExpandoObject ShapeData(T entity, string fieldString)
    {
        var requiredProperties = GetRequiredProperties(fieldString);

        return FetchDataForEntity(entity, requiredProperties);
    }

    private IEnumerable<PropertyInfo> GetRequiredProperties(string fieldsString)
    {
        var requiredProperties = new List<PropertyInfo>();

        if (!string.IsNullOrWhiteSpace(fieldsString))
        {
            var fields = fieldsString
                .Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var field in fields)
            {
                var property = Properties
                    .FirstOrDefault(pi => pi.Name.Equals(field.Trim(),
                        StringComparison.InvariantCultureIgnoreCase));

                if (property is null)
                    continue;

                requiredProperties.Add(property);
            }
        }
        else
        {
            requiredProperties = Properties.ToList();
        }

        return requiredProperties;
    }

    private IEnumerable<ExpandoObject> FetchData(IEnumerable<T> entities,
        IEnumerable<PropertyInfo> requiredProperties)
    {
        var ShapedData = new List<ExpandoObject>();

        foreach (var entity in entities)
        {
            var shapedObject = FetchDataForEntity(entity, requiredProperties);
            ShapedData.Add(shapedObject);
        }

        return ShapedData;
    }

    private ExpandoObject FetchDataForEntity(T entity, IEnumerable<PropertyInfo> requiredProperties)
    {
        var shapedObject = new ExpandoObject();

        foreach (var property in requiredProperties)
        {
            var objectPropertyValue = property.GetValue(entity);
            shapedObject.TryAdd(property.Name, objectPropertyValue);
        }

        return shapedObject;
    }
}