using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;

namespace ITL.DataDefine
{
    //[DataContract]
    //public class FloorRelationInfo : Attribute
    public class FloorRelationInfo
    {

        public string actualFloorNum { get; set; } 
        //[DataMember]
        public string detectedFloorNum { get; set; }

        //[DataMember]
        public string floorName { get; set; }

        //[DataMember]
        public string id { get; set; }

        //[DataMember]
        public string terminalNum { get; set; }
        //[DataMember]
        public string terminalNumIntercom { get; set; }
        //[DataMember]
        public string terminalNumSlave1 { get; set; }
        //[DataMember]
        public string terminalNumSlave2 { get; set; }

    }

}