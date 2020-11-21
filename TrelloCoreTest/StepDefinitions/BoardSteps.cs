using OpenQA.Selenium;
using TrelloCoreTest.PageObjects;
using TechTalk.SpecFlow;
using NUnit.Framework;
using TrelloCoreTest.Support;
using System.Linq;

namespace TrelloCoreTest.StepDefinitions
{
    [Binding, Parallelizable]
    public class BoardSteps
    {
        private BoardMainPage BoardMainPage => new BoardMainPage(_driver);

        private IWebDriver _driver;
        private ScenarioContext _scenarioContext;

        public BoardSteps(IWebDriver driver, ScenarioContext scenarioContext)
        {
            _driver = driver;
            _scenarioContext = scenarioContext;
        }

        [When(@"I create a card in the first left hand side list")]
        public void WhenIOpenATrelloBoard()
        {
            _scenarioContext.Add("UsedListName", BoardMainPage.GetFirstLeftHandSideListName()); // Save left hand side list name for verification at the end
            _scenarioContext.Add("UsedCardName", DataDrivenTestHelper.Instance.CardToCreate); // Get the name set in the json data field into the scenario context

            BoardMainPage.AddCardToList(_scenarioContext.Get<string>("UsedCardName"));
        }

        [Then(@"the new card is successfully created")]
        public void ThenTheNewCardIsSuccessfullyCreated()
        {
            var cards_displayed = BoardMainPage.GetAllCardTitlesInList(_scenarioContext.Get<string>("UsedListName"));

            CollectionAssert.Contains(cards_displayed, _scenarioContext.Get<string>("UsedCardName"), "Card is not within the list");
        }


    }
}


