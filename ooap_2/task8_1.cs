class Food { };
class Mouse : Food {};

// исходный класс
class Animal
{
    private const string DefaultAnimalName = "Animal";
    public virtual string Name { get; init; } = DefaultAnimalName;

    public virtual Food Hunt()
    {
        return new Food();
    }
}

// класс-наследник изменяет поведение и возвращает подтип от исходного возвращаемого типа
class Cat : Animal
{
    private const string DefaultCatName = "Cat";
    public override string Name { get; init; } = DefaultCatName;

    public override Mouse Hunt()
    {
        return new Mouse();
    }
}

// ключевое слово in разрешает принимать указанный тип только на вход  
interface IPetDoctor<in T>
{
    public void Heal(T pet);
}

class CatDoctor : IPetDoctor<Cat>
{
    public void Heal(Cat cat)
    {
        Console.WriteLine($"Healing a cat with name: {cat.Name}.");
    }
}

class AnimalDoctor : IPetDoctor<Animal>
{
    public void Heal(Animal animal)
    {
        Console.WriteLine($"Healing an animal with name: {animal.Name}.");
    }
}

class Program
{
    public static void Main()
    {
        // пример ковариантности
        // класс Cat наследник Animal возвращает класс Mouse наследник Food, который возвращает Animal
        var cat = new Cat();
        Mouse catFood = cat.Hunt();

        var animal = new Animal();
        Food animalFood = animal.Hunt();

        // пример контрвариантности
        // переменная класса IPetDoctor<Animal> присвоена переменной типа IPetDoctor<Cat>, тк является частным случаем (наследник) IPetDoctor<Cat> 
        IPetDoctor<Cat> catDoctor = new CatDoctor();
        IPetDoctor<Animal> animalDoctor = new AnimalDoctor();
        IPetDoctor<Cat> anotherCatDoctor = animalDoctor;

        catDoctor.Heal(cat);
        anotherCatDoctor.Heal(cat);
    }
}