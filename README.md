# Proyecto de jardinería 🌿

Este proyecto proporciona una API que permite llevar el control, gestión y registro de todos los productos y servicios de una jardinería.

## Características 🌟

- Registro de usuarios.
- Autenticación con usuario y contraseña.
- Generación y utilización del token.
- CRUD completo para cada entidad.
- Vista de las consultas requeridas.

## Uso 🕹

Una vez que el proyecto esté en marcha, puedes acceder a los diferentes endpoints disponibles:

 En el archivo CSV se encuentra registrado el administrador con:   
 **Usuario**: `Admini`  
 **Contraseña**: `pass1234`       
Necesitaremos de este usuario para obtener el token que se utilizará para el registro de usuarios, ya que solo el administrador podra hacer todo con respecto al CRUD de los usuarios.

## 1. Generación del token 🔑:

**Endpoint**: `http://localhost:5033/api/usuario/token`

**Método**: `POST`

**Payload**:

    {
        "Nombre": "Admini",
        "Contraseña": "pass1234"
    }

Al obtener el token del administrador, se podrá realizar el registro de usuarios.

## 2. Registro de Usuarios 📝:

**Endpoint**: `http://localhost:5033/api/usuario/register`

**Método**: `POST`

**Payload**:

Json

    {
        "Nombre": "<nombre_de_usuario>",
        "Contraseña": "<contraseña>",
        "CorreoElectronico": "<correo_electronico>"
    }

Este endpoint permite a los usuarios registrarse en el sistema.

Una vez registrado el usuario tendrá que ingresar para recibir un token, este será ingresado al siguiente Endpoint que es el de Refresh Token.

## 3. Refresh Token 🔄:

**Endpoint**: `http://localhost:5033/api/usuario/refresh-token`

**Método**: `POST`

**Payload**:

    {
        "Nombre": "<nombre_de_usuario>",
        "Contraseña": "<contraseña>"
    }

Se dejan los mismos datos en el Body y luego se ingresa al "Auth", "Bearer", allí se ingresa el token obtenido en el anterior Endpoint.

## Otros Endpoints

Obtener Todos los Usuarios: **GET** `http://localhost:5033/api/usuario`

Obtener Usuario por ID: **GET** `http://localhost:5033/api/usuario/{id}`

Actualizar Usuario: **PUT** `http://localhost:5033/api/usuario/{id}`

Eliminar Usuario: **DELETE** `http://localhost:5033/api/usuario/{id}`


## Desarrollo de los Endpoints requeridos⌨️

Hay Endpoints que tiene su versión 1.0 y 1.1, al igual que están con y sin paginación.

### Endpoint con paginación 📄

**1.** Devuelve un listado con el nombre de los todos los clientes españoles.    


**2.** Devuelve un listado con los distintos estados por los que puede pasar un pedido.


**3.** Devuelve un listado con el código de cliente de aquellos clientes que realizaron algún pago en 2008. Tenga en cuenta que deberá eliminar aquellos códigos de cliente que aparezcan repetidos.   


**4.** Devuelve un listado con el código de pedido, código de cliente, fecha esperada y fecha de entrega de los pedidos que no han sido entregados a tiempo. 


**8.** Devuelve un listado con todos los pagos que se realizaron en el año 2008 mediante Paypal. Ordene el resultado de mayor a menor.    


**9.** Devuelve un listado con todas las formas de pago que aparecen en la tabla pago. Tenga en cuenta que no deben aparecer formas de pago repetidas.


**10.** Devuelve un listado con todos los productos que pertenecen a la gama Ornamentales y que tienen más de 100 unidades en stock. El listado deberá estar ordenado por su precio de venta, mostrando en primer lugar los de mayor precio. 


**17.** Devuelve un listado que muestre el nombre de cada empleados, el nombre de su jefe y el nombre del jefe de sus jefe.


**19.** Devuelve un listado de las diferentes gamas de producto que ha comprado cada cliente.    


**22.** Devuelve un listado que muestre solamente los empleados que no tienen un cliente asociado junto con los datos de la oficina donde trabajan.


**24.** Devuelve un listado de los productos que nunca han aparecido en un pedido.


**26.** Devuelve las oficinas donde no trabajan ninguno de los empleados que hayan sido los representantes de ventas de algún cliente que haya realizado la compra de algún producto de la gama Frutales.    


Para consultar la versión 1.0 de todos se ingresa únicamente el Endpoint; para consultar la versión 1.1 se deben seguir los siguientes pasos: 

En el Thunder Client se va al apartado de "Headers" y se ingresa lo siguiente:

![image](https://github.com/SilviaJaimes/Proyecto-Veterinaria/assets/132016483/8044ee3d-76d9-4437-9f08-da8e5d7cff9a)

Para realizar la paginación se va al apartado de "Query" y se ingresa lo siguiente:

![image](https://github.com/SilviaJaimes/Proyecto-Veterinaria/assets/132016483/22683e46-037e-4f30-96b8-161df8622b40)      

#### 1. Devuelve un listado con el nombre de los todos los clientes españoles:  

Endpoint: `http://localhost:5033/api/cliente/consulta-1`  

Método: `GET`  


#### 2. Devuelve un listado con los distintos estados por los que puede pasar un pedido: 

Endpoint: `http://localhost:5033/api/pedido/consulta-2`  

Método: `GET`  


#### 3. Devuelve un listado con el código de cliente de aquellos clientes que realizaron algún pago en 2008. Tenga en cuenta que deberá eliminar aquellos códigos de cliente que aparezcan repetidos:  

Endpoint: `http://localhost:5033/api/cliente/consulta-3`  

Método: `GET`  


#### 4. Devuelve un listado con el código de pedido, código de cliente, fecha esperada y fecha de entrega de los pedidos que no han sido entregados a tiempo:  

Endpoint: `http://localhost:5033/api/pedido/consulta-4`  

Método: `GET`  


#### 5. Devuelve un listado con el código de pedido, código de cliente, fecha esperada y fecha de entrega de los pedidos cuya fecha de entrega ha sido al menos dos días antes de la fecha esperada:  

Endpoint: `http://localhost:5033/api/pedido/consulta-5`  

Método: `GET`  


#### 6. Devuelve un listado de todos los pedidos que fueron rechazados en 2009:  

Endpoint: `http://localhost:5033/api/pedido/consulta-6`  

Método: `GET`  


#### 7. Devuelve un listado de todos los pedidos que han sido entregados en el mes de enero de cualquier año:  

Endpoint: `http://localhost:5033/api/pedido/consulta-7`  

Método: `GET`  


#### 8. Devuelve un listado con todos los pagos que se realizaron en el año 2008 mediante Paypal. Ordene el resultado de mayor a menor:  

Endpoint: `http://localhost:5033/api/pago/consulta-8`  

Método: `GET`  


#### 9.  Devuelve un listado con todas las formas de pago que aparecen en la tabla pago. Tenga en cuenta que no deben aparecer formas de pago repetidas:  

Endpoint: `http://localhost:5033/api/pago/consulta-9`  

Método: `GET`  


#### 10. Devuelve un listado con todos los productos que pertenecen a la gama Ornamentales y que tienen más de 100 unidades en stock. El listado deberá estar ordenado por su precio de venta, mostrando en primer lugar los de mayor precio:  

Endpoint: `http://localhost:5033/api/producto/consulta-10`  

Método: `GET`  


#### 11. Devuelve un listado con todos los clientes que sean de la ciudad de Madrid y cuyo representante de ventas tenga el código de empleado 11 o 30:  

Endpoint: `http://localhost:5033/api/cliente/consulta-11`  

Método: `GET`  


#### 12. Obtén un listado con el nombre de cada cliente y el nombre y apellido de su representante de ventas:  

Endpoint: `http://localhost:5033/api/cliente/consulta-12`  

Método: `GET`  


#### 13. Muestra el nombre de los clientes que hayan realizado pagos junto con el nombre de sus representantes de ventas:  

Endpoint: `http://localhost:5033/api/cliente/consulta-13`  

Método: `GET`  


#### 14. Muestra el nombre de los clientes que no hayan realizado pagos junto con el nombre de sus representantes de ventas:  

Endpoint: `http://localhost:5033/api/cliente/consulta-14`  

Método: `GET`  


#### 15. Devuelve el nombre de los clientes que han hecho pagos y el nombre de sus representantes junto con la ciudad de la oficina a la que pertenece el representante:  

Endpoint: `http://localhost:5033/api/cliente/consulta-15`  

Método: `GET`  


#### 16. Devuelve el nombre de los clientes que no hayan hecho pagos y el nombre de sus representantes junto con la ciudad de la oficina a la que pertenece el representante:  

Endpoint: `http://localhost:5033/api/cliente/consulta-16`  

Método: `GET`  


#### 17. Devuelve un listado que muestre el nombre de cada empleados, el nombre de su jefe y el nombre del jefe de sus jefe:  

Endpoint: `http://localhost:5033/api/empleado/consulta-17`  

Método: `GET`  


#### 18. Devuelve el nombre de los clientes a los que no se les ha entregado a tiempo un pedido:  

Endpoint: `http://localhost:5033/api/cliente/consulta-18`  

Método: `GET`  


#### 19. Devuelve un listado de las diferentes gamas de producto que ha comprado cada cliente:  

Endpoint: `http://localhost:5033/api/gamaProducto/consulta-19`  

Método: `GET`  


#### 20. Devuelve un listado que muestre solamente los clientes que no han realizado ningún pago:  

Endpoint: `http://localhost:5033/api/cliente/consulta-20`  

Método: `GET`  


#### 21. Devuelve un listado que muestre los clientes que no han realizado ningún pago y los que no han realizado ningún pedido:  

Endpoint: `http://localhost:5033/api/cliente/consulta-21`  

Método: `GET`  


#### 22. Devuelve un listado que muestre solamente los empleados que no tienen un cliente asociado junto con los datos de la oficina donde trabajan:  

Endpoint: `http://localhost:5033/api/empleado/consulta-22`  

Método: `GET`  


#### 23. Devuelve un listado que muestre los empleados que no tienen una oficina asociada y los que no tienen un cliente asociado:  

Endpoint: `http://localhost:5033/api/empleado/consulta-23`  

Método: `GET`  


#### 24. Devuelve un listado de los productos que nunca han aparecido en un pedido:  

Endpoint: `http://localhost:5033/api/producto/consulta-24`  

Método: `GET`  


#### 25. Devuelve un listado de los productos que nunca han aparecido en un pedido. El resultado debe mostrar el nombre, la descripción y la imagen del producto:  

Endpoint: `http://localhost:5033/api/producto/consulta-25`  

Método: `GET`  


#### 26. Devuelve las oficinas donde no trabajan ninguno de los empleados que hayan sido los representantes de ventas de algún cliente que haya realizado la compra de algún producto de la gama Frutales:  

Endpoint: `http://localhost:5033/api/oficina/consulta-26`  

Método: `GET` 


#### 27. Devuelve un listado con los clientes que han realizado algún pedido pero no han realizado ningún pago:  

Endpoint: `http://localhost:5033/api/cliente/consulta-27`  

Método: `GET`  


#### 28. Devuelve un listado con los datos de los empleados que no tienen clientes asociados y el nombre de su jefe asociado:  

Endpoint: `http://localhost:5033/api/empleado/consulta-28`  

Método: `GET`  


#### 29. ¿Cuántos empleados hay en la compañía?:  

Endpoint: `http://localhost:5033/api/empleado/consulta-29`  

Método: `GET`  


#### 30. ¿Cuántos clientes tiene cada país?:  

Endpoint: `http://localhost:5033/api/cliente/consulta-30`  

Método: `GET`  


#### 31. ¿Cuál fue el pago medio en 2009?:  

Endpoint: `http://localhost:5033/api/pago/consulta-31`  

Método: `GET`  


#### 32. ¿Cuántos pedidos hay en cada estado? Ordena el resultado de forma descendente por el número de pedidos:  

Endpoint: `http://localhost:5033/api/pedido/consulta-32`  

Método: `GET`  


#### 33. ¿Cuántos clientes existen con domicilio en la ciudad de Madrid?:  

Endpoint: `http://localhost:5033/api/cliente/consulta-33`  

Método: `GET`  


#### 34. ¿Calcula cuántos clientes tiene cada una de las ciudades que empiezan por M?:  

Endpoint: `http://localhost:5033/api/cliente/consulta-34`  

Método: `GET`  


#### 35. Devuelve el nombre de los representantes de ventas y el número de clientes al que atiende cada uno:  

Endpoint: `http://localhost:5033/api/empleado/consulta-35`  

Método: `GET`  


#### 36. Calcula el número de clientes que no tiene asignado representante de ventas:  

Endpoint: `http://localhost:5033/api/cliente/consulta-36`  

Método: `GET`  


#### 37. Calcula la fecha del primer y último pago realizado por cada uno de los clientes. El listado deberá mostrar el nombre y los apellidos de cada cliente:  

Endpoint: `http://localhost:5033/api/cliente/consulta-37`  

Método: `GET`  


#### 38. Calcula el número de productos diferentes que hay en cada uno de los pedidos:  

Endpoint: `http://localhost:5033/api/pedido/consulta-38`  

Método: `GET`  


#### 39. Calcula la suma de la cantidad total de todos los productos que aparecen en cada uno de los pedidos:  

Endpoint: `http://localhost:5033/api/pedido/consulta-39`  

Método: `GET`  


#### 40. Devuelve un listado de los 20 productos más vendidos y el número total de unidades que se han vendido de cada uno. El listado deberá estar ordenado por el número total de unidades vendidas:  

Endpoint: `http://localhost:5033/api/producto/consulta-40`  

Método: `GET`  


#### 41. La misma información que en la pregunta anterior, pero agrupada por código de producto:  

Endpoint: `http://localhost:5033/api/producto/consulta-41`  

Método: `GET`  


#### 42. La misma información que en la pregunta anterior, pero agrupada por código de producto filtrada por los códigos que empiecen por OR:  

Endpoint: `http://localhost:5033/api/producto/consulta-42`  

Método: `GET`  


#### 43. Lista las ventas totales de los productos que hayan facturado más de 3000 euros. Se mostrará el nombre, unidades vendidas, total facturado y total facturado con impuestos (21% IVA):  

Endpoint: `http://localhost:5033/api/producto/consulta-43`  

Método: `GET`  


#### 44. Muestre la suma total de todos los pagos que se realizaron para cada uno de los años que aparecen en la tabla pagos:  

Endpoint: `http://localhost:5033/api/pago/consulta-44`  

Método: `GET`  


#### 45. Devuelve el nombre del cliente con mayor límite de crédito:  

Endpoint: `http://localhost:5033/api/cliente/consulta-45`  

Método: `GET`  


#### 46. Devuelve el nombre del producto que tenga el precio de venta más caro:  

Endpoint: `http://localhost:5033/api/producto/consulta-46`  

Método: `GET`  


#### 47. Devuelve el nombre del producto del que se han vendido más unidades:  

Endpoint: `http://localhost:5033/api/producto/consulta-47`  

Método: `GET`  


#### 48. Los clientes cuyo límite de crédito sea mayor que los pagos que haya realizado:  

Endpoint: `http://localhost:5033/api/cliente/consulta-48`  

Método: `GET`  


#### 49. Devuelve el nombre del cliente con mayor límite de crédito:  

Endpoint: `http://localhost:5033/api/cliente/consulta-49`  

Método: `GET`  


#### 50. Devuelve el nombre del producto que tenga el precio de venta más caro:  

Endpoint: `http://localhost:5033/api/producto/consulta-50`  

Método: `GET`  


#### 51. Devuelve un listado que muestre solamente los clientes que no han realizado ningún pago:  

Endpoint: `http://localhost:5033/api/cliente/consulta-51`  

Método: `GET`  


#### 52. Devuelve un listado que muestre solamente los clientes que sí han realizado algún pago:  

Endpoint: `http://localhost:5033/api/cliente/consulta-52`  

Método: `GET` 


#### 53. Devuelve un listado de los productos que nunca han aparecido en un pedido:  

Endpoint: `http://localhost:5033/api/producto/consulta-53`  

Método: `GET`  


#### 54. Devuelve el nombre, apellidos, puesto y teléfono de la oficina de aquellos empleados que no sean representante de ventas de ningún cliente:  

Endpoint: `http://localhost:5033/api/empleado/consulta-54`  

Método: `GET`  


#### 55. Devuelve un listado que muestre solamente los clientes que no han realizado ningún pago:  

Endpoint: `http://localhost:5033/api/cliente/consulta-55`  

Método: `GET`  


#### 56. Devuelve un listado que muestre solamente los clientes que sí han realizado algún pago:  

Endpoint: `http://localhost:5033/api/cliente/consulta-56`  

Método: `GET`  


#### 57. Devuelve el listado de clientes indicando el nombre del cliente y cuántos pedidos ha realizado. Tenga en cuenta que pueden existir clientes que no han realizado ningún pedido:  

Endpoint: `http://localhost:5033/api/cliente/consulta-57`  

Método: `GET`  


#### 58. Devuelve el nombre de los clientes que hayan hecho pedidos en 2008 ordenados alfabéticamente de menor a mayor:  

Endpoint: `http://localhost:5033/api/cliente/consulta-58`  

Método: `GET`  


#### 59. Devuelve el nombre del cliente, el nombre y primer apellido de su representante de ventas y el número de teléfono de la oficina del representante de ventas, de aquellos clientes que no hayan realizado ningún pago:  

Endpoint: `http://localhost:5033/api/cliente/consulta-59`  

Método: `GET`  


#### 60. Devuelve el listado de clientes donde aparezca el nombre del cliente, el nombre y primer apellido de su representante de ventas y la ciudad donde está su oficina:  

Endpoint: `http://localhost:5033/api/cliente/consulta-60`  

Método: `GET`  


#### 61. Devuelve el nombre, apellidos, puesto y teléfono de la oficina de aquellos empleados que no sean representante de ventas de ningún cliente:  

Endpoint: `http://localhost:5033/api/empleado/consulta-61`  

Método: `GET`  


## Desarrollo ⌨️
Este proyecto utiliza varias tecnologías y patrones, incluidos:

Patrón Repository y Unit of Work para la gestión de datos.

AutoMapper para el mapeo entre entidades y DTOs.

## Agradecimientos 🎁

A todas las librerías y herramientas utilizadas en este proyecto.

A ti, por considerar el uso de este sistema.

⌨️ con ❤️ por Silvia.
