# Load environment variables from .env file
Get-Content .env | ForEach-Object {
    $name, $value = $_ -split '=', 2
    Set-Item -Path "env:$name" -Value $value
}

# Retrieve the connection string
$CONN_STR = $env:CONN_STR

# Run the EF Core scaffold command
dotnet ef dbcontext scaffold $CONN_STR Npgsql.EntityFrameworkCore.PostgreSQL `
    --output-dir ../Core.Domain/Entities `
    --context-dir . `
    --context MyDbContext `
    --no-onconfiguring `
    --namespace Core.Domain.Entities `
    --context-namespace Infrastructure.Postgres.Scaffolding `
    --schema surveillance `
    --force
