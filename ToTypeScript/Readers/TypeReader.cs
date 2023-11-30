using System.Reflection;

namespace ToTypeScript.Readers
{
    /// <summary>
    /// A class for reading type information
    /// </summary>
    public class TypeReader
    {
        public virtual IEnumerable<TypeInfo> GetTypes(Assembly assembly)
        {
            return assembly.GetExportedTypes()
                .Select(x => x.GetTypeInfo())
                .Where(x => x.IsPublic)
                .Where(x => !x.IsPointer);
        }

        public virtual IEnumerable<FieldInfo> GetFields(TypeInfo type)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }

        public virtual IEnumerable<PropertyInfo> GetProperties(TypeInfo type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(x => !x.PropertyType.IsPointer)
                .Where(x => !x.IsSpecialName);
        }

        public virtual IEnumerable<MethodInfo> GetMethods(TypeInfo type)
        {
            return type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(x => !x.GetParameters().Any(y => y.ParameterType.IsByRef))
                .Where(x => !x.GetParameters().Any(y => y.ParameterType.IsPointer))
                .Where(x => !x.ReturnType.IsPointer)
                .Where(x => !x.IsSpecialName);
        }

        public virtual IEnumerable<ParameterInfo> GetParameters(MethodInfo method)
        {
            return method.GetParameters()
                .Where(x => x.Name != null);
        }
    }
}
