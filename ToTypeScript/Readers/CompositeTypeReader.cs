using System.Reflection;

namespace ToTypeScript.Readers
{
    public class CompositeTypeReader : TypeReader
    {
        private IEnumerable<TypeReader> Readers { get; set; }

        public CompositeTypeReader(params TypeReader[] readers)
        {
            Readers = readers;
        }

        public override IEnumerable<TypeInfo> GetTypes(Assembly assembly)
        {
            return Readers.SelectMany(x => x.GetTypes(assembly)).Distinct();
        }

        public override IEnumerable<FieldInfo> GetFields(TypeInfo type)
        {
            return Readers.SelectMany(x => x.GetFields(type)).Distinct();
        }

        public override IEnumerable<PropertyInfo> GetProperties(TypeInfo type)
        {
            return Readers.SelectMany(x => x.GetProperties(type)).Distinct();
        }

        public override IEnumerable<MethodInfo> GetMethods(TypeInfo type)
        {
            return Readers.SelectMany(x => x.GetMethods(type)).Distinct();
        }

        public override IEnumerable<ParameterInfo> GetParameters(MethodInfo method)
        {
            return Readers.SelectMany(x => x.GetParameters(method)).Distinct();
        }
    }
}