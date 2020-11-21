using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TrelloCoreTest.Support
{
    public sealed class EnvironmentConfig
    {
        string base_url;
        string username;
        string adminuser;
        string adminpassword;
        string api_uri;
        string oauth;
        string apikey;
        string apitoken;
        List<string> browser_args;

        EnvironmentConfig()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace("\\bin\\Debug\\netcoreapp3.1", "");
            string data = System.IO.File.ReadAllText(new Uri($"{path}\\environmentconfig.json").LocalPath);
            var dobj = JsonConvert.DeserializeObject<dynamic>(data);

            base_url = dobj.appSettings.BASE_URL.ToString();
            adminuser = dobj.users.admin.Email.ToString();
            username = dobj.users.admin.UserName.ToString();
            adminpassword = dobj.users.admin.Password.ToString();
            api_uri = dobj.api.uri.ToString();
            apikey = dobj.api.key.ToString();
            apitoken = dobj.api.token.ToString();
            oauth = dobj.api.oauthsecret.ToString();
            browser_args = JsonConvert.DeserializeObject<List<string>>(dobj.browser.ToString());
        }

        private static readonly object padlock = new object();
        private static EnvironmentConfig instance = null;
        public static EnvironmentConfig Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new EnvironmentConfig();
                    }
                    return instance;
                }
            }
        }

        public string Base_url { get => base_url; set => base_url = value; }
        public string Username { get => username; set => username = value; }
        public (string Username, string Password) AdminUser
        {
            get => (adminuser, adminpassword);
        }
        public string Api_uri { get => api_uri; set => api_uri = value; }
        public string ApiKey { get => apikey; set => apikey = value; }
        public string ApiToken { get => apitoken; set => apitoken = value; }
        public string ApiOAuth { get => oauth; set => oauth = value; }
        public List<string> Browser_args { get => browser_args; set => browser_args = value; }

    }
}
