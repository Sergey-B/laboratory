abstract class BoundedStack<T>
{
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
  public BoundedStack<T> BoundedStack(int stackMaxSize = 0);

  // команды
  // предусловие: количество элементов в стеке меньше максимального
  // постусловие: в стек добавлено новое значение
  public void Push(T value);

  // предусловие: стек не пустой
  // постусловие: из стека удалён верхний элемент
  public void Pop();

  // постусловие: из стека удалятся все значения
  public void Clear();

  // запросы
  // предусловие: стек не пустой
  public T Peek();

  public int Size();

  // запросы статусов
  public int GetPopStatus(); // возвращает значение Pop*
  public int GetPeekStatus(); // возвращает значение Peek*
  public int GetPushStatus(); // возвращает значение Push*
}

class BoundedStack<T>
{
  // скрытые поля
  private List<T> _stack; // основное хранилище стека
  private int _peekStatus; // статус запроса Peek()
  private int _popStatus; // статус команды Pop()
  private int _pushStatus; // статус команды Push()
  private readonly int _stackMaxSize; // максимальное количество элементов в стеке
  private const int DefaultStackMaxSize = 32; // максимальное количество элементов в стеке по умолчанию

  // интерфейс класса, реализующий АТД BoundedStack
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
  public BoundedStack<T> BoundedStack(int stackMaxSize = 0)
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

    return this;
  }

  // предусловие: количество элементов в стеке меньше максимального
  // постусловие: в стек добавлено новое значение
  public void Push(T value)
  {
    if (Size() < GetStackMaxSize())
    {
      stack.Append(value);
      _pushStatus = PushOk;
    }
    else
    {
      _pushStatus = PushErr;
    }
  }

  // предусловие: стек не пустой
  // постусловие: из стека удалён верхний элемент
  public void Pop()
  {
    if (Size() > 0)
    {
      stack.RemoveAt(-1);
      _popStatus = PopOk;
    }
    else
    {
      _popStatus = PopErr;
    }
  }

  // постусловие: из стека удалятся все значения
  public void Clear()
  {
    _stack = [];// пустой список/стек

    // начальные статусы для предусловий Peek(), Pop() и Push()
    _peekStatus = PeekNil;
    _popStatus = PopNil;
    _pushStatus = PushNil;
  }

  // предусловие: стек не пустой
  public T Peek()
  {
    if (Size() > 0)
    {
      result = _stack[-1];
      _peekStatus = PeekOk;
    }
    else
    {
      result = 0;
      _peekStatus = PeekErr;
    }

    return result;
  }

  public int Size()
  {
    return _stack.Length();
  }

  // запросы статусов
  public int GetPopStatus()
  {
    return _popStatus;
  }

  public int GetPeekStatus()
  {
    return _peekStatus;
  }

  public int GetPushStatus()
  {
    return _pushStatus;
  }
}

// добавить пре и пост условия как комментарии
// прочитать про инициализацию класса
// нужен ли абстрактный класс или интерфейс?