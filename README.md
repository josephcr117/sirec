
# 🏥 SIREC - Sistema de Registro de Citas Médicas

SIREC es una aplicación web completa que permite a pacientes, médicos y administradores gestionar citas médicas. El sistema está construido con **Angular** para el frontend y **ASP.NET Core + Entity Framework + SQL Server** en el backend.

---

## 🚀 Características

### Pacientes
- Registro e inicio de sesión
- Solicitud de citas médicas
- Visualización de historial de citas

### Médicos
- Acceso a sus citas asignadas
- Cambiar estado de la cita (Pendiente, Realizada, Cancelada)

### Administradores
- Gestión de usuarios (CRUD)
- Asignación de médicos a consultorios
- Dashboard administrativo

---

## 🛠️ Tecnologías

- **Frontend**: Angular 17+, Bootstrap 5, JWT
- **Backend**: ASP.NET Core 6+, Entity Framework Core, SQL Server
- **Base de datos**: SQL Server
- **Autenticación**: JWT

---

## 📁 Estructura del Proyecto

```
SIREC/
├── sirec-frontend/      → Proyecto Angular
└── sirec-backend/       → Proyecto ASP.NET Core Web API
```
---

## ⚙️ Configuración

### Requisitos

- Node.js 18+
- .NET 6 SDK
- SQL Server
- Angular CLI (`npm install -g @angular/cli`)

### Instrucciones

1. Clona el repositorio:
   git clone https://github.com/<TU_USUARIO>/sirec.git

2. Configura el backend:
   - Crea la base de datos en SQL Server.
   - Ajusta `appsettings.json` con tu cadena de conexión.
   - Aplica migraciones:
```
     dotnet ef database update
```
   - Ejecuta el backend:
   ```
       dotnet run --project sirec-backend
```
3. Ejecuta el frontend:
```
     cd sirec-frontend
     npm install
     ng serve
```
---

## 🧪 Créditos

Desarrollado por: Brian Morales, Maria Arrieta, Steven Fonseca
Proyecto para: Programación 6 - UMCA
Año: 2025

---
