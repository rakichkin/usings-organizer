using System.Collections.Generic;
using System.Linq;


namespace UsingsOrganizer;

public static class ListExtensions
{
	public static List<T> GetAndRemove<T>(this List<T> list, Predicate<T> selector)
	{
		var targetItems = list.Where(selector.Invoke).ToList();
		list.RemoveAll(selector);
		return targetItems;
	}
}
