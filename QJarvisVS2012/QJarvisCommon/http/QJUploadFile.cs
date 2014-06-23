using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace com.quadramma.qjarvis.http
{
    public class QJUploadFile
    {
        public QJUploadFile()
        {
            ContentType = "application/octet-stream";
        }
        public string Name { get; set; }
        public string Filename { get; set; }
        public string ContentType { get; set; }
        public Stream Stream { get; set; }
    }
}
