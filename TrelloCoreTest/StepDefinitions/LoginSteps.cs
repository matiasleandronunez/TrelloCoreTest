﻿using OpenQA.Selenium;
using TrelloCoreTest.PageObjects;
using TechTalk.SpecFlow;
using NUnit.Framework;
using TrelloCoreTest.Support;

namespace TrelloCoreTest.StepDefinitions
{
    [Binding, Parallelizable]
    public class LoginSteps
    {
        private LoginPage LoginPage => new LoginPage(driver);
        private AtlassianSSOPage AtlassianSSOPage => new AtlassianSSOPage(driver);

        private IWebDriver driver;
        private ScenarioContext scenarioContext;

        public LoginSteps(IWebDriver driver, ScenarioContext scenarioContext)
        {
            this.driver = driver;
            this.scenarioContext = scenarioContext;
        }

        [Given(@"I navigate to Trello login page")]
        public void GivenINavigateToPage()
        {
            LoginPage.Open();
        }


        [Given(@"I log into Trello as an admin")]
        public void GivenILogIntoTrelloAsAnAdmin()
        {
            var admin = EnvironmentConfig.Instance.AdminUser;

            GivenINavigateToPage();

            LoginPage.InputEmail(admin.Username);

            //Removed as new Trello login flow requires Atlassian SSO
            //LoginPage.InputPassword(admin.Password);
            
            LoginPage.SubmitLogin();
        }

        [Given(@"I SSO into the Atlassian Account")]
        public void GivenISSOIntoTheAtlassianAccount()
        {
            var admin = EnvironmentConfig.Instance.AdminUser;

            AtlassianSSOPage.InputPassword(admin.Password);
            AtlassianSSOPage.SubmitLogin();
        }

        [Given(@"I log into Trello as an admin with an Atlassian Account")]
        public void GivenILogIntoTrelloAsAnAdminWithAnAtlassianAccount()
        {
            GivenILogIntoTrelloAsAnAdmin();
            GivenISSOIntoTheAtlassianAccount();
        }

    }
}


