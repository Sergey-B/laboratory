using System;
using System.Collections.Generic;

namespace Task5;

// задание 5
// задача 1. Спроектировать АТД Queue и выполните её реализацию.
// задача 2. Оцените меру сложности для операций enqueue() (добавление) и dequeue() (удаление) в данной реализации.
// Push - сложность O(n), Pop - сложность O(1)
public abstract class QueueAtd<T>
{
    public const int PopNil = 0;
    public const int PopOk = 1;
    public const int PopErr = 2;
    public const int PushNil = 0;
    public const int PushOk = 1;
    public const int PushErr = 2;

    // конструктор
    // постусловие: создана новая пустая очередь
    public QueueAtd()
    {
    }

    // команды
    // постусловие: в конец очереди добавлен новый элемент
    public abstract void Push(T item);

    // предусловие: очередь не пустая
    // постусловие: из головы очереди удалён первый  элемент
    public abstract T? Pop();

    // запросы
    public abstract int Size();

    // запросы статусов
    public abstract int GetPopStatus(); // возвращает значение Pop*
    public abstract int GetPushStatus(); // возвращает значение Push*
}

public class Queue<T> : QueueAtd<T>
{
    private List<T> _queue;
    private int _popStatus;
    private int _pushStatus;

    public Queue() : base()
    {
        _queue = [];

        _pushStatus = PushNil;
        _popStatus = PushNil;
    }

    public override T? Pop()
    {
        if (Size() == 0)
        {
            _popStatus = PopErr;

            return default;
        }

        var lastIndex = Size() - 1;
        var item = _queue[lastIndex];
        _queue.RemoveAt(lastIndex); // O(1)

        _popStatus = PopOk;

        return item;
    }

    public override void Push(T item)
    {
        _queue.Insert(0, item); // O(n) ?

        _pushStatus = PushOk;

        return;
    }

    public override int Size()
    {
        return _queue.Count;
    }

    public override int GetPopStatus()
    {
        return _popStatus;
    }

    public override int GetPushStatus()
    {
        return _pushStatus;
    }
}