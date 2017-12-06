CREATE TABLE ItemType (
    ItemTypeId int,
	Name nvarchar(50),
	CONSTRAINT ItemType_Id_PK PRIMARY KEY (ItemTypeId)
);

CREATE TABLE PropertyType (
    PropertyTypeId int,
	Name nvarchar(50),
	CONSTRAINT PropertyType_Id_PK PRIMARY KEY (PropertyTypeId)
);

CREATE TABLE Producer (
    Email nvarchar(150),
	Passphrase nvarchar(50),
	Name nvarchar(50),
	CONSTRAINT Producer_Email_PK PRIMARY KEY (Email)
);

GO

CREATE TABLE Item (
    ItemId int,
	Name nvarchar(50),
	ProductNumber nvarchar(150),
	ItemTypeId int,
	ProducerEmail nvarchar(150),
	CONSTRAINT Item_Id_PK PRIMARY KEY (ItemId),
	CONSTRAINT Item_ItemTypeId_FK FOREIGN KEY (ItemTypeId) REFERENCES ItemType(ItemTypeId),
	CONSTRAINT Item_ProducerEmail_FK FOREIGN KEY (ProducerEmail) REFERENCES Producer(Email)
);

CREATE TABLE Property (
    PropertyId int,
	Name nvarchar(50),
	PropertyTypeId int
	CONSTRAINT Property_Id_PK PRIMARY KEY (PropertyId),
	CONSTRAINT Property_PropertyTypeId_FK FOREIGN KEY (PropertyTypeId) REFERENCES PropertyType(PropertyTypeId)
);

GO

CREATE TABLE ItemPropertyValue (
    PropertyId int,
	ItemId int,
	Value nvarchar(256),	
	CONSTRAINT ItemPropertyValue_PropertyId_ItemId_PK PRIMARY KEY (PropertyId,ItemId),
	CONSTRAINT ItemPropertyValue_PropertyId_FK FOREIGN KEY (PropertyId) REFERENCES Property(PropertyId),
	CONSTRAINT ItemPropertyValue_ItemId_FK FOREIGN KEY (ItemId) REFERENCES Item(ItemId)
);

GO

CREATE TABLE ItemTypeDefaultProperty (
    ItemTypeId int,
	PropertyId int,
	Value nvarchar(256),
	CONSTRAINT ItemTypeDefaultProperty_ItemTypeId_PropertyId_PK PRIMARY KEY (ItemTypeId,PropertyId),
	CONSTRAINT ItemTypeDefaultProperty_ItemTypeId_FK FOREIGN KEY (ItemTypeId) REFERENCES ItemType(ItemTypeId),
	CONSTRAINT ItemTypeDefaultProperty_PropertyId_FK FOREIGN KEY (PropertyId) REFERENCES Property(PropertyId)
);
