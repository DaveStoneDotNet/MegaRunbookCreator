-- -----------------------------------------------------------------------------------------------------------------------
-- CONSTRAINTS
-- -----------------------------------------------------------------------------------------------------------------------

-- -----------------------------------------------------------------------------------------------------------------------
-- [Links].[ApplicationLink]
-- -----------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='ApplicationLink_ApplicationGroup_FK')
    BEGIN
           ALTER TABLE [Links].[ApplicationLink]
       DROP CONSTRAINT [ApplicationLink_ApplicationGroup_FK]
                 PRINT 'OK: [ApplicationLink_ApplicationGroup_FK] CONSTRAINT DROPPED'
    END

GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='ApplicationLink_ApplicationType_FK')
    BEGIN
           ALTER TABLE [Links].[ApplicationLink]
       DROP CONSTRAINT [ApplicationLink_ApplicationType_FK]
                 PRINT 'OK: [ApplicationLink_ApplicationType_FK] CONSTRAINT DROPPED'
    END

GO

-- -----------------------------------------------------------------------------------------------------------------------
-- [Links].[EnvironmentLink]
-- -----------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='EnvironmentLink_ServiceLink_FK')
    BEGIN
           ALTER TABLE [Links].[EnvironmentLink]
       DROP CONSTRAINT [EnvironmentLink_ServiceLink_FK]
                 PRINT 'OK: [EnvironmentLink_ServiceLink_FK] CONSTRAINT DROPPED'
    END

GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='EnvironmentLink_Server_FK')
    BEGIN
           ALTER TABLE [Links].[EnvironmentLink]
       DROP CONSTRAINT [EnvironmentLink_Server_FK]
                 PRINT 'OK: [EnvironmentLink_Server_FK] CONSTRAINT DROPPED'
    END

GO

-- -----------------------------------------------------------------------------------------------------------------------
-- [Links].[Server]
-- -----------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='Server_Environment_FK')
    BEGIN
           ALTER TABLE [Links].[Server]
       DROP CONSTRAINT [Server_Environment_FK]
                 PRINT 'OK: [Server_Environment_FK] CONSTRAINT DROPPED'
    END

GO

-- -----------------------------------------------------------------------------------------------------------------------
-- [Links].[ServiceLink]
-- -----------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='ServiceLink_ApplicationLink_FK')
    BEGIN
           ALTER TABLE [Links].[ServiceLink]
       DROP CONSTRAINT [ServiceLink_ApplicationLink_FK]
                 PRINT 'OK: [ServiceLink_ApplicationLink_FK] CONSTRAINT DROPPED'
    END

GO

-- -----------------------------------------------------------------------------------------------------------------------
-- [People].[TeamMember]
-- -----------------------------------------------------------------------------------------------------------------------

-- NONE

-- -----------------------------------------------------------------------------------------------------------------------
-- [Runbook].[Rfc]
-- -----------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='Rfc_Contact_FK')
    BEGIN
           ALTER TABLE [Runbook].[Rfc]
       DROP CONSTRAINT [Rfc_Contact_FK]
                 PRINT 'OK: [Rfc_Contact_FK] CONSTRAINT DROPPED'
    END

GO

-- -----------------------------------------------------------------------------------------------------------------------
-- [Runbook].[RunbookStep]
-- -----------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='RunbookStep_Rfc_FK')
    BEGIN
           ALTER TABLE [Runbook].[RunbookStep]
       DROP CONSTRAINT [RunbookStep_Rfc_FK]
                 PRINT 'OK: [RunbookStep_Rfc_FK] CONSTRAINT DROPPED'
    END

GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='RunbookStep_RunbookStepStatus_FK')
    BEGIN
           ALTER TABLE [Runbook].[RunbookStep]
       DROP CONSTRAINT [RunbookStep_RunbookStepStatus_FK]
                 PRINT 'OK: [RunbookStep_RunbookStepStatus_FK] CONSTRAINT DROPPED'
    END

GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='RunbookStep_RunbookStepType_FK')
    BEGIN
           ALTER TABLE [Runbook].[RunbookStep]
       DROP CONSTRAINT [RunbookStep_RunbookStepType_FK]
                 PRINT 'OK: [RunbookStep_RunbookStepType_FK] CONSTRAINT DROPPED'
    END

GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='RunbookStep_RunbookTemplate_FK')
    BEGIN
           ALTER TABLE [Runbook].[RunbookStep]
       DROP CONSTRAINT [RunbookStep_RunbookTemplate_FK]
                 PRINT 'OK: [RunbookStep_RunbookTemplate_FK] CONSTRAINT DROPPED'
    END

GO

-- -----------------------------------------------------------------------------------------------------------------------
-- [Runbook].[RunbookStepDeveloper]
-- -----------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='RunbookStepDeveloper_Contact_FK')
    BEGIN
           ALTER TABLE [Runbook].[RunbookStepDeveloper]
       DROP CONSTRAINT [RunbookStepDeveloper_Contact_FK]
                 PRINT 'OK: [RunbookStepDeveloper_Contact_FK] CONSTRAINT DROPPED'
    END

GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='RunbookStepDeveloper_RunbookStep_FK')
    BEGIN
           ALTER TABLE [Runbook].[RunbookStepDeveloper]
       DROP CONSTRAINT [RunbookStepDeveloper_RunbookStep_FK]
                 PRINT 'OK: [RunbookStepDeveloper_RunbookStep_FK] CONSTRAINT DROPPED'
    END

GO

-- -----------------------------------------------------------------------------------------------------------------------
-- [Runbook].[RunbookStepPbi]
-- -----------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='RunbookStepPbi_RunbookStep_FK')
    BEGIN
           ALTER TABLE [Runbook].[RunbookStepPbi]
       DROP CONSTRAINT [RunbookStepPbi_RunbookStep_FK]
                 PRINT 'OK: [RunbookStepPbi_RunbookStep_FK] CONSTRAINT DROPPED'
    END

GO

-- -----------------------------------------------------------------------------------------------------------------------
-- [Runbook].[RunbookStepResource]
-- -----------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='RunbookStepResource_RunbookStep_FK')
    BEGIN
           ALTER TABLE [Runbook].[RunbookStepResource]
       DROP CONSTRAINT [RunbookStepResource_RunbookStep_FK]
                 PRINT 'OK: [RunbookStepResource_RunbookStep_FK] CONSTRAINT DROPPED'
    END

GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='RunbookStepResource_Contact_FK')
    BEGIN
           ALTER TABLE [Runbook].[RunbookStepResource]
       DROP CONSTRAINT [RunbookStepResource_Contact_FK]
                 PRINT 'OK: [RunbookStepResource_Contact_FK] CONSTRAINT DROPPED'
    END

GO

-- -----------------------------------------------------------------------------------------------------------------------
-- [Runbook].[RunbookStepTeam]
-- -----------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='RunbookStepTeam_Team_FK')
    BEGIN
           ALTER TABLE [Runbook].[RunbookStepTeam]
       DROP CONSTRAINT [RunbookStepTeam_Team_FK]
                 PRINT 'OK: [RunbookStepTeam_Team_FK] CONSTRAINT DROPPED'
    END

GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME ='RunbookStepTeam_RunbookStep_FK')
    BEGIN
           ALTER TABLE [Runbook].[RunbookStepTeam]
       DROP CONSTRAINT [RunbookStepTeam_RunbookStep_FK]
                 PRINT 'OK: [RunbookStepTeam_RunbookStep_FK] CONSTRAINT DROPPED'
    END

GO

-- -----------------------------------------------------------------------------------------------------------------------
-- TABLES
-- -----------------------------------------------------------------------------------------------------------------------

-- -----------------------------------------------------------------------------------------------------------------------
-- [Links]
-- -----------------------------------------------------------------------------------------------------------------------

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Links' AND TABLE_NAME = 'ApplicationGroup'))
    BEGIN
        DROP TABLE [Links].[ApplicationGroup]
             PRINT 'OK: [Links].[ApplicationGroup] TABLE DROPPED'
    END

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Links' AND TABLE_NAME = 'ApplicationLink'))
    BEGIN
        DROP TABLE [Links].[ApplicationLink]
             PRINT 'OK: [Links].[ApplicationLink] TABLE DROPPED'
    END

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Links' AND TABLE_NAME = 'Environment'))
    BEGIN
        DROP TABLE [Links].[Environment]
             PRINT 'OK: [Links].[Environment] TABLE DROPPED'
    END

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Links' AND TABLE_NAME = 'EnvironmentLink'))
    BEGIN
        DROP TABLE [Links].[EnvironmentLink]
             PRINT 'OK: [Links].[EnvironmentLink] TABLE DROPPED'
    END

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Links' AND TABLE_NAME = 'Server'))
    BEGIN
        DROP TABLE [Links].[Server]
             PRINT 'OK: [Links].[Server] TABLE DROPPED'
    END

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Links' AND TABLE_NAME = 'ServiceLink'))
    BEGIN
        DROP TABLE [Links].[ServiceLink]
             PRINT 'OK: [Links].[ServiceLink] TABLE DROPPED'
    END

-- -----------------------------------------------------------------------------------------------------------------------
-- [Lookup]
-- -----------------------------------------------------------------------------------------------------------------------

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Lookup' AND  TABLE_NAME = 'ApplicationType'))
    BEGIN
        DROP TABLE [Lookup].[ApplicationType]
             PRINT 'OK: [Lookup].[ApplicationType] TABLE DROPPED'
    END

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Lookup' AND TABLE_NAME = 'RunbookStepStatus'))
    BEGIN
        DROP TABLE [Lookup].[RunbookStepStatus]
             PRINT 'OK: [Lookup].[RunbookStepStatus] TABLE DROPPED'
    END

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Lookup' AND TABLE_NAME = 'RunbookStepType'))
    BEGIN
        DROP TABLE [Lookup].[RunbookStepType]
             PRINT 'OK: [Lookup].[RunbookStepType] TABLE DROPPED'
    END

-- -----------------------------------------------------------------------------------------------------------------------
-- [People]
-- -----------------------------------------------------------------------------------------------------------------------

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'People' AND TABLE_NAME = 'Contact'))
    BEGIN
        DROP TABLE [People].[Contact]
             PRINT 'OK: [People].[Contact] TABLE DROPPED'
    END

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'People' AND TABLE_NAME = 'Team'))
    BEGIN
        DROP TABLE [People].[Team]
             PRINT 'OK: [People].[Team] TABLE DROPPED'
    END

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'People' AND TABLE_NAME = 'TeamMember'))
    BEGIN
        DROP TABLE [People].[TeamMember]
             PRINT 'OK: [People].[TeamMember] TABLE DROPPED'
    END

-- -----------------------------------------------------------------------------------------------------------------------
-- [Runbook]
-- -----------------------------------------------------------------------------------------------------------------------

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Runbook' AND TABLE_NAME = 'Rfc'))
    BEGIN
        DROP TABLE [Runbook].[Rfc]
             PRINT 'OK: [Runbook].[Rfc] TABLE DROPPED'
    END

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Runbook' AND TABLE_NAME = 'RunbookStep'))
    BEGIN
        DROP TABLE [Runbook].[RunbookStep]
             PRINT 'OK: [Runbook].[RunbookStep] TABLE DROPPED'
    END

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Runbook' AND TABLE_NAME = 'RunbookStepDeveloper'))
    BEGIN
        DROP TABLE [Runbook].[RunbookStepDeveloper]
             PRINT 'OK: [Runbook].[RunbookStepDeveloper] TABLE DROPPED'
    END

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Runbook' AND TABLE_NAME = 'RunbookStepPbi'))
    BEGIN
        DROP TABLE [Runbook].[RunbookStepPbi]
             PRINT 'OK: [Runbook].[RunbookStepPbi] TABLE DROPPED'
    END

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Runbook' AND TABLE_NAME = 'RunbookStepResource'))
    BEGIN
        DROP TABLE [Runbook].[RunbookStepResource]
             PRINT 'OK: [Runbook].[RunbookStepResource] TABLE DROPPED'
    END

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Runbook' AND TABLE_NAME = 'RunbookStepTeam'))
    BEGIN
        DROP TABLE [Runbook].[RunbookStepTeam]
             PRINT 'OK: [Runbook].[RunbookStepTeam] TABLE DROPPED'
    END

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Runbook' AND TABLE_NAME = 'RunbookTemplate'))
    BEGIN
        DROP TABLE [Runbook].[RunbookTemplate]
             PRINT 'OK: [Runbook].[RunbookTemplate] TABLE DROPPED'
    END

-- -----------------------------------------------------------------------------------------------------------------------
-- SCHEMAS
-- -----------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = 'Links' ) 
    BEGIN
        DROP SCHEMA [Links]
              PRINT 'OK: [Links] SCHEMA DROPPED'
    END

IF EXISTS (SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = 'Lookup' ) 
    BEGIN
        DROP SCHEMA [Lookup]
              PRINT 'OK: [Lookup] SCHEMA DROPPED'
    END

IF EXISTS (SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = 'People' ) 
    BEGIN
        DROP SCHEMA [People]
              PRINT 'OK: [People] SCHEMA DROPPED'
    END

IF EXISTS (SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = 'Runbook' ) 
    BEGIN
        DROP SCHEMA [Runbook]
              PRINT 'OK: [Runbook] SCHEMA DROPPED'
    END

