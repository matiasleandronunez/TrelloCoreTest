# TrelloCoreTest
Sample Test Automation project in .Net Core 3.1 / C#

Features API and UI testing from the same project.

Project uses RestSharp, Selenium, Appium, Specflow, NUnit

Once cloned, and after setting the credentials in the environmentconfig.json file, the solution can be run directly from Visual Studio 2019.

Updated on 12 Feb 2021. All dependencies installed from NuGet for easy management.


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

# Build and run completely in docker containers!

NOTE: Keep in mind that the tests are pointed to run in Selenium3 Grid Chrome and Firefox out of the box. You'll need to deploy the grid ([Refer to grid readme file](https://github.com/matiasleandronunez/TrelloCoreTest/blob/master/README_SELENIUM_GRID_SETUP.md)) or manually change before building the image. The parameters JSON must also be configured before building the image.

To build and run the tests in docker, first you need to build the image using the docker compose command. Assuming you're currently in solution's directory:
```
docker build -t <some_image_name> .
```

Once the image is successfully built, you can proceed to run it with 
```
docker run <the_image_name> --name <friendly_name>
```

While on execution, you can check the UI tests in the Grid. 
Note: The Grid provided has debug images with VNC enabled for demostration purposes. Production grids must be regular nodes, with multiple node instances.

Once the tests are run, copy the results into the host machine with:
```
docker cp <friendly_name>:/src/TestResults/ ./
```

Alternatively, you can deploy a minimum grid with a Firefox and Chrome node and the build machine with a single command, by composing file TrelloCoreTest
/docker-compose.yml with:
```
docker-compose up --build
```

IMPORTANT: Bear in mind that if selenium-hub is running in the same machine as the container that's running the tests you may have to specify docker's selenium hub IP in the environment configuration file, this would look like 
```
"selenium_grid": {
    "RemoteHubURI": "http://172.18.0.2:4444/wd/hub"
  },
```
Refer to https://docs.docker.com/machine/reference/ip/

# Multibrowser support
Now with the multibrowser support, you can also try cloud, grid, different local browsers!

Now Selenium Grid also includes emulated Android devices!

To mark a test to be run in an specific browser, add a decorator to the test in the feature file and then configure browser Options and Driver in Hooks\DriverSetup.cs
