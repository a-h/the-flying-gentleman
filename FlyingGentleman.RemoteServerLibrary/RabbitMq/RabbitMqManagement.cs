using FlyingGentleman.Library;
using FlyingGentleman.Library.RabbitMqManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace FlyingGentleman.RemoteServerLibrary.RabbitMq
{
    /// <summary>
    /// Gives access to the RabbitMq management functionality.
    /// </summary>
    [Serializable]
    public class RabbitMqManagement : LogEventCreatorBase, IRabbitMqManagement
    {
        private void CheckRequiredSettings(RabbitMqManagementSettings settings)
        {
            if (String.IsNullOrWhiteSpace(settings.AdminUserName))
            {
                throw new ArgumentException("No AdminUserName was specified");
            }

            if (String.IsNullOrWhiteSpace(settings.AdminPassword))
            {
                throw new ArgumentException("No AdminPassword was specified");
            }
        }

        private void CallApi(RabbitMqManagementSettings settings, string uri, string httpMethod, string jsonBody)
        {
            LogEventHelper.CreateEvent(this, "CallApi", String.Format("Calling RabbitMq Server on Uri {0} with body {1}", uri, jsonBody));

            // Create a web request for creating this user
            var request = (HttpWebRequest)WebRequest.Create(uri);

            request.Credentials = new NetworkCredential(settings.AdminUserName, settings.AdminPassword);
            request.ContentType = "application/json";
            request.Method = httpMethod;

            using (Stream webStream = request.GetRequestStream())
            {
                // Using UTF8 encoding causes RabbitMq to report a 400 bad request not_json error.
                using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
                {
                    requestWriter.Write(jsonBody);
                }
            }

            using (StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                string response = sr.ReadToEnd();

                LogEventHelper.CreateEvent(this, "CallApi", String.Format("Called RabbitMq Server on Uri {0} with body {1}, the response was {2}", uri, jsonBody, response));
            }
        }

        /// <summary>
        /// Creates a set of users on a RabbitMq server
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="server">The rabbit mq server to call e.g. 192.168.142.50 or PRDUKRAB01</param>
        public void CreateUsers(string server, RabbitMqManagementSettings settings)
        {
            CheckRequiredSettings(settings);

            LogEventHelper.CreateEvent(this, "CreateUsers", String.Format("Creating {0} Users for RabbitMq Server {1} and VHost {2}", settings.Users.Count(), server, settings.VHost));
                       
            foreach (var user in settings.Users)
            {
                string uri = String.Format("http://{0}:{1}/api/users/{2}", server, settings.RabbitMqManagementPort, user.UserName);
                string jsonBody = String.Format("{{ \"password\" : \"{0}\", \"tags\" : \"{1}\" }}", user.Password, user.Tag);

                CallApi(settings, uri, WebRequestMethods.Http.Put, jsonBody);
            }
        }

        /// <summary>
        /// Set user permissions on RabbitMq server
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="server">The rabbit mq server to call e.g. 192.168.142.50 or PRDUKRAB01</param>
        public void SetUserPermissions(string server, RabbitMqManagementSettings settings)
        {
            CheckRequiredSettings(settings);

            LogEventHelper.CreateEvent(this, "SetUserPermissions", String.Format("Setting Permision for {0} Users for RabbitMq Server {1} and VHost {2}", settings.Users.Count(), server, settings.VHost));

            if (settings.VHost == "/")
            {
                settings.VHost = HttpUtility.UrlEncode(settings.VHost);
            }

            foreach (var user in settings.Users)
            {
                string uri = String.Format("http://{0}:{1}/api/permissions/{2}/{3}", server, settings.RabbitMqManagementPort, settings.VHost, user.UserName);
                string jsonBody = String.Format("{{ \"configure\" : \"{0}\", \"write\" : \"{1}\", \"read\" : \"{2}\" }}", user.ConfigureRegEx, user.WriteRegEx, user.ReadRegEx);

                CallApi(settings, uri, WebRequestMethods.Http.Put, jsonBody);
            }
        }
    }
}
