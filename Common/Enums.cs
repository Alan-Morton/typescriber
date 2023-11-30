using ToTypeScript.Tests;
using ToTypeScript.Test.Examples.Dom;

namespace ToTypeScript.Test.Common
{
    [TestClass]
    public class Enums : BaseTest
    {
        [TestMethod]
        public void CanOutputEnums()
        {
            var scripter = new TypeScriber();
            var output = scripter
                .AddType(typeof(Person))
                .ToString();

            ValidateTypeScript(output);
        }

        [TestMethod]
        public void TestThatEnumIsRendered()
        {
            var scripter = new TypeScriber();
            var output = scripter
                .AddType(typeof(Person))
                .ToString();

            Assert.IsTrue(output.Contains("Female"));
            Assert.IsTrue(output.Contains("Male"));
            Assert.IsTrue(output.Contains("Undisclosed"));
            Assert.IsTrue(output.Contains("gender: ToTypeScript.Test.Examples.Dom.Gender"));
        }

        [TestMethod]
        public void TestThatEnumIsRenderedAsString()
        {
            var scripter = new TypeScriber();
            var output = scripter.UsingFormatter(new TsFormatter
            {
                EnumsAsString = true
            }).AddType(typeof(Person)).ToString();
            Assert.IsTrue(output.Contains("type Gender = 'Female' | 'Male' | 'Undisclosed'"));
            Assert.IsTrue(output.Contains("gender: ToTypeScript.Test.Examples.Dom.Gender;"));
        }

        [TestMethod]
        public void EnumValueIsValidated()
        {
            var scripter = new TypeScriber();
            var output = scripter.UsingFormatter(new TsFormatter
            {
                EnumsAsString = true
            }).AddType(typeof(Person)).ToString();
            var code = "\n var x: ToTypeScript.Test.Examples.Dom.Person; \n" +
                       "x.gender = 'test'";
            AssertNotValidTypeScript(output + code);
            code = "\n var x: TToTypeScript.Test.Examples.Dom.Person; \n" +
                       "x.gender = 'Female'";
            ValidateTypeScript(output + code);
        }
    }
}