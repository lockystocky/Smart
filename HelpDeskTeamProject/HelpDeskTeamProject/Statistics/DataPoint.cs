using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HelpDeskTeamProject.Statistics
{
    [DataContract]
    public class DataPoint
    {
        public DataPoint(int y, string indexLabel)
        {
            this.IndexLabel = indexLabel;
            this.Y = y;
        }

        [DataMember(Name = "email")]
        public string IndexLabel = null;

        [DataMember(Name = "y")]
        public Nullable<int> Y = null;
    }
}