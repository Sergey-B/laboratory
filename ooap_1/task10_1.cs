using System.Collections.Generic;
using System.Collections;
using System;
using System.IO;
using System.Drawing;

namespace Task10;

public abstract class BloomFilterAtd<T>
{
	public const int AddNil = 0;
	public const int AddOk = 1;
	public const int AddErr = 2;
	public const int IsValueNil = 0;
	public const int IsValueOk = 1;
	public const int IsValueErr = 2;

	protected int size;

	// конструктор
	// постусловие: создано пустое хранилище заданного размера
	public BloomFilterAtd(int size)
	{
	}
	
	// команды
	// постусловие: в фильтр добавлен новый элемент;
	public abstract void Add(string str); // добавляем строку str в фильтр

	// запросы
	public abstract bool IsValue(string str); // проверка, имеется ли строка str1 в фильтре
											  // хэш-функции
	public abstract int Hash1(string str);

	public abstract int Hash2(string str);
	public abstract int Size(); // текущий размер фильтра

	// запросы статустов
	public abstract int GetAddStatus(); // возвращает значение Add*.
	public abstract int GetIsValueStatus(); // возвращает значение IsValue*.
}

public class BloomFilter : BloomFilterAtd<string>
{
	public const int DefaultFilterSize = 32;

	private int _addStatus;
	private int _isValueStatus;
	private int _size;
	private int _bits;

	// скрытые поля используемы для вычисления хэш-кода
	private readonly int _a;
	private readonly int _b;

	// конструктор
	// постусловие: создано пустое хранилище заданного размера
	public BloomFilter(int size = DefaultFilterSize) : base(size)
	{
		_size = size;
		_bits = 0;

		_addStatus = AddNil;
		_isValueStatus = IsValueNil;

		// инициализация переменных для вычисления хэш-кода
		_a = 17;
		_b = 223;
	}

	// команды
	// постусловие: в фильтр добавлен новый элемент; 
	public override void Add(string str)
	{
		var index1 = Hash1(str);
		var index2 = Hash2(str);

		_bits |= 1 << index1;
		_bits |= 1 << index2;

		_addStatus = AddOk;
	}

	// запросы
	public override bool IsValue(string str)
	{
		var index1 = Hash1(str);
		var index2 = Hash2(str);

		_isValueStatus = IsValueOk;

		var bit1 = (_bits & (1 << index1)) != 0;
		var bit2 = (_bits & (1 << index2)) != 0;

		return bit1 && bit2;
	}

	public override int Hash1(string str)
	{
		var hash = 0;
		foreach (char c in str)
		{
			hash = (hash * _a) + c;// % _size;
		}

		return Math.Abs(hash) % _size;
	}

    public override int Hash2(string str)
    {
		var hash = 0;
		foreach (char c in str)
		{
			hash = (hash * _b) + c;// % _size;
		}

		return Math.Abs(hash) % _size;
	}

	public override int Size()
	{
		return _size;
	}

	public override int GetAddStatus()
	{
		return _addStatus;
	}

	public override int GetIsValueStatus()
	{
		return _isValueStatus;
	}
}