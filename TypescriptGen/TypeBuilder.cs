using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TypeGen.FileTypes;

namespace TypeGen
{
    public class TypeBuilder
    {
        private readonly Type[] _allTypes;
        public TsDir RootDir { get; set; }
        
        public Dictionary<Type, ClassFile> ClassFiles { get; } = new Dictionary<Type, ClassFile>();
        public Dictionary<Type, EnumFile> EnumFiles { get; } = new Dictionary<Type, EnumFile>();
        public Dictionary<Type, InterfaceFile> InterfaceFiles { get; } = new Dictionary<Type, InterfaceFile>();

        public List<UnionTypeDefinition> UnionTypeDefinitions { get; } = new List<UnionTypeDefinition>();

        public static bool LineBetweenProperties { get; set; } = false;
        public static string DefaultExtension { get; set; } = ".ts";
        public static string TickStile { get; set; } = "'";
        public List<Decorator> DefaultClassPropertyDecorators { get; } = new List<Decorator>();

        public TypeBuilder(params Assembly[] additionalAssemblies)
        {
            RootDir = TsDir.Create(Directory.GetCurrentDirectory()).Down("Types");

            _allTypes = Assembly.GetExecutingAssembly()
                .GetReferencedAssemblies()
                .Select(Assembly.Load)
                .Union(new[]
                {
                    Assembly.GetExecutingAssembly(),
                    Assembly.GetCallingAssembly(),
                    Assembly.GetEntryAssembly(),
                })
                .Union(Assembly.GetCallingAssembly().GetReferencedAssemblies().Select(Assembly.Load))
                .Union(Assembly.GetEntryAssembly().GetReferencedAssemblies().Select(Assembly.Load))
                .Union(additionalAssemblies)
                .SelectMany(x => x.GetTypes())
                .ToArray();

            //todo move all .net types into definitions, to do this we need a way to indicate a type that doesn't necessarily need to be imported. That way a user could swap out all instances of a type (like DateTime) with a specific typescript/javascript type or even a type from a library import. Should also change it so that a type can have multiple associated files with it, files would associate back to a type so you could do file->type->files[]
        }

        public TypedFile Type<TType>()
        {
            return Type(typeof(TType));
        }

        public TypedFile Type(Type type)
        {
            while (true)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    type = type.GenericTypeArguments[0];
                    continue;
                }

                if (type.IsEnum) return Enum(type);

                if (type.IsInterface || InterfaceFiles.ContainsKey(type)) return Interface(type);

                return Class(type);
                break;
            }
        }

        public void Types(params Func<Type, bool>[] filters)
        {
            foreach (var type in _allTypes.Where(t => filters.Any(f => f(t))))
            {
                Type(type);
            }
        }

        public ClassFile Class<TType>()
        {
            var type = typeof(TType);
            return Class(type);
        }

        public ClassFile Class(Type type)
        {
            if (!ClassFiles.ContainsKey(type))
                ClassFiles[type] = new ClassFile(this, type, RootDir);

            return ClassFiles[type];
        }

        public void Classes(params Func<Type, bool>[] filters)
        {
            foreach (var type in _allTypes.Where(t => filters.Any(f => f(t))))
            {
                Class(type);
            }
        }

        public EnumFile Enum(Type type)
        {
            if (!EnumFiles.ContainsKey(type))
                EnumFiles[type] = new EnumFile(type, RootDir);

            return EnumFiles[type];
        }

        public InterfaceFile Interface<TType>()
        {
            var type = typeof(TType);
            return Interface(type);
        }

        /// <summary>
        /// Adds a <see cref="InterfaceFile"/> to be used to generate an interface (classes can be used here if you want to generate a typescript interface from a c# class)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="shouldCreate"></param>
        /// <returns>The created <see cref="InterfaceFile"/> so you can make changes like adjusting property names or values.</returns>
        public InterfaceFile Interface(Type type)
        {
            if (InterfaceFiles.ContainsKey(type))
                return InterfaceFiles[type];
            InterfaceFiles[type] = new InterfaceFile(this, type, RootDir);

            return InterfaceFiles[type];
        }

        /// <summary>
        /// types are checked against the passed in filters to determine if a typescript interface should be generated
        /// </summary>
        /// <param name="filters"></param>
        public void Interfaces(params Func<Type, bool>[] filters)
        {
            foreach (var type in _allTypes.Where(t => filters.Any(f => f(t))))
            {
                Interface(type);
            }
        }

        /// <summary>
        /// Creates a Typescript type union
        /// </summary>
        /// <param name="name">The name used for the type</param>
        /// <param name="directory">The <see cref="TsDir"/> where the resulting file should be placed.</param>
        /// <param name="filters">The filters used to find the types used in the union.</param>
        /// <returns></returns>
        public UnionTypeDefinition UnionType(string name, TsDir directory = null, params Func<Type, bool>[] filters)
        {
            var typesForUnion = new List<TypedFile>();
            foreach (var type in _allTypes.Where(t => filters.Any(f => f(t))))
            {
                TypedFile typeForUnion;

                if (InterfaceFiles.ContainsKey(type))
                    typeForUnion = Interface(type);
                else if (ClassFiles.ContainsKey(type))
                    typeForUnion = Class(type);
                else if (type.IsInterface)
                    typeForUnion = Interface(type);
                else
                    typeForUnion = Class(type);

                typesForUnion.Add(typeForUnion);
            }

            return UnionType(name, directory, typesForUnion.ToArray());
        }

        public UnionTypeDefinition UnionType(string name, TsDir directory = null, params TypedFile[] files)
        {
            var unionTypeDefinition = new UnionTypeDefinition(name, directory ?? RootDir);

            unionTypeDefinition.TypesForUnion.AddRange(files);

            UnionTypeDefinitions.Add(unionTypeDefinition);

            return unionTypeDefinition;
        }

        public void WriteAllFiles()
        {
            foreach (var cd in ClassFiles.Values)
            {
                if (!Directory.Exists(cd.Directory.ToPath()))
                    Directory.CreateDirectory(cd.Directory.ToPath());
                File.WriteAllText(cd.FilePath, cd);
            }

            foreach (var cd in InterfaceFiles.Values)
            {
                if (!Directory.Exists(cd.Directory.ToPath()))
                    Directory.CreateDirectory(cd.Directory.ToPath());
                File.WriteAllText(cd.FilePath, cd);
            }

            foreach (var cd in EnumFiles.Values)
            {
                if (!Directory.Exists(cd.Directory.ToPath()))
                    Directory.CreateDirectory(cd.Directory.ToPath());
                File.WriteAllText(cd.FilePath, cd);
            }

            foreach (var cd in UnionTypeDefinitions)
            {
                if (!Directory.Exists(cd.Directory.ToPath()))
                    Directory.CreateDirectory(cd.Directory.ToPath());
                File.WriteAllText(cd.FilePath, cd);
            }
        }
    }
}