using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joostit.NeuralNerd.NnLib.Learning
{
    internal interface ILearnTaskDispatcher
    {

        void GetNextTask(double lastCost);

    }
}
