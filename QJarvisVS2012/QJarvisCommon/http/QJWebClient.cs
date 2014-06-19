using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace com.quadramma.qjarvis.http
{
    public class QJWebClient
    {


        public static QJUploadFileResponse UploadFile(string address, string fileInputName, string filename, byte[] fileBytes, NameValueCollection data = null)
        {
            if (data == null)
            {
                data = new NameValueCollection();
            }
            var files = new[] 
            {
                new QJUploadFile
                {
                    Name = fileInputName,
                    Filename = Path.GetFileName(filename),
                    ContentType = "text/plain",
                    Stream = new MemoryStream(fileBytes)
                }
            };
            data["fileInputName"] = fileInputName; //QJarvis PHP read this for file name.
            var json = Encoding.Default.GetString(UploadFiles(address, files, data));
            QJUploadFileResponse stuff = new JavaScriptSerializer().Deserialize<QJUploadFileResponse>(json);
            return stuff;
        }

        
          
        public static byte[] UploadFiles(string address, IEnumerable<QJUploadFile> files, NameValueCollection values)
        {
            var request = WebRequest.Create(address);
            request.Method = "POST";
            var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            boundary = "--" + boundary;

            using (var requestStream = request.GetRequestStream())
            {
                // Write the values
                foreach (string name in values.Keys)
                {
                    var buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.ASCII.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{1}", name, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.UTF8.GetBytes(values[name] + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                }

                // Write the files
                foreach (var file in files)
                {
                    var buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"{2}", file.Name, file.Filename, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.ASCII.GetBytes(string.Format("Content-Type: {0}{1}{1}", file.ContentType, Environment.NewLine));
                    requestStream.Write(buffer, 0, buffer.Length);
                    file.Stream.CopyTo(requestStream);
                    buffer = Encoding.ASCII.GetBytes(Environment.NewLine);
                    requestStream.Write(buffer, 0, buffer.Length);
                }

                var boundaryBuffer = Encoding.ASCII.GetBytes(boundary + "--");
                requestStream.Write(boundaryBuffer, 0, boundaryBuffer.Length);
            }

            using (var response = request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var stream = new MemoryStream())
            {
                responseStream.CopyTo(stream);
                return stream.ToArray();
            }
        }
        public static byte[] GetFileByteArray(string filename)
        {
            FileStream oFileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            byte[] FileByteArrayData = new byte[oFileStream.Length];
            oFileStream.Read(FileByteArrayData, 0, System.Convert.ToInt32(oFileStream.Length));
            oFileStream.Close();
            return FileByteArrayData; 
        }
    }
}
