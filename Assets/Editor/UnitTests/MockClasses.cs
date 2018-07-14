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
public class MockPlaceable : BlockType
{

    public MockPlaceable(string name, bool allowsXZSnapping, bool allowsYSnapping, GameObject prefab, float blockThickness, bool isTrigger, bool requiresSomeFoundation, bool canBePlacedAtZeroLevelWithoutFoundation, bool isFullBlock, Material material, int maxLengthWithoutSupport)
    : base(name, allowsXZSnapping,  allowsYSnapping, prefab, blockThickness, isTrigger, requiresSomeFoundation, canBePlacedAtZeroLevelWithoutFoundation, isFullBlock, material, maxLengthWithoutSupport)
    {
        block = new GameObject();
        block.AddComponent<BoxCollider>();        
        renderer = block.AddComponent<MeshRenderer>();
    }
    
    /// <summary>
    /// gives access
    /// </summary>    
    new public bool CanBePlaced(World world)
    {
        return base.CanBePlaced(world);
    }
}