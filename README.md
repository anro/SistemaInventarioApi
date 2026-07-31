# 📦 ApiInventario

API REST desarrollada con ASP.NET Core para la gestión de inventario, compras, ventas y control de stock.

## 🚀 Tecnologías utilizadas

- ASP.NET Core
- C#
- Entity Framework Core
- SQL Server
- JWT Authentication
- Swagger / OpenAPI
- Arquitectura por Servicios
- DTO (Data Transfer Objects)

## ✨ Funcionalidades

- Autenticación con JWT
- Gestión de usuarios
- Roles y permisos por módulos
- Gestión de productos
- Gestión de proveedores
- Registro de compras
- Registro de ventas
- Control automático de stock
- Reportes
- Documentación con Swagger

## 📁 Estructura del proyecto

```
ApiInventario
│
├── Controllers
├── Services
├── DTOs
├── Models
├── Data
├── Security
├── Middleware
├── Migrations
└── Program.cs
```

## ⚙️ Requisitos

- .NET 9 SDK (o la versión que utilices)
- SQL Server
- Visual Studio 2022

## ▶️ Cómo ejecutar el proyecto

1. Clonar el repositorio

```bash
git clone https://github.com/anro/SistemaInventarioApi.git
```

2. Abrir la solución en Visual Studio.

3. Configurar la cadena de conexión en `appsettings.json`.

4. Ejecutar la API.

5. Abrir Swagger:

```
https://localhost:xxxx/swagger
```

## 📌 Próximas mejoras

- Paginación
- Pruebas unitarias
- Logging con Serilog
- Frontend en React + TypeScript
- Docker
- CI/CD con GitHub Actions

## 👩‍💻 Autor

Ana Alonso

GitHub: https://github.com/anro
