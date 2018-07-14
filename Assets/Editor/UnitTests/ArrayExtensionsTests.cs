using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using RaftTest;
using System.Linq;
using RaftTest.Utils;

public class ArrayExtensionsTests
{
    [Test]
    public void TestGetCoordsWithRadiusZero(
        [Values(0, 1, 3, 11)]int x,
        [Values(0, 1, 2, 17)]int z)
    {
        var map = new bool[12, 14, 18];
        var expected = new List<Vector2Int> { new Vector2Int(x, z) };
        var result = map.GetCoordsWithRadius(x, z, 0).ToList();
        Assert.That(result, Is.EquivalentTo(expected));
    }
    [Test]
    public void TestGetCoordsWithRadiusZeroWrongIndexes(
        [Values(-2, -1, 12, 13)]int x,
        [Values(-2, -1, 0, 17, 18, 19)]int z)
    {
        var map = new bool[12, 14, 18];        
        var result = map.GetCoordsWithRadius(x, z, 0).ToList();
        Assert.That(result, Is.Empty);
    }
    //[Test]
    //public void TestGetCoordsWithRadiusZeroWrongIndexes_2(
    //    [Values(-2, -1, 12, 13)]int x,
    //    [Values(-2, -1, 0, 17, 18, 19)]int z)
    //{
    //    var map = new bool[12, 14, 18];
    //    var expected = new List<Vector2Int> { new Vector2Int(x, z) };
    //    var result = map.GetCoordsWithRadius(x, z, 1).ToList();
    //    Assert.That(result, Is.Empty);
    //}
    [Test]
    public void TestGetCoordsWithRadius1()
    {
        var map = new bool[5, 4, 6];
        var result = map.GetCoordsWithRadius(0, 0, 1).ToList();
        var expected = new List<Vector2Int> { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(1, 1) };
        Assert.That(result, Is.EquivalentTo(expected));
    }
    [Test]
    public void TestGetCoordsWithRadius1_2()
    {
        var map = new bool[5, 4, 6];
        var result = map.GetCoordsWithRadius(4, 5, 1).ToList();
        var expected = new List<Vector2Int> { new Vector2Int(4, 5), new Vector2Int(3, 5), new Vector2Int(4, 4), new Vector2Int(3, 4) };
        Assert.That(result, Is.EquivalentTo(expected));
    }

}
