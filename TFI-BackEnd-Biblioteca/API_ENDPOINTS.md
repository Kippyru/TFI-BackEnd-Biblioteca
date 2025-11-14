# ?? API de Biblioteca - Documentación de Endpoints

## ?? Base URL
```
https://localhost:7063/api
```

## ?? Documentación Interactiva
```
https://localhost:7063/scalar/v1
```

---

## ?? LIBROS

### Obtener todos los libros
```http
GET /api/libros
```

### Obtener libro por ID
```http
GET /api/libros/{id}
```

### Obtener libros disponibles
```http
GET /api/libros/disponibles
```

### Buscar libros
```http
GET /api/libros/buscar?termino=garcia
```
Busca por título, autor o ISBN.

### Crear nuevo libro
```http
POST /api/libros
Content-Type: application/json

{
  "titulo": "Cien años de soledad",
  "autor": "Gabriel García Márquez",
  "isbn": "9780307474728",
  "editorial": "Editorial Sudamericana",
  "añoPublicacion": 1967,
  "genero": "Realismo mágico",
  "cantidadDisponible": 5,
  "cantidadTotal": 5
}
```

### Actualizar libro
```http
PUT /api/libros/{id}
Content-Type: application/json

{
  "id": 1,
  "titulo": "Cien años de soledad",
  "autor": "Gabriel García Márquez",
  "isbn": "9780307474728",
  "editorial": "Editorial Sudamericana",
  "añoPublicacion": 1967,
  "genero": "Realismo mágico",
  "cantidadDisponible": 3,
  "cantidadTotal": 5,
  "fechaRegistro": "2024-01-01T00:00:00"
}
```

### Eliminar libro
```http
DELETE /api/libros/{id}
```
?? No se puede eliminar si tiene préstamos activos.

---

## ?? SOCIOS

### Obtener todos los socios
```http
GET /api/socios
```

### Obtener socio por ID
```http
GET /api/socios/{id}
```
Incluye los préstamos del socio.

### Obtener socios activos
```http
GET /api/socios/activos
```

### Buscar socios
```http
GET /api/socios/buscar?termino=perez
```
Busca por nombre, apellido o email.

### Obtener préstamos de un socio
```http
GET /api/socios/{id}/prestamos
```

### Obtener préstamos activos de un socio
```http
GET /api/socios/{id}/prestamos/activos
```

### Crear nuevo socio
```http
POST /api/socios
Content-Type: application/json

{
  "nombre": "Juan",
  "apellido": "Pérez",
  "email": "juan.perez@example.com",
  "telefono": "+54 11 1234-5678",
  "direccion": "Av. Corrientes 1234, CABA"
}
```

### Actualizar socio
```http
PUT /api/socios/{id}
Content-Type: application/json

{
  "id": 1,
  "nombre": "Juan",
  "apellido": "Pérez",
  "email": "juan.perez@example.com",
  "telefono": "+54 11 1234-5678",
  "direccion": "Av. Corrientes 1234, CABA",
  "fechaRegistro": "2024-01-01T00:00:00",
  "activo": true
}
```

### Desactivar socio
```http
PUT /api/socios/{id}/desactivar
```
?? No se puede desactivar si tiene préstamos activos.

### Activar socio
```http
PUT /api/socios/{id}/activar
```

### Eliminar socio
```http
DELETE /api/socios/{id}
```
?? No se puede eliminar si tiene préstamos registrados.

---

## ?? PRÉSTAMOS

### Obtener todos los préstamos
```http
GET /api/prestamos
```
Incluye información del libro y socio.

### Obtener préstamo por ID
```http
GET /api/prestamos/{id}
```

### Obtener préstamos activos
```http
GET /api/prestamos/activos
```

### Obtener préstamos atrasados
```http
GET /api/prestamos/atrasados
```
Préstamos activos cuya fecha de devolución estimada ya pasó.

### Crear nuevo préstamo
```http
POST /api/prestamos
Content-Type: application/json

{
  "libroId": 1,
  "socioId": 1,
  "fechaDevolucionEstimada": "2024-12-31T00:00:00"
}
```
Si no se especifica `fechaDevolucionEstimada`, se establece automáticamente en 15 días.

**Validaciones:**
- El libro debe existir y tener ejemplares disponibles
- El socio debe existir y estar activo
- El socio no debe tener préstamos atrasados

### Devolver préstamo
```http
PUT /api/prestamos/{id}/devolver
Content-Type: application/json

"Observaciones opcionales: libro en buen estado"
```

### Renovar préstamo
```http
PUT /api/prestamos/{id}/renovar?dias=15
```
Extiende la fecha de devolución estimada.

### Actualizar préstamo
```http
PUT /api/prestamos/{id}
Content-Type: application/json

{
  "id": 1,
  "libroId": 1,
  "socioId": 1,
  "fechaPrestamo": "2024-01-01T00:00:00",
  "fechaDevolucionEstimada": "2024-01-15T00:00:00",
  "fechaDevolucionReal": null,
  "estado": "Activo",
  "observaciones": ""
}
```

### Eliminar préstamo
```http
DELETE /api/prestamos/{id}
```
Si el préstamo está activo, restaura la cantidad disponible del libro.

---

## ?? Estados de Préstamos

- **Activo**: Préstamo en curso
- **Devuelto**: Préstamo completado
- **Atrasado**: Se determina cuando `FechaDevolucionEstimada < DateTime.Now` y `Estado == "Activo"`

---

## ?? Códigos de Respuesta HTTP

- `200 OK`: Operación exitosa
- `201 Created`: Recurso creado exitosamente
- `204 No Content`: Operación exitosa sin contenido de respuesta
- `400 Bad Request`: Datos inválidos o reglas de negocio no cumplidas
- `404 Not Found`: Recurso no encontrado

---

## ?? Reglas de Negocio

### Libros
- No se puede eliminar un libro con préstamos activos
- La cantidad disponible se actualiza automáticamente al crear/devolver préstamos

### Socios
- El email debe ser único
- No se puede desactivar un socio con préstamos activos
- No se puede eliminar un socio con préstamos registrados
- Un socio desactivado no puede realizar préstamos

### Préstamos
- Un socio con préstamos atrasados no puede realizar nuevos préstamos
- Al crear un préstamo, se reduce la cantidad disponible del libro
- Al devolver un préstamo, se incrementa la cantidad disponible del libro
- Solo se pueden renovar préstamos activos
