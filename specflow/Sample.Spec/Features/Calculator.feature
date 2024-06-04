Feature: Calculator
![Calculator](https://specflow.org/wp-content/uploads/2020/09/calculator.png)
Simple calculator for adding **two** numbers

Link to a feature: [Calculator](Sample.Spec/Features/Calculator.feature)
***Further read***: **[Learn more about how to generate Living Documentation](https://docs.specflow.org/projects/specflow-livingdoc/en/latest/LivingDocGenerator/Generating-Documentation.html)**

Background: 
	Given a calculator


Scenario Outline: Add my numbers
	Given the first number is <first>
	And the second number is <second>
	When the two numbers are added
	Then the result should be <result>

	Examples: 
		| first | second | result |
		| 1     | 2      | 3      |
		| 2     | 3      | 5      |
		| 3     | 4      | 7      |
		| 4     | 5      | 9      |



Scenario Outline: Add your numbers
	Given the first number is <first>
	Then the result should be <result>

	Examples: 
		| first | result |
		| random int between 10 and 20     | 19      |
		| 4     | 4      |
