using ToTypeScript.Tests;
using ToTypeScript.Test.Examples.Dom;

namespace ToTypeScript.Test.Examples
{
    [TestClass]
    public class WithToFile : BaseTest
    {
        [TestMethod]
        public void WithToFileExample()
        {
            var result =  ToFileResult.ToFile<AccountVM>("accountVm");
            Assert.IsNotNull(result);
        }
    }
}
