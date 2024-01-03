using System.ComponentModel;
namespace STP.Common.Enums
{
    public class Organisation
    {
        public enum OrganisationType
        {
            [Description("county council")]
            CC = 237001,
            [Description("borough and district council")]
            BDC = 237002,
            [Description("city council")]
            City = 237003,
            [Description("london borough")]
            LB = 237004,
            [Description("metropolitan council")]
            MC = 237005,
            [Description("unitary authority")]
            UA = 237005,
            [Description("police")]
            Police = 237005,
            [Description("management agent")]
            MA = 237005,
            [Description("rail operator")]
            RO = 237005,
            [Description("water and port authority")]
            WPA = 237005,
            [Description("private owner")]
            PO = 237005,
            [Description("bridge and tunnel operator")]
            BTO = 237005,
            [Description("haulier")]
            Haulier = 237005,
            [Description("haulier association")]
            HA = 237005,
            [Description("dbfo")]
            DBFO = 237005,
            [Description("system provider")]
            SP = 237005,
            [Description("ha mac")]
            HAM = 237005,
            [Description("transport for london")]
            TFL = 237005,
            [Description("utility company")]
            UC = 237005,
        }
    }
}
