using UnityEngine;
using NUnit.Framework;

namespace RaftTest
{
    public class NUnityTests
    {

        [Test]
        public void TestConstructor()
        {
            var placeable = new Placeable("Empty air", true, null, 1f, false, false, true, isFullBlock: false, material: null);

            Assert.True(!placeable.IsFullBlock);
        }

        //[Test]
        //public void TestCell()
        //{
        //    var testable = new Cell();

        //    testable.Init( World.AirBlock);
        //    System.Random random = new System.Random();

        //    var manager = new GManager();

        //    var randomBlock = manager.allBlocks[random.Next(manager.allBlocks.Length)];

        //    testable.Place(randomBlock, Vector2Int.left);        
        //    Assert.AreSame(testable.Get(Vector2Int.left), randomBlock);
        //}
    }
}