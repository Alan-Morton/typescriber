namespace ToTypeScript.TypeScript
{
    /// <summary>
    /// A TypeScript type parameter
    /// </summary>
    public sealed class TsTypeParameter : TsObject
    {
        public TsName Extends { get; set; }

        public TsTypeParameter(TsName name) : base(name)
        {
        }

        public TsTypeParameter(TsName name, TsName extends) : this(name)
        {
            Extends = extends;
        }
    }
}
