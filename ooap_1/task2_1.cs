abstract class LinkedList<T>
{
    public const int PutNil = 0;
    public const int PutOk = 1;
    public const int PutErr = 2;
    public const int RemoveNil = 0;
    public const int RemoveOk = 1;
    public const int RemoveErr = 2;
    public const int FindNil = 0;
    public const int FindOk = 1;
    public const int FindErr = 2;
    public const int GetNil = 0;
    public const int GetOk = 1;
    public const int GetErr = 2;

    // конструктор
    // постусловие: создан новый пустой linked list
    public LinkedList()
    {
    }

    // команды
    // постусловие: добавлено новое значение следом за текущим узлом 
    public abstract void PutRight(T item);

    // постусловие: добавлено новое значение перед текущим узлом 
    public abstract void PutLeft(T item);

    // постусловие: текущий узел удален, узел справа становится текущим, если узла справа нет, то узел слева
    public abstract void Remove();

    // постусловие: из связного списка удалены все значения
    public abstract void Clear();

    // предусловие: список пустой
    // постусловие: добавлен новый узел в список
    public abstract void AddToEmpty(T item);

    // предусловие: список не пустой
    // постусловие: добавлен новый узел в конец списка
    public abstract void AddToTail(T item);

    // предусловие: список не пустой
    // постусловие: текущий узел заменен на заданное значение
    public abstract void Replace(T item);

    // постусловие: в списке удалены все узлы с заданным значением
    public abstract void RemoveAll(T value);

    // запросы
    // предусловие: стек не пустой
    // постусловие: первый узел становится текущим узлом
    public abstract LinkedList<T> Head();

    // предусловие: стек не пустой
    // постусловие: последний становится текущим узлом
    public abstract LinkedList<T> Tail();

    // предусловие: стек не пустой
    // постусловие: узел справа становится текущим узлом
    public abstract LinkedList<T> Right();

    // предусловие: стек не пустой
    public abstract LinkedList<T> Get();

    // предусловие: стек не пустой
    public abstract LinkedList<T> Find(T value);
    public abstract int Size();

    // запросы статуса команд
    public abstract int GetPutStatus(); // возвращает значение Put*
    public abstract int GetRemoveStatus(); // возвращает значение Remove*
    public abstract int GetGetStatus(); // возвращает значение Get*
    public abstract int GetFindStatus(); // возвращает значение Find*

    // дополнительные запросы
    public abstract bool IsHead();
    public abstract bool IsTail();
    public abstract bool IsValue();
}