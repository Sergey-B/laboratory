using Task4;
using Xunit;

public class DynArrayTests
{
    public DynArrayTests()
    {
    }

    [Fact]
    public void ConstructorShouldReturnNewEmptyArray()
    {
        // Arrange
        // Act
        var array = new DynArray<int>();

        // Assert
        Assert.Equal(0, array.Size());
        Assert.Equal(DynArrayAtd<int>.DefaultCapacity, array.Length());
    }

    // --добавление элемента, когда в итоге размер буфера не превышен;
    [Fact]
    public void AddShouldAppendNewItemToEndOfArray()
    {
        // Arrange
        var array = new DynArray<int>();

        // Act
        array.Add(1);
        var _addStatus = array.GetAddStatus();

        // Assert
        Assert.Equal(DynArray<int>.AddOk, _addStatus);
        Assert.Equal(1, array.Size());
        Assert.Equal(DynArrayAtd<int>.DefaultCapacity, array.Length());
    }

    // --добавление элемента, когда в результате превышен размер буфера;
    [Fact]
    public void AddShouldAppendNewItemWhenArrayBufferSizeExceeded()
    {
        // Arrange
        var array = new DynArray<int>();

        // Act
        array.Add(1);
        array.Add(2);
        array.Add(3);
        array.Add(4);
        array.Add(5);
        var _addStatus = array.GetAddStatus();

        // Assert
        Assert.Equal(DynArray<int>.AddOk, _addStatus);
        Assert.Equal(5, array.Size());
        Assert.Equal(8, array.Length());
    }

    // --вставка элемента, когда в итоге размер буфера не превышен;
    // --попытка вставки элемента в недопустимую позицию;
    [Theory]
    [InlineData(0, 0, DynArray<int>.InsertOk, 1, DynArray<int>.DefaultCapacity)]
    [InlineData(3, 3, DynArray<int>.InsertOk, 1, DynArray<int>.DefaultCapacity)]
    [InlineData(4, 4, DynArray<int>.InsertOk, 1, 8)]
    [InlineData(0, -1, DynArray<int>.InsertErr, 0, DynArray<int>.DefaultCapacity)]
    public void InsertShouldAddNewElementAtGivenIndex(int item, int index, int expectedInsertStatus, int expectedSize, int expectedCapacity)
    {
        // Arrange
        var array = new DynArray<int>();

        // Act
        array.Insert(item, index);
        var _insertStatus = array.GetInsertStatus();

        // Assert
        Assert.Equal(expectedInsertStatus, _insertStatus);
        Assert.Equal(expectedSize, array.Size());
        Assert.Equal(expectedCapacity, array.Length());
    }

    // --вставка элемента, когда в результате превышен размер буфера;
    [Theory]
    [InlineData(0, 0, DynArray<int>.InsertOk, 5, 8)]
    [InlineData(3, 3, DynArray<int>.InsertOk, 5, 8)]
    [InlineData(4, 4, DynArray<int>.InsertOk, 5, 8)]
    [InlineData(0, 17, DynArray<int>.InsertOk, 5, 34)]
    [InlineData(0, -1, DynArray<int>.InsertErr, 4, 4)] // !
    public void InsertShouldAddNewElementAtGivenIndexWhenBufferSizeExceeded(int item, int index, int expectedInsertStatus, int expectedSize, int expectedCapacity)
    {
        // Arrange
        var array = new DynArray<int>();
        array.Add(1);
        array.Add(2);
        array.Add(3);
        array.Add(4);

        // Act
        array.Insert(item, index);
        var _insertStatus = array.GetInsertStatus();

        // Assert
        Assert.Equal(expectedInsertStatus, _insertStatus);
        Assert.Equal(expectedSize, array.Size());
        Assert.Equal(expectedCapacity, array.Length());
    }

    // --удаление элемента, когда в результате размер буфера остаётся прежним;
    // --удаление элемента, когда в результате понижается размер буфера;
    // --попытка удаления элемента в недопустимой позиции.
    [Theory]
    [InlineData(0, DynArray<int>.RemoveOk, 3, 4)]
    [InlineData(3, DynArray<int>.RemoveOk, 3, 4)]
    [InlineData(4, DynArray<int>.RemoveErr, 4, 4)]
    [InlineData(17, DynArray<int>.RemoveErr, 4, 4)] // !
    [InlineData(-1, DynArray<int>.RemoveErr, 4, 4)] // !

    public void RemoveShouldRemoveElementAndChangeCapacity(int index, int expectedRemoveStatus, int expectedSize, int expectedCapacity)
    {
        // Arrange
        var array = new DynArray<int>();
        array.Add(1);
        array.Add(2);
        array.Add(3);
        array.Add(4);

        // Act
        array.Remove(index);
        var removeStatus = array.GetRemoveStatus();

        // Assert
        Assert.Equal(expectedRemoveStatus, removeStatus);
        Assert.Equal(expectedSize, array.Size());
        Assert.Equal(expectedCapacity, array.Length());
    }

    [Theory]
    [InlineData(0, DynArray<int>.RemoveOk, 7, 8)]
    [InlineData(3, DynArray<int>.RemoveOk, 7, 8)]
    [InlineData(4, DynArray<int>.RemoveOk, 7, 8)]
    [InlineData(17, DynArray<int>.RemoveErr, 8, 8)] // !
    [InlineData(-1, DynArray<int>.RemoveErr, 8, 8)] // !
    public void RemoveShouldRemoveElementAndChangeCapacityWhenBufferExceeded(int index, int expectedRemoveStatus, int expectedSize, int expectedCapacity)
    {
        // Arrange
        var array = new DynArray<int>();
        array.Add(1);
        array.Add(2);
        array.Add(3);
        array.Add(4);
        array.Add(5);
        array.Add(6);
        array.Add(7);
        array.Add(8);

        // Act
        array.Remove(index);
        var removeStatus = array.GetRemoveStatus();

        // Assert
        Assert.Equal(expectedRemoveStatus, removeStatus);
        Assert.Equal(expectedSize, array.Size());
        Assert.Equal(expectedCapacity, array.Length());
    }

    // --попытка передачи капасити меньше тукущего.
    // --попытка передачи капасити меньше размера текущего массива.
    // --попытка передачи допустимого капасити.
    [Theory]
    [InlineData(0, DynArray<int>.MakeArrayErr, 4)]
    [InlineData(-1, DynArray<int>.MakeArrayErr, 4)]
    [InlineData(3, DynArray<int>.MakeArrayOk, 3)]
    [InlineData(5, DynArray<int>.MakeArrayOk, 5)]
    [InlineData(16, DynArray<int>.MakeArrayOk, 16)]
    public void MakeArrayShouldChangeArrayCapacity(int capacity, int expectedMakeArrayStatus, int expectedCapacity)
    {
        // Arrange
        var array = new DynArray<int>();

        // Act
        array.MakeArray(capacity);
        var _makeArrayStatus = array.GetMakeArrayStatus();

        // Assert
        Assert.Equal(expectedCapacity, array.Length());
        Assert.Equal(expectedMakeArrayStatus, _makeArrayStatus);
    }

    // --попытка передачи капасити меньше размера текущего массива.
    // --попытка передачи допустимого капасити.
    [Theory]
    [InlineData(0, DynArray<int>.MakeArrayErr, 8)]
    [InlineData(-1, DynArray<int>.MakeArrayErr, 8)]
    [InlineData(3, DynArray<int>.MakeArrayErr, 8)]
    [InlineData(5, DynArray<int>.MakeArrayOk, 5)]
    [InlineData(16, DynArray<int>.MakeArrayOk, 16)]
    public void MakeArrayShouldChangeArrayCapacityWhenItMoreThenDefault(int capacity, int expectedMakeArrayStatus, int expectedCapacity)
    {
        // Arrange
        var array = new DynArray<int>();
        array.Add(0);
        array.Add(1);
        array.Add(2);
        array.Add(3);
        array.Add(4);

        // Act
        array.MakeArray(capacity);
        var _makeArrayStatus = array.GetMakeArrayStatus();

        // Assert
        Assert.Equal(expectedCapacity, array.Length());
        Assert.Equal(expectedMakeArrayStatus, _makeArrayStatus);
    }

    // --попытка получения элемента в допустимой позиции.
    // --попытка получения элемента в недопустимой позиции.
    [Theory]
    [InlineData(-1, default(int), DynArray<int>.GetErr)]
    [InlineData(17, default(int), DynArray<int>.GetErr)]
    [InlineData(0, 1, DynArray<int>.GetOk)]
    public void GetShouldReturnItemOnGivenIndex(int index, int expectedResult, int expectedGetStatus)
    {
        // Arrange
        var array = new DynArray<int>();
        array.MakeArray(4);
        array.Add(1);
        array.Add(2);
        array.Add(3);
        array.Add(4);

        // Act
        var item = array.Get(index);
        var getStatus = array.GetGetStatus();

        // Assert
        Assert.Equal(expectedGetStatus, getStatus);
        Assert.Equal(expectedResult, item);
    }

    // // --попытка получения элемента в пустом массиве.
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    public void GetShouldReturnDefaultWhenEmptyArray(int index)
    {
        // Arrange
        var array = new DynArray<int>();
        array.MakeArray(4);
        
        // Act
        var item = array.Get(index);
        var getStatus = array.GetGetStatus();

        // Assert
        Assert.Equal(DynArray<int>.GetErr, getStatus);
        Assert.Equal(default, item);
    }
}