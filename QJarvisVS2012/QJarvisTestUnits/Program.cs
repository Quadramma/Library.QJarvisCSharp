using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using CodeBetter.Json;
using com.quadramma.qjarvis.http;

namespace QJarvisTestUnits
{

    class Program
    {
        static void Main(string[] args)
        {
            byte[] bytes = QJWebClient.GetFileByteArray("asdasdas.jpg");
            string url = "http://www.quadramma.com/pruebas/qjarvis/api/file/upload";
            QJUploadFileResponse response = QJWebClient.UploadFile(url, "fileUpload", "asdasdas.jpg", bytes, null);

        
            Console.WriteLine(response.message);
            Console.WriteLine(response.url);
            Console.WriteLine(response.ok);
        }
    }
}
