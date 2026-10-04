
using Xunit.Abstractions;

namespace Task4;

public abstract class DynArrayAtd<T>
{
    // константы
    public const int AddNil = 0;
    public const int AddOk = 1;
    public const int AddErr = 2;
    public const int GetNil = 0;
    public const int GetOk = 1;
    public const int GetErr = 2;
    public const int InsertNil = 0;
    public const int InsertOk = 1;
    public const int InsertErr = 2;
    public const int MakeArrayNil = 0;
    public const int MakeArrayOk = 1;
    public const int MakeArrayErr = 2;
    public const int RemoveNil = 0;
    public const int RemoveOk = 1;
    public const int RemoveErr = 2;
    public const int FindNil = 0;
    public const int FindOk = 1;
    public const int FindErr = 2;

    // защищенные поля
    protected int count;
    protected int capacity;
    protected T[] array;
    public const int DefaultCapacity = 4;

    // конструктор
    // постусловие: создан новый пустой динамический массив
    public DynArrayAtd(int capacity = DefaultCapacity)
    {
        count = 0;
        array = new T[capacity];
    }

    // команды
    // постусловие: в конце массива добавлен новый элемент
    public abstract void Add(T item);

    // предусловие: значение индекса больше или равно нулю
    // постусловие: в массив добавлен новый элемент по указанному индексу
    public abstract void Insert(T item, int index);

    // предусловие: массив не пустой
    // постусловие: из массива удален элемент по указанному индексу
    public abstract void Remove(int index);

    // предусловие: параметр капасити больше текущего размера массива
    // постусловие: размер массива изменен, с сохранением элементов массива
    public abstract void MakeArray(int capacity);

    // запросы
    // предусловие: массив не пустой, значение индекса больше или равно нулю и не выходит за границы массива
    // постусловие: возвращен элемент по указанному индексу
    public abstract T? Get(int index);

    // постусловие: возвращено количество элементвов в массиве
    public abstract int Size();

    // постусловие: возвращено значение capacity массива
    public abstract int Length();

    // запросы статусов
    public abstract int GetAddStatus(); // возвращает значение Add*
    public abstract int GetInsertStatus(); // возвращает значение Insert*
    public abstract int GetRemoveStatus(); // возвращает значение Remove*
    public abstract int GetMakeArrayStatus(); // возвращает значение MakeArray*
    public abstract int GetGetStatus(); // возвращает значение Get*
}

public class DynArray<T> : DynArrayAtd<T>
{

    // скрытые поля
    private int _addStatus;
    private int _getStatus;
    private int _insertStatus;
    private int _makeArrayStatus;
    private int _removeStatus;
    private int _size;

    public DynArray() : base(DefaultCapacity)
    {
        _addStatus = AddNil;
        _getStatus = GetNil;
        _insertStatus = InsertNil;
        _makeArrayStatus = MakeArrayNil;
        _removeStatus = RemoveNil;
        _size = 0;
        
        capacity = DefaultCapacity;
    }

    public override void Add(T item)
    {
        var index = Size();

        // буффер переполнен
        if (index > Length() - 1 || Size() == Length())
        {
            var newCapacity = capacity * 2;
            MakeArray(newCapacity);
        }

        array[index] = item; // O(1)
        _size += 1;

        _addStatus = AddOk;
    }

    public override T? Get(int index)
    {
        if (Size() == 0 || index > Size() - 1 || index < 0)
        {
            _getStatus = GetErr;

            return default;
        }

        var item = array[index]; // O(1)
        _getStatus = GetOk;

        return item;
    }

    public override void Insert(T item, int index)
    {
        if (index < 0)
        {
            _insertStatus = InsertErr;

            return;
        }

        var newSize = Size() + 1;

        // превышен размер буффера или индекс за пределами буффера
        if (index > Length() - 1 || newSize > Length())
        {
            var newCapacity = index > capacity ? index * 2 : capacity * 2;
            MakeArray(newCapacity);

            // элементы сдвигаются вправо
            for (var i = index; i <= newSize - 1; i++)
            {
                array[i + 1] = array[i];
            }
        }

        array[index] = item;

        _size += 1;
        _insertStatus = InsertOk;

        return;
    }

    public override void MakeArray(int newCapacity)
    {
        if (newCapacity <= 0 || newCapacity < Size())
        {
            _makeArrayStatus = MakeArrayErr;
            return;
        }

        var buffer = array;
        array = new T[newCapacity];
        Array.Copy(buffer, array, Size());
        capacity = newCapacity;

        _makeArrayStatus = MakeArrayOk;
    }

    public override void Remove(int index)
    {
        if (index < 0 || index > Length() - 1)
        {
            _removeStatus = RemoveErr;

            return;
        }

        var newSize = Size() - 1;
        
        // превышен размер буффера или индекс за пределами буффера
        if (newSize <= (Length() / 2) && (Length() / 2) >= DefaultCapacity)
        {
            var newCapacity = capacity / 2;

            MakeArray(newCapacity);
        }

        for (var i = index + 1; i <= Size() - 1; i++)
        {
            array[i - 1] = array[i];
        }

        _size -= 1;
        _removeStatus = RemoveOk;
    }

    public override int Size()
    {
        return _size;
    }

    public override int Length()
    {
        return capacity;
    }

    public override int GetAddStatus()
    {
        return _addStatus;
    }

    public override int GetGetStatus()
    {
        return _getStatus;
    }

    public override int GetInsertStatus()
    {
        return _insertStatus;
    }

    public override int GetMakeArrayStatus()
    {
        return _makeArrayStatus;
    }

    public override int GetRemoveStatus()
    {
        return _removeStatus;
    }
}