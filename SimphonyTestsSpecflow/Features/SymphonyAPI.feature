@api
Feature: SimphonyAPI

Background: Login
	When I login with username "testuser34" and password "TestPassword123"
	Then the response status should be 200
	And I save the token from the response

@api
Scenario: _01_Register a new user
  
	When I register a new user with dynamic username and email
	Then the response status should be 201
@api
Scenario: _02_Login
	When I login with username "testuser34" and password "TestPassword123"
	Then the response status should be 200
	And I save the token from the response
@api
Scenario: _03_Create a post
	Given I am logged in
	When I create a post with title "Test Post" and content "This is a test post."
	Then the response status should be 201
	And I save the post ID from the response
@api
Scenario: _04_Add a comment to the post
	Given I have a post ID
	When I add a comment "This is a test comment."
	Then the response status should be 201
@api
Scenario: _05_Get comments for the post
	Given I have a post ID
	When I get comments for the post
	Then the response status should be 200
	And the response should contain the comment "This is a test comment."

    # Negative test cases
@api
Scenario: _06_Register a new user with missing fields
	When I register a new user with missing fields
	Then the response status should be 400
@api
Scenario: _07_Login with invalid credentials
	When I login with username "invaliduser" and password "InvalidPassword123"
	Then the response status should be 404
@api
Scenario: _08_Create a post without being logged in
	When I create a post with title "Test Post" and content "This is a test post." without login
	Then the response status should be 401
@api
Scenario: _09_Add a comment without post ID
	When I add a comment "This is a test comment." without post ID
	Then the response status should be 400
@api
Scenario: _10_Get comments for non-existing post
	When I get comments for a non-existing post
	Then the response status should be 404