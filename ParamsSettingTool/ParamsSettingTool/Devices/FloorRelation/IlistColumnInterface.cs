using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITL.ParamsSettingTool
{
    //public class IlistColumnInterface
    //{
    //}
    #region 运行时绑定到实现Ilist接口的数据源

    public class IlistColumnInterface
    {
        int id;
       // DateTime birth;
        string colAuthFlag, colFloor, colKeyName, colActualFloor, colDevNo, colDevCheckFloor;
        //float math, chinese, english;

        //权限标识、按键名称、实际楼层、端子号、检测楼层
        public IlistColumnInterface(int id, string colAuthFlag, /*string colFloor,*/  string colKeyName, string colActualFloor, string colDevNo, string colDevCheckFloor)
        {
            this.id = id;
            this.colAuthFlag = colAuthFlag;
            //this.colFloor = colFloor;
            this.colKeyName = colKeyName;
            this.colActualFloor = colActualFloor;
            this.colDevNo = colDevNo;
            this.colDevCheckFloor = colDevCheckFloor;
           // this.birth = birth;
            //this.math = math;
            //this.chinese = chinese;
            //this.english = english;
            //this.remark = remark;
        }
        public int ID { get { return id; } }
        public string AuthFlag
        {
            get { return colAuthFlag; }
            set { colAuthFlag = value; }
        }
        public string Floor
        {
            get { return colFloor; }
            set { colFloor = value; }
        }
        public string KeyName
        {
            get { return colKeyName; }
            set { colKeyName = value; }
        }
        public string ActualFloor
        {
            get { return colActualFloor; }
            set { colActualFloor = value; }
        }
        public string DevNo
        {
            get { return colDevNo; }
            set { colDevNo = value; }
        }
        public string DevCheckFloor
        {
            get { return colDevCheckFloor; }
            set { colDevCheckFloor = value; }
        }
        //  public DateTime Birth
        //  {
        //     get { return birth; }
        //    set { birth = value; }
        // }
        //public float Math
        //{
        //    get { return math; }
        //    set { math = value; }
        //}
        //public float Chinese
        //{
        //    get { return chinese; }
        //    set { chinese = value; }
        //}
        //public float English
        //{
        //    get { return english; }
        //    set { english = value; }
        //}



    }

    #endregion

}
