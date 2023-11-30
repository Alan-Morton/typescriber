namespace ToTypeScript.TypeScript
{
    /// <summary>
    /// A TypeScript property
    /// </summary>
    public class TsProperty : TsObject
    {
        public bool Optional { get; set; }

        public TsType Type { get; set; }

        public TsProperty(TsName name, TsType type, bool optional = false) : base(name)
        {
            Type = type;
            Optional = optional;
        }
    }
}
