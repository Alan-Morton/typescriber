using System.Reflection;
using System.Text;
using ToTypeScript.Readers;
using ToTypeScript.TypeScript;

namespace ToTypeScript
{
    public partial class TypeScriber
    {
        // constructor 
        public TypeScriber()
        {
            Types = new HashSet<TsType>();
            TypeLookup = new Dictionary<Type, TsType>()
            {                
                { typeof(byte), TsPrimitive.Number },
                { typeof(short), TsPrimitive.Number },              
                { typeof(int), TsPrimitive.Number },
                { typeof(long), TsPrimitive.Number },
                { typeof(float), TsPrimitive.Number },
                { typeof(double), TsPrimitive.Number },
                { typeof(void), TsPrimitive.Void },
                { typeof(object), TsPrimitive.Any },
                { typeof(string), TsPrimitive.String },
                { typeof(bool), TsPrimitive.Boolean },

            };
            RegisterTypeMapping(CreateGenericDictionaryType(), typeof(Dictionary<,>));
            RegisterTypeMapping(TsPrimitive.Any, typeof(ValueType));
            Formatter = new TsFormatter();
            Reader = new TypeReader();
        }

        /// <summary>
        /// Gets current output writer 
        /// </summary>
        /// <returns>The scripter output</returns>
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            foreach (TsModule? module in Modules().OrderBy(x => x.Name))
                str.Append(value: Formatter?.Format(module));
            return str.ToString();
        }

        /// <summary>
        /// config reader
        /// </summary>
        /// <param name="reader">The type reader</param>
        /// <returns>The scripter</returns>
        public TypeScriber UsingTypeReader(TypeReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");
            Reader = reader;
            return this;
        }

        public TypeScriber AddType(TsType tsType)
        {
            Types?.Add(tsType);
            return this;
        }

        private TypeScriber AddType(TsType tsType, Type type)
        {
            AddType(tsType);
            RegisterTypeMapping(tsType, type);
            return this;
        }

        private void RegisterTypeMapping(TsType tsType, Type type)
        {
            TypeLookup[type] = tsType;
        }

        public TypeScriber WithTypeMapping(TsType tsType, Type type)
        {
            if (TypeLookup.ContainsKey(type))
                throw new ArgumentException("Mapping: " + type.FullName + " already defined.", "type");
            TypeLookup[type] = tsType;
            return this;
        }

        public TypeScriber AddType(Type type)
        {
            Resolve(type);
            return this;
        }

        public TypeScriber AddTypes(IEnumerable<Type> types)
        {
            foreach (Type type in types)
                AddType(type);
            return this;
        }

        public TypeScriber AddTypes(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");
            return AddTypes(new[] { assembly });
        }

        public TypeScriber AddTypes(IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null)
                throw new ArgumentNullException("assemblies");
            foreach (Assembly assembly in assemblies)
                AddTypes(Reader.GetTypes(assembly).Select(x => x.AsType()));
            return this;
        }

        public TypeScriber UsingAssembly(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");
            return UsingAssemblies(new[] { assembly });
        }

        public TypeScriber UsingAssemblies(IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null)
                throw new ArgumentNullException("assemblies");
            HashSet<Assembly> assembliesLookup = new HashSet<Assembly>(assemblies);
            return UsingAssemblyFilter(x => assembliesLookup.Contains(x));
        }

        public TypeScriber UsingAssemblyFilter(Func<Assembly, bool> filter)
        {
            AssemblyFilter = filter;
            return this;
        }

        public TypeScriber UsingTypeFilter(Func<Type, bool> filter)
        {
            TypeFilter = filter;
            return this;
        }

        private TsInterface GenerateInterface(Type type)
        {
            TypeInfo typeInfo = type.GetTypeInfo();
            TsInterface tsInterface = new TsInterface(GetName(type));
            AddType(tsInterface, type);
            IEnumerable<Type> interfaces = typeInfo.BaseType == null ? typeInfo.GetInterfaces() : typeInfo.GetInterfaces().Except(typeInfo.BaseType.GetTypeInfo().GetInterfaces());
            foreach (Type interfaceType in interfaces)
                AddType(interfaceType);
            if (typeInfo.IsGenericType)
            {
                if (typeInfo.IsGenericTypeDefinition)
                {
                    foreach (Type genericArgument in typeInfo.GetGenericArguments())
                    {
                        TsTypeParameter tsTypeParameter = new TsTypeParameter(new TsName(genericArgument.Name));
                        GenericArgumentsMethod(genericArgument, tsTypeParameter);
                        tsInterface.TypeParameters.Add(tsTypeParameter);
                    }
                }
                else
                {
                    Type genericType = type.GetGenericTypeDefinition();
                    TsType tsGenericType = Resolve(genericType);
                }
            }
            if (typeInfo.BaseType != null)
            {
                TsType baseType = Resolve(typeInfo.BaseType);
                if (baseType != null && baseType != TsPrimitive.Any)
                    tsInterface.BaseInterfaces.Add(baseType);
            }
            foreach (FieldInfo field in Reader.GetFields(typeInfo)) //Reader
            {
                TsProperty tsProperty = Resolve(field);
                if (tsProperty != null)
                    tsInterface.Properties.Add(tsProperty);
            }
            foreach (PropertyInfo property in Reader.GetProperties(typeInfo)) //Reader
            {
                TsProperty tsProperty = Resolve(property);
                if (tsProperty != null)
                    tsInterface.Properties.Add(tsProperty);
            }
            foreach (MethodInfo method in Reader.GetMethods(typeInfo)) //Reader
            {
                TsFunction tsFunction = Resolve(method);
                if (tsFunction != null)
                    tsInterface.Functions.Add(tsFunction);
            }
            return tsInterface;
        }

        private void GenericArgumentsMethod(Type genericArgument, TsTypeParameter tsTypeParameter)
        {
            Type? genericArgumentType = genericArgument.GetTypeInfo().GetGenericParameterConstraints().FirstOrDefault();
            if (genericArgumentType != null)
            {
                TsType tsTypeParameterType = Resolve(genericArgumentType);
                tsTypeParameter.Extends = tsTypeParameterType.Name;
            }
        }

        private TsEnum GenerateEnum(Type type)
        {
            TypeInfo typeInfo = type.GetTypeInfo();
            string[] names = typeInfo.GetEnumNames();
            Array values = typeInfo.GetEnumValues();
            Dictionary<string, long?> entries = new Dictionary<string, long?>();
            for (int i = 0; i < values.Length; i++)
                entries.Add(names[i], Convert.ToInt64(values.GetValue(i)));
            TsEnum tsEnum = new TsEnum(GetName(type), entries);
            AddType(tsEnum, type);
            return tsEnum;
        }

        protected virtual TsName GetName(Type type)
        {
            const char genericNameSymbol = '`';
            TypeInfo typeInfo = type.GetTypeInfo();
            string typeName = type.Name;
            if (typeInfo.IsGenericType)
            {
                if (typeName.Contains(genericNameSymbol))
                    typeName = typeName.Substring(0, typeName.IndexOf(genericNameSymbol));
            }
            return new TsName(typeName, type.Namespace);
        }

        protected virtual TsName GetName(ParameterInfo parameter) => new TsName(parameter.Name);

        protected virtual TsName GetName(MemberInfo member) => new TsName(member.Name);

        protected TsType Resolve(Type type)
        {
            // see if we have already processed the type
            TsType tsType;
            if (!TypeLookup.TryGetValue(type, out tsType))
                tsType = OnResolve(type);
            AddType(tsType, type);
            return tsType;
        }

        protected virtual TsType OnResolve(Type type)
        {
            TypeInfo typeInfo = type.GetTypeInfo();
            TsType tsType;
            if (TypeLookup.TryGetValue(type, out tsType)) 
                return tsType;
            else if (AssemblyFilter != null && !AssemblyFilter(typeInfo.Assembly)) 
                tsType = TsPrimitive.Any;
            else if (TypeFilter != null && !TypeFilter(type))
                tsType = TsPrimitive.Any;
            else if (type.IsGenericParameter)
                tsType = new TsGenericType(new TsName(type.Name));
            else if (typeInfo.IsGenericType && !typeInfo.IsGenericTypeDefinition)            
                tsType = BuilTsTypeFunction(type);            
            else if (type.IsArray && type.HasElementType)            
                tsType = BuildElementFunction(type);            
            else if (typeInfo.IsEnum)
                tsType = GenerateEnum(type);
            else if (typeInfo.IsAnsiClass)
                tsType = GenerateInterface(type);
            else if (typeInfo.IsInterface)
                tsType = GenerateInterface(type);
            else
                tsType = TsPrimitive.Any;
            return tsType;
        }

        private TsType BuildElementFunction(Type type)
        {
            TsType tsType;
            TsType elementType = Resolve(type.GetElementType());
            tsType = new TsArray(elementType, type.GetArrayRank());
            return tsType;
        }

        private TsType BuilTsTypeFunction(Type type)
        {
            TsType tsType;
            TsType tsGenericTypeDefinition = Resolve(type.GetGenericTypeDefinition());
            TsGenericType tsGenericType = new TsGenericType(tsGenericTypeDefinition.Name);
            Type[] array = type.GetTypeInfo().GetGenericArguments();
            for (int i = 0; i < array.Length; i++)
            {
                Type argument = array[i];
                TsType tsArgType = Resolve(argument);
                tsGenericType.TypeArguments.Add(tsArgType);
            }
            tsType = tsGenericType;
            return tsType;
        }

        protected virtual TsProperty Resolve(FieldInfo field)
        {
            TsType propertyType;
            bool optional = false;
            TypeInfo fieldTypeInfo = field.FieldType.GetTypeInfo();
            if (fieldTypeInfo.IsGenericType && fieldTypeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Type[] genericArguments = fieldTypeInfo.GetGenericArguments();
                propertyType = Resolve(genericArguments[0]);
                optional = true;
            }
            else
            {
                propertyType = Resolve(field.FieldType);
            }
            return new TsProperty(GetName(field), propertyType, optional);
        }

        protected virtual TsProperty Resolve(PropertyInfo property)
        {
            TsType propertyType;
            bool optional = false;
            TypeInfo propertyTypeInfo = property.PropertyType.GetTypeInfo();
            if (propertyTypeInfo.IsGenericType && propertyTypeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Type[] genericArguments = propertyTypeInfo.GetGenericArguments();
                propertyType = Resolve(genericArguments[0]);
                optional = true;
            }
            else if (propertyTypeInfo.IsGenericType && !propertyTypeInfo.IsGenericTypeDefinition && propertyTypeInfo.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                // find type dictionary values
                List<Type> genericArguments = propertyTypeInfo.GetGenericArguments().ToList();
                Type keyType = genericArguments.First();
                Type valueType = genericArguments.Last();
                // non linq, AM ToDo I have had some issues when linq failed and had to use generic [] unsure why 06-09-2022
                //Type[] genericArguments = propertyTypeInfo.GetGenericArguments();
                //Type keyType = genericArguments[0];
                //Type valueType = genericArguments[1];
                TsType tsKeyType = Resolve(keyType);
                TsType tsArgType = Resolve(valueType);
                TsInterface inlineInterfaceType = new TsInterface();
                inlineInterfaceType.IndexerProperties.Add(new TsIndexerProperty(new TsName("key"), tsKeyType, tsArgType));
                //ToDo Test this
                //inlineInterfaceType.IndexerProperties.Add(new TsIndexerProperty(new TsName("key"), Resolve(genericArguments.First()), Resolve(genericArguments.Last())));
                propertyType = inlineInterfaceType;
            }
            else
            {
                propertyType = Resolve(property.PropertyType);
            }
            return new TsProperty(GetName(property), propertyType, optional);
        }

        protected virtual TsFunction Resolve(MethodInfo method)
        {
            TsType returnType = Resolve(method.ReturnType);
            IEnumerable<ParameterInfo> parameters = Reader.GetParameters(method); //Reader
            TsFunction tsFunction = new TsFunction(GetName(method));
            tsFunction.ReturnType = returnType;
            if (method.IsGenericMethod)
            {
                foreach (Type genericArgument in method.GetGenericArguments())
                {
                    TsTypeParameter tsTypeParameter = new TsTypeParameter(new TsName(genericArgument.Name));
                    tsFunction.TypeParameters.Add(tsTypeParameter);
                }
            }
            foreach (TsParameter? param in parameters.Select(x => new TsParameter(GetName(x), Resolve(x.ParameterType))))
                tsFunction.Parameters.Add(param);
            return tsFunction;
        }

        public IEnumerable<TsModule> Modules()
        {
            return Types.GroupBy(x => x.Name.Namespace).Where(x => !string.IsNullOrEmpty(x.Key)).Select(x => new TsModule(new TsName(x.Key), x));
        }

        public TypeScriber UsingFormatter(TsFormatter formatter)
        {
            Formatter = formatter;
            return this;
        }

        public TypeScriber SaveToFile(string file)
        {
            File.WriteAllText(file, ToString());
            return this;
        }

        public TypeScriber SaveToDirectory(string directory)
        {
            StringBuilder includeContent = new StringBuilder();
            string includeRef = string.Format("/// <reference path=\"include.ts\" />{0}", Environment.NewLine);
            foreach (TsModule module in Modules())
            {
                string fileName = module.Name.FullName + ".d.ts";
                string path = Path.Combine(directory, fileName);
                string output = Formatter.Format(module); // Formatter
                File.WriteAllText(path, includeRef + Environment.NewLine + output);
                includeContent.AppendFormat("/// <reference path=\"{0}\" />", fileName);                
                includeContent.AppendLine();
            }          
            File.WriteAllText(Path.Combine(directory, "include.ts"), includeContent.ToString());
            return this;
        }

        //testing only
        //public override bool Equals(object? obj)
        //{
        //    return obj is TypeScriber scriber &&
        //           EqualityComparer<Dictionary<Type, TsType>>.Default.Equals(TypeLookup, scriber.TypeLookup) &&
        //           EqualityComparer<HashSet<TsType>>.Default.Equals(Types, scriber.Types) &&
        //           EqualityComparer<Func<Assembly, bool>>.Default.Equals(AssemblyFilter, scriber.AssemblyFilter) &&
        //           EqualityComparer<Func<Type, bool>>.Default.Equals(TypeFilter, scriber.TypeFilter) &&
        //           EqualityComparer<TypeReader>.Default.Equals(Reader, scriber.Reader) &&
        //           EqualityComparer<TsFormatter>.Default.Equals(Formatter, scriber.Formatter);
        //}
    }
}