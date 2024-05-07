Feature: Calculator
![Calculator](https://specflow.org/wp-content/uploads/2020/09/calculator.png)
Simple calculator for adding **two** numbers

Link to a feature: [Calculator](Sample.Spec/Features/Calculator.feature)
***Further read***: **[Learn more about how to generate Living Documentation](https://docs.specflow.org/projects/specflow-livingdoc/en/latest/LivingDocGenerator/Generating-Documentation.html)**

Background: 
	Given a calculator


Scenario Outline: Add two numbers
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
		| 5     | 6      | 11     |
		| 6     | 7      | 13     |
		| 7     | 8      | 15     |
		| 8     | 9      | 17     |
		| 9     | 10     | 19     |
		| 10    | 11     | 21     |
