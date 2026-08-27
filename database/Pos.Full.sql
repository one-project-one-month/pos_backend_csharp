/*
    POS database - complete fresh installation

    Run this file with SQLCMD mode from the repository root:

        sqlcmd -S . -E -b -i .\database\Pos.Full.sql

    The base script creates the Pos database, all existing business objects and
    seed data. The migration then adds the .NET 10 authentication and sale-draft
    objects. SQLCMD exits immediately if either script fails.

    For an existing Pos database, run only:

        sqlcmd -S . -E -b -d Pos -i .\database\migrations\20260827_add_auth_and_sale_drafts.sql
*/

:On Error exit

:r .\scripts-2.sql

USE [Pos]
GO

:r .\database\migrations\20260827_add_auth_and_sale_drafts.sql
