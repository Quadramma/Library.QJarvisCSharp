using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.quadramma.qjarvis.http
{
   
    public class QJUploadFileResponse
    {
        public string ok { get; set; }
        public string root { get; set; }
        public string filename { get; set; }
        public string url { get; set; }
        public string message { get; set; }
    }

    /*
     *   "ok"=>$response["ok"],
        "root"=> $root,
        "filename"=>$response["filename"],
        "url" => $root . '/' . $response["filename"],
        "message"=> $extraMessage . $response["message"]
     */
}
