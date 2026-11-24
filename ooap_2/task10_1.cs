// В C# есть возможность запрета переопределения методов в потомках.

// По умолчанию все методы в классе запрещено переопределять, чтобы разрешить переопределение нужно явно добавить virtual или abstract к методу.
public abstract class BaseClass
{
    public virtual void TestMethod()
    {
        Console.WriteLine("Invoked TestMethod.");
    }

    public abstract void AbstractMethod();
}

public abstract class ChildClass : BaseClass
{
    public override void TestMethod()
    {
        Console.WriteLine("Invoked TestMethod from ChildClass.");
    }   
}

// Также можно запретить переопределение метода в наследниках использованием ключевого слова sealed.
public abstract class AnotherChildClass : BaseClass
{
    // Наследники AnotherChildClass не смогут переопределить TestMethod.
    public sealed override void TestMethod()
    {
        Console.WriteLine("Invoked TestMethod from AnotherChildClass.");
    }
}

// Также есть полный запрет наследования класса с помощью ключевого слова sealed.
public sealed class SealedClass
{

}

