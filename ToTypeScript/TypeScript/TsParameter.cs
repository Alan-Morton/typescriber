
namespace ToTypeScript.TypeScript
{
    /// <summary>
    /// A  TypeScript parameter
    /// </summary>
    public sealed class TsParameter : TsObject
    {
        public bool Optional { get; set; }

        public TsType Type { get; set; }

        public TsParameter(TsName name, TsType type) : base(name)
        {
            Type = type;
            Optional = false;
        }
    }
}
