using ToTypeScript.Tests;
using ToTypeScript.Test.Examples.Dom;

namespace ToTypeScript.Examples
{
    [TestClass]
    public class Basic : BaseTest
    {
        [TestMethod]
        public void BasicExample()
        {
            var scripter = new TypeScriber();
            var output = scripter
                .AddType(typeof(Alan2))
                .ToString();
            ValidateTypeScript(output);
        }
    }
}
