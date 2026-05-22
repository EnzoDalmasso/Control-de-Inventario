# Inventory Management System

Welcome! This is a desktop application developed in **.NET 8 (Windows Forms)** designed for efficient product management, storage, and stock control. It features local data persistence in JSON format, visual alerts, and advanced report export tools.

---

## Key Features

* **Product Management:** Dynamically add, update, remove, and view inventory items.
* **Transaction History:** Real-time, detailed tracking of every stock entry or exit (type of movement, quantity, date, and time).
* **Automatic Data Persistence:** Local storage utilizing `inventario.json` and `movimientos.json` files (eliminating heavy database dependencies for rapid deployment).
* **Low-Stock Alert System:** Intelligent filtering that automatically highlights products with critical stock levels (less than 5 units) or out-of-stock items.
* **Integrated Search Engine:** Real-time search filter by product name directly within the main grid.
* **Report Exporting:**
    * **PDF Generation:** Clean, professional reports utilizing the **iText** library.
    * **Excel Exporting:** Native tabulated spreadsheets using **ClosedXML**.
    * **Printing Support:** Built-in capability to send stock lists directly to physical or virtual system printers.

---

## Technologies Used

* **Language:** C# (`.NET 8.0`)
* **Graphical User Interface:** Windows Forms (WinForms)
* **External Libraries (NuGet):**
    * `Newtonsoft.Json` (Data file management and serialization).
    * `ClosedXML` & `DocumentFormat.OpenXml` (Excel file creation).
    * `itext.kernel` & `itext.layout` (PDF document structuring).

---

## Code Architecture

The project follows a clean, decoupled structure focused on object-oriented programming:
* `Producto.cs`: Data model representing the properties of items in stock.
* `Movimiento.cs`: Audit model that logs all system entries and exits.
* `Form1.cs`: Controller and UI design for the main inventory screen.
* `Form2.cs`: Secondary screen dedicated to rendering the DataGridView with the complete transaction history.

---

## Installation & Execution

To clone and run this project locally, you will need the **.NET 8 SDK** and a compatible IDE installed (such as Visual Studio or VS Code).

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/EnzoDalmasso/Control-de-Inventario.git](https://github.com/EnzoDalmasso/Control-de-Inventario.git)
