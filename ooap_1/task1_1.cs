using System.Collections.Generic;

abstract class BoundedStackATD<T>
{
    // защищенные поля
    protected List<T> _stack; // основное хранилище стека

    protected readonly int _stackMaxSize; // максимальное количество элементов в стеке
    private const int DefaultStackMaxSize = 32; // максимальное количество элементов в стеке по умолчанию

    public const int PopNil = 0;
    public const int PopOk = 1;
    public const int PopErr = 2;
    public const int PeekNil = 0;
    public const int PeekOk = 1;
    public const int PeekErr = 2;
    public const int PushNil = 0;
    public const int PushOk = 1;
    public const int PushErr = 2;

    // конструктор
    // постусловие: создан новый пустой стек
    public BoundedStackATD(int stackMaxSize = 0)
    {
        if (stackMaxSize == 0)
        {
            _stackMaxSize = DefaultStackMaxSize;
        }
        else
        {
            _stackMaxSize = stackMaxSize;
        }
        Clear();

        _stack = [];
    }

    // команды
    // предусловие: количество элементов в стеке меньше максимального
    // постусловие: в стек добавлено новое значение
    public abstract void Push(T value);
    // предусловие: стек не пустой
    // постусловие: из стека удалён верхний элемент
    public abstract void Pop();

    // постусловие: из стека удалятся все значения
    public abstract void Clear();

    // запросы
    // предусловие: стек не пустой
    public abstract T? Peek();

    public abstract int Size();

    public abstract int GetMaxSize(); // возвращает значение stack MaxSize

    // запросы статусов
    public abstract int GetPopStatus(); // возвращает значение Pop*
    public abstract int GetPeekStatus(); // возвращает значение Peek*
    public abstract int GetPushStatus(); // возвращает значение Push*
}

class BoundedStack<T> : BoundedStackATD<T>
{
    // скрытые поля
    private int _peekStatus; // статус запроса Peek()
    private int _popStatus; // статус команды Pop()
    private int _pushStatus; // статус команды Push()

    // интерфейс класса, реализующий АТД BoundedStack

    // конструктор
    // постусловие: создан новый пустой стек
    public BoundedStack(int stackMaxSize = 0) : base(stackMaxSize)
    {    }

    // предусловие: количество элементов в стеке меньше максимального
    // постусловие: в стек добавлено новое значение
    public override void Push(T value)
    {
        if (Size() < GetMaxSize())
        {
            _stack.Add(value);
            _pushStatus = PushOk;
        }
        else
        {
            _pushStatus = PushErr;
        }
    }

    // предусловие: стек не пустой
    // постусловие: из стека удалён верхний элемент
    public override void Pop()
    {
        var size = Size();
        if (size > 0)
        {
            _stack.RemoveAt(size - 1);
            _popStatus = PopOk;
        }
        else
        {
            _popStatus = PopErr;
        }
    }

    // постусловие: из стека удалятся все значения
    public override void Clear()
    {
        _stack = [];
        
        // начальные статусы для предусловий Peek(), Pop() и Push()
        _peekStatus = PeekNil;
        _popStatus = PopNil;
        _pushStatus = PushNil;
    }

    // предусловие: стек не пустой
    public override T? Peek()
    {
        T? result;

        var size = Size();
        if (size > 0)
        {
            result = _stack[size - 1];
            _peekStatus = PeekOk;
        }
        else
        {
            result = default(T);
            _peekStatus = PeekErr;
        }

        return result;
    }

    public override int Size()
    {
        return _stack.Count;
    }

    // запросы статусов
    public override int GetPopStatus()
    {
        return _popStatus;
    }

    public override int GetPeekStatus()
    {
        return _peekStatus;
    }

    public override int GetPushStatus()
    {
        return _pushStatus;
    }

    public override int GetMaxSize()
    {
        return _stackMaxSize;
    }
}