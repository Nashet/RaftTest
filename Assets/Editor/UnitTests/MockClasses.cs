using RaftTest;
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

    public MockPlaceable(string name, bool allowsEdgePlacing, GameObject prefab, float blockThickness, bool isTrigger, bool requiresSomeFoundation, bool canBePlacedAtZeroLevelWithoutFoundation, bool isFullBlock, Material material, int maxLengthWithoutSupport, Vector2Int side)
    : base(name, allowsEdgePlacing, prefab, blockThickness, isTrigger, requiresSomeFoundation, canBePlacedAtZeroLevelWithoutFoundation, isFullBlock, material, maxLengthWithoutSupport)
    {
        block = new GameObject();
        block.AddComponent<BoxCollider>();
        sideSnapping = side;
        renderer = block.AddComponent<MeshRenderer>();
    }
}