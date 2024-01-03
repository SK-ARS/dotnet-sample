
using System.Xml.Serialization;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.Generic;

namespace STP.Domain.RouteAssessment.XmlAnalysedAnnotations
{
    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    [XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/routeanalysis", IsNullable = false)]
    public partial class AnalysedAnnotations
    {

        /// <remarks/>
        [XmlElementAttribute("AnalysedAnnotationsPart")]
        public List<AnalysedAnnotationsPart> AnalysedAnnotationsPart
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class AnalysedAnnotationsPart
    {

        /// <remarks/>
        public string Name
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute("Annotation")]
        public List<Annotation> Annotation
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int Id
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class Annotation
    {

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/annotation")]
        public Text Text
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute("Road")]
        public AnnotationRoad Road
        {
            get;
            set;
        }

        /// <remarks/>
        public AnnotatedEntity AnnotatedEntity
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public int AnnotationId
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttributeAttribute()]
        public string AnnotationType
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/annotation")]
    [XmlRootAttribute(Namespace = "http://www.esdal.com/schemas/core/annotation", IsNullable = false)]
    public partial class Text
    {

        /// <remarks/>
        [XmlElementAttribute("Bold", Namespace = "http://www.esdal.com/schemas/core/formattedtext")]
        public List<string> Bold
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlTextAttribute()]
        public List<string> TextValue
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class AnnotationRoad
    {

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
        public string Name
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElementAttribute(Namespace = "http://www.esdal.com/schemas/core/esdalcommontypes")]
        public string Number
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class AnnotatedEntity
    {

        /// <remarks/>
        public AnnotedEntityRoad Road
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.esdal.com/schemas/core/routeanalysis")]
    public partial class AnnotedEntityRoad
    {
        /// <remarks/>
        public string OSGridRef
        {
            get;
            set;
        }
    }

}