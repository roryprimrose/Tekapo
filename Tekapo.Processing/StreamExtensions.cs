namespace Tekapo.Processing
{
    using System.Globalization;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using EnsureThat;

    public static class StreamExtensions
    {
        public static string CalculateHash(this Stream stream)
        {
            Ensure.Any.IsNotNull(stream, nameof(stream));

            stream.Position = 0;

            using (var algorithm = SHA1.Create())
            {
                var hashBuffer = algorithm.ComputeHash(stream);

                return hashBuffer.ToHex(true);
            }
        }

        private static string ToHex(this byte[] bytes, bool upperCase)
        {
            var result = new StringBuilder(bytes.Length * 2);

            for (var index = 0; index < bytes.Length; index++)
            {
                result.Append(bytes[index].ToString(upperCase ? "X2" : "x2", CultureInfo.InvariantCulture));
            }

            return result.ToString();
        }
    }
}