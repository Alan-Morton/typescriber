namespace ToTypeScript.TypeScript
{
    /// <summary>
    /// TypeScript function
    /// </summary>
    public sealed class TsFunction : TsType
    {
        public TsType ReturnType { get; set; }

        public IList<TsParameter> Parameters { get; private set; }

        public IList<TsTypeParameter> TypeParameters { get; private set; }

        public TsFunction(TsName name) : base(name)
        {
            ReturnType = TsPrimitive.Void;
            Parameters = new List<TsParameter>();
            TypeParameters = new List<TsTypeParameter>();
        }
    }
}
