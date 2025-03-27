using System.Collections;
using System.Collections.Generic;

public class WeightedList<T> : IEnumerable
{
	private class WeightItem
	{
		public T Item;

		public int Weight;
	}

	private readonly List<WeightItem> _list = new List<WeightItem>();

	public void Add(T item, int weight)
	{
		_list.Add(new WeightItem
		{
			Item = item,
			Weight = weight
		});
	}

	public T Pick()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < _list.Count; i++)
		{
			for (int j = 0; j < _list[i].Weight; j++)
			{
				list.Add(i);
			}
		}
		return _list[list.Pick()].Item;
	}

	public IEnumerator<T> GetEnumerator()
	{
		return _list.ConvertAll((WeightItem i) => i.Item).GetEnumerator();
	}

	private IEnumerator GetEnumerator1()
	{
		return GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator1();
	}
}