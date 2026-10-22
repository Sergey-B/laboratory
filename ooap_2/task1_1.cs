// реализует композицию Items
internal class Order
{
    public bool IsPaid { get; internal set; }
    public int Amount { get; internal set; }
    public int RetryCount { get; internal set; }
    private List<OrderItem> _items = [];
    public IReadOnlyCollection<OrderItem>? Items { get; internal set; } = _items.AsReadOnly();

    public bool IsValid()
    {
        if (IsPaid)
        {
            return true;
        }

        if (RetryCount > 3)
        {
            return false;
        }

        if (Items!.Any(item => item.Price < 0))
        {
            return false;
        }

        return false;
    }

    public void AddItem(int price)
    {
        var newItem = new OrderItem { Price = price };
        _items.Add(newItem);
    }
}

internal class OrderItem
{
    public int Price { get; internal set; }
}

internal abstract class Delivery
{
    public Order Order { get; init; }

    public virtual string GetStatus()
    {
        return "Normal Processing";
    }
}

// реализует наследование и полиморфизм подтипов.
internal class PriorityDelivery : Delivery
{
    public override string GetStatus()
    {
        return "Priority Express";
    }
}

// реализует наследование и полиморфизм подтипов.
internal class StandardDelivery : Delivery
{
    public override string GetStatus()
    {
        return "Standard Express";
    }
}