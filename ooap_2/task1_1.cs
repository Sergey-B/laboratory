class Animal
{
    public virtual string Move()
    {
        return "Move";
    }
}

class NyanCat : Animal
{
    // Специализация, так как изменяется поведение базового класса.
    public override string Move()
    {
        return $"{base.Move()} and Jump";
    }

    // Расширение, так как поведение дополняется новым методом, который есть только в этом классе.
    public string Meow()
    {
        return "Meow";
    }

    // Расширение, так как поведение дополняется новой функциональностью.
    public string Fly()
    {
        return "Fly";
    }
}