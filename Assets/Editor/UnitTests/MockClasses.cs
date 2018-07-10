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

    public MockPlaceable(string name, bool allowsXZSnapping, bool allowsYSnapping, GameObject prefab, float blockThickness, bool isTrigger, bool requiresSomeFoundation, bool canBePlacedAtZeroLevelWithoutFoundation, bool isFullBlock, Material material, int maxLengthWithoutSupport, Placeable.Side side)
    : base(name, allowsXZSnapping,  allowsYSnapping, prefab, blockThickness, isTrigger, requiresSomeFoundation, canBePlacedAtZeroLevelWithoutFoundation, isFullBlock, material, maxLengthWithoutSupport)
    {
        block = new GameObject();
        block.AddComponent<BoxCollider>();
        sideSnapping = side;
        renderer = block.AddComponent<MeshRenderer>();
    }
    public void SetPosition(Vector3 position)
    {
        block.transform.position = position;
    }
    new public bool CanBePlaced(World world)
    {
        return base.CanBePlaced(world);
    }
}