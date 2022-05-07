using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamepadCmdMapper
{
    /// <summary>
    /// Manage command line arguments
    /// </summary>
    public class GamepadCmdArgs
    {
        public string[] Args { get; set; }

        public string FilePath => Args != null && Args.Length > 0 ? Args[0] : string.Empty;
    }
}
