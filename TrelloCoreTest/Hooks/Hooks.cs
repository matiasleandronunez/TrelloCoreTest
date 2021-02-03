using OpenQA.Selenium;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;
using TrelloCoreTest.Support;

namespace TrelloCoreTest.Hooks
{
    [Binding]
    public class Hooks
    {
        public IWebDriver Driver;
        private ScenarioContext scenarioContext;

        public Hooks(IWebDriver driver, ScenarioContext scenarioContext)
        {
            Driver = driver;
            this.scenarioContext = scenarioContext;
        }

        [BeforeTestRun(Order = 0)]
        public static void CleanUpPreviousData()
        {
            var api = new APIHelper();
            api.DeleteAllBoards();
        }

        [BeforeTestRun(Order = 1)]
        public static void GenerateTrelloTestData()
        {
            var api = new APIHelper();
            var newBoardId = api.CreateBoard(DataDrivenTestHelper.Instance.Board1ToCreate);
            foreach (string listName in DataDrivenTestHelper.Instance.Board1ListsToCreate)
            {
                api.CreateList(newBoardId, listName);
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            Driver.Quit();
        }
    }
}

