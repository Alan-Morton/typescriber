using ToTypeScript.Tests;
using ToTypeScript.Test.Examples.Dom;

namespace ToTypeScript.Examples
{
    [TestClass]
    public class Generics : BaseTest
    {
        [TestMethod]
        public void GenericsExample()
        {
            var scripter = new TypeScriber();
            var output = scripter
                .AddType(typeof(UniversePeople))
                .ToString();
            ValidateTypeScript(output);
        }
    }
}
