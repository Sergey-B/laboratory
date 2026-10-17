using Task10;
using Xunit;

public class BloomFilterTests
{
    private Task10.BloomFilter _filter;
    private int _size;
    private string[] testStrings;

    public BloomFilterTests()
    {
        _filter = new Task10.BloomFilter();

        testStrings = [
            "0123456789",
            "1234567890",
            "2345678901",
            "3456789012",
            "4567890123",
            "5678901234",
            "6789012345",
            "7890123456",
            "8901234567",
            "9012345678"
        ];
    }

    [Fact]
    public void Constructor_ShouldCreateNewEmptyStorageWithDefaultSize()
    {
        // Arrange
        // Act
        var filter = new BloomFilter();

        // Assert
        Assert.Equal(BloomFilter.DefaultFilterSize, filter.Size());
    }

    [Fact]
    public void Constructor_ShouldCreateNewEmptyStorageWithGivenSize()
    {
        // Arrange
        var size = 17;

        // Act
        var filter = new BloomFilter(size);

        // Assert
        Assert.Equal(size, filter.Size());
    }

    [Theory]
    [InlineData("0123456789", true)]
    [InlineData("1234567890", true)]
    [InlineData("2345678901", true)]
    [InlineData("3456789012", true)]
    [InlineData("4567890123", true)]
    [InlineData("5678901234", true)]
    [InlineData("6789012345", true)]
    [InlineData("7890123456", true)]
    [InlineData("8901234567", true)]
    [InlineData("9012345678", true)]
    public void Add_ShouldInsertGivenElement(string item, bool expectedResult)
    {
        // Arrange
        var filter = new BloomFilter();

        for (var i = 5; i < 10; i++)
        {
            filter.Add(testStrings[i]);
        }

        // Act
        var result = filter.IsValue(item);
        var addStatus = filter.GetAddStatus();

        // Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(BloomFilter.AddOk, addStatus);
    }

    [Theory]
    [InlineData("0123456789", true)]
    [InlineData("1234567890", true)]
    [InlineData("2345678901", true)]
    [InlineData("3456789012", true)]
    [InlineData("4567890123", true)]
    [InlineData("5678901234", true)] // ложное срабатывание
    public void IsValue_ShouldReturnWhetherGivenElementPresentOrNot(string item, bool expectedResult)
    {
        // Arrange
        var filter = new BloomFilter();
        
        filter.Add("0123456789");
        filter.Add("1234567890");
        filter.Add("2345678901");
        filter.Add("3456789012");
        filter.Add("4567890123");

        // Act
        var result = filter.IsValue(item);
        var isValueStatus = filter.GetIsValueStatus();

        // Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(BloomFilter.IsValueOk, isValueStatus);
    }
}