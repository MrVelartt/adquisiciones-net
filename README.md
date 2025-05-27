# AdquisicionesAPI

API RESTful desarrollada con .NET 8 para gestionar adquisiciones, con control de versiones e historial de cambios.

## 🚀 Tecnologías utilizadas

- ASP.NET Core 8
- Entity Framework Core
- AutoMapper
- SQL Server
- Reversion-style audit (próximamente)

## 📂 Estructura del proyecto

- `Controllers/` – Controladores para manejar las peticiones HTTP.
- `Models/` – Modelos de datos.
- `DTOs/` – Objetos de transferencia de datos.
- `Services/` – Lógica de negocio.
- `Data/` – Configuración de la base de datos.
- `MappingProfiles/` – Configuración de AutoMapper.
- `Migrations/` – Migraciones de EF Core.

## ⚙️ Configuración

1. Clona el repositorio:
   ```bash
   git clone https://github.com/MrVelartt/adquisiciones-net.git
   cd adquisiciones-net

2. Restaura los paquetes y corre las migraciones:


   dotnet restore
   dotnet ef database update

3. Corre el servidor 

   dotnet run
