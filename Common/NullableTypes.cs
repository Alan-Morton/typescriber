using ToTypeScript.Tests;

namespace ToTypeScript.Test.Common
{
    public class TypeWithNullables
    {
        public Enum1? Property1 { get; set; }
        public int? IntProperty { get; set; }

        public long? LongField; // testing without {get;set;} 
    }

    public enum Enum1
    {
        Value1,
        Value2,
        Value3
    }

    [TestClass]
    public class NullableTypes : BaseTest
    {
        [TestMethod]
        public void CanOutputEnums()
        {
            var scripter = new TypeScriber();
            var output = scripter
                .AddType(typeof(TypeWithNullables))
                .ToString();

            ValidateTypeScript(output);
        }

        [TestMethod]
        public void TestThatNullableTypeIsRendered()
        {
            var scripter = new TypeScriber();
            var output = scripter
                .AddType(typeof(TypeWithNullables))
                .ToString();
            Assert.IsTrue(output.Contains("const enum Enum1"), "continue to resolve enum");
            Assert.IsTrue(output.Contains("IntProperty?: number"));
            Assert.IsTrue(output.Contains("LongField?: number"));
            Assert.IsTrue(output.Contains("Property1?: ToTypeScript.Test.Common.Enum1"));
        }
    }
}