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
            ChromeOptions options = new ChromeOptions();

            //This can be used to set custom settings for a given scenario before instatiating the Driver.
            //For instance, this will set Chrome GeoLocation for an scenario matching the tag
            if (_scenarioContext.ScenarioInfo.Tags.Any(x => x == "ScenarioThatRequiresGeoLocForInstance"))
            {
                options.AddUserProfilePreference("profile.default_content_setting_values.geolocation", 2);
            }

            _scenarioContext.Add("options", options);
        }

        [BeforeScenario(Order = 0)]
        public void BeforeScenarioMain()
        {
            IWebDriver Driver;
            ChromeOptions options = _scenarioContext.Get<ChromeOptions>("options");
            options.AddArgument("disable-infobars");
            options.SetLoggingPreference(LogType.Browser, LogLevel.Severe);

            //Add Arguments set universally in environment config
            foreach (string s in EnvironmentConfig.Instance.Browser_args)
            {
                options.AddArguments(s);
            }

            Driver = new ChromeDriver(options);

            Driver.Manage().Cookies.DeleteAllCookies();
            _objectContainer.RegisterInstanceAs(Driver);
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

