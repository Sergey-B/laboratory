using Task6;
using Xunit;

public class DequeTests
{
    private Task6.Deque<string> _deque;

    public DequeTests()
    {
        _deque = new Task6.Deque<string>();
    }

    [Fact]
    public void ConstructorShouldReturnNewEmptyInstance()
    {
        // Arrange
        // Act
        var Deque = new Task6.Deque<string>();

        // Assert
        Assert.Equal(0, Deque.Size());
    }

    [Fact]
    public void AddTailShouldAddNewElementToQueueEnd()
    {
        // Arrange
        // Act
        _deque.AddTail("first");
        var getAddTailStatus = _deque.GetAddTailStatus();

        // Assert
        Assert.Equal(1, _deque.Size());
        Assert.Equal(Deque<string>.AddTailOk, getAddTailStatus);
    }

    [Fact]
    public void RemoveFrontShouldReturnAndRemoveFirstElementFromQueueHead()
    {
        // Arrange
        _deque.AddTail("first");
        _deque.AddTail("second");

        // Act
        var item = _deque.RemoveFront();
        var removeFrontStatus = _deque.GetRemoveFrontStatus();

        // Assert
        Assert.Equal("first", item);
        Assert.Equal(DequeAtd<string>.RemoveFrontOk, removeFrontStatus);
    }

    [Fact]
    public void RemoveFrontShouldReturnNothingWhenQueueIsEmpty()
    {
        // Arrange
        // Act
        var item = _deque.RemoveFront();
        var removeFrontStatus = _deque.GetRemoveFrontStatus();

        // Assert
        Assert.Equal(default, item);
        Assert.Equal(Deque<string>.RemoveFrontErr, removeFrontStatus);
    }

    [Fact]
    public void GetRemoveFrontStatusShouldReturnErrorWhenQueueIsEmpty()
    {
        // Arrange
        // Act
        var item = _deque.RemoveFront();
        var removeFrontStatus = _deque.GetRemoveFrontStatus();

        // Assert
        Assert.Equal(default, item);
        Assert.Equal(Task6.DequeAtd<string>.RemoveFrontErr, removeFrontStatus);
    }

    [Fact]
    public void AddFrontShouldAddNewElementToQueueHead()
    {
        // Arrange
        // Act
        _deque.AddFront("first");
        var getAddFrontStatus = _deque.GetAddFrontStatus();

        // Assert
        Assert.Equal(1, _deque.Size());
        Assert.Equal(Deque<string>.AddFrontOk, getAddFrontStatus);
    }

    [Fact]
    public void AddFrontShouldReturnErrirWhenQueueFull()
    {
        // Arrange
        _deque.AddFront("1");
        _deque.AddFront("2");
        _deque.AddFront("3");
        _deque.AddFront("4");
        _deque.AddFront("5");
        _deque.AddFront("6");
        _deque.AddFront("7");
        _deque.AddFront("8");
        _deque.AddFront("9");
        _deque.AddFront("10");

        // Act
        _deque.AddFront("11");
        var getAddFrontStatus = _deque.GetAddFrontStatus();

        // Assert
        Assert.Equal(10, _deque.Size());
        Assert.Equal(Deque<string>.AddFrontErr, getAddFrontStatus);
    }

    [Fact]
    public void RemoveTailShouldReturnAndRemoveFirstElementFromQueueTail()
    {
        // Arrange
        _deque.AddTail("first");
        _deque.AddTail("last");

        // Act
        var item = _deque.RemoveTail();
        var removeTailStatus = _deque.GetRemoveTailStatus();

        // Assert
        Assert.Equal("last", item);
        Assert.Equal(DequeAtd<string>.RemoveTailOk, removeTailStatus);
    }

    [Fact]
    public void RemoveTailShouldReturnErrorWhenQueueIsEmpty()
    {
        // Arrange
        // Act
        var item = _deque.RemoveTail();
        var removeTailStatus = _deque.GetRemoveTailStatus();

        // Assert
        Assert.Equal(default, item);
        Assert.Equal(Deque<string>.RemoveTailErr, removeTailStatus);
    }
}