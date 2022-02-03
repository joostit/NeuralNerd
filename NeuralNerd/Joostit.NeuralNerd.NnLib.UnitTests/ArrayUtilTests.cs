using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Joostit.NeuralNerd.NnLib.Utils;
using NUnit.Framework;

namespace Joostit.NeuralNerd.NnLib.UnitTests
{
    public class ArrayUtilTests
    {


        [Test]
        public void TestSingleDimClone()
        {
            int[] orig = { 1,2,3,4,5,6 };

            int[] clone = ArrayUtil.CloneArray(orig);

            Assert.AreEqual(orig.Length, clone.Length);

            Assert.AreEqual(orig[0], clone[0]);
            Assert.AreEqual(orig[3], clone[3]);
            Assert.AreEqual(orig[5], clone[5]);

        }


        [Test]
        public void TestDoubleDimClone()
        {
            int[] orig1 = { 1, 2, 3, 4, 5, 6 };
            int[] orig2 = { 21, 22, 23, 24, 25, 26 };
            int[] orig3 = { 31, 32, 33, 34, 35, 36 };

            int[][] orig = { orig1, orig2, orig3 };

            int[][] clone = ArrayUtil.CloneDoubleArray(orig);

            Assert.AreEqual(orig.Length, clone.Length);

            Assert.AreEqual(orig[0].Length, clone[0].Length);
            Assert.AreEqual(orig[0][0], clone[0][0]);
            Assert.AreEqual(orig[0][3], clone[0][3]);
            Assert.AreEqual(orig[0][5], clone[0][5]);

            Assert.AreEqual(orig[1].Length, clone[1].Length);
            Assert.AreEqual(orig[1][0], clone[1][0]);
            Assert.AreEqual(orig[1][3], clone[1][3]);
            Assert.AreEqual(orig[1][5], clone[1][5]);

            Assert.AreEqual(orig[1].Length, clone[1].Length);
            Assert.AreEqual(orig[2][0], clone[2][0]);
            Assert.AreEqual(orig[2][3], clone[2][3]);
            Assert.AreEqual(orig[2][5], clone[2][5]);


        }


        [Test]
        public void TestTripleDimClone()
        {
            int[] orig11 = { 111, 112, 113, 114, 115, 116 };
            int[] orig12 = { 121, 122, 123, 124, 125, 126 };
            int[] orig13 = { 131, 132, 133, 134, 135, 136 };
            int[][] orig1 = { orig11, orig12, orig13 };

            int[] orig21 = { 211, 212, 213, 214, 215, 216 };
            int[] orig22 = { 221, 222, 223, 224, 225, 226 };
            int[] orig23 = { 231, 232, 233, 234, 235, 236 };
            int[][] orig2 = { orig21, orig22, orig23 };

            int[] orig31 = { 311, 312, 313, 314, 315, 316 };
            int[] orig32 = { 321, 322, 323, 324, 325, 326 };
            int[] orig33 = { 331, 332, 333, 334, 335, 336 };
            int[][] orig3 = { orig11, orig12, orig13 };

            int[][][] orig = { orig1, orig2, orig3 };

            int[][][] clone = ArrayUtil.CloneTripleArray(orig);

            Assert.AreEqual(orig.Length, clone.Length);

            Assert.AreEqual(orig[0].Length, clone[0].Length);

            Assert.AreEqual(orig[0][0].Length, clone[0][0].Length);
            Assert.AreEqual(orig[0][0][0], clone[0][0][0]);
            Assert.AreEqual(orig[0][0][3], clone[0][0][3]);
            Assert.AreEqual(orig[0][0][5], clone[0][0][5]);

            Assert.AreEqual(orig[0][1].Length, clone[0][1].Length);
            Assert.AreEqual(orig[0][1][0], clone[0][1][0]);
            Assert.AreEqual(orig[0][1][3], clone[0][1][3]);
            Assert.AreEqual(orig[0][1][5], clone[0][1][5]);

            Assert.AreEqual(orig[0][2].Length, clone[0][2].Length);
            Assert.AreEqual(orig[0][2][0], clone[0][2][0]);
            Assert.AreEqual(orig[0][2][3], clone[0][2][3]);
            Assert.AreEqual(orig[0][2][5], clone[0][2][5]);


            Assert.AreEqual(orig[1].Length, clone[1].Length);

            Assert.AreEqual(orig[1][0].Length, clone[1][0].Length);
            Assert.AreEqual(orig[1][0][0], clone[1][0][0]);
            Assert.AreEqual(orig[1][0][3], clone[1][0][3]);
            Assert.AreEqual(orig[1][0][5], clone[1][0][5]);

            Assert.AreEqual(orig[1][1].Length, clone[1][1].Length);
            Assert.AreEqual(orig[1][1][0], clone[1][1][0]);
            Assert.AreEqual(orig[1][1][3], clone[1][1][3]);
            Assert.AreEqual(orig[1][1][5], clone[1][1][5]);

            Assert.AreEqual(orig[1][2].Length, clone[1][2].Length);
            Assert.AreEqual(orig[1][2][0], clone[1][2][0]);
            Assert.AreEqual(orig[1][2][3], clone[1][2][3]);
            Assert.AreEqual(orig[1][2][5], clone[1][2][5]);


            Assert.AreEqual(orig[2].Length, clone[2].Length);

            Assert.AreEqual(orig[2][0].Length, clone[2][0].Length);
            Assert.AreEqual(orig[2][0][0], clone[2][0][0]);
            Assert.AreEqual(orig[2][0][3], clone[2][0][3]);
            Assert.AreEqual(orig[2][0][5], clone[2][0][5]);

            Assert.AreEqual(orig[2][1].Length, clone[2][1].Length);
            Assert.AreEqual(orig[2][1][0], clone[2][1][0]);
            Assert.AreEqual(orig[2][1][3], clone[2][1][3]);
            Assert.AreEqual(orig[2][1][5], clone[2][1][5]);

            Assert.AreEqual(orig[2][2].Length, clone[2][2].Length);
            Assert.AreEqual(orig[2][2][0], clone[2][2][0]);
            Assert.AreEqual(orig[2][2][3], clone[2][2][3]);
            Assert.AreEqual(orig[2][2][5], clone[2][2][5]);



        }

    }
}
