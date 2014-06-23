using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using CodeBetter.Json;


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
            //var jsontres = "{\"ok\":\"1\",\"root\":\"http:\\/\\/www.quadramma.com\\/pruebas\\/qjarvis\\/api\\/file\\/images\",\"filename\":\"image_6199f023168488f2deb72a43dbfa117d_gen.jpg\",\"url\":\"http:\\/\\/www.quadramma.com\\/pruebas\\/qjarvis\\/api\\/file\\/images\\/image_6199f023168488f2deb72a43dbfa117d_gen.jpg\",\"message\":\"Everything work just fine\"}";
            //var jsondos = "{\"ok\":\"1\",\"root\":\"www.quadramma.com/pruebas/qjarvis/api/file\\/images\",\"filename\":\"image_6199f023168488f2deb72a43dbfa117d_gen.jpg\",\"url\":\"http:\\/\\/www.quadramma.com\\/pruebas\\/qjarvis\\/api\\/file\\/images\\/image_6199f023168488f2deb72a43dbfa117d_gen.jpg\",\"message\":\"Everything work just fine\"}";
            QJUploadFileResponse stuff = Converter.Deserialize<QJUploadFileResponse>(json, "_");

            

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
                    //file.Stream.CopyTo(requestStream);

                    //SStreamCopyTo(file.Stream, requestStream);
                    //
                    byte[] buffertemp = new byte[16 * 1024]; // Fairly arbitrary size
                    int bytesRead;

                    while ((bytesRead = file.Stream.Read(buffertemp, 0, buffertemp.Length)) > 0)
                    {
                        requestStream.Write(buffertemp, 0, bytesRead);
                    }
                    //

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
                //responseStream.CopyTo(stream);
                //
                byte[] buffertemp = new byte[16 * 1024]; // Fairly arbitrary size
                int bytesRead;

                while ((bytesRead = responseStream.Read(buffertemp, 0, buffertemp.Length)) > 0)
                {
                    stream.Write(buffertemp, 0, bytesRead);
                }
                //
                return stream.ToArray();
            }
        }

       /* 
        // Only useful before .NET 4
        private static void SStreamCopyTo(this Stream input, Stream output)
        {
            byte[] buffer = new byte[16 * 1024]; // Fairly arbitrary size
            int bytesRead;

            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }
        * */
         
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
