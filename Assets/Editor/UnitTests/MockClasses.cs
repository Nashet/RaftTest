using RaftTest;
using RaftTest.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes class usable in editor mode
/// </summary>
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

/// <summary>
/// Makes class usable in editor mode
/// </summary>
public class MockPlaceable : Placeable
{

    public MockPlaceable(string name, bool allowsXZSnapping, bool allowsYSnapping, float blockThickness, bool isTrigger, bool requiresSomeFoundation, bool canBePlacedAtZeroLevelWithoutFoundation, bool isFullBlock, int maxLengthWithoutSupport, GameObject prefab)
    : base(name, allowsXZSnapping, allowsYSnapping, prefab, blockThickness, isTrigger, requiresSomeFoundation, canBePlacedAtZeroLevelWithoutFoundation, isFullBlock, maxLengthWithoutSupport)
    {
        block = new GameObject();
        block.AddComponent<BoxCollider>();
        placingAllowedSelector = TimedSelectorWithMaterial.AddTo(block, null, 0f);
        placingDeniedSelector = TimedSelectorWithMaterial.AddTo(block, null, 0f);
    }

    /// <summary>
    /// gives access
    /// </summary>    
    new public bool CanBePlaced(World world)
    {
        return base.CanBePlaced(world);
    }
}