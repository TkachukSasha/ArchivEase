using System.IO.Compression;

namespace Core.Helper;

public static class JsonResolverCoding
{
    public static byte[] Compress<T>(T input)
    {
        byte[] inputBytes = SerializeToBytes(input);

        using (MemoryStream outputStream = new MemoryStream())
        {
            using (GZipStream gzipStream = new GZipStream(outputStream, CompressionMode.Compress))
            {
                gzipStream.Write(inputBytes, 0, inputBytes.Length);
            }

            return outputStream.ToArray();
        }
    }

    public static T Decompress<T>(byte[] inputBytes)
    {
        using (MemoryStream inputStream = new MemoryStream(inputBytes))
        {
            using (MemoryStream outputStream = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                {
                    gzipStream.CopyTo(outputStream);
                }

                byte[] outputBytes = outputStream.ToArray();

                return DeserializeFromBytes<T>(outputBytes);
            }
        }
    }

    static byte[] SerializeToBytes<T>(T obj)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
            serializer.WriteObject(ms, obj);
            return ms.ToArray();
        }
    }

    static T DeserializeFromBytes<T>(byte[] bytes)
    {
        using (MemoryStream ms = new MemoryStream(bytes))
        {
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
            return (T)serializer.ReadObject(ms);
        }
    }
}
