# BPSeguros

## Configuración de la Conexión a la Base de Datos

- Cada microservicio debe configurar su conexión a la base de datos de forma independiente.
- Para esto, se utiliza la variable de entorno **`SQL_SERVER_CONNECTION_STRING`** ubicada en el archivo `launchsettings.json`.
- **Recomendación:** conectar el motor de base de datos a **localhost** para asegurar la correcta ejecución del proyecto en desarrollo.

---

## Base de Datos y Arquitectura

- El proyecto fue creado con la metodología **First Code**.
- Incluye mecanismos automáticos para:
  - Creación automática de bases de datos.
  - Carga inicial de catálogos (solo una vez).
- Las bases de datos que se crean son:
  - **SeguridadDb**
  - **PolizaDb**
  - **PersonaDb**
- Esto respeta el principio de microservicios:  
  **cada microservicio debe tener su propia base de datos independiente.**

---

## ¿Cómo ejecutar el proyecto?

### Frontend

- El frontend está desarrollado en **React**.
- Pasos para ejecutar:
  1. Tener instalado **Node.js**.
  2. Instalar dependencias con:
     ```bash
     npm install
     ```
  3. Ejecutar el servidor de desarrollo con:
     ```bash
     npm run dev
     ```
  4. En caso de cambiar puertos:
     - Modificar las variables dentro de la carpeta `constants` en el proyecto web.
     - Cambiar las variables de entorno de cada microservicio.

---

### Backend

- El backend consta de **3 proyectos Web API** basados en microservicios.
- Cada microservicio tiene su propia base de datos independiente.
- Para ejecutar:
  - Desde **Visual Studio**, abrir el proyecto y ejecutar.
  - Desde la consola, en la ruta del proyecto, ejecutar:
    ```bash
    dotnet run
    ```
- **Importante:** Tener instalado el SDK de **.NET** para la ejecución correcta.

---

Con esta configuración y pasos se garantiza la correcta ejecución y despliegue del proyecto BPSeguros, respetando las buenas prácticas de arquitectura basada en microservicios.
