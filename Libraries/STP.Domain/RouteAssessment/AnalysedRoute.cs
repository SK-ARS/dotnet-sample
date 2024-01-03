using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace STP.Domain.RouteAssessment
{
    public class AnalysedRoute
    {
        [DataMember]
        public string RouteAnalysisXml { get; set; }

        [DataMember]
        public byte[] RouteAnalysisBlob { get; set; }

        [DataMember]
        public long PreviousAnalysisId { get; set; }

        [DataMember]
        public long NewAnalysisId { get; set; }

        [DataMember]
        public AnalysisType AnalysisType { get; set; }

        public AnalysedRoute()
        {
            PreviousAnalysisId = 0;
            RouteAnalysisXml = null;
            RouteAnalysisBlob = null;
        }
    }

    public enum AnalysisType
    {

        /// <remarks/>
        structure,

        /// <remarks/>
        drivinginstructions,

        /// <remarks/>
        affectedparties,

        /// <remarks/>
        resolvedparties,

        /// <remarks/>
        routeanalysis,

        /// <remarks/>
        affectedconstraints,

        /// <remarks/>
        annotations,

        /// <remarks/>
        cautions,

        /// <remarks/>
        affectedroads,

    }
}