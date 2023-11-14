# Proyecto de veterinaria

Este proyecto proporciona una API que permite gestionar todo el apartado de la administración de una veterinaria.

## Características 🌟

- Registro de usuarios.
- Autenticación con usuario y contraseña.
- Generación y utilización del token.
- CRUD completo para cada entidad.
- Vista de las consultas requeridas.

## Uso 🕹

Una vez que el proyecto esté en marcha, puedes acceder a los diferentes endpoints disponibles:

 En el archivo CSV se encuentra registrado el administrador con:  
 **usuario**: `Admini`  
 **Contraseña**: `pass1234`   
 Necesitaremos de este usuario para obtener el token que se utilizará para el registro de usuarios, ya que solo el administrador podra hacer todo con respecto al CRUD de los usuarios.

## 1. Generación del token 🔑:

**Endpoint**: `http://localhost:5158/api/usuario/token`

**Método**: `POST`

**Payload**:

`{
    "Nombre": "Admini",
    "Contraseña": "pass1234"
}`

Al obtener el token del administrador, se podrá realizar el registro de usuarios.

## 2. Registro de Usuarios 📝:

**Endpoint**: `http://localhost:5158/api/usuario/register`

**Método**: `POST`

**Payload**:

json
`{
    "Usuario": "<nombre_de_usuario>",
    "Contraseña": "<contraseña>",
    "CorreoElectronico": "<correo_electronico>"
}`

Este endpoint permite a los usuarios registrarse en el sistema.

Una vez registrado el usuario tendrá que ingresar para recibir un token, este será ingresado al siguiente Endpoint que es el de Refresh Token.

## 3. Refresh Token 🔄:

**Endpoint**: `http://localhost:5158/api/usuario/refresh-token`

**Método**: `POST`

**Payload**:

`{
    "Nombre": "<nombre_de_usuario>",
    "Contraseña": "<contraseña>"
}`

Se dejan los mismos datos en el Body y luego se ingresa al "Auth", "Bearer", allí se ingresa el token obtenido en el anterior Endpoint.

**Otros Endpoints**

Obtener Todos los Usuarios: GET `http://localhost:5158/api/usuario`

Obtener Usuario por ID: GET `http://localhost:5158/api/usuario/{id}`

Actualizar Usuario: PUT `http://localhost:5158/api/usuario/{id}`

Eliminar Usuario: DELETE `http://localhost:5158/api/usuario/{id}`


## Desarrollo de los Endpoints requeridos⌨️

Cada Endpoint tiene su versión 1.0 y 1.1, al igual que están con y sin paginación.

Para consultar la versión 1.0 de todos se ingresa únicamente el Endpoint; para consultar la versión 1.1 se deben seguir los siguientes pasos: 

En el Thunder Client se va al apartado de "Headers" y se ingresa lo siguiente:

![image](https://github.com/SilviaJaimes/Proyecto-Veterinaria/assets/132016483/8044ee3d-76d9-4437-9f08-da8e5d7cff9a)

Para realizar la paginación se va al apartado de "Query" y se ingresa lo siguiente:

![image](https://github.com/SilviaJaimes/Proyecto-Veterinaria/assets/132016483/22683e46-037e-4f30-96b8-161df8622b40)


## 1. Visualizar los veterinarios cuya especialidad sea Cirujano vascular 🩺:

**Endpoint**: `http://localhost:5158/api/veterinario/consulta-1/{Especialidad}`

**Método**: `GET`

En este caso para realizar la consulta se ingresa la especialidad de la que quiera conocer los veterinarios en la última parte del Endpoint: {Especialidad} = `Cirujano vascular`.

## 2. Listar los propietarios y sus mascotas 🚶‍♂️🐕:

**Endpoint**: `http://localhost:5158/api/propietario/consulta-4`

**Método**: `GET`




## Desarrollo ⌨️
Este proyecto utiliza varias tecnologías y patrones, incluidos:

Patrón Repository y Unit of Work para la gestión de datos.

AutoMapper para el mapeo entre entidades y DTOs.

## Agradecimientos 🎁

A todas las librerías y herramientas utilizadas en este proyecto.

A ti, por considerar el uso de este sistema.

⌨️ con ❤️ por Silvia.
