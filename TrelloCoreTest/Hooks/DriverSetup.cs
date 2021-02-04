namespace TrelloCoreTest.Hooks
{
    using BoDi;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using TechTalk.SpecFlow;
    using OpenQA.Selenium.Support.UI;
    using OpenQA.Selenium.Remote;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using NUnit.Framework.Interfaces;
    using System.ComponentModel;
    using TrelloCoreTest.Support;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.Opera;
    using OpenQA.Selenium.Safari;
    using OpenQA.Selenium.Appium;
    using OpenQA.Selenium.Appium.Enums;
    using OpenQA.Selenium.Appium.Android;

    [Binding]
    public class DriverSetup
    {
        private IObjectContainer objectContainer;
        private ScenarioContext scenarioContext;
        public readonly IWebDriver Driver;

        public DriverSetup(IObjectContainer objectContainer, ScenarioContext scenarioContext)
        {
            this.objectContainer = objectContainer;
            this.scenarioContext = scenarioContext;
        }

        [BeforeScenario(Order = -1)]
        public void BeforeScenarioOptions()
        {
            //This pre-requesite step will set options for the various browsers types. As selenium 3, this is per browser. 
            
            ChromeOptions SetChromeOptions(string[] parameters = null, Dictionary<string, object> extraAdditionalCaps = null)
            {
                var options = new ChromeOptions();

                //Set Chrome GeoLocation for an scenario matching the tag
                if (scenarioContext.ScenarioInfo.Tags.Any(x => x == "ScenarioThatRequiresGeoLocForInstance"))
                {
                    options.AddUserProfilePreference("profile.default_content_setting_values.geolocation", 2);
                }

                options.AddArgument("disable-infobars");
                options.SetLoggingPreference(LogType.Browser, LogLevel.Severe);

                try
                {
                    options.AddArguments(EnvironmentConfig.Instance.BrowserArgs);
                }
                catch (Exception)
                {
                    //
                }

                if (parameters != null)
                {
                    options.AddArguments(parameters);
                }

                if (extraAdditionalCaps != null)
                {
                    foreach (KeyValuePair<string, object> ac in extraAdditionalCaps)
                    {
                        options.AddAdditionalCapability(ac.Key, ac.Value);
                    }
                }

                return options;
            }

            FirefoxOptions SetFirefoxOptions(string[] parameters= null, Dictionary<string, object> extraAdditionalCaps = null)
            {
                var options = new FirefoxOptions();

                options.AcceptInsecureCertificates = true;

                try
                {
                    options.AddArguments(EnvironmentConfig.Instance.BrowserArgs);
                }
                catch (Exception)
                {
                    //
                }

                if (parameters != null)
                {
                    options.AddArguments(parameters);
                }

                if (extraAdditionalCaps != null) 
                {
                    foreach (KeyValuePair<string, object> ac in extraAdditionalCaps)
                    {
                        options.AddAdditionalCapability(ac.Key, ac.Value);
                    }
                }

                return options;
            }

            AppiumOptions SetAndroidWebMobileOptions(string device, string platformVersion, string androidVersion, Dictionary<string, object> extraAdditionalCaps = null)
            {
                var options = new AppiumOptions();

                options.AddAdditionalCapability(MobileCapabilityType.BrowserName, "chrome");
                options.AddAdditionalCapability(MobileCapabilityType.PlatformName, MobilePlatform.Android);
                options.AddAdditionalCapability(MobileCapabilityType.FullReset, false);
                options.AddAdditionalCapability(MobileCapabilityType.NoReset, true);

                options.AddAdditionalCapability(MobileCapabilityType.DeviceName, device);
                options.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, platformVersion);
                options.AddAdditionalCapability("version", androidVersion);

                if (extraAdditionalCaps != null)
                {
                    foreach (KeyValuePair<string, object> ac in extraAdditionalCaps)
                    {
                        options.AddAdditionalCapability(ac.Key, ac.Value);
                    }
                }
                return options;
            }

            OperaOptions SetOperaOptions(string version, string[] parameters = null, Dictionary<string, object> extraAdditionalCaps = null)
            {

                var options = new OperaOptions();

                options.AddAdditionalCapability("version", version);

                if (parameters != null)
                {
                    options.AddArguments(parameters);
                }

                if (extraAdditionalCaps != null)
                {
                    foreach (KeyValuePair<string, object> ac in extraAdditionalCaps)
                    {
                        options.AddAdditionalCapability(ac.Key, ac.Value);
                    }
                }
                return options;
            }

            SafariOptions SetSafariOptions(Dictionary<string, object> extraAdditionalCaps = null)
            {
                SafariOptions options = new SafariOptions();

                if (extraAdditionalCaps != null)
                {
                    foreach (KeyValuePair<string, object> ac in extraAdditionalCaps)
                    {
                        options.AddAdditionalCapability(ac.Key, ac.Value);
                    }
                }
                return options;
            }

            //Gets the browser tag of the test for crossbrowser testing to set options before instantiating driver, defaults to chrome
            scenarioContext.TryGetValue("Browser", out var browser);

            /*Switch browser and set options
              These are examples for local, grid and cloud (Browserstack) browsers */
            switch (browser)
            {
                case "BrowserStack_iOS11":
                    Dictionary<string, object> browserstackOptions = new Dictionary<string, object>();
                    browserstackOptions.Add("osVersion", "14");
                    browserstackOptions.Add("deviceName", "iPhone 11");
                    browserstackOptions.Add("realMobile", "true");
                    browserstackOptions.Add("appiumVersion", "1.16.0");
                    browserstackOptions.Add("local", "false");
                    browserstackOptions.Add("debug", "true");
                    browserstackOptions.Add("networkLogs", "true");
                    browserstackOptions.Add("userName", EnvironmentConfig.Instance.BrowserStack.Username);
                    browserstackOptions.Add("accessKey", EnvironmentConfig.Instance.BrowserStack.AutomateKey);

                    scenarioContext.Add("options", SetSafariOptions(extraAdditionalCaps:
                            new Dictionary<string, object> { { "bstack:options", browserstackOptions } }
                    ));
                    break;
                case "Grid_Android_Chrome":
                    scenarioContext.Add("options", SetAndroidWebMobileOptions(device: "samsung_galaxy_s10_11.0", platformVersion: "11.0.0", androidVersion: "11.0"));
                    break;
                case "Grid_OperaBlink":
                    //must use desired capabilities to use custom browser name
                    var gridOperaBlinkOptions = new DesiredCapabilities();

                    gridOperaBlinkOptions.SetCapability("browserName", "operablink");

                    scenarioContext.Add("options", gridOperaBlinkOptions);
                    break;
                case "Headless":
                    scenarioContext.Add("options", SetChromeOptions(parameters: new string[] {"headless"}));
                    break;
                case "Firefox":
                    scenarioContext.Add("options", SetFirefoxOptions());
                    break;
                case "Chrome":
                default:
                    scenarioContext.Add("options", SetChromeOptions());
                    break;
            }
        }

        [BeforeScenario(Order = 0)]
        public void BeforeScenarioMain()
        {
            //Set the Driver using the options set in the pre-step
            IWebDriver Driver;

            scenarioContext.TryGetValue("Browser", out var browser);

            //Get options by browser, instantiate and register drivers
            switch (browser)
            {
                case "BrowserStack_iOS11":
                    var browserstackIOS11 = scenarioContext.Get<SafariOptions>("options");

                    Driver = new RemoteWebDriver(
                            new Uri("https://hub-cloud.browserstack.com/wd/hub/"), browserstackIOS11);

                    try
                    {
                        Driver.Manage().Cookies.DeleteAllCookies();
                    }
                    catch (NotImplementedException)
                    {

                    }

                    objectContainer.RegisterInstanceAs(Driver);
                    break;
                case "Grid_Android_Chrome":
                    var gridAndroidChrome = scenarioContext.Get<AppiumOptions>("options");

                    Driver = new AndroidDriver<IWebElement>(EnvironmentConfig.Instance.SGrid.RemoteHubURI, gridAndroidChrome);

                    objectContainer.RegisterInstanceAs(Driver);
                    break;
                case "Grid_OperaBlink":
                    var op = scenarioContext.Get<DesiredCapabilities>("options");

                    Driver = new RemoteWebDriver(EnvironmentConfig.Instance.SGrid.RemoteHubURI, op);

                    Driver.Manage().Cookies.DeleteAllCookies();
                    objectContainer.RegisterInstanceAs(Driver);
                    break;
                case "Firefox":
                    var ffo = scenarioContext.Get<FirefoxOptions>("options");

                    //Known issue with geckodriver and Net Core, refer to: https://stackoverflow.com/questions/53629542/selenium-geckodriver-executes-findelement-10-times-slower-than-chromedriver-ne
                    var svc = FirefoxDriverService.CreateDefaultService();
                    svc.Host = "::1";

                    Driver = new FirefoxDriver(svc, ffo);

                    Driver.Manage().Cookies.DeleteAllCookies();
                    objectContainer.RegisterInstanceAs(Driver);
                    break;
                case "Chrome":
                case "Headless":
                default:
                    var co = scenarioContext.Get<ChromeOptions>("options");

                    Driver = new ChromeDriver(co);

                    Driver.Manage().Cookies.DeleteAllCookies();
                    objectContainer.RegisterInstanceAs(Driver);
                    break;
            }
        }

        /*Add tags of scenarios that are developed or waiting fix, so they don't report falsely as failed.
          To be used as per SDET's discretion */
        [BeforeScenario(Order = 0)]
        public void ScenariosWithPendingTicketsOrNotImplementedYet()
        {
            if (scenarioContext.ScenarioInfo.Tags.Any(x => x == "ScenariosFailingBecauseOfExternalFactorsOrPendingFixes"))
            {
                Assert.Ignore("Scenario tagged as Pending Ticket or still not implemented");
            }
        }

    }
}

