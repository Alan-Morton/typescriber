using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToTypeScript.TypeScript
{
    /// <summary>
    /// A class representing a TypeScript name
    /// </summary>
    public class TsName : IComparable<TsName>
    {
        public static readonly TsName None = new TsName(string.Empty);

        public string Namespace { get; set; }

        public string Name { get; set; }

        public string FullName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Namespace))
                    return string.Format("{0}.{1}", this.Namespace, this.Name);
                else
                    return Name;
            }
        }

        public TsName(string tsName)
        {
            Name = tsName;
        }

        public TsName(string tsName, string tsNamespace) : this(tsName)
        {
            Namespace = tsNamespace;
        }

        public override string ToString()
        {
            return FullName;
        }

        public int CompareTo(TsName other)
        {
            return FullName.CompareTo(other.FullName);
        }
    }
}
