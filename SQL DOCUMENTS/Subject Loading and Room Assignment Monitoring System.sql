/* ================================
   DATABASE
================================ */
IF DB_ID('Schooldb') IS NOT NULL
    DROP DATABASE Schooldb;
GO

CREATE DATABASE Schooldb;
GO

USE Schooldb;
GO


/* ================================
   LOGIN TABLE
================================ */
CREATE TABLE log_in (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    Password NVARCHAR(50) NOT NULL
);
GO

INSERT INTO log_in (Username, Password)
VALUES ('admin', '123');
GO


/* ================================
   REGISTER TABLE
================================ */
CREATE TABLE register (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    Password NVARCHAR(50) NOT NULL
);
GO


/* ================================
   PROGRAM TABLE
================================ */
CREATE TABLE tblProgram (
    ProgramID INT IDENTITY(1,1) PRIMARY KEY,
    ProgramName NVARCHAR(50) NOT NULL
);
GO

INSERT INTO tblProgram (ProgramName)
VALUES
('BSIT'),
('BSCS'),
('BSA');
GO


/* ================================
   DEPARTMENT TABLE
================================ */
CREATE TABLE tblDepartment (
    DepartmentID INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentName NVARCHAR(50) NOT NULL,
    ProgramID INT NOT NULL,
    CONSTRAINT FK_tblDepartment_Program
        FOREIGN KEY (ProgramID)
        REFERENCES tblProgram (ProgramID)
);
GO

INSERT INTO tblDepartment (DepartmentName, ProgramID)
VALUES
('CCE', 2),
('CAE', 1),
('CBAE', 3);
GO


/* ================================
   SUBJECT TABLE
================================ */
CREATE TABLE tblsubject (
    subjectId INT IDENTITY(1,1) PRIMARY KEY,
    Code INT NOT NULL,
    Title NVARCHAR(50) NOT NULL,
    LectureUnits INT NOT NULL,
    LaboratoryUnits INT NOT NULL,
    DepartmentID INT NOT NULL,
    CONSTRAINT FK_tblsubject_Department
        FOREIGN KEY (DepartmentID)
        REFERENCES tblDepartment (DepartmentID)
);
GO

INSERT INTO tblsubject (Code, Title, LectureUnits, LaboratoryUnits, DepartmentID)
VALUES
(101, 'Intro to Programming', 3, 1, 1),
(102, 'Data Structures', 3, 1, 2);
GO


/* ================================
   SUBJECT OFFERING TABLE
================================ */
CREATE TABLE tblsubjectOffering (
    offeringId INT IDENTITY(1,1) PRIMARY KEY,
    Semester NVARCHAR(50) NOT NULL,
    SchoolYear NVARCHAR(50) NOT NULL,
    subjectId INT NOT NULL,
    CONSTRAINT FK_tblsubjectOffering_Subject
        FOREIGN KEY (subjectId)
        REFERENCES tblsubject (subjectId)
);
GO

INSERT INTO tblsubjectOffering (Semester, SchoolYear, subjectId)
VALUES
('1st Semester', '2024-2025', 1),
('2nd Semester', '2024-2025', 2);
GO


/* ================================
   VERIFY DATA
================================ */
SELECT * FROM log_in;
SELECT * FROM register;
SELECT * FROM tblProgram;
SELECT * FROM tblDepartment;
SELECT * FROM tblsubject;
SELECT * FROM tblsubjectOffering;
GO