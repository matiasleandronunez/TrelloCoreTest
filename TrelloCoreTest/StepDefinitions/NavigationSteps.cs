using OpenQA.Selenium;
using TrelloCoreTest.PageObjects;
using TechTalk.SpecFlow;
using NUnit.Framework;
using TrelloCoreTest.Support;

namespace TrelloCoreTest.StepDefinitions
{
    [Binding, Parallelizable]
    public class NavigateSteps
    {
        private BoardsDashboardPage BoardsDashboardPage => new BoardsDashboardPage(driver);

        private IWebDriver driver;
        private ScenarioContext scenarioContext;

        public NavigateSteps(IWebDriver driver, ScenarioContext scenarioContext)
        {
            this.driver = driver;
            this.scenarioContext = scenarioContext;
        }

        [When(@"I open a trello board")]
        public void WhenIOpenATrelloBoard()
        {
            BoardsDashboardPage.ClickOnBoard(DataDrivenTestHelper.Instance.Board1ToCreate);
        }
    }
}