# KitchenControlSystem
# 🍳 Cocina Manager

Sistema web integral para la gestión de cocinas institucionales, desarrollado con **ASP.NET Core 8**, arquitectura en capas y Razor Pages.

---

## 📋 Índice

- [Descripción](#descripción)
- [Tecnologías](#tecnologías)
- [Arquitectura](#arquitectura)
- [Módulos](#módulos)
- [Requisitos](#requisitos)
- [Instalación](#instalación)
- [Configuración](#configuración)
- [Migraciones](#migraciones)
- [Usuarios por defecto](#usuarios-por-defecto)
- [Estructura del proyecto](#estructura-del-proyecto)

---

## 📖 Descripción

Cocina Manager es una aplicación web para la gestión integral de cocinas, que permite administrar personal, turnos, stock de víveres, mantenimiento de herramientas, recetas, menús semanales, incidentes, ausencias y comunicación interna entre usuarios.

---

## 🛠️ Tecnologías

| Tecnología | Versión | Uso |
|---|---|---|
| ASP.NET Core | 8.0 | Framework principal |
| Entity Framework Core | 8.0 | ORM y migraciones |
| SQL Server | 2019+ | Base de datos |
| Razor Pages | - | Interfaz web |
| Bootstrap | 5.3 | Estilos y componentes UI |
| FullCalendar | 6.1 | Calendario de turnos |
| ClosedXML | - | Exportación a Excel |
| Bootstrap Icons | 1.11 | Iconografía |

---

## 🏗️ Arquitectura

El proyecto sigue una arquitectura en capas limpia:

```
CocinaManager/
├── CocinaManager.API           # Capa de presentación (Controllers, Razor Pages, Middleware)
├── CocinaManager.Application   # Lógica de negocio (Services, Interfaces, DTOs)
├── CocinaManager.Domain        # Entidades y Enums del dominio
└── CocinaManager.Infrastructure # Acceso a datos (DbContext, Repositories, Migrations)
```

**Flujo de dependencias:**
```
API → Application → Domain
Infrastructure → Application → Domain
```

---

## 📦 Módulos

### 👥 Personal y Turnos
- Alta, baja y modificación de empleados
- Estados: Activo, Enfermo, Licencia, Ausente
- Asignación de turnos (Mañana, Tarde, Noche)
- Historial de turnos por empleado

### 📅 Calendario
- Vista mensual de turnos con FullCalendar
- Asignación de turnos haciendo click en un día
- Código de colores por tipo de turno

### 📦 Depósito y Stock
- Registro de productos con unidad de medida
- Entradas y salidas de stock
- Alertas automáticas de stock bajo
- Historial de movimientos

### 🔧 Mantenimiento
- Registro de herramientas
- Estados: Operativa, En Reparación, Fuera de Servicio
- Órdenes de mantenimiento
- Cambio automático de estado al crear/resolver órdenes

### 🍽️ Recetas y Menús
- Gestión de recetas con ingredientes y porciones
- Planificación de menú semanal (Desayuno, Almuerzo, Merienda, Cena)
- Vista en tabla semanal

### ⚠️ Incidentes
- Registro de incidentes (Accidente, Robo, Daño, Otro)
- Estados: Abierto / Resuelto
- Cualquier usuario puede reportar incidentes

### 🗓️ Ausencias y Licencias
- Registro de ausencias con fecha inicio y fin
- Tipos: Enfermedad, Licencia, Ausencia Injustificada, Vacaciones
- Cálculo automático de días
- Solo Admin puede registrar ausencias

### ✉️ Buzón de Mensajes
- Mensajería interna entre usuarios
- Bandeja de entrada y enviados
- Marcado automático de leídos / no leídos
- Respuesta a mensajes
- Contador de no leídos en el sidebar

### 📊 Dashboard
- Estadísticas en tiempo real
- Turnos del día
- Órdenes de mantenimiento pendientes
- Personal fuera de servicio
- Accesos rápidos a cada módulo

### 📤 Exportación a Excel
- Reporte de Personal
- Reporte de Turnos
- Reporte de Productos y Stock
- Reporte de Movimientos de Stock

---

## ✅ Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- [SQL Server 2019+](https://www.microsoft.com/sql-server) o SQL Server Express
- [Visual Studio 2022](https://visualstudio.microsoft.com/) o VS Code
- [dotnet-ef tools](https://docs.microsoft.com/ef/core/cli/dotnet)

---

## 🚀 Instalación

### 1. Clonar el repositorio

```bash
git clone https://github.com/tu-usuario/CocinaManager.git
cd CocinaManager
```

### 2. Instalar herramientas de EF Core

```bash
dotnet tool install --global dotnet-ef
```

### 3. Restaurar paquetes

```bash
dotnet restore
```

---

## ⚙️ Configuración

Editá el archivo `CocinaManager.API/appsettings.json` y configurá la cadena de conexión:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=CocinaManagerDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore.Database.Command": "Warning",
      "CocinaManager": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

> Para SQL Server con usuario y contraseña:
> ```
> Server=localhost;Database=CocinaManagerDB;User Id=sa;Password=tuPassword;TrustServerCertificate=True;
> ```

---

## 🗄️ Migraciones

Desde la raíz de la solución, ejecutá:

```bash
# Aplicar todas las migraciones
dotnet ef database update --project CocinaManager.Infrastructure --startup-project CocinaManager.API
```

> La base de datos se crea automáticamente si no existe. Al iniciar la aplicación por primera vez, se ejecuta un **seeder** que crea los usuarios por defecto.

---

## 👤 Usuarios por defecto

| Usuario | Contraseña | Rol | Permisos |
|---|---|---|---|
| `admin` | `admin123` | Admin | Acceso total |
| `operario` | `operario123` | Operario | Solo lectura, sin eliminar |

> ⚠️ Se recomienda cambiar las contraseñas por defecto antes de usar en producción.

---

## 📁 Estructura del proyecto

```
CocinaManager/
│
├── CocinaManager.API/
│   ├── Controllers/          # API Controllers (Reportes, Calendario)
│   ├── Middleware/           # ErrorHandlingMiddleware
│   ├── Pages/                # Razor Pages
│   │   ├── Shared/           # _Layout.cshtml
│   │   ├── Personal/
│   │   ├── Deposito/
│   │   ├── Mantenimiento/
│   │   ├── Calendario/
│   │   ├── Recetas/
│   │   ├── Incidentes/
│   │   ├── Ausencias/
│   │   ├── Buzon/
│   │   ├── Usuarios/
│   │   └── Perfil/
│   └── Program.cs
│
├── CocinaManager.Application/
│   ├── DTOs/                 # Data Transfer Objects
│   ├── Interfaces/           # Contratos de servicios y repositorios
│   └── Services/             # Lógica de negocio
│
├── CocinaManager.Domain/
│   ├── Entities/             # Entidades del dominio
│   └── Enums/                # Enumeraciones
│
└── CocinaManager.Infrastructure/
    ├── Data/                 # DbContext, DbSeeder, DesignTimeFactory
    ├── Migrations/           # Migraciones de EF Core
    └── Repositories/         # Implementaciones de repositorios
```

---

## 🔒 Seguridad

- Autenticación por **Cookie Authentication**
- Contraseñas hasheadas con **SHA-256**
- Autorización por roles en páginas y handlers
- Middleware global de manejo de errores
- Validaciones en DTOs con DataAnnotations

---

## 📝 Licencia

Este proyecto fue desarrollado con fines educativos y de gestión interna.
