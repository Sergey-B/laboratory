using Xunit;

public class BoundedStackTests
{

    [Fact]
    public void ConstructorShouldReturnNewEmptyBoundedStackWithDefaultMaxSize()
    {
        // Arrange
        var stack = new BoundedStack<int>();

        // Assert
        // stack.GetMaxSize().Should.Be(32);
        var expected = 32;
        var actual = stack.GetMaxSize();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ConstructorShouldShouldAllowSetCustomMaxSize()
    {
        // Arrange
        var customMaxSize = 15;
        var stack = new BoundedStack<int>(customMaxSize);

        // Assert
        var expected = customMaxSize;
        var actual = stack.GetMaxSize();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void PushShouldNotAddNewItemWhenStackSizeLessThenLimit()
    {
        // Arrange
        var stack = new BoundedStack<int>(1);

        // stack is empty, no operations yet
        Assert.Equal(BoundedStack<int>.PushNil, stack.GetPushStatus());

        // stack has elements
        stack.Push(1);
        Assert.Equal(1, stack.Size());
        Assert.Equal(BoundedStack<int>.PushOk, stack.GetPushStatus());

        // stack reached its size limit
        stack.Push(2);
        Assert.Equal(1, stack.Size());
        Assert.Equal(BoundedStack<int>.PushErr, stack.GetPushStatus());
    }

    [Fact]
    public void PopShouldRemoveNothingWhenStackIsNotEmpty()
    {
        // Arrange
        var stack = new BoundedStack<int>(2);

        // stack is empty, no operations yet
        Assert.Equal(0, stack.Size());
        Assert.Equal(BoundedStack<int>.PopNil, stack.GetPopStatus());

        // stack has elements
        stack.Push(1);
        stack.Pop();

        Assert.Equal(0, stack.Size());
        Assert.Equal(BoundedStack<int>.PopOk, stack.GetPopStatus());

        // stack has no elements
        stack.Pop();

        Assert.Equal(0, stack.Size());
        Assert.Equal(BoundedStack<int>.PopErr, stack.GetPopStatus());
    }

    [Fact]
    public void PeekShouldReturnTopElementWhenStackIsNotEmpty()
    {
        // Arrange
        var stack = new BoundedStack<int>(2);

        // When stack is empty
        Assert.Equal(BoundedStack<int>.PeekNil, stack.GetPeekStatus());

        var peek1 = stack.Peek();
        Assert.Equal(peek1, default(int));
        Assert.Equal(0, stack.Size());
        Assert.Equal(BoundedStack<int>.PeekErr, stack.GetPeekStatus());

        // When stack has elements
        stack.Push(1);
        
        var peek2 = stack.Peek();
        Assert.Equal(peek2, 1);
        Assert.Equal(1, stack.Size());
        Assert.Equal(BoundedStack<int>.PeekOk, stack.GetPeekStatus());
    }

    [Fact]
    public void ClearShouldRemoveAllElementsWhenStackIsNotEmpty()
    {
        // Arrange
        var stack = new BoundedStack<int>(2);

        // Act
        stack.Push(1);
        stack.Clear();

        // Assert
        Assert.Equal(0, stack.Size());
        Assert.Equal(BoundedStack<int>.PeekNil, stack.GetPeekStatus());
        Assert.Equal(BoundedStack<int>.PushNil, stack.GetPushStatus());
        Assert.Equal(BoundedStack<int>.PopNil, stack.GetPopStatus());
    }

    [Fact]
    public void SizeShouldReturnStackSize()
    {
        // Arrange
        var stack = new BoundedStack<int>(2);

        // Act
        stack.Push(1);
        stack.Push(2);

        // Assert
        Assert.Equal(2, stack.Size());
    }

    [Fact]
    public void GetMaxSizeShouldReturnMaxSize()
    {
        // Arrange
        var stack = new BoundedStack<int>(2);

        // Assert
        Assert.Equal(2, stack.GetMaxSize());
    }

    [Fact]
    public void GetPopStatusShouldReturnPopStatus()
    {
        // stack initially empty
        var stack = new BoundedStack<int>(2);
        Assert.Equal(BoundedStack<int>.PopNil, stack.GetPopStatus());

        // stack has elements
        stack.Push(1);
        stack.Pop();
        Assert.Equal(BoundedStack<int>.PopOk, stack.GetPopStatus());

        // stack has no elements
        stack.Pop();
        Assert.Equal(BoundedStack<int>.PopErr, stack.GetPopStatus());
    }

    [Fact]
    public void GetPushStatusShouldReturnPushStatus()
    {
        // stack initially empty
        var stack = new BoundedStack<int>(1);
        Assert.Equal(BoundedStack<int>.PushNil, stack.GetPushStatus());

        // stack has elements
        stack.Push(1);
        Assert.Equal(BoundedStack<int>.PushOk, stack.GetPushStatus());

        // reached stack size limit
        stack.Push(1);
        Assert.Equal(BoundedStack<int>.PushErr, stack.GetPushStatus());
    }

    [Fact]
    public void GetPeekStatusShouldReturnPeekStatus()
    {
        // stack initially empty
        var stack = new BoundedStack<int>(1);
        Assert.Equal(BoundedStack<int>.PeekNil, stack.GetPeekStatus());

        // stack has elements
        stack.Push(1);
        var peek1 = stack.Peek();
        Assert.Equal(BoundedStack<int>.PeekOk, stack.GetPeekStatus());
        Assert.Equal(1, peek1);

        // stack has no elements
        stack.Clear();
        var peek2 = stack.Peek();

        Assert.Equal(BoundedStack<int>.PeekErr, stack.GetPeekStatus());
        Assert.Equal(default(int), peek2);
    }
}