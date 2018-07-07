using UnityEngine;
using NUnit.Framework;
using RaftTest;

public class MockWorld : World
{
    public void SetUp()
    {
        xSize = 10;
        ySize = 10;
        zSize = 10;

        SetUpLogic();       
    }

}

public class MockPlaceable : Placeable
{

    public MockPlaceable(string name, bool allowsEdgePlacing, GameObject prefab, float blockThickness, bool isTrigger, bool requiresSomeFoundation, bool canBePlacedAtZeroLevelWithoutFoundation, bool isFullBlock, Material material, int maxLengthWithoutSupport, Vector2Int side)
    : base(name, allowsEdgePlacing, prefab, blockThickness, isTrigger, requiresSomeFoundation, canBePlacedAtZeroLevelWithoutFoundation, isFullBlock, material, maxLengthWithoutSupport)
    {
        gameObject = new GameObject();
        gameObject.AddComponent<BoxCollider>();
        sideSnapping = side;
        renderer = gameObject.AddComponent<MeshRenderer>();
    }
}
public class TestPlaceable
{
    [Test]
    public void TestConstructor()
    {
        var placeable = new MockPlaceable("Empty air", true, null, 1f, false, false, true, isFullBlock: false, material: null, maxLengthWithoutSupport: 0, side: new Vector2Int(0, 0));
        Assert.True(!placeable.IsFullBlock);
    }

    /// <summary>
    /// Test placing of all types ob blocks
    /// </summary>    
    [Test]    
    public void TestPlacing(
        [Values(-1, 0, 1)]int x,
        [Values(-1, 0, 1)]int y,
        [Values(true, false)] bool allowsEdgePlacing,
        //[Values(0f, 1f, 0.1f, -0.2f)]float blockThickness,
        [Values(0.2f)]float blockThickness,
        [Values(true, false)]bool isTrigger,
        [Values(true, false)]bool requiresSomeFoundation,
        [Values(true)]bool canBePlacedAtZeroLevelWithoutFoundation,
        [Values(true, false)]bool isFullBlock,
        //[Values(-1, 0, 1, 2)]int maxLengthWithoutSupport
        [Values(0)]int maxLengthWithoutSupport
        )
    {
        var side = new Vector2Int(x, y);
        // placed at 0,0,0
        var testBlock = new MockPlaceable("TestBlock", allowsEdgePlacing, null, blockThickness, isTrigger, requiresSomeFoundation,
            canBePlacedAtZeroLevelWithoutFoundation, isFullBlock, null, maxLengthWithoutSupport, side);


        var gameObject = new GameObject("Mono holder");
        var world = gameObject.AddComponent<MockWorld>();
        world.SetUp();

        testBlock.Place(world);

        var placed = world.GetBlock(testBlock.GetIntegerCoords(), side);
        Assert.AreSame(testBlock, placed);
    }
    //[Test]
    //public void TestPlacing2(
    //    [Values(-1, 0, 1)]int xSise,
    //    [Values(-1, 0, 1)]int ySide,
    //    [Values(true, false)] bool allowsEdgePlacing,
    //    //[Values(0f, 1f, 0.1f, -0.2f)]float blockThickness,
    //    [Values(0.2f)]float blockThickness,
    //    [Values(true, false)]bool isTrigger,
    //    [Values(true, false)]bool requiresSomeFoundation,
    //    [Values(true)]bool canBePlacedAtZeroLevelWithoutFoundation,
    //    [Values(true, false)]bool isFullBlock,
    //    //[Values(-1, 0, 1, 2)]int maxLengthWithoutSupport
    //    [Values(0)]int maxLengthWithoutSupport
    //    )
    //{

    //    TestPlacing(xSise, ySide, allowsEdgePlacing, blockThickness, isTrigger, requiresSomeFoundation,
    //        canBePlacedAtZeroLevelWithoutFoundation, isFullBlock, maxLengthWithoutSupport);
    //}
}