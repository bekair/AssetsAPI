# Asset Information Service

This project implements a simple service that provides information about different assets available on the market. The service reads information about assets from a CSV file and provides various endpoints for asset management. The assets are defined by name and ID, with each asset described by a predefined list of properties.

## Technologies

- **.NET Core 3.1** for backend development
- **C#** programming language
- **Web API** for creating the service endpoints
- **SQL Server** for database storage
- **Entity Framework Core 3.1.5** for ORM (Code-first approach)

## Database Schema

### Asset Table
- **AssetId**: Primary Key, unique identifier for each asset.
- **Name**: Name of the asset.
- **Properties**: List of predefined properties (e.g., is fix income, is convertible).
- **Timestamp**: Timestamp of when the asset record was last updated.

### Asset Properties Table
- **AssetId**: Foreign Key to the Asset table.
- **PropertyName**: The name of the property (e.g., is cash, is convertible).
- **Value**: Boolean value (True/False) for the property.
- **Timestamp**: Timestamp for when the property was updated.

### Default values
- Default value for properties is `false`.
- Default timestamp is `DateTime.Min`.

## Endpoints

### 1. Process File
This endpoint triggers the processing of a CSV file. The file should contain asset data with the following structure:

```
AssetId,Properties,Value,Timestamp
1,"is cash","true","2020-07-01T16:32:32"
3,"is convertible","false","2020-08-02T16:32:32"
...
```

- Records with a timestamp greater than the last update should be updated.
- Default property values are `false`, and the default timestamp is `DateTime.Min`.
- Duplicates in the file should be resolved by keeping the record with the latest timestamp.
- If an asset from the file is not found in the database, it should be logged.

**Optional**: The file data should be sent to the database in batches. If the batch size is set to 1000, for example, a file containing 9900 lines will create 10 batches, which can be processed in parallel.

### 2. Get Asset IDs for Property Set to Specific Value
This endpoint allows you to fetch asset IDs where a specific property is set to a given value.

**Request Example**:
```json
{
  "property": "is cash",
  "value": "true"
}
```

**Response Example**:
```json
{
  "assetIds": [1, 2, 3, 4]
}
```

### 3. Set Property for Asset
This endpoint allows you to update a property for a specific asset. The timestamp should be checked to ensure that the property is only updated if the provided timestamp is greater than the one in the database.

**Request Example**:
```json
{
  "AssetId": 1,
  "Property": "is cash",
  "Value": "true",
  "Timestamp": "2020-07-01T16:32:32"
}
```

**Response Example**:
```json
{
  "success": true
}
```

## Installation

1. Clone the repository:
```bash
git clone https://github.com/your-username/AssetInformationService.git
```

2. Install dependencies:
```bash
dotnet restore
```

3. Build the solution:
```bash
dotnet build
```

4. Run the application:
```bash
dotnet run
```

## Configuration

The application supports configurable batch sizes for processing files. This can be set in the `appsettings.json` file:

```json
{
  "BatchSize": 1000
}
```

## License

This project is licensed under the MIT License.
