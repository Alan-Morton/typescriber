using System.Text;
using ToTypeScript.Tests;

namespace ToTypeScript.Test.Common
{
    [TestClass]
    public class Inheritance : BaseTest
    {
        public class Foo
        {
            public string GetFooValue()
            {
                return "Foo";
            }
        }

        public class Bar : Foo
        {
            public string GetBarValue()
            {
                return "Bar";
            }
        }


        [TestMethod]
        public void CanOutputDerivedTypes()
        {
            var output = new StringBuilder();
            output.Append(
                new TypeScriber().AddType(typeof(Bar))
            );
            output.AppendLine();
            output.AppendLine("var bar: ToTypeScript.Test.Common.Bar;");
            output.AppendLine("var result1: string = bar.GetFooValue();");
            output.AppendLine("var result2: string = bar.GetBarValue();");

            ValidateTypeScript(output);
        }
    }
}

