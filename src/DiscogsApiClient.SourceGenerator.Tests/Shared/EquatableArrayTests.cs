using System.Collections.Immutable;
using DiscogsApiClient.SourceGenerator.Shared;

namespace DiscogsApiClient.SourceGenerator.Tests.Shared;

public sealed class EquatableArrayTests
{
    [Test]
    public async Task ShouldBeEqual_WhenBothAreDefault()
    {
        var array1 = default(EquatableArray<int>);
        var array2 = default(EquatableArray<int>);

        await Assert.That(array1 == array2).IsTrue();
        await Assert.That(array1.Equals(array2)).IsTrue();
        await Assert.That(array1.Equals((object)array2)).IsTrue();
    }

    [Test]
    public async Task ShouldNotBeEqual_WhenOneIsDefault()
    {
        var array1 = default(EquatableArray<int>);
        var array2 = new EquatableArray<int>([1, 2, 3]);

        await Assert.That(array1 != array2).IsTrue();
        await Assert.That(array1.Equals(array2)).IsFalse();
    }

    [Test]
    public async Task ShouldBeEqual_WhenSequenceIsIdentical()
    {
        var array1 = new EquatableArray<int>([1, 2, 3]);
        var array2 = new EquatableArray<int>([1, 2, 3]);

        await Assert.That(array1 == array2).IsTrue();
        await Assert.That(array1.Equals(array2)).IsTrue();
    }

    [Test]
    public async Task ShouldNotBeEqual_WhenSequenceDiffers()
    {
        var array1 = new EquatableArray<int>([1, 2, 3]);
        var array2 = new EquatableArray<int>([1, 2, 4]);

        await Assert.That(array1 != array2).IsTrue();
        await Assert.That(array1.Equals(array2)).IsFalse();
    }

    [Test]
    public async Task ShouldNotBeEqual_WhenSequenceHasSameValuesInDifferentOrder()
    {
        var array1 = new EquatableArray<int>([1, 2, 3]);
        var array2 = new EquatableArray<int>([3, 2, 1]);

        await Assert.That(array1 != array2).IsTrue();
        await Assert.That(array1.Equals(array2)).IsFalse();
    }

    [Test]
    public async Task ShouldGenerateSameHashCode_WhenSequencesAreIdentical()
    {
        var array1 = new EquatableArray<int>([1, 2, 3]);
        var array2 = new EquatableArray<int>([1, 2, 3]);

        await Assert.That(array1.GetHashCode()).IsEqualTo(array2.GetHashCode());
    }

    [Test]
    public async Task ShouldGenerateDifferentHashCode_WhenSequencesDiffer()
    {
        var array1 = new EquatableArray<int>([1, 2, 3]);
        var array2 = new EquatableArray<int>([1, 2, 4]);

        await Assert.That(array1.GetHashCode()).IsNotEqualTo(array2.GetHashCode());
    }

    [Test]
    public async Task ShouldGenerateDifferentHashCode_WhenSequenceHasSameValuesInDifferentOrder()
    {
        var array1 = new EquatableArray<int>([1, 2, 3]);
        var array2 = new EquatableArray<int>([3, 2, 1]);

        await Assert.That(array1.GetHashCode()).IsNotEqualTo(array2.GetHashCode());
    }

    [Test]
    public async Task ShouldImplicitlyConvert_ToAndFromImmutableArray()
    {
        ImmutableArray<int> raw = [1, 2, 3];
        EquatableArray<int> equatable = raw;
        ImmutableArray<int> convertedBack = equatable;

        await Assert.That(equatable.Length).IsEqualTo(3);
        await Assert.That(convertedBack.SequenceEqual(raw)).IsTrue();
    }

    [Test]
    public async Task ShouldSupportCollectionExpressionSyntax()
    {
        EquatableArray<int> array = [1, 2, 3];

        await Assert.That(array.Length).IsEqualTo(3);
        await Assert.That(array[0]).IsEqualTo(1);
        await Assert.That(array[1]).IsEqualTo(2);
        await Assert.That(array[2]).IsEqualTo(3);
    }
}
