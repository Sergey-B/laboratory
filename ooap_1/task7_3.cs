using Task7;
using Xunit;

public class HashTableTests
{
    private Task7.HashTable _hashTable;
    private int _size;

    public HashTableTests()
    {
        _size = 17;
        _hashTable = new Task7.HashTable(_size);
    }
    
    [Fact]
    public void ConstructorShouldCreateNewEmptyStorage()
    {
        // Arrange
        var size = 16;

        // Act
        var hashTable = new Task7.HashTable(size);

        // Assert
        Assert.Equal(0, hashTable.Count());
        Assert.Equal(size, hashTable.Size());
    }

    [Fact]
    public void AddShouldAddNewElementToStorage()
    {
        // Arrange
        // Act
        _hashTable.Add("key", "value");
        var addStatus = _hashTable.GetAddStatus();

        // Assert
        Assert.Equal(HashTableAtd.AddOk, addStatus);
    }

    [Fact]
    public void AddShouldReturnOverwriteExistingKeyIfElementAlreadyExistsInStorage()
    {
        // Arrange
        // Act
        _hashTable.Add("key1", "value1");
        _hashTable.Add("key1", "value2");
        var addStatus = _hashTable.GetAddStatus();
        var value = _hashTable.Get("key1");

        // Assert
        Assert.Equal("value2", value);
        Assert.Equal(HashTableAtd.AddOk, addStatus);
    }

    [Fact]
    public void RemoveShouldRemoveElementByGivenKey()
    {
        // Arrange
        _hashTable.Add("key", "value");

        var resultBeforeRemoval = _hashTable.Exists("key");

        // Act
        _hashTable.Remove("key");
        var removeStatus = _hashTable.GetRemoveStatus();
        var resultAfterRemoval = _hashTable.Exists("key");

        // Assert
        Assert.True(resultBeforeRemoval);
        Assert.Equal(HashTableAtd.RemoveOk, removeStatus);
        Assert.False(resultAfterRemoval);
    }

    [Fact]
    public void RemoveShouldReturnErrorIfElementWithGivenValueNotExists()
    {
        // Arrange
        var resultBeforeRemoval = _hashTable.Exists("first");

        // Act
        _hashTable.Remove("first");
        var removeStatus = _hashTable.GetRemoveStatus();
        var resultAfterRemoval = _hashTable.Exists("first");

        // Assert
        Assert.False(resultBeforeRemoval);
        Assert.Equal(HashTableAtd.RemoveErr, removeStatus);
        Assert.False(resultAfterRemoval);
    }

    [Fact]
    public void ExistsShouldReturnTrueIfElementWithGivenKeyExists()
    {
        // Arrange
        _hashTable.Add("key", "value");

        // Act
        var result = _hashTable.Exists("key");
        var existsStatus = _hashTable.GetExistsStatus();

        // Assert
        Assert.True(result);
        Assert.Equal(HashTableAtd.ExistsOk, existsStatus);
    }

    [Fact]
    public void ExistsShouldReturnFalseIfElementWithGivenKeyNotExists()
    {
        // Arrange
        // Act
        var result = _hashTable.Exists("key");
        var existsStatus = _hashTable.GetExistsStatus();

        // Assert
        Assert.False(result);
        Assert.Equal(HashTableAtd.ExistsErr, existsStatus);
    }

    [Fact]
    public void GetShouldReturnElementByGivenValue()
    {
        // Arrange
        _hashTable.Add("key", "value");

        // Act
        var item = _hashTable.Get("key");
        var getStatus = _hashTable.GetGetStatus();

        // Assert
        Assert.Equal("value", item);
        Assert.Equal(HashTableAtd.GetOk, getStatus);
    }

    [Fact]
    public void GetShouldReturnErrorIfGivenValueNotExistsInStorage()
    {
        // Arrange
        // Act
        var item = _hashTable.Get("key");
        var getStatus = _hashTable.GetGetStatus();

        // Assert
        Assert.Equal(default, item);
        Assert.Equal(HashTableAtd.GetErr, getStatus);
    }
}