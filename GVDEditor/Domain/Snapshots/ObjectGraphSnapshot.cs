using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace GVDEditor.Domain.Snapshots;

/// <summary>
/// Snimka stavu grafu objektov: zapamata si hodnoty vsetkych poli dosiahnutelnych objektov a pri
/// <see cref="Restore" /> ich zapise spat do tych istych instancii. Odkazy medzi objektmi (vlak → kolaj,
/// kolaj → logicka tabula...) preto po obnoveni ostanu platne a objekty pridane medzitym vypadnu zo zoznamov.
/// </summary>
/// <remarks>
/// Sledovane su len objekty, pre ktore <c>isTracked</c> vrati <see langword="true" />, polia a genericke kolekcie
/// z <c>System.Collections.Generic</c> / <c>System.Collections.ObjectModel</c> (aj ako zaklad odvodenych tried,
/// napr. <c>BindingList</c>). Ostatne objekty (retazce, delegaty, obrazky, zvukova banka...) sa povazuju za
/// nemenne - obnovi sa len odkaz na ne. Delegaty sa obnovia tiez, takze odbery udalosti pridane po snimke
/// (napr. vazby zoznamov v zatvorenom okne) zaniknu.
/// </remarks>
internal sealed class ObjectGraphSnapshot
{
    private static readonly ConcurrentDictionary<Type, FieldInfo[]> FieldsCache = new();

    private readonly List<(object Target, FieldInfo[] Fields, object?[] Values)> _objects = [];
    private readonly List<(Array Target, Array Copy)> _arrays = [];
    private readonly List<object?> _roots = [];

    private ObjectGraphSnapshot()
    {
    }

    /// <summary>
    /// Pocet zapamatanych objektov (vratane poli).
    /// </summary>
    public int Count => _objects.Count + _arrays.Count;

    /// <summary>
    /// Zapamata stav vsetkych sledovanych objektov dosiahnutelnych z <paramref name="roots" />.
    /// </summary>
    /// <param name="roots">Korene grafu.</param>
    /// <param name="isTracked">Urci, ci sa objekty daneho typu (mimo kolekcii) maju sledovat.</param>
    /// <param name="includeDelegates">
    /// Zapamatat aj delegaty (odbery udalosti). Bez nich obnova necha odbery, ako su - napr. vazbu tabulky hlavneho okna.
    /// </param>
    public static ObjectGraphSnapshot Capture(IEnumerable<object?> roots, Func<Type, bool> isTracked, bool includeDelegates = true)
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
        {
            snapshot._roots.Add(root);
            Visit(root);
        }

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
            if (!includeDelegates)
                fields = Array.FindAll(fields, f => !typeof(Delegate).IsAssignableFrom(f.FieldType));
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
    /// Vrati vsetky zapamatane objekty do stavu v case snimky. Udalosti zoznamov sa pritom nevyvolaju -
    /// naviazane prvky treba obnovit napr. cez <c>ResetBindings</c>.
    /// </summary>
    public void Restore()
    {
        foreach (var (target, copy) in _arrays)
            Array.Copy(copy, target, target.Length);

        foreach (var (target, fields, values) in _objects)
            for (var i = 0; i < fields.Length; i++)
                fields[i].SetValue(target, values[i]);
    }

    /// <summary>
    /// Ci <paramref name="other" /> zachytil graf v rovnakom stave: od korenov rovnake hodnoty v rovnakej strukture
    /// (porovnanie ako stromov) - zalezi na hodnotach, nie na instanciach ani na tom, ktore objekty su zdielane.
    /// Zoznam nahradeny novym s rovnakymi prvkami alebo stanica trasy ako kopia namiesto zdielaneho objektu je teda
    /// ten isty stav. Pocitadlo zmien kolekcii (<c>_version</c>) a volne miesto v ich poliach sa neporovnavaju.
    /// Retazce a hodnoty cez <see cref="object.Equals(object, object)" />, nesledovane objekty (obrazky, zvukova
    /// banka...) referenciou.
    /// </summary>
    public bool SameAs(ObjectGraphSnapshot other)
    {
        ArgumentNullException.ThrowIfNull(other);
        if (_roots.Count != other._roots.Count)
            return false;

        var (objects, arrays) = Index();
        var (otherObjects, otherArrays) = other.Index();
        // dvojice objektov, ktore sa uz porovnavaju - pri cykloch a zdielanych objektoch sa porovnanie neopakuje
        var seen = new HashSet<(object, object)>(PairComparer.Instance);
        var pending = new Stack<(object A, object B)>();

        bool Same(object? a, object? b)
        {
            if (a is null || b is null)
                return a is null && b is null;
            var tracked = objects.ContainsKey(a) || arrays.ContainsKey(a);
            if (tracked != (otherObjects.ContainsKey(b) || otherArrays.ContainsKey(b)))
                return false;
            if (!tracked)
                return SameValue(a, b);
            if (seen.Add((a, b)))
                pending.Push((a, b));
            return true;
        }

        for (var i = 0; i < _roots.Count; i++)
            if (!Same(_roots[i], other._roots[i]))
                return false;

        while (pending.Count > 0)
        {
            var (a, b) = pending.Pop();
            if (a.GetType() != b.GetType())
                return false;

            if (a is Array)
            {
                var copy = arrays[a];
                var otherCopy = otherArrays[b];
                // pole kolekcie moze mat inu kapacitu - volne miesto ma predvolenu hodnotu
                var elementType = copy.GetType().GetElementType()!;
                var empty = elementType.IsValueType ? Activator.CreateInstance(elementType) : null;
                for (var j = 0; j < Math.Max(copy.Length, otherCopy.Length); j++)
                    if (!Same(j < copy.Length ? copy.GetValue(j) : empty, j < otherCopy.Length ? otherCopy.GetValue(j) : empty))
                        return false;
                continue;
            }

            var (fields, values) = objects[a];
            var otherValues = otherObjects[b].Values;
            if (values.Length != otherValues.Length)
                return false;
            var collection = IsCollection(a.GetType());
            for (var j = 0; j < values.Length; j++)
                if (!(collection && fields[j].Name == "_version") && !Same(values[j], otherValues[j]))
                    return false;
        }

        return true;
    }

    /// <summary>Zapamatane hodnoty sledovanych objektov a poli podla instancie.</summary>
    private (Dictionary<object, (FieldInfo[] Fields, object?[] Values)> Objects, Dictionary<object, Array> Arrays) Index()
    {
        var objects = new Dictionary<object, (FieldInfo[], object?[])>(_objects.Count, ReferenceEqualityComparer.Instance);
        foreach (var (target, fields, values) in _objects)
            objects[target] = (fields, values);
        var arrays = new Dictionary<object, Array>(_arrays.Count, ReferenceEqualityComparer.Instance);
        foreach (var (target, copy) in _arrays)
            arrays[target] = copy;
        return (objects, arrays);
    }

    /// <summary>Dvojica instancii porovnana referenciou.</summary>
    private sealed class PairComparer : IEqualityComparer<(object, object)>
    {
        public static readonly PairComparer Instance = new();

        public bool Equals((object, object) x, (object, object) y) => ReferenceEquals(x.Item1, y.Item1) && ReferenceEquals(x.Item2, y.Item2);

        public int GetHashCode((object, object) obj) => HashCode.Combine(RuntimeHelpers.GetHashCode(obj.Item1), RuntimeHelpers.GetHashCode(obj.Item2));
    }

    /// <summary>Hodnoty a retazce podla hodnoty, ostatne objekty referenciou.</summary>
    private static bool SameValue(object? a, object? b) =>
        a is null || b is null || a.GetType().IsValueType || a is string ? Equals(a, b) : ReferenceEquals(a, b);

    private static bool IsPlainObject(Type type) =>
        type != typeof(string) && !typeof(Delegate).IsAssignableFrom(type);

    /// <summary>
    /// Genericke kolekcie .NET (List, Dictionary, HashSet, Collection, BindingList...) a z nich odvodene triedy.
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
