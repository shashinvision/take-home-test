## Scaffold

```bash
dotnet ef dbcontext scaffold "Name=ConnectionStrings:SqlConnection" Microsoft.EntityFrameworkCore.SqlServer -o Models -c FundoDbContext --context-dir Data --force --no-onconfiguring
```

```bash
dotnet ef dbcontext scaffold "Name=ConnectionStrings:SqlConnection" Microsoft.EntityFrameworkCore.SqlServer -o Models -c FundoDbContext --context-dir Data --force --no-onconfiguring --table USER
```
```bash
dotnet ef dbcontext scaffold "Name=ConnectionStrings:SqlConnection" Microsoft.EntityFrameworkCore.SqlServer -o Models -c FundoDbContext --context-dir Data --force --no-onconfiguring --table APPLICANT
```
```bash
dotnet ef dbcontext scaffold "Name=ConnectionStrings:SqlConnection" Microsoft.EntityFrameworkCore.SqlServer -o Models -c FundoDbContext --context-dir Data --force --no-onconfiguring --table LOAN
```
```bash
dotnet ef dbcontext scaffold "Name=ConnectionStrings:SqlConnection" Microsoft.EntityFrameworkCore.SqlServer -o Models -c FundoDbContext --context-dir Data --force --no-onconfiguring --table PAYMENT
```
