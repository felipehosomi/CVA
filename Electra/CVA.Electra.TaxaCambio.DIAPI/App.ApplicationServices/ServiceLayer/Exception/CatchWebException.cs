using App.Repository.Exception;
using App.Repository.Generic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace App.ApplicationServices.ServiceLayer
{
    public class CatchWebException : Exception
    {
        public CatchWebException(Exception e) :
            base(null, e)
        {
        }

        public override string Message
        {
            get
            {
                var e = InnerException as WebException;
                if (e != null)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;

                        using (StreamReader reader = new StreamReader(response.GetResponseStream(), httpResponse.GetEncoding()))
                        {
                            var ret = JsonConvert.DeserializeObject<ErrorServiceLayerException>(reader.ReadToEnd());
                            stringBuilder.AppendLine($"Ret code: {httpResponse.StatusCode}");
                            stringBuilder.AppendLine("Error code: " + ret.error.code);
                            stringBuilder.AppendLine("Error lang: " + ret.error.message.lang);
                            stringBuilder.AppendLine("Error value: " + ret.error.message.value);
                        }
                    }

                    return stringBuilder.ToString();
                }

                return base.Message;
            }
        }

        public static void ExceptionError(WebException e)
        {
            StringBuilder stringBuilder = new StringBuilder();

            using (WebResponse response = e.Response)
            {
                if (response == null)
                {
                    throw new Exception(e.Message);
                }
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    var httpResponse = e.Response as HttpWebResponse;
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        var ret = JsonConvert.DeserializeObject<ErrorServiceLayerException>(reader.ReadToEnd());
                        //stringBuilder.Append(Environment.NewLine);
                        stringBuilder.Append($"-> Error: {httpResponse?.StatusCode}");
                        stringBuilder.Append(Environment.NewLine);
                        stringBuilder.Append("-> (Code: "+ ret?.error?.code + "): " + ret?.error?.message?.value);


                        try
                        {
                            string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                            path = $@"{path}\\CreateRequest.txt";
                            System.IO.File.AppendAllText(path, Environment.NewLine + "--Error: " + ret?.error?.message?.value);

                        }
                        catch { }

                    }
                }
            }
            throw new Exception(stringBuilder.ToString());
        }
    }
}

