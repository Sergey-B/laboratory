namespace Task6;

// задание 6
// задача 1. Спроектировать АТД Deque и выполните её реализацию.
// задача 2. Совместить АТД и реализации Queue и Deque в одну иерархию.

public abstract class ParentQueueAtd<T>
{
    public const int AddTailNil = 0;
    public const int AddTailOk = 1;
    public const int AddTailErr = 2;
    public const int RemoveFrontNil = 0;
    public const int RemoveFrontOk = 1;
    public const int RemoveFrontErr = 2;

    // статусы
    protected int _addTailStatus;
    protected int _removeFrontStatus;
    protected T?[] _queue;
    protected int _head; // указатель на начало очереди
    protected int _size; // текущее количество элементов в очереди
    protected int _capacity; // максимальное количество элементов в очереди
    protected const int DefaultCapacity = 10;

    // конструктор
    // постусловие: создана новая пустая очередь
    public ParentQueueAtd()
    {
        _capacity = DefaultCapacity;
        _queue = new T[_capacity];

        _head = 0;
        _size = 0;

        _addTailStatus = AddTailNil;
        _removeFrontStatus = RemoveFrontNil;
    }

    // команды
    // постусловие: в конец очереди добавлен новый элемент
    public void AddTail(T item)
    {
        if (Size() == _capacity)
        {
            _addTailStatus = AddTailErr;

            return;
        }

        // индекс двигается по кольцу, сначала до конца массива, 
        // при достижении конца продолжается с 0 элемента
        var index = (_head + _size) % _capacity;
        _queue[index] = item; // O(1)

        _size++;
        _addTailStatus = AddTailOk;
    }

    // предусловие: очередь не пустая
    // постусловие: из головы очереди удалён первый  элемент
    public T? RemoveFront()
    {
        // если очередь пустая, то вернуть ошибку
        if (Size() == 0)
        {
            _removeFrontStatus = RemoveFrontErr;

            return default;
        }

        var item = _queue[_head]; // O(1)
        _queue[_head] = default; // обнулить ячейку

        // голова очереди куда добавляются элементы смещается вправо, 
        // при достижении конца массива (когда остаток от деления = 0) голова очереди перемещается в начало массива
        // и принимает значения начиная с 0 
        _head = (_head + 1) % _capacity;
        _size--;
        _removeFrontStatus = RemoveFrontOk;

        return item;
    }

    // запросы
    public int Size()
    {
        return _size;
    }

    // запросы статусов
    public int GetAddTailStatus() // возвращает значение AddTail*
    {
        return _addTailStatus;
    }
    public int GetRemoveFrontStatus() // возвращает значение RemoveFront*
    {
        return _removeFrontStatus;
    }
}

public abstract class QueueAtd<T> : ParentQueueAtd<T>
{}

public abstract class DequeAtd<T> : ParentQueueAtd<T>
{
    public const int AddFrontNil = 0;
    public const int AddFrontOk = 1;
    public const int AddFrontErr = 2;
    public const int RemoveTailNil = 0;
    public const int RemoveTailOk = 1;
    public const int RemoveTailErr = 2;

    // конструктор
    // постусловие: создано новое пустое хранилище
    public DequeAtd()
    {
    }

    // команды
    // предусловие: очередь не полная
    // постусловие: в голову очереди добавлен новый элемент
    public abstract void AddFront(T item);

    // предусловие: очередь не пустая
    // постусловие: из хвоста очереди удалён первый  элемент
    public abstract T? RemoveTail();

    // запросы статусов
    public abstract int GetAddFrontStatus(); // возвращает значение AddFront*
    public abstract int GetRemoveTailStatus(); // возвращает значение RemoveTail*
}

public class Deque<T> : DequeAtd<T>
{
    private int _addFrontStatus;
    private int _removeTailStatus;

    public Deque() : base()
    {
        _addFrontStatus = AddFrontNil;
        _removeTailStatus = RemoveTailNil;
    }

    public override void AddFront(T item)
    {
        // вернуть ошибку если очередь полная
        if (Size() == _capacity)
        {
            _addFrontStatus = AddFrontErr;

            return;
        }

        // индекс двигается по кольцу, к началу массива, 
        // при достижении начала массива продолжается с последнего элемента
        _head = (_head - 1 + _capacity) % _capacity;
        _queue[_head] = item; // O(1)

        _size++;
        _addFrontStatus = AddFrontOk;
    }

    public override T? RemoveTail()
    {
        // если очередь пустая, то вернуть ошибку
        if (Size() == 0)
        {
            _removeTailStatus = RemoveTailErr;

            return default;
        }

        int tail = (_head + _size - 1) % _capacity;
        var item = _queue[tail];
        _queue[tail] = default; // обнулить ячейку
        
        _size--;
        _removeTailStatus = RemoveTailOk;

        return item;
    }

    public override int GetAddFrontStatus()
    {
        return _addFrontStatus;
    }

    public override int GetRemoveTailStatus()
    {
        return _removeTailStatus;
    }
}