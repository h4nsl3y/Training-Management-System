Feature: SignUpAsAManager
	Testing base specflow on chrome - signining as a manager

@tag1
Scenario: Add items to the ToDoApp - Chrome
	Given I navigate to TrainingManagementSystem App on following environment
		| Browser | BrowserVersion | OS         |
		| Chrome  | 121.0.6167.85  | Windows 11 |
	And I wait for DOM to be ready
	And I click on Sign Up button
	And I wait for DOM to be ready
	And I Fill the form
		| Key                | Value           |
		| FirstName          | firstName       |
		| LastName           | lastName        |
		| IdentificationNber | T00123456787654 |
		| MobileNumber       | 51234567        |
		| Email              | Email           |
		| Role               | Manager         |
		| Password           | password1*      |
	And I click on Sign Up button
	And I select role 
		| Key  | Value   |
		| Role | Manager |
	And I click on Submit button and wait for DOM to be ready
	Then I verify whether the item is added to the list 


