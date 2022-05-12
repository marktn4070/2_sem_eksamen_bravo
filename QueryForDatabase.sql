IF db_id('HjemIs') IS NULL Create Database HjemIs
go

use HjemIs
CREATE TABLE Message (
    MessageID int IDENTITY(1,1) PRIMARY KEY,
    Headline varchar(200) NOT NULL,
    Subheadline varchar(200) NOT NULL,
    Text varchar(1500) NOT NULL,
    Time datetime NOT NULL,
    Email bit NOT NULL,
    Sms bit NOT NULL
); 

CREATE TABLE Address(
    VejkodeID int PRIMARY KEY not null,
    Road varchar(21) NOT NULL,
    Zip int NOT NULL,
    Municipality varchar(21) NOT NULL
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
    AddressID int NOT NULL FOREIGN KEY REFERENCES Address(VejkodeID)
);  

CREATE TABLE Message_history (
    MessageID int NOT NULL FOREIGN KEY REFERENCES Message(MessageID),
    CustomerID int NOT NULL FOREIGN KEY REFERENCES Customer(CustomerID)
);
