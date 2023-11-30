namespace ToTypeScript.TypeScript
{
    /// <summary>
    /// A class for ex: { [key:string]: any} 
    /// </summary>
	public class TsIndexerProperty : TsObject
    {
        public TsType IndexerType { get; set; }

	    public TsType ReturnType { get; set; }

        public TsIndexerProperty(TsName name, TsType indexerType, TsType returnType) : base(name)
        {
            IndexerType = indexerType;
            ReturnType = returnType;
        }
    }
}