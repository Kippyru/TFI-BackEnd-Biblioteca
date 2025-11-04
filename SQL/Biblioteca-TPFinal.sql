

USE Biblioteca;
GO

/*
-- Crear las Tablas RAÍZ (las que no tienen FKs)
*/
CREATE TABLE Editorial (
    ID_Editorial INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(150),
    Direccion VARCHAR(250),
    Telefono VARCHAR(20)
);

CREATE TABLE Estudiante (
    N_Legajo VARCHAR(20) PRIMARY KEY,
    Fecha_Ingreso_Facultad DATE,
    Materias_Cursando VARCHAR(100)
);
GO

/*
-- Crear Tablas de Nivel 1 (dependen de las raíces)
*/
CREATE TABLE Libro (
    ISBN VARCHAR(13) PRIMARY KEY,
    Titulo VARCHAR(250) NOT NULL,
    Categoria VARCHAR(100),
    Autores VARCHAR(500),
    Paginas INT,
    Fecha_Publicacion DATE,
    
    -- La relación con Editorial
    ID_Editorial INT,
    CONSTRAINT FK_Libro_Editorial FOREIGN KEY (ID_Editorial) REFERENCES Editorial(ID_Editorial)
);

CREATE TABLE Usuarios (
    ID_Usuario INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(150),
    Telefono VARCHAR(20),
    Direccion VARCHAR(250),
    
    -- La única relación que queda
    N_Legajo VARCHAR(20) NULL, -- 'NULL' permite usuarios que no sean estudiantes
    
    CONSTRAINT FK_Usuario_Estudiante FOREIGN KEY (N_Legajo) REFERENCES Estudiante(N_Legajo)
);
GO

/*
-- Crear Tablas de Nivel 2
*/
CREATE TABLE Prestamos (
    ID_Prestamo INT PRIMARY KEY IDENTITY(1,1),
    ID_User INT,
    Codigo_Inventario VARCHAR(50),
    Fecha_de_Inicio DATE,
    Fecha_de_Fin DATE,
    
    -- La relación con Usuarios
    ID_Usuario INT,
    CONSTRAINT FK_Prestamo_Usuario FOREIGN KEY (ID_Usuario) REFERENCES Usuarios(ID_Usuario)
);
GO

/*
-- Crear Tablas de Nivel 3 (las más dependientes)
*/
CREATE TABLE EjemplarLibros (
    ID_Codigo_Inventario VARCHAR(50) PRIMARY KEY, -- Corregido el nombre
    Codigo_Ubicacion VARCHAR(100),
    Fecha_Alta DATE,
    Fecha_Baja DATE,
    
    -- Relaciones
    ISBN VARCHAR(13),
    ID_Prestamo INT NULL, -- Lo pongo NULLable
    
    CONSTRAINT FK_Ejemplar_Libro FOREIGN KEY (ISBN) REFERENCES Libro(ISBN),
    CONSTRAINT FK_Ejemplar_Prestamo FOREIGN KEY (ID_Prestamo) REFERENCES Prestamos(ID_Prestamo)
);
GO
