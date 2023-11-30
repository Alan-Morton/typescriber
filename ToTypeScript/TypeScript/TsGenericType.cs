namespace ToTypeScript.TypeScript
{
    /// <summary>
    /// generic TypeScript type
    /// </summary>
    public sealed class TsGenericType : TsType
    {
        public IList<TsType> TypeArguments { get; private set; }

        public TsGenericType(TsName name) : base(name)
        {
            TypeArguments = new List<TsType>();
        }
    }
}
