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
        private BoardsDashboardPage BoardsDashboardPage => new BoardsDashboardPage(_driver);

        private IWebDriver _driver;
        private ScenarioContext _scenarioContext;

        public NavigateSteps(IWebDriver driver, ScenarioContext scenarioContext)
        {
            _driver = driver;
            _scenarioContext = scenarioContext;
        }

        [When(@"I open a trello board")]
        public void WhenIOpenATrelloBoard()
        {
            BoardsDashboardPage.ClickOnBoard(DataDrivenTestHelper.Instance.Board1ToCreate);
        }
    }
}