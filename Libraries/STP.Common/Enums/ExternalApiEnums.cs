using System.ComponentModel;

namespace STP.Common.Enums
{
    public class ExternalApiEnums
    {
        public enum ExternalApiVehicleType
        {
            [Description("VT001")] VT001 = 244001,//drawbar vehicle
            [Description("VT002")] VT002 = 244002,//semi vehicle
            [Description("VT003")] VT003 = 244003,//rigid vehicle
            [Description("VT004")] VT004 = 244004,//tracked vehicle
            [Description("VT005")] VT005 = 244005,//spmt
            [Description("VT006")] VT006 = 244008,//Boat mast Exception
            [Description("VT007")] VT007 = 244009,//semi trailer(3-8) vehicle
            [Description("VT008")] VT008 = 244010,//drawbar trailer(3-8) vehicle
            [Description("VT009")] VT009 = 244011,//rigid and drag
            [Description("VT010")] VT010 = 244012,//crane
            [Description("VT011")] VT011 = 244013 //Recovery Vehicle
        }
        public enum ExternalApiMovementClassification
        {
            [Description("MC001")] MC001 = 270001,//C & U
            [Description("MC002")] MC002 = 270009,//stgo ail cat1
            [Description("MC003")] MC003 = 270010,//stgo ail cat2
            [Description("MC004")] MC004 = 270011,//stgo ail cat3
            [Description("MC005")] MC005 = 270012,//stgo mobile crane cat a
            [Description("MC006")] MC006 = 270013,//stgo mobile crane cat b
            [Description("MC007")] MC007 = 270014,//stgo mobile crane cat c
            [Description("MC008")] MC008 = 270004, //stgo engineering plant wheeled
            [Description("MC009")] MC009 = 270005,//stgo road recovery
            [Description("MC010")] MC010 = 270006,//special order
            [Description("MC011")] MC011 = 270007,//vehicle special order
            [Description("MC012")] MC012 = 270008,//tracked
            [Description("MC013")] MC013 = 270015,//stgo engineering plant tracked
            [Description("No movement classification")] MC017 = 270017
        }
        public enum ExternalApiMovementClassificationMapping
        {
            [Description("MC101")] MC101 = 270101,//No Notification
            [Description("MC102")] MC102 = 270102,//C & U Notification (POL-2)
            [Description("MC103")] MC103 = 270103,//STGO CAT 1 Notification (POL-2)
            [Description("MC104")] MC104 = 270104,//STGO CAT 1 Notification (SOA-2)
            [Description("MC105")] MC105 = 270105,//STGO CAT 1 Notification (SOA–2, POL-2)
            [Description("MC106")] MC106 = 270106,//STGO CAT 2 Notification (SOA-2)
            [Description("MC107")] MC107 = 270107,//STGO CAT 2 Notification (SOA-2, POL-2) 
            [Description("MC108")] MC108 = 270108,//STGO CAT 3 Notification (SOA-2)
            [Description("MC109")] MC109 = 270109,//STGO CAT 3 Notification (SOA-5, POL-2) 
            [Description("MC110")] MC110 = 270110,//STGO CAT 1 VR-1 Notification (POL-2) 
            [Description("MC111")] MC111 = 270111,//STGO CAT 2 VR-1 Notification (SOA-2, POL-2) 
            [Description("MC112")] MC112 = 270112,//STGO CAT 3 VR-1 Notification (SOA-5, POL-2) 
            [Description("MC113")] MC113 = 270113,//VSO Notification (SOA-2/POL-2/SOA-2, POL-2) 
            [Description("MC114")] MC114 = 270114,//Special Order Notification (SOA-5, POL-5) 
            [Description("MC115")] MC115 = 270115,//Tracked Vehicles Notification
            [Description("MC116")] MC116 = 270116,//SO Application
            [Description("MC117")] MC117 = 270117,//VR-1 Application
            [Description("MC118")] MC118 = 270118,//STGO CAT A (SOA-2)
            [Description("MC119")] MC119 = 270119,//STGO CAT A (SOA-2, POL-2)
            [Description("MC120")] MC120 = 270120,//STGO CAT B (SOA-2)
            [Description("MC121")] MC121 = 270121,//STGO CAT B (SOA-2, POL-2)
            [Description("MC122")] MC122 = 270122,//STGO CAT B (SOA-5, POL-2)
            [Description("MC123")] MC123 = 270123,//STGO CAT C (SOA-2)
            [Description("MC124")] MC124 = 270124,//STGO CAT C (SOA-2, POL-2) 
            [Description("MC125")] MC125 = 270125,//STGO CAT C (SOA-5, POL-2)
            [Description("MC126")] MC126 = 270126,//STGO engineering plant tracked CAT 1 Notification (POL-2)
            [Description("MC127")] MC127 = 270127,//STGO engineering plant wheeled CAT 1 Notification (POL-2)
            [Description("MC128")] MC128 = 270128,//STGO road recovery vehicle CAT 1 Notification (POL-2)
            [Description("MC129")] MC129 = 270129,//STGO tracked CAT 1 Notification (POL-2)
            [Description("MC130")] MC130 = 270130,//STGO engineering plant tracked CAT 1 Notification (SOA-2)
            [Description("MC131")] MC131 = 270131,//STGO engineering plant wheeled CAT 1 Notification (SOA-2)
            [Description("MC132")] MC132 = 270132,//STGO road recovery vehicle CAT 1 Notification (SOA-2)
            [Description("MC133")] MC133 = 270133,//STGO tracked CAT 1 Notification (SOA-2)
            [Description("MC134")] MC134 = 270134,//STGO engineering plant tracked CAT 1 Notification (SOA-2, POL-2)
            [Description("MC135")] MC135 = 270135,//STGO engineering plant wheeled CAT 1 Notification (SOA-2, POL-2)
            [Description("MC136")] MC136 = 270136,//STGO road recovery vehicle CAT 1 Notification (SOA-2, POL-2)
            [Description("MC137")] MC137 = 270137,//STGO tracked CAT 1 Notification (SOA-2, POL-2)
            [Description("MC138")] MC138 = 270138,//STGO engineering plant tracked CAT-2 Notification (SOA-2)   
            [Description("MC139")] MC139 = 270139,//STGO engineering plant wheeled CAT-2 Notification (SOA-2)
            [Description("MC140")] MC140 = 270140,//STGO road recovery vehicle CAT-2 Notification (SOA-2)
            [Description("MC141")] MC141 = 270141,//STGO tracked CAT-2 Notification (SOA-2)
            [Description("MC142")] MC142 = 270142,//STGO engineering plant tracked CAT-2 Notification (SOA-2, POL-2)
            [Description("MC143")] MC143 = 270143,//STGO engineering plant wheeled CAT-2 Notification (SOA-2, POL-2)
            [Description("MC144")] MC144 = 270144,//STGO road recovery vehicle CAT-2 Notification (SOA-2, POL-2)
            [Description("MC145")] MC145 = 270145,//STGO tracked CAT-2 Notification (SOA-2, POL-2)
            [Description("MC146")] MC146 = 270146,//STGO engineering plant tracked CAT-3 Notification (SOA-2)
            [Description("MC147")] MC147 = 270147,//STGO engineering plant wheeled CAT-3 Notification (SOA-2)
            [Description("MC148")] MC148 = 270148,//STGO road recovery vehicle CAT-3 Notification (SOA - 2)
            [Description("MC149")] MC149 = 270149,//STGO tracked CAT-3 Notification (SOA - 2)
            [Description("MC150")] MC150 = 270150,//STGO engineering plant tracked CAT-3 Notification (SOA-5, POL-2)
            [Description("MC151")] MC151 = 270151,//STGO engineering plant wheeled CAT-3 Notification (SOA-5, POL-2)
            [Description("MC152")] MC152 = 270152,//STGO road recovery vehicle CAT-3 Notification (SOA-5, POL-2)
            [Description("MC153")] MC153 = 270153,//STGO tracked CAT-3 Notification (SOA-5, POL-2)
            [Description("MC154")] MC154 = 270154,//C & U No Notification
            [Description("MC155")] MC155 = 270155,//No Notification: Non-Crane
            [Description("MC156")] MC156 = 270156,//STGO CAT 1 VR-1 Notification (SOA - 2, POL - 2)
            [Description("MC157")] MC157 = 270157,//STGO CAT 3 Notification (SOA – 2, POL - 2)
            [Description("MC158")] MC158 = 270158,//STGO CAT-1 VR-1 Engineering Plant - Wheeled (SOA - 2, POL - 2)
            [Description("MC159")] MC159 = 270159,//STGO CAT-1 VR-1 Engineering Plant - Wheeled (SOA - 2)
            [Description("MC160")] MC160 = 270160,//STGO CAT-1 VR-1 Engineering Plant - Wheeled (POL - 2)
            [Description("MC161")] MC161 = 270161,//STGO CAT-2 VR-1 Engineering Plant - Wheeled (SOA - 2, POL - 2)
            [Description("MC162")] MC162 = 270162,//STGO CAT-2 VR-1 Engineering Plant - Wheeled (SOA - 2)
            [Description("MC163")] MC163 = 270163,//STGO CAT-3 VR-1 Engineering Plant - Wheeled (SOA - 5, POL - 2)
            [Description("MC164")] MC164 = 270164,//STGO CAT-3 VR-1 Engineering Plant - Wheeled (SOA - 2)
            [Description("MC165")] MC165 = 270165,//STGO CAT-1 VR-1 Engineering Plant - Tracked (SOA - 2, POL - 2)
            [Description("MC166")] MC166 = 270166,//STGO CAT-1 VR-1 Engineering Plant - Tracked (SOA - 2)
            [Description("MC167")] MC167 = 270167,//STGO CAT-1 VR-1 Engineering Plant - Tracked (POL - 2)
            [Description("MC168")] MC168 = 270168,//STGO CAT-2 VR-1 Engineering Plant - Tracked (SOA - 2, POL - 2)
            [Description("MC169")] MC169 = 270169,//STGO CAT-2 VR-1 Engineering Plant - Tracked (SOA - 2)
            [Description("MC170")] MC170 = 270170,//STGO CAT-3 VR-1 Engineering Plant - Wheeled (SOA - 2, POL - 2)
            [Description("MC171")] MC171 = 270171,//STGO CAT-3 VR-1 Engineering Plant - Tracked (SOA - 2)
            [Description("MC172")] MC172 = 270172,//STGO CAT-1 VR-1 Road Recovery (SOA - 2, POL - 2)
            [Description("MC173")] MC173 = 270173,//STGO CAT-1 VR-1 Road Recovery (SOA - 2)
            [Description("MC174")] MC174 = 270174,//STGO CAT-1 VR-1 Road Recovery (POL - 2)
            [Description("MC175")] MC175 = 270175,//STGO CAT-2 VR-1 Road Recovery (SOA - 2, POL - 2)
            [Description("MC176")] MC176 = 270176,//STGO CAT-2 VR-1 Road Recovery (SOA - 2)
            [Description("MC177")] MC177 = 270177,//STGO CAT-3 VR-1 Road Recovery (SOA - 5, POL - 2)
            [Description("MC178")] MC178 = 270178 //STGO CAT-3 VR-1 Road Recovery (SOA - 2)
        }
        public enum ExternalApiGeneralClassificationType
        {
            [Description("GC001")] GC001 = 241001,//vehicle special order
            [Description("GC002")] GC002 = 241002,//special order
            [Description("GC003")] GC003 = 241003,//stgo ail cat 1
            [Description("GC004")] GC004 = 241004,//stgo ail cat 2
            [Description("GC005")] GC005 = 241005,//stgo ail cat 3
            [Description("GC006")] GC006 = 241006,//STGO Mobile Crane Cat A
            [Description("GC007")] GC007 = 241007,//STGO Mobile Crane Cat B
            [Description("GC008")] GC008 = 241008,//STGO Mobile Crane Cat C
            [Description("GC009")] GC009 = 241010,//STGO Road Recovery Vehicle
            [Description("GC010")] GC010 = 241011,//Wheeled Construction and Use
            [Description("GC011")] GC011 = 241012,//Tracked
            [Description("GC012")] GC012 = 241013,//STGO Engineering Plant Wheeled
            [Description("GC013")] GC013 = 241014, //STGO Engineering Plant Tracked
            [Description("No general classification")] GC015 = 241015 //No Vehicle Class
        }
        public enum ExternalApiGeneralClassificationTypeMapping
        {
            [Description("GC015")] GC015 = 241015,//No Vehicle Classification
            [Description("GC017")] GC017 = 241017,//stgo cat 1 engineering plant wheeled GC012
            [Description("GC018")] GC018 = 241018,//stgo cat 2 engineering plant wheeled GC012
            [Description("GC019")] GC019 = 241019,//stgo cat 3 engineering plant wheeled GC012
            [Description("GC020")] GC020 = 241020,//stgo cat 1 engineering plant tracked GC013
            [Description("GC021")] GC021 = 241021,//stgo cat 2 engineering plant tracked GC013
            [Description("GC022")] GC022 = 241022,//stgo cat 3 engineering plant tracked GC013
            [Description("GC023")] GC023 = 241023,//stgo cat 1 road recovery GC009
            [Description("GC024")] GC024 = 241024,//stgo cat 2 road recovery GC009
            [Description("GC025")] GC025 = 241025,//stgo cat 3 road recovery GC009
            [Description("GC026")] GC026 = 241026,//stgo vr-1 cat 1 engineering plant wheeled GC012
            [Description("GC027")] GC027 = 241027,//stgo vr-1 cat 2 engineering plant wheeled GC012
            [Description("GC028")] GC028 = 241028,//stgo vr-1 cat 3 engineering plant wheeled GC012
            [Description("GC029")] GC029 = 241029,//stgo vr-1 cat 1 engineering plant tracked GC013
            [Description("GC030")] GC030 = 241030,//stgo vr-1 cat 2 engineering plant tracked GC013
            [Description("GC031")] GC031 = 241031,//stgo vr-1 cat 3 engineering plant tracked GC013
            [Description("GC032")] GC032 = 241032,//stgo vr-1 cat 1 road recovery GC009
            [Description("GC033")] GC033 = 241033,//stgo vr-1 cat 2 road recovery GC009
            [Description("GC034")] GC034 = 241034 //stgo vr-1 cat 3 road recovery GC009
        }
        public enum ExternalApiComponentType
        {
            [Description("CT001")] CT001 = 234001,//ballast tractor
            [Description("CT002")] CT002 = 234002,//conventional tractor
            [Description("CT003")] CT003 = 234003,//rigid vehicle
            [Description("CT004")] CT004 = 234004,//tracked vehicle
            [Description("CT005")] CT005 = 234005,//semi trailer
            [Description("CT006")] CT006 = 234006,//drawbar trailer
            [Description("CT007")] CT007 = 234007,//spmt
            [Description("CT008")] CT008 = 234008,//Mobile Crane
            [Description("CT009")] CT009 = 234009,//Engineering Plant
            [Description("CT010")] CT010 = 234010,//Engineering Plant–Semi Trailer
            [Description("CT011")] CT011 = 234011,//Engineering Plant–Drawbar Trailer
            [Description("CT012")] CT012 = 234012,//Recovery Vehicle
            [Description("CT013")] CT013 = 234013 //Girder Set
        }
        public enum ExternalApiComponentSubType
        {
            [Description("CST001")] CST001 = 224001,//ballast tractor
            [Description("CST002")] CST002 = 224002,//conventional tractor
            [Description("CST003")] CST003 = 224003,//other tractor
            [Description("CST004")] CST004 = 224004,//semi trailer
            [Description("CST005")] CST005 = 224005,//semi low loader
            [Description("CST006")] CST006 = 224006,//trombone trailer
            [Description("CST007")] CST007 = 224007,//other semi trailer
            [Description("CST008")] CST008 = 224008,//drawbar trailer
            [Description("CST009")] CST009 = 224009,//other drawbar trailer
            [Description("CST010")] CST010 = 224010,//bogie
            [Description("CST011")] CST011 = 224011,//twin bogies
            [Description("CST012")] CST012 = 224012,//tracked vehicle
            [Description("CST013")] CST013 = 224013,//rigid vehicle
            [Description("CST014")] CST014 = 224014,//spmt
            //[Description("CST015")] CST015 = 224015,//girder set
            [Description("CST016")] CST016 = 224016,//wheeled load
            //[Description("CST017")] CST017 = 224017,//recovery vehicle
            [Description("CST018")] CST018 = 224018,//recovered vehicle
            [Description("CST019")] CST019 = 224019,//mobile crane
            //[Description("CST020")] CST020 = 224020,//engineering plant
            [Description("CST021")] CST021 = 224021,//Eng Plant-Conventional Tractor
            [Description("CST022")] CST022 = 224022,//Eng Plant–Rigid
            [Description("CST023")] CST023 = 224023,//Eng Plant-Tracked
            [Description("CST024")] CST024 = 224024,//Eng Plant-Ballast Tractor
            [Description("CST025")] CST025 = 224025,//Engineering Plant–Semi Trailer
            [Description("CST026")] CST026 = 224026,//Engineering Plant–Drawbar Trailer
            [Description("CST027")] CST027 = 224027,//Recovery Vehicle
            [Description("CST028")] CST028 = 224028,//Girder Set
            [Description("CST029")] CST029 = 224029 //Clamp System
        }
        public enum ExternalApiCouplingType
        {
            [Description("None")] None = 201001,
            [Description("Fifth Wheel")] FifthWheel = 201002,
            [Description("Drawbar")] Drawbar = 201003,
            [Description("Tow Hitch")] TowHitch = 201004
        }
        public enum ExternalApiDimensionUnitSystem
        {
            [Description("imperial system")] Imperial = 208006,
            [Description("metric")] Metric = 208001
        }
        public enum ExternalApiWeightUnitSystem
        {
            [Description("imperial system")] Imperial = 240003,
            [Description("metric")] Metric = 240001
        }
        public enum ExternalApiSpeedUnitSystem
        {
            [Description("imperial system")] Imperial = 229001,
            [Description("metric")] Metric = 229002
        }
        public enum ExternalApiBitSystem
        {
            [Description("YES")] Yes = 1,
            [Description("NO")] No = 0
        }
        public enum ExternalApiVSOType
        {
            [Description("SOA")] soa = 267001,
            [Description("POL")] police = 267002,
            [Description("SOAANDPOL")] soapolice = 267003
        }
        public enum ExternalAPIStatus
        {
            [Description("Work in progress")] wip = 305001,
            [Description("Proposed")] proposed = 305002,
            [Description("Reproposed")] reproposed = 305003,
            [Description("Agreed")] agreed = 305004,
            [Description("Agreed revised")] agreed_revised = 305005,
            [Description("Agreed recleared")] agreed_recleared = 305006,
            [Description("Withdrawn")] withdrawn = 305007,
            [Description("Declined")] declined = 305008,
            [Description("Historical")] historical = 305009,
            [Description("Revised")] revised = 305010,
            [Description("Agreement work in progress")] agreement_wip = 305011,
            [Description("Agreement reclearance work in progress")] agreement_reclearance_wip = 305012,
            [Description("Submitted")] planned = 305013,
            [Description("Approved")] approved = 305014,

            [Description("Approved")] approvedapp = 308007,
            [Description("Work in progress")] workinprogress = 308001,
            [Description("Submitted")] submitted = 308002,
            [Description("Received by NH")] receivedbynh = 308003,
            [Description("Declined")] declinedapp = 308004,
            [Description("Withdrawn")] withdrawnapp = 308005,
            [Description("Historical")] historicalapp = 308006
        }
        public enum ExternalApiMessageTypes
        {
            [Description("Proposal")] proposal = 312001,
            [Description("Reproposal")] reproposal = 312002,
            [Description("Agreement")] agreement = 312005,
            [Description("Amendment to agreement")] amendment_to_agreement = 312006,
            [Description("No longer affected")] nolonger_affected = 312008,
            [Description("Notification")] notification = 312009,
            [Description("Renotification")] renotification = 312010,
            [Description("NE Notification")] ne_notification = 312014,
            [Description("NE Renotification")] ne_renotification = 312015,
            [Description("NE Notification")] ne_notif_api = 312016,
            [Description("NE ReNotification")] ne_renotif_api = 312017,
            //For nen notification via pdf
            [Description("NE Notification")] Unplanned = 911001,
            [Description("NE Notification")] Planned = 911002,
            [Description("NE Notification")] Planning_error = 911003,
            [Description("NE Notification")] Replanned = 911004,
            [Description("NE Notification")] Agreed = 911006,
            [Description("NE Notification")] Accepted = 911007,
            [Description("NE Notification")] Rejected = 911008,
            [Description("NE Notification")] Under_assessment = 911009,
            //AS means Assigned for scrutiny
            [Description("NE Notification")] AS_unplanned = 911005,
            [Description("NE Notification")] AS_planned = 911010,
            [Description("NE Notification")] AS_replanned = 911011,
            //Not Available
            [Description("withdrawl")] withdrawl = 312003,
            [Description("declination")] declination = 312004,
            [Description("recleared")] recleared = 312007,
            [Description("delivery failure")] delivery_failure = 312011,
            [Description("delegation failure alert")] delegation_failure_alert = 312012,
            [Description("vr1 planned route")] vr1_planned_route = 312013
        }
        public enum ExternalApiInboxStatus
        {
            [Description("withdrawl")] delegation_failure = 313001,
            [Description("Withdrawn")] withdrawn = 313002,
            [Description("Declined")] declined = 313003,
            [Description("Under assessment")] undecided = 313004,
            [Description("Accepted")] accepted = 313005,
            [Description("Rejected")] rejected = 313006,
            [Description("Unspecified")] unspecified = 313007,
            [Description("Under assessment")] underassessment = 313008,
            [Description("Unspecified")] unopened = 313009
        }
        public enum ExternalApiSuitability
        {
            [Description("Unknown")] unknown = 277001,
            [Description("Suitable")] suitable = 277002,
            [Description("Marginal")] marginal = 277003,
            [Description("Unsuitable")] unsuitable = 277004,
            [Description("Erroneous")] erroneous = 277005
        }
        public enum ExternalApiProjectStatus
        {
            [Description("Unallocated")] unallocated = 307001,
            [Description("Work In Progress")] workInProgress = 307002,
            [Description("Proposed")] proposed = 307003,
            [Description("Reproposed")] reproposed = 307004,
            [Description("Agreed")] agreed = 307005,
            [Description("Agreed Revised")] agreedRevised = 307006,
            [Description("Agreed Recleared")] agreedRecleared = 307007,
            [Description("Withdrawn")] withdrawn = 307008,
            [Description("Declined")] declined = 307009,
            [Description("Historical")] historical = 307010,
            [Description("Revised")] revised = 307011,
            [Description("Agreement Work In Progress")] agreementWorkInProgress = 307012,
            [Description("Agreement Reclearance Work In Progress")] agreementReclearanceWorkInProgress = 307013,
            [Description("Planned")] planned = 307014,
            [Description("Allocated")] allocated = 307015,
            [Description("Approved")] approved = 307016
        }
        public enum ExternalApiCheckStatus
        {
            [Description("Not Checking")] notChecking = 301001,
            [Description("Checking")] checking = 301002,
            [Description("Checked Positively")] checkedPositively = 301003,
            [Description("Checked Negatively")] checkedNegatively = 301004,
            [Description("Qa Checking")] qAChecking = 301008,
            [Description("Qa Checked Positively")] qACheckedPositively = 301009,
            [Description("Qa Checked Negatively")] qACheckedNegatively = 301010,
            [Description("Checking Final")] checkingFinal = 301005,
            [Description("Checked Final Positively")] checkedFinalPositively = 301006,
            [Description("Checked Final Negatively")] checkedFinalNegatively = 301007
        }
    }
}
