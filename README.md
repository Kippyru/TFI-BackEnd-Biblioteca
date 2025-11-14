# ?? TFI-BackEnd-Biblioteca

**Sistema de Gestión de Biblioteca - Backend API**

## ??? Tecnologías

- **C# / .NET 9**
- **Entity Framework Core** - ORM para gestión de base de datos
- **ASP.NET Core Web API** - Framework para API REST
- **SQL Server** - Base de datos
- **Scalar** - Documentación interactiva de API

## ?? URLs del Proyecto

- **API Base**: `https://localhost:7063/api`
- **Documentación Scalar**: `https://localhost:7063/scalar/v1`

---

## ? Estado del Proyecto

### ?? Completado

- [x] Configuración inicial del entorno
- [x] Instalación de paquetes NuGet (EF Core, SQL Server, Tools)
- [x] Creación de la **base de datos** usando **Entity Framework**
- [x] Configuración del **DbContext** y las entidades (Libros, Socios, Préstamos)
- [x] Implementación de operaciones **CRUD** (Create, Read, Update, Delete)
- [x] Creación de controladores y endpoints para:
  - [x] **Libros** - CRUD completo + búsqueda + disponibilidad
  - [x] **Socios** - CRUD completo + búsqueda + activación/desactivación
  - [x] **Préstamos y devoluciones** - Crear, devolver, renovar, consultar activos/atrasados

### ?? Pendiente

- [ ] Conectar el backend con el **frontend**
- [ ] Agregar autenticación y roles (Admin / Usuario)
- [ ] Implementar validaciones avanzadas y manejo de errores
- [ ] Testing unitario e integración

---

## ??? Estructura del Proyecto

```
TFI-BackEnd-Biblioteca/
??? Controllers/
?   ??? LibrosController.cs       # API de gestión de libros
?   ??? SociosController.cs       # API de gestión de socios
?   ??? PrestamosController.cs    # API de gestión de préstamos
??? Data/
?   ??? BibliotecaContext.cs      # DbContext de Entity Framework
??? Models/
?   ??? Libro.cs             # Entidad Libro
?   ??? Socio.cs            # Entidad Socio
?   ??? Prestamo.cs           # Entidad Préstamo
??? Migrations/        # Migraciones de base de datos
??? appsettings.json  # Configuración y cadena de conexión
??? Program.cs   # Punto de entrada de la aplicación
??? API_ENDPOINTS.md        # Documentación detallada de endpoints
```

---

## ?? Instalación y Ejecución

### Requisitos previos
- **.NET 9 SDK**
- **SQL Server** o **SQL Server LocalDB**
- **Visual Studio 2022** o **VS Code**

### Pasos de instalación

1. **Clonar el repositorio**
   ```bash
   git clone https://github.com/Kippyru/TFI-BackEnd-Biblioteca.git
   cd TFI-BackEnd-Biblioteca
   ```

2. **Restaurar paquetes**
```bash
   dotnet restore
   ```

3. **Configurar la base de datos**
   
   Editar `appsettings.json` si necesitas cambiar la cadena de conexión:
   ```json
   {
     "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BibliotecaDB;Trusted_Connection=True;"
     }
   }
   ```

4. **Aplicar migraciones** (si es necesario)
   ```bash
   dotnet ef database update
   ```

   Las migraciones se aplican automáticamente al ejecutar la aplicación en modo desarrollo.

5. **Ejecutar la aplicación**
   ```bash
   dotnet run
   ```

6. **Acceder a la documentación**
   
   Abrir el navegador en: `https://localhost:7063/scalar/v1`

---

## ?? Endpoints Principales

### ?? Libros
- `GET /api/libros` - Listar todos
- `GET /api/libros/{id}` - Obtener por ID
- `GET /api/libros/disponibles` - Libros con stock
- `GET /api/libros/buscar?termino=` - Búsqueda
- `POST /api/libros` - Crear
- `PUT /api/libros/{id}` - Actualizar
- `DELETE /api/libros/{id}` - Eliminar

### ?? Socios
- `GET /api/socios` - Listar todos
- `GET /api/socios/{id}` - Obtener por ID
- `GET /api/socios/activos` - Socios activos
- `GET /api/socios/{id}/prestamos` - Préstamos del socio
- `POST /api/socios` - Crear
- `PUT /api/socios/{id}` - Actualizar
- `PUT /api/socios/{id}/desactivar` - Desactivar
- `DELETE /api/socios/{id}` - Eliminar

### ?? Préstamos
- `GET /api/prestamos` - Listar todos
- `GET /api/prestamos/activos` - Préstamos activos
- `GET /api/prestamos/atrasados` - Préstamos vencidos
- `POST /api/prestamos` - Crear préstamo
- `PUT /api/prestamos/{id}/devolver` - Devolver libro
- `PUT /api/prestamos/{id}/renovar` - Renovar préstamo
- `DELETE /api/prestamos/{id}` - Eliminar

?? **Documentación completa**: Ver [API_ENDPOINTS.md](API_ENDPOINTS.md)

---

## ?? Reglas de Negocio

### Libros
- ? Control automático de stock disponible
- ? No se puede eliminar si tiene préstamos activos

### Socios
- ? Email único por socio
- ? Sistema de activación/desactivación
- ? No se puede desactivar con préstamos activos
- ? Socios desactivados no pueden pedir préstamos

### Préstamos
- ? Duración predeterminada: 15 días
- ? Control de stock automático
- ? No se permiten nuevos préstamos si hay préstamos atrasados
- ? Socio debe estar activo

---

## ??? Modelo de Datos

### Libro
```csharp
- Id (int)
- Titulo (string, requerido)
- Autor (string, requerido)
- ISBN (string, opcional)
- Editorial (string, opcional)
- AñoPublicacion (int?, opcional)
- Genero (string, opcional)
- CantidadDisponible (int)
- CantidadTotal (int)
- FechaRegistro (DateTime)
```

### Socio
```csharp
- Id (int)
- Nombre (string, requerido)
- Apellido (string, requerido)
- Email (string, requerido, único)
- Telefono (string, opcional)
- Direccion (string, opcional)
- FechaRegistro (DateTime)
- Activo (bool)
```

### Prestamo
```csharp
- Id (int)
- LibroId (int, FK)
- SocioId (int, FK)
- FechaPrestamo (DateTime)
- FechaDevolucionEstimada (DateTime)
- FechaDevolucionReal (DateTime?, nullable)
- Estado (string: "Activo", "Devuelto")
- Observaciones (string, opcional)
```

---

## ?? Comandos Útiles

### Entity Framework

```bash
# Crear una nueva migración
dotnet ef migrations add NombreDeLaMigracion

# Aplicar migraciones pendientes
dotnet ef database update

# Revertir a una migración específica
dotnet ef database update NombreDeLaMigracion

# Eliminar la última migración (si no se aplicó)
dotnet ef migrations remove

# Ver lista de migraciones
dotnet ef migrations list
```

### Compilación y Ejecución

```bash
# Compilar el proyecto
dotnet build

# Ejecutar el proyecto
dotnet run

# Ejecutar en modo watch (recarga automática)
dotnet watch run

# Limpiar archivos de compilación
dotnet clean
```

---

## ?? Contribución

Este es un proyecto académico (TFI - Trabajo Final Integrador).

---

## ?? Licencia

Este proyecto es de uso académico.

---

## ????? Autor

**Kippyru**
- GitHub: [@Kippyru](https://github.com/Kippyru)
- Repositorio: [TFI-BackEnd-Biblioteca](https://github.com/Kippyru/TFI-BackEnd-Biblioteca)

---

## ?? Soporte

Para consultas o problemas, crear un **Issue** en el repositorio de GitHub.
