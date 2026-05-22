#  Sistema de Control de Inventario

¡Bienvenido! Este es un sistema de escritorio desarrollado en **.NET 8 (Windows Forms)** diseñado para la gestión eficiente, almacenamiento y control de stock de productos. Cuenta con persistencia de datos local en formato JSON, alertas visuales y herramientas avanzadas de exportación de reportes.

---

##  Características Principales

* **Gestión de Productos:** Alta, baja, modificación y eliminación de artículos del inventario de forma dinámica.
* **Historial de Movimientos:** Registro detallado en tiempo real de cada ingreso o egreso de mercadería (tipo de movimiento, cantidad, fecha y hora).
* **Persistencia de Datos Automática:** Almacenamiento local mediante archivos `inventario.json` y `movimientos.json` (evitando dependencias pesadas de bases de datos para despliegues rápidos).
* **Sistema de Alertas (Bajo Stock):** Filtrado inteligente que resalta automáticamente los productos con un stock crítico (menor a 5 unidades) o sin existencias.
* **Buscador Integrado:** Filtro de búsqueda en tiempo real por nombre de producto directamente en la grilla principal.
* **Exportación de Reportes:**
    *  **Generación de PDF:** Reportes limpios utilizando la librería profesional **iText**.
    *  **Exportación a Excel:** Reportes tabulados nativos utilizando **ClosedXML**.
    *  **Impresión:** Soporte integrado para enviar listas de stock directamente a impresoras físicas o virtuales del sistema.

---

##  Tecnologías Utilizadas

* **Lenguaje:** C# (`.NET 8.0`)
* **Interfaz Gráfica:** Windows Forms (WinForms)
* **Bibliotecas Externas (NuGet):**
    * `Newtonsoft.Json` (Gestión y serialización de archivos de datos).
    * `ClosedXML` & `DocumentFormat.OpenXml` (Creación de archivos Excel).
    * `itext.kernel` & `itext.layout` (Estructuración de documentos PDF).

---

##  Arquitectura del Código

El proyecto sigue una estructura limpia y desacoplada enfocada en la programación orientada a objetos:
* `Producto.cs`: Modelo de datos para representar las propiedades de los artículos en stock.
* `Movimiento.cs`: Modelo que registra la auditoría de entradas y salidas del sistema.
* `Form1.cs`: Controlador y diseño de la pantalla principal de inventario.
* `Form2.cs`: Pantalla secundaria encargada de renderizar el DataGridView con el historial completo de transacciones.

---

##  Instalación y Ejecución

Para clonar y ejecutar este proyecto en tu entorno local, necesitás tener instalado el **SDK de .NET 8** y un IDE compatible (como Visual Studio o VS Code).

1. **Clonar el repositorio:**
   ```bash
   git clone [https://github.com/EnzoDalmasso/Control-de-Inventario.git](https://github.com/EnzoDalmasso/Control-de-Inventario.git)
