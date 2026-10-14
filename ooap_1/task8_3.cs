using Task8;
using Xunit;

public class NativeDictionaryTests
{
    private Task8.NativeDictionary<string> _nativeDictionary;
    private int _size;

    public NativeDictionaryTests()
    {
        _size = 17;
        _nativeDictionary = new Task8.NativeDictionary<string>(_size);
    }
    
    [Fact]
    public void ConstructorShouldCreateNewEmptyStorage()
    {
        // Arrange
        var size = 16;

        // Act
        var NativeDictionary = new Task8.NativeDictionary<string>(size);

        // Assert
        Assert.Equal(0, NativeDictionary.Count());
        Assert.Equal(size, NativeDictionary.Size());
    }

    [Fact]
    public void PutShouldPutNewElementToStorage()
    {
        // Arrange
        // Act
        _nativeDictionary.Put("key", "value");
        var putStatus = _nativeDictionary.GetPutStatus();

        // Assert
        Assert.Equal(1, _nativeDictionary.Count());
        Assert.Equal(NativeDictionaryAtd<string>.PutOk, putStatus);
    }

    [Fact]
    public void PutShouldPutNewElementToStorageWithSizeEqualsTo0()
    {
        // Arrange
        var nativeDictionary = new NativeDictionary<string>(0);

        // Act
        nativeDictionary.Put("key", "value");
        var putStatus = nativeDictionary.GetPutStatus();

        // Assert
        Assert.Equal(1, nativeDictionary.Count());
        Assert.Equal(NativeDictionaryAtd<string>.PutOk, putStatus);
    }

    [Fact]
    public void PutShouldPutNewElementToStorageWithSizeEqualsTo1()
    {
        // Arrange
        var nativeDictionary = new NativeDictionary<string>(1);

        // Act
        nativeDictionary.Put("key", "value");
        var putStatus = nativeDictionary.GetPutStatus();

        // Assert
        Assert.Equal(1, nativeDictionary.Count());
        Assert.Equal(NativeDictionaryAtd<string>.PutOk, putStatus);
    }

    [Fact]
    public void PutShouldReturnOverwriteExistingKeyIfElementAlreadyExistsInStorage()
    {
        // Arrange
        // Act
        _nativeDictionary.Put("key1", "value1");
        _nativeDictionary.Put("key1", "value2");
        var putStatus = _nativeDictionary.GetPutStatus();
        var value = _nativeDictionary.Get("key1");

        // Assert
        Assert.Equal("value2", value);
        Assert.Equal(1, _nativeDictionary.Count());
        Assert.Equal(NativeDictionaryAtd<string>.PutOk, putStatus);
    }

    [Fact]
    public void HasKeyShouldReturnTrueIfGivenKeyExists()
    {
        // Arrange
        _nativeDictionary.Put("key", "value");

        // Act
        var result = _nativeDictionary.HasKey("key");
        var hasKeyStatus = _nativeDictionary.GetHasKeyStatus();

        // Assert
        Assert.True(result);
        Assert.Equal(NativeDictionaryAtd<string>.HasKeyOk, hasKeyStatus);
    }

    [Fact]
    public void HasKeyShouldReturnFalseIfElementWithGivenKeyNotExists()
    {
        // Arrange
        // Act
        var result = _nativeDictionary.HasKey("key");
        var hasKeyStatus = _nativeDictionary.GetHasKeyStatus();

        // Assert
        Assert.False(result);
        Assert.Equal(NativeDictionaryAtd<string>.HasKeyErr, hasKeyStatus);
    }

    [Fact]
    public void GetShouldReturnElementByGivenKey()
    {
        // Arrange
        _nativeDictionary.Put("key", "value");

        // Act
        var item = _nativeDictionary.Get("key");
        var getStatus = _nativeDictionary.GetGetStatus();

        // Assert
        Assert.Equal("value", item);
        Assert.Equal(NativeDictionaryAtd<string>.GetOk, getStatus);
    }

    [Fact]
    public void GetShouldReturnErrorIfGivenValueNotExistsInStorage()
    {
        // Arrange
        // Act
        var item = _nativeDictionary.Get("key");
        var getStatus = _nativeDictionary.GetGetStatus();

        // Assert
        Assert.Equal(default, item);
        Assert.Equal(NativeDictionaryAtd<string>.GetErr, getStatus);
    }
}