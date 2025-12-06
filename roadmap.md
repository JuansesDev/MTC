🗺️ Roadmap: MTC CLI (Modular Template CLI)

Este documento define la hoja de ruta para el desarrollo de MTC (Modular Template CLI), una herramienta de scaffolding enfocada en el ecosistema .NET. El objetivo es automatizar la creación de estructuras de proyectos (Architectural Patterns) y la generación de características funcionales (Feature Slices).

🤖 Contexto para Asistentes AI (Copilot/Gemini)

Si eres una IA leyendo esto, por favor adhiérete a las siguientes directrices:

Scope: El proyecto es exclusivo para .NET (C#) por ahora. No sugerir soporte para Node/Python en esta fase.

Core Philosophy: Separación estricta entre Motor del CLI (C#) y Plantillas (Archivos de texto/Scriban). No hardcodear strings de clases dentro del código C#.

UI/UX: Usar Spectre.Console para toda salida visual y System.CommandLine para el parsing de argumentos.

Architecture: El CLI debe soportar "Solution Templates" (Estructuras vacías) y "Feature Templates" (CRUDs, Auth) de forma dinámica basada en configuración (JSON Manifests).

🚀 Fase 1: El Motor (Core Engine)

Objetivo: Crear la base tecnológica que pueda leer plantillas, procesar variables y generar archivos.

[x] Inicialización del Proyecto

[x] Crear solución de consola .NET 8/9 llamada MTC.

[x] Configurar Inyección de Dependencias (Microsoft.Extensions.DependencyInjection).

[x] Integrar Spectre.Console y configurar un layout base (Header, colores).

[x] Integrar System.CommandLine para el manejo de verbos (mtc new, mtc add).

[x] Sistema de Definición de Plantillas (Blueprints)

[x] Diseñar estructura JSON para manifest.json (metadata de la plantilla).

[x] Crear el TemplateService que localice carpetas de plantillas en el disco.

[x] Implementar motor de renderizado usando Scriban o Handlebars.NET.

[x] Lógica de Scaffolding Base

[x] Implementar lógica para copiar directorios recursivamente.

[x] Implementar lógica para renombrar archivos/carpetas dinámicamente (ej: {{ProjectName}}.Domain).

[x] Implementar sustitución de contenido dentro de archivos .cs y .csproj.

🏗️ Fase 2: Solution Templates (El Arquitecto)

Objetivo: Generar soluciones completas (.sln) compilables con diferentes arquitecturas.

[x] Template: Console App (Prueba de Concepto)

[x] Plantilla simple para validar que el motor funciona.

[x] Template: MVC Monolítico

[x] Estructura clásica: Models, Views, Controllers, Services.

[x] Configuración básica de DI y Logging.

[x] Template: Clean Architecture (El estándar)

[x] Estructura de Capas: Domain, Application, Infrastructure, API.

[x] Configuración de referencias entre proyectos (Dependency Rule).

[x] Setup básico de MediatR (opcional) y Swagger.

[x] Template: Vertical Slice Architecture (Avanzado)

[x] Estructura basada en Features en lugar de Capas técnicas.

🧩 Fase 3: Feature Templates (Productivity)

Objetivo: Inyectar código en soluciones existentes. Requiere "Context Awareness".

[x] Sistema de Contexto

[x] Implementar configuración global (ej: mtc config set author "Juanse").

[x] Persistir configuración en ~/.mtc/config.json.

[x] Detectar qué arquitectura está usando el proyecto actual para saber dónde poner los archivos.

[x] Generador de CRUD Básico

[x] Parser de inputs de entidad (ej: --fields "Name:string, Age:int").

[x] Para MVC: Generar Controller + Service + Entity.

[x] Para Clean Arch: Generar Command/Query + Handler + Repository Interface + Entity.

[x] Generador de Value Objects / DTOs

[x] Comandos rápidos para generar clases repetitivas.

📦 Fase 4: Distribución y Paquetería (OS Native)

Objetivo: Hacer que MTC sea instalable nativamente (apt, winget, pacman) sin requerir .NET SDK preinstalado.

[ ] Compilación Nativa (Prerrequisito)

[x] Configurar publicación SingleFile (Un solo ejecutable autocontenido).

[x] Configurar NativeAOT (Opcional, para arranque instantáneo y menor peso).

[x] Generar binarios para Win-x64, Linux-x64 y OSX-x64.

[ ] Distribución Windows (Winget)

[ ] Crear Manifiesto YAML conforme al estándar de Microsoft Winget.

[ ] Publicar en repositorio microsoft/winget-pkgs.

[ ] Distribución Linux (Arch / AUR)

[ ] Crear script PKGBUILD para Arch Linux.

[ ] Publicar en AUR (Arch User Repository) para instalación vía yay -S mtc.

[ ] Distribución Linux (Debian/Ubuntu)

[x] Generar paquete .deb.

[x] (Opcional) Configurar PPA en Launchpad para soporte de apt install mtc.

[ ] Distribución .NET Standard

[x] Empaquetar como herramienta global NuGet (dotnet tool package) para devs que ya tienen .NET.

🛠️ Fase 5: Experiencia de Desarrollo (DX) y Calidad

Objetivo: Pulir la herramienta para uso profesional.

[ ] Configuración de Usuario

[ ] Guardar preferencias globales (ej: "Siempre usar Clean Architecture", "Autor por defecto").

[ ] Testing

[x] Unit Tests para lógica de scaffolding.

[x] Integration Tests (Build & Run de proyectos generados).r un proyecto y tratar de compilarlo programáticamente (dotnet build) para asegurar que las plantillas no están rotas.

[ ] CI/CD Pipeline

[x] GitHub Actions para Build & Test.

[x] GitHub Actions para Release (NuGet/Binaries). automáticamente a NuGet y GitHub Releases.

🔮 Fase 6: Futuro / Ideas (Backlog)

[ ] Soporte DDD: Plantillas estrictas con Aggregates Roots y Domain Events.

[ ] Docker Support: Generar Dockerfile y docker-compose automáticamente según la plantilla elegida.

[ ] Soporte Multilenguaje: Adaptar el motor para soportar estructuras de Node/Python (Post-MVP).

[ ] GUI: Interfaz interactiva con selectores (Spectre.Console.SelectionPrompt) en lugar de solo argumentos de texto.