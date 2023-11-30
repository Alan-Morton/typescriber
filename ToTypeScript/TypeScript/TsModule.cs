namespace ToTypeScript.TypeScript
{
    /// <summary>
    /// when all TypeScript modules
    /// </summary>
    public class TsModule : TsObject
    {
        public IList<TsType> Types { get; private set; }

        public TsModule(TsName name, IEnumerable<TsType> types = null) : base(name)
        {
            Types = types != null ? new List<TsType>(types) : new List<TsType>();
        }
    }
}
