Feature: SignUpAsAManager
	Testing base specflow on chrome - signining as a manager

@tag1
Scenario: Add items to the ToDoApp - Chrome
	Given I navigate to TrainingManagementSystem App on following environment
		| Key     | Value                   |
		| Browser | Chrome					| 
		| URL	  |  https://localhost:81/  |
	And I wait for DOM to be ready
	And I click on Sign Up button
	And I wait for DOM to be ready
	And I Fill the form
		| Key                | Value                  |
		| FirstName          | firstName              |
		| LastName           | lastName               |
		| IdentificationNber | T0123456787654         |
		| MobileNumber       | 51234567               |
		| Email              | Email@email.com        |
		| Department         | Product and Technology |
		| Role               | Manager                |
		| Password           | password1*             |
	And I click on Sign Up button
	And I select role 
		| Key  | Value   |
		| Role | Manager |
	When I click on Submit button and wait for DOM to be ready
	Then correct element should be displayed
		| Key                | Value                  |
		| FirstName          | firstName              |
		| LastName           | lastName               |

