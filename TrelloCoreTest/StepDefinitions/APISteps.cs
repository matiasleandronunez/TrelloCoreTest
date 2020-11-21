using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using NUnit.Framework;
using TrelloCoreTest.PageObjects;
using TrelloCoreTest.Support;
using TrelloCoreTest.DataEntities;

namespace TrelloCoreTest.StepDefinitions
{
    [Binding, Parallelizable]
    public sealed class APISteps
    {
        private IWebDriver _driver;
        private ScenarioContext _scenarioContext;

        public APISteps(IWebDriver driver, ScenarioContext scenarioContext)
        {
            _driver = driver;
            _scenarioContext = scenarioContext;
        }

        [Given(@"I create a new card in the first list of a board through the api")]
        public void GivenICreateANewCardInTheFirstListOfABoardThroughTheApi()
        {
            var api = new APIHelper();
            _scenarioContext.Add("BoardID", api.GetBoardIdByName(DataDrivenTestHelper.Instance.Board1ToCreate));
            _scenarioContext.Add("FirstListOfBoard", api.GetAllListsInBoard(DataDrivenTestHelper.Instance.Board1ToCreate).First());
            _scenarioContext.Add("APIUsedCardName", DataDrivenTestHelper.Instance.APICardToCreate);

            api.CreateCard(_scenarioContext.Get<TrelloList>("FirstListOfBoard").id, DataDrivenTestHelper.Instance.APICardToCreate);
        }

        [Then(@"the card is successfully created")]
        public void ThenTheCardIsSuccessfullyCreated()
        {
            var api = new APIHelper();

            var card_names = api.GetAllCardsInList(_scenarioContext.Get<TrelloList>("FirstListOfBoard").id).Select(c => c.name);

            CollectionAssert.Contains(card_names, _scenarioContext.Get<string>("APIUsedCardName"), "Card created with the API is not within the list");
        }

    }
}
