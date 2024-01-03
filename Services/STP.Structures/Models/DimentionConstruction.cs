using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace STP.Structures.Models
{
    public class DimentionConstruction
    {
        public string Desc { get; set; }
        public string objectCarried { get; set; }
        public string objectCrossed { get; set; }
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Please enter valid skew angle")]
        public double? SkewAngle { get; set; }
        [RegularExpression(@"^[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*$", ErrorMessage = "Please enter valid length")]
        public double? Length { get; set; }
        [RegularExpression(@"^[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*$", ErrorMessage = "Please enter valid max span length")]
        public double? MaxLength { get; set; }
        [RegularExpression(@"^[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*$", ErrorMessage = "Please enter valid number of spans")]
        public long? SpansCount { get; set; }
        [RegularExpression(@"^[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*$", ErrorMessage = "Please enter valid number of decks")]
        public long? DecksCount { get; set; }
        public string ConstructionTypeOne { get; set; }
        public string ConstructionTypeTwo { get; set; }
        public string ConstructionTypeThree { get; set; }
        public string DeckMaterialOne { get; set; }
        public string DeckMaterialTwo { get; set; }
        public string DeckMaterialThree { get; set; }
        public string BearingsTypeOne { get; set; }
        public string BearingsTypeTwo { get; set; }
        public string BearingsTypeThree { get; set; }
        public string FoundationTypeOne { get; set; }
        public string FoundationTypeTwo { get; set; }
        public string FoundationTypeThree { get; set; }
        [RegularExpression(@"^[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*$", ErrorMessage = "Please enter valid carriageway widths")]
        public double? CarrigewayWidth { get; set; }
        [RegularExpression(@"^[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*$", ErrorMessage = "Please enter valid deck widths")]
        public double? DeckWidth { get; set; }
        public long CarrigewayID { get; set; }
        public long DeckID { get; set; }
        public string Chainage { get; set; }
        public string TxtConstructionTypeOne { get; set; }
        public string TxtConstructionTypeTwo { get; set; }
        public string TxtConstructionTypeThree { get; set; }
        public string TxtDeckMaterialOne { get; set; }
        public string TxtDeckMaterialTwo { get; set; }
        public string TxtDeckMaterialThree { get; set; }
        public string TxtBearingsTypeOne { get; set; }
        public string TxtBearingsTypeTwo { get; set; }
        public string TxtBearingsTypeThree { get; set; }
        public string TxtFoundationTypeOne { get; set; }
        public string TxtFoundationTypeTwo { get; set; }
        public string TxtFoundationTypeThree { get; set; }
        public string UserName { get; set; }
    }
    public class StucDDList
    {
        public long Code { get; set; }
        public string Name { get; set; }
    }
    public class SpanData
    {
        public long? SpanNo { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid number")]
        public int? sequence { get; set; }
        [Required(ErrorMessage = "The span position is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid number")]
        public int? position { get; set; }
        [Required(ErrorMessage = "The span length is required")]
        [RegularExpression(@"^[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*$", ErrorMessage = "Please enter valid length")]
        public double Len { get; set; }
        public string Description { get; set; }
        public string StructType1 { get; set; }
        public string StructType2 { get; set; }
        public string StructType3 { get; set; }
        public string ConstructionType1 { get; set; }
        public string ConstructionType2 { get; set; }
        public string ConstructionType3 { get; set; }
        public string DeckMaterial1 { get; set; }
        public string DeckMaterial2 { get; set; }
        public string DeckMaterial3 { get; set; }
        public string SpanBearing1 { get; set; }
        public string SpanBearing2 { get; set; }
        public string SpanBearing3 { get; set; }
        public string SpanFoundation1 { get; set; }
        public string SpanFoundation2 { get; set; }
        public string SpanFoundation3 { get; set; }
        public string TxtStructTypeOne { get; set; }
        public string TxtStructTypeTwo { get; set; }
        public string TxtStructTypeThree { get; set; }
        public string TxtConstructionTypeOne { get; set; }
        public string TxtConstructionTypeTwo { get; set; }
        public string TxtConstructionTypeThree { get; set; }
        public string TxtDeckMaterialOne { get; set; }
        public string TxtDeckMaterialTwo { get; set; }
        public string TxtDeckMaterialThree { get; set; }
        public string TxtBearingsTypeOne { get; set; }
        public string TxtBearingsTypeTwo { get; set; }
        public string TxtBearingsTypeThree { get; set; }
        public string TxtFoundationTypeOne { get; set; }
        public string TxtFoundationTypeTwo { get; set; }
        public string TxtFoundationTypeThree { get; set; }
        public string UserName { get; set; }
    }
}