namespace ToTypeScript.TypeScript
{
    /// <summary>
    /// A Ts function
    /// </summary>
    public sealed class TsInterface : TsType
    {
        public IList<TsProperty> Properties { get; private set; }

        public IList<TsIndexerProperty> IndexerProperties { get; private set; }

        public IList<TsFunction> Functions { get; private set; }

        public IList<TsType> BaseInterfaces { get; private set; }

        public IList<TsTypeParameter> TypeParameters { get; private set; }

        public bool IsLiteral { get { return this.Name == TsName.None; } }


        public TsInterface(TsName name) : base(name)
        {
            TypeParameters = new List<TsTypeParameter>();
            BaseInterfaces = new List<TsType>();
            Properties = new List<TsProperty>();
            IndexerProperties = new List<TsIndexerProperty>();
            Functions = new List<TsFunction>();
        }

        public TsInterface() : this(TsName.None)
        {
        }
    }
}
