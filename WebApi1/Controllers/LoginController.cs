using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json;
namespace WebApi1.Controllers
{
    public class LoginController:ApiController
    {
        public DataTable getjion([ModelBinder(typeof(MyMutableObjectModelBinder))]DataTable dt)
        {
            DataTable data = dt.Clone();
            DataRow row = data.NewRow();
            row["MSG"] = "error";
            data.Rows.Add(row);         
            return data;
        }
        [HttpPost]
        [ResponseType(typeof(DataTable))]
        public HttpResponseMessage UserLogin([ModelBinder(typeof(MyMutableObjectModelBinder))]DataTable dt)
        {
            DataTable data = dt.Clone();
            data.Columns.Add("result");
            DataRow row = data.NewRow();
            row["result"] = "error";
            data.Rows.Add(row);
            return Request.CreateResponse<DataTable>(HttpStatusCode.OK, data);
        }
        [HttpPost]
        [ResponseType(typeof(DataTable))]
        public HttpResponseMessage UserExit([ModelBinder(typeof(MyMutableObjectModelBinder))]DataTable dt)
        {
            DataTable data = dt.Clone();
            data.Columns.Add("result");
            DataRow row = data.NewRow();
            row["result"] = "sucess";
            data.Rows.Add(row);
            return Request.CreateResponse<DataTable>(HttpStatusCode.OK, data);
        }
    }
    public class MyMutableObjectModelBinder:IModelBinder
    {   
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {

            //EvPlatformSvc.Dao.Common.m_logApiInvoke.Info("czm test1");
            if (!CanBindType(bindingContext.ModelType))
            {
                return false;
            }
            bindingContext.Model = Activator.CreateInstance(bindingContext.ModelType);

           System.Threading.Tasks.Task<string> s = actionContext.Request.Content.ReadAsStringAsync();
              string json = s.Result;
            if (json.Length > 0 && json[0] != '[')
            {
                json = json.Insert(0, "[");
                json = json + "]";
            }

            DataTable dt = (DataTable)JsonConvert.DeserializeObject(json.ToString(), typeof(DataTable));
            bindingContext.Model = dt;
            return true;
        }

        private async System.Threading.Tasks.Task<string> GetContent(HttpActionContext actionContext)
        {
            string s = await actionContext.Request.Content.ReadAsStringAsync();
            return s;
        }
        internal static bool CanBindType(Type modelType)
        {
            if (modelType == typeof(DataTable))
                return true;
            else
                return false;
        }
    }
}