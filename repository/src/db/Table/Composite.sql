CREATE TABLE test.Composite
(
	App varchar(100) not null,
	Name varchar(100) not null,
	Value nvarchar(max) not null,
	constraint PK_Composite primary key clustered (App, Name),
)
