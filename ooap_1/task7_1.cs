using System.Security.Authentication.ExtendedProtection;

namespace Task7;

public abstract class HashTableAtd
{
    public const int AddNil = 0;
    public const int AddOk = 1;
    public const int AddErr = 2;
    public const int RemoveNil = 0;
    public const int RemoveOk = 1;
    public const int RemoveErr = 2;
    public const int ExistsNil = 0;
    public const int ExistsOk = 1;
    public const int ExistsErr = 2;
    public const int GetNil = 0;
    public const int GetOk = 1;
    public const int GetErr = 2;

    // конструктор
    // постусловие: создана новое пустое хранилище.
    public HashTableAtd(int size)
    {
    }

    // команды
    // предусловие: хранилище не заполнено, элемента с таким значением нет в хранилище.
    // постусловие: добавляет новое значение.
    public abstract void Add(string key, string value);

    // предусловие: хранилище не пустое, элемента с таким значением нет в хранилище.
    // постусловие: значение удалено.
    public abstract void Remove(string key);

    // запросы
    // предусловие: хранилище не пустое.
    // постусловие: вернулось булево значение, указывающее имеется ли в хранилище указанное значение.
    public abstract bool Exists(string value);

    // предусловие: хранилище не пустое. 
    // постусловие: возвращает значение.
    public abstract string? Get(string key);

    // постусловие:  возвращает текущее количество элементво в хранилище.
    public abstract int Count();

    // постусловие:  возвращает размер хранилища.
    public abstract int Size();

    // запросы статусов
    public abstract int GetAddStatus(); // возвращает значение Add*.
    public abstract int GetRemoveStatus(); // возвращает значение Remove*.
    public abstract int GetExistsStatus(); // возвращает значение Exists*.
    public abstract int GetGetStatus(); // возвращает значение Get*.
}

public class HashTable : HashTableAtd
{
    private List<(string Key, string Value)>?[] _slots;
    private int _count;
    private int _size;
    private int _addStatus;
    private int _removeStatus;
    private int _existsStatus;
    private int _getStatus;

    // скрытые поля используемы для вычисления хэш-кода
    private readonly int _p;
    private readonly int _a;
    private readonly int _b;

    public HashTable(int size) : base(size)
    {
        _size = size;
        _slots = new List<(string Key, string Value)>?[_size]; 
        _count = 0;

        _addStatus = AddNil;
        _removeStatus = RemoveNil;
        _existsStatus = ExistsNil;
        _getStatus = GetNil;

        // инициализация переменных для вычисления хэш-кода
        _p = 31;
        Random rand = new Random();

        _a = rand.Next(1, _p);
        _b = rand.Next(0, _p);
    }

    public override void Add(string key, string value)
    {
        // вернуть ошибку если хранилище заполнено
        if (Count() == _size)
        {
            _addStatus = AddErr;

            return;
        }

        var index = GetHashFun(key);

        // инициализировать если нет цепочки по данному индексу
        if (_slots[index] == null)
        {
            _slots[index] = new List<(string key, string value)>();
        }

        var chain = _slots[index]!;

        // найти и перезаписать значение ключа
        for (var i = 0; i < chain.Count; i++)
        {
            if (!chain[i].Key.Equals(key))
            {
                continue;
            }

            var tuple = chain[i];
            tuple.Value = value;
            chain[i] = tuple;
        }

        // добавить новый элемент в цепочку
        chain.Add((key, value));

        _count++;
        _addStatus = AddOk;

        return;
    }


    public override void Remove(string key)
    {
        // вернуть ошибку если хранилище пустое
        if (Count() == 0)
        {
            _removeStatus = RemoveErr;

            return;
        }

        var index = GetHashFun(key);

        var chain = _slots[index];
        if (chain is null)
        {
            _removeStatus = RemoveErr;
            return;
        }

        // удалить все элементы с ключом равным key
        for(var i = 0; i < chain.Count; i++)
        {
            if (chain[i].Key.Equals(key))
            {
                chain.RemoveAt(i);
                _count--;
            }
        }

        _removeStatus = RemoveOk;
        return;
    }
    public override bool Exists(string key)
    {
        if (Count() == 0)
        {
            _existsStatus = ExistsErr;

            return default;
        }

        var item = Get(key);
        if (GetGetStatus() != GetOk)
        {
            _existsStatus = ExistsErr;

            return false;
        }

        _existsStatus = ExistsOk;

        if (item is null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public override string? Get(string key)
    {
        // если хранилище пустое то вернуть ошибку
        if (Count() == 0)
        {
            _getStatus = GetErr;

            return default;
        }

        var index = GetHashFun(key);
        var chain = _slots[index];

        // если цепочки по заданному индексу нет то вернуть ошибку
        if (chain is null)
        {
            _getStatus = GetErr;

            return default;
        }

        // найти элемент или вернут дефолтное значение
        string? value = default;
        for(var i = 0;i < chain.Count; i++)
        {
            if (!chain[i].Key.Equals(key)) continue;

            value = chain[i].Value;
            break;
        }

        _getStatus = GetOk;

        return value;
    }

    public override int GetAddStatus()
    {
        return _addStatus;
    }

    public override int GetRemoveStatus()
    {
        return _removeStatus;
    }

    public override int GetExistsStatus()
    {
        return _existsStatus;
    }

    public override int GetGetStatus()
    {
        return _getStatus;
    }

    public override int Count()
    {
        return _count;
    }

    public override int Size()
    {
        return _size;
    }

    private int GetHashFun(string value)
    {
        var hash = 0;
        foreach (char c in value)
        {
            hash = ((hash * _a) + c) % _p;
        }

        hash = (hash + _b) % _p;

        return hash % _size;
    }
}
