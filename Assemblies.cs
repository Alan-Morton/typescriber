using System.Reflection;
using System.Text;

namespace ToTypeScript.Tests
{
    [TestClass]
    public class Assemblies : BaseTest
    {
        [TestMethod]
        public void CanOutputTypesFromAssembly()
        {
            var assemlby = GetType().GetTypeInfo().Assembly;
            var output = new StringBuilder();
            output.Append(
                new TypeScriber()
                    .UsingAssembly(assemlby)
                    .AddTypes(assemlby)
            );
            output.AppendLine();
            output.AppendLine("var assemblyTest: ToTypeScript.Tests.Assemblies;");
            output.AppendLine("assemblyTest.CanOutputTypesFromAssembly();");
            ValidateTypeScript(output);
        }
    }
}

