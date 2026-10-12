namespace Task8;

public abstract class NativeDictionaryAtd<T>
{
  public const int PutNil = 0;
  public const int PutOk = 1;
  public const int PutErr = 2;
  public const int HasKeyNil = 0;
  public const int HasKeyOk = 1;
  public const int HasKeyErr = 2;
  public const int GetNil = 0;
  public const int GetOk = 1;
  public const int GetErr = 2;

  // конструктор
  // постусловие: создана новое пустое хранилище.
  public NativeDictionaryAtd(int size)
  {
  }

  // команды
  // постусловие: добавляет значение для ключа или переписываеть текущее значение новым значением.
  public abstract void Put(string key, T value);

  // запросы
  // постусловие: вернулось булево значение, указывающее имеется ли в хранилище указанное значение.
  public abstract bool HasKey(string key);

  // предусловие: хранилище не пустое. 
  // постусловие: возвращает значение по данному ключу или null если не найден
  public abstract T? Get(string key);

  // постусловие:  возвращает размер хранилища.
  public abstract int Size();

  // постусловие:  возвращает текущее количество элементво в хранилище.
  public abstract int Count();

  // запросы статусов
  public abstract int GetPutStatus(); // возвращает значение Put*.
  public abstract int GetHasKeyStatus(); // возвращает значение HasKey*.
  public abstract int GetGetStatus(); // возвращает значение Get*
}

public class NativeDictionary<T> : NativeDictionaryAtd<T>
{
    private int _size;
    private int _count;
    private string[] _keys;
    private T[] _values;
    private int _putStatus;
    private int _hasKeyStatus;
    private int _getStatus;

	public NativeDictionary(int size) : base(size)
	{
		_size = size;
		_count = 0;
		_keys = new string[_size];
		_values = new T[size];

		_putStatus = PutNil;
		_hasKeyStatus = HasKeyNil;
		_getStatus = GetNil;
	}
	
	// O(logn)
	public override T? Get(string key)
	{
		// вернуть ошибку если хранилище пустое
		if (Count() == 0)
		{
			_getStatus = GetErr;

			return default;
		}

		// получить индекс ключа, если он отрицательный значит ключа нет в хранилище
		var index = HashFun(key);
		if (index >= 0)
		{
			var value = _values[index];
			_getStatus = GetOk;

			return value;
		}
		else
		{
			_getStatus = GetErr;

			return default;
		}
	}

	// O(logn)
    public override bool HasKey(string key)
	{
		// вернуть ошибку если хранилище пустое
		if (Count() == 0)
		{
			_hasKeyStatus = HasKeyErr;

			return false;
		}

		// получить индекс ключа, если он отрицательный значит ключа нет в хранилище
		var index = HashFun(key);
		if (index >= 0)
		{
			_hasKeyStatus = HasKeyOk;

			return true;
		}
		else
		{
			_hasKeyStatus = HasKeyErr;

			return false;
		}
    }

	// когда нет ключа O(logn), в худшем случае O(n)
	public override void Put(string key, T value)
	{
		// получить индекс ключа, если он отрицательный значит ключа нет в хранилище
		var index = HashFun(key);

		// ключа нет в хранилище
		if (index < 0)
		{
			index = ~index;

			// сделать resize если хранилище полное
			if (Count() >= Size())
			{
				Resize(Count() == 0 ? 4 : Count() * 2);
			}

			// сдвинуть элементы вправо
			for (var i = _count; i > index; i--)
			{
				_keys[i] = _keys[i - 1];
				_values[i] = _values[i - 1];
			}

			// вставить элемент по индексу
			_keys[index] = key;
			_values[index] = value;
			_count++;
		}
		else // ключ найден
		{
			// вставить элемент по индексу
			_values[index] = value;
		}

		_putStatus = PutOk;
	}

    public override int Size()
	{
		return _size;
	}

	public override int Count()
	{
		return _count;
	}

	public override int GetGetStatus()
	{
		return _getStatus;
	}

	public override int GetHasKeyStatus()
	{
		return _hasKeyStatus;
	}

	public override int GetPutStatus()
	{
		return _putStatus;
	}

	//  возвращает индекс ключа, если ключ не найден возвращается отрицательное значение
	private int HashFun(string key)
	{
		return Array.BinarySearch(_keys, 0, _count, key);
	}

	//  изменяет размер хранилища
	private void Resize(int newSize)
	{
		// скопировать keys в новое хранилище
		var keysBuffer = _keys;
		_keys = new string[newSize];
		Array.Copy(keysBuffer, _keys, Count());

		// скопировать values в новое хранилище
		var valuesBuffer = _values;
		_values = new T[newSize];
		Array.Copy(valuesBuffer, _values, Count());

		// установить новое значение размера хранилища
		_size = newSize;
	}
}