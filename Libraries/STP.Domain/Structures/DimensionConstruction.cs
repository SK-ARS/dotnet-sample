using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.Domain.Structures
{
    public class DimensionConstruction
    {
        public string Desc { get; set; }
        public string ObjectCarried { get; set; }
        public string ObjectCrossed { get; set; }
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
        public string ConstructionType1 { get; set; }
        public string ConstructionType2 { get; set; }
        public string ConstructionType3 { get; set; }
        public string DeckMaterial1 { get; set; }
        public string DeckMaterial2 { get; set; }
        public string DeckMaterial3 { get; set; }
        public string BearingsType1 { get; set; }
        public string BearingsType2 { get; set; }
        public string BearingsType3 { get; set; }
        public string FoundationType1 { get; set; }
        public string FoundationType2 { get; set; }
        public string FoundationType3 { get; set; }
        [RegularExpression(@"^[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*$", ErrorMessage = "Please enter valid carriageway widths")]
        public double? CarrigeWayWidth { get; set; }
        [RegularExpression(@"^[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*$", ErrorMessage = "Please enter valid deck widths")]
        public double? DeckWidth { get; set; }
        public long CarrigeWayId { get; set; }
        public long DeckId { get; set; }
        public string Chainage { get; set; }
        public string TxtConstructionType1 { get; set; }
        public string TxtConstructionType2 { get; set; }
        public string TxtConstructionType3 { get; set; }
        public string TxtDeckMaterial1 { get; set; }
        public string TxtDeckMaterial2 { get; set; }
        public string TxtDeckMaterial3 { get; set; }
        public string TxtBearingsType1 { get; set; }
        public string TxtBearingsType2 { get; set; }
        public string TxtBearingsType3 { get; set; }
        public string TxtFoundationType1 { get; set; }
        public string TxtFoundationType2 { get; set; }
        public string TxtFoundationType3 { get; set; }
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
        public int? Sequence { get; set; }
        [Required(ErrorMessage = "The span position is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid number")]
        public int? Position { get; set; }
        [Required(ErrorMessage = "The span length is required")]
        [RegularExpression(@"^[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*$", ErrorMessage = "Please enter valid length")]
        public double Length { get; set; }
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
        public string TxtStructType1 { get; set; }
        public string TxtStructType2 { get; set; }
        public string TxtStructType3 { get; set; }
        public string TxtConstructionType1 { get; set; }
        public string TxtConstructionType2 { get; set; }
        public string TxtConstructionType3 { get; set; }
        public string TxtDeckMaterial1 { get; set; }
        public string TxtDeckMaterial2 { get; set; }
        public string TxtDeckMaterial3 { get; set; }
        public string TxtBearingsType1 { get; set; }
        public string TxtBearingsType2 { get; set; }
        public string TxtBearingsType3 { get; set; }
        public string TxtFoundationType1 { get; set; }
        public string TxtFoundationType2 { get; set; }
        public string TxtFoundationType3 { get; set; }
        public string UserName { get; set; }
    }
}