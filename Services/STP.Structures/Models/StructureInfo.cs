using NetSdoGeometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace STP.Structures.Models
{
    #region StructureInfo
    /// <summary>
    /// Structure Info Class
    /// </summary>
    public class StructureInfo
    {

     
        public string StructureDescr { get; set; }
     
        public string StructureName { get; set; }
     
        public long StructureId { get; set; }
     
        public string StructureCode { get; set; }
     
        public string StructureClass { get; set; }
     
        public string SectionDescr { get; set; }
     
        public long SectionId { get; set; }
     
        public long SectionNo { get; set; }

     
        public long Northing { get; set; }
     
        public long Easting { get; set; }

     
        public int FromNorthing { get; set; }
     
        public int FromEasting { get; set; }
     
        public int ToNorthing { get; set; }
     
        public int ToEasting { get; set; }


     
        public sdogeometry PointGeometry { get; set; }
     
        public sdogeometry LineGeometry { get; set; }
     
        public SDOPOINT Point { get; set; }
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
    #region RoadContact
    
    public class RoadContactModal
    {
        public string ContactName { get; set; }

        public int Position { get; set; }

        public string Description { get; set; }

        public long ContactId { get; set; }

        public short ContactNo { get; set; }

        public string FirstName { get; set; }

        public string FullName { get; set; }

        public string OrganisationName { get; set; }
        public string OrganisationType { get; set; }

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
    }
    #endregion
}