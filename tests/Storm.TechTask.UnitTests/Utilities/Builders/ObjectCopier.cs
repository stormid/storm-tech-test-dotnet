using System.Runtime.Serialization;

namespace Storm.TechTask.UnitTests.Utilities.Builders
{
    public static class ObjectCopier
    {
        private static readonly Lazy<Type[]> SerializableTypes = new(LoadSerializableTypes);

        private static Type[] LoadSerializableTypes()
        {
            return [.. AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.FullName?.StartsWith("Storm.", StringComparison.OrdinalIgnoreCase) ?? false)
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.GetCustomAttributes(typeof(SerializableAttribute), false).Length > 0)];
        }

        public static T Copy<T>(T source)
            where T : notnull
        {
            DataContractSerializer serializer = new(
                type: source.GetType(),
                knownTypes: SerializableTypes.Value);

            using var stream = new MemoryStream();
            serializer.WriteObject(stream, source);
            stream.Position = 0;
            return (T)serializer.ReadObject(stream)!;
        }
    }

}
