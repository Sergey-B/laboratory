using System;
using System.Reflection;
using System.Text.Json;

// Самый верхний класс General содержит набор операций и закрыт для изменений
abstract class General : Object
{
    // -- копирование объекта;
    public T? Copy<T>(T targetObject) where T : General
    {
        if (targetObject is null)
            return default;
        if (targetObject.GetInstanceType() != GetInstanceType())
            throw new InvalidOperationException("Wrong type to copy current object.");

        var clone = Clone<T>();
        var currentType = GetInstanceType();

        while(currentType != null && currentType != typeof(object))
        {
            var fields = currentType.GetFields(
                BindingFlags.Instance | 
                BindingFlags.Public | 
                BindingFlags.NonPublic | 
                BindingFlags.DeclaredOnly);

            foreach(var field in fields)
            {
                field.SetValue(targetObject, field.GetValue(clone));
            }
            currentType = currentType.BaseType;
        }

        return targetObject;
    }

    // -- клонирование объекта;
    public T Clone<T>() where T : General
    {
        var instanceType = GetInstanceType();

        var serialized = JsonSerializer.Serialize(this, instanceType);

        return (T)JsonSerializer.Deserialize(serialized, instanceType);
    }

    // -- сравнение объектов;
    public bool Compare<T>(T sourceObject) where T : General
    {
        if (sourceObject is null)
            return false;
        if (sourceObject.GetType() != GetInstanceType())
            return false;
        if (ReferenceEquals(this, sourceObject))
            return true;

        return Serialize() == sourceObject.Serialize();
    }

    // -- сериализация;
    public string Serialize()
    {
        return JsonSerializer.Serialize(this, GetInstanceType());
    }

    // -- десериализация;
    public static T Deserialize<T>(string sourceString) where T : General
    {
        return JsonSerializer.Deserialize<T>(sourceString);
    }

    // -- печать;
    public void Print()
    {
        var serialized = Serialize();

        Console.WriteLine(serialized);
    }

    // -- проверка типа;
    public bool IsType(Type typeToCheck)
    {
        return typeToCheck.IsInstanceOfType(this);
    }

    // -- получение реального типа объекта.
    public Type GetInstanceType()
    {
        return GetType();
    }
}

// наследник открытый для изменения
class Any : General
{
}