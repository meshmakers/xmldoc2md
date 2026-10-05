---

title: MyClass

---

[`< Back`](./)

---

# MyClass

Namespace: MyClassLib

My class.

```csharp
public class MyClass : IMyInterface
```

Inheritance [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) → [MyClass](./myclass)<br/>
Implements [IMyInterface](./imyinterface)

**Remarks:**

A remark.

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
The property value. Used by [MyClass.DoGeneric\<T\>(T)](./myclass#dogenerictt).

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

[MyEnum](./myenum)<br/>
The enum value

## Constructors

### **MyClass()**

Initializes a new instance of the [MyClass](./myclass) class.

```csharp
public MyClass()
```

**Remarks:**

See also [MyClass.MyClass(String, Int32)](./myclass#myclassstring-int32).

```csharp
if (true)
{
    var foo = new MyClass("foo", 1);
    Console.WriteLine(foo.ToString());
}
```

### **MyClass(String, Int32)**

Initializes a new instance of the [MyClass](./myclass) class.

```csharp
public MyClass(string firstParam, int secondParam)
```

#### Parameters

`firstParam` [String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br/>
The first param.

`secondParam` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br/>
The second param.

## Methods

### **Do(String, Int32)**

Do some thing.

```csharp
public void Do(string firstParam, int secondParam)
```

#### Parameters

`firstParam` [String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br/>
The first param.

`secondParam` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br/>
The second param.

#### Exceptions

[Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception)<br/>
Thrown when...

### **DoGeneric\<T\>(T)**

Do some thing.

```csharp
public int DoGeneric<T>(T value)
```

#### Type Parameters

`T`<br/>
The type argument. Used by [MyClass.DoGeneric\<T\>(T)](./myclass#dogenerictt).

#### Parameters

`value` T<br/>
The param. Used by [MyClass.DoGeneric\<T\>(T)](./myclass#dogenerictt).

#### Returns

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br/>
Returns a value [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32).

#### Exceptions

[ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception)<br/>
Thrown instead of [Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception).

### **Get(List\<String\>)**

Gets some thing.

```csharp
public string Get(List<string> param)
```

#### Parameters

`param` [List\<String\>](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)<br/>
The param.

#### Returns

[String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br/>
An empty string.

#### Exceptions

[Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception)<br/>
Thrown when...

#### Example

This example call the `Get` method.

```csharp
var bar = foo.Get("bar");
```

### **StaticMethod()**

A static method.

```csharp
public static void StaticMethod()
```

### **MarkupLikeText\<TItem\>(Int32)**

Text with characters that are markup in Markdown or MDX but plain text in XML doc:
 the CLR arity name List\`1, the generic IList&lt;T&gt;, a JSX-like &lt;T&gt; and braces \{x\}.

```csharp
public TItem[] MarkupLikeText<TItem>(int count)
```

#### Type Parameters

`TItem`<br/>
The item type.

#### Parameters

`count` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br/>
How many items.

#### Returns

TItem[]<br/>
Nothing useful.

**Remarks:**

Inline code may contain a backtick ``List`1``, start with one `` `quoted` ``,
 use braces `{ "a": 1 }` or chevrons `IReadOnlyList<T>`.
 References: `count` and `TItem`.

## Events

### **MyEvent**

My event.

```csharp
public event EventHandler<EventArgs> MyEvent;
```

## Example

```csharp
var foo = new MyClass("one", 2);

foo.Do("one", 2);
```

---

[`< Back`](./)
