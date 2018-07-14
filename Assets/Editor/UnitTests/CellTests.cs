using UnityEngine;
using NUnit.Framework;
using RaftTest;

public class TestCell
{
    [Test]
    /// <summary>
    /// Test placing of all types ob blocks
    /// </summary> 
    public void TestPlacing(
        [Values(BlockType.Side.North, BlockType.Side.East, BlockType.Side.West, BlockType.Side.South, BlockType.Side.Center)]BlockType.Side side,
        [Values(true, false)] bool allowsXZSnapping,
        [Values(true, false)] bool allowsYSnapping,
        [Values(1f)]float blockThickness,
        [Values(true, false)]bool isTrigger,
        [Values(true, false)]bool requiresSomeFoundation,
        [Values(true, false)]bool canBePlacedAtZeroLevelWithoutFoundation,
        [Values(true, false)]bool isFullBlock,
        [Values(-1, 0, 1, 2)]int maxLengthWithoutSupport)
    {

        var testableCell = new Cell();

        testableCell.Init(World.AirBlock);
        // placed at 0,0,0
        var randomBlock = new MockPlaceable("dsntmatter", allowsXZSnapping,  allowsYSnapping, null, blockThickness, isTrigger, requiresSomeFoundation,
            canBePlacedAtZeroLevelWithoutFoundation, isFullBlock, null, maxLengthWithoutSupport);

        testableCell.Place(randomBlock, side);
        Assert.AreSame(randomBlock, testableCell.Get(side));
    }
}