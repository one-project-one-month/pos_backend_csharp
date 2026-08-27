SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF OBJECT_ID(N'dbo.Tbl_RefreshToken', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Tbl_RefreshToken
    (
        RefreshTokenId BIGINT IDENTITY(1, 1) NOT NULL
            CONSTRAINT PK_RefreshToken PRIMARY KEY,
        StaffId INT NOT NULL,
        FamilyId UNIQUEIDENTIFIER NOT NULL,
        TokenHash BINARY(32) NOT NULL,
        CreatedAtUtc DATETIME2(7) NOT NULL,
        ExpiresAtUtc DATETIME2(7) NOT NULL,
        UsedAtUtc DATETIME2(7) NULL,
        RevokedAtUtc DATETIME2(7) NULL,
        ReplacedByTokenHash BINARY(32) NULL,
        RowVersion ROWVERSION NOT NULL,
        CONSTRAINT FK_RefreshToken_Staff FOREIGN KEY (StaffId)
            REFERENCES dbo.Tbl_Staff (StaffId)
    );
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_RefreshToken_TokenHash' AND object_id = OBJECT_ID(N'dbo.Tbl_RefreshToken'))
    CREATE UNIQUE INDEX UX_RefreshToken_TokenHash ON dbo.Tbl_RefreshToken (TokenHash);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_RefreshToken_StaffId_FamilyId' AND object_id = OBJECT_ID(N'dbo.Tbl_RefreshToken'))
    CREATE INDEX IX_RefreshToken_StaffId_FamilyId ON dbo.Tbl_RefreshToken (StaffId, FamilyId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_RefreshToken_ExpiresAtUtc' AND object_id = OBJECT_ID(N'dbo.Tbl_RefreshToken'))
    CREATE INDEX IX_RefreshToken_ExpiresAtUtc ON dbo.Tbl_RefreshToken (ExpiresAtUtc);

IF OBJECT_ID(N'dbo.Tbl_SaleDraft', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Tbl_SaleDraft
    (
        SaleDraftId INT IDENTITY(1, 1) NOT NULL
            CONSTRAINT PK_SaleDraft PRIMARY KEY,
        StaffId INT NOT NULL,
        DraftName NVARCHAR(100) NULL,
        CreatedAtUtc DATETIME2(7) NOT NULL,
        UpdatedAtUtc DATETIME2(7) NOT NULL,
        RowVersion ROWVERSION NOT NULL,
        CONSTRAINT FK_SaleDraft_Staff FOREIGN KEY (StaffId)
            REFERENCES dbo.Tbl_Staff (StaffId)
    );
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_SaleDraft_StaffId_UpdatedAtUtc' AND object_id = OBJECT_ID(N'dbo.Tbl_SaleDraft'))
    CREATE INDEX IX_SaleDraft_StaffId_UpdatedAtUtc ON dbo.Tbl_SaleDraft (StaffId, UpdatedAtUtc DESC);

-- ProductCode is the stable product identifier used by sale drafts. The legacy
-- schema only keys Tbl_Product by ProductId, so make ProductCode a candidate key
-- before referencing it from Tbl_SaleDraftDetail.
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_Product_ProductCode' AND object_id = OBJECT_ID(N'dbo.Tbl_Product'))
    CREATE UNIQUE INDEX UX_Product_ProductCode ON dbo.Tbl_Product (ProductCode);

IF OBJECT_ID(N'dbo.Tbl_SaleDraftDetail', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Tbl_SaleDraftDetail
    (
        SaleDraftDetailId INT IDENTITY(1, 1) NOT NULL
            CONSTRAINT PK_SaleDraftDetail PRIMARY KEY,
        SaleDraftId INT NOT NULL,
        ProductCode NVARCHAR(50) NOT NULL,
        Quantity INT NOT NULL,
        UnitPrice DECIMAL(18, 2) NOT NULL,
        RowVersion ROWVERSION NOT NULL,
        CONSTRAINT CK_SaleDraftDetail_Quantity CHECK (Quantity > 0),
        CONSTRAINT CK_SaleDraftDetail_UnitPrice CHECK (UnitPrice >= 0),
        CONSTRAINT FK_SaleDraftDetail_Draft FOREIGN KEY (SaleDraftId)
            REFERENCES dbo.Tbl_SaleDraft (SaleDraftId) ON DELETE CASCADE,
        CONSTRAINT FK_SaleDraftDetail_Product FOREIGN KEY (ProductCode)
            REFERENCES dbo.Tbl_Product (ProductCode)
    );
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_SaleDraftDetail_Draft_Product' AND object_id = OBJECT_ID(N'dbo.Tbl_SaleDraftDetail'))
    CREATE UNIQUE INDEX UX_SaleDraftDetail_Draft_Product ON dbo.Tbl_SaleDraftDetail (SaleDraftId, ProductCode);

COMMIT TRANSACTION;
