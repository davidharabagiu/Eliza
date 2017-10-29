using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eliza_Desktop_App
{
    class ElizaClientException : Exception
    {
        public bool ShouldTerminateProgram { get; private set; }

        public ElizaClientException(ElizaStatus status)
            : base(string.Format("Internal error: {0}.", status.ToString()))
        {
            ShouldTerminateProgram = true;
        }
    }
}
