using Joostit.NeuralNerd.NnLib.Networking.Elements;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Joostit.NeuralNerd.NnLib.UnitTests
{
    public class NeuronCoordinateTests
    {

        [Test]
        public void TestConstructor()
        {
            NeuronCoordinate coord = new NeuronCoordinate(2, 3);
            ClassicAssert.AreEqual(2, coord.Layer, coord.Row);
            ClassicAssert.AreNotEqual(0, coord.GetHashCode());
        }

        [Test]
        public void TestHasChangedAfterPropertyChange()
        {
            NeuronCoordinate coord = new NeuronCoordinate(2, 3);

            int firstHash = coord.GetHashCode();
            coord.Layer = 5;

            int secondHash = coord.GetHashCode();
            ClassicAssert.AreNotEqual(firstHash, secondHash);

            coord.Row = 12;

            int thirdHash = coord.GetHashCode();
            ClassicAssert.AreNotEqual(secondHash, thirdHash);
        }

        [Test]
        public void TestEqualCoordinates()
        {
            NeuronCoordinate coordA = new NeuronCoordinate(2, 3);
            NeuronCoordinate coordB = new NeuronCoordinate(2, 3);

            ClassicAssert.IsTrue(coordA.Equals(coordB));
            ClassicAssert.AreEqual(coordA.GetHashCode(), coordB.GetHashCode());
            ClassicAssert.AreEqual(coordA, coordB);
        }

        [Test]
        public void TestNonEqualCoordinates()
        {
            NeuronCoordinate coordA = new NeuronCoordinate(2, 3);
            NeuronCoordinate coordB = new NeuronCoordinate(4, 5);

            ClassicAssert.IsFalse(coordA.Equals(coordB));
            ClassicAssert.AreNotEqual(coordA.GetHashCode(), coordB.GetHashCode());
            ClassicAssert.AreNotEqual(coordA, coordB);

            ClassicAssert.IsFalse(coordA.Equals(null));
        }
    }
}