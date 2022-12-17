namespace Application.Common;

public static class CollectionUtils
{
    public static bool HasDuplicates<T>(IEnumerable<T> collection)
    {
        var l = collection.ToList();
        return new HashSet<T>(l).Count != l.Count;
    }
}