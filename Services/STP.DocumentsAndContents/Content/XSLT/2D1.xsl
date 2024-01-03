<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="2.0"
xmlns:o="urn:schemas-microsoft-com:office:office"
xmlns:w="urn:schemas-microsoft-com:office:word"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:ns1="http://www.esdal.com/schemas/core/specialorder"
xmlns:ns2="http://www.esdal.com/schemas/core/movement"
xmlns:ns3="http://www.esdal.com/schemas/core/vehicle"
xmlns:ns4="http://www.esdal.com/schemas/core/esdalcommontypes"
xmlns:ns5="http://www.esdal.com/schemas/core/route">

  <xsl:template match="/ns1:SpecialOrder">
    <html>
      <head>
        <xml>
          <w:WordDocument>
            <w:View></w:View>
            <w:Zoom></w:Zoom>
            <w:DoNotOptimizeForBrowser/>
          </w:WordDocument>
        </xml>
      </head>
      <body style="font-family:Arial">
        <b>
          <p style="font-color:red">
            2D1
          </p>
        </b>
        <p>
          ORDER No.
          <xsl:value-of select="ns2:OrderNumber"/>
        </p>
        <p> ROAD TRAFFIC ACT 1988 </p>
        <p> ORDER OF THE SECRETARY OF STATE UNDER SECTION 44 </p>
        <p>
          The Secretary of State in exercise of his powers under section 44 of the Road Traffic Act 1988 hereby authorises the use on roads of a trailer being a trailer which has

          <xsl:value-of select="ns1:VehiclesSchedule/ns1:Vehicle/ns1:CustomNumberOfAxles"/>

          or more axles which is so constructed as to be capable of carrying an abnormal indivisible load (within the meaning of article 3 of the Road Vehicles (Authorisation of Special Types) (General) Order 2003 (" the 2003 Order") being a

          <xsl:value-of select="ns1:Load"/>

          which by reason of its
          <xsl:choose>
            <xsl:when test="ns1:Width &gt;6.1 and ns1:RigidLength &gt;30.0">
              length/width
            </xsl:when>
            <xsl:when test="ns1:Width &lt;6.1 and ns1:RigidLength &gt;30.0">
              length
            </xsl:when>
            <xsl:when test="ns1:Width &gt;6.1 and ns1:RigidLength &lt;30.0">
              width
            </xsl:when>
            <xsl:otherwise>
            </xsl:otherwise>
          </xsl:choose>
          cannot be carried on roads under that Order, notwithstanding that such a trailer does not comply in all respects with the requirements of the Road Vehicles (Construction and Use) Regulations 1986 ("the 1986 Regulations") subject to conditions and restrictions as follows -

        </p>
        <ol>
          <li style="margin-bottom:5px">
            A trailer shall be used under this Order only -
            <table style="font-family:Arial">
              <tr>
                <td valign="top" align="right">(a)</td>
                <td valign="top" colspan="22" style="padding-left: 10px;">
                  by or on behalf of
                  <xsl:value-of select="ns1:HaulierName"/>
                  <table style="font-family:Arial">
                    <tr>
                      <td>
                        <xsl:for-each select="ns1:HaulierAddress/ns4:Line">
                          <xsl:if test=". != ''">
                            <xsl:value-of select="."/>,
                          </xsl:if>
                        </xsl:for-each>
                        <xsl:if test="ns1:HaulierAddress/ns4:PostCode != ''">
                          <xsl:value-of select="ns1:HaulierAddress/ns4:PostCode"/>, 
                        </xsl:if>
                        <xsl:if test="ns1:HaulierAddress/ns4:Country != ''">
                          <xsl:call-template name="Capitalize">
                            <xsl:with-param name="word" select="substring-before(concat(ns1:HaulierAddress/ns4:Country, ' '), ' ')"/>
                          </xsl:call-template>
                        </xsl:if>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
              <tr>
                <td valign="top" align="right">(b)</td>
                <td valign="top" colspan="22" style="padding-left: 10px;">
                  for the purpose of   <xsl:value-of select="ns1:NoOfJourneysInWord"/>
                  <xsl:choose>
                    <xsl:when test="ns1:NoOfJourneys &gt; 1">
                      journeys,
                    </xsl:when>
                    <xsl:otherwise>
                      journey,
                    </xsl:otherwise>
                  </xsl:choose>
                  which shall take place before the
                  <xsl:value-of select="ns1:CustomFormatExpiryDate"/>; and
                </td>
              </tr>
              <tr>
                <td valign="top" align="right">(c)</td>
                <td valign="top" colspan="22" style="padding-left: 10px;">
                  for the carriage of such a  <xsl:value-of select="ns1:Load"/>
                  from  <xsl:value-of select="ns1:StartLocation"/>
                  to  <xsl:value-of select="ns1:EndLocation"/> 
                  route as may previously have been authorised by the Secretary of State.
                </td>
              </tr>

            </table >
          </li>
          <li>
            The conditions specified in -
            <table style="font-family:Arial">
              <tr>
                <td valign="top"></td>
                <td colspan="24" style="padding-left: 10px;">

                  <table style="font-family:Arial">
                    <tr>
                      <td width="85%">
                        <xsl:choose>
                          <xsl:when test="ns1:Width &gt;6.1">
                            (a)<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;&nbsp;]]></xsl:text>Schedule 1 - paragraph 5-15, 17(2), 20, 26 - 37;
                          </xsl:when>
                          <xsl:when test="ns1:RigidLength &gt;30.0">
                            (a)<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;&nbsp;]]></xsl:text>Schedule 1 - paragraph 5-15, 17(2), 20, 24, 26 - 37;
                          </xsl:when>
                          <xsl:otherwise>
                            (a)<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;&nbsp;]]></xsl:text>Schedule 1 - paragraph 5-15, 17(2), 20, 24, 26 - 37;
                          </xsl:otherwise>
                        </xsl:choose>
                      </td>
                    </tr>
                    <tr>
                      <td width="85%">
                        (b)<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;&nbsp;]]></xsl:text>Schedule 6;
                      </td>
                    </tr>
                    <tr>
                      <td width="85%">
                        (c)<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;&nbsp;]]></xsl:text>Article 18(1) and (2); and
                      </td>
                    </tr>
                    <tr>
                      <td width="85%">
                        (d)<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;&nbsp;]]></xsl:text>Schedule 8 and article 16
                      </td>
                    </tr>
                    <tr>
                      <td >of the 2003 Order shall apply to the use of a  trailer authorised by this Order as they apply to the use of a trailer authorised by Part 2 and Part 4 articles 28 to 31 and Schedule 1 to that Order.</td>
                    </tr>
                  </table>
                </td>
              </tr >
            </table>
            <P style="padding-bottom:0px;"></P>
          </li>
          <xsl:if test="ns1:RigidLength &gt;30.0">
            <li style="margin-bottom:20px">
              The overall length of the trailer and its load shall not exceed
              <xsl:value-of select="ns1:RigidLength"/>
              metres.
              <P style="padding-bottom:0px;"></P>
            </li>
          </xsl:if>
          <xsl:if test="ns1:Width &gt;5.0">
            <li style="margin-bottom:20px">
              The overall width of the trailer and its load shall not exceed
              <xsl:value-of select="ns1:Width"/>
              metres.
              <P style="padding-bottom:0px;"></P>
            </li>
          </xsl:if>
          <li style="margin-bottom:5px">
            Unless specifically directed in writing by or on behalf of the chief officer of police of every police area through which the journey is to be made the trailer shall not be used under this Order during any of the following periods:-
            <P style="padding-bottom:10px;"></P>

            <xsl:for-each select="ns1:BankHolidayExclusion">
              <xsl:if test="ns1:End/ns1:Time!='' and ns1:Start/ns1:Day!='' and ns1:Start/ns1:Date!='' and ns1:End/ns1:Day!='' and ns1:End/ns1:Date!=''">
                <p style="padding-left: 18px;">
                  from 
                  <xsl:value-of select="ns1:End/ns1:Time"/>
                  <!--<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                  <xsl:call-template name="Capitalize">
                    <xsl:with-param name="word" select="substring-before(concat(ns1:Start/ns1:Day, ' '), ' ')"/>
                  </xsl:call-template>-->
                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                  <xsl:value-of select="ns1:Start/ns1:CustomStartDate"/>
                  to 
                  <xsl:value-of select="ns1:End/ns1:Time"/>
                  <!--<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                  <xsl:call-template name="Capitalize">
                    <xsl:with-param name="word" select="substring-before(concat(ns1:End/ns1:Day, ' '), ' ')"/>
                  </xsl:call-template>-->
                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                  <xsl:value-of select="ns1:End/ns1:CustomEndDate"/>
                </p >
              </xsl:if >
            </xsl:for-each>
          </li>
          <li style="margin-bottom:10px">
            Notwithstanding paragraph 37 of Schedule 1 to the 2003 Order, regulation 7 of the 1986 Regulations shall not apply in the case of:
            <table style="font-family:Arial">
              <tr>
                <td valign="top" align="right">(a)</td>
                <td valign="top" colspan="22"  style="padding-left: 10px;">
                  an articulated vehicle, or a motor vehicle and a trailer, where the semi trailer or the trailer is constructed such that the major part of the load platform does not extend over or between the wheels and is at a height that is below the height of the top most point of the tyres of those wheels, measured on level ground and with any adjustable suspension at the normal travelling height, and where the height or stability of the load being carried necessitates the use of such a trailer; or
                </td>
              </tr>
              <tr>
                <td valign="top" align="right">(b)</td>
                <td valign="top" colspan="22"  style="padding-left: 10px;">
                  a vehicle or combination of vehicles unable to comply with that regulation because of the requirements of paragraph 30 or paragraph 32 of Schedule 1 to the 2003 Order.
                </td>
              </tr>
            </table>
          </li>
          <li style="margin-bottom:15px">
            Before the trailer is used under the terms of this Order:
            <table style="font-family:Arial">
              <tr>
                <td valign="top" align="right">(a)</td>
                <td valign="top" colspan="22" style="padding-left: 10px;">
                  an indemnity in the form set out in Part 2 of Schedule 9 to the 2003 Order shall be given by or on behalf of
                  <xsl:value-of select="ns1:HaulierName"/>
                  to every road authority and every bridge authority responsible for the maintenance and repair of any road or bridge over which it is proposed that the trailer shall pass; and
                </td>
              </tr>
              <tr>
                <td valign="top" align="right">(b)</td>
                <td valign="top" colspan="22" style="padding-left: 10px;">
                  on each occasion before the trailer is used under the terms of this Order notice of the intended use shall be given to each such authority in accordance with Part 1 of Schedule 9 to the 2003 Order and to the chief officer of police of every police area through which the journey is to be made in accordance with Schedule 5 to the 2003 Order except that in every case five clear days notice shall be given.
                </td>
              </tr>
            </table>
            <P style="padding-bottom:0px;"></P>
          </li>
          <li style="margin-bottom:20px">
            Any directions in respect of time, date or route given by the chief officer of police of any police area through which the journey is to be made shall be complied with, provided that if any such direction involves making a deviation from the route for the journey previously authorised by the Secretary of State pursuant to article 1 of this Order the deviation shall not be made without the prior approval of the Secretary of State.
            <P style="padding-bottom:0px;"></P>
          </li>
          <li style="margin-bottom:20px">
            Before the trailer crosses any automatic half-barrier railway level crossing or any other railway level crossing, equipped with a telephone, the driver of the towing vehicle shall telephone the railway signaller of the intention to cross the railway with the trailer. The trailer and the vehicles used with it shall not cross except with the permission of and in accordance with the instructions of the railway signaller. After crossing the driver shall again telephone the signaller to inform him that the crossing is clear.
            <P style="padding-bottom:0px;"></P>
          </li>
          <li style="margin-bottom:20px">
            The overall height of the trailer inclusive of its load or of any motor vehicle used in combination with the  trailer travelling under any overhead cable or wire shall in no circumstances exceed the headroom specified on any traffic sign or notice relating to such cable or wire except with the prior approval of the owner of the cable or wire.
            <P style="padding-bottom:0px;"></P>
          </li>
          <li style="margin-bottom:5px">
            (1)  Subject to the provisions of paragraph (2) below, where the trailer (or any vehicles used in combination with it) is caused to stop for any reason while it is on a bridge, it shall, as soon as practical, be moved clear of the bridge by appropriate action, without applying any concentrated load to the surface of the part of the road carried by the bridge.
            <table style="font-family:Arial">
              <tr>
                <td valign="top">(2)</td>
                <td colspan="24">
                  If the action described in paragraph (1) above is not practicable and it becomes necessary to apply any concentrated load to the surface by means of jacks, rollers or other similar means, then the person in charge of the vehicle shall -
                </td>
              </tr>
              <tr>
                <td></td>
                <td colspan="24">
                  <table style="font-family:Arial">
                    <tr>
                      <td valign="top">(a)</td>
                      <td colspan="550" style="padding-left: 5px;">
                        before any such load is applied to that surface, seek the advice of the bridge authority for that bridge or any other person responsible for the maintenance and repair of the bridge pursuant to an agreement with that authority about the use of spreader plates to reduce the possibility of any damage caused by the application of any such load; and
                      </td>
                    </tr>
                    <tr>
                      <td valign="top">(b)</td>
                      <td colspan="550" style="padding-left: 5px;">
                        arrange that no such load shall be applied without using such spreader plates as the bridge authority or such other person may have advised.
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>
            <P style="padding-bottom:0px;"></P>

          </li>
          <li style="margin-bottom:5px">
            A copy of the Special Order must be carried in the cab of the drawing vehicle each time a vehicle combination is used under the terms of the Order.<br></br>
            <br/>
            <br/>
            Signed by authority of the Secretary of State on the
            <xsl:value-of select="ns1:SigningDetails/ns2:CustomSigningDate"/>
            <br/><br/><br/><br/>
            <xsl:call-template name="CamelCase">
              <xsl:with-param name="text">
                <xsl:value-of select="ns1:SigningDetails/ns2:Signatory"/>,
              </xsl:with-param>
            </xsl:call-template>
            <br/>
            <xsl:value-of select="ns1:SigningDetails/ns2:SignatoryRole"/>
            <br/>
            Ref:<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:value-of select="ns2:JobFileReferenceNumber"/>
          </li>
        </ol>
      </body>
    </html>
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
  <xsl:template name="FormatDate">
    <xsl:param name="DateTime" />
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

    <xsl:value-of select="$day"/>
    <xsl:value-of select="' '"/>
    <xsl:value-of select="$month"/>
    <xsl:value-of select="' '"/>
    <xsl:value-of select="$year"/>

  </xsl:template>

  <xsl:template name="CamelCase">
    <xsl:param name="text"/>
    <xsl:choose>
      <xsl:when test="contains($text,' ')">
        <xsl:call-template name="CamelCaseWord">
          <xsl:with-param name="text" select="substring-before($text,' ')"/>
        </xsl:call-template>
        <xsl:text> </xsl:text>
        <xsl:call-template name="CamelCase">
          <xsl:with-param name="text" select="substring-after($text,' ')"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:call-template name="CamelCaseWord">
          <xsl:with-param name="text" select="$text"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="CamelCaseWord">
    <xsl:param name="text"/>
    <xsl:value-of select="translate(substring($text,1,1),'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
    <xsl:value-of select="translate(substring($text,2,string-length($text)-1),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')" />
  </xsl:template>
  
</xsl:stylesheet>
