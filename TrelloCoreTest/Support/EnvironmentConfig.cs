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
        string baseUrl;
        string userName;
        string adminUser;
        string adminPassword;
        string apiUri;
        string oauth;
        string apiKey;
        string apiToken;
        List<string> browserArgs;
        SeleniumGridConfig sgrid;
        string browserstackUserName;
        string browserstackAutomateKey;

        EnvironmentConfig()
        {
            var path = Directory.GetCurrentDirectory().Replace(Path.Combine("bin", "Debug", "netcoreapp3.1").ToString(), "");

            try
            {
                var dobj = JsonConvert.DeserializeObject<dynamic>(
                    System.IO.File.ReadAllText(Path.Combine(path, "environmentconfig.json")));

                baseUrl = dobj.appSettings.BASE_URL.ToString();
                adminUser = dobj.users.admin.Email.ToString();
                userName = dobj.users.admin.UserName.ToString();
                adminPassword = dobj.users.admin.Password.ToString();
                apiUri = dobj.api.uri.ToString();
                apiKey = dobj.api.key.ToString();
                apiToken = dobj.api.token.ToString();
                oauth = dobj.api.oauthsecret.ToString();
                browserArgs = JsonConvert.DeserializeObject<List<string>>(dobj.browser.ToString());
                browserstackUserName = dobj.browserstack.USERNAME.ToString();
                browserstackAutomateKey = dobj.browserstack.AUTOMATE_KEY.ToString();
                sgrid = JsonConvert.DeserializeObject<SeleniumGridConfig>(dobj.selenium_grid.ToString());

                /* Check for required fields and throw a user friendly message if something is missing. 
                  Added this in case someone downloads and runs the tests without reading docs, as the JSON exception 
                  is kind of cryptic */
                if (new List<String>{baseUrl, adminUser, userName, adminPassword, apiUri, apiKey, apiToken, oauth}.Any(x => String.IsNullOrEmpty(x)))
                {
                    throw new Exception(message: "Seems like some required values are missing in environmentconfig.json. Please refer to the README.md at https://github.com/matiasleandronunez/TrelloCoreTest");
                }
            }
            catch (JsonReaderException e)
            {
                throw new Exception(message: "Seems like the environmentconfig.json is either absent, empty or bad formated. Please refer to the README.md at https://github.com/matiasleandronunez/TrelloCoreTest", innerException: e);
            }
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

        public string BaseURL { get => baseUrl; set => baseUrl = value; }
        public string Username { get => userName; set => userName = value; }
        public (string Username, string Password) AdminUser
        {
            get => (adminUser, adminPassword);
        }
        public string ApiURI { get => apiUri; set => apiUri = value; }
        public string ApiKey { get => apiKey; set => apiKey = value; }
        public string ApiToken { get => apiToken; set => apiToken = value; }
        public string ApiOAuth { get => oauth; set => oauth = value; }
        public List<string> BrowserArgs { get => browserArgs; set => browserArgs = value; }
        public SeleniumGridConfig SGrid { get => sgrid; set => sgrid = value; }
        public (string Username, string AutomateKey) BrowserStack
        {
            get => (browserstackUserName, browserstackAutomateKey);
        }
    }

    public class SeleniumGridConfig
    {
        public Uri RemoteHubURI { get; set; }
    }
}
