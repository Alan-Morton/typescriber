using ToTypeScript.TypeScript;

namespace ToTypeScript
{
    public partial class TypeScriber
    {
        private TsType CreateGenericDictionaryType()
        {
            TsInterface tsInterface = new TsInterface(new TsName("Dictionary", "System.Collections.Generic"));
            //TsInterface tsKeyType = new TsInterface(new TsName("TKey"));
            TsInterface tsValueType = new TsInterface(new TsName("TValue"));
            tsInterface.TypeParameters.Add(new TsTypeParameter(new TsName("Tkey")));
            tsInterface.TypeParameters.Add(new TsTypeParameter(new TsName("TValue")));
            tsInterface.IndexerProperties.Add(new TsIndexerProperty(new TsName("key"), TsPrimitive.String, tsValueType));
            return tsInterface;
        }
    }
}
