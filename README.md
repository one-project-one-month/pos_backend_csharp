
Staff Token
```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjAiLCJTdGFmZk5hbWUiOiJTdSBTdSIsIlN0YWZmQ29kZSI6IlUwMDAwMSIsIlRva2VuRXhwaXJlZCI6IjIwMjQtMDQtMjJUMTY6MzY6NDMuNjE1MTc1NFoiLCJuYmYiOjE3MTM4MDI5MDMsImV4cCI6MTcxMzgwMzgwMywiaWF0IjoxNzEzODAyOTAzfQ.IA6JMyYx1yaM2K9ch38sS1Fr2eukLKjOOhh-u5oPTI4
```
EF Core Database First အနေနဲ့
အသုံးပြုချင်ပါက အောက်က Command များကို အသုံးပြုနိုင်ပါတယ်
```
Scaffold-DbContext "Server=.;Database=Pos;Integrated Security=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context AppDbContext -f
Scaffold-DbContext "Server=.;Database=Pos;User ID=sa;Password=sasa@123;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context AppDbContext

dotnet tool install --global dotnet-ef 7.0.17
dotnet ef dbcontext scaffold "Server=.;Database=Pos;User ID=sa;Password=sasa@123;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models -c AppDbContext

```
Database အတွက် လိုအပ်တဲ့ Table များကို ဒီ [Link](https://github.com/sannlynnhtun-coding/pos_backend_csharp/blob/main/pos_db_script.sql) ကနေ ကြည့်နိုင်ပါတယ်

Postman Collection ကို ဒီ [Link](https://github.com/sannlynnhtun-coding/pos_backend_csharp/blob/main/POS.postman_collection.json) ကနေ ကြည့်နိုင်ပါတယ်

[How to turn IDENTITY_INSERT on and off using SQL Server?](https://stackoverflow.com/questions/7063501/how-to-turn-identity-insert-on-and-off-using-sql-server-2008)
```
SET IDENTITY_INSERT Tbl_ProductCategory ON

INSERT INTO [dbo].[Tbl_ProductCategory]
           ([ProductCategoryId]
		   ,[ProductCategoryCode]
           ,[ProductCategoryName])
     VALUES
           (1
		   ,'C001'
           ,'Foods')

SET IDENTITY_INSERT Tbl_ProductCategory OFF
```
