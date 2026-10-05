---

title: GenericClass<T>

---

[`< Back`](./)

---

# GenericClass\<T\>

Namespace: MyClassLib.SubNamespace

Generic class.

```csharp
public class GenericClass<T>
```

#### Type Parameters

`T`<br/>
The type param.

Inheritance [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) → [GenericClass\<T\>](./genericclass-1)

## Constructors

### **GenericClass()**

Initializes a new instance of the [GenericClass\<T\>](./genericclass-1) class.

```csharp
public GenericClass()
```

### **GenericClass(T)**

Initializes a new instance of the [GenericClass\<T\>](./genericclass-1) class.

```csharp
public GenericClass(T param)
```

#### Parameters

`param` T<br/>
The generic parameter.

## Methods

### **GetGenericInstance\<TSource\>()**

Gets a new instance of generic param.

```csharp
public TSource GetGenericInstance<TSource>()
```

#### Type Parameters

`TSource`<br/>
The generic param.

#### Returns

TSource<br/>
The new instance.

### **GetGenericInstance\<TSource\>(TSource)**

Gets a new instance of generic param.

```csharp
public TSource GetGenericInstance<TSource>(TSource source)
```

#### Type Parameters

`TSource`<br/>
The generic param.

#### Parameters

`source` TSource<br/>
The object source.

#### Returns

TSource<br/>
The new instance.

### **Map\<TSource, TTarget\>(TSource, TTarget)**

Map object.

```csharp
public TTarget Map<TSource, TTarget>(TSource source, TTarget target)
```

#### Type Parameters

`TSource`<br/>
The source type.

`TTarget`<br/>
The source target.

#### Parameters

`source` TSource<br/>
The object source.

`target` TTarget<br/>
The target

#### Returns

TTarget<br/>
The mapped object.

---

[`< Back`](./)
