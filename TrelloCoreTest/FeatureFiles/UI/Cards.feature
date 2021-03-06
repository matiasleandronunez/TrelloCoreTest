﻿@TrelloTestFeature
Feature: Cards
	In order to organize tasks
	As a user
	I want to be able to manage cards in Trello

@Browser:Grid_Chrome
@Browser:Grid_Firefox
Scenario: Create a new Card in a Board
	Given I log into Trello as an admin with an Atlassian Account
	When I open a trello board
		And I create a card in the first left hand side list
	Then the new card is successfully created