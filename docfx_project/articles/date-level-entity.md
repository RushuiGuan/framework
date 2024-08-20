## Assembly
[Albatross.DateLevel](xref:Albatross.DateLevel)
## Overview
DateLevelEntity is a pattern that manages entity in a date series. It tracks the day over day changes of an entity by using the StartDate and EndDate columns.  By doing so it provides the ability to access the entity data using an effective date.  The effective date can be in the past, present or the future.

## Approach
### Date Level 1 Approach
A date level entity can be reprensented by a single date column: `StartDate`.  Consider the following `ProductIdentifier` table.  The product with the Id of 1 has the VendorProductCode of `AZ2222` on 2023-01-01.  But on 2023-09-22, its VendorProductCode was changed to `AZX222`.  The date level 1 entity has a relative simple write model because the end date of an entity is determined automatically by the next entry.  However, it has a relatively complex read model.  To determine the VendorProductCode of product 1 on 2023-05-01, the system has to sort the table by start date and read at least 2 records.  

`ProductIdentifier` Table

|ProductId|VendorProductCode|StartDate|
|---|---|---|
|1|AZ2222|2023-01-01|
|1|AZX222|2023-09-22|
|2|AC23124|2022-01-01|

### Date Level 2 Approach
The date level 2 approach put both `StartDate` and `EndDate` on the entity as the example is shown below.  This is the preferred approach where there are few writes and many reads.  **Albatross.DataLevel namespace implements the DateLevel pattern using the DataLevel2 approach!**

`ProductIdentifier` Table

|ProductId|VendorProductCode|StartDate|EndDate|
|---|---|---|---|
|1|AZ2222|2023-01-01|2023-09-21|
|1|AZX222|2023-09-22|9999-12-31|
|2|AC23124|2022-01-01|9999-12-31|

The date level 2 approach has a more complex write since the program has to determine the proper `end date`.  But the read operation is simple.  To get the VendorProductCode effective 2023-05-01, the system can run a simple query: 
```sql
select * 
from ProductIdentifier 
where Id = 1 
	and StartDate <= '2023-05-01' 
	and EndDate <= '2023-05-01'
```
## Use Cases
### Who wants to make the changes tomorrow at 12am?
A program talks to a vendor api to download the price of products.  The vendor made an anouncement that there will be a product name change effective tonight at 12am.  To ensure that the program works 24/7 without a glitch, your boss has asked you to wake up at 12am and make that change in the database.  But since you have the foresight to implement the product identifier as a date level entity, you simply create a new entry in the system with a start date of tomorrow and everything should work as expected!

`Product` Table

|Id|Name|
|---|---|
|1|Product A|
|2|Product B|

`ProductIdentifier` Table

|ProductId|VendorProductCode|StartDate|EndDate|
|---|---|---|---|
|1|AZ2222|2023-01-01|2023-09-21|
|1|AZX222|2023-09-22|9999-12-31|
|2|AC23124|2022-01-01|9999-12-31|

In the example above, the `ProductIdentifier` table is a date level entity.  It has a natural key of `(ProductId, StartDate)`.  When the system query the `ProductIdentifier` table with an effective date:
```sql
declare @effectiveDate datetime = '2023-09-22'
select * from ProductIdentifier
where ProductId = 1 
	and StartDate <= @effectiveDate 
	and @effectiveDate <= EndDate 
```
It will always return the correct VendorProductCode for the date asked.

## Rules
Two Rules exists for Date Level Entities
1. **A date level entity should have no gap between the first StartDate and the max end date (9999-12-31).**  
The `ProductIdentifier` entity for `Product A` below broke the rule since it is missing data after `2023-03-21`.  There is no clarity from the data to figure out what happens after 2023-03-21.  While it is possible that program logic can be used in its place.  The design mandates that data must provide clarity.

	|ProductId|VendorProductCode|StartDate|EndDate|
	|---|---|---|---|
	|1|AZ2222|2023-01-01|2023-02-21|
	|1|AZX222|2023-02-22|2023-03-21|


1. **A date level entity should not have overlap of dates among its values.**  
The date level entry for an entity doesn't make sense if its dates are overlapped.  In the example below, the VendorProductCode for `Product A` on 2023-02-21 has two values since the end date of the first entry overlaps with the start date of the second entry.  

	|ProductId|VendorProductCode|StartDate|EndDate|
	|---|---|---|---|
	|1|AZ2222|2023-01-01|2023-02-21|
	|1|AZX222|2023-02-21|9999-12-31|

## Model
[DateLevelEntity](xref:Albatross.DateLevel.DateLevelEntity) should be used as the base class of any date level entity.  