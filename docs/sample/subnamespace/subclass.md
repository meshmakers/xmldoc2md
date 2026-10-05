---

title: SubClass

---

[`< Back`](./)

---

# SubClass

Namespace: MyClassLib.SubNamespace

Sub class from [MyClass](./../myclass)

```csharp
public class SubClass : MyClass, IMyInterface
```

Inheritance [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) → [MyClass](./../myclass) → [SubClass](./subclass)<br/>
Implements [IMyInterface](./../imyinterface)

## Fields

### **myField**

My field.

```csharp
public int myField;
```

## Properties

### **MyProperty**

My property.

```csharp
public string MyProperty { get; protected set; }
```

#### Property Value

[String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br/>
The property value. Used by [MyClass.DoGeneric\<T\>(T)](./../myclass#dogenerictt).

#### Example

This example assign `"foo"` to MyProperty.

```csharp
foo.MyProperty = "foo";
```

### **MyEnum**

My enum

```csharp
public MyEnum MyEnum { get; set; }
```

#### Property Value

[MyEnum](./../myenum)<br/>
The enum value

## Constructors

### **SubClass()**

```csharp
public SubClass()
```

## Methods

### **ToString()**

Convert instance to string.

```csharp
public string ToString()
```

#### Returns

[String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br/>
A string.

## Events

### **MyEvent**

My event.

```csharp
public event EventHandler<EventArgs> MyEvent;
```

---

[`< Back`](./)
