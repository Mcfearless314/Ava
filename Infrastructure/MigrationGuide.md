Du skal være inde i din `Infrastructure` mappe for at kunne køre denne kommando
```bash
dotnet ef --startup-project ../Infrastructure/Infrastructure.Migrations migrations add [navn på migration]
```

Fjern seneste migration
```bash
dotnet ef --startup-project ../Infrastructure/Infrastructure.Migrations migrations remove
```
