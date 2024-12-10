CREATE TABLE Vehicles (
    VehicleID INT IDENTITY(1,1) PRIMARY KEY,
    RegistrationNumber NVARCHAR(20) UNIQUE NOT NULL
);



