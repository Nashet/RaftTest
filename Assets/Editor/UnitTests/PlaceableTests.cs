using UnityEngine;
using NUnit.Framework;
using RaftTest;
using UnityEngine.TestTools;


public class TestPlaceable
{
    [Test]
    public void TestConstructor()
    {
        var placeable = new MockPlaceable("Empty air", true, true, null, 1f, false, false, true, isFullBlock: false, material: null, maxLengthWithoutSupport: 0);
        Assert.True(!placeable.IsFullBlock);
    }

    /// <summary>
    /// Test placing of all types ob blocks
    /// </summary>    
    [Test]
    public void TestBlockPlacing(
        [Values(BlockType.Side.North, BlockType.Side.East, BlockType.Side.West, BlockType.Side.South, BlockType.Side.Center)]BlockType.Side side,
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


        var testBlock = new MockPlaceable("TestBlock", allowsXZSnapping, allowsYSnapping, null, blockThickness, isTrigger, requiresSomeFoundation,
            canBePlacedAtZeroLevelWithoutFoundation, isFullBlock, null, maxLengthWithoutSupport);

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
        [Values(BlockType.Side.North, BlockType.Side.East, BlockType.Side.West, BlockType.Side.South, BlockType.Side.Center, BlockType.Side.Top, BlockType.Side.Bottom)]BlockType.Side side,
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


        var testBlock = new MockPlaceable("TestBlock", allowsXZSnapping, allowsYSnapping, null, blockThickness, isTrigger, requiresSomeFoundation,
            canBePlacedAtZeroLevelWithoutFoundation, isFullBlock, null, maxLengthWithoutSupport);

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
        [Values(BlockType.Side.North, BlockType.Side.East, BlockType.Side.West, BlockType.Side.South, BlockType.Side.Center, BlockType.Side.Top, BlockType.Side.Bottom)]BlockType.Side blockForDeletingSide,
        [Values(true, false)] bool blockForDeletingAllowsXZSnapping,
        [Values(true, false)] bool blockForDeletingAllowsYSnapping,
        [Values(true, false)]bool blockForDeletingIsFullBlock,
        [Values(BlockType.Side.North, BlockType.Side.East, BlockType.Side.West, BlockType.Side.South, BlockType.Side.Center, BlockType.Side.Top, BlockType.Side.Bottom)]BlockType.Side testBlockSide,
        [Values(true, false)] bool testBlockAllowsXZSnapping,
        [Values(true, false)] bool testBlockAllowsYSnapping,
        [Values(true, false)]bool testBlockIsFullBlock

        )
    {

        var gameObject = new GameObject("Mono holder");
        var world = gameObject.AddComponent<MockWorld>();
        world.SetUp();


        var blockForDeleting = new MockPlaceable("blockForDeleting", blockForDeletingAllowsXZSnapping, blockForDeletingAllowsYSnapping, null, 0.2f, false, true,
            true, blockForDeletingIsFullBlock, null, 0);

        blockForDeleting.SetPosition(new Vector3(0f, 0f, 0f), blockForDeletingSide);


        var placedBlock = blockForDeleting.Place(world);

        LogAssert.ignoreFailingMessages = true;
        world.Remove(placedBlock);
        LogAssert.ignoreFailingMessages = false;

        var testBlock = new MockPlaceable("TestBlock", testBlockAllowsXZSnapping, testBlockAllowsYSnapping, null, 0.2f, false, true,
            true, testBlockIsFullBlock, null, 0 );
        testBlock.SetPosition(new Vector3(0f, 0f, 0f), testBlockSide);
        testBlock.Place(world);

        Assert.AreSame(testBlock, world.GetBlock(testBlock.GetIntegerCoords(), testBlockSide));// should be empty after deletion
    }

    /// <summary>
    /// Tries  to put testBlock on bottomBlock
    /// </summary>    

    [Test]
    public void CanFullBlockBePlacedOnFullBlock(
        [Values(BlockType.Side.North, BlockType.Side.East, BlockType.Side.West, BlockType.Side.South, BlockType.Side.Center, BlockType.Side.Top, BlockType.Side.Bottom)]BlockType.Side bottomBlockSide,

        [Values(BlockType.Side.North, BlockType.Side.East, BlockType.Side.West, BlockType.Side.South, BlockType.Side.Center, BlockType.Side.Top, BlockType.Side.Bottom)]BlockType.Side testBlockSide,
        [Values(true, false)]bool isTestBlockIsFullBlock,
        [Values(0)]int maxLengthWithoutSupport
        )
    {
        Assert.IsTrue(TestCanBePlaced(bottomBlockSide, true, testBlockSide, isTestBlockIsFullBlock, maxLengthWithoutSupport));
    }
    [Test]
    public void CanFullBlockBePlacedOnHalfBlockExceptBottomBlock(
        [Values(BlockType.Side.North, BlockType.Side.East, BlockType.Side.West, BlockType.Side.South, BlockType.Side.Center, BlockType.Side.Top)]BlockType.Side bottomBlockSide,
        [Values(BlockType.Side.North, BlockType.Side.East, BlockType.Side.West, BlockType.Side.South, BlockType.Side.Center, BlockType.Side.Top, BlockType.Side.Bottom)]BlockType.Side testBlockSide,

        [Values(0)]int maxLengthWithoutSupport
        )
    {
        Assert.IsTrue(TestCanBePlaced(bottomBlockSide, false, testBlockSide, true, maxLengthWithoutSupport));
    }

    [Test]
    public void CanFullBlockBePlacedOnHalfBlockBottom(

        [Values(BlockType.Side.North, BlockType.Side.East, BlockType.Side.West, BlockType.Side.South, BlockType.Side.Center, BlockType.Side.Top, BlockType.Side.Bottom)]BlockType.Side testBlockSide,

        [Values(0)]int maxLengthWithoutSupport
        )
    {
        Assert.IsFalse(TestCanBePlaced(BlockType.Side.Bottom, false, testBlockSide, true, maxLengthWithoutSupport));
    }
    [Test]
    public void CanHalfBlockBePlacedhalfBlock(
        [Values(BlockType.Side.North, BlockType.Side.East, BlockType.Side.West, BlockType.Side.South, BlockType.Side.Center, BlockType.Side.Top, BlockType.Side.Bottom)]BlockType.Side bottomBlockSide,
        [Values(BlockType.Side.North, BlockType.Side.East, BlockType.Side.West, BlockType.Side.South, BlockType.Side.Center, BlockType.Side.Top, BlockType.Side.Bottom)]BlockType.Side testBlockSide,
        [Values(0)]int maxLengthWithoutSupport
        )
    {
        var res = TestCanBePlaced(bottomBlockSide, false, testBlockSide, false, maxLengthWithoutSupport);
        if (bottomBlockSide == testBlockSide || bottomBlockSide == BlockType.Side.Top || testBlockSide == BlockType.Side.Bottom)
            Assert.IsTrue(res);
        else
            Assert.IsFalse(res);
    }


    public bool TestCanBePlaced(
       BlockType.Side bottomBlockSide,
       bool isBottomBlockIsFullBlock,

       BlockType.Side testBlockSide,
       bool isTestBlockIsFullBlock,
       int maxLengthWithoutSupport
       )
    {
        var gameObject = new GameObject("Mono holder");
        var world = gameObject.AddComponent<MockWorld>();
        world.SetUp();
        
        var bottomBlock = new MockPlaceable("TestBlock", true, true, null, 0.2f, false, true,
            true, isBottomBlockIsFullBlock, null, maxLengthWithoutSupport); // sets bottom block


        bottomBlock.SetPosition(Vector3.zero, bottomBlockSide);

        bottomBlock.Place(world);

        var testBlock = new MockPlaceable("TestBlock", true, true, null, 0.2f, false, true,
            true, isTestBlockIsFullBlock, null, maxLengthWithoutSupport);

        testBlock.SetPosition(new Vector3(0f, 1f, 0f), testBlockSide);


        return testBlock.CanBePlaced(world);
    }
}