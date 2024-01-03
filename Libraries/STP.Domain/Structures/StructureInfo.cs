using NetSdoGeometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;


namespace STP.Domain.Structures
{
    #region StructureInfo
    /// <summary>
    /// Structure Info Class
    /// </summary>
    public class StructureInfo
    {

        [DataMember]
        public string StructureDescr { get; set; }
        [DataMember]
        public string StructureName { get; set; }
        [DataMember]
        public long StructureId { get; set; }
        [DataMember]
        public string StructureCode { get; set; }
        [DataMember]
        public string StructureClass { get; set; }
        [DataMember]
        public string SectionDescr { get; set; }
        [DataMember]
        public long SectionId { get; set; }
        [DataMember]
        public long SectionNo { get; set; }

        [DataMember]
        public long Northing { get; set; }
        [DataMember]
        public long Easting { get; set; }

        [DataMember]
        public int FromNorthing { get; set; }
        [DataMember]
        public int FromEasting { get; set; }
        [DataMember]
        public int ToNorthing { get; set; }
        [DataMember]
        public int ToEasting { get; set; }


        [DataMember]
        public sdogeometry PointGeometry { get; set; }

        [DataMember]
        public sdogeometry LineGeometry { get; set; }

        [DataMember]
        public SDOPOINT Point { get; set; }
        
        [DataMember]
        public List<string> Suitability { get; set; }
    };


    #endregion
    #region StructureContact
    /// <summary>
    /// StructureContact
    /// </summary>
    
    public class StructureContact
    {
        public long OwnerId { get; set; }
        public string OwnerName { get; set; }
        public long ContactId { get; set; }
        public string ContactName { get; set; }
        public int Position { get; set; }
        public string Description { get; set; }
        public short ContactNo { get; set; }
        public string FullName { get; set; }
        public string OrganisationName { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string AddressLine4 { get; set; }

        public string AddressLine5 { get; set; }

        public string PostCode { get; set; }

        public int CountryId { get; set; }

        public string CountryName { get; set; }

        public string Telephone { get; set; }

        public string Extension { get; set; }

        public string Mobile { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public string EmailPreference { get; set; }

        public long CautionId { get; set; }

        public decimal TotalRecordCount { get; set; }

        public object OrganisationId { get; set; }

        public object RoleType { get; set; }

        public object IsAdHoc { get; set; }

        public object UserSchema { get; set; }

    };
    #endregion

}