namespace ToTypeScript.TypeScript
{
    public class TsPrimitive : TsType
    {

        public static readonly TsPrimitive Any = new TsPrimitive(new TsName("any"));

        public static readonly TsPrimitive Void = new TsPrimitive(new TsName("void"));

        public static readonly TsPrimitive Boolean = new TsPrimitive(new TsName("boolean"));

        public static readonly TsPrimitive Number = new TsPrimitive(new TsName("number"));

        public static readonly TsPrimitive String = new TsPrimitive(new TsName("string"));

        public static readonly TsPrimitive Undefined = new TsPrimitive(new TsName("undefined"));

        protected TsPrimitive(TsName name) : base(name)
        {
        }
    }
}
