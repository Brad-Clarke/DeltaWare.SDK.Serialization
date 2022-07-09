This package provides an easy to use high performance approach to Deserializing and Serializing CSV files whilst being highly extensible for that annoying edge case.

Current Features

 - Fully Supports the [RFC 4180](https://datatracker.ietf.org/doc/html/rfc4180) standard.
 - Serialize CSVs.
 - Serialize CSV Records.
 - Mapping Properties to Column Index or Header Name
 - Validation
 - Custom serializtion of types.
 - Nullable Types
 - Standalone Reader and Writer
 - Async and Sync operations.

Planned Features

 - Anonymous Types
 - Serialization using the CsvReader and CsvWriter.
 - Allow Serialization to Map Properties to both Header and Column Index.
 At presnet if you're using the ColumnHeaderAttribute it is not possible to specify the order using the ColumnIndexAttribute, ordering can only be done by changing the property order in .Net
 
 Get it from Nuget ```Install-Package DeltaWare.SDK.Serialization.Csv```

# Creating a Model
There are several ways to declared a model and we'll go over each of them.

### Barebones
In this scenario how the CSV data will be written or read depends on what the ```HasHeader``` parameter has been set to during deserialization or serialization.

If this parameter has been set to true, the serializer will add and or expected headers matching the property names of your model. The below model would create or expect a header like this ```Id,FirstName,LastName,BirthDate```.

If this parameter has been set to false, the serializer will match the column index to the order that the properties are declared in. As an example, Id would be column 0 and LastName would be Column 2.
```csharp
public class Student
{
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? BirthDate { get; set; }
}
```
### Using Attributes
A model can be created using several attributes. The attributes that are support are listed below.

- **ColumnHeader** (*Property*)
	Declares the Header to be used for this property.
	
- **ColumnIndex** (*Property*) - *If this property is declared it must be used on **ALL** properties.*
	Declares the Index of this property
	
- **HeaderRequired** (*Class*) 
	Enforces the use of a header, overriding the ```HasHeader``` parameter.
	
- **Required** (*Property*) 
	Validates that the CSV data contains this property.
	
- **MaxLength** (*Property*) 
Validates that the CSV data has a length that is smaller or equal to this value.

- **MinLength** (*Property*) 
Validates that the CSV data has a length that is larger or equal than this value.

- **UseTransformer** (*Property*)
Allows an object transformer to be specified. Either for custom type serialization or to change the default serialization.

## Deserializing CSVs
```csharp
CsvSerializer serializer = new CsvSerializer();

IEnumerable<Foo> myFoos = await serializer.DeserializeAsync<Foo>(csvString);
```
```csharp
CsvSerializer serializer = new CsvSerializer();

using (Stream stream = new FileStream(filePath, FileMode.Open))
{
	IEnumerable<Foo> myFoos = await serializer.DeserializeAsync<Foo>(stream);
}
```
## Serializing CSVs
```csharp
CsvSerializer serializer = new CsvSerializer();

string csvString = await serializer.SerializeAsync(myFoos)
```
```csharp
CsvSerializer serializer = new CsvSerializer();

using (Stream stream = new FileStream("~/foo.csv", FileMode.Create))
{
	await serializer.SerializeAsync(myFoos, stream);
}
```

# Deserializing and Serializing Records
A Record type CSV contains multiple different data types and no header. The following section will outline how to serialize, deserialize and create models so that you can quickly and easily handle this form of CSV.

### Example of a Record Type CSV
```
users,5,John,Doe,1970/04/24
users,172,Debra,Thompson
orders,A95A067E-A77C-43FE-BAFF-211725608AA7,2020/07/24,516
transactions,553245234,true
orders,5B24142F-B003-4CF2-81E8-E036D9A0E3A1,2021/01/15,12572.15
users,59,Grant,Burk,1985/11/15
transactions,3482739487,false
```
### Example of the users Report Type model
```csharp
[RecordType("users")]
public class UserRecord
{
	// Note in this example, the Column Index attribute isn't entirely necessary.
	// The default column index for properties is based off the order that they where declared in.
    [ColumnIndex(0)]
    public int Id { get; set; }

    [ColumnIndex(1)]
    public string FirstName { get; set; }

    [ColumnIndex(2)]
    public string LastName { get; set; }

    [ColumnIndex(3)]
    public DateTime? BirthDate { get; set; }
}
```
## Deserializing a Record
```csharp
CsvSerializer serializer = new CsvSerializer();

IEnumerable<object> myRecords = await serializer.DeserializeRecordsAsync(csvString, new[] { typeof(UserRecord), typeof(OrderRecord), typeof(TransactionRecord) });

foreach(object myRecord in myRecords)
{
	if (myRecord is UserRecord user)
	{
		// Do a thing with user.
	}
	else if (myRecord is OrderRecord order)
	{
		// Do a thing with order.
	}
	else if (myRecord is TransactionRecord transaction)
	{
		// Do a thing with transaction.
	}
}
```
Or alternatively you can use a Record Container. Below is an overview of its usage.
``` csharp
public class RecordContainer
{
    public List<UserRecord> Users { get; set; }

    public List<OrderRecord> Orders { get; set; }

    public List<TransactionRecord> Transactions { get; set; }
}
```
```csharp
CsvSerializer serializer = new CsvSerializer();

RecordContainer myRecords = await serializer.DeserializeRecordsAsync<RecordContainer>(csvString);

// Do a thing with user.
myRecords.Users;
// Do a thing with order.
myRecords.Orders;
// Do a thing with transaction.
myRecords.Transactions;
```
## Serializing a Record
Below are some examples of serializing a Record Type.
> **NOTE**: If you're using a CsvWriter as the Stream ensure to set the Mode to WriteMode.Record during instantation.
### Serializing a list of records
```csharp
List<object> myRecords;

CsvSerializer serializer = new CsvSerializer();

string csvString = await serializer.SerializeRecordsAsync(myRecords);
```
### Serializing a Record Container
```csharp
CsvSerializer serializer = new CsvSerializer();

string csvString = await serializer.SerializeRecordsAsync(myRecordContainer);
```

# CsvReader and CsvWriter
This section is not complete.