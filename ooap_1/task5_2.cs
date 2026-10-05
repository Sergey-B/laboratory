using System;
using System.Collections.Generic;
using Xunit;

namespace Task5;

// задание 5

// задача 4.* Реализуйте очередь с помощью двух стеков.
// Сложность добавления в очередь (Push) - O(1), сложность удаления элемента из очереди (Pop) - в среднем O(1), в худшем случае O(1)
public class TwoStacksQueue<T> : QueueAtd<T>
{
    private Stack<T> _queue_in;
    private Stack<T> _queue_out;
    private int _size;
    private int _popStatus;
    private int _pushStatus;

    public TwoStacksQueue()
    {
        _queue_in = new Stack<T>();
        _queue_out = new Stack<T>();
        _size = 0;

        _pushStatus = PushNil;
        _popStatus = PushNil;
    }

    public override void Push(T item)
    {
        _queue_in.Push(item);
        _size++;
        _pushStatus = PushOk;
    }

    public override T? Pop()
    {
        // если очередь пустая, то вернуть ошибку
        if (_queue_in.Count == 0 && _queue_out.Count == 0)
        {
            _popStatus = PopErr;

            return default;
        }

        while (_queue_in.Count > 0)
        {
            var item = _queue_in.Pop();
            if (item == null)
            {
                _popStatus = PopErr;
                return default;
            }

            _queue_out.Push(item);
        }

        var result = _queue_out.Pop();
        _size--;

        _popStatus = PopOk;

        return result;
    }

    public override int Size()
    {
        return _size;
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

// задача 3.* Напишите функцию, которая "вращает" очередь по кругу на N элементов.
// Добавлен метод Rotate, который перемещает n элементов из начала очереди в конец, cложность вращения очереди - O(n)

// задача 5.* Добавьте функцию, которая обращает все элементы в очереди в обратном порядке.
// Добавлен метод Reverse, cложность вращения очереди - O(n)

// задача 6.* Реализуйте круговую (циклическую буферную) очередь статическим массивом фиксированного размера.
// Сложность добавления в очередь (Push) - O(1), сложность удаления элемента из очереди (Pop) - O(1)
public abstract class CyclicQueueAtd<T> : QueueAtd<T>
{
    public const int ReverseNil = 0;
    public const int ReverseOk = 1;
    public const int ReverseErr = 2;
    public const int RotateNil = 0;
    public const int RotateOk = 1;
    public const int RotateErr = 2;

    public CyclicQueueAtd()
    {}

    // команды
    // предусловие: очередь не пустая
    // постусловие: элементы поменяли порядок на обратный
    public abstract void Reverse();

    // предусловие: очередь не пустая
    // постусловие: n элементов из конца добавлены в начало очереди
    public abstract void Rotate(int numberOfElements);

    // запросы
    public abstract int GetReverseStatus(); // возвращает значение Reverse*
    public abstract int GetRotateStatus(); // возвращает значение Rotate*
}

public class CyclicQueue<T> : CyclicQueueAtd<T>
{
    private T[] _queue;
    private int _popStatus;
    private int _pushStatus;
    private int _reverseStatus;
    private int _rotateStatus;
    private int _head; // указатель на начало очереди
    private int _size; // количество элементов в очереди
    private int _capacity; // количество элементов в очереди

    private const int DefaultCapacity = 10;

    public CyclicQueue()
    {
        _capacity = DefaultCapacity;
        _queue = new T[_capacity];

        _head = 0;
        _size = 0;

        _pushStatus = PushNil;
        _popStatus = PushNil;
        _reverseStatus = ReverseNil;
        _rotateStatus = RotateNil;
    }

    public override void Rotate(int numberOfElements)
    {
        // вернуть ошибку если очередь пустая
        if (Size() == 0)
        {
            _rotateStatus = RotateErr;
            return;
        }

        var numberOfRotations = numberOfElements % Size();
        while (numberOfRotations > 0)
        {
            var item = Pop();
            if (item is null || _popStatus != PopOk)
            {
                _rotateStatus = RotateErr;
                break;
            }

            Push(item);
            if (_pushStatus != PushOk)
            {
                _rotateStatus = RotateErr;
                break;
            }

            numberOfRotations--;
        }

        _rotateStatus = RotateOk;
    }

    public override void Reverse()
    {
        // вернуть ошибку если очередь пустая
        if (Size() == 0)
        {
            _reverseStatus = ReverseErr;
            return;
        }

        var buffer = new Stack<T>();

        while (_size > 0)
        {
            var item = Pop();
            if (item is null || _popStatus != PopOk)
            {
                _reverseStatus = ReverseErr;
                break;
            }

            buffer.Push(item!);
        }

        while (buffer.Count > 0)
        {
            Push(buffer.Pop());
        }

        _reverseStatus = ReverseOk;
    }

    public override T? Pop()
    {
        // если очередь пустая, то вернуть ошибку
        if (Size() == 0)
        {
            _popStatus = PopErr;

            return default;
        }

        var item = _queue[_head]; // O(1)

        // голова очереди куда добавляются элементы смещается вправо, 
        // при достижении конца массива (когда остаток от деления = 0) голова очереди перемещается в начало массива
        // и принимает значения начиная с 0 
        _head = (_head + 1) % _capacity;
        _size--;
        _popStatus = PopOk;

        return item;
    }
    
    public override void Push(T value)
    {
        // если очередь заполнена, то вернуть ошибку
        if (Size() == _capacity)
        {
            _pushStatus = PushErr;

            return;
        }
        
        // индекс двигается по кольцу, сначала до конца массива, 
        // при достижении конца продолжается с 0 элемента
        var index = (_head + _size) % _capacity;
        _queue[index] = value; // O(1)

        _size++;
        _pushStatus = PushOk;

        return;
    }

    public override int Size()
    {
        return _size;
    }

    public override int GetPopStatus()
    {
        return _popStatus;
    }

    public override int GetPushStatus()
    {
        return _pushStatus;
    }


    public override int GetReverseStatus()
    {
        return _reverseStatus;
    }
    
    public override int GetRotateStatus()
    {
        return _rotateStatus;
    }
}

public class CyclicQueueTests
{
    private Task5.CyclicQueue<string> _queue;

    public CyclicQueueTests()
    {
        _queue = new Task5.CyclicQueue<string>();
    }

    [Fact]
    public void ConstructorShouldReturnNewEmptyQueue()
    {
        // Arrange
        // Act
        var queue = new Task5.CyclicQueue<string>();

        // Assert
        Assert.Equal(0, queue.Size());
    }

    [Fact]
    public void PushShouldAddNewElementToQueueEnd()
    {
        // Arrange
        _queue.Push("first");

        // Act
        _queue.Push("second");
        var getPushStatus = _queue.GetPushStatus();

        // Assert
        Assert.Equal(2, _queue.Size());
        Assert.Equal(QueueAtd<string>.PushOk, getPushStatus);
    }

    [Fact]
    public void PopShouldReturnAndRemoveFirstElementFromQueueHead()
    {
        // Arrange
        _queue.Push("first");
        _queue.Push("second");

        // Act
        var item = _queue.Pop();
        var popStatus = _queue.GetPopStatus();

        // Assert
        Assert.Equal(1, _queue.Size());
        Assert.Equal("first", item);
        Assert.Equal(QueueAtd<string>.PopOk, popStatus);
    }


    [Fact]
    public void PushShouldNotAddNewElementWhenQueueIsFull()
    {
        // Arrange
        _queue.Push("1");
        _queue.Push("2");
        _queue.Push("3");
        _queue.Push("4");
        _queue.Push("5");
        _queue.Push("6");
        _queue.Push("7");
        _queue.Push("8");
        _queue.Push("9");
        _queue.Push("10");

        // Act
        _queue.Push("11");
        var getPushStatus = _queue.GetPushStatus();

        // Assert
        Assert.Equal(10, _queue.Size());
        Assert.Equal(QueueAtd<string>.PushErr, getPushStatus);
    }

    [Fact]
    public void PushShouldReturnElementAfterPopping()
    {
        // Arrange
        _queue.Push("1");
        _queue.Push("2");
        _queue.Push("3");
        _queue.Push("4");
        _queue.Push("5");
        _queue.Push("6");
        _queue.Push("7");
        _queue.Push("8");
        _queue.Push("9");
        _queue.Push("10");
        _queue.Pop();

        // Act
        _queue.Push("11");
        var getPushStatus = _queue.GetPushStatus();

        // Assert
        Assert.Equal(10, _queue.Size());
        Assert.Equal(QueueAtd<string>.PushOk, getPushStatus);
    }

    [Fact]
    public void ReverseShouldChangeElementsOrder()
    {
        // Arrange
        _queue.Push("1");
        _queue.Push("2");
        _queue.Push("3");
        _queue.Push("4");
        _queue.Push("5");
        _queue.Push("6");
        _queue.Push("7");
        _queue.Push("8");
        _queue.Push("9");
        _queue.Push("10");

        // Act
        _queue.Reverse();
        var getReverseStatus = _queue.GetReverseStatus();
        var item = _queue.Pop();
        var getPopStatus = _queue.GetPopStatus();

        // Assert
        Assert.Equal("10", item);
        Assert.Equal(CyclicQueue<string>.PopOk, getPopStatus);
        Assert.Equal(CyclicQueue<string>.ReverseOk, getReverseStatus);
    }

    [Fact]
    public void ReverseShouldReturnErrorWhenQueueIsEmpty()
    {
        // Arrange
        // Act
        _queue.Reverse();
        var getReverseStatus = _queue.GetReverseStatus();

        // Assert
        Assert.Equal(CyclicQueue<string>.ReverseErr, getReverseStatus);
    }

    [Fact]
    public void PopShouldReturnNothingWhenQueueIsEmpty()
    {
        // Arrange
        // Act
        var item = _queue.Pop();
        var popStatus = _queue.GetPopStatus();

        // Assert
        Assert.Equal(default, item);
        Assert.Equal(QueueAtd<string>.PopErr, popStatus);
    }

    [Fact]
    public void RotateShouldPutFromEndToHeadNElements()
    {
        // Arrange
        _queue.Push("1");
        _queue.Push("2");
        _queue.Push("3");
        _queue.Push("4");
        _queue.Push("5");
        _queue.Push("6");
        _queue.Push("7");
        _queue.Push("8");
        _queue.Push("9");
        _queue.Push("10");

        // до Rotate
        // [1, 2, 3, 4, ...9, 10]
        // после Rotate
        // [4, 5, 6, 7, 8, ...9, 10, 1, 2, 3]

        // Act
        _queue.Rotate(3);
        var getRotateStatus = _queue.GetRotateStatus();
        var item1 = _queue.Pop();
        var item2 = _queue.Pop();
        var item3 = _queue.Pop();
        var getPopStatus = _queue.GetPopStatus();

        // Assert
        Assert.Equal("4", item1);
        Assert.Equal("5", item2);
        Assert.Equal("6", item3);
        Assert.Equal(CyclicQueue<string>.PopOk, getPopStatus);
        Assert.Equal(CyclicQueue<string>.RotateOk, getRotateStatus);
    }

    [Fact]
    public void GetPopStatusShouldReturnErrorWhenQueueIsEmpty()
    {
        // Arrange
        // Act
        var item = _queue.Pop();
        var popStatus = _queue.GetPopStatus();

        // Assert
        Assert.Equal(default, item);
        Assert.Equal(QueueAtd<string>.PopErr, popStatus);
    }
}