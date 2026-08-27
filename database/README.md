# POS database scripts

## Fresh database

Open PowerShell in the repository root and run:

```powershell
sqlcmd -S . -E -b -i .\database\Pos.Full.sql
```

`Pos.Full.sql` is the complete SQLCMD entry script. It creates the existing POS
schema and seed data from `scripts-2.sql`, then installs the .NET 10 refresh-token
and multi-sale-draft schema.

The historical base dump contains SQL Server 2022 default-instance MDF/LDF paths.
If the SQL Server instance stores database files elsewhere, update the two
`FILENAME` values near the beginning of `scripts-2.sql` before running a fresh
installation.

## Existing database

Do not run the full installer against an existing `Pos` database. Apply the
idempotent migration only:

```powershell
sqlcmd -S . -E -b -d Pos -i .\database\migrations\20260827_add_auth_and_sale_drafts.sql
```

Replace `.` with the SQL Server instance name. Use `-U <user> -P <password>` in
place of `-E` only when SQL authentication is required.
