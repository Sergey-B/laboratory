using Task9;
using Xunit;

public class PowerSetTests
{
    private Task9.PowerSet<string> _powerSet;
    private int _size;

    public PowerSetTests()
    {
        _size = 20_000;
        _powerSet = new Task9.PowerSet<string>(_size);
    }

    [Fact]
    public void ConstructorShouldCreateNewEmptyStorage()
    {
        // Arrange
        var size = 16;

        // Act
        var powerSet = new Task9.PowerSet<string>(size);

        // Assert
        Assert.Equal(0, powerSet.Count());
        Assert.Equal(size, powerSet.Size());
    }

    [Fact]
    public void Put_Should_InsertNonExistingElement()
    {
        // Arrange
        // Act
        _powerSet.Put("value");
        var putStatus = _powerSet.GetPutStatus();

        // Assert
        Assert.Equal(1, _powerSet.Count());
        Assert.Equal(PowerSetAtd<string>.PutOk, putStatus);
    }
    
    [Fact]
    public void Put_ShouldNot_InsertExistingElement()
    {
        // Arrange
        _powerSet.Put("value");

        // Act
        _powerSet.Put("value");
        var putStatus = _powerSet.GetPutStatus();

        // Assert
        Assert.Equal(1, _powerSet.Count());
        Assert.Equal(PowerSetAtd<string>.PutErr, putStatus);
    }
    
    [Fact]
    public void Remove_Should_Remove_ExistingElement()
    {
        // Arrange
        _powerSet.Put("value");

        // Act
        _powerSet.Remove("value");
        var removeStatus = _powerSet.GetRemoveStatus();

        // Assert
        Assert.Equal(0, _powerSet.Count());
        Assert.Equal(PowerSetAtd<string>.RemoveOk, removeStatus);
    }
    
    [Fact]
    public void Remove_Should_ReturnError_WhenNonExistingElement()
    {
        // Arrange
        // Act
        _powerSet.Remove("value");
        var removeStatus = _powerSet.GetRemoveStatus();

        // Assert
        Assert.Equal(0, _powerSet.Count());
        Assert.Equal(PowerSetAtd<string>.RemoveErr, removeStatus);
    }
    
    [Fact]
    public void Get_Should_ReturnTrueForExistingElement()
    {
        // Arrange
        _powerSet.Put("value");

        // Act
        var result = _powerSet.Get("value");
        var getStatus = _powerSet.GetGetStatus();

        // Assert
        Assert.Equal(1, _powerSet.Count());
        Assert.True(result);
        Assert.Equal(PowerSetAtd<string>.GetOk, getStatus);
    }
    
    [Fact]
    public void Get_Should_ReturnFalseForNonExistingElement()
    {
        // Arrange
        // Act
        var result = _powerSet.Get("value");
        var getStatus = _powerSet.GetGetStatus();

        // Assert
        Assert.Equal(0, _powerSet.Count());
        Assert.False(result);
        Assert.Equal(PowerSetAtd<string>.GetOk, getStatus);
    }
    
    [Fact]
    public void Union_Should_ReturnMergeSetsWhenNonEmpty()
    {
        // Arrange
        var set2 = new PowerSet<string>(10);
        set2.Put("2");
        _powerSet.Put("1");
        
        // Act
        var result = _powerSet.Union(set2);
        var unionStatus = _powerSet.GetUnionStatus();

        // Assert
        Assert.Equal(1, _powerSet.Count());
        Assert.Equal(1, set2.Count());
        Assert.Equal(2, result.Count());
        Assert.True(result.Get("1"));
        Assert.True(result.Get("2"));
        Assert.Equal(PowerSetAtd<string>.UnionOk, unionStatus);
    }
    
    [Fact]
    public void Union_Should_ReturnMergeSetsWhenSingleSetIsEmpty()
    {
        // Arrange
        var set2 = new PowerSet<string>(10);
        _powerSet.Put("1");

        // Act
        var result = _powerSet.Union(set2);
        var unionStatus = _powerSet.GetUnionStatus();

        // Assert
        Assert.Equal(1, _powerSet.Count());
        Assert.Equal(0, set2.Count());
        Assert.Equal(1, result.Count());
        Assert.True(result.Get("1"));
        Assert.Equal(PowerSetAtd<string>.UnionOk, unionStatus);
    }
    
    [Fact]
    public void Intersection_ShouldReturnResult()
    {
        // Arrange
        var set2 = new PowerSet<string>(10);
        set2.Put("2");
        set2.Put("3");
        _powerSet.Put("1");
        _powerSet.Put("2");

        // Act
        var result = _powerSet.Intersection(set2);
        var intersectionStatus = _powerSet.GetIntersectionStatus();

        // Assert
        Assert.Equal(2, _powerSet.Count());
        Assert.Equal(2, set2.Count());
        Assert.Equal(1, result.Count());
        Assert.True(result.Get("2"));
        Assert.Equal(PowerSetAtd<string>.IntersectionOk, intersectionStatus);
    }
    
    [Fact]
    public void Intersection_ShouldReturnEmptySetWhenNoIntersectedElements()
    {
        // Arrange
        var set2 = new PowerSet<string>(10);
        set2.Put("2");
        set2.Put("3");
        _powerSet.Put("4");
        _powerSet.Put("5");

        // Act
        var result = _powerSet.Intersection(set2);
        var intersectionStatus = _powerSet.GetIntersectionStatus();

        // Assert
        Assert.Equal(2, _powerSet.Count());
        Assert.Equal(2, set2.Count());
        Assert.Equal(0, result.Count());
        Assert.False(result.Get("2"));
        Assert.Equal(PowerSetAtd<string>.IntersectionOk, intersectionStatus);
    }
    
    [Fact]
    public void Difference_Should_ReturnResult()
    {
        // Arrange
        var set2 = new PowerSet<string>(10);
        set2.Put("2");
        set2.Put("3");
        _powerSet.Put("4");
        _powerSet.Put("5");

        // Act
        var result = _powerSet.Difference(set2);
        var differenceStatus = _powerSet.GetDifferenceStatus();

        // Assert
        Assert.Equal(2, set2.Count());
        Assert.Equal(2, _powerSet.Count());
        Assert.Equal(2, result.Count());
        Assert.True(result.Get("4"));
        Assert.True(result.Get("5"));
        Assert.Equal(PowerSetAtd<string>.DifferenceOk, differenceStatus);
    }
    
    [Fact]
    public void Difference_Should_ReturnEmptySetWhenNoDiffElements()
    {
        // Arrange
        var set2 = new PowerSet<string>(10);
        set2.Put("2");
        set2.Put("3");
        _powerSet.Put("3");
        _powerSet.Put("2");

        // Act
        var result = _powerSet.Difference(set2);
        var differenceStatus = _powerSet.GetDifferenceStatus();

        // Assert
        Assert.Equal(2, set2.Count());
        Assert.Equal(2, _powerSet.Count());
        Assert.Equal(0, result.Count());
        Assert.False(result.Get("2"));
        Assert.False(result.Get("3"));
        Assert.Equal(PowerSetAtd<string>.DifferenceOk, differenceStatus);
    }
    
    [Fact]
    public void IsSubset_Should_ReturnResultWhenCurrentSetIsSubsetOfSet2()
    {
        // Arrange
        var set2 = new PowerSet<string>(10);
        set2.Put("1");
        set2.Put("2");
        set2.Put("3");
        _powerSet.Put("3");
        _powerSet.Put("2");

        // Act
        var result = _powerSet.IsSubset(set2);
        var isSubsetStatus = _powerSet.GetIsSubsetStatus();

        // Assert
        Assert.Equal(3, set2.Count());
        Assert.Equal(2, _powerSet.Count());
        Assert.True(result);
        Assert.Equal(PowerSetAtd<string>.IsSubsetOk, isSubsetStatus);
    }
    
    [Fact]
    public void IsSubset_Should_ReturnResultWhenSet2IsSubsetOfCurrent()
    {
        // Arrange
        var set2 = new PowerSet<string>(10);
        set2.Put("1");
        set2.Put("2");
        _powerSet.Put("3");
        _powerSet.Put("2");
        _powerSet.Put("1");

        // Act
        var result = _powerSet.IsSubset(set2);
        var isSubsetStatus = _powerSet.GetIsSubsetStatus();

        // Assert
        Assert.Equal(2, set2.Count());
        Assert.Equal(3, _powerSet.Count());
        Assert.False(result);
        Assert.Equal(PowerSetAtd<string>.IsSubsetOk, isSubsetStatus);
    }
    
    [Fact]
    public void IsSubset_Should_ReturnResultWhenSomeElementsOfSet2ExistsInCurrent()
    {
        // Arrange
        var set2 = new PowerSet<string>(10);
        set2.Put("1");
        _powerSet.Put("3");
        _powerSet.Put("2");
        _powerSet.Put("1");

        // Act
        var result = _powerSet.IsSubset(set2);
        var isSubsetStatus = _powerSet.GetIsSubsetStatus();

        // Assert
        Assert.Equal(1, set2.Count());
        Assert.Equal(3, _powerSet.Count());
        Assert.False(result);
        Assert.Equal(PowerSetAtd<string>.IsSubsetOk, isSubsetStatus);
    }
    
    [Fact]
    public void Equals_Should_ReturnTrueWhenBothEquals()
    {
        // Arrange
        var set2 = new PowerSet<string>(10);
        set2.Put("1");
        set2.Put("2");
        _powerSet.Put("2");
        _powerSet.Put("1");

        // Act
        var result = _powerSet.Equals(set2);
        var equalsStatus = _powerSet.GetEqualsStatus();

        // Assert
        Assert.Equal(2, set2.Count());
        Assert.Equal(2, _powerSet.Count());
        Assert.True(result);
        Assert.Equal(PowerSetAtd<string>.EqualsOk, equalsStatus);
    }
    
    [Fact]
    public void Equals_Should_ReturnFalseWhenDifferentElements()
    {
        // Arrange
        var set2 = new PowerSet<string>(10);
        set2.Put("1");
        set2.Put("3");
        _powerSet.Put("2");
        _powerSet.Put("1");

        // Act
        var result = _powerSet.Equals(set2);
        var equalsStatus = _powerSet.GetEqualsStatus();

        // Assert
        Assert.Equal(2, set2.Count());
        Assert.Equal(2, _powerSet.Count());
        Assert.False(result);
        Assert.Equal(PowerSetAtd<string>.EqualsOk, equalsStatus);
    }
}