public class Animal
{
    // родительский класс открывает метод для переопределения в наследниках,
    // с помощью добавления атрибута virtual
    public virtual void Speak()
    {
        Console.WriteLine("Animal says something.");
    }
}

public class Cat : Animal
{
    // класс-наследник переопределяет поведение родительского класса
    public override void Speak()
    {
        Console.WriteLine("Cat says meow.");
    }
}

public class Fox : Animal
{
    public override void Speak()
    {
        Console.WriteLine("Fox says ring-ding-ding.");
    }
}

public class Program
{
    public static void Main()
    {
        // инициализируется список обьектов типа Animal, список хранит ссылки на обьекты разного типа.
        var animals = new List<Animal>()
        {
            new Animal(),
            new Cat(),
            new Fox()
        };

        // вызываемый метод определяется в рантайме, в зависимости от типа обьекта
        foreach (var animal in animals)
        {
            animal.Speak();
        }
    }
}