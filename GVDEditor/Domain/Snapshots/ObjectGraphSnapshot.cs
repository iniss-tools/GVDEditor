using System.Collections.Concurrent;
using System.Reflection;

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
    /// Ci <paramref name="other" /> zachytil graf v rovnakom stave: rovnaka struktura a hodnoty, pricom sledovane objekty
    /// sa porovnavaju podla miesta v grafe, nie podla instancie - zoznam nahradeny novym s rovnakymi prvkami je ten isty
    /// stav. Pocitadlo zmien kolekcii (<c>_version</c>) a volne miesto v ich poliach sa neporovnavaju. Retazce a hodnoty
    /// cez <see cref="object.Equals(object, object)" />, ostatne objekty (obrazky, zvukova banka...) referenciou.
    /// </summary>
    public bool SameAs(ObjectGraphSnapshot other)
    {
        ArgumentNullException.ThrowIfNull(other);
        if (_objects.Count != other._objects.Count || _arrays.Count != other._arrays.Count)
            return false;

        var places = Places();
        var otherPlaces = other.Places();

        bool Same(object? a, object? b)
        {
            if (a is not null && places.TryGetValue(a, out var place))
                return b is not null && otherPlaces.TryGetValue(b, out var otherPlace) && place == otherPlace;
            return (b is null || !otherPlaces.ContainsKey(b)) && SameValue(a, b);
        }

        for (var i = 0; i < _arrays.Count; i++)
        {
            var (_, copy) = _arrays[i];
            var (_, otherCopy) = other._arrays[i];
            if (copy.GetType() != otherCopy.GetType())
                return false;
            // pole kolekcie moze mat inu kapacitu - volne miesto ma predvolenu hodnotu
            var elementType = copy.GetType().GetElementType()!;
            var empty = elementType.IsValueType ? Activator.CreateInstance(elementType) : null;
            for (var j = 0; j < Math.Max(copy.Length, otherCopy.Length); j++)
                if (!Same(j < copy.Length ? copy.GetValue(j) : empty, j < otherCopy.Length ? otherCopy.GetValue(j) : empty))
                    return false;
        }

        for (var i = 0; i < _objects.Count; i++)
        {
            var (target, fields, values) = _objects[i];
            var (otherTarget, _, otherValues) = other._objects[i];
            if (target.GetType() != otherTarget.GetType() || values.Length != otherValues.Length)
                return false;
            var collection = IsCollection(target.GetType());
            for (var j = 0; j < values.Length; j++)
                if (!(collection && fields[j].Name == "_version") && !Same(values[j], otherValues[j]))
                    return false;
        }

        return true;
    }

    /// <summary>Miesto sledovanych objektov a poli v grafe - poradie prechodu (polia zaporne).</summary>
    private Dictionary<object, int> Places()
    {
        var places = new Dictionary<object, int>(_objects.Count + _arrays.Count, ReferenceEqualityComparer.Instance);
        for (var i = 0; i < _objects.Count; i++)
            places[_objects[i].Target] = i;
        for (var i = 0; i < _arrays.Count; i++)
            places[_arrays[i].Target] = -1 - i;
        return places;
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
