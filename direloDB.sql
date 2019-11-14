CREATE TABLE Users (
  Id int PRIMARY KEY AUTO_INCREMENT,
  CPF blob,
  ReadAccess tinyint(1),
  WriteAccess tinyint(1)
);

CREATE TABLE Categories (
  Id int PRIMARY KEY AUTO_INCREMENT,
  Description varchar(255),
  ImageId varchar(255)
);

CREATE TABLE Products (
  Id int PRIMARY KEY AUTO_INCREMENT,
  MaterialId varchar(255),
  Description varchar(255),  
  CategoryId int,
  ExpirationDays int,
  UnitPrice float,
  STTax float,
  STPrice float,
  ImageId varchar(255)
);

CREATE TABLE ProductQuantity (
  ProductId int,
  Quantity int,
  Price float,
  PRIMARY KEY (ProductId, Quantity)
);

CREATE TABLE Paletization (
  ProductId int PRIMARY KEY,
  BoxQuantity int,
  BoxLayerQuantity int,
  LayerPalletQuantity int
);

ALTER TABLE Products ADD FOREIGN KEY (CategoryId) REFERENCES Categories (Id);

ALTER TABLE ProductQuantity ADD FOREIGN KEY (ProductId) REFERENCES Products (Id);

ALTER TABLE Paletization ADD FOREIGN KEY (ProductId) REFERENCES Products (Id);
