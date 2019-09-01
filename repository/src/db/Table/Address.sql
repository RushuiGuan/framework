create table test.[Address](
	AddressID int identity(1,1) not null,
	constraint PK_Address primary key clustered (AddressID),

	Street nvarchar(100),
	City nvarchar(100),
	[State] nvarchar(50),

	ContactID int not null,
	constraint FK_Address_ContactID foreign key (ContactID) references test.Contact(ContactID),

    Created datetime2 not null,
    CreatedBy int not null,
    Modified datetime2 not null,
    ModifiedBy int not null,
)
