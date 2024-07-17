Feature: SimphonyUI


Background: _01_Maximize browser window and navigate to Symphony website
	 Given I have launched the browser and maximized the window
	When I navigate to "https://symphony.is"
	Then the URL should be "https://symphony.is/"


@ui
Scenario: _01_Verify About Us -> Company page
	Given I am on the Symphony homepage
	When I click on "About Us" and select "Company"
	Then the URL should be "https://symphony.is/about-us/company"
	And the HQ should be "San Francisco"
	And the Founded year should be "2007"
	And the Consulting Offices should include the following locations
		| Location    |
		| Amsterdam   |
		| Berlin      |
		| Geneva      |
		| London      |
		| Los Angeles |
		| Madrid      |
		| New York    |
		| Portland    |
		| Zürich      |

	And the Engineering Hubs should include the following locations
		| Location      |
		| Banja Luka    |
		| Belgrade      |
		| Nis           |
		| Novi Sad      |
		| Santo Domingo |
		| Sarajevo      |
		| Skopje        |
	And the Clients should include the following count
		| Count |
		| 300+  |
@ui
Scenario:_02_Verify career openings
	Given I am on the Symphony homepage
	When I click on Careers menu item
	Then the number of open positions should be 5
@ui
Scenario: _03_Save job titles and locations to .txt file
	Given I am on the Symphony homepage
	When I click on Careers menu item
	Then I save all job titles and locations to a file named "job_positions.txt"
