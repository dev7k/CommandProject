select * from "CommandItems"

insert into "CommandItems" ("HowTo", "Platform", "CommandLine")
values ('Create an EF migration', 'Entity Framework Core Command Line', 'dotnet ef migrations add');

insert into "CommandItems" ("HowTo", "Platform", "CommandLine")
values ('Apply Migrations to DB', 'Entity Framework Core Command Line', 'dotnet ef database update');

insert into "CommandItems" ("HowTo", "Platform", "CommandLine")
values ('Create an EF Migration', 'Entity Framework Package Manager Console', 'add-migration <name of migration>')

insert into "CommandItems" ("HowTo", "Platform", "CommandLine")
values ('Apply Migrations to DB', 'Entity Framework Package Manager Console', 'update database')
