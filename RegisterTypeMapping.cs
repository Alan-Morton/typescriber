using System.Text; 

namespace ToTypeScript.Tests
{
    [TestClass]
    public class RegisterTypeMapping : BaseTest
    {
        public class TestDev
        {
            public HashSet<string> Skills { get; set; }
            public LinkedList<int> FavouriteFrameworks { get; set; }

            public TestDev(HashSet<string> skills, LinkedList<int> favouriteFrameworks)
            {
                Skills = skills;
                FavouriteFrameworks = favouriteFrameworks;
            }
        }

        [TestMethod]
        public void CanOutputWithCustomTypeMapping()
        {
            var output = new StringBuilder();
            output.Append(new TypeScriber()
                    .WithTypeMapping(new TsInterface(new TsName("Array")), typeof(HashSet<>))
                    .WithTypeMapping(new TsArray(TsPrimitive.String, 1), typeof(LinkedList<int>))
                    .AddType(typeof(TestDev))
            );

            output.AppendLine();
            output.AppendLine("var dev: ToTypeScript.Tests.TestDev");
            output.AppendLine("var skills: string[] = dev.Skills;");
            output.AppendLine("var favNumbers: string[] = dev.FavouriteFrameworks;");

            ValidateTypeScript(output);
        }
    }
}

