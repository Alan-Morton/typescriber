using System.Text;

namespace ToTypeScript.TypeScript
{
    /// <summary>
    /// TypeScript output
    /// </summary>
    public class TsFormatter
    {
        private class StringBuilderContext : IDisposable
        {
            StringBuilderContext PriorContext { get; set; }

            public StringBuilder StringBuilder { get; set; }

            public int IndentLevel { get; set; }

            TsFormatter Writer { get; set; }

            public StringBuilderContext(TsFormatter writer)
            {
                Writer = writer;
                PriorContext = writer.Context;
                IndentLevel = PriorContext != null ? PriorContext.IndentLevel : 0;
                StringBuilder = new StringBuilder();
                Writer.Context = this;
            }

            public override string ToString()
            {
                return StringBuilder.ToString();
            }

            void IDisposable.Dispose()
            {
                if (PriorContext != null)
                {
                    Writer.Context = PriorContext;
                    PriorContext = null;
                }
            }
        }

        private class IndentContext : IDisposable
        {
            private TsFormatter mFormatter;
            public IndentContext(TsFormatter formatter)
            {
                mFormatter = formatter;
                mFormatter.Context.IndentLevel++;
            }

            void IDisposable.Dispose()
            {
                if (mFormatter != null)
                {
                    mFormatter.Context.IndentLevel--;
                    mFormatter = null;
                }
            }
        }

        private StringBuilderContext Context { get; set; }

        public IDictionary<string, string> ReservedWordsMapping { get; private set; }

        public bool EnumsAsString { get; set; }
        
        public TsFormatter()
        {
            Context = new StringBuilderContext(this);
            ReservedWordsMapping = new Dictionary<string, string>()
            {
                {"function","_function"}
            };
        }

        public virtual string Format(TsModule module)
        {
            using (StringBuilderContext sbc = new StringBuilderContext(this))
            {
                WriteIndent();
                Write("declare module {0} {{", Format(module.Name));
                WriteNewline();
                using (Indent())
                {
                    foreach (TsEnum? type in module.Types.OfType<TsEnum>().OrderBy(x => x.Name))
                        Write(Format(type));
                    foreach (TsInterface? type in module.Types.OfType<TsInterface>().OrderBy(x => x.Name))
                        Write(Format(type));
                }
                WriteIndent();
                Write("}");
                WriteNewline();
                return sbc.ToString();
            }
        }

        public virtual string Format(TsInterface tsInterface)
        {
            using (StringBuilderContext sbc = new StringBuilderContext(this))
            {
                if (tsInterface.IsLiteral)
                {
                    Write("{");
                    foreach (TsProperty? property in tsInterface.Properties.OrderBy(x => x.Name))
                        Write(Format(property));
                    foreach (TsIndexerProperty? property in tsInterface.IndexerProperties.OrderBy(x => x.Name))
                        Write(Format(property));
                    foreach (TsFunction? function in tsInterface.Functions.OrderBy(x => x.Name))
                        Write(Format(function));
                    Write("}");
                    return sbc.ToString();
                }
                else
                {
                    WriteIndent();
                    Write("interface {0}{1} {2} {{",
                        Format(tsInterface.Name),
                        Format(tsInterface.TypeParameters),
                        tsInterface.BaseInterfaces.Count > 0 ? string.Format("extends {0}", string.Join(", ", tsInterface.BaseInterfaces.OrderBy(x => x.Name).Select(Format))) : string.Empty);
                    WriteNewline();
                    using (Indent())
                    {
                        foreach (TsProperty? property in tsInterface.Properties.OrderBy(x => x.Name))
                        {
                            WriteIndent();
                            Write(Format(property));
                            WriteNewline();
                        }
                        foreach (TsIndexerProperty? property in tsInterface.IndexerProperties.OrderBy(x => x.Name))
                        {
                            WriteIndent();
                            Write(Format(property));
                            WriteNewline();
                        }
                        foreach (TsFunction? function in tsInterface.Functions.OrderBy(x => x.Name))
                        {
                            WriteIndent();
                            Write(Format(function));
                            WriteNewline();
                        }
                    }
                    WriteIndent();
                    Write("}");
                    WriteNewline();
                    WriteNewline();
                    return sbc.ToString();
                }
            }
        }

        public virtual string Format(TsProperty property)
        {
            using (StringBuilderContext sbc = new StringBuilderContext(this))
            {
                Write("{0}{1}: {2};", Format(property.Name), property.Optional?"?":"", Format(property.Type));
                return sbc.ToString();
            }
        }

        public virtual string Format(TsIndexerProperty property)
        {
            using (StringBuilderContext sbc = new StringBuilderContext(this))
            {
                Write("[{0}: {1}]: {2};", Format(property.Name), Format(property.IndexerType), Format(property.ReturnType));
                return sbc.ToString();
            }
        }

        public virtual string Format(TsFunction function)
        {
            using (StringBuilderContext sbc = new StringBuilderContext(this))
            {
                Write("{0}{1}({2}){3};",
                    Format(function.Name),
                    Format(function.TypeParameters),
                    Format(function.Parameters),
                    function.ReturnType == TsPrimitive.Any ? string.Empty : string.Format(": {0}", FormatReturnType(function.ReturnType))
                );
                return sbc.ToString();
            }
        }

        public virtual string FormatReturnType(TsType tsReturnType)
        {
            return Format(tsReturnType);
        }

        public virtual string Format(TsType tsType)
        {
            if (tsType is TsGenericType)
                return Format((TsGenericType)tsType);
            TsInterface? tsInterface = tsType as TsInterface;
            if (tsInterface != null && tsInterface.IsLiteral)
                return Format(tsInterface);
            return tsType.Name.FullName;
        }

        public virtual string Format(TsEnum tsEnum)
        {
            if (EnumsAsString)            
                return FormatEnumAsStrings(tsEnum);            
            else            
                return FormatEnumAsIntegers(tsEnum);            
        }

        protected string FormatEnumAsStrings(TsEnum tsEnum)
        {
            using (StringBuilderContext sbc = new StringBuilderContext(this))
            {
                WriteIndent();
                Write("type {0} = ", Format(tsEnum.Name));
                KeyValuePair<string, long?>[] values = tsEnum.Values.OrderBy(x => x.Key).ToArray();
                for (int i = 0; i < values.Length; i++)
                {
                    string postFix = i < values.Length - 1 ? " | " : string.Empty;
                    KeyValuePair<string, long?> entry = values[i];
                    Write("\'{0}\'{1}", entry.Key, postFix);
                }
                Write(";");
                WriteNewline();
                return sbc.ToString();
            }
        }

        protected string FormatEnumAsIntegers(TsEnum tsEnum)
        {
            using (StringBuilderContext sbc = new StringBuilderContext(this))
            {
                WriteIndent();
                Write("const enum {0} {{", Format(tsEnum.Name));
                WriteNewline();
                using (Indent())
                {
                    KeyValuePair<string, long?>[] values = tsEnum.Values.OrderBy(x => x.Key).ToArray();
                    for (int i = 0; i < values.Length; i++)
                    {
                        string postFix = i < values.Length - 1 ? "," : string.Empty;
                        KeyValuePair<string, long?> entry = values[i];
                        WriteIndent();
                        if (entry.Value.HasValue)
                            Write("{0} = {1}{2}", entry.Key, entry.Value, postFix);
                        else
                            Write("{0}{1}", entry.Key, postFix);
                        WriteNewline();
                    }
                }
                WriteIndent();
                Write("}");
                WriteNewline();
                return sbc.ToString();
            }
        }

        public virtual string Format(TsParameter parameter)
        {
            using (StringBuilderContext sbc = new StringBuilderContext(this))
            {
                Write("{0}{1}: {2}", Format(parameter.Name), parameter.Optional ? "?" : string.Empty, Format(parameter.Type));
                return sbc.ToString();
            }
        }

        public virtual string Format(IEnumerable<TsParameter> parameters)
        {
            return string.Join(", ", parameters.Select(Format));
        }

        public virtual string Format(TsTypeParameter typeParameter)
        {
            return string.Format("{0}{1}", typeParameter.Name, typeParameter.Extends == null ? string.Empty : string.Format(" extends {0}", typeParameter.Extends.FullName));
        }

        public virtual string Format(IEnumerable<TsTypeParameter> typeParameters)
        {
            if (typeParameters.Count() == 0)
                return string.Empty;
            return string.Format("<{0}>", string.Join(", ", typeParameters.Select(Format)));
        }

        public virtual string Format(TsGenericType tsGenericType)
        {
            return string.Format("{0}{1}", tsGenericType.Name.FullName, tsGenericType.TypeArguments.Count > 0 ? string.Format("<{0}>", string.Join(", ", tsGenericType.TypeArguments.Select(Format))) : string.Empty);
        }

        public virtual string Format(TsName name)
        {
            if (name == null || name.Name == null)
                return string.Empty;
            string result = null;
            if (!ReservedWordsMapping.TryGetValue(name.Name, out result))
                result = name.Name;
            return result;
        }

        private void Write(string output)
        {
            Context.StringBuilder.Append(output);
        }

        private void Write(string format, params object[] args)
        {
            Write(string.Format(format, args));
        }

        private void WriteIndent()
        {
            string indent = string.Empty;
            for (int i = 0; i < Context.IndentLevel; i++)
                indent += "\t";
            Write(indent);
        }

        private void WriteNewline()
        {
            Write(Environment.NewLine);
        }

        private IndentContext Indent()
        {
            return new IndentContext(this);
        }
    }
}
