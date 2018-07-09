﻿using UnityEngine;
using NUnit.Framework;
using RaftTest;

public class TestCell
{
    [Test]
    /// <summary>
    /// Test placing of all types ob blocks
    /// </summary> 
    public void TestPlacing(
        [Values(Placeable.Side.North, Placeable.Side.East, Placeable.Side.West, Placeable.Side.South, Placeable.Side.Center)]Placeable.Side side,
        [Values(true, false)] bool allowsEdgePlacing,
        [Values(1f)]float blockThickness,
        [Values(true, false)]bool isTrigger,
        [Values(true, false)]bool requiresSomeFoundation,
        [Values(true, false)]bool canBePlacedAtZeroLevelWithoutFoundation,
        [Values(true, false)]bool isFullBlock,
        [Values(-1, 0, 1, 2)]int maxLengthWithoutSupport)
    {

        var testable = new Cell();

        testable.Init(World.AirBlock);
        // placed at 0,0,0
        var randomBlock = new MockPlaceable("dsntmatter", allowsEdgePlacing, null, blockThickness, isTrigger, requiresSomeFoundation,
            canBePlacedAtZeroLevelWithoutFoundation, isFullBlock, null, maxLengthWithoutSupport, side);

        testable.Place(randomBlock, side);
        Assert.AreSame(randomBlock, testable.Get(side));
    }
}