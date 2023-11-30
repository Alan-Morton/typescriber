using System.Reflection;

namespace ToTypeScript
{
    public static class LinqExtention
    {
        public static string ToTypeScriptString<T>()
        {
            TypeScriber typeScriber = new TypeScriber();
            string returnValue = typeScriber.AddType(typeof(T)).ToString();
            return returnValue;
        }

        public static string ToTypeScriptString<T>(this T source)
        {
            TypeScriber typeScriber = new TypeScriber();
            string returnValue = typeScriber.AddType(typeof(T)).ToString();
            return returnValue;
        }
    }
}
