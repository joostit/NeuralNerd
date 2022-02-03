using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joostit.NeuralNerd.NnLib.Utils
{
    public static class ArrayUtil
    {

        public static TArray[][] CloneDoubleArray<TArray>(TArray[][] source)
        {
            var len = source.Length;
            var dest = new TArray[len][];

            for (var x = 0; x < len; x++)
            {
                var inner = source[x];
                var ilen = inner.Length;
                var newer = new TArray[ilen];
                Array.Copy(inner, newer, ilen);
                dest[x] = newer;
            }

            return dest;
        }


        public static TArray[][][] CloneTripleArray<TArray>(TArray[][][] source)
        {
            TArray[][][] dest = new TArray[source.Length][][];
            for (int i = 0; i < source.Length; i++)
            {
                dest[i] = CloneDoubleArray(source[i]);
            }

            return dest;
        }


        public static TArray[] CloneArray<TArray>(TArray[] source)
        {
            TArray[] retVal = new TArray[source.Length];
            Array.Copy(source, retVal, source.Length);
            return retVal;
        }


    }
}
