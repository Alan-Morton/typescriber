using System.Text;
using ToTypeScript.Tests;

namespace ToTypeScript.Test.Common
{
    [TestClass]
    public class Structs : BaseTest
    {
        public struct TestStruct
        {
            public string Lastname { get; set; }
            public string Firstname;
        }        

        [TestMethod]
        public void CanOutputStructs()
        {
            var output = new StringBuilder();
            output.Append(
                new TypeScriber()
                    .AddType(typeof(TestStruct))
            );
            output.AppendLine("var foo: ToTypeScript.Test.Common.TestStruct;");
            ValidateTypeScript(output);
            var outputAsString = output.ToString();
            Assert.IsTrue(outputAsString.Contains("interface TestStruct  {"));
            Assert.IsFalse(outputAsString.Contains("ValueType"));
            Assert.IsTrue(outputAsString.Contains("Firstname: string"));
            Assert.IsTrue(outputAsString.Contains("Lastname: string"));
        }
    }
}

