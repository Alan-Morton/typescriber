using System.Text;
using ToTypeScript.Tests;

namespace ToTypeScript.Test.Common
{
    [TestClass]
    public class Generics : BaseTest
    {
        public class GenericClass<T>
        {
            public T GetInstance()
            {
                return default;
            }
        }

        public class Foo : GenericClass<string>
        {
        }

        public class Bar : Foo
        {
            public G GetInstanceOfType<G>()
            {
                return default;
            }
        }

        [TestMethod]
        public void CanOutputGenericTypes()
        {
            var output = new StringBuilder();
            output.Append(
                new TypeScriber()
                    .AddType(typeof(Foo))
            );
            output.AppendLine();
            output.AppendLine("var foo: ToTypeScript.Test.Common.Foo;");
            output.AppendLine("var result: string = foo.GetInstance();");
            ValidateTypeScript(output);
        }

        [TestMethod]
        public void CanOutputGenericMethods()
        {
            var output = new StringBuilder();
            output.Append(
                new TypeScriber()
                    .AddType(typeof(Bar))
            );
            output.AppendLine();
            output.AppendLine("var bar:  ToTypeScript.Test.Common.Bar;");
            output.AppendLine("var result1: string = bar.GetInstance();");
            output.AppendLine("var result2: number = bar.GetInstanceOfType<number>();");

            ValidateTypeScript(output);
        }
    }
}

