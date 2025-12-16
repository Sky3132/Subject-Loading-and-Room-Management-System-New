create table log_in (
Id int identity (1,1)primary key,
Username nvarchar(50),
Password nvarchar(50)
);

Select * from log_in

insert into log_in (Id ,Username ,Password )
values (1, 'admin', '123')

set identity_insert log_in ON

create table register (
Id int identity(1,1) primary key,
Username nvarchar(50),
Password nvarchar(50)
);

create table tblsubject (
subjectId int identity(1,1) primary key,
Code int,
Title nvarchar(50),
Units int,
Department nvarchar(50),
Program nvarchar(50)
);

select * from tblsubject

insert into tblsubject (subjectId,Code ,Title, Units, Department, Program)
values (1, 2080,'IT/14', 6, 'DCE', 'BSIT');

drop table tblsubject
drop table register

select * from register

set identity_insert tblsubject ON

