# Unit Conversion API

## Overview
The Unit Conversion API is a production-quality ASP.NET Core 8 RESTful Web API designed to convert numerical values between various units of measurement. It supports standard measurement categories such as length, temperature, weight, area, volume, and speed. Built with clean architecture principles, this service is fully tested, robust, and optimized for local execution.

## Features
- **Supported Categories & Units**:
  - **Length**: meter, kilometer, centimeter, mile, foot, inch, yard.
  - **Temperature**: celsius, fahrenheit, kelvin.
  - **Weight**: kilogram, gram, pound, ounce, tonne.
  - **Area**: squaremeter, squarekilometer, squarefeet, squaremile, hectare, acre.
  - **Volume**: liter, milliliter, gallon, quart, pint, cup, cubicmeter.
  - **Speed**: meterspersecond, kilometersperhour, milesperhour, knot.
- **Key Design Choices**:
  - Base-unit design pattern for clean, linear factor conversion.
  - Custom delegate conversion functions for non-linear units like temperature.
  - Normalized case-insensitive lookup of unit identifiers.
  - Global Exception Handling Middleware mapping custom conversion exceptions to 400 Bad Request.
  - Complete integration and unit test coverage.

## Technology Stack
- .NET 8
- ASP.NET Core Web API
- Swagger/OpenAPI
- xUnit
- Dependency Injection
- Middleware-based Exception Handling

## Getting Started
- dotnet build
Expected:
- Build succeeded

1. **Clone the repository**:
   ```bash
   git clone <repository-url>
   cd UnitConversionApi
   ```
2. **Restore dependencies**:
   ```bash
   dotnet restore
   ```
3. **Run the API**:
   ```bash
   dotnet run --project src/UnitConversionApi
   ```
4. **Access Swagger UI**:
   Open [http://localhost:5000/swagger](http://localhost:5000/swagger) in your browser.

## Running Tests
Run the entire xUnit unit and integration test suite using:
```bash
dotnet test
```

## API Reference

| Endpoint | Method | Description |
| :--- | :--- | :--- |
| `/api/conversions` | `POST` | Converts a numerical value from one unit to another within the same category. |
| `/api/conversions/units` | `GET` | Returns all supported measurement categories and units. |

### Request and Response Examples

#### POST `/api/conversions`

##### Request Body
```json
{
  "value": 100,
  "fromUnit": "celsius",
  "toUnit": "fahrenheit"
}
```

##### Success Response (200 OK)
```json
{
  "inputValue": 100,
  "fromUnit": "celsius",
  "toUnit": "fahrenheit",
  "result": 212,
  "category": "temperature"
}
```

##### Validation Error Response (400 Bad Request)
```json
{
  "error": "Unsupported unit: 'xyz'",
  "supportedUnits": {
    "length": ["meter", "kilometer", "mile", "foot", "inch", "centimeter", "yard"],
    "temperature": ["celsius", "fahrenheit", "kelvin"],
    "weight": ["kilogram", "gram", "pound", "ounce", "tonne"],
    "area": ["squaremeter", "squarekilometer", "squarefeet", "squaremile", "hectare", "acre"],
    "volume": ["liter", "milliliter", "gallon", "quart", "pint", "cup", "cubicmeter"],
    "speed": ["meterspersecond", "kilometersperhour", "milesperhour", "knot"]
  }
}
```

---

#### GET `/api/conversions/units`

##### Response (200 OK)
```json
{
  "length": ["meter", "kilometer", "mile", "foot", "inch", "centimeter", "yard"],
  "temperature": ["celsius", "fahrenheit", "kelvin"],
  "weight": ["kilogram", "gram", "pound", "ounce", "tonne"],
  "area": ["squaremeter", "squarekilometer", "squarefeet", "squaremile", "hectare", "acre"],
  "volume": ["liter", "milliliter", "gallon", "quart", "pint", "cup", "cubicmeter"],
  "speed": ["meterspersecond", "kilometersperhour", "milesperhour", "knot"]
}
```

## Design Decisions

- **Base-Unit Approach**:
  To prevent an \(N \times N\) conversion matrix explosion, every linear category designates a single "base unit" (e.g., `meter` for length, `kilogram` for weight). Each unit registers a conversion factor defining how it converts relative to this base. Conversions are calculated by first mapping input values to the base unit, and then dividing by the target unit's base factor:
  \[\text{resultInBase} = \text{value} \times \text{fromUnit.ToBaseFactor}\]
  \[\text{result} = \frac{\text{resultInBase}}{\text{toUnit.ToBaseFactor}}\]
- **Custom Delegates for Temperature**:
  Temperature scales (Celsius, Fahrenheit, Kelvin) are non-linear because they require addition/subtraction offsets, not just multiplication factors. To accommodate this, the registry maps custom conversion delegate lambdas for these units. If the registry detects a temperature category conversion, it executes these delegates in a chain (`FromUnit` -> `Celsius` -> `ToUnit`) instead of factor multiplication.
- **Extensibility with `IUnitRegistry`**:
  The database of units is separated into `UnitRegistry` implementing `IUnitRegistry` registered as a Singleton. This decouples service layer calculation logic from the configuration of supportable units. Adding a new category or unit is as simple as registering it in `UnitRegistry` without modifying `ConversionService` code.
- **Global Exception Middleware Pattern**:
  A global exception middleware wraps the HTTP request pipeline, separating domain-specific logic from error delivery. Catching `UnsupportedConversionException` transforms domain exceptions into a standardized HTTP `400 Bad Request` payload carrying the exception error and the listing of available supported units.

## Project Structure

```
UnitConversionApi/
├── src/
│   └── UnitConversionApi/       # Web API source code
│       ├── Controllers/         # API Controllers
│       ├── Models/              # Request and Response models
│       ├── Services/            # Business logic and conversion service
│       ├── Data/                # Unit Registry definitions and interfaces
│       ├── Exceptions/          # Custom exceptions
│       ├── Middleware/          # Global exception handling middleware
│       └── Program.cs           # Application bootstrap and DI configuration
├── tests/
│   └── UnitConversionApi.Tests/ # xUnit test suite (unit and integration tests)
├── UnitConversionApi.sln        # Solution file
├── .gitignore                   # Git ignore file
├── .editorconfig                # .NET coding style conventions
└── README.md                    # Project documentation
```
## Assumptions and Trade-offs

- Units are stored in-memory and hardcoded for this assessment.
- No database is required.
- Authentication is intentionally omitted because it is not part of the assignment.
- The registry design allows future migration to database-backed unit configuration.