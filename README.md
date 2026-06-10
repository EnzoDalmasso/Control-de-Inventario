# Control de Inventario

Aplicacion de escritorio para gestionar productos, stock e historial de movimientos.

El proyecto comenzo como una aplicacion **WinForms** y fue evolucionado para conectarse con una **API propia en ASP.NET Core**. La app de escritorio consume endpoints HTTP para crear productos, ingresar stock, descontar stock, eliminar productos y consultar historial. La API persiste la informacion en **SQLite** usando **Entity Framework Core**.

## Estado actual

- Aplicacion de escritorio en WinForms.
- API local en ASP.NET Core.
- Persistencia en SQLite mediante EF Core.
- Historial de ingresos y egresos de stock.
- Exportacion de inventario a PDF y Excel.
- Tests unitarios con xUnit para reglas de dominio.

## Demo

[Ver demo del proyecto](docs/demo-control-inventario.mp4)

## Tecnologias

- C#
- .NET 8 para WinForms
- .NET 10 para API, dominio, infraestructura y tests
- Windows Forms
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- xUnit
- ClosedXML
- iText
- Newtonsoft.Json
- Git y GitHub

## Arquitectura

```text
WinForms
  |
  | HTTP
  v
Inventory.Api
  |
  | EF Core
  v
SQLite
```

### Capas del proyecto

```text
Control de Inventario/
  Aplicacion WinForms. Es la pantalla que usa el usuario.

src/Inventory.Api/
  API en ASP.NET Core. Expone endpoints para productos y movimientos.

src/Inventory.Domain/
  Reglas principales del negocio. Por ejemplo: no descontar mas stock del disponible.

src/Inventory.Infrastructure/
  Acceso a datos con EF Core y SQLite.

tests/Inventory.Tests/
  Tests unitarios para validar reglas de dominio.
```

## Funcionalidades

- Crear productos.
- Listar productos.
- Buscar productos.
- Ingresar stock.
- Descontar stock.
- Eliminar productos.
- Consultar historial de movimientos.
- Detectar productos con bajo stock.
- Exportar reportes a PDF.
- Exportar reportes a Excel.
- Imprimir listado de inventario.

## Endpoints principales

| Metodo | Endpoint | Descripcion |
| --- | --- | --- |
| GET | `/` | Verifica que la API este funcionando |
| GET | `/api/products` | Lista productos |
| GET | `/api/products/{id}` | Obtiene un producto por id |
| POST | `/api/products` | Crea un producto |
| PUT | `/api/products/{id}` | Actualiza nombre o stock minimo |
| DELETE | `/api/products/{id}` | Elimina un producto |
| POST | `/api/products/{id}/stock/increase` | Ingresa stock |
| POST | `/api/products/{id}/stock/decrease` | Descuenta stock |
| GET | `/api/stock-movements` | Lista movimientos de stock |

## Como ejecutar el proyecto

### 1. Clonar el repositorio

```powershell
git clone https://github.com/EnzoDalmasso/Control-de-Inventario.git
cd "Control-de-Inventario"
```

Si queres probar la rama donde se esta trabajando la API:

```powershell
git checkout feature/api-backend
```

### 2. Compilar la solucion

```powershell
dotnet build
```

### 3. Ejecutar los tests

```powershell
dotnet test
```

### 4. Levantar la API

En una terminal:

```powershell
dotnet run --project .\src\Inventory.Api\Inventory.Api.csproj
```

La API queda disponible en:

```text
http://localhost:5071
```

### 5. Ejecutar WinForms

En otra terminal:

```powershell
dotnet run --project ".\Control de Inventario\Control de Inventario.csproj"
```

Importante: la API debe estar ejecutandose antes de abrir WinForms, porque la aplicacion de escritorio consulta y guarda los datos a traves de la API.

## Prueba rapida de la API

Crear un producto:

```powershell
$body = @{
  name = "Teclado"
  stock = 10
  minimumStock = 5
} | ConvertTo-Json

Invoke-RestMethod -Uri http://localhost:5071/api/products -Method Post -ContentType "application/json" -Body $body
```

Listar productos:

```powershell
Invoke-RestMethod http://localhost:5071/api/products | Format-Table id,name,stock,minimumStock
```

Descontar stock:

```powershell
$body = @{
  quantity = 2
} | ConvertTo-Json

Invoke-RestMethod -Uri http://localhost:5071/api/products/1/stock/decrease -Method Post -ContentType "application/json" -Body $body
```

Consultar historial:

```powershell
Invoke-RestMethod http://localhost:5071/api/stock-movements | Format-Table id,productName,type,quantity,createdAtUtc
```

## Reglas de negocio validadas

El proyecto incluye tests unitarios para validar reglas como:

- No permitir productos sin nombre.
- No permitir stock inicial negativo.
- No permitir ingresar cantidades invalidas.
- No permitir descontar mas stock del disponible.

## Que aprendi con este proyecto

- Separar una aplicacion de escritorio de la logica backend.
- Consumir una API desde WinForms usando HTTP.
- Crear endpoints en ASP.NET Core.
- Persistir datos con EF Core y SQLite.
- Modelar reglas de negocio en una capa de dominio.
- Agregar tests unitarios para validar comportamiento importante.
- Usar Git y ramas para versionar avances.

## Proximos pasos

- Mejorar la presentacion visual del historial.
- Agregar capturas al README.
- Publicar la API y base de datos en un entorno online.
- Preparar una demo en video para LinkedIn.
- Agregar mas tests para endpoints de API.

## Autor

**Enzo Dalmasso**

Proyecto desarrollado como parte de mi camino hacia roles de desarrollo .NET / Backend Jr.
