CREATE TABLE Users (
	UserId INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	Username NVARCHAR(20) NOT NULL UNIQUE,
	Password NVARCHAR(20) NOT NULL,
	UserType NVARCHAR(20) NOT NULL DEFAULT 'User',
	IsEnabled BIT NOT NULL DEFAULT 1
);

CREATE TABLE Calendar (
	CalendarId INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	UserId INT NOT NULL,
	TaskId INT NOT NULL,
	TaskDescription TEXT,
	DateCreated DATE DEFAULT GETDATE(),
	LastModified DATE DEFAULT GETDATE(),
	IsDeleted BIT NOT NULL DEFAULT 0
);

CREATE TABLE Tasks (
	TaskId INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	TaskName NVARCHAR(30),
	IsActive BIT NOT NULL DEFAULT 1
);

CREATE TABLE Notes (
	NoteId INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	UserId INT NOT NULL UNIQUE,
	NoteContent TEXT
);