using UnityEngine;
using NUnit.Framework;
using RaftTest;
using UnityEngine.TestTools;


public class TestPlaceable
{
    [Test]
    public void TestConstructor()
    {
        var placeable = new MockPlaceable("Empty air", allowsXZSnapping: true, allowsYSnapping: true,
            blockThickness: 1f, isTrigger: false, requiresSomeFoundation: false,
            canBePlacedAtZeroLevelWithoutFoundation: true, isFullBlock: false, maxLengthWithoutSupport: 0,
            prefab: null);
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

        var gameObject = new GameObject("Mono holder");
        var world = gameObject.AddComponent<MockWorld>();
        world.SetUp();


        var testBlock = new MockPlaceable("TestBlock", allowsXZSnapping, allowsYSnapping, blockThickness, isTrigger, requiresSomeFoundation, canBePlacedAtZeroLevelWithoutFoundation,
            isFullBlock, maxLengthWithoutSupport, null);

        // placed at 0,0,0
        testBlock.SetPosition(Vector3.zero, side);
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
        var gameObject = new GameObject("Mono holder");
        var world = gameObject.AddComponent<MockWorld>();
        world.SetUp();


        var testBlock = new MockPlaceable("TestBlock", allowsXZSnapping, allowsYSnapping, blockThickness, isTrigger, requiresSomeFoundation, canBePlacedAtZeroLevelWithoutFoundation,
            isFullBlock, maxLengthWithoutSupport, null);

        // placed at 0,0,0
        testBlock.SetPosition(Vector3.zero, side);

        var placedBlock = testBlock.Place(world);

        LogAssert.ignoreFailingMessages = true;
        world.Remove(placedBlock);
        LogAssert.ignoreFailingMessages = false;
        Assert.IsTrue(world.GetBlock(testBlock.GetIntegerCoords(), side) == World.AirBlock);// should be empty after deletion
    }
    /// <summary>
    /// Test placing of all types ob blocks
    /// </summary>    
    [Test]
    public void TestBlockReplacing(
        [Values(Placeable.Side.North, Placeable.Side.East, Placeable.Side.West, Placeable.Side.South, Placeable.Side.Center, Placeable.Side.Top, Placeable.Side.Bottom)]Placeable.Side blockForDeletingSide,
        [Values(true, false)] bool blockForDeletingAllowsXZSnapping,
        [Values(true, false)] bool blockForDeletingAllowsYSnapping,
        [Values(true, false)]bool blockForDeletingIsFullBlock,
        [Values(Placeable.Side.North, Placeable.Side.East, Placeable.Side.West, Placeable.Side.South, Placeable.Side.Center, Placeable.Side.Top, Placeable.Side.Bottom)]Placeable.Side testBlockSide,
        [Values(true, false)] bool testBlockAllowsXZSnapping,
        [Values(true, false)] bool testBlockAllowsYSnapping,
        [Values(true, false)]bool testBlockIsFullBlock

        )
    {

        var gameObject = new GameObject("Mono holder");
        var world = gameObject.AddComponent<MockWorld>();
        world.SetUp();


        var blockForDeleting = new MockPlaceable("blockForDeleting", blockForDeletingAllowsXZSnapping, blockForDeletingAllowsYSnapping,
            blockThickness: 0.2f, isTrigger: false, requiresSomeFoundation: true, canBePlacedAtZeroLevelWithoutFoundation: true,
            isFullBlock: blockForDeletingIsFullBlock, maxLengthWithoutSupport: 0, prefab: null);

        blockForDeleting.SetPosition(new Vector3(0f, 0f, 0f), blockForDeletingSide);


        var placedBlock = blockForDeleting.Place(world);

        LogAssert.ignoreFailingMessages = true;
        world.Remove(placedBlock);
        LogAssert.ignoreFailingMessages = false;

        var testBlock = new MockPlaceable("TestBlock", testBlockAllowsXZSnapping, testBlockAllowsYSnapping,
            blockThickness: 0.2f, isTrigger: false, requiresSomeFoundation: true,
            canBePlacedAtZeroLevelWithoutFoundation: true, isFullBlock: testBlockIsFullBlock,
            maxLengthWithoutSupport: 0, prefab: null);
        testBlock.SetPosition(new Vector3(0f, 0f, 0f), testBlockSide);
        testBlock.Place(world);

        Assert.AreSame(testBlock, world.GetBlock(testBlock.GetIntegerCoords(), testBlockSide));// should be empty after deletion
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
        if (bottomBlockSide == testBlockSide || bottomBlockSide == Placeable.Side.Top || testBlockSide == Placeable.Side.Bottom)
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
        var gameObject = new GameObject("Mono holder");
        var world = gameObject.AddComponent<MockWorld>();
        world.SetUp();

        var bottomBlock = new MockPlaceable("TestBlock", allowsXZSnapping: true, allowsYSnapping: true,
            blockThickness: 0.2f, isTrigger: false, requiresSomeFoundation: true,
            canBePlacedAtZeroLevelWithoutFoundation: true, isFullBlock: isBottomBlockIsFullBlock,
            maxLengthWithoutSupport: maxLengthWithoutSupport, prefab: null); // sets bottom block


        bottomBlock.SetPosition(Vector3.zero, bottomBlockSide);

        bottomBlock.Place(world);

        var testBlock = new MockPlaceable("TestBlock", allowsXZSnapping: true, allowsYSnapping: true,
            blockThickness: 0.2f, isTrigger: false, requiresSomeFoundation: true,
            canBePlacedAtZeroLevelWithoutFoundation: true, isFullBlock: isTestBlockIsFullBlock,
            maxLengthWithoutSupport: maxLengthWithoutSupport, prefab: null);

        testBlock.SetPosition(new Vector3(0f, 1f, 0f), testBlockSide);


        return testBlock.CanBePlaced(world);
    }
}