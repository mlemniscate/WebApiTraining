using System.Reflection;
using System.Text;
using Entities.Models;

namespace Repository.Extensions.Utility;

public class OrderQueryBuilder
{
    public static string? CreateOrderQuery<T>(string orderByQueryString)
    {
        var orderParams = orderByQueryString.Trim().Split(',');
        var propertyInfos = typeof(Employee)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var orderQueryBuilder = new StringBuilder();
        
        foreach (var param in orderParams)
        {
            if(string.IsNullOrWhiteSpace(param)) continue;
        
            var propertyFromQueryName = param.Split(" ")[0];
            var objectProperty = propertyInfos.FirstOrDefault(pi =>
                pi.Name.Equals(propertyFromQueryName, StringComparison.CurrentCultureIgnoreCase));
            if (objectProperty is null) continue;
        
            var direction = param.EndsWith(" desc") ? "descending" : "ascending";
            orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction}, ");
        }
        
        var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
        return orderQuery;
    }
}