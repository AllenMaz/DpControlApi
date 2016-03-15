using DpControl.Models;
using DpControl.Utility;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Controllers
{
    public class BaseController : Controller
    {
        public TableParams GetJqueryTableParams()
        {
           
            HttpRequest rq = Request;
            StreamReader srRequest = new StreamReader(rq.Body);
            String strReqStream = srRequest.ReadToEnd();
            List<JQueryTableParam> lsParams = JsonHandler.UnJson<List<JQueryTableParam>>(strReqStream);

            TableParams tableParames = new TableParams();
            if (lsParams.Count >0)
            {
                tableParames.sEcho = (int)lsParams.Where(v => v.name == "sEcho").First().value;
                tableParames.iDisplayStart = (int)lsParams.Where(v => v.name == "iDisplayStart").First().value;
                tableParames.iDisplayLength = (int)lsParams.Where(v => v.name == "iDisplayLength").First().value;


            }
            return tableParames;
        }
    }
}
