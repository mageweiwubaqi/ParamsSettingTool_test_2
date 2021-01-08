using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITL.DataDefine
{
    //[DataContract]
    //public class FloorRelationInfo : Attribute
    public class FloorRelationBaseInfo : FloorRelationInfo
    {
        public FloorRelationBaseInfo(string id, string floorName, string actualFloorNum, string terminalNum, string detectedFloorNum)
        {
            this.id = id;
            this.floorName = floorName;
            this.actualFloorNum = actualFloorNum;
            this.terminalNum = terminalNum;
            this.detectedFloorNum = detectedFloorNum;
        }


        //public string actualFloorNum { get; set; }
        ////[DataMember]
        //public string detectedFloorNum { get; set; }

        ////[DataMember]
        //public string floorName { get; set; }

        ////[DataMember]
        //public string id { get; set; }

        ////[DataMember]
        //public string terminalNum { get; set; }

        //public string terminalNumIntercom { get; set; }
    }

}