using System.Collections.Concurrent;
using System.Reflection;

namespace GVDEditor.Tools;

/// <summary>
///     Snimka stavu grafu objektov: zapamata si hodnoty vsetkych poli dosiahnutelnych objektov a pri
///     <see cref="Restore" /> ich zapise spat do tych istych instancii. Odkazy medzi objektmi (vlak → kolaj,
///     kolaj → logicka tabula...) preto po obnoveni ostanu platne a objekty pridane medzitym vypadnu zo zoznamov.
/// </summary>
/// <remarks>
///     Sledovane su len objekty, pre ktore <c>isTracked</c> vrati <see langword="true" />, polia a genericke kolekcie
///     z <c>System.Collections.Generic</c> / <c>System.Collections.ObjectModel</c> (aj ako zaklad odvodenych tried,
///     napr. <c>BindingList</c>). Ostatne objekty (retazce, delegaty, obrazky, zvukova banka...) sa povazuju za
///     nemenne - obnovi sa len odkaz na ne. Delegaty sa obnovia tiez, takze odbery udalosti pridane po snimke
///     (napr. vazby zoznamov v zatvorenom okne) zaniknu.
/// </remarks>
internal sealed class ObjectGraphSnapshot
{
    private static readonly ConcurrentDictionary<Type, FieldInfo[]> FieldsCache = new();

    private readonly List<(object Target, FieldInfo[] Fields, object?[] Values)> _objects = [];
    private readonly List<(Array Target, Array Copy)> _arrays = [];

    private ObjectGraphSnapshot()
    {
    }

    /// <summary>
    ///     Pocet zapamatanych objektov (vratane poli).
    /// </summary>
    public int Count => _objects.Count + _arrays.Count;

    /// <summary>
    ///     Zapamata stav vsetkych sledovanych objektov dosiahnutelnych z <paramref name="roots" />.
    /// </summary>
    /// <param name="roots">Korene grafu.</param>
    /// <param name="isTracked">Urci, ci sa objekty daneho typu (mimo kolekcii) maju sledovat.</param>
    public static ObjectGraphSnapshot Capture(IEnumerable<object?> roots, Func<Type, bool> isTracked)
    {
        var snapshot = new ObjectGraphSnapshot();
        var visited = new HashSet<object>(ReferenceEqualityComparer.Instance);
        var pending = new Stack<object>();

        void Visit(object? value)
        {
            if (value == null) return;

            var type = value.GetType();
            if (type.IsValueType)
            {
                // struktura sa kopiruje hodnotou, sledovat treba len objekty, na ktore odkazuje
                if (type is { IsPrimitive: false, IsEnum: false })
                    foreach (var field in GetFields(type))
                        if (field.FieldType is { IsPrimitive: false, IsEnum: false })
                            Visit(field.GetValue(value));
                return;
            }

            if ((type.IsArray || IsCollection(type) || (IsPlainObject(type) && isTracked(type))) && visited.Add(value))
                pending.Push(value);
        }

        foreach (var root in roots)
            Visit(root);

        while (pending.Count > 0)
        {
            var obj = pending.Pop();
            if (obj is Array array)
            {
                snapshot._arrays.Add((array, (Array)array.Clone()));
                var elementType = array.GetType().GetElementType()!;
                if (!elementType.IsPrimitive && !elementType.IsEnum && elementType != typeof(string))
                    foreach (var item in array)
                        Visit(item);
                continue;
            }

            var fields = GetFields(obj.GetType());
            var values = new object?[fields.Length];
            for (var i = 0; i < fields.Length; i++)
            {
                values[i] = fields[i].GetValue(obj);
                Visit(values[i]);
            }

            snapshot._objects.Add((obj, fields, values));
        }

        return snapshot;
    }

    /// <summary>
    ///     Vrati vsetky zapamatane objekty do stavu v case snimky. Udalosti zoznamov sa pritom nevyvolaju -
    ///     naviazane prvky treba obnovit napr. cez <c>ResetBindings</c>.
    /// </summary>
    public void Restore()
    {
        foreach (var (target, copy) in _arrays)
            Array.Copy(copy, target, target.Length);

        foreach (var (target, fields, values) in _objects)
            for (var i = 0; i < fields.Length; i++)
                fields[i].SetValue(target, values[i]);
    }

    private static bool IsPlainObject(Type type) =>
        type != typeof(string) && !typeof(Delegate).IsAssignableFrom(type);

    /// <summary>
    ///     Genericke kolekcie .NET (List, Dictionary, HashSet, Collection, BindingList...) a z nich odvodene triedy.
    /// </summary>
    private static bool IsCollection(Type type)
    {
        for (var t = type; t != null && t != typeof(object); t = t.BaseType)
        {
            if (!t.IsGenericType) continue;

            var definition = t.GetGenericTypeDefinition();
            if (definition == typeof(BindingList<>))
                return true;
            if (definition.Assembly == typeof(List<>).Assembly &&
                definition.Namespace is "System.Collections.Generic" or "System.Collections.ObjectModel")
                return true;
        }

        return false;
    }

    private static FieldInfo[] GetFields(Type type) => FieldsCache.GetOrAdd(type, static t =>
    {
        var fields = new List<FieldInfo>();
        for (var current = t; current != null && current != typeof(object); current = current.BaseType)
            fields.AddRange(current.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly));
        return fields.ToArray();
    });
}
