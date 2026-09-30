using Task3;
using Xunit;

public class TwoWayListTests
{
    [Fact]
    public void ConstructorShouldReturnNewEmptyList()
    {
        // Arrange
        var list = new TwoWayList<int>();

        // Assert
        Assert.IsType<TwoWayList<int>>(list);
        Assert.Equal(0, list.Size());
    }

    [Fact]
    public void HeadShouldReturnFirstElement()
    {
        // Arrange
        var list = new TwoWayList<int>();
        list.AddToEmpty(1);

        // Act
        var headValue = list.Head();
        var headStatus = list.GetHeadStatus();

        // Assert
        Assert.Equal(TwoWayList<int>.HeadOk, headStatus);
        Assert.Equal(1, headValue);
    }

    [Fact]
    public void TailShouldReturnLastElement()
    {
        // Arrange
        var list = new TwoWayList<int>();
        list.AddToEmpty(1);

        // Act
        var tailValue = list.Tail();
        var tailStatus = list.GetTailStatus();

        // Assert
        Assert.Equal(TwoWayList<int>.TailOk, tailStatus);
        Assert.Equal(1, tailValue);
    }

    [Fact]
    public void RightShouldReturnElementRightToCurrent()
    {
        // Arrange
        var list = new TwoWayList<int>();
        list.AddToEmpty(1);
        list.PutRight(2);

        // Act
        var head = list.Head();
        var right = list.Right();
        var rightStatus = list.GetRightStatus();

        // Assert
        Assert.Equal(TwoWayList<int>.RightOk, rightStatus);
        Assert.Equal(2, right);
    }

    [Fact]
    public void PutRightShouldInsertElementRightToCurrent()
    {
        // Arrange
        var list = new TwoWayList<int>();
        list.AddToEmpty(1);

        // Act
        list.PutRight(2);
        var putRightStatus = list.GetPutRightStatus();

        // Assert
        Assert.Equal(TwoWayList<int>.PutRightOk, putRightStatus);
        Assert.Equal(2, list.Size());
    }

    [Fact]
    public void PutLeftShouldInsertElementLeftToCurrent()
    {
        // Arrange
        var list = new TwoWayList<int>();
        list.AddToEmpty(2);

        // Act
        list.PutLeft(1);
        var putLeftStatus = list.GetPutLeftStatus();

        // Assert
        Assert.Equal(TwoWayList<int>.PutLeftOk, putLeftStatus);
        Assert.Equal(2, list.Size());
    }

    [Fact]
    public void AddToEmptyShouldAddElementToList()
    {
        // Arrange
        var list = new TwoWayList<int>();

        // Act
        list.AddToEmpty(1);
        var addToEmptyStatus = list.GetAddToEmptyStatus();

        // Assert
        Assert.Equal(TwoWayList<int>.AddToEmptyOk, addToEmptyStatus);
        Assert.Equal(1, list.Size());
    }

    [Fact]
    public void AddToTailShouldAddElementToEndOfList()
    {
        // Arrange
        var list = new TwoWayList<int>();
        list.AddToEmpty(1);
        list.Right();
        list.PutRight(2);

        // Act
        list.AddToTail(3);
        var addToTailStatus = list.GetAddToTailStatus();
        var tail = list.Tail();

        // Assert
        Assert.Equal(TwoWayList<int>.AddToTailOk, addToTailStatus);
        Assert.Equal(3, list.Size());
        Assert.Equal(3, tail);
    }

    [Fact]
    public void RemoveAllShouldRemoveAllElementsFromList()
    {
        var list = new TwoWayList<int>();
        list.AddToEmpty(1);
        list.PutRight(2);
        list.AddToTail(2);

        // Act
        list.RemoveAll(2);
        var removeAllStatus = list.GetRemoveAllStatus();
        var head = list.Head();

        // Assert
        Assert.Equal(TwoWayList<int>.RemoveAllOk, removeAllStatus);
        Assert.Equal(1, list.Size());
        Assert.Equal(1, head);
    }

    [Fact]
    public void RemoveShouldRemoveCurrentElementFromList()
    {
        var list = new TwoWayList<int>();
        list.AddToEmpty(1);
        list.PutRight(2);

        // Act
        list.Right();
        list.Remove();
        var removeStatus = list.GetRemoveStatus();

        // Assert
        Assert.Equal(TwoWayList<int>.RemoveOk, removeStatus);
        Assert.Equal(1, list.Size());
    }

    [Fact]
    public void ClearShouldRemoveAllElementsFromList()
    {
        // Arrange
        var list = new TwoWayList<int>();
        list.AddToEmpty(1);
        list.PutRight(2);

        // Act
        list.Clear();

        // Assert
        Assert.Equal(0, list.Size());
    }

    [Fact]
    public void ReplaceShouldReplaceCurrentElementValue()
    {
        // Arrange
        var list = new TwoWayList<int>();
        list.AddToEmpty(1);
        list.PutRight(2);
        var current = list.Get();
        Assert.Equal(1, current);

        // Act
        list.Replace(2);
        var newCurrent = list.Get();
        var replaceStatus = list.GetReplaceStatus();


        // Assert
        Assert.Equal(TwoWayList<int>.ReplaceOk, replaceStatus);
        Assert.Equal(2, newCurrent);
    }

    [Fact]
    public void FindShouldFindFirstElementByValueAndSetAsCurrentElement()
    {
        // Arrange
        var list = new TwoWayList<int>();
        list.AddToEmpty(1);
        list.PutRight(2);
        list.Right();
        list.PutRight(3);

        // Act
        list.Find(2);
        var result = list.Get();
        var findStatus = list.GetFindStatus();

        // Assert
        Assert.Equal(TwoWayList<int>.FindOk, findStatus);
        Assert.Equal(2, result);
    }

    [Fact]
    public void GetShouldReturnElementValue()
    {
        /// Arrange
        var list = new TwoWayList<int>();
        list.AddToEmpty(1);

        // Act
        var result = list.Get();
        var getStatus = list.GetGetStatus();

        // Assert
        Assert.Equal(TwoWayList<int>.GetOk, getStatus);
        Assert.Equal(1, result);
    }

    [Fact]
    public void LeftShouldReturnLeftElementToCurrentAndMakeItCurrent()
    {
        // Arrange
        var list = new TwoWayList<int>();
        list.AddToEmpty(1);
        list.PutRight(2);
        list.AddToTail(3);

        // Act
        var head = list.Head();
        var right = list.Right();
        var left = list.Left();
        var leftStatus = list.GetLeftStatus();

        // Assert
        Assert.Equal(TwoWayList<int>.LeftOk, leftStatus);
        Assert.Equal(1, left);
    }
}
