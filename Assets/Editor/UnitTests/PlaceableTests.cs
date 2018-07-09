using UnityEngine;
using NUnit.Framework;
using RaftTest;
using UnityEngine.TestTools;


public class TestPlaceable
{
    [Test]
    public void TestConstructor()
    {
        var placeable = new MockPlaceable("Empty air", true, true, null, 1f, false, false, true, isFullBlock: false, material: null, maxLengthWithoutSupport: 0, side: Placeable.Side.Center);
        Assert.True(!placeable.IsFullBlock);
    }

    /// <summary>
    /// Test placing of all types ob blocks
    /// </summary>    
    [Test]
    public void TestBlockPlacing(
        [Values(Placeable.Side.North, Placeable.Side.East, Placeable.Side.West, Placeable.Side.South, Placeable.Side.Center)]Placeable.Side side,
        [Values(true, false)] bool allowsXZSnapping,
        [Values(true, false)] bool allowsYSnapping,
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
        // placed at 0,0,0
        var testBlock = new MockPlaceable("TestBlock", allowsXZSnapping, allowsYSnapping, null, blockThickness, isTrigger, requiresSomeFoundation,
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
        [Values(Placeable.Side.North, Placeable.Side.East, Placeable.Side.West, Placeable.Side.South, Placeable.Side.Center, Placeable.Side.Top, Placeable.Side.Bottom)]Placeable.Side side,
        [Values(true, false)] bool allowsXZSnapping,
        [Values(true, false)] bool allowsYSnapping,
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

        // placed at 0,0,0
        var testBlock = new MockPlaceable("TestBlock", allowsXZSnapping, allowsYSnapping, null, blockThickness, isTrigger, requiresSomeFoundation,
            canBePlacedAtZeroLevelWithoutFoundation, isFullBlock, null, maxLengthWithoutSupport, side);


        var gameObject = new GameObject("Mono holder");
        var world = gameObject.AddComponent<MockWorld>();
        world.SetUp();

        var placedBlock = testBlock.Place(world);

        LogAssert.ignoreFailingMessages = true;
        world.Remove(placedBlock);
        LogAssert.ignoreFailingMessages = false;
        Assert.IsTrue(world.GetBlock(testBlock.GetIntegerCoords(), side) == World.AirBlock);// should be empty after deletion
    }

    /// <summary>
    /// Tries  to put testBlock on bottomBlock
    /// </summary>    

    [Test]
    public void CanFullBlockBePlacedOnFullBlock(
        [Values(Placeable.Side.North, Placeable.Side.East, Placeable.Side.West, Placeable.Side.South, Placeable.Side.Center, Placeable.Side.Top, Placeable.Side.Bottom)]Placeable.Side bottomBlockSide,

        [Values(Placeable.Side.North, Placeable.Side.East, Placeable.Side.West, Placeable.Side.South, Placeable.Side.Center, Placeable.Side.Top, Placeable.Side.Bottom)]Placeable.Side testBlockSide,
        [Values(true, false)]bool isTestBlockIsFullBlock,
        [Values(0)]int maxLengthWithoutSupport
        )
    {
        Assert.IsTrue(TestCanBePlaced(bottomBlockSide, true, testBlockSide, isTestBlockIsFullBlock, maxLengthWithoutSupport));
    }
    [Test]
    public void CanFullBlockBePlacedOnHalfBlockExceptBottomBlock(
        [Values(Placeable.Side.North, Placeable.Side.East, Placeable.Side.West, Placeable.Side.South, Placeable.Side.Center, Placeable.Side.Top)]Placeable.Side bottomBlockSide,
        [Values(Placeable.Side.North, Placeable.Side.East, Placeable.Side.West, Placeable.Side.South, Placeable.Side.Center, Placeable.Side.Top, Placeable.Side.Bottom)]Placeable.Side testBlockSide,

        [Values(0)]int maxLengthWithoutSupport
        )
    {
        Assert.IsTrue(TestCanBePlaced(bottomBlockSide, false, testBlockSide, true, maxLengthWithoutSupport));
    }

    [Test]
    public void CanFullBlockBePlacedOnHalfBlockBottom(

        [Values(Placeable.Side.North, Placeable.Side.East, Placeable.Side.West, Placeable.Side.South, Placeable.Side.Center, Placeable.Side.Top, Placeable.Side.Bottom)]Placeable.Side testBlockSide,

        [Values(0)]int maxLengthWithoutSupport
        )
    {
        Assert.IsFalse(TestCanBePlaced(Placeable.Side.Bottom, false, testBlockSide, true, maxLengthWithoutSupport));
    }
    [Test]
    public void CanHalfBlockBePlacedhalfBlock(
        [Values(Placeable.Side.North, Placeable.Side.East, Placeable.Side.West, Placeable.Side.South, Placeable.Side.Center, Placeable.Side.Top, Placeable.Side.Bottom)]Placeable.Side bottomBlockSide,
        [Values(Placeable.Side.North, Placeable.Side.East, Placeable.Side.West, Placeable.Side.South, Placeable.Side.Center, Placeable.Side.Top, Placeable.Side.Bottom)]Placeable.Side testBlockSide,
        [Values(0)]int maxLengthWithoutSupport
        )
    {
        var res = TestCanBePlaced(bottomBlockSide, false, testBlockSide, false, maxLengthWithoutSupport);
        if (bottomBlockSide == testBlockSide || bottomBlockSide== Placeable.Side.Top || testBlockSide == Placeable.Side.Bottom)
            Assert.IsTrue(res);
        else
            Assert.IsFalse(res);
    }
    

    public bool TestCanBePlaced(
       Placeable.Side bottomBlockSide,
       bool isBottomBlockIsFullBlock,

       Placeable.Side testBlockSide,
       bool isTestBlockIsFullBlock,
       int maxLengthWithoutSupport
       )
    {

        // placed at 0,0,0
        var bottomBlock = new MockPlaceable("TestBlock", true, true, null, 0.2f, false, true,
            true, isBottomBlockIsFullBlock, null, maxLengthWithoutSupport, bottomBlockSide); // sets bottom block


        var gameObject = new GameObject("Mono holder");
        var world = gameObject.AddComponent<MockWorld>();
        world.SetUp();

        bottomBlock.Place(world);

        var testBlock = new MockPlaceable("TestBlock", true, true, null, 0.2f, false, true,
            true, isTestBlockIsFullBlock, null, maxLengthWithoutSupport, testBlockSide);

        testBlock.SetPosition(new Vector3(0f, 1f, 0f));


        return testBlock.CanBePlaced(world);
    }
}