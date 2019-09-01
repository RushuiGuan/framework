create table test.Contact(
	ContactID int identity(1,1) not null,
	constraint PK_Contact primary key clustered (ContactID),

	[Name] nvarchar(100) not null,
	constraint UQ_Contact_Name unique ([name]),

	Tag nvarchar(100),

    Created datetime2 not null,
    CreatedBy int not null,
    Modified datetime2 not null,
    ModifiedBy int not null,
)