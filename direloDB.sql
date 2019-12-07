CREATE TABLE Users (
  Id int PRIMARY KEY AUTO_INCREMENT,
  CPF blob,
  ReadAccess tinyint(1),
  WriteAccess tinyint(1),
  Name varchar(255)
);

CREATE TABLE Categories (
  Id int PRIMARY KEY AUTO_INCREMENT,
  Description varchar(255),
  LargeImagePath varchar(255),
  SmallImagePath varchar(255)
);

CREATE TABLE Products (
  Id int PRIMARY KEY AUTO_INCREMENT,
  MaterialId varchar(255),
  Description varchar(255),  
  CategoryId int,
  ExpirationDays int,
  STTax float,
  ImageId int NULL
);

CREATE TABLE ProductPrices (
  Id int PRIMARY KEY AUTO_INCREMENT,
  ProductId int,  
  Price float,
  Description varchar(255),
  IsPrimary tinyint(1)
);

CREATE TABLE ProductImages (
  Id int PRIMARY KEY AUTO_INCREMENT,
  ProductId int,
  ImagePath varchar(255)
);

CREATE TABLE Paletization (
  ProductId int PRIMARY KEY,
  BoxQuantity int,
  BoxLayerQuantity int,
  LayerPalletQuantity int
);

ALTER TABLE Products ADD FOREIGN KEY (CategoryId) REFERENCES Categories (Id);

ALTER TABLE ProductPrices ADD FOREIGN KEY (ProductId) REFERENCES Products (Id);

ALTER TABLE ProductImages ADD FOREIGN KEY (ProductId) REFERENCES Products (Id);

ALTER TABLE Products ADD FOREIGN KEY (ImageId) REFERENCES ProductImages (Id);

ALTER TABLE Paletization ADD FOREIGN KEY (ProductId) REFERENCES Products (Id);
