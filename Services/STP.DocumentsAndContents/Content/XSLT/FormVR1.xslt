<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
 xmlns:o="urn:schemas-microsoft-com:office:office"
 xmlns:w="urn:schemas-microsoft-com:office:word"
 xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
 xmlns:ns1="http://www.esdal.com/schemas/core/vr1"
 xmlns:ns2="http://www.esdal.com/schemas/core/movement"
 xmlns:ns3="http://www.esdal.com/schemas/core/esdalcommontypes"
 xmlns:ns4="http://www.esdal.com/schemas/core/contact"                
 exclude-result-prefixes="msxsl">

  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="/ns1:VR1">
    <html>
      <head>
        <xml>
          <w:WordDocument>
            <w:View></w:View>
            <w:Zoom></w:Zoom>
            <w:DoNotOptimizeForBrowser/>
          </w:WordDocument>
        </xml>
      </head >
      <body style="font-family:Verdana">
        <table width="100%">
          <tr>
            <td align="left">
              <b>National Highways</b>
            </td>
            <td style="text-align:right">
              <b>
                <xsl:value-of select="ns2:VR1Number" />
              </b>
            </td>
          </tr>
          <tr>
            <td colspan="2">
              <h2>Form VR1</h2>
            </td>
          </tr>
          <tr>
            <td colspan="2">
              Application for Authority to move a vehicle which, with load exceeds 5.0 metres but does not exceed 6.1 meters in width.
            </td>
          </tr>
        </table>
        <table width="100%" border="1" style="font-size:13px">
          <tr>
            <td width="50%">
              <table border="0" valign="top" width="100%" style="font-size:13px">
                <tr>
                  <td  valign="top"> 1. </td>
                  <td  valign="top">
                    Applicant's name  and address <br/><br/>
                    <xsl:value-of select="ns1:Haulier/ns2:HaulierName" /><br/>
                    <xsl:for-each select="ns1:Haulier/ns2:HaulierAddress/ns3:Line">
                      <xsl:if test=".!= ''">
                        <xsl:value-of select="."/>
                        <br/>
                      </xsl:if>
                    </xsl:for-each>
                    <xsl:value-of select="ns1:Haulier/ns2:HaulierAddress/ns3:PostCode" />
                    <br/>
                    <xsl:call-template name="Capitalize">
                      <xsl:with-param name="word" select="substring-before(concat(ns1:Haulier/ns2:HaulierAddress/ns3:Country, ' '), ' ')"/>
                    </xsl:call-template>
                  </td>
                </tr>
              </table>

            </td>
            <td width="50%" valign="top">
              <table border="0" valign="top" width="100%" style="font-size:13px">
                <tr>
                  <td  valign="top"> 2. </td>
                  <td  valign="top">
                    Name and address of Haulier (if different from 1.) <br/><br/>
                  <xsl:value-of select="ns1:ContactStructure/ns4:OrganisationName" /><br/>
                    <xsl:for-each select="ns1:ContactStructure/ns4:Address/ns3:Line">
                      <xsl:if test=".!= ''">
                        <xsl:value-of select="."/>
                        <br/>
                      </xsl:if>
                    </xsl:for-each>
                    <xsl:value-of select="ns1:ContactStructure/ns4:Address/ns3:PostCode" /><br/>
                    <xsl:value-of select="ns1:ContactStructure/ns4:TelephoneNumber" />
                    <br/>
                    <xsl:value-of select="ns1:ContactStructure/ns4:EmailAddress" />
                    <br/>
                    <xsl:if test="ns1:ContactStructure/ns4:FullName != ''">
                      <xsl:call-template name="Capitalize">
                      <xsl:with-param name="word" select="substring-before(concat(ns1:ContactStructure/ns4:Address/ns3:Country, ' '), ' ')"/>
                    </xsl:call-template>
                    </xsl:if>
                  </td>
                </tr>
              </table>
            </td>
          </tr>
          <tr>
            <td width="50%" valign="top">

              3. Approximate date(s) of movement.
            </td>
            <td width="50%" valign="top">
              <xsl:element name="newdate">
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="ns1:JourneyTiming/ns2:FirstMoveDate"/>
                </xsl:call-template>
              </xsl:element>
              to
              <xsl:element name="newdate">
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="ns1:JourneyTiming/ns2:LastMoveDate"/>
                </xsl:call-template>
              </xsl:element>

            </td>
          </tr>
          <tr>
            <td width="50%" valign="top">

              4. Address from which journey will start.
            </td>
            <td width="50%" valign="top">
              <xsl:value-of select="ns1:JourneyFrom" />
            </td>
          </tr>
          <tr>
            <td width="50%" valign="top">

              5. Address at which journey will end.
            </td >
            <td width="50%" valign="top">
              <xsl:value-of select="ns1:JourneyTo" />
            </td>
          </tr>
          <tr>
            <td width="50%" valign="top">

              6. Description of vehicle.
            </td>
            <td width="50%"  valign="top">
                <xsl:value-of select="ns1:VehicleDescription" />
            </td>
          </tr>
          <tr>
            <td width="50%" valign="top">
              <table border="0" style="font-size:13px">
                <tr>
                  <td  valign="top">7.</td>
                  <td  valign="top">
                    Overall width of vehicle with load. <br/>
                    Overall length of vehicle with load. <br/>
                    Overall height of vehicle with load. <br/>
                    Overall weight of vehicle with load.
                  </td>
                </tr>
              </table>

            </td>
            <td width="50%"  valign="top">
              <!--<xsl:value-of select="ns1:Width" /> metres
              <br/>
              <xsl:value-of select="ns1:Length" /> metres
              <br/>
              <xsl:value-of select="ns1:Height" /> metres
              <br/>
              <xsl:value-of select="ns1:GrossWeight" /> kg-->
            <xsl:value-of select="ns1:Width" />
              <br/>
              <xsl:value-of select="ns1:Length" />
              <br/>
              <xsl:value-of select="ns1:Height" />
              <br/>
              <xsl:value-of select="ns1:GrossWeight" />
            </td>
          </tr>
          <tr>
            <td width="50%">
              <table border="0" style="font-size:13px">
                <tr>
                  <td  valign="top"> 8.</td>
                  <td  valign="top">Nature and Description of load with net dimensions and weight.</td>
                </tr>
              </table>

            </td>
            <td width="50%"  valign="top">
              <xsl:value-of select="ns1:LoadDescription" />
            </td>
          </tr>
          <tr>
            <td width="50%">
              <table border="0" style="font-size:13px">
                <tr>
                  <td  valign="top">9.</td>
                  <td  valign="top">Maximum number of pieces carried at one time  and the number of loads this represent</td>
                </tr>
              </table>

            </td >
            <td width="50%">
              <xsl:if test="ns1:MaxPiecesPerMove = 1">
              There is a maximum of
              <xsl:value-of select="ns1:MaxPiecesPerMove" /> piece per move and a total of
              </xsl:if>
              
              <xsl:if test="ns1:MaxPiecesPerMove &gt; 1">
              There are a maximum of
              <xsl:value-of select="ns1:MaxPiecesPerMove" /> pieces per move and a total of
              </xsl:if>

              <xsl:if test="ns1:TotalMoves = 1">
                <xsl:value-of select="ns1:TotalMoves" /> load.
              </xsl:if>

              <xsl:if test="ns1:TotalMoves &gt; 1">
                <xsl:value-of select="ns1:TotalMoves" /> loads.
              </xsl:if>
              
            </td >
          </tr>
          <tr>
            <td width="50%" colspan="2">
              <table border="0" width="80%" style="font-size:13px">
                <tr>
                  <td>Name</td>
                  <td>
                    <xsl:value-of select="ns1:Haulier/ns2:HaulierName" />
                  </td>
                  <td>Date</td>
                  <td>
                    <xsl:element name="newdate">
                      <xsl:call-template name="FormatDate">
                        <xsl:with-param name="DateTime" select="ns1:ApplicationDate"/>
                      </xsl:call-template>
                    </xsl:element>
                  </td>
                </tr>
                <tr>
                  <td>Phone</td>
                  <td>
                    <xsl:value-of select="ns1:Haulier/ns2:TelephoneNumber" />
                  </td>
                    <td>
                    Fax
                  </td>
                  <td>
                    <xsl:value-of select="ns1:Haulier/ns2:FaxNumber" />
                  </td>
                </tr>
              </table>
            </td>
          </tr>
        </table>
        <table width="100%" border="0">
          <tr>
            <td colspan="2" >Part2- Authority for movement</td>
          </tr>
        </table>
        <table width="100%" border="1">
          <tr>
            <td>
              <table border="0" style="font-size:13px">
                <tr>
                  <td style="border: 0px" colspan="2">
                    The Secretary of State hereby authorises under the  provisions of the Road Vehicles (Authorisation of Special Types)
                    (General) Order  2003 the movement of vehicle(s) and load(s) detailed above.
                    <br />
                    This Authority should not  be taken in any way as relieving the haulier of any obligations  under the Road Vehicles
                    (Construction and Use) Regulations, the Road Vehicles (Authorisation of Special Types) (General) Order 2003, or otherwise.

                  </td>
                </tr>
                <tr>
                  <td colspan="2"></td>
                </tr>
                <tr>
                  <td width="50%" style="border: 0px; height: 76px">
                    <table style="font-size:13px">
                      <tr>
                        <td style="font-style: italic">
                          Attention is drawn to the safety precautions
                          <br />
                          mentioned on VR1 supplement No. 2 attached.
                        </td>
                      </tr>
                      <tr style="height: 10px">
                        <td>
                         </td>
                      </tr>
                      <tr>
                        <td>
                                 <xsl:value-of select="ns2:ESDALReferenceNumber/ns2:Mnemonic"/>/<xsl:value-of select="ns2:ESDALReferenceNumber/ns2:MovementProjectNumber"/>/<xsl:value-of select="ns2:ESDALReferenceNumber/ns2:MovementVersion"/>
                
                        </td>
                      </tr>
                      <tr style="height: 10px">
                        <td></td>
                      </tr>
                      <tr>
                        <td>
                          This actual Authority must be carried by the Driver of vehicle on any journey authorised by it.
                          Photocopies or Faxes of this form are not acceptable.
                        </td>
                      </tr>
                    </table>
                  </td>
                  <td>
                    <table width="100%" style="font-size:13px">
                      <tr>
                        <td>
                          <hr />
                          Signed by Authority of the Secretary of State<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;]]></xsl:text>  Date
                        </td>
                      </tr>
                      <tr style="height: 10px">
                        <td></td>
                      </tr>
                      <tr>
                        <td style="border-style: solid; border-width: 1px; border-color: inherit; height: 70px;"></td>
                      </tr>
                    </table>
                  </td>
                </tr>

                <tr>
                  <td style="border: 0px" />
                  <td style="border: 0px; text-align: left">Official Embossed Stamp</td>
                </tr>
              </table>
            </td>
          </tr>
        </table>
      </body>
    </html>
  </xsl:template>

  <xsl:template name="FormatDate">
    <xsl:param name="DateTime" />
    <xsl:param name="DateTime1" />
    <xsl:variable name="day">
      <xsl:value-of select=" substring($DateTime, 9, 2)" />
    </xsl:variable>

    <xsl:variable name="month">

      <xsl:choose>
        <xsl:when test="substring($DateTime, 6, 2) = '01'">January</xsl:when>
        <xsl:when test="substring($DateTime, 6, 2) = '02'">February</xsl:when>
        <xsl:when test="substring($DateTime, 6, 2) = '03'">March</xsl:when>
        <xsl:when test="substring($DateTime, 6, 2) = '04'">April</xsl:when>
        <xsl:when test="substring($DateTime, 6, 2) = '05'">May</xsl:when>
        <xsl:when test="substring($DateTime, 6, 2) = '06'">June</xsl:when>
        <xsl:when test="substring($DateTime, 6, 2) = '07'">July</xsl:when>
        <xsl:when test="substring($DateTime, 6, 2) = '08'">August</xsl:when>
        <xsl:when test="substring($DateTime, 6, 2) = '09'">September</xsl:when>
        <xsl:when test="substring($DateTime, 6, 2) = '10'">October</xsl:when>
        <xsl:when test="substring($DateTime, 6, 2) = '11'">November</xsl:when>
        <xsl:when test="substring($DateTime, 6, 2) = '12'">December</xsl:when>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="year">
      <xsl:value-of select="substring($DateTime, 1, 4)" />
    </xsl:variable>

    <xsl:variable name="hh">
      <xsl:value-of select="substring($DateTime1, 12, 2)" />
    </xsl:variable>

    <xsl:variable name="mm">
      <xsl:value-of select="substring($DateTime1, 15, 2)" />
    </xsl:variable>

    <xsl:variable name="ss">
      <xsl:value-of select="substring($DateTime1, 18, 2)" />
    </xsl:variable>

    <xsl:value-of select="$day"/>
    <xsl:value-of select="' '"/>
    <xsl:value-of select="$month"/>
    <xsl:value-of select="' '"/>
    <xsl:value-of select="$year"/>
    <xsl:value-of select="' '"/>

    <xsl:choose>
      <xsl:when test="not($DateTime1)">
        <!-- parameter has not been supplied -->
      </xsl:when>
      <xsl:otherwise>
        <!--parameter has been supplied -->
        <xsl:value-of select="concat($hh,':',$mm,':',$ss)"/>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>
  <xsl:template name="Capitalize">
    <xsl:param name="word" select="''"/>
    <xsl:value-of select="concat(
        translate(substring($word, 1, 1),
            'abcdefghijklmnopqrstuvwxyz',
            'ABCDEFGHIJKLMNOPQRSTUVWXYZ'),
        translate(substring($word, 2),
            'ABCDEFGHIJKLMNOPQRSTUVWXYZ',
            'abcdefghijklmnopqrstuvwxyz'))"/>
  </xsl:template>
</xsl:stylesheet>
