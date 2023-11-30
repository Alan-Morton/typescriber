using System.Reflection; 
using ToTypeScript.Tests;
using ToTypeScript.Test.Examples.Dom;

namespace ToTypeScript.Examples
{
    [TestClass]
    public class TypeReaders : BaseTest
    {
        public class PlanetTypeReader : TypeReader
        {
            public override IEnumerable<TypeInfo> GetTypes(Assembly assembly)
            {
                return base.GetTypes(assembly)
                    .Where(x => x.IsSubclassOf(typeof(Planet)))
                    .Where(x => !x.IsAbstract);
            }
        }

        [TestMethod]
        public void TypeReaderExample()
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var scripter = new TypeScriber();
            var output = scripter
                .UsingTypeReader(
                    new PlanetTypeReader()
                )
                .AddTypes(assembly)
                .ToString();
            ValidateTypeScript(output);
        }
    }
}
