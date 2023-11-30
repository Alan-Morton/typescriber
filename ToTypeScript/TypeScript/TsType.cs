namespace ToTypeScript.TypeScript
{
    /// <summary>
    /// TypeScript types
    /// </summary>
    public abstract class TsType : TsObject
    {

        protected TsType(TsName name) : base(name)
        {
        }
        public override string ToString()
        {
            return this.Name.FullName;
        }
    }
}
