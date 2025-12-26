using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Models
{
    [DataContract]
    public class ResultObject
    {
        [DataMember]
        public int Flag { get; set; }

        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public object Result { get; set; }
        [DataMember]
        public object Count { get; set; }
        [DataMember]
        public object Subsidiary { get; set; }
        [DataMember]
        public object Subsidiary2 { get; set; }
   
    }
}
