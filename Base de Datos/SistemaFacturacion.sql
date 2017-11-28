-- Creamos la base de Datos:
go
create database SistemaFacturacion;
-- Seleccionamos la base de datos creada:
go
use SistemaFacturacion;

--USUARIO
--Los usuarios seran Admins o usuarios de la empresa, no se relaciona con clientes
--Con bit 0 o 1 lo pasaremos a bool en el programa y veremos si es admin o no


go
create table Usuario
(
	Usuario varchar(50) primary key not null,
	Contrasena varchar(16) not null,
	EsAdmin bit not null,
	--Agregamos el campo para el borrado logico
	habilitado bit not null,
)

-- Creamos las tablas:
-- CLIENTE
go
create table Cliente
(
	cedula int primary key not null,
	nombre varchar(50) not null,
	domicilio varchar(50) not null,
	fechaNacimiento datetime not null,
	--Agregamos el campo para ver si esta habilitado o no
	habilitado bit not null,

	--Por ultimo agregamos el nick del usuario para el historial
	nickUsuario varchar(50) not null,
	FOREIGN KEY(nickUsuario) REFERENCES Usuario(Usuario),
)

-- PRODUCTO
go
create table Producto
(
	id int primary key not null,
	nombre varchar(50) not null,
	marca varchar(50) not null,
	precio decimal not null,
	--Agregamos el campo para ver si esta habilitado o no
	habilitado bit not null,

	--Por ultimo agregamos el nick del usuario para el historial:
	nickUsuario varchar(50) not null,
	FOREIGN KEY(nickUsuario) REFERENCES Usuario(Usuario),
)



-- FACTURA
go
create table Factura
(
	--Creamos una primary key que se agregara sola:
	numeroFactura int primary key identity(1,1),

	fecha datetime not null,
	numeroCliente int not null,
	FOREIGN KEY(numerocliente) REFERENCES Cliente(cedula),
	total decimal not null,

	--Por ultimo agregamos el nick del usuario para el historial:
	nickUsuario varchar(50) not null,
	FOREIGN KEY(nickUsuario) REFERENCES Usuario(Usuario),
)



-- DETALLE FACTURA
go
create table DetallesFactura
(
	id int primary key identity(1,1),

	cantidad int not null,

	subTotal decimal not null,

	--EL idproducto hace referencia a la id del producto de la tabla producto:
	idProducto int not null,
	Foreign key(idProducto) references Producto(id),
	
	--La Factura hace referencia al numero de factura:
	factura int not null,
	FOREIGN KEY(factura) REFERENCES Factura(numeroFactura),
)



-- ************************************************************************* --

-- Agregamos para probar:

--USUARIOS
go
INSERT INTO Usuario values('SuperAdmin','5Uper@dmin',1,1)
INSERT INTO Usuario values('UnEmpleado','5Uper@mpleado',0,1)

--CLIENTES:
go
INSERT INTO Cliente values(56212039,'Daniel','Siempreviva',2001/12/12,1,'SuperAdmin')
INSERT INTO Cliente values(48292021,'camila','Siempreviva',2010/11/02,1,'SuperAdmin')
INSERT INTO Cliente values(35211239,'jorge','Siempreviva',2011/10/06,1,'SuperAdmin')
INSERT INTO Cliente values(19212113,'manuel','Siempreviva',2012/09/11,1,'SuperAdmin')

--PRODUCTOS
go
INSERT INTO Producto values(1,'Jabon','Dove',10,1,'UnEmpleado')
INSERT INTO Producto values(2,'Fideos','Adria',10,1,'UnEmpleado')
INSERT INTO Producto values(3,'Shampoo','Sedal',10,1,'UnEmpleado')
INSERT INTO Producto values(4,'Harina','Puritas',10,1,'SuperAdmin')
INSERT INTO Producto values(5,'Chocolate','Garoto',10,1,'SuperAdmin')

--FACTURAS
go
INSERT INTO Factura values(2012/09/11,56212039,50,'SuperAdmin')
INSERT INTO Factura values(2021/01/11,56212039,20,'UnEmpleado')
INSERT INTO Factura values(2014/12/12,48292021,20,'UnEmpleado')
INSERT INTO Factura values(2012/09/11,48292021,20,'UnEmpleado')
INSERT INTO Factura values(2021/01/11,35211239,20,'SuperAdmin')
INSERT INTO Factura values(2014/12/12,35211239,10,'UnEmpleado')
INSERT INTO Factura values(2012/09/11,19212113,10,'UnEmpleado')
INSERT INTO Factura values(2021/01/11,19212113,30,'SuperAdmin')

--DETALLES DE LAS FACTURA
go
INSERT INTO DetallesFactura values(1,10,1,1)
INSERT INTO DetallesFactura values(1,10,2,1)
INSERT INTO DetallesFactura values(1,10,2,1)
INSERT INTO DetallesFactura values(1,10,5,1)
INSERT INTO DetallesFactura values(1,10,3,1)
INSERT INTO DetallesFactura values(1,10,3,2)
INSERT INTO DetallesFactura values(1,10,4,2)
INSERT INTO DetallesFactura values(1,10,4,3)
INSERT INTO DetallesFactura values(1,10,3,3)
INSERT INTO DetallesFactura values(1,10,4,4)
INSERT INTO DetallesFactura values(1,10,4,4)
INSERT INTO DetallesFactura values(1,10,5,5)
INSERT INTO DetallesFactura values(1,10,5,5)
INSERT INTO DetallesFactura values(1,10,1,6)
INSERT INTO DetallesFactura values(1,10,1,7)
INSERT INTO DetallesFactura values(1,10,4,8)
INSERT INTO DetallesFactura values(1,10,4,8)
INSERT INTO DetallesFactura values(1,10,2,8)

Select * from Producto