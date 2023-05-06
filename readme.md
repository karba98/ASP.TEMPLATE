##
   <h1 align="center">
    <img width="10%" src="https://user-images.githubusercontent.com/49042638/236642407-f4311613-fa15-44b6-8c11-b6ad6ca3bd82.png" align="left">
   </h1>
   
# 🌐 ASP .NET CORE MVC (Web + API)

Este proyecto consiste en una aplicación web desarrollada en ASP .NET Core MVC que se conecta a una API de datos llamada DEDOMENA para mostrar al usuario ofertas de empleo, cursos y artículos de un nicho concreto. DEDOMENA se encarga de almacenar todas las ofertas de empleo en una base de datos MySQL y obtener noticias y artículos de distintas fuentes RSS. Además, automáticamente actualiza las ofertas de empleo en base a fuentes RSS como Infojobs, Infoempleo, Jooble, entre otros.

## Características de la parte cliente

La parte cliente de esta aplicación web cuenta con las siguientes secciones:

- **Noticias**: Sección donde se muestran las últimas noticias relacionadas con el nicho.
- **Empleo**: Sección donde se muestran las ofertas de empleo más recientes relacionadas con el nicho.
- **Cursos**: Sección donde se muestran los cursos de formación y actualización para profesionales del sector del nicho.
- **Artículos**: Sección donde se muestran artículos de opinión, análisis y consejos sobre el nicho.

## Arquitectura del proyecto

La arquitectura del proyecto se compone de las siguientes partes:

- **Parte Cliente**: La aplicación web desarrollada en ASP .NET Core MVC.
- **API de datos (DEDOMENA)**: API que se encarga de acceder a los datos de ofertas de empleo, cursos, licitaciones y demás datos extraídos de bases de datos y fuentes RSS.
- **API de notificaciones (ICARO)**: API que se encarga de enviar notificaciones a redes sociales concretas como Facebook, Twitter o LinkedIn.

## Configuración del proyecto

Para configurar y ejecutar el proyecto se deben seguir los siguientes pasos:

1. Crear la base de datos ejecutando en MySQL el DDL de estructura y después insertar algunos datos de prueba usando el DML.
2. Abrir el proyecto en Visual Studio 2019 o superior.
3. Instalar las dependencias en cada una de las 3 soluciones.
4. Añadir en appsettings.json de DEDOMENA e ICARO una SecretKey aleatoria (caracteres aleatorios)
5. Ejecutar en este orden: ICARO, DEDOMENA y WEB.

## Licencia

Este proyecto está bajo la licencia MIT. Ver el archivo [LICENSE](LICENSE) para más detalles.

## Contacto

Puedes contactarnos a través de nuestras redes sociales:

- 📱 Twitter: [@raulkarba](https://twitter.com/raulkarba)
- 💼 LinkedIn: [Raúl Castro](https://www.linkedin.com/in/ra%C3%BAl-castro-de-la-torre-861508103/)

## Licencia

Este proyecto está bajo la licencia MIT. Ver el archivo [LICENSE](LICENSE) para más detalles.
