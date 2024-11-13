Feature: Calculator

@mytag
Scenario: Test Accessing of Object Property
	Given create a random object of type MyClass => "MyObject"
	And Verify that the Name property of the object MyObject equal context:MyObject.Name
	And Verify that the Value property of the object MyObject equal context:MyObject.Value
	And Verify that the Age property of the object MyObject equal context:MyObject.Age
	Then Test the TimeOnly parameter: 10:00
	Then Show value of context:MyObject.Name
