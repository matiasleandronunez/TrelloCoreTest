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
        private ScenarioContext _scenarioContext;

        public Hooks(IWebDriver driver, ScenarioContext scenarioContext)
        {
            Driver = driver;
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun(Order = 0)]
        public static void CleanUpPreviousData()
        {
            var _api = new APIHelper();
            _api.DeleteAllBoards();
        }

        [BeforeTestRun(Order = 1)]
        public static void GenerateTrelloTestData()
        {
            var _api = new APIHelper();
            var new_board_id = _api.CreateBoard(DataDrivenTestHelper.Instance.Board1ToCreate);
            foreach (string list_name in DataDrivenTestHelper.Instance.Board1ListsToCreate)
            {
                _api.CreateList(new_board_id, list_name);
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            Driver.Quit();
        }
    }
}

