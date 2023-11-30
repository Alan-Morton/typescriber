using System.Text;
using ToTypeScript.Tests;
using ToTypeScript.Test.Examples.Dom;

namespace ToTypeScript.Test.Common
{
    [TestClass]
    public class ReadonlyFieldsTest : BaseTest
    {

        [TestMethod]
        public void CanOutputReadOnlyFields()
        {
            var output = new StringBuilder();
            output.Append(
                new TypeScriber()
                    .AddType(typeof(Alan))
            );

            output.AppendLine();
            output.AppendLine("var alan: ToTypeScript.Test.Examples.Dom.Alan;");
            output.AppendLine("var name: string = alan.Name;");

            ValidateTypeScript(output);
        }
    }
}

