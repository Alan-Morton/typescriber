namespace ToTypeScript.TypeScript
{
    /// <summary>
    /// TypeScript enumeration
    /// </summary>
    public sealed class TsEnum : TsType
    {
        public IDictionary<string, long?> Values { get; set; }

        public TsEnum(TsName name, IDictionary<string, long?> values) : base(name)
        {
            Values = values;
        }
    }
}
