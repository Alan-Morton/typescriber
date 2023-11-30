using System.Reflection;
using ToTypeScript.Readers;
using ToTypeScript.TypeScript;

namespace ToTypeScript
{
    public partial class TypeScriber
    {
        private Dictionary<Type, TsType> TypeLookup { get; set; }

        private HashSet<TsType> Types { get; set; }

        private Func<Assembly, bool> AssemblyFilter { get; set; }

        private Func<Type, bool> TypeFilter { get; set; }

        private TypeReader Reader { get; set; }

        private TsFormatter Formatter { get; set; }
    }
}
