using IoT_Gas.CRUD;
using IoT_Gas.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IoT_Gas
{
    public partial class RequestPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CRUDSample crudSample = new CRUDSample();
            StringBuilder sb = new StringBuilder();
            if (Request["GetDevices"] == "true")
            {

                var devices = crudSample.GetDevicesID();
                sb.Append("{ \"data\" : [ ");
                foreach (var savedSample in devices)
                {
                    sb.Append("{ ");
                    sb.AppendFormat("\"id\" : \"{0}\", " +
                                    "\"lat\" : \"{1}\", " +
                                    "\"lng\" : \"{2}\"",
                                    savedSample.id, savedSample.lat, savedSample.lng);

                    sb.Append(" }, ");
                }
                sb.Remove(sb.Length - 2, 2);
                sb.Append(" ] }");
            }

            else
            {
                if (Request["key"] != null)
                {
                    string key = Request["key"];
                    crudSample = new CRUDSample();
                    /*Read*/
                    IEnumerable<Sample> savedSamples = crudSample.GetSamplesOfDevice(key);

                    sb.Append("{ \"data\" : [ ");
                    foreach (var savedSample in savedSamples)
                    {
                        sb.Append("{ ");
                        sb.AppendFormat("\"id\" : \"{0}\", " +
                                        "\"counter\" : \"{1}\", " +
                                        "\"value\" : \"{2}\", " +
                                        "\"time\" : \"{3} {4}\"",
                                        savedSample.Key, savedSample.counter, savedSample.gas, savedSample.time.Substring(0, 10), savedSample.time.Substring(11, 8));

                        sb.Append(" }, ");
                    }
                    sb.Remove(sb.Length - 2, 2);
                    sb.Append(" ] }");
                }
            }
            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.Flush();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            HttpContext.Current.Response.SuppressContent = true; 
        }
    }
}