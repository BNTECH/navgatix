IF OBJECT_ID(N'[dbo].[PushDeviceTokens]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[PushDeviceTokens]
    (
        [Id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT [PK_PushDeviceTokens] PRIMARY KEY DEFAULT NEWID(),
        [UserId] VARCHAR(550) NOT NULL,
        [DeviceToken] VARCHAR(512) NOT NULL,
        [Platform] VARCHAR(20) NOT NULL,
        [DeviceId] VARCHAR(128) NULL,
        [IsActive] BIT NOT NULL CONSTRAINT [DF_PushDeviceTokens_IsActive] DEFAULT ((1)),
        [CreatedAt] DATETIME NOT NULL CONSTRAINT [DF_PushDeviceTokens_CreatedAt] DEFAULT (GETDATE()),
        [LastSeenAt] DATETIME NOT NULL CONSTRAINT [DF_PushDeviceTokens_LastSeenAt] DEFAULT (GETDATE())
    );

    CREATE UNIQUE INDEX [UX_PushDeviceTokens_DeviceToken]
        ON [dbo].[PushDeviceTokens]([DeviceToken]);

    ALTER TABLE [dbo].[PushDeviceTokens]
        ADD CONSTRAINT [FK_PushDeviceTokens_Users]
        FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id]) ON DELETE CASCADE;
END;
ELSE
BEGIN
    IF EXISTS (
        SELECT 1
        FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_NAME = 'PushDeviceTokens'
          AND COLUMN_NAME = 'UserId'
          AND CHARACTER_MAXIMUM_LENGTH <> 550
    )
    BEGIN
        ALTER TABLE [dbo].[PushDeviceTokens] ALTER COLUMN [UserId] VARCHAR(550) NOT NULL;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM sys.foreign_keys
        WHERE name = 'FK_PushDeviceTokens_Users'
          AND parent_object_id = OBJECT_ID(N'[dbo].[PushDeviceTokens]')
    )
    BEGIN
        ALTER TABLE [dbo].[PushDeviceTokens]
            ADD CONSTRAINT [FK_PushDeviceTokens_Users]
            FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id]) ON DELETE CASCADE;
    END;
END;
