using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ContosoUniversity.Infrastructure.Extensions
{
    public static class ModelExtensions
    {
        public static string GetPropertyDisplayName<T>(string propertyName)
        {
            var property = typeof(T).GetProperty(propertyName);

            if (property == null)
                return string.Empty;


            if (property.GetCustomAttribute(typeof(DisplayAttribute)) is DisplayAttribute dd)
                return dd.Name;

            return propertyName.BreakUpCamelCase();
        }
    }
}
