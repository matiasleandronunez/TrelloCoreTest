# TrelloCoreTest
Sample Test Automation project in .Net Core 3.1 / C#

Features API and UI testing from the same project.

Project uses RestSharp, Selenium, Appium, Specflow, NUnit

Once cloned, and after setting the credentials in the environmentconfig.json file, the solution can be run directly from Visual Studio 2019.

Updated on 31 Jan 2021. All dependencies installed from NuGet for easy management.


# Credentials Cofiguration
Input your Trello API tokens and the Atlassian managed account for UI login in environmentconfig.json

Sample:
```
{
  "appSettings": {
    "BASE_URL": "https://trello.com"
  },
  "users": {
    "admin": {
      "UserName": "mytrellousername",
      "Email": "mytrelloemail@example.com",
      "Password": "mytrellopassword"
    }
  },
  "api": {
    "uri": "https://api.trello.com",
    "key": "the32hexcharsapikeyyougotfromtrelloapi",
    "token": "the64hexcharstoken",
    "oauthsecret": "the64hexcharsoauthsecret"
  },
  "browser": [

  ],
  "selenium_grid": {
    "RemoteHubURI": "http://192.168.1.100:4444/wd/hub"
  },
  "browserstack": {
    "USERNAME": "yourbsusername",
    "AUTOMATE_KEY": "yourbsautomationkey"
  }
}

```

Now with the multibrowser support, you can also try cloud, grid, different local browsers!
