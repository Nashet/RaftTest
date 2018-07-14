using UnityEngine;

namespace RaftTest
{
    public interface IPlaceable : IHideable, IHoldable
    {
        bool IsFullBlock { get; }
        bool OnlyCenterPlacing { get; }

        Vector3Int GetIntegerCoords();
        PlacedBlock Place(World world);
        void SetPosition(Vector3 position, Placeable.Side side);
    }
}