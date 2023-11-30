using System.Text;

namespace ToTypeScript.Tests
{
    /// <summary>
    /// The base class 
    /// </summary>
    public abstract class BaseTest
    {
        protected void ValidateTypeScript(TypeScriber typeScriber)
        {
            ValidateTypeScript(typeScriber.ToString());
        }

        protected void ValidateTypeScript(string typeScript)
        {
            Console.WriteLine(typeScript);
            var result = TypeScriptCompiler.Compile(typeScript);
            Assert.AreEqual(0, result.ReturnCode, result.Output);
        }

        protected void ValidateTypeScript(StringBuilder typeScript)
        {
            ValidateTypeScript(typeScript.ToString());
        }

        protected void AssertNotValidTypeScript(string typeScript)
        {
            Console.WriteLine(typeScript);
            var result = TypeScriptCompiler.Compile(typeScript);
            Assert.AreNotEqual(0, result.ReturnCode, result.Output);
        }
    }
}

