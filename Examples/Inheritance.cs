using ToTypeScript.Tests;
using ToTypeScript.Test.Examples.Dom;

namespace ToTypeScript.Examples
{
    [TestClass]
    public class Inheritance : BaseTest
    {
        [TestMethod]
        public void InheritanceExample()
        {
            var scripter = new TypeScriber();
            var output = scripter
                .AddType(typeof(People))
                .ToString();

            ValidateTypeScript(output);
        }
    }
}
