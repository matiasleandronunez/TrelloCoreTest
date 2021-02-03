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
        private IWebDriver driver;
        private ScenarioContext scenarioContext;

        public APISteps(IWebDriver driver, ScenarioContext scenarioContext)
        {
            this.driver = driver;
            this.scenarioContext = scenarioContext;
        }

        [Given(@"I create a new card in the first list of a board through the api")]
        public void GivenICreateANewCardInTheFirstListOfABoardThroughTheApi()
        {
            var api = new APIHelper();
            scenarioContext.Add("BoardID", api.GetBoardIdByName(DataDrivenTestHelper.Instance.Board1ToCreate));
            scenarioContext.Add("FirstListOfBoard", api.GetAllListsInBoard(DataDrivenTestHelper.Instance.Board1ToCreate).First());
            scenarioContext.Add("APIUsedCardName", DataDrivenTestHelper.Instance.APICardToCreate);

            api.CreateCard(scenarioContext.Get<TrelloList>("FirstListOfBoard").id, DataDrivenTestHelper.Instance.APICardToCreate);
        }

        [Then(@"the card is successfully created")]
        public void ThenTheCardIsSuccessfullyCreated()
        {
            var api = new APIHelper();

            var cardNames = api.GetAllCardsInList(scenarioContext.Get<TrelloList>("FirstListOfBoard").id).Select(c => c.name);

            CollectionAssert.Contains(cardNames, scenarioContext.Get<string>("APIUsedCardName"), "Card created with the API is not within the list");
        }

    }
}
