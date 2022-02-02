using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joostit.NeuralNerd.NnLib.Learning
{
    internal interface ILearnTaskDispatcher
    {

        /// <summary>
        /// Should be called by a learning task, with the latest cost result
        /// </summary>
        /// <param name="lastCost">The cost that was achieved with the previous task</param>
        /// <param name="lastParameters">The parameters that were used to achieve this cost, or null if this is the first task</param>
        /// <returns>A new set of parameters</returns>
        NetworkLearnParameters GetNextTask(double lastCost, NetworkLearnParameters lastParameters);

    }
}
