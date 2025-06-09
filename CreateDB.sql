Use master
GO

create database eBanking

use eBanking
GO

create table BankAccounts
(
	BankAccountId INT PRIMARY KEY IDENTITY NOT NULL,
	IBAN VARCHAR(22) UNIQUE NOT NULL,
	Balance DECIMAL (16, 2) NOT NULL,
);

create table Users
(
	UserId INT PRIMARY KEY IDENTITY NOT NULL,
	Name NVARCHAR(100) NOT NULL,
	Username NVARCHAR(50) NOT NULL,
	Password VARCHAR(256) NOT NULL
);


create table Statuses
(
	StatusId INT PRIMARY KEY IDENTITY NOT NULL,
	Description NVARCHAR(20) NOT NULL
);


create table BankAccountsUsers
(
BankAccountId INT NOT NULL FOREIGN KEY REFERENCES BankAccounts(BankAccountId),
UserId INT NOT NULL FOREIGN KEY REFERENCES Users(UserId),
PRIMARY KEY (BankAccountId, UserId)
);



create table Payments
(
	PaymentId INT PRIMARY KEY IDENTITY NOT NULL,
	SenderBankAccountId INT  NOT NULL FOREIGN KEY REFERENCES BankAccounts(BankAccountId),
	SenderUserId INT NOT NULL FOREIGN KEY REFERENCES Users(UserId),
	RecieverIBAN VARCHAR(22) NOT NULL,
	RecieverName NVARCHAR(100) NOT NULL,
	Reference NVARCHAR(32) NOT NULL,
	Amount DECIMAL (16, 2) NOT NULL,
	StatusId INT NOT NULL FOREIGN KEY REFERENCES Statuses(StatusId),
);

--seed
INSERT INTO Users (Name, Username, Password)
VALUES 
('Alice Johnson', 'alice',	'6B86B273FF34FCE19D6B804EFF5A3F5747ADA4EAA22F1D49C01E52DDB7875B4B'),
('Bob Smith', 'bob',		'6B86B273FF34FCE19D6B804EFF5A3F5747ADA4EAA22F1D49C01E52DDB7875B4B'),
('Henry Ford', 'henry.f',	'6B86B273FF34FCE19D6B804EFF5A3F5747ADA4EAA22F1D49C01E52DDB7875B4B'),
('Isla Brown', 'isla.b',	'6B86B273FF34FCE19D6B804EFF5A3F5747ADA4EAA22F1D49C01E52DDB7875B4B'),
('Jack Davis', 'jack.d',	'6B86B273FF34FCE19D6B804EFF5A3F5747ADA4EAA22F1D49C01E52DDB7875B4B'),
('Scott Cahton', 'scotty',	'6B86B273FF34FCE19D6B804EFF5A3F5747ADA4EAA22F1D49C01E52DDB7875B4B'),
('Ted Scott', 'ted',		'6B86B273FF34FCE19D6B804EFF5A3F5747ADA4EAA22F1D49C01E52DDB7875B4B'),
('Desi Nikolova', 'dess',	'6B86B273FF34FCE19D6B804EFF5A3F5747ADA4EAA22F1D49C01E52DDB7875B4B'),
('Test Testov', 'test',		'6B86B273FF34FCE19D6B804EFF5A3F5747ADA4EAA22F1D49C01E52DDB7875B4B'),
('Phill Scott', 'phill',	'6B86B273FF34FCE19D6B804EFF5A3F5747ADA4EAA22F1D49C01E52DDB7875B4B')


INSERT INTO Statuses (Description)
VALUES
('»«◊¿ ¬¿'),
('Œ¡–¿¡Œ“≈Õ'),
('Œ“ ¿«¿Õ')

INSERT INTO BankAccounts (IBAN, Balance)
VALUES 
('BG10RBBG93004742491415', 15750.50),
('BG21RBBG80948981997524', 12000.98),
('BG96RBBG93005829749966', 1750.50),
('BG65RBBG94002696442852', 1850.80),
('BG49RBBG93001252492696', 15750.50),
('BG51RBBG94405467546519', 123.00),
('BG31RBBG94402438989211', 8700050.25),
('BG32RBBG70001698573187', 5430002.10),
('BG65RBBG93009238267862', 1002.00),
('BG77RBBG94008567166264', 12340.00),
('BG34RBBG94003941817874', 1234440.00),
('BG44RBBG70002398137941', 8944420.75),
('BG65RBBG25815696442852', 8777650.25)

INSERT INTO BankAccountsUsers (BankAccountId, UserId)
VALUES 
(1, 1),
(2, 8),
(3, 1 ),
(4, 3),
(5, 3),
(6, 4),
(7 , 5),
(8, 5),
(9, 6 ),
(10, 8),
(11, 8),
(12, 8),
(13, 8)


INSERT INTO Payments (SenderBankAccountId, SenderUserId, RecieverIBAN, RecieverName, Reference, Amount, StatusId) VALUES 
(1, 1, 'DE89370400440532013000', 'Jane Smith', 'Invoice #1001 - Services', 1250.00,  2),
(1, 1, 'INVALID123456789013456', 'Unknown Recipient', 'Test Payment - Invalid IBAN', 100.00,  3),
(2, 8, 'US64SVBKUS6S3300958879', 'Michael Johnson', 'Q1 Bonus Payment', 7500.00, 1),
(2, 8, 'FR14200410100505002606', 'Michael Johnson', 'Rent Payment January 2024', 1800.00, 2),
(2, 8, 'CH00000000000000003000', 'Insufficient Balance', 'Large Transfer - Failed', 50000.00, 3),
(3, 1, 'FR99999999999999999999', 'Non-existent Account', 'Failed Transfer Attempt', 500.00, 3),
(6, 4, 'IT60X05428111000123456', 'Emily Davis', 'Equipment Purchase', 3450.75, 2),
(7, 5, 'GB33BUKB20201555555555', 'David Wilson', 'Partnership Agreement Payment', 12000.00, 1),
(8, 8, 'ES91210004184500051332', 'David Wilson', 'Project Payment #2024-001', 5600.50,  2),
(13, 8,'DE00000000000000000000', 'Blocked Account', 'Payment to Suspended Account', 750.50, 3);

-- Summary Query to verify the data
SELECT 
    'BankAccounts' AS TableName, 
    COUNT(*) AS RecordCount,
    COUNT(DISTINCT UserId) AS UniqueUsers
FROM BankAccounts
UNION ALL
SELECT 
    'Payments' AS TableName, 
    COUNT(*) AS RecordCount,
    COUNT(DISTINCT SenderUserId) AS UniqueUsers
FROM Payments;

-- Status distribution in Payments
SELECT 
    s.Desctiption,
    COUNT(p.PaymentId) AS PaymentCount,
    SUM(p.Amount) AS TotalAmount
FROM Payments p
JOIN Statuses s ON p.StatusId = s.StatusId
GROUP BY s.StatusId, s.Desctiption
ORDER BY s.StatusId;

select * from BankAccounts where UserId = 8