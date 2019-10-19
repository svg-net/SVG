using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace Svg
{
    internal static class Utility
    {
        public static string GetUrlString(string url)
        {
            url = url.Trim();
            if (url.StartsWith("url(", StringComparison.OrdinalIgnoreCase) && url.EndsWith(")"))
            {
                url = new StringBuilder(url).Remove(url.Length - 1, 1).Remove(0, 4).ToString().Trim();

                if ((url.StartsWith("\"") && url.EndsWith("\"")) || (url.StartsWith("'") && url.EndsWith("'")))
                    url = new StringBuilder(url).Remove(url.Length - 1, 1).Remove(0, 1).ToString().Trim();
            }
            return url;
        }
        public static FileData GetBytesFromUri(Uri uri)
        {
            if (uri.IsAbsoluteUri && uri.Scheme == "data")
                return GetBytesFromDataUri(uri.ToString());
            var request = WebRequest.Create(uri);
            using (var response = request.GetResponse())
            {
                var responseStream = response.GetResponseStream();
                if (responseStream.CanSeek)
                    responseStream.Position = 0;

                byte[] receiveBytes = new byte[1024 * 100];
                var ms = new MemoryStream();
                var size = 0;
                do
                {
                    size = responseStream.Read(receiveBytes, 0, receiveBytes.Length);
                    ms.Write(receiveBytes, 0, size);
                } while (size > 0);
                return new FileData(ms.GetBuffer(), response.ContentType, null);
            }
        }
        public static FileData GetBytesFromDataUri(string uriString)
        {
            var headerStartIndex = 5;
            var headerEndIndex = uriString.IndexOf(",", headerStartIndex);
            if (headerEndIndex < 0 || headerEndIndex + 1 >= uriString.Length)
                throw new Exception("Invalid data URI");

            var mimeType = "text/plain";
            var charset = System.Text.Encoding.ASCII;
            var base64 = false;

            var headers = new List<string>(uriString.Substring(headerStartIndex, headerEndIndex - headerStartIndex).Split(';'));
            if (headers[0].Contains("/"))
            {
                mimeType = headers[0].Trim();
                headers.RemoveAt(0);
            }

            if (headers.Count > 0 && headers[headers.Count - 1].Trim().Equals("base64", StringComparison.InvariantCultureIgnoreCase))
            {
                base64 = true;
                headers.RemoveAt(headers.Count - 1);
            }

            foreach (var param in headers)
            {
                var p = param.Split('=');
                if (p.Length < 2)
                    continue;

                var attribute = p[0].Trim();
                if (attribute.Equals("charset", StringComparison.InvariantCultureIgnoreCase))
                    charset = System.Text.Encoding.GetEncoding(p[1].Trim());
            }

            var data = uriString.Substring(headerEndIndex + 1);
            var dataBytes = base64 ? Convert.FromBase64String(data) : (charset ?? Encoding.UTF8).GetBytes(data);
            return new FileData(dataBytes, mimeType, charset);
        }
    }
    internal class FileData
    {
        public FileData(byte[] dataBytes, string mimeType, Encoding charset)
        {
            this.Charset = charset;
            this.MimeType = mimeType;
            this.DataBytes = dataBytes;
        }
        public Encoding Charset { get; }
        public string MimeType { get; }
        public byte[] DataBytes { get; }
    }
}