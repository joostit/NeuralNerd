using Joostit.NeuralNerd.NnLib.Networking.Elements;
using NUnit.Framework;

namespace Joostit.NeuralNerd.NnLib.UnitTests
{
    public class NeuronCoordinateTests
    {

        [Test]
        public void TestConstructor()
        {
            NeuronCoordinate coord = new NeuronCoordinate(2, 3);
            Assert.AreEqual(2, coord.Layer, coord.Row);
            Assert.AreNotEqual(0, coord.GetHashCode());
        }

        [Test]
        public void TestHasChangedAfterPropertyChange()
        {
            NeuronCoordinate coord = new NeuronCoordinate(2, 3);

            int firstHash = coord.GetHashCode();
            coord.Layer = 5;

            int secondHash = coord.GetHashCode();
            Assert.AreNotEqual(firstHash, secondHash);

            coord.Row = 12;

            int thirdHash = coord.GetHashCode();
            Assert.AreNotEqual(secondHash, thirdHash);
        }

        [Test]
        public void TestEqualCoordinates()
        {
            NeuronCoordinate coordA = new NeuronCoordinate(2, 3);
            NeuronCoordinate coordB = new NeuronCoordinate(2, 3);

            Assert.IsTrue(coordA.Equals(coordB));
            Assert.AreEqual(coordA.GetHashCode(), coordB.GetHashCode());
            Assert.AreEqual(coordA, coordB);
        }

        [Test]
        public void TestNonEqualCoordinates()
        {
            NeuronCoordinate coordA = new NeuronCoordinate(2, 3);
            NeuronCoordinate coordB = new NeuronCoordinate(4, 5);

            Assert.IsFalse(coordA.Equals(coordB));
            Assert.AreNotEqual(coordA.GetHashCode(), coordB.GetHashCode());
            Assert.AreNotEqual(coordA, coordB);

            Assert.IsFalse(coordA.Equals(null));
        }
    }
}