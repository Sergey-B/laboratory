using Task5;
using Xunit;

public class QueueTests
{
    private Task5.Queue<string> _queue;

    public QueueTests()
    {
        _queue = new Task5.Queue<string>();
    }

    [Fact]
    public void ConstructorShouldReturnNewEmptyQueue()
    {
        // Arrange
        // Act
        var queue = new Task5.Queue<string>();

        // Assert
        Assert.Equal(0, queue.Size());
    }

    [Fact]
    public void PushShouldAddNewElementToQueueEnd()
    {
        // Arrange
        // Act
        _queue.Push("first");
        var getPushStatus = _queue.GetPushStatus();

        // Assert
        Assert.Equal(1, _queue.Size());
        Assert.Equal(QueueAtd<string>.PushOk, getPushStatus);
    }

    [Fact]
    public void PopShouldReturnAndRemoveFirstElementFromQueueHead()
    {
        // Arrange
        _queue.Push("first");
        _queue.Push("second");
        var getPushStatus = _queue.GetPushStatus();

        // Act
        var item = _queue.Pop();
        var popStatus = _queue.GetPopStatus();

        // Assert
        Assert.Equal("first", item);
        Assert.Equal(QueueAtd<string>.PopOk, popStatus);
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