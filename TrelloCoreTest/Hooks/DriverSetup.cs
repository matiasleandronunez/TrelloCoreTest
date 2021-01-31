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
        private IObjectContainer _objectContainer;
        private ScenarioContext _scenarioContext;
        public IWebDriver Driver;

        public DriverSetup(IObjectContainer objectContainer, ScenarioContext scenarioContext)
        {
            _objectContainer = objectContainer;
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario(Order = -1)]
        public void BeforeScenarioOptions()
        {
            //This pre-requesite step will set options for the various browsers types. As selenium 3, this is per browser. 
            
            ChromeOptions SetChromeOptions(string[] parameters = null, Dictionary<string, object> extra_additional_caps = null)
            {
                ChromeOptions options = new ChromeOptions();

                //Set Chrome GeoLocation for an scenario matching the tag
                if (_scenarioContext.ScenarioInfo.Tags.Any(x => x == "ScenarioThatRequiresGeoLocForInstance"))
                {
                    options.AddUserProfilePreference("profile.default_content_setting_values.geolocation", 2);
                }

                options.AddArgument("disable-infobars");
                options.SetLoggingPreference(LogType.Browser, LogLevel.Severe);

                try
                {
                    options.AddArguments(EnvironmentConfig.Instance.Browser_args);
                }
                catch (Exception)
                {
                    //
                }

                if (parameters != null)
                {
                    options.AddArguments(parameters);
                }

                if (extra_additional_caps != null)
                {
                    foreach (KeyValuePair<string, object> ac in extra_additional_caps)
                    {
                        options.AddAdditionalCapability(ac.Key, ac.Value);
                    }
                }

                return options;
            }

            FirefoxOptions SetFirefoxOptions(string[] parameters= null, Dictionary<string, object> extra_additional_caps = null)
            {
                FirefoxOptions options = new FirefoxOptions();

                options.AcceptInsecureCertificates = true;

                try
                {
                    options.AddArguments(EnvironmentConfig.Instance.Browser_args);
                }
                catch (Exception)
                {
                    //
                }

                if (parameters != null)
                {
                    options.AddArguments(parameters);
                }

                if (extra_additional_caps != null) 
                {
                    foreach (KeyValuePair<string, object> ac in extra_additional_caps)
                    {
                        options.AddAdditionalCapability(ac.Key, ac.Value);
                    }
                }

                return options;
            }

            AppiumOptions SetAndroidWebMobileOptions(string device, string platform_v, string android_v, Dictionary<string, object> extra_additional_caps = null)
            {
                AppiumOptions options = new AppiumOptions();

                options.AddAdditionalCapability(MobileCapabilityType.BrowserName, "chrome");
                options.AddAdditionalCapability(MobileCapabilityType.PlatformName, MobilePlatform.Android);
                options.AddAdditionalCapability(MobileCapabilityType.FullReset, false);
                options.AddAdditionalCapability(MobileCapabilityType.NoReset, true);

                options.AddAdditionalCapability(MobileCapabilityType.DeviceName, device);
                options.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, platform_v);
                options.AddAdditionalCapability("version", android_v);

                if (extra_additional_caps != null)
                {
                    foreach (KeyValuePair<string, object> ac in extra_additional_caps)
                    {
                        options.AddAdditionalCapability(ac.Key, ac.Value);
                    }
                }
                return options;
            }

            OperaOptions SetOperaOptions(string version, string[] parameters = null, Dictionary<string, object> extra_additional_caps = null)
            {

                OperaOptions options = new OperaOptions();

                options.AddAdditionalCapability("version", version);

                if (parameters != null)
                {
                    options.AddArguments(parameters);
                }

                if (extra_additional_caps != null)
                {
                    foreach (KeyValuePair<string, object> ac in extra_additional_caps)
                    {
                        options.AddAdditionalCapability(ac.Key, ac.Value);
                    }
                }
                return options;
            }

            SafariOptions SetSafariOptions(Dictionary<string, object> extra_additional_caps = null)
            {
                SafariOptions options = new SafariOptions();

                if (extra_additional_caps != null)
                {
                    foreach (KeyValuePair<string, object> ac in extra_additional_caps)
                    {
                        options.AddAdditionalCapability(ac.Key, ac.Value);
                    }
                }
                return options;
            }

            //Gets the browser tag of the test for crossbrowser testing to set options before instantiating driver, defaults to chrome
            _scenarioContext.TryGetValue("Browser", out var browser);

            //Switch browser and set options
            //These are examples for local, grid and cloud (Browserstack) browsers 
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

                    _scenarioContext.Add("options", SetSafariOptions(extra_additional_caps:
                            new Dictionary<string, object> { { "bstack:options", browserstackOptions } }
                    ));
                    break;
                case "Grid_Android_Chrome":
                    _scenarioContext.Add("options", SetAndroidWebMobileOptions(device: "samsung_galaxy_s10_11.0", platform_v: "11.0.0", android_v: "11.0"));
                    break;
                case "Grid_OperaBlink":
                    //must use desired capabilities to use custom browser name
                    DesiredCapabilities grid_operablink_opt = new DesiredCapabilities();

                    grid_operablink_opt.SetCapability("browserName", "operablink");

                    _scenarioContext.Add("options", grid_operablink_opt);
                    break;
                case "Headless":
                    _scenarioContext.Add("options", SetChromeOptions(parameters: new string[] {"headless"}));
                    break;
                case "Firefox":
                    _scenarioContext.Add("options", SetFirefoxOptions());
                    break;
                case "Chrome":
                default:
                    _scenarioContext.Add("options", SetChromeOptions());
                    break;
            }
        }

        [BeforeScenario(Order = 0)]
        public void BeforeScenarioMain()
        {
            //Set the Driver using the options set in the pre-step
            IWebDriver Driver;

            _scenarioContext.TryGetValue("Browser", out var browser);

            //Get options by browser, instantiate and register drivers
            switch (browser)
            {
                case "BrowserStack_iOS11":
                    SafariOptions bs_ios11 = _scenarioContext.Get<SafariOptions>("options");

                    Driver = new RemoteWebDriver(
                            new Uri("https://hub-cloud.browserstack.com/wd/hub/"), bs_ios11);

                    try
                    {
                        Driver.Manage().Cookies.DeleteAllCookies();
                    }
                    catch (NotImplementedException)
                    {

                    }

                    _objectContainer.RegisterInstanceAs(Driver);
                    break;
                case "Grid_Android_Chrome":
                    AppiumOptions grid_android_chrome = _scenarioContext.Get<AppiumOptions>("options");

                    Driver = new AndroidDriver<IWebElement>(EnvironmentConfig.Instance.SGrid.RemoteHubURI, grid_android_chrome);

                    _objectContainer.RegisterInstanceAs(Driver);
                    break;
                case "Grid_OperaBlink":
                    DesiredCapabilities op = _scenarioContext.Get<DesiredCapabilities>("options");

                    Driver = new RemoteWebDriver(EnvironmentConfig.Instance.SGrid.RemoteHubURI, op);

                    Driver.Manage().Cookies.DeleteAllCookies();
                    _objectContainer.RegisterInstanceAs(Driver);
                    break;
                case "Firefox":
                    FirefoxOptions ffo = _scenarioContext.Get<FirefoxOptions>("options");

                    //Known issue with geckodriver and Net Core, refer to: https://stackoverflow.com/questions/53629542/selenium-geckodriver-executes-findelement-10-times-slower-than-chromedriver-ne
                    var svc = FirefoxDriverService.CreateDefaultService();
                    svc.Host = "::1";

                    Driver = new FirefoxDriver(svc, ffo);

                    Driver.Manage().Cookies.DeleteAllCookies();
                    _objectContainer.RegisterInstanceAs(Driver);
                    break;
                case "Chrome":
                case "Headless":
                default:
                    ChromeOptions co = _scenarioContext.Get<ChromeOptions>("options");

                    Driver = new ChromeDriver(co);

                    Driver.Manage().Cookies.DeleteAllCookies();
                    _objectContainer.RegisterInstanceAs(Driver);
                    break;
            }
        }

        //Add tags of scenarios that are developed or waiting fix, so they don't report falsely as failed.
        //To be used as per SDET's discretion
        [BeforeScenario(Order = 0)]
        public void ScenariosWithPendingTicketsOrNotImplementedYet()
        {
            if (_scenarioContext.ScenarioInfo.Tags.Any(x => x == "ScenariosFailingBecauseOfExternalFactorsOrPendingFixes"))
            {
                Assert.Ignore("Scenario tagged as Pending Ticket or still not implemented");
            }
        }

    }
}

