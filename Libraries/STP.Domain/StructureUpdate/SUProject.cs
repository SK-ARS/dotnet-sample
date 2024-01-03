using Oracle.DataAccess.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STP.Domain.StructureUpdate
{
   public  class SUProject
    {
        private bool m_bIsNull;       // Whether the SUProject object is NULL 
        [OracleObjectMappingAttribute("PROJECT_ID")]
        public int PROJECT_ID { get; set; }
        [OracleObjectMappingAttribute("DATA_OWNER_ID")]
        public int DATA_OWNER_ID { get; set; }
        [OracleObjectMappingAttribute("VERSION")]
        public int VERSION { get; set; }
        [OracleObjectMappingAttribute("STATE")]
        public int STATE { get; set; }
        [OracleObjectMappingAttribute("PROJECT_NAME")]
        public string PROJECT_NAME { get; set; }
        //[OracleObjectMappingAttribute("ORGNAISATION_NAME")]
        public string ORGNAISATION_NAME { get; set; }
        [OracleObjectMappingAttribute("CREATED_ORG_ID")]
        public int CREATED_ORG_ID { get; set; }
        [OracleObjectMappingAttribute("CREATED_USER_ID")]
        public int CREATED_USER_ID { get; set; }
        [OracleObjectMappingAttribute("DATE_CREATED")]
        public DateTime DATE_CREATED { get; set; }
        [OracleObjectMappingAttribute("DATE_LAST_UPDATED")]
        public DateTime DATE_LAST_UPDATED { get; set; }
        public int totalRecordCount { get; set; }
        [OracleObjectMappingAttribute("DESCRIPTION")]
        public string DESCRIPTION { get; set; }
        [OracleObjectMappingAttribute("IS_COMPLETE")]
        public short IS_COMPLETE { get; set; }

    }
}
