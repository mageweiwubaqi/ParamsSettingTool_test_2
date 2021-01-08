using System;
using System.Collections.Generic;
using System.Linq;

namespace ITL.Public
{
    /// <summary>
    /// Hint消息回调返回参数
    /// </summary>
    public class HintArgs
    {
        
        public bool IsStop { get; set; } = false;

        public string LastErrorMsg { get; set; }

        public object Obj { get; set; }
    }
}
