namespace Task3;

public abstract class ParentListAtd<T>
{
    public const int HeadNil = 0;
    public const int HeadOk = 1;
    public const int HeadErr = 2;
    public const int TailNil = 0;
    public const int TailOk = 1;
    public const int TailErr = 2;
    public const int RightNil = 0;
    public const int RightOk = 1;
    public const int RightErr = 2;
    public const int PutRightNil = 0;
    public const int PutRightOk = 1;
    public const int PutRightErr = 2;
    public const int PutLeftNil = 0;
    public const int PutLeftOk = 1;
    public const int PutLeftErr = 2;
    public const int AddToEmptyNil = 0;
    public const int AddToEmptyOk = 1;
    public const int AddToEmptyErr = 2;
    public const int AddToTailNil = 0;
    public const int AddToTailOk = 1;
    public const int AddToTailErr = 2;
    public const int RemoveNil = 0;
    public const int RemoveOk = 1;
    public const int RemoveErr = 2;
    public const int RemoveAllNil = 0;
    public const int RemoveAllOk = 1;
    public const int RemoveAllErr = 2;
    public const int ReplaceNil = 0;
    public const int ReplaceOk = 1;
    public const int ReplaceErr = 2;
    public const int FindNil = 0;
    public const int FindOk = 1;
    public const int FindErr = 2;
    public const int GetNil = 0;
    public const int GetOk = 1;
    public const int GetErr = 2;

    // скрытые поля
    protected List<T> _list;
    protected int? _current;
    protected int? _head;
    protected int? _tail;

    protected int _headStatus; // статус запроса head()
    protected int _tailStatus; // статус запроса tail()
    protected int _rightStatus; // статус запроса right()
    protected int _putRightStatus; // статус запроса _PutRight()
    protected int _putLeftStatus; // статус запроса _PutLeft()
    protected int _addToEmptyStatus; // статус запроса _AddToEmpty()
    protected int _addToTailStatus; // статус запроса _AddToTail()
    protected int _removeStatus; // статус запроса _Remove()
    protected int _removeAllStatus; // статус запроса _RemoveAll()
    protected int _replaceStatus; // статус запроса _Replace()
    protected int _findStatus; // статус запроса _Find()
    protected int _getStatus; // статус запроса _Get()

    // конструктор
    // постусловие: создан новый пустой linked list
    public ParentListAtd()
    {
        _current = null;
        _head = null;
        _tail = null;

        Clear();

        _list = [];
    }

    // команды
    // предусловие: список не пуст;
    // постусловие: курсор установлен на первый узел в списке;
    public T? Head()
    {
        if (Size() == 0)
        {
            _headStatus = HeadErr;

            return default;
        }

        if (_head is null)
        {
            _headStatus = HeadErr;

            return default;
        }

        _current = _head;
        _headStatus = HeadOk;

        return Get();
    }

    // предусловие: список не пуст;
    // постусловие: курсор установлен на последний узел в списке;
    public T? Tail()
    {
        if (Size() == 0 || _tail is null)
        {
            _tailStatus = TailErr;

            return default;
        }

        _tailStatus = TailOk;
        _current = _tail;

        return Get();
    }

    // предусловие: правее курсора есть элемент;
    // постусловие: курсор сдвинут на один узел вправо
    public T? Right()
    {
        if (Size() == 0 ||  Size() == 1 || _head is null)
        {
            _rightStatus = RightErr;

            return default;
        }

        _rightStatus = RightOk;
        _current += 1;

        return Get();
    }

    // предусловие: список не пуст;
    // постусловие: следом за текущим узлом добавлено новый узел с заданным значением
    public void PutRight(T value)
    {
        if (Size() == 0 || _current == null)
        {
            _putRightStatus = PutRightErr;

            return;
        }

        _tail++;
        _putRightStatus = PutRightOk;

        _list.Insert((int)_current + 1, value);
    }

    // // предусловие: список не пуст;
    // // постусловие: перед текущим узлом добавлено новый узел с заданным значением
    public  void PutLeft(T value)
    {
        if (Size() == 0 || _current == null)
        {
            _putLeftStatus = PutLeftErr;

            return;
        }

        if (Size() == 1)
        {
            _list.Insert(0, value);

            _tail++;
            _putLeftStatus = PutLeftOk;

            return;
        }

        _list.Insert((int)_current - 1, value);

        _tail++;
        _putLeftStatus = PutLeftOk;

        return;
    }

    // предусловие: список пуст;
    // постусловие: в списке один узел
    public void AddToEmpty(T value)
    {
        if (Size() != 0)
        {
            _addToEmptyStatus = AddToEmptyErr;

            return;
        }

        _addToEmptyStatus = AddToEmptyOk;

        _list.Add(value);
        _head = 0;
        _tail = 0;
        _current = 0;

        return;
    }

    // предусловие: список не пуст;
    // постусловие: новый узел добавлен в конец списка
    public void AddToTail(T value)
    {
        if (Size() == 0)
        {
            _addToTailStatus = AddToTailErr;

            return;
        }

        _list.Add(value);

        _addToTailStatus = AddToTailOk;
        _tail += 1;

        return;
    }

    // предусловие: список не пуст; 
    // постусловие: в списке удалены все узлы с заданным значением
    public void RemoveAll(T value)
    {
        if (Size() == 0 || _current == null)
        {
            _removeAllStatus = RemoveAllErr;

            return;
        }

        while(true)
        {
            Find(value);
            if (GetFindStatus() == FindOk)
            {
                Remove();
                continue;
            }

            break;
        }

        _removeAllStatus = RemoveAllOk;
    }

    // предусловие: список не пуст; 
    // постусловие: текущий узел удален, узел справа становится текущим, если узла справа нет, то узел слева
    public void Remove()
    {
        if (Size() == 0 || _current == null)
        {
            _removeStatus = RemoveErr;

            return;
        }

        _list.RemoveAt((int)_current);
        _tail -= 1;

        var newSize = Size();
        if (newSize == 0)
        {
            _current = null;
            Clear();
        }

        if (_current >= newSize)
        {
            _current = newSize - 1;
        }

        _removeStatus = RemoveOk;

        return;
    }

    // предусловие: список не пуст; 
    // постусловие: из списка удалены все значения
    public virtual void Clear()
    {
        _list = [];
        _current = null;

        _headStatus = HeadNil;
        _tailStatus = TailNil;
        _rightStatus = RightNil;
        _putRightStatus = PutRightNil;
        _putLeftStatus = PutLeftNil;
        _addToEmptyStatus = AddToEmptyNil;
        _addToTailStatus = AddToTailNil;
        _removeStatus = RemoveNil;
        _removeAllStatus = RemoveAllNil;
        _replaceStatus = ReplaceNil;
        _findStatus = FindNil;
        _getStatus = GetNil;
    }

    // предусловие: список не пуст; 
    // постусловие: значение текущего узла заменено на заданное значение
    public void Replace(T value)
    {
        if (Size() == 0 || _current == null)
        {
            _replaceStatus = ReplaceErr;

            return;
        }

        _list[(int)_current] = value;

        _replaceStatus = ReplaceOk;

        return;
    }

    // предусловие: стек не пустой
    // постусловие: курсор установлен на следующий узел с заданным значением, если такой узел найден
    public void Find(T value)
    {
        if (Size() == 0 || _current == null)
        {
            _findStatus = FindErr;

            return;
        }

        int? result = null;
        for (int i = (int)_current; i < Size(); i++)
        {
            if (_list[i] == null)
            {
                _findStatus = FindErr;
                break;
            }

            if (!_list[i]!.Equals(value))
            {
                continue;
            }

            result = i;
            _findStatus = FindOk;
            
            break;
        }

        if (result == null)
        {
            _findStatus = FindErr;
            return;
        }

        _current = result;
        _findStatus = FindOk;

        return;
    }


    // запросы
    public bool IsHead()
    {
        return _current == _head;
    }

    public bool IsTail()
    {
        return _current == _tail;
    }

    public bool IsValue()
    {
        var getValue = Get();

        return !getValue.Equals(default) && GetGetStatus() == GetOk;
    }

    public int Size()
    {
        return _list.Count;
    }

    // предусловие: стек не пустой
    public T? Get()
    {
        if (Size() == 0 || _current == null)
        {
            _getStatus = GetErr;

            return default;
        }
        _getStatus = GetOk;

        return _list[(int)_current];
    }

    // запросы статусов (возможные значения статусов)
    // возвращает значение Head*
    public int GetHeadStatus()
    {
        return _headStatus;
    }

    // возвращает значение Tail*
    public int GetTailStatus()
    {
        return _tailStatus;
    }

    // возвращает значение Right*
    public int GetRightStatus()
    {
        return _rightStatus;
    }

    // возвращает значение PutRight*
    public int GetPutRightStatus()
    {
        return _putRightStatus;
    }

    // возвращает значение PutLeft*
    public int GetPutLeftStatus()
    {
        return _putLeftStatus;
    }

    // возвращает значение AddToEmpty*
    public int GetAddToEmptyStatus()
    {
        return _addToEmptyStatus;
    }

    // возвращает значение AddToTail*
    public int GetAddToTailStatus()
    {
        return _addToTailStatus;
    }

    // возвращает значение Remove*
    public int GetRemoveStatus()
    {
        return _removeStatus;
    }

    // возвращает значение RemoveAll*
    public int GetRemoveAllStatus()
    {
        return _removeAllStatus;
    }

    // возвращает значение Replace*
    public int GetReplaceStatus()
    {
        return _replaceStatus;
    }

    // возвращает значение Find*
    public int GetFindStatus()
    {
        return _findStatus;
    }

    // возвращает значение Get*
    public int GetGetStatus()
    {
        return _getStatus;
    }
}

public abstract class LinkedListAtd<T> : ParentListAtd<T>
{
    public LinkedListAtd()
    { }
}

public abstract class TwoWayListAtd<T> : ParentListAtd<T>
{
    public TwoWayListAtd()
    { }

    // запросы
    // предусловие: стек не пустой
    // постусловие: узел слева становится текущим узлом если он есть
    public abstract T? Left();
    
    // команды
    public abstract int GetLeftStatus(); // возвращает значение Left*
}

// Реализация
public class LinkedList<T> : LinkedListAtd<T>
{
}

public class TwoWayList<T> : TwoWayListAtd<T>
{
    private int _leftStatus; // статус запроса left()
    public const int LeftNil = 0;
    public const int LeftOk = 1;
    public const int LeftErr = 2;

    public TwoWayList() : base()
    {
        _leftStatus = LeftNil;
    }

    public override void Clear()
    {
        base.Clear();
        _leftStatus = LeftNil;
    }

    public override T? Left()
    {
        if (Size() == 0 ||  Size() == 1 || _current is null || _current == 0)
        {
            _leftStatus = LeftErr;

            return default;
        }

        _leftStatus = LeftOk;
        _current -= 1;

        return Get();
    }

    public override int GetLeftStatus()
    {
        return _leftStatus;
    }
}
