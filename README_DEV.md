## Scaffold
```bash
dotnet ef dbcontext scaffold "Name=ConnectionStrings:SqlConnection" Microsoft.EntityFrameworkCore.SqlServer -o Models -c LoanDbContext --context-dir Data --force --no-onconfiguring

```
