# 🏛️ Simulador Legislativo ![Estado del Proyecto](https://img.shields.io/badge/Estado-Prototipo-yellow) ![.NET Version](https://img.shields.io/badge/.NET-8.0-blue) ![.NET Version](https://img.shields.io/badge/Razor-violet)

## 📝 Sitio web
http://expedientesar.somee.com

## 📝 Descripción
Es un prototipo que simula la gestión, clasificación y visualización del proceso legislativo sobre proyectos de ley para un Legislador en particular.

## 🎯 Propósito
El objetivo del este proyecto es aprender las tecnologías AspNet Core 8 + Razor para la creación de aplicaciones web aplicando algunas prácticas de Ingeniería de Software.

## 🚀 Características Principales
*   **Gestión básica de Proyectos de Ley:** Creación, edición y categorización de proyectos.
*   **Motor de Clasificación:** Sistema básico que simula la asignación de comisiones basado en palabras claves (Salud, Presupuesto, Trabajo, etc.).
*   **Interfaz Dinámica:** Dashboard construido con ASP.NET Core MVC y Bootstrap para una visualización clara del estado parlamentario.

## 🛠️ Stack Tecnológico
*   **Backend:** C# con .NET 8.0 y ASP.NET Core MVC extendido.
*   **Base de Datos:** InMemoryRepositories, SQL Server / Entity Framework Core (configurado).
*   **Frontend:** Razor Views, Bootstrap 5, y SweetAlert2 para notificaciones interactivas.
*   **Arquitectura:** Clean Architecture (Separación de preocupaciones).

## 📂 Estructura del Proyecto
*   `/Controllers`: Lógica de control y rutas del website y simulador.
*   `/Domain/Models`: Definición de entidades.
*   `/Domain/Services`: Definición de Services de Dominio (IProjectService, ...).
*   `/Domain/Repositories`: Definición de Repositorios de Dominio (IProjectRepository, ...).
*   `/Infrastructure`: Implementaciones de Servicios y Repositorios de Dominio.
*   `/Infrastructure/Interfaces`: Interfaces de infrastructura. 
*   `/Infrastructure/Services`: Implementaciones de servicios.
*   `/Infrastructure/Repositories`: Implementaciones de repositorios.
*   `/Filters`: Manejo de Excepciones y Validaciones.
*   `/Views`: Plantillas Razor para la interfaz de usuario.
*   `/ViewsModels`: DTOs para comunicación de datos con la vista a través de DataBinding.

## 🛡️ Consideraciones
* Hay muchas **funcionalidades que no están desarrolladas** por temas de complejidad y falta de conocimiento interno del área legislativa.
* El sitio web solo contempla la **creación, edición y visualización de proyectos** de ley para un Legislador.
* Se deben **simular los resultados que no son por acciones del usuario** (Legislador). Para hacerlo, se usan métodos POST al endpoint http://expedientesar.somee.com/api/Simulation
* **No hay login de usuario:** la aplicación supone que eres un Legislador y ya estas logueado.
* **La persistencia de datos** complejos sucede en memoria (por motivos de tiempo de entrega).
* Para comprobar una **conección a base de datos** sql: http://expedientesar.somee.com/api/person

 
# Cómo usar
* **Flujo** básico de proyectos: Crear Proyecto, Editar Proyecto, Enviar Proyecto, Asignación de Comisiones, Evaluación de cada Comisión, Enviar a Sesión y Dictaminar.
* **Ir** a http://expedientesar.somee.com
    * Se visualizan los proyectos actuales del Legislador (precargados en memoria).
    * Lo más importante es el _estado de cada proyecto_, los cuales se van a ir modificando en tanto se realicen acciones de Legislador y Simulador.
* **Estados** de un proyecto:
    * Borrador
    * Enviado a Comisiones
    * En Comisiones
    * Rechazado por Comisiones
    * En Sesión
    * Aprobado
    * Rechazado en Sesión
    * Eliminado por Legislador
* **Editar**: abre un proyecto en _estado Borrador_ para hacer modifiaciones.
    * **Borrar**: es el único momento en el que se puede _Eliminar_ un proyecto de ley.
    * **Actualizar**: guarda los cambios realizados.
    * **Enviar**: Cambia el estado del proyecto a _Enviado a Comisiones_.
        * A partir de acá, solo se puede _ver_ y hacer modificaciones _a través del simulador_.
* **Ver**: Ver el proyecto en modo _Solo Lectura_, luego de _enviar a comisiones_.
    * Si el proyecto ya tiene _Comisiones asociadas_, se mostrarán como tags.
* **Comisiones**: Acción que aparece solo si el proyecto fue _asignado a Comisiones_.
    * Permite ver los _Giros de Comisiones_ a los que el proyecto está sujeto. En dicha página se ven los _Giros de Comisiones_ y sus _estados_.
* **Crear proyecto**
    1. Ir a http://expedientesar.somee.com/Projects
    2. Presionar "Nuevo proyecto"
    3. Ingresar valores para cada campo.
    4. Presionar "Crear". El proyecto aparecerá en la _tabla de proyectos_.
    * **Nota**: puedes usar los siguientes campos como ejemplo:
        * **Título**: Ley de Salud Mental
        * **Artículos**: 
            * Art. 20°.- La internación involuntaria se considera una medida terapéutica excepcional. Se procederá a ella cuando el equipo de salud determine la existencia de riesgo cierto e inminente para sí o para terceros, debiendo notificarse al juez en un plazo de diez (10) horas.
            * Art. 27°.- Queda prohibida la creación de nuevos manicomios, pero se faculta la modernización y especialización de los hospitales monovalentes existentes como centros de referencia regional.
            * Art. 39°.- Las fuerzas de seguridad que intervengan en traslados sanitarios deberán recibir capacitación específica para el abordaje de personas en crisis, garantizando el respeto a la dignidad humana.
        * **Fundamentos**: Optimizar la respuesta del sistema de salud ante crisis agudas, otorgando herramientas más ágiles a las familias y médicos para intervenir en casos de adicciones graves o riesgo de vida.
        * **Resumen**: Normativa que equilibra los derechos del paciente con la necesidad de intervención médica urgente, permitiendo el uso de centros especializados y simplificando procesos de internación en crisis.
* Página de **Giros de Comisiones**
    * Tabla que refleja los _Giros de Comisiones_ asociadas a un proyecto y el _estado_ actual de cada _Giro_.
    * **Estados de Giro**:
        * Asignado: Primer estado, luego de realizarse la _Asignación de Comisiones_.
        * Evaluando: Primera petición de "Realizar siguiente Operación de Giro".
        * Aprobado: "Realizar siguiente Operación de Giro" con 50% de ser _aprobado_.
        * Rechazado: "Realizar siguiente Operación de Giro" con 50% de ser _rechazado_.

* Realizar **simulaciones:** Ver apartado _Simulaciones_. **Nota:** Cada vez que se realiza algún cambio en las entidades, se _genera_ una notificación. Para que aparezca la notificación, ir a http://expedientesar.somee.com/Projects .

* Una vez que el proyecto llega al estado _Aprobado_ o _Rechazado en Sesión_, se da por **finalizado el flujo**.

## Simulaciones
### > Asignar comisiones a un proyecto
```
POST http://expedientesar.somee.com/api/Simulation/AssignCommissions/{projectId}
```
_Ejemplo_: http://expedientesar.somee.com/api/Simulation/AssignCommissions/1

**Requisitos:** El proyecto tiene que estar _Enviado a Comisiones_

**Efectos:** Cambio de estado del proyecto a _En Comisiones_ y aparición de botón _Comisiones_.

**Lógica de asignación**: Una Comisión es asignada si alguna palabra dentro de Artículos del Proyecto coincide con al menos una de las palabras claves de dicha comisión. Si no se encuentran coincidencias, el proyecto es _Rechazado por Comisiones_.

### > Realizar siguiente Operación de Giro
```
POST http://expedientesar.somee.com/api/Simulation/DoReferring/{projectId}
```
_Ejemplo_: http://expedientesar.somee.com/api/Simulation/DoReferring/1

**Requisitos:** El proyecto tiene que estar en _En Comisiones_.

**Efectos:** Cambios en los _estados de Giro (Evaluando/Aprobado/Rechazado por Comisiones)_.  Luego, ver página de _Comisiones_.

**Nota**: continuar enviado peticiones hasta que todos los giros finalicen en _Aprobado o Rechazado por Comisiones_. Ver _Estados de Giro_.

### > Enviar a Sesión
```
POST http://expedientesar.somee.com/api/Simulation/SendToSession/{projectId}
```
_Ejemplo_: POST http://expedientesar.somee.com/api/Simulation/SendToSession/1

**Requisitos:** Todas las comisiones deben haber _aprobado_ el proyecto.

**Efectos:** Cambio de estado del proyecto a _En Sesión_.

### > Dictaminar
```
POST http://expedientesar.somee.com/api/Simulation/DoSession/{projectId}
```
_Ejemplo_: http://expedientesar.somee.com/api/Simulation/DoSession/1

**Requisitos:** El proyecto debe estar _En Sesión_.

**Lógica de Dictamen**: 50% de éxito o de fallo.

**Efectos:** Cambio de estado del proyecto _Aprobado o Rechazado en Sesión_.
# 🏁 To Do
* **Features**
    * Listar todos los proyectos de todos los legisladores en la Home
        * Busqueda y Filtros
    * Login (Legislador)
    * Mejorar Sistema de Notificación (UI Notification y servidor).
* **Tecnologías:**
    * Aplicar Entity Framework + CodeFirst + SQL server para Proyectos.
    * TestSuit para Proyectos (Servicios, Repositorios, Entidades).
    * ASP.Net Identity.
# ✉️ Contacto
¡Gracias por visitar mi proyecto! Si estás interesado en colaborar, tienes alguna duda o simplemente quieres charlar sobre desarrollo en .NET, no dudes en contactarme.

**LinkedIn**: https://www.linkedin.com/in/mauricio-manuel-chamorro/
