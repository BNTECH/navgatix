-- 1. Add missing columns to Vehicles Table
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Vehicles]') AND name = 'CTTyreType')
BEGIN
    ALTER TABLE [dbo].[Vehicles] ADD [CTTyreType] INT NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Vehicles]') AND name = 'CTBodyType')
BEGIN
    ALTER TABLE [dbo].[Vehicles] ADD [CTBodyType] INT NULL;
END
GO

-- 2. Add missing columns to Drivers Table
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Drivers]') AND name = 'ProfileStatus')
BEGIN
    ALTER TABLE [dbo].[Drivers] ADD [ProfileStatus] NVARCHAR(50) NULL;
END
GO

-- 3. Add Requirement Columns to Bookings Table
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Bookings]') AND name = 'CT_VehicleType')
BEGIN
    ALTER TABLE [dbo].[Bookings] ADD [CT_VehicleType] INT NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Bookings]') AND name = 'CTBodyType')
BEGIN
    ALTER TABLE [dbo].[Bookings] ADD [CTBodyType] INT NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Bookings]') AND name = 'CTTyreType')
BEGIN
    ALTER TABLE [dbo].[Bookings] ADD [CTTyreType] INT NULL;
END
GO

-- 3. Seed/Update CommonType Data
-- Vehicle Types (VEHTYP)
IF NOT EXISTS (SELECT 1 FROM CommonType WHERE Code = 'VEHTYP')
BEGIN
    INSERT INTO CommonType (Name, Code, IsDeleted, CreatedDatetime) VALUES ('Vehicle Type', 'VEHTYP', 0, GETDATE());
END
GO

DECLARE @VehTypId INT = (SELECT Id FROM CommonType WHERE Code = 'VEHTYP');

-- Add Vehicle Types if missing
IF NOT EXISTS (SELECT 1 FROM CommonType WHERE Code = 'TATA_ACE' AND CTID = @VehTypId)
    INSERT INTO CommonType (Name, Code, IsDeleted, CreatedDatetime, CTID) VALUES ('Tata Ace/Chhota Hathi', 'TATA_ACE', 0, GETDATE(), @VehTypId);
IF NOT EXISTS (SELECT 1 FROM CommonType WHERE Code = 'MAH_PICKUP' AND CTID = @VehTypId)
    INSERT INTO CommonType (Name, Code, IsDeleted, CreatedDatetime, CTID) VALUES ('Mahindra Bolero Pickup', 'MAH_PICKUP', 0, GETDATE(), @VehTypId);
IF NOT EXISTS (SELECT 1 FROM CommonType WHERE Code = 'TRUCK_14FT' AND CTID = @VehTypId)
    INSERT INTO CommonType (Name, Code, IsDeleted, CreatedDatetime, CTID) VALUES ('14 ft Truck', 'TRUCK_14FT', 0, GETDATE(), @VehTypId);
IF NOT EXISTS (SELECT 1 FROM CommonType WHERE Code = 'TRUCK_32FT' AND CTID = @VehTypId)
    INSERT INTO CommonType (Name, Code, IsDeleted, CreatedDatetime, CTID) VALUES ('32 ft Truck', 'TRUCK_32FT', 0, GETDATE(), @VehTypId);

-- Body Types (BDTYP)
IF NOT EXISTS (SELECT 1 FROM CommonType WHERE Code = 'BDTYP')
BEGIN
    INSERT INTO CommonType (Name, Code, IsDeleted, CreatedDatetime) VALUES ('Body Type', 'BDTYP', 0, GETDATE());
END
GO

DECLARE @BdTypId INT = (SELECT Id FROM CommonType WHERE Code = 'BDTYP');

IF NOT EXISTS (SELECT 1 FROM CommonType WHERE Code = 'CONTAINER' AND CTID = @BdTypId)
    INSERT INTO CommonType (Name, Code, IsDeleted, CreatedDatetime, CTID) VALUES ('Container', 'CONTAINER', 0, GETDATE(), @BdTypId);
IF NOT EXISTS (SELECT 1 FROM CommonType WHERE Code = 'OPEN_BODY' AND CTID = @BdTypId)
    INSERT INTO CommonType (Name, Code, IsDeleted, CreatedDatetime, CTID) VALUES ('Open Body', 'OPEN_BODY', 0, GETDATE(), @BdTypId);

-- Vehicle Tyre (VEHTYR)
IF NOT EXISTS (SELECT 1 FROM CommonType WHERE Code = 'VEHTYR')
BEGIN
    INSERT INTO CommonType (Name, Code, IsDeleted, CreatedDatetime) VALUES ('Vehicle Tyre', 'VEHTYR', 0, GETDATE());
END
GO

DECLARE @VehTyrId INT = (SELECT Id FROM CommonType WHERE Code = 'VEHTYR');

IF NOT EXISTS (SELECT 1 FROM CommonType WHERE Name = '6 Tyre' AND CTID = @VehTyrId)
    INSERT INTO CommonType (Name, Code, IsDeleted, CreatedDatetime, CTID) VALUES ('6 Tyre', '6TYRE', 0, GETDATE(), @VehTyrId);
IF NOT EXISTS (SELECT 1 FROM CommonType WHERE Name = '10 Tyre' AND CTID = @VehTyrId)
    INSERT INTO CommonType (Name, Code, IsDeleted, CreatedDatetime, CTID) VALUES ('10 Tyre', '10TYRE', 0, GETDATE(), @VehTyrId);
GO
