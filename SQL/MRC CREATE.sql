
-- -----------------------------------------------------------------------------------------------------------------------
-- SCHEMAS
-- -----------------------------------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = 'Links' ) 
    BEGIN
        EXEC SP_EXECUTESQL N'CREATE SCHEMA Links'
    END

IF NOT EXISTS (SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = 'Lookup' ) 
    BEGIN
        EXEC SP_EXECUTESQL N'CREATE SCHEMA Lookup'
    END

IF NOT EXISTS (SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = 'People' ) 
    BEGIN
        EXEC SP_EXECUTESQL N'CREATE SCHEMA People'
    END

IF NOT EXISTS (SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = 'Runbook' ) 
    BEGIN
        EXEC SP_EXECUTESQL N'CREATE SCHEMA Runbook'
    END

-- -----------------------------------------------------------------------------------------------------------------------
-- TABLES
-- -----------------------------------------------------------------------------------------------------------------------

-- -----------------------------------------------------------------------------------------------------------------------
-- [Links]
-- -----------------------------------------------------------------------------------------------------------------------

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Links' AND TABLE_NAME = 'ApplicationGroup'))
    BEGIN
        CREATE TABLE [Links].[ApplicationGroup] (
            [Id]                   INT          IDENTITY (1, 1) NOT NULL,
            [Name]                 VARCHAR (20) NOT NULL,
            PRIMARY KEY CLUSTERED ([Id] ASC)
        );
    END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Links' AND TABLE_NAME = 'ApplicationLink'))
    BEGIN
        CREATE TABLE [Links].[ApplicationLink] (
            [Id]                   INT            IDENTITY (1, 1) NOT NULL,
            [ApplicationGroupId]   INT            NULL,
            [Name]                 VARCHAR (250)  NULL,
            [AppTypeCode]          VARCHAR (20)   NULL,
            [Notes]                VARCHAR (2000) NULL,
            PRIMARY KEY CLUSTERED ([Id] ASC)
        );
    END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Links' AND TABLE_NAME = 'Environment'))
    BEGIN
        CREATE TABLE [Links].[Environment] (
            [Id]                   INT         IDENTITY (1, 1) NOT NULL,
            [Name]                 VARCHAR (5) NULL,
            [SortOrder]            INT         NULL,
            PRIMARY KEY CLUSTERED ([Id] ASC)
        );
    END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Links' AND TABLE_NAME = 'EnvironmentLink'))
    BEGIN
        CREATE TABLE [Links].[EnvironmentLink] (
            [Id]                   INT           IDENTITY (1, 1) NOT NULL,
            [ServiceLinkId]        INT           NULL,
            [ServerId]             INT           NULL,
            [Url]                  VARCHAR (500) NULL,
            [FolderPath]           VARCHAR (250) NULL,
            PRIMARY KEY CLUSTERED ([Id] ASC)
        );
    END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Links' AND TABLE_NAME = 'Server'))
    BEGIN
        CREATE TABLE [Links].[Server] (
            [Id]                   INT          IDENTITY (1, 1) NOT NULL,
            [Name]                 VARCHAR (50) NULL,
            [EnvironmentId]        INT          NULL,
            PRIMARY KEY CLUSTERED ([Id] ASC)
        );
    END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Links' AND TABLE_NAME = 'ServiceLink'))
    BEGIN
        CREATE TABLE [Links].[ServiceLink] (
            [Id]                   INT          IDENTITY (1, 1) NOT NULL,
            [ApplicationLinkId]    INT          NOT NULL,
            [ServiceName]          VARCHAR (50) NULL,
            PRIMARY KEY CLUSTERED ([Id] ASC)
        );
    END

-- -----------------------------------------------------------------------------------------------------------------------
-- [Lookup]
-- -----------------------------------------------------------------------------------------------------------------------

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Lookup' AND  TABLE_NAME = 'ApplicationType'))
    BEGIN
        CREATE TABLE [Lookup].[ApplicationType] (
            [Code]        VARCHAR (20) NOT NULL,
            [Description] VARCHAR (50) NULL,
            [SortOrder]   INT          NULL,
            PRIMARY KEY CLUSTERED ([Code] ASC)
        );
    END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Lookup' AND TABLE_NAME = 'RunbookStepStatus'))
    BEGIN
        CREATE TABLE [Lookup].[RunbookStepStatus] (
            [Code]        VARCHAR (20) NOT NULL,
            [Description] VARCHAR (50) NOT NULL,
            [SortOrder]   INT          NULL,
            PRIMARY KEY CLUSTERED ([Code] ASC)
        );
    END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Lookup' AND TABLE_NAME = 'RunbookStepType'))
    BEGIN
        CREATE TABLE [Lookup].[RunbookStepType] (
            [Code]        VARCHAR (20) NOT NULL,
            [Description] VARCHAR (50) NULL,
            [SortOrder]   INT          NULL,
            PRIMARY KEY CLUSTERED ([Code] ASC)
        );
    END

-- -----------------------------------------------------------------------------------------------------------------------
-- [People]
-- -----------------------------------------------------------------------------------------------------------------------

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'People' AND TABLE_NAME = 'Contact'))
    BEGIN
        CREATE TABLE [People].[Contact] (
            [Id]          INT           IDENTITY (1, 1) NOT NULL,
            [UserId]      INT           NULL,
            [DisplayName] VARCHAR (100) NULL,
            PRIMARY KEY CLUSTERED ([Id] ASC)
        );
    END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'People' AND TABLE_NAME = 'Team'))
    BEGIN
        CREATE TABLE [People].[Team] (
            [Id]   INT          IDENTITY (1, 1) NOT NULL,
            [Name] VARCHAR (50) NOT NULL,
            PRIMARY KEY CLUSTERED ([Id] ASC)
        );
    END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'People' AND TABLE_NAME = 'TeamMember'))
    BEGIN
        CREATE TABLE [People].[TeamMember] (
            [TeamId]    INT NOT NULL,
            [ContactId] INT NOT NULL
        );
    END

-- -----------------------------------------------------------------------------------------------------------------------
-- [Runbook]
-- -----------------------------------------------------------------------------------------------------------------------

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Runbook' AND TABLE_NAME = 'Rfc'))
    BEGIN
        CREATE TABLE [Runbook].[Rfc] (
            [Id]        INT          IDENTITY (1, 1) NOT NULL,
            [Number]    INT          NOT NULL,
            [Name]      VARCHAR (50) NOT NULL,
            [StartTime] DATETIME     NULL,
            [EndTime]   DATETIME     NULL,
            [ContactId] INT          NULL,
            PRIMARY KEY CLUSTERED ([Id] ASC)
        );
    END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Runbook' AND TABLE_NAME = 'RunbookStep'))
    BEGIN
        CREATE TABLE [Runbook].[RunbookStep] (
            [Id]                    INT                IDENTITY (1, 1) NOT NULL,
            [TemplateId]            INT                NULL,
            [RfcId]                 INT                NULL,
            [GroupNumber]           INT                NULL,
            [StepNumber]            INT                NULL,
            [Duration]              INT                NULL,
            [Name]                  VARCHAR (100)      NULL,
            [Description]           VARCHAR (2000)     NULL,
            [Notes]                 VARCHAR (2000)     NULL,
            [Time]                  DATETIMEOFFSET (7) NULL,
            [IsHtml]                BIT                NULL,
            [RunbookStepTypeCode]   VARCHAR (20)       NULL,
            [RunbookStepStatusCode] VARCHAR (20)       NULL,
            PRIMARY KEY CLUSTERED ([Id] ASC)
        );
    END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Runbook' AND TABLE_NAME = 'RunbookStepDeveloper'))
    BEGIN
        CREATE TABLE [Runbook].[RunbookStepDeveloper] (
            [RunbookStepId] INT NOT NULL,
            [ContactId]     INT NOT NULL,
            CONSTRAINT [PK_RunbookStepDeveloper] PRIMARY KEY CLUSTERED ([RunbookStepId] ASC, [ContactId] ASC)
        );
    END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Runbook' AND TABLE_NAME = 'RunbookStepPbi'))
    BEGIN
        CREATE TABLE [Runbook].[RunbookStepPbi] (
            [RunbookStepId] INT NOT NULL,
            [PbiNumber]     INT NOT NULL,
            CONSTRAINT [PK_RunbookStepPbi] PRIMARY KEY CLUSTERED ([RunbookStepId] ASC, [PbiNumber] ASC)
        );
    END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Runbook' AND TABLE_NAME = 'RunbookStepResource'))
    BEGIN
        CREATE TABLE [Runbook].[RunbookStepResource] (
            [RunbookStepId] INT NOT NULL,
            [ContactId]     INT NOT NULL,
            CONSTRAINT [PK_RunbookStepResource] PRIMARY KEY CLUSTERED ([RunbookStepId] ASC, [ContactId] ASC)
        );
    END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Runbook' AND TABLE_NAME = 'RunbookStepTeam'))
    BEGIN
        CREATE TABLE [Runbook].[RunbookStepTeam] (
            [RunbookStepId] INT NOT NULL,
            [TeamId]        INT NOT NULL,
            CONSTRAINT [PK_RunbookStepTeam] PRIMARY KEY CLUSTERED ([RunbookStepId] ASC, [TeamId] ASC)
        );
    END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Runbook' AND TABLE_NAME = 'RunbookTemplate'))
    BEGIN
        CREATE TABLE [Runbook].[RunbookTemplate] (
            [Id]   INT          IDENTITY (1, 1) NOT NULL,
            [Name] VARCHAR (50) NOT NULL,
            PRIMARY KEY CLUSTERED ([Id] ASC)
        );
    END

-- -----------------------------------------------------------------------------------------------------------------------
-- CONSTRAINTS
-- -----------------------------------------------------------------------------------------------------------------------

-- -----------------------------------------------------------------------------------------------------------------------
-- [Links].[ApplicationLink]
-- -----------------------------------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='ApplicationLink_ApplicationGroup_FK')
    BEGIN
           ALTER TABLE [Links].[ApplicationLink]
        ADD CONSTRAINT [ApplicationLink_ApplicationGroup_FK]
           FOREIGN KEY ([ApplicationGroupId])
            REFERENCES [Links].[ApplicationGroup] ([Id])
    END
         PRINT 'OK: [ApplicationLink_ApplicationGroup_FK] CONSTRAINT CREATED'

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='ApplicationLink_ApplicationType_FK')
    BEGIN
           ALTER TABLE [Links].[ApplicationLink]
        ADD CONSTRAINT [ApplicationLink_ApplicationType_FK]
           FOREIGN KEY ([AppTypeCode])
            REFERENCES [Lookup].[ApplicationType] ([Code])
    END
         PRINT 'OK: [ApplicationLink_ApplicationType_FK] CONSTRAINT CREATED'

GO

-- -----------------------------------------------------------------------------------------------------------------------
-- [Links].[EnvironmentLink]
-- -----------------------------------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='EnvironmentLink_ServiceLink_FK')
    BEGIN
           ALTER TABLE [Links].[EnvironmentLink]
        ADD CONSTRAINT [EnvironmentLink_ServiceLink_FK]
           FOREIGN KEY ([ServiceLinkId])
            REFERENCES [Links].[ServiceLink] ([Id])
    END
         PRINT 'OK: [EnvironmentLink_ServiceLink_FK] CONSTRAINT CREATED'

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='EnvironmentLink_Server_FK')
    BEGIN
           ALTER TABLE [Links].[EnvironmentLink]
        ADD CONSTRAINT [EnvironmentLink_Server_FK]
           FOREIGN KEY ([ServerId])
            REFERENCES [Links].[Server] ([Id])
    END
         PRINT 'OK: [EnvironmentLink_Server_FK] CONSTRAINT CREATED'

GO

-- -----------------------------------------------------------------------------------------------------------------------
-- [Links].[Server]
-- -----------------------------------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='Server_Environment_FK')
    BEGIN
           ALTER TABLE [Links].[Server]
        ADD CONSTRAINT [Server_Environment_FK]
           FOREIGN KEY ([EnvironmentId])
            REFERENCES [Links].[Environment] ([Id])
    END
         PRINT 'OK: [Server_Environment_FK] CONSTRAINT CREATED'

GO

-- -----------------------------------------------------------------------------------------------------------------------
-- [Links].[ServiceLink]
-- -----------------------------------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='ServiceLink_ApplicationLink_FK')
    BEGIN
           ALTER TABLE [Links].[ServiceLink]
        ADD CONSTRAINT [ServiceLink_ApplicationLink_FK]
           FOREIGN KEY ([ApplicationLinkId])
            REFERENCES [Links].[ApplicationLink] ([Id])
    END
         PRINT 'OK: [ServiceLink_ApplicationLink_FK] CONSTRAINT CREATED'

GO

-- -----------------------------------------------------------------------------------------------------------------------
-- [People].[TeamMember]
-- -----------------------------------------------------------------------------------------------------------------------

-- NONE

-- -----------------------------------------------------------------------------------------------------------------------
-- [Runbook].[Rfc]
-- -----------------------------------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='Rfc_Contact_FK')
    BEGIN
           ALTER TABLE [Runbook].[Rfc]
        ADD CONSTRAINT [Rfc_Contact_FK]
           FOREIGN KEY ([ContactId])
            REFERENCES [People].[Contact] ([Id])
    END
         PRINT 'OK: [Rfc_Contact_FK] CONSTRAINT CREATED'

GO

-- -----------------------------------------------------------------------------------------------------------------------
-- [Runbook].[RunbookStep]
-- -----------------------------------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='RunbookStep_Rfc_FK')
    BEGIN
           ALTER TABLE [Runbook].[RunbookStep]
        ADD CONSTRAINT [RunbookStep_Rfc_FK]
           FOREIGN KEY ([RfcId])
            REFERENCES [Runbook].[Rfc] ([Id])
    END
         PRINT 'OK: [RunbookStep_Rfc_FK] CONSTRAINT CREATED'

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='RunbookStep_RunbookStepStatus_FK')
    BEGIN
           ALTER TABLE [Runbook].[RunbookStep]
        ADD CONSTRAINT [RunbookStep_RunbookStepStatus_FK]
           FOREIGN KEY ([RunbookStepStatusCode])
            REFERENCES [Lookup].[RunbookStepStatus] ([Code])
    END
         PRINT 'OK: [RunbookStep_RunbookStepStatus_FK] CONSTRAINT CREATED'

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='RunbookStep_RunbookStepType_FK')
    BEGIN
           ALTER TABLE [Runbook].[RunbookStep]
        ADD CONSTRAINT [RunbookStep_RunbookStepType_FK]
           FOREIGN KEY ([RunbookStepTypeCode])
            REFERENCES [Lookup].[RunbookStepType] ([Code])
    END
         PRINT 'OK: [RunbookStep_RunbookStepType_FK] CONSTRAINT CREATED'

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='RunbookStep_RunbookTemplate_FK')
    BEGIN
           ALTER TABLE [Runbook].[RunbookStep]
        ADD CONSTRAINT [RunbookStep_RunbookTemplate_FK]
           FOREIGN KEY ([TemplateId])
            REFERENCES [Runbook].[RunbookTemplate] ([Id])
    END
         PRINT 'OK: [RunbookStep_RunbookTemplate_FK] CONSTRAINT CREATED'

GO

-- -----------------------------------------------------------------------------------------------------------------------
-- [Runbook].[RunbookStepDeveloper]
-- -----------------------------------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='RunbookStepDeveloper_Contact_FK')
    BEGIN
           ALTER TABLE [Runbook].[RunbookStepDeveloper]
        ADD CONSTRAINT [RunbookStepDeveloper_Contact_FK]
           FOREIGN KEY ([ContactId])
            REFERENCES [People].[Contact] ([Id])
    END
         PRINT 'OK: [RunbookStepDeveloper_Contact_FK] CONSTRAINT CREATED'

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='RunbookStepDeveloper_RunbookStep_FK')
    BEGIN
           ALTER TABLE [Runbook].[RunbookStepDeveloper]
        ADD CONSTRAINT [RunbookStepDeveloper_RunbookStep_FK]
           FOREIGN KEY ([RunbookStepId])
            REFERENCES [Runbook].[RunbookStep] ([Id])
    END
         PRINT 'OK: [RunbookStepDeveloper_RunbookStep_FK] CONSTRAINT CREATED'

GO

-- -----------------------------------------------------------------------------------------------------------------------
-- [Runbook].[RunbookStepPbi]
-- -----------------------------------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='RunbookStepPbi_RunbookStep_FK')
    BEGIN
           ALTER TABLE [Runbook].[RunbookStepPbi]
        ADD CONSTRAINT [RunbookStepPbi_RunbookStep_FK]
           FOREIGN KEY ([RunbookStepId])
            REFERENCES [Runbook].[RunbookStep] ([Id])
    END
         PRINT 'OK: [RunbookStepPbi_RunbookStep_FK] CONSTRAINT CREATED'

GO

-- -----------------------------------------------------------------------------------------------------------------------
-- [Runbook].[RunbookStepResource]
-- -----------------------------------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='RunbookStepResource_RunbookStep_FK')
    BEGIN
           ALTER TABLE [Runbook].[RunbookStepResource]
        ADD CONSTRAINT [RunbookStepResource_RunbookStep_FK]
           FOREIGN KEY ([RunbookStepId])
            REFERENCES [Runbook].[RunbookStep] ([Id])
    END
         PRINT 'OK: [RunbookStepResource_RunbookStep_FK] CONSTRAINT CREATED'

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='RunbookStepResource_Contact_FK')
    BEGIN
           ALTER TABLE [Runbook].[RunbookStepResource]
        ADD CONSTRAINT [RunbookStepResource_Contact_FK]
           FOREIGN KEY ([ContactId])
            REFERENCES [People].[Contact] ([Id])
    END
         PRINT 'OK: [RunbookStepResource_Contact_FK] CONSTRAINT CREATED'

GO

-- -----------------------------------------------------------------------------------------------------------------------
-- [Runbook].[RunbookStepTeam]
-- -----------------------------------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='RunbookStepTeam_Team_FK')
    BEGIN
           ALTER TABLE [Runbook].[RunbookStepTeam]
        ADD CONSTRAINT [RunbookStepTeam_Team_FK]
           FOREIGN KEY ([TeamId])
            REFERENCES [People].[Team] ([Id])
    END
         PRINT 'OK: [RunbookStepTeam_Team_FK] CONSTRAINT CREATED'

GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='RunbookStepTeam_RunbookStep_FK')
    BEGIN
           ALTER TABLE [Runbook].[RunbookStepTeam]
        ADD CONSTRAINT [RunbookStepTeam_RunbookStep_FK]
           FOREIGN KEY ([RunbookStepId])
            REFERENCES [Runbook].[RunbookStep] ([Id])
    END
         PRINT 'OK: [RunbookStepTeam_RunbookStep_FK] CONSTRAINT CREATED'

GO

