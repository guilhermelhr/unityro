using System;
using System.Collections.Generic;

public static class ICollectionExtensions {

    public static bool IsEmpty<T>(this IList<T> list) => list.Count == 0;
}
