using System;
using System.Collections.Generic;
using System.Linq;

namespace ITL.Public
{
    /// <summary>
    /// Hint��Ϣ�ص����ز���
    /// </summary>
    public class HintArgs
    {
        
        public bool IsStop { get; set; } = false;

        public string LastErrorMsg { get; set; }

        public object Obj { get; set; }
    }
}
