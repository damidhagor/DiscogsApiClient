using System.Collections;
using System.Runtime.CompilerServices;

namespace DiscogsApiClient.SourceGenerator.Shared;

/// <summary>
/// Wraps <see cref="ImmutableArray{T}"/> with structural equality for incremental pipeline caching.
/// </summary>
/// <remarks>
/// Based on the EquatableArray implementation by Andrew Lock.
/// See: https://andrewlock.net/creating-a-source-generator-part-7-solving-the-caching-problem/
/// </remarks>
[CollectionBuilder(typeof(EquatableArray), nameof(EquatableArray.Create))]
internal readonly struct EquatableArray<T>(ImmutableArray<T> array) : IEquatable<EquatableArray<T>>, IEnumerable<T>
    where T : IEquatable<T>
{
    private readonly ImmutableArray<T> _array = array;

    public ImmutableArray<T> AsImmutableArray() => _array.IsDefault ? [] : _array;

    public int Length => _array.IsDefault ? 0 : _array.Length;

    public T this[int index] => _array[index];

    public bool Equals(EquatableArray<T> other)
    {
        if (_array.IsDefault && other._array.IsDefault)
        {
            return true;
        }

        if (_array.IsDefault || other._array.IsDefault)
        {
            return false;
        }

        return _array.SequenceEqual(other._array);
    }

    public override bool Equals(object? obj) => obj is EquatableArray<T> other && Equals(other);

    public override int GetHashCode()
    {
        if (_array.IsDefault)
        {
            return 0;
        }

        unchecked
        {
            var hash = 17;
            foreach (var item in _array)
            {
                hash = (hash * 31) + item.GetHashCode();
            }

            return hash;
        }
    }

    public static bool operator ==(EquatableArray<T> left, EquatableArray<T> right) => left.Equals(right);

    public static bool operator !=(EquatableArray<T> left, EquatableArray<T> right) => !left.Equals(right);

    public ImmutableArray<T>.Enumerator GetEnumerator() => AsImmutableArray().GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => ((IEnumerable<T>)AsImmutableArray()).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)AsImmutableArray()).GetEnumerator();

    public static implicit operator EquatableArray<T>(ImmutableArray<T> array) => new(array);

    public static implicit operator ImmutableArray<T>(EquatableArray<T> array) => array.AsImmutableArray();
}

/// <summary>
/// Builder companion for <see cref="EquatableArray{T}"/> to enable collection expression syntax.
/// </summary>
internal static class EquatableArray
{
    public static EquatableArray<T> Create<T>(ReadOnlySpan<T> items) where T : IEquatable<T>
        => new(ImmutableArray.Create(items));
}
