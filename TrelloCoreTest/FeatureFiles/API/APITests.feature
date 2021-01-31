@APIFeature
Feature: APITests
	In order to CRUD elements
	As a user
	I want to be able to make calls to trello API

@Browser:Headless
Scenario: Create a new Card in a Board through the API
	Given I create a new card in the first list of a board through the api
	Then the card is successfully created