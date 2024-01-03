using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Domain.MovementsAndNotifications.Notification
{
    class AffectedByNotificationDetails
    {
        byte[] AffectedStructure;
        byte[] AffectedConstraint;
    }
    public class ESDALAFFECTEDSTRUCT : INullable, IOracleCustomType
    {
        private bool MbIsNull;

        [OracleObjectMappingAttribute("NOTIFICATION_ID")]
        public long NotificationId
        { get; set; }

        [OracleObjectMappingAttribute("CODE")]
        public string Code
        { get; set; }


        [OracleObjectMappingAttribute("STRUCTURESECTION_ID")]
        public long StructureSectionId
        { get; set; }


        public virtual bool IsNull
        {
            get
            {
                return MbIsNull;
            }
        }

        // SUProject.Null is used to return a NULL SUProject object
        public static ESDALAFFECTEDSTRUCT Null
        {
            get
            {
                ESDALAFFECTEDSTRUCT p = new ESDALAFFECTEDSTRUCT();
                p.MbIsNull = true;
                return p;
            }
        }

        public void FromCustomObject(OracleConnection con, IntPtr pUdt)
        {
            OracleUdt.SetValue(con, pUdt, "NOTIFICATION_ID", NotificationId);
            OracleUdt.SetValue(con, pUdt, "CODE", Code);
            OracleUdt.SetValue(con, pUdt, "STRUCTURESECTION_ID", StructureSectionId);
        }

        public void ToCustomObject(OracleConnection con, IntPtr pUdt)
        {
            NotificationId = (long)OracleUdt.GetValue(con, pUdt, "NOTIFICATION_ID");
            Code = (string)OracleUdt.GetValue(con, pUdt, "CODE");
            StructureSectionId = (long)OracleUdt.GetValue(con, pUdt, "STRUCTURESECTION_ID");
        }

    }

    [OracleCustomTypeMappingAttribute("PORTAL.ESDALAFFECTEDSTRUCT")]
    public class EsdalStructuresFactory : IOracleCustomTypeFactory
    {
        // Implementation of IOracleCustomTypeFactory.CreateObject()
        public IOracleCustomType CreateObject()
        {
            // Return a new custom object
            return new ESDALAFFECTEDSTRUCT();
        }
    }

    public class ESDALAFFECTEDSTRUCTARRAY : INullable, IOracleCustomType
    {
        [OracleArrayMapping()]
        public ESDALAFFECTEDSTRUCT[] ESDALAffectedStructure
        { get; set; }

        private bool MbIsNull;
        public virtual bool IsNull
        {
            get
            {
                return MbIsNull;
            }
        }
        public static ESDALAFFECTEDSTRUCTARRAY Null
        {
            get
            {
                ESDALAFFECTEDSTRUCTARRAY p = new ESDALAFFECTEDSTRUCTARRAY();
                p.MbIsNull = true;
                return p;
            }
        }


        public void FromCustomObject(OracleConnection con, IntPtr pUdt)
        {
            OracleUdt.SetValue(con, pUdt, 0, ESDALAffectedStructure);
        }

        public void ToCustomObject(OracleConnection con, IntPtr pUdt)
        {
            ESDALAffectedStructure = (ESDALAFFECTEDSTRUCT[])OracleUdt.GetValue(con, pUdt, 0);
        }
    }

    [OracleCustomTypeMapping("PORTAL.ESDALAFFECTEDSTRUCTARRAY")]
    public class ESDALAFFECTEDSTRUCTARRAYFactory : IOracleCustomTypeFactory, IOracleArrayTypeFactory
    {
        #region IOracleCustomTypeFactory Members
        public IOracleCustomType CreateObject()
        {
            return new ESDALAFFECTEDSTRUCTARRAY();
        }

        #endregion

        #region IOracleArrayTypeFactory Members
        public Array CreateArray(int numElems)
        {
            return new ESDALAFFECTEDSTRUCT[numElems];
        }

        public Array CreateStatusArray(int numElems)
        {
            return null;
        }
        #endregion
    }
    public class ESDALAFFECTEDCONSTR : INullable, IOracleCustomType
    {
        private bool MbIsNull;

        [OracleObjectMappingAttribute("NOTIFICATION_ID")]
        public long NotificationId
        { get; set; }

        [OracleObjectMappingAttribute("CODE")]
        public string Code
        { get; set; }


        public virtual bool IsNull
        {
            get
            {
                return MbIsNull;
            }
        }

        // SUProject.Null is used to return a NULL SUProject object
        public static ESDALAFFECTEDCONSTR Null
        {
            get
            {
                ESDALAFFECTEDCONSTR p = new ESDALAFFECTEDCONSTR();
                p.MbIsNull = true;
                return p;
            }
        }

        public void FromCustomObject(OracleConnection con, IntPtr pUdt)
        {
            OracleUdt.SetValue(con, pUdt, "NOTIFICATION_ID", NotificationId);
            OracleUdt.SetValue(con, pUdt, "CODE", Code);
        }

        public void ToCustomObject(OracleConnection con, IntPtr pUdt)
        {
            NotificationId = (long)OracleUdt.GetValue(con, pUdt, "NOTIFICATION_ID");
            Code = (string)OracleUdt.GetValue(con, pUdt, "CODE");
        }

    }

    [OracleCustomTypeMappingAttribute("PORTAL.ESDALAFFECTEDCONSTR")]
    public class EsdalConstraintsFactory : IOracleCustomTypeFactory
    {
        // Implementation of IOracleCustomTypeFactory.CreateObject()
        public IOracleCustomType CreateObject()
        {
            // Return a new custom object
            return new ESDALAFFECTEDCONSTR();
        }
    }

    public class ESDALAFFECTEDCONSTRARRAY : INullable, IOracleCustomType
    {
        [OracleArrayMapping()]
        public ESDALAFFECTEDCONSTR[] ESDALAffectedConstraint
        { get; set; }

        private bool MbIsNull;
        public virtual bool IsNull
        {
            get
            {
                return MbIsNull;
            }
        }
        public static ESDALAFFECTEDCONSTRARRAY Null
        {
            get
            {
                ESDALAFFECTEDCONSTRARRAY p = new ESDALAFFECTEDCONSTRARRAY();
                p.MbIsNull = true;
                return p;
            }
        }


        public void FromCustomObject(OracleConnection con, IntPtr pUdt)
        {
            OracleUdt.SetValue(con, pUdt, 0, ESDALAffectedConstraint);
        }

        public void ToCustomObject(OracleConnection con, IntPtr pUdt)
        {
            ESDALAffectedConstraint = (ESDALAFFECTEDCONSTR[])OracleUdt.GetValue(con, pUdt, 0);
        }
    }

    [OracleCustomTypeMapping("PORTAL.ESDALAFFECTEDCONSTRARRAY")]
    public class ESDALAFFECTEDCONSTRARRAYyFactory : IOracleCustomTypeFactory, IOracleArrayTypeFactory
    {
        #region IOracleCustomTypeFactory Members
        public IOracleCustomType CreateObject()
        {
            return new ESDALAFFECTEDCONSTRARRAY();
        }

        #endregion

        #region IOracleArrayTypeFactory Members
        public Array CreateArray(int numElems)
        {
            return new ESDALAFFECTEDCONSTR[numElems];
        }

        public Array CreateStatusArray(int numElems)
        {
            return null;
        }
        #endregion
    }

}