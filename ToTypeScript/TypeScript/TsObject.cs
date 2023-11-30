namespace ToTypeScript.TypeScript
{
    /// <summary>
    /// A base class for all TypeScript obj
    /// </summary>
    public abstract class TsObject
    {
        public TsName Name { get; private set; }

        protected TsObject(TsName name)
        {
            Name = name;
        }

        public override string ToString()
        {
            TsName name = Name;
            if (name != null)
                return name.ToString();
            return base.ToString();
        }
    }
}
