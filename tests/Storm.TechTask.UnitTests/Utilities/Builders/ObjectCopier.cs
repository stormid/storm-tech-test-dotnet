using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Storm.TechTask.UnitTests.Utilities.Builders
{
    public static class ObjectCopier
    {
        public static T Copy<T>(T source)
            where T : notnull
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);

                return (T)formatter.Deserialize(stream);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
            }
        }
    }

}
