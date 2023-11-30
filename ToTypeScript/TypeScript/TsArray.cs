using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToTypeScript.TypeScript
{
    /// <summary>
    /// TypeScript Array
    /// </summary>
    public sealed class TsArray : TsPrimitive
    {

        public TsArray(TsType elementType, int dimensions) : base(new TsName(elementType.Name.Name + GenerateArrayNotation(dimensions), elementType.Name.Namespace))
        {
        }

        private static string GenerateArrayNotation(int dimensions)
        {
            string notation = "[]";
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < dimensions; i++)
                str.Append(notation);
            return str.ToString();
        }
    }
}
