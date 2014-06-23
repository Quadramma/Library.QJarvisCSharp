using System;
using System.Collections.Generic;
using System.Text;
using CodeBetter.Json;


namespace com.quadramma.qjarvis.http
{

    [SerializeIncludingBase]
    public class QJUploadFileResponse
    {
        private string _ok;
        private string _root;
        private string _filename;
        private string _url;
        private string _message;
        public string ok { get { return _ok; } set { _ok = value; } }
        public string root { get { return _root; } set { _root = value; } }
        public string filename { get { return _filename; } set { _filename = value; } }
        public string url { get { return _url; } set { _url = value; } }
        public string message { get { return _message; } set { _message = value; } }

         public QJUploadFileResponse(string ok,string root,string filename,string url,string message) {
            this.ok = ok;
            this.root = root;
            this.filename = filename;
            this.url = url;
            this.message = message;
        }
         private QJUploadFileResponse() { }
    }

    /*
     *   "ok"=>$response["ok"],
        "root"=> $root,
        "filename"=>$response["filename"],
        "url" => $root . '/' . $response["filename"],
        "message"=> $extraMessage . $response["message"]
     */
}
