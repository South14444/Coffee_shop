create table Coffee(
	ID int not null identity(1,1) constraint PK_CoffeeID primary key,
	Name nvarchar(30) not null,
	Type_of_coffeeId int not null,
	Manufacturer_sideId int not null,
	Description nvarchar(30) not null,
	Number_of_grams int not null,
	Cost_price money not null
);
go

create table Manufacturer_side(
	ID int not null identity(1,1) constraint PK_Manufacturer_sideID primary key,
	Name nvarchar(30) not null 
);
go

create table Type_of_coffee(
	ID int not null identity(1,1) constraint PK_Type_of_coffeeID primary key,
	Name nvarchar(30) not null 
);
go

alter table Coffee add foreign key ([Type_of_coffeeId]) references [Type_of_coffee](ID);
go
alter table Coffee add foreign key ([Manufacturer_sideId]) references [Manufacturer_side](ID);
go


INSERT INTO Manufacturer_side(Name)
VALUES (N'Бразилия'),
(N'Колумбия'),
(N'Вьетнам');

INSERT INTO Type_of_coffee(Name)
VALUES (N'Арабика'),
(N'Робуста'),
(N'Сидамо');


INSERT INTO Coffee(Name,Type_of_coffeeId,Manufacturer_sideId,Description,Number_of_grams,Cost_price)
values (N'zxz',1,1,N'Крутой вкус',1057,100),
(N'1',2,1,N'Крутой вкус',456,445),
(N'2',3,1,N'Крутой вкус',46353,43333),
(N'3',1,2,N'Крутой вкус',7856,453),
(N'4',2,2,N'Крутой вкус',7,8321),
(N'5',3,2,N'Крутой вкус',876,453),
(N'6',2,3,N'Крутой вкус',433,45343),
(N'7',3,3,N'Крутой вкус',486,4),
(N'8',1,3,N'Крутой вкус',333,86)