```
Staff Token
----------
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjAiLCJTdGFmZk5hbWUiOiJTdSBTdSIsIlN0YWZmQ29kZSI6IlUwMDAwMSIsIlRva2VuRXhwaXJlZCI6IjIwMjQtMDQtMjJUMTY6MzY6NDMuNjE1MTc1NFoiLCJuYmYiOjE3MTM4MDI5MDMsImV4cCI6MTcxMzgwMzgwMywiaWF0IjoxNzEzODAyOTAzfQ.IA6JMyYx1yaM2K9ch38sS1Fr2eukLKjOOhh-u5oPTI4
```
```
Scaffold-DbContext "Server=.;Database=Pos;Integrated Security=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context AppDbContext -f
Scaffold-DbContext "Server=.;Database=Pos;User ID=sa;Password=sasa@123;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context AppDbContext

dotnet tool install --global dotnet-ef 7.0.17
dotnet ef dbcontext scaffold "Server=.;Database=Pos;User ID=sa;Password=sasa@123;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models -c AppDbContext

```

```
USE [Pos]
GO
/****** Object:  Table [dbo].[Tbl_Customer]    Script Date: 4/30/2024 1:05:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Customer](
	[CustomerId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerCode] [nvarchar](50) NOT NULL,
	[CustomerName] [nvarchar](50) NOT NULL,
	[MobileNo] [nvarchar](50) NOT NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[Gender] [nvarchar](50) NOT NULL,
	[StateCode] [nvarchar](50) NOT NULL,
	[TownshipCode] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Tbl_Customer] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Product]    Script Date: 4/30/2024 1:05:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Product](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[ProductCode] [nvarchar](50) NOT NULL,
	[ProductCategoryCode] [nvarchar](50) NOT NULL,
	[ProductName] [nvarchar](50) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_ProductCategory]    Script Date: 4/30/2024 1:05:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_ProductCategory](
	[ProductCategoryId] [int] IDENTITY(1,1) NOT NULL,
	[ProductCategoryCode] [varchar](50) NOT NULL,
	[ProductCategoryName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ProductCategory] PRIMARY KEY CLUSTERED 
(
	[ProductCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_SaleInvoice]    Script Date: 4/30/2024 1:05:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_SaleInvoice](
	[SaleInvoiceId] [int] IDENTITY(1,1) NOT NULL,
	[SaleInvoiceDateTime] [datetime] NOT NULL,
	[VoucherNo] [nvarchar](20) NOT NULL,
	[TotalAmount] [decimal](18, 2) NOT NULL,
	[Discount] [decimal](18, 2) NOT NULL,
	[StaffCode] [nvarchar](50) NOT NULL,
	[Tax] [decimal](18, 2) NOT NULL,
	[PaymentType] [nvarchar](10) NULL,
	[CustomerAccountNo] [nvarchar](20) NULL,
	[PaymentAmount] [decimal](18, 2) NULL,
	[ReceiveAmount] [decimal](18, 2) NULL,
	[Change] [decimal](18, 2) NULL,
	[CustomerCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_Tbl_SaleInvoice] PRIMARY KEY CLUSTERED 
(
	[SaleInvoiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_SaleInvoiceDetail]    Script Date: 4/30/2024 1:05:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_SaleInvoiceDetail](
	[SaleInvoiceDetailId] [int] IDENTITY(1,1) NOT NULL,
	[VoucherNo] [nvarchar](20) NOT NULL,
	[ProductCode] [nvarchar](50) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Tbl_SaleInvoiceDetail] PRIMARY KEY CLUSTERED 
(
	[SaleInvoiceDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Shop]    Script Date: 4/30/2024 1:05:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Shop](
	[ShopId] [int] IDENTITY(1,1) NOT NULL,
	[ShopCode] [varchar](50) NOT NULL,
	[ShopName] [varchar](50) NOT NULL,
	[MobileNo] [varchar](50) NOT NULL,
	[Address] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Shop] PRIMARY KEY CLUSTERED 
(
	[ShopId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tbl_Staff]    Script Date: 4/30/2024 1:05:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Staff](
	[StaffId] [int] IDENTITY(1,1) NOT NULL,
	[StaffCode] [varchar](50) NOT NULL,
	[StaffName] [varchar](50) NOT NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[MobileNo] [varchar](50) NOT NULL,
	[Address] [varchar](50) NOT NULL,
	[Gender] [varchar](50) NOT NULL,
	[Position] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Staff] PRIMARY KEY CLUSTERED 
(
	[StaffId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

```