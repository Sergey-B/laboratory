using System.Collections;

namespace Task9;

public abstract class HashTableAtd<T> : IEnumerable<T>
{
    public const int PutNil = 0;
    public const int PutOk = 1;
    public const int PutErr = 2;
    public const int RemoveNil = 0;
    public const int RemoveOk = 1;
    public const int RemoveErr = 2;
    public const int GetNil = 0;
    public const int GetOk = 1;
    public const int GetErr = 2;

    // защищенные поля
    protected int _putStatus;
    protected int _removeStatus;
    protected int _getStatus;
    protected int _count;
    protected int _size;
    protected const int DefaultCapacity = 20_000;
    protected List<T>[] _slots;

    // конструктор
    // постусловие: создана пустая хэш-таблица;
    public HashTableAtd(int size = DefaultCapacity)
    {
        _size = size;
        _slots = new List<T>[_size];
        _count = 0;

        _putStatus = PutNil;
        _removeStatus = RemoveNil;
        _getStatus = GetNil;
    }

    // команды
    // предусловие: в таблице имеется свободный слот для value;
    // постусловие: в таблицу добавлено новое значение;
    public void Put(T value)
    {
        var index = GetSlotIndex(value);
        if (_slots[index] == null)
        {
            _slots[index] = [];
        }

        if (_slots[index].Contains(value))
        {
            _putStatus = PutErr;

            return;
        }

        _slots[index].Add(value);
        _count++;
        _putStatus = PutOk;
    }

    protected int GetSlotIndex(T? value)
    {
        if (value == null) return 0;

        return Math.Abs(value.GetHashCode()) % _size;
    }

    // предусловие: в таблице имеется значение value;
    // постусловие: из таблицы удалено значение value;
    public void Remove(T value)
    {
        var index = GetSlotIndex(value);
        if (_slots[index] == null)
        {
            _removeStatus = RemoveErr;

            return;
        }

        if (!_slots[index].Contains(value))
        {
            _removeStatus = RemoveErr;

            return;
        }

        _slots[index].Remove(value);
        _count--;
        _removeStatus = RemoveOk;
    }

    // запросы
    // предусловие: таблица не пустая;
    public bool Get(T value) // содержится ли значение value в таблице
    {
        var index = GetSlotIndex(value);
        if (_slots[index] == null)
        {
            _getStatus = GetOk;

            return false;
        }

        _getStatus = GetOk;

        return _slots[index].Contains(value);
    }

    public int Count() // количество элементов в таблице
    {
        return _count;
    }

    public int Size() // размер хранилища
    {
        return _size;
    }

    // запросы статусов
    public int GetPutStatus() // возвращает значение Put*.
    {
        return _putStatus;
    }
    public int GetRemoveStatus() // возвращает значение Remove*.
    {
        return _removeStatus;
    }
    public int GetGetStatus() // возвращает значение Get*.
    {
        return _getStatus;
    }


    public IEnumerator<T> GetEnumerator()
    {
        if (_count == 0)
        {
            yield break;
        }
        
        for (int i = 0; i < _size; i++)
        {
            var slot = _slots[i];
            if (slot == null || slot.Count == 0)
            {
                continue;
            }

            for (var j = 0; j < slot.Count; j++)
            {
                yield return slot[j];
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public abstract class PowerSetAtd<T> : HashTableAtd<T>
{
    public const int UnionNil = 0;
    public const int UnionOk = 1;
    public const int UnionErr = 2;
    public const int IntersectionNil = 0;
    public const int IntersectionOk = 1;
    public const int IntersectionErr = 2;
    public const int DifferenceNil = 0;
    public const int DifferenceOk = 1;
    public const int DifferenceErr = 2;
    public const int EqualsNil = 0;
    public const int EqualsOk = 1;
    public const int EqualsErr = 2;
    public const int IsSubsetNil = 0;
    public const int IsSubsetOk = 1;
    public const int IsSubsetErr = 2;

    // конструктор
    // постусловие: создано пустое множество заданного размера;
    public PowerSetAtd(int size) : base()
    {
    }

    // команды
    // предусловие: текущее множество не пустое и set2 не пустое;
    // постусловие: возвращает обьединение текущего множества с элементами из множества set2;
    public abstract PowerSetAtd<T> Union(PowerSetAtd<T> set2);

    // запросы
    // предусловие: текущее множество не пустое и set2 не пустое;
    // постусловие: возвращает пересечение текущего множества и set2;
    public abstract PowerSetAtd<T> Intersection(PowerSetAtd<T> set2);

    // предусловие: текущее множество не пустое и set2 не пустое;
    // постусловие: возвращает разницу текущего множества и set2;
    public abstract PowerSetAtd<T> Difference(PowerSetAtd<T> set2);

    // предусловие: текущее множество не пустое и set2 не пустое;
    // постусловие: возвращает true, если set2 есть подмножество текущего множества, иначе false;
    public abstract bool IsSubset(PowerSetAtd<T> set2);

    // предусловие: текущее множество не пустое и set2 не пустое;
    // постусловие: возвращает true, если set2 равно текущему множеству, иначе false;
    public abstract bool Equals(PowerSetAtd<T> set2);

    // запросы статусов
    public abstract int GetUnionStatus(); // возвращает значение Union*.
    public abstract int GetIntersectionStatus(); // возвращает значение Intersection*.
    public abstract int GetDifferenceStatus(); // возвращает значение Difference*.
    public abstract int GetEqualsStatus(); // возвращает значение Equals*.
    public abstract int GetIsSubsetStatus(); // возвращает значение IsSubset*.
}

public class PowerSet<T> : PowerSetAtd<T>
{
    private int _unionStatus;
    private int _equalsStatus;
    private int _intersectionStatus;
    private int _differenceStatus;
    private int _isSubsetStatus;

    // конструктор
    public PowerSet(int size) : base(size)
    {
        _size = size;

        _unionStatus = UnionNil;
        _equalsStatus = EqualsNil;
        _intersectionStatus = IntersectionNil;
        _isSubsetStatus = IsSubsetNil;
    }

    // запросы
    public override PowerSet<T> Union(PowerSetAtd<T> set2)
    {
        if (Count() == 0 && set2.Count() == 0)
        {
            return new PowerSet<T>(0);
        }

        var sourceSet = Count() >= set2.Count() ? this : set2;
        var targetSet = Count() >= set2.Count() ? set2 : this;
        var result = new PowerSet<T>(sourceSet.Count() + Count());

        foreach (var item in sourceSet)
        {
            result.Put(item);
        }

        foreach (var item in targetSet)
        {
            if (!result.Get(item))
            {
               result.Put(item);
            }
        }

        _unionStatus = UnionOk;

        return result;
    }

    public override PowerSet<T> Difference(PowerSetAtd<T> set2)
    {
        var result = new PowerSet<T>(Count());

        if (set2.Count() == 0)
        {
            _differenceStatus = DifferenceOk;

            foreach (var item in this) result.Put(item);

            return result;
        }

        foreach (var item in this)
        {
            if (!set2.Get(item))
            {
                result.Put(item);
            }
        }

        _differenceStatus = DifferenceOk;

        return result;
    }

    public override bool Equals(PowerSetAtd<T> set2)
    {
        if (Count() != set2.Count())
        {
            _equalsStatus = EqualsErr;

            return false;
        }

        _equalsStatus = EqualsOk;
        foreach (var item in this)
        {
            if (!set2.Get(item))
            {
                return false;
            }
        }

        return true;
    }

    public override PowerSet<T> Intersection(PowerSetAtd<T> set2)
    {
        if (Count().Equals(0) || set2.Count().Equals(0))
        {
            _intersectionStatus = IntersectionOk;

            return new PowerSet<T>(0);
        }

        var sourceSet = Count() >= set2.Count() ? this : set2;
        var targetSet = Count() >= set2.Count() ? set2 : this;
        var result = new PowerSet<T>(sourceSet.Count());

        foreach(var item in sourceSet)
        {
            if (targetSet.Get(item))
            {
                result.Put(item);
            }
        }
        
        _intersectionStatus = IntersectionOk;

        return result;
    }

    public override bool IsSubset(PowerSetAtd<T> set2)
    {
        _isSubsetStatus = IsSubsetOk;

        if (Count().Equals(0))
        {
            return true;
        }

        if (Count() > set2.Count())
        {
            return false;
        }

        foreach (var item in this)
        {
            if (!set2.Get(item))
            {
                return false;
            }
        }

        return true;
    }

    // запросы статусов
    public override int GetUnionStatus()
    {
        return _unionStatus;
    }

    public override int GetEqualsStatus()
    {
        return _equalsStatus;
    }

    public override int GetIntersectionStatus()
    {
        return _intersectionStatus;
    }

    public override int GetDifferenceStatus()
    {
        return _differenceStatus;
    }

    public override int GetIsSubsetStatus()
    {
        return _isSubsetStatus;
    }
}