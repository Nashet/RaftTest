using UnityEngine;
using NUnit.Framework;
using RaftTest;
using UnityEngine.TestTools;


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
    /// <summary>
    /// Test placing of all types ob blocks
    /// </summary>    
    [Test]

    public void TestBlockDeleting(
        [Values(-1, 0, 1)]int x,
        [Values(-1, 0, 1)]int y,
        [Values(true, false)] bool allowsEdgePlacing,
        //[Values(0f, 1f, 0.1f, -0.2f)]float blockThickness,
        [Values(0.2f)]float blockThickness,
        [Values(true)]bool isTrigger,
        [Values(true)]bool requiresSomeFoundation,
        [Values(true)]bool canBePlacedAtZeroLevelWithoutFoundation,
        [Values(true, false)]bool isFullBlock,
        //[Values(-1, 0, 1, 2)]int maxLengthWithoutSupport
        [Values(0, 1, 2, 3)]int maxLengthWithoutSupport
        )
    {
        var side = new Vector2Int(x, y);
        // placed at 0,0,0
        var testBlock = new MockPlaceable("TestBlock", allowsEdgePlacing, null, blockThickness, isTrigger, requiresSomeFoundation,
            canBePlacedAtZeroLevelWithoutFoundation, isFullBlock, null, maxLengthWithoutSupport, side);


        var gameObject = new GameObject("Mono holder");
        var world = gameObject.AddComponent<MockWorld>();
        world.SetUp();

        var placedBlock = testBlock.Place(world);

        //[LogAssert.Expect(LogType.Error, "")]
        LogAssert.ignoreFailingMessages = true;
        world.Remove(placedBlock);
        LogAssert.ignoreFailingMessages = false;
        Assert.IsTrue(world.GetBlock(testBlock.GetIntegerCoords(), side) == World.AirBlock);// should be empty after deletion
    }    
}