CREATE TABLE Message (
    MessageID int IDENTITY(1,1) PRIMARY KEY,
    Headline varchar(200) NOT NULL,
    Text varchar(1500) NOT NULL,
    Time datetime NOT NULL
); 

CREATE TABLE Address(
    AddressID int IDENTITY(1,1) PRIMARY KEY,
    Road varchar(64) NOT NULL,
    Zip int NOT NULL,
    City varchar(64) NOT NULL,
    Roadcode int NOT NULL,
    Municipality int NOT NULL
);

CREATE TABLE Customer (
    CustomerID int IDENTITY(1,1) PRIMARY KEY,
    FirstName varchar(64) NOT NULL,
    LastName varchar(64) NOT NULL,
    Registered bit NOT NULL,
    Gender varchar(6) NOT NULL,
    Birth date NOT NULL,
    Phone int NOT NULL,
    Email varchar(64) NOT NULL,
    AddressID int NOT NULL FOREIGN KEY REFERENCES Address(AddressID)
);  

CREATE TABLE Message_history (
    MessageID int NOT NULL FOREIGN KEY REFERENCES Message(MessageID),
    CustomerID int NOT NULL FOREIGN KEY REFERENCES Customer(CustomerID)
); 