using System.Collections.Generic;
using System.Linq;

namespace STP.Common.General
{
    public static class CountryDetails
    {
        public static int GetCountryId(string countryISOCode)
        {
            int countryId = 0;
            var countryCodesMapping = CountryCodemapping();

            countryId = (from x in countryCodesMapping
                         where x.Key == countryISOCode
                         select x.Value).FirstOrDefault();
            return countryId;
        }

        public static string GetCountryCode(int countryId)
        {
            string countryCode = string.Empty;
            var countryCodesMapping = CountryCodemapping();

            countryCode = (from x in countryCodesMapping
                           where x.Value == countryId
                           select x.Key).FirstOrDefault();
            return countryCode;
        }

        public static Dictionary<string, int> CountryCodemapping()
        {
            var countryCodesMapping = new Dictionary<string, int>() {
                { "ABW", 223021 }, //Aruba
                { "AFG", 223010 }, //Afghanistan
                { "AGO", 223015 }, //Angola
                { "AIA", 223016 }, //Anguilla
                { "ALB", 223011 }, //Albania
                { "AND", 223014 }, //Andorra
                { "ARE", 223225 }, //United Arab Emirates
                { "ARG", 223019 }, //Argentina
                { "ARM", 223020 }, //Armenia
                { "ASM", 223013 }, //American Samoa
                { "ATA", 223017 }, //Antarctica
                { "ATF", 223081 }, //French Southern Territories
                { "ATG", 223018 }, //Antigua And Barbuda
                { "AUS", 223022 }, //Australia
                { "AUT", 223023 }, //Austria
                { "AZE", 223024 }, //Azerbaijan
                { "BDI", 223043 }, //Burundi
                { "BEL", 223030 }, //Belgium
                { "BEN", 223032 }, //Benin
                { "BFA", 223042 }, //Burkina Faso
                { "BGD", 223027 }, //Bangladesh
                { "BGR", 223041 }, //Bulgaria
                { "BHR", 223026 }, //Bahrain
                { "BHS", 223025 }, //Bahamas
                { "BIH", 223036 }, //Bosnia And Herzegovina
                { "BLR", 223029 }, //Belarus
                { "BLZ", 223031 }, //Belize
                { "BMU", 223033 }, //Bermuda
                { "BOL", 223035 }, //Bolivia
                { "BRA", 223038 }, //Brazil
                { "BRB", 223028 }, //Barbados
                { "BRN", 223040 }, //Brunei
                { "BTN", 223034 }, //Bhutan
                { "BWA", 223037 }, //Botswana
                { "CAF", 223048 }, //Central African Republic
                { "CAN", 223006 }, //Canada
                { "CHE", 223208 }, //Switzerland
                { "CHL", 223050 }, //Chile
                { "CHN", 223051 }, //China
                { "CIV", 223108 }, //Ivory Coast
                { "CMR", 223045 }, //Cameroon
                { "COG", 223054 }, //Congo
                { "COK", 223055 }, //Cook Islands
                { "COL", 223052 }, //Colombia
                { "COM", 223053 }, //Comoros
                { "CPV", 223046 }, //Cape Verde
                { "CRI", 223056 }, //Costa Rica
                { "CUB", 223058 }, //Cuba
                { "CYM", 223047 }, //Cayman Islands
                { "CYP", 223059 }, //Cyprus
                { "CZE", 223060 }, //Czech Republic
                { "DEU", 223085 }, //Germany
                { "DJI", 223062 }, //Djibouti
                { "DMA", 223063 }, //Dominica
                { "DNK", 223061 }, //Denmark
                { "DOM", 223064 }, //Dominican Republic
                { "DZA", 223012 }, //Algeria
                { "ECU", 223066 }, //Ecuador
                { "EGY", 223067 }, //Egypt
                { "ERI", 223070 }, //Eritrea
                { "ESH", 223235 }, //Western Sahara
                { "ESP", 223201 }, //Spain
                { "EST", 223071 }, //Estonia
                { "ETH", 223072 }, //Ethiopia
                { "FIN", 223077 }, //Finland
                { "FJI", 223076 }, //Fiji Islands
                { "FLK", 223074 }, //Falkland Islands
                { "FRA", 223078 }, //France
                { "FRO", 223075 }, //Faroe Islands
                { "FSM", 223141 }, //Micronesia
                { "GAB", 223082 }, //Gabon
                { "GBR", 223251 }, //United Kingdom
                { "GEO", 223084 }, //Georgia
                { "GGY", 223094 }, //Guernsey And Alderney
                { "GHA", 223086 }, //Ghana
                { "GIB", 223087 }, //Gibraltar
                { "GIN", 223095 }, //Guinea
                { "GLP", 223091 }, //Guadeloupe
                { "GMB", 223083 }, //Gambia
                { "GNB", 223096 }, //Guinea-Bissau
                { "GNQ", 223069 }, //Equatorial Guinea
                { "GRC", 223088 }, //Greece
                { "GRD", 223090 }, //Grenada
                { "GRL", 223089 }, //Greenland
                { "GTM", 223093 }, //Guatemala
                { "GUF", 223079 }, //French Guiana
                { "GUM", 223092 }, //Guam
                { "GUY", 223097 }, //Guyana
                { "HKG", 223247 }, //Hong Kong S.A.R
                { "HND", 223099 }, //Honduras
                { "HRV", 223057 }, //Croatia
                { "HTI", 223098 }, //Haiti
                { "HUN", 223100 }, //Hungary
                { "IDN", 223103 }, //Indonesia
                { "IND", 223102 }, //India
                { "IRL", 223008 }, //Ireland
                { "IRN", 223104 }, //Iran
                { "IRQ", 223105 }, //Iraq
                { "ISL", 223101 }, //Iceland
                { "ISR", 223106 }, //Israel
                { "ITA", 223107 }, //Italy
                { "JAM", 223109 }, //Jamaica
                { "JEY", 223111 }, //Jersey
                { "JOR", 223112 }, //Jordan
                { "JPN", 223110 }, //Japan
                { "KAZ", 223113 }, //Kazakhstan
                { "KEN", 223114 }, //Kenya
                { "KGZ", 223119 }, //Kyrgyzstan
                { "KHM", 223044 }, //Cambodia
                { "KIR", 223115 }, //Kiribati
                { "KNA", 223181 }, //Saint Kitts And Nevis
                { "KOR", 223117 }, //South Korea
                { "KWT", 223118 }, //Kuwait
                { "LAO", 223120 }, //Laos
                { "LBN", 223122 }, //Lebanon
                { "LBR", 223124 }, //Liberia
                { "LBY", 223125 }, //Libya
                { "LCA", 223182 }, //Saint Lucia
                { "LIE", 223126 }, //Liechtenstein
                { "LKA", 223202 }, //Sri Lanka
                { "LSO", 223123 }, //Lesotho
                { "LTU", 223127 }, //Lithuania
                { "LUX", 223128 }, //Luxembourg
                { "LVA", 223121 }, //Latvia
                { "MAC", 223248 }, //Macao S.A.R.
                { "MAR", 223146 }, //Morocco
                { "MCO", 223143 }, //Monaco
                { "MDA", 223142 }, //Moldova
                { "MDG", 223130 }, //Madagascar
                { "MDV", 223133 }, //Maldives
                { "MEX", 223007 }, //Mexico
                { "MHL", 223136 }, //Marshall Islands
                { "MKD", 223129 }, //Macedonia
                { "MLI", 223134 }, //Mali
                { "MLT", 223135 }, //Malta
                { "MMR", 223148 }, //Myanmar
                { "MNE", 223249 }, //Montenegro
                { "MNG", 223144 }, //Mongolia
                { "MNP", 223161 }, //Northern Mariana Islands
                { "MOZ", 223147 }, //Mozambique
                { "MRT", 223138 }, //Mauritania
                { "MSR", 223145 }, //Montserrat
                { "MTQ", 223137 }, //Martinique
                { "MUS", 223139 }, //Mauritius
                { "MWI", 223131 }, //Malawi
                { "MYS", 223132 }, //Malaysia
                { "MYT", 223140 }, //Mayotte
                { "NAM", 223149 }, //Namibia
                { "NCL", 223154 }, //New Caledonia
                { "NER", 223157 }, //Niger
                { "NFK", 223160 }, //Norfolk Island
                { "NGA", 223158 }, //Nigeria
                { "NIC", 223156 }, //Nicaragua
                { "NIR", 223004 }, //Northern Ireland
                { "NIU", 223159 }, //Niue
                { "NLD", 223152 }, //Netherlands
                { "NOR", 223162 }, //Norway
                { "NPL", 223151 }, //Nepal
                { "NRU", 223150 }, //Nauru
                { "NZL", 223155 }, //New Zealand
                { "OMN", 223163 }, //Oman
                { "PAK", 223164 }, //Pakistan
                { "PAN", 223167 }, //Panama
                { "PER", 223170 }, //Peru
                { "PHL", 223171 }, //Philippines
                { "PLW", 223165 }, //Palau
                { "PNG", 223168 }, //Papua New Guinea
                { "POL", 223172 }, //Poland
                { "PRI", 223174 }, //Puerto Rico
                { "PRK", 223116 }, //North Korea
                { "PRT", 223173 }, //Portugal
                { "PRY", 223169 }, //Paraguay
                { "PSE", 223166 }, //Palestine
                { "PYF", 223080 }, //French Polynesia
                { "QAT", 223175 }, //Qatar
                { "REU", 223176 }, //Reunion
                { "ROU", 223177 }, //Romania
                { "RUS", 223178 }, //Russia
                { "RWA", 223179 }, //Rwanda
                { "SAU", 223187 }, //Saudi Arabia
                { "SCG", 223189 }, //Serbia And Montenegro
                { "SDN", 223203 }, //Sudan
                { "SEN", 223188 }, //Senegal
                { "SGP", 223192 }, //Singapore
                { "SHN", 223180 }, //Saint Helena
                { "SJM", 223205 }, //Svalbard And Jan Mayen
                { "SLB", 223198 }, //Solomon Islands
                { "SLE", 223191 }, //Sierra Leone
                { "SLV", 223068 }, //El Salvador
                { "SMR", 223186 }, //San Marino
                { "SOM", 223199 }, //Somalia
                { "SPM", 223183 }, //Saint Pierre And Miquelon
                { "SRB", 223250 }, //Serbia
                { "STP", 223210 }, //São Tomé And Príncipe
                { "SUR", 223204 }, //Suriname
                { "SVK", 223193 }, //Slovakia
                { "SVN", 223194 }, //Slovenia
                { "SWE", 223207 }, //Sweden
                { "SWZ", 223206 }, //Swaziland
                { "SYC", 223190 }, //Seychelles
                { "SYR", 223209 }, //Syria
                { "TAJ", 223212 }, //Tajikistan
                { "TCA", 223221 }, //Turks And Caicos Islands
                { "TCD", 223049 }, //Chad
                { "TGO", 223215 }, //Togo
                { "THA", 223214 }, //Thailand
                { "TKL", 223216 }, //Tokelau
                { "TKM", 223220 }, //Turkmenistan
                { "TLS", 223065 }, //East Timor
                { "TON", 223217 }, //Tonga
                { "TTO", 223009 }, //Trinidad And Tobago
                { "TUN", 223218 }, //Tunisia
                { "TUR", 223219 }, //Turkey
                { "TUV", 223222 }, //Tuvalu
                { "TWN", 223211 }, //Taiwan
                { "TZA", 223213 }, //Tanzania
                { "UGA", 223223 }, //Uganda
                { "UKR", 223224 }, //Ukraine
                { "URY", 223227 }, //Uruguay
                { "USA", 223005 }, //Usa
                { "UZB", 223228 }, //Uzbekistan
                { "VAT", 223230 }, //Vatican City
                { "VCT", 223184 }, //Saint Vincent And The Grenadines
                { "VEN", 223231 }, //Venezuela
                { "VGB", 223039 }, //British Virgin Islands
                { "VIR", 223233 }, //Virgin Islands Of The United States
                { "VNM", 223232 }, //Vietnam
                { "VUT", 223229 }, //Vanuatu
                { "WLF", 223234 }, //Wallis And Futuna
                { "WSM", 223185 }, //Samoa
                { "YEM", 223236 }, //Yemen
                { "ZAF", 223200 }, //South Africa
                { "ZMB", 223237 }, //Zambia
                { "ZWE", 223238 } //Zimbabwe
             };
            return countryCodesMapping;
        }
    }
}
