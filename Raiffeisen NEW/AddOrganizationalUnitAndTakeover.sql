ALTER TABLE dbo.PartCodes 
ADD OrganizationalUnit varchar(50);
ALTER TABLE dbo.RWTable
ADD OrganizationalUnit varchar(50);

ALTER TABLE dbo.PartCodes 
ADD TakeoverDate date;
ALTER TABLE dbo.BankTable 
ADD TakeoverDate date;