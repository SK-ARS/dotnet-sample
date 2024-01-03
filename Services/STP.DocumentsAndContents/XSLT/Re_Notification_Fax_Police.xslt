<?xml version="1.0" encoding="UTF-8" ?>

<xsl:stylesheet version="2.0"
    xmlns:o="urn:schemas-microsoft-com:office:office"
    xmlns:w="urn:schemas-microsoft-com:office:word"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:xs="http://www.w3.org/2001/XMLSchema"
    xmlns:fn="http://www.w3.org/2005/xpath-functions "
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:fo="http://www.w3.org/1999/XSL/Format"
    xmlns:fox="http://xml.apache.org/fop/extensions"
    xmlns:ns1="http://www.esdal.com/schemas/core/notification"
    xmlns:ns2="http://www.esdal.com/schemas/core/movement"
    xmlns:ns3="http://www.esdal.com/schemas/core/vehicle"
    xmlns:ns4="http://www.esdal.com/schemas/core/esdalcommontypes"
    xmlns:ns5="http://www.esdal.com/schemas/core/route">

  <xsl:param name="Contact_ID"></xsl:param>
  <xsl:param name="Organisation_ID"></xsl:param>
  <xsl:param name="DocType"></xsl:param>

  <xsl:template match="/ns1:OutboundNotification">
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

        <table width = "100%" cellspacing ="0" cellpadding ="0">
          <tr>
            <td colspan="4">
              <xsl:choose>
                <xsl:when test="$DocType = 'EMAIL'">
                  <img align="left" width="540" height="80" src="https://esdal.dft.gov.uk/Content/Images/ESDAL 2 Logo_org.png"/>
                </xsl:when>
                <xsl:otherwise>
                  <img align="left" width="120" id="hdr_imgs"  height="80" src="~Content/Images/logo.png" />
                </xsl:otherwise>
              </xsl:choose>
            </td>
          </tr>
          <tr>
            <td>
              <br></br>
            </td>
          </tr>
        </table>

        <table width = "100%" cellspacing ="0" cellpadding ="0">
          <tr>
            <td colspan="16">
              <B>Note</B>
            </td>
          </tr>
          <tr>
            <td colspan="16">
              Items that have been removed are shown with a line through them e.g. <strike>This line has been removed </strike>
              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>Items that have been added are shown in <b>bold</b> and <b>
                <u>underlined</u>
              </b> e.g. <b>
                <u>This line has been added</u>
              </b>
            </td>
          </tr>
        </table>
        <br />

        <table width = "100%" cellspacing ="0" cellpadding ="0">

          <tr>
            <td>
              <b>Re-notification of movement:</b>
            </td>
            <td colspan="3">
              <br/>

              <xsl:if test="contains(ns2:ESDALReferenceNumber/ns2:ESDALReferenceNo, '##**##')">
                <strike>
                  <xsl:value-of select="substring-before(ns2:ESDALReferenceNumber/ns2:ESDALReferenceNo, '##**##')"/>
                </strike>
                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                <b>
                  <u>
                    <xsl:value-of select="substring-after(ns2:ESDALReferenceNumber/ns2:ESDALReferenceNo, '##**##')"/>
                  </u>
                </b>
              </xsl:if>

              <xsl:if test="contains(ns2:ESDALReferenceNumber/ns2:ESDALReferenceNo, '##**##')=false()">
                <xsl:value-of select="ns2:ESDALReferenceNumber/ns2:ESDALReferenceNo"/>
              </xsl:if>

              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:call-template name="parseString">
                <xsl:with-param name="list" select="ns1:JourneyFromTo/ns2:From"/>
              </xsl:call-template> to
              <xsl:call-template name="parseString">
                <xsl:with-param name="list" select="ns1:JourneyFromTo/ns2:To"/>
              </xsl:call-template>
            </td>
          </tr>
          <tr>
            <td>
              <b>Date sent:</b>
            </td>
            <td colspan="3">
              <xsl:if test="contains(ns1:SentDateTime, '##**##')">
                <xsl:element name="newdate">
                  <xsl:call-template name="FormatDate">
                    <xsl:with-param name="DateTime" select="substring-after(ns1:SentDateTime, '##**##')"/>
                    <xsl:with-param name="DateTime1" select="substring-after(ns1:SentDateTime, '##**##')"/>
                  </xsl:call-template>
                </xsl:element>
              </xsl:if>

              <xsl:if test="contains(ns1:SentDateTime, '##**##')=false()">
                <xsl:value-of select="ns1:SentDateTime"/>
              </xsl:if>
				

            </td>
          </tr>

          <tr>
            <td>
              <b>Classification:</b>
            </td>
            <td colspan="3">

              <xsl:choose>
                <xsl:when test="contains(ns1:Classification, '##**##')">
                  <xsl:value-of select="translate(substring-after(ns1:Classification, '##**##'), 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="translate(ns1:Classification, 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
                </xsl:otherwise>
              </xsl:choose>
            </td>
          </tr>

          <!--Start code for RM#3846-->
          <xsl:if test="ns1:VR1Information/ns1:Numbers/ns2:Scottish !=''">
            <tr>
              <td>
                <b>VR1 number:</b>
              </td>
              <td colspan="3">
                <xsl:if test="contains(ns1:VR1Information/ns1:Numbers/ns2:Scottish, '##**##')">
                  <strike>
                    <xsl:value-of select="substring-before(ns1:VR1Information/ns1:Numbers/ns2:Scottish, '##**##')"/>
                  </strike>
                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                  <b>
                    <u>
                      <xsl:value-of select="substring-after(ns1:VR1Information/ns1:Numbers/ns2:Scottish, '##**##')"/>
                    </u>
                  </b>
                </xsl:if>

                <xsl:if test="contains(ns1:VR1Information/ns1:Numbers/ns2:Scottish, '##**##')=false()">
                  <xsl:value-of select="ns1:VR1Information/ns1:Numbers/ns2:Scottish"/>
                </xsl:if>
              </td>
            </tr>
          </xsl:if>
          <!--End code for RM#3846-->
          
          <xsl:if test="ns1:Classification = 'special order' or ns1:Classification = 'Special order'">
            <tr>
              <td>
                <b>Order no :</b>
              </td>
              <td colspan="3">
                <xsl:choose>
                  <xsl:when test="ns1:SOInformation/ns1:Summary/ns2:OrderSummary/ns2:CurrentOrder/ns2:OrderNumber != ''">
                    <xsl:for-each select="ns1:SOInformation/ns1:Summary/ns2:OrderSummary">
                      <xsl:value-of select="ns2:CurrentOrder/ns2:OrderNumber" />
                      <xsl:if test="position() != last()">
                        , <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                      </xsl:if>
                    </xsl:for-each>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:call-template name="parseString">
                      <xsl:with-param name="list" select="ns1:DftReference"/>
                    </xsl:call-template>
                  </xsl:otherwise>
                </xsl:choose>
              </td>
            </tr>
          </xsl:if >
        </table>
        <br/>
        <xsl:if test="ns1:Dispensations/ns1:OutboundDispensation/ns1:DRN !='' and ns1:Dispensations/ns1:OutboundDispensation/ns1:Summary!=''">
          <table border="1" width="100%">
            <tr >
              <td colspan="4" align="center" valign="top">
                <b>Dispensations</b>
              </td>
            </tr>
            <tr>
              <td>
                <b>Number</b>
              </td>
              <td colspan="3">
                <b>Summary </b>
              </td>
            </tr>
            <xsl:for-each select="ns1:Dispensations/ns1:OutboundDispensation">
              <xsl:if test="ns1:GrantedBy = $Organisation_ID">
                <tr>
                  <td>
                    <xsl:value-of select="ns1:DRN"/>
                  </td>
                  <td colspan="3">
                    <xsl:value-of select="ns1:Summary"/>
                  </td>
                </tr>
              </xsl:if>
            </xsl:for-each>
          </table>
        </xsl:if>
        <br/>
        <p align="center">
          <b> Form of notice to Police</b>
        </p>
        <p align="center">
          <b>The Road Vehicles (Authorisation of Special Types)</b>
        </p>
        <p align="center">
          <b>(General) Order, 2003 Schedule 5</b>
        </p>
        <br></br>
        <table border = "1" width = "100%">
          <tr>
            <td>
              <table border = "0" width = "100%">
                <tr>
                  <td colspan="2" valign="top">
                    <b>Operator:</b>
                  </td>
                  <td colspan="3" valign="top">
                    <xsl:call-template name="parseString">
                      <xsl:with-param name="list" select="ns1:HaulierDetails/ns2:HaulierName"/>
                    </xsl:call-template>
                  </td>
                  <td colspan="3" valign="top">
                    <b>Telephone no:</b>
                  </td>
                  <td colspan="3" valign="top">
                    <xsl:call-template name="parseString">
                      <xsl:with-param name="list" select="ns1:HaulierDetails/ns2:TelephoneNumber"/>
                    </xsl:call-template>
                  </td>
                </tr>
				  <xsl:if test="ns1:OnBehalfOf!=''">
					  <tr>
						  <td colspan="2" valign="top">
							  <b>On Behalf of:</b>
						  </td>
						  <td colspan="3" valign="top">
							  <xsl:if test="ns1:OnBehalfOf">
								  <xsl:if test="contains(ns1:OnBehalfOf, '##**##')">
									  <xsl:value-of select="substring-after(ns1:OnBehalfOf, '##**##')"/>
								  </xsl:if>
								  <xsl:if test="contains(ns1:OnBehalfOf, '##**##')=false()">
									  <xsl:value-of select="ns1:OnBehalfOf"/>
								  </xsl:if>
							  </xsl:if>
						  </td>
					  </tr>
				  </xsl:if>
                <tr>
                  <td colspan="2" valign="top">
                    <b>Contact name:</b>
                  </td>
                  <td colspan="3" valign="top">
                    <xsl:call-template name="parseString">
                      <xsl:with-param name="list" select="ns1:HaulierDetails/ns2:HaulierContact"/>
                    </xsl:call-template>
                  </td>
                  <td colspan="3" valign="top">
                    <b>Fax no:</b>
                  </td>
                  <td colspan="3" valign="top">
                    <xsl:call-template name="parseString">
                      <xsl:with-param name="list" select="ns1:HaulierDetails/ns2:FaxNumber"/>
                    </xsl:call-template>
                  </td>
                </tr>
                <tr>
                  <td colspan="2" valign="top">
                    <b>Address:</b>
                  </td>
                  <td colspan="3" valign="top">
                    <table style="font-family:Arial">
                      <xsl:for-each select="ns1:HaulierDetails/ns2:HaulierAddress/ns4:Line">
                        <tr>
                          <td>
                            <xsl:call-template name="parseString">
                              <xsl:with-param name="list" select="."/>
                            </xsl:call-template>
                          </td>
                        </tr>
                      </xsl:for-each>
                    </table>

                  </td>
                  <td colspan="3" valign="top">
                    <b>E-mail address:</b>
                    <br></br>
                    <br></br>
                    <b>Operator licence no:</b>
                    <br></br>
                    <br></br>
                    <b>Operator reference no:</b>
                  </td>
                  <td colspan="3" valign="top">
                    <xsl:call-template name="parseString">
                      <xsl:with-param name="list" select="ns1:HaulierDetails/ns2:EmailAddress"/>
                    </xsl:call-template>
                    <br></br>
                    <br></br>
                    <xsl:call-template name="parseString">
                      <xsl:with-param name="list" select="ns1:HaulierDetails/ns2:Licence"/>
                    </xsl:call-template>
                    <br></br>
                    <br></br>
                    <xsl:call-template name="parseString">
                      <xsl:with-param name="list" select="ns1:HauliersReference"/>
                    </xsl:call-template>

                  </td>
                </tr>
                <tr>
                  <td colspan="2" valign="top">
                    <b>Postcode:</b>
                  </td>
                  <td colspan="3" valign="top">
                    <xsl:call-template name="parseString">
                      <xsl:with-param name="list" select="ns1:HaulierDetails/ns2:HaulierAddress/ns4:PostCode"/>
                    </xsl:call-template>

                  </td>
                  <td colspan="3" valign="top"></td>
                  <td colspan="3" valign="top"></td>
                </tr>
              </table >
            </td>
          </tr >
        </table >

        <table border = "0" width = "100%">
          <tr>
            <td>
              In pursuance of Part 2 or Part 4 of the above Order, I being the user of the under mentioned vehicle(s) to
              which the Order applies, hereby give notice that it is my intention to use the said vehicle(s) on the roads specified
              below.
            </td>
          </tr >
        </table >
        <br></br>
        <table border = "0" width = "100%">
          <tr>
            <td>
              <b> Details of the journey</b>
            </td>
          </tr>
        </table >
        <table border = "1" width = "100%">
          <tr>
            <td align="center" valign="top">
              <b> From</b>
            </td>
            <td align="center" valign="top">
              <b>Date and time</b>
            </td>
            <td align="center" valign="top">
              <b>To</b>
            </td>
            <td align="center" valign="top">
              <b>Date and time</b>
            </td>
          </tr>
          <tr>
            <td align="center" valign="top">
              <xsl:call-template name="parseString">
                <xsl:with-param name="list" select="ns1:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:StartPointListPosition/ns2:StartPoint/ns5:Description"/>
              </xsl:call-template>

            </td>
            <td align="center" valign="top">
              <xsl:if test="contains(ns1:JourneyTiming/ns2:FirstMoveDate, '##**##')">
                <strike>
                  <xsl:element name="newdate">
                    <xsl:call-template name="FormatDate">
                      <xsl:with-param name="DateTime" select="substring-before(ns1:JourneyTiming/ns2:FirstMoveDate, '##**##')"/>
                    </xsl:call-template>
                  </xsl:element>
                </strike>
                <xsl:if test="contains(ns1:JourneyTiming/ns1:StartTime, '##**##')">
                  <strike>
                    <xsl:value-of select="substring-before(ns1:JourneyTiming/ns1:StartTime, '##**##')"/>
                  </strike>
                </xsl:if>
                <xsl:if test="contains(ns1:JourneyTiming/ns1:StartTime, '##**##')=false()">
                  <strike>
                    <xsl:value-of select="ns1:JourneyTiming/ns1:StartTime"/>
                  </strike>
                </xsl:if>

                <BR />
                <b>
                  <u>
                    <xsl:element name="newdate">
                      <xsl:call-template name="FormatDate">
                        <xsl:with-param name="DateTime" select="substring-after(ns1:JourneyTiming/ns2:FirstMoveDate, '##**##')"/>
                      </xsl:call-template>
                    </xsl:element>
                  </u>
                </b>
                <xsl:if test="contains(ns1:JourneyTiming/ns1:StartTime, '##**##')">
                  <b>
                    <u>
                      <xsl:value-of select="substring-after(ns1:JourneyTiming/ns1:StartTime, '##**##')"/>
                    </u>
                  </b>
                </xsl:if>
                <xsl:if test="contains(ns1:JourneyTiming/ns1:StartTime, '##**##')=false()">
                  <b>
                    <u>
                      <xsl:value-of select="ns1:JourneyTiming/ns1:StartTime"/>
                    </u>
                  </b>
                </xsl:if>


              </xsl:if>

              <xsl:if test="contains(ns1:JourneyTiming/ns2:FirstMoveDate, '##**##')=false()">
                <xsl:element name="newdate">
                  <xsl:call-template name="FormatDate">
                    <xsl:with-param name="DateTime" select="ns1:JourneyTiming/ns2:FirstMoveDate"/>
                  </xsl:call-template>
                </xsl:element>
                <xsl:if test="contains(ns1:JourneyTiming/ns1:StartTime, '##**##')=false()">
                  <xsl:value-of select="ns1:JourneyTiming/ns1:StartTime"/>
                </xsl:if>
              </xsl:if>


            </td>
            <td align="center" valign="top">
              <xsl:call-template name="parseString">
                <xsl:with-param name="list" select="ns1:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:EndPointListPosition/ns2:EndPoint/ns5:Description"/>
              </xsl:call-template>

            </td>
            <td align="center" valign="top">
              <xsl:if test="contains(ns1:JourneyTiming/ns2:LastMoveDate, '##**##')">
                <strike>
                  <xsl:element name="newdate">
                    <xsl:call-template name="FormatDate">
                      <xsl:with-param name="DateTime" select="substring-before(ns1:JourneyTiming/ns2:LastMoveDate, '##**##')"/>
                    </xsl:call-template>
                  </xsl:element>
                </strike>
                <xsl:if test="contains(ns1:JourneyTiming/ns1:EndTime, '##**##')">
                  <strike>
                    <xsl:value-of select="substring-before(ns1:JourneyTiming/ns1:EndTime, '##**##')"/>
                  </strike>
                </xsl:if>
                <xsl:if test="contains(ns1:JourneyTiming/ns1:EndTime, '##**##')=false()">
                  <strike>
                    <xsl:value-of select="ns1:JourneyTiming/ns1:EndTime"/>
                  </strike>
                </xsl:if>

                <BR />

                <b>
                  <u>
                    <xsl:element name="newdate">
                      <xsl:call-template name="FormatDate">
                        <xsl:with-param name="DateTime" select="substring-after(ns1:JourneyTiming/ns2:LastMoveDate, '##**##')"/>
                      </xsl:call-template>
                    </xsl:element>
                  </u>
                </b>
                <xsl:if test="contains(ns1:JourneyTiming/ns1:EndTime, '##**##')">
                  <b>
                    <u>
                      <xsl:value-of select="substring-after(ns1:JourneyTiming/ns1:EndTime, '##**##')"/>
                    </u>
                  </b>
                </xsl:if>
                <xsl:if test="contains(ns1:JourneyTiming/ns1:EndTime, '##**##')=false">
                  <b>
                    <u>
                      <xsl:value-of select="ns1:JourneyTiming/ns1:EndTime"/>
                    </u>
                  </b>
                </xsl:if>

              </xsl:if>
              <xsl:if test="contains(ns1:JourneyTiming/ns2:LastMoveDate, '##**##')=false()">
                <xsl:element name="newdate">
                  <xsl:call-template name="FormatDate">
                    <xsl:with-param name="DateTime" select="ns1:JourneyTiming/ns2:LastMoveDate"/>
                  </xsl:call-template>
                </xsl:element>
                <xsl:if test="contains(ns1:JourneyTiming/ns1:EndTime, '##**##')=false()">
                  <xsl:value-of select="ns1:JourneyTiming/ns1:EndTime"/>
                </xsl:if>

              </xsl:if>

            </td>
          </tr>

          <xsl:variable name="splitRouteDescription">
            <xsl:choose>
              <xsl:when test="contains(ns1:RouteParts/ns2:RoutePartListPosition/ns2:Route, '##**##')">
                <xsl:variable name="valueLength" select="string-length(substring-before(ns1:RouteParts/ns2:RoutePartListPosition/ns2:Route, '##**##'))-1"/>
                <xsl:call-template name="parseHtmlString">
                  <xsl:with-param name="list" select="substring(substring-before(ns1:RouteParts/ns2:RoutePartListPosition/ns2:Route, '##**##'),1,$valueLength)"/>
                </xsl:call-template>
              </xsl:when>
              <xsl:otherwise>
                <xsl:variable name="valueLength" select="string-length(ns1:RouteParts/ns2:RoutePartListPosition/ns2:Route)-1"/>
                <xsl:call-template name="parseHtmlString">
                  <xsl:with-param name="list" select="substring(ns1:RouteParts/ns2:RoutePartListPosition/ns2:Route,1,$valueLength)"/>
                </xsl:call-template>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <xsl:variable name="splitRouteDescriptionChange">
            <xsl:choose>
              <xsl:when test="contains(ns1:RouteParts/ns2:RoutePartListPosition/ns2:Route, '##**##')">
                <xsl:variable name="valueLength" select="string-length(substring-after(ns1:RouteParts/ns2:RoutePartListPosition/ns2:Route, '##**##'))-1"/>
                <xsl:call-template name="parseHtmlString">
                  <xsl:with-param name="list" select="substring(substring-after(ns1:RouteParts/ns2:RoutePartListPosition/ns2:Route, '##**##'),1,$valueLength)"/>
                </xsl:call-template>
              </xsl:when>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="arrayRouteDescription" select="msxsl:node-set($splitRouteDescription)/option" />
          <xsl:variable name="arrayRouteDescriptionChange" select="msxsl:node-set($splitRouteDescriptionChange)/option" />
         

          <tr>
            <td colspan="4" valign="top">
              <b>Route: </b>
              <br/>
              <xsl:for-each select="ns1:RouteParts/ns2:RoutePartListPosition">

                <xsl:variable name="OldNewValue" select="ns2:Route" />

                <xsl:variable name="getPosition" select="position() + 1" />
                <b>
                  Leg <xsl:number/> :
                </b>
                <br/>
                <xsl:choose>
                  <xsl:when test="contains(ns2:RoutePart/ns2:RoadPart/ns2:StartPointListPosition/ns2:StartPoint/ns5:Description, '##**##')">
                    <strike>
                      <xsl:value-of select="substring-before(ns2:RoutePart/ns2:RoadPart/ns2:StartPointListPosition/ns2:StartPoint/ns5:Description, '##**##')"/>
                    </strike>
                    <b>
                      <u>
                        <xsl:value-of select="substring-after(ns2:RoutePart/ns2:RoadPart/ns2:StartPointListPosition/ns2:StartPoint/ns5:Description, '##**##')"/>
                      </u>
                    </b>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="$arrayRouteDescriptionChange[$getPosition] != ''">
                        <b>
                          <u>
                            <xsl:value-of select="ns2:RoutePart/ns2:RoadPart/ns2:StartPointListPosition/ns2:StartPoint/ns5:Description"/>
                          </u>
                        </b>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="ns2:RoutePart/ns2:RoadPart/ns2:StartPointListPosition/ns2:StartPoint/ns5:Description"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
                <xsl:choose>
                  <xsl:when test="$arrayRouteDescriptionChange[$getPosition] != ''">
                    <b>
                      <u> to</u>
                    </b>
                  </xsl:when>
                  <xsl:otherwise> to </xsl:otherwise>
                </xsl:choose>
                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                <xsl:choose>
                  <xsl:when test="contains(ns2:RoutePart/ns2:RoadPart/ns2:EndPointListPosition/ns2:EndPoint/ns5:Description, '##**##')">
                    <strike>
                      <xsl:value-of select="substring-before(ns2:RoutePart/ns2:RoadPart/ns2:EndPointListPosition/ns2:EndPoint/ns5:Description, '##**##')"/>
                    </strike>
                    <b>
                      <u>
                        <xsl:value-of select="substring-after(ns2:RoutePart/ns2:RoadPart/ns2:EndPointListPosition/ns2:EndPoint/ns5:Description, '##**##')"/>
                      </u>
                    </b>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="$arrayRouteDescriptionChange[$getPosition] != ''">
                        <b>
                          <u>
                            <xsl:value-of select="ns2:RoutePart/ns2:RoadPart/ns2:EndPointListPosition/ns2:EndPoint/ns5:Description"/>
                          </u>
                        </b>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="ns2:RoutePart/ns2:RoadPart/ns2:EndPointListPosition/ns2:EndPoint/ns5:Description"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>:
                <xsl:choose>
                  <xsl:when test="contains($OldNewValue, '##**##')">
                    <strike>
                      <xsl:value-of select="$arrayRouteDescription[$getPosition]" />
                    </strike>
                    <b>
                      <u>
                        <xsl:value-of select="$arrayRouteDescriptionChange[$getPosition]" />
                      </u>
                    </b>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="$arrayRouteDescription[$getPosition] != ''">
                        <xsl:value-of select="$arrayRouteDescription[$getPosition]" />
                      </xsl:when>
                      <xsl:otherwise>
                        <b>
                          <u>
                            <xsl:value-of select="$arrayRouteDescriptionChange[$getPosition]"/>
                          </u>
                        </b>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
                <xsl:choose>
                  <xsl:when  test="position() = last()">

                  </xsl:when>
                  <xsl:otherwise>
                    <br/>
                    <table  width="100%" cellpadding="0" cellspacing="0">
                      <tr>
                        <td style="border-top: 1px solid black"></td>
                      </tr>
                    </table>                    
                  </xsl:otherwise>
                </xsl:choose>

              </xsl:for-each>
            </td>
          </tr>

          <tr>
            <td>

            </td>
          </tr>
        </table>

        <table border = "0" width = "100%">
          <tr>
            <td>
              <b>  Notes On Escort: </b>
            </td >
          </tr >
        </table >
        <table border = "1" width = "100%">
          <tr>
            <td align="left" valign="top">
              <xsl:choose>
                <xsl:when test="contains(ns1:NotificationOnEscort, '##**##')">
                  <xsl:value-of  select="substring-after(ns1:NotificationOnEscort, '##**##')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of  select="ns1:NotificationOnEscort"/>
                </xsl:otherwise>
              </xsl:choose>
            </td>
          </tr >
        </table >
        <br></br>



        <table border = "0" width = "100%">
          <tr>
            <td>
              <b>  Notes supplied by haulier at time of notification: </b>
            </td >
          </tr >
        </table >
        <table border = "1" width = "100%">
          <tr>
            <td align="left" valign="top">
              MOVEMENT PROGRAMME:
              <xsl:call-template name="parseString">
                <xsl:with-param name="list" select="ns1:NotificationNotesFromHaulier"/>
              </xsl:call-template>
            </td>
          </tr >
        </table >
        <br></br>
        <table border = "0" width = "100%">
          <tr>
            <td>
              <b> Details of the load</b>
            </td>
          </tr>
        </table >
        <table border = "1" width = "100%">
          <tr>
            <td width="30%" valign="top">
              <b> Description of load</b>
            </td>
            <td colspan="2" width="70%" valign="top">
              <xsl:call-template name="parseString">
                <xsl:with-param name="list" select="ns1:LoadDetails/ns2:Description"/>
              </xsl:call-template>

            </td>
          </tr>
          <tr>
            <td width="30%" valign="top">
              <b> No. of movements</b>
            </td>
            <td colspan="2" width="70%" valign="top">
              <xsl:call-template name="parseString">
                <xsl:with-param name="list" select="ns1:LoadDetails/ns2:TotalMoves"/>
              </xsl:call-template>

            </td>
          </tr>
          <tr>
            <td width="30%" valign="top">
              <b>No. of pieces moved at one time</b>
            </td>
            <td colspan="2" width="70%" valign="top">
              <xsl:if test="contains(ns1:LoadDetails/ns2:MaxPiecesPerMove, '##**##')">
                <strike>
                  <xsl:value-of select="substring-before(ns1:LoadDetails/ns2:MaxPiecesPerMove, '##**##')"/>
                </strike>
                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                <b>
                  <u>
                    <xsl:value-of select="substring-after(ns1:LoadDetails/ns2:MaxPiecesPerMove, '##**##')"/>
                  </u>
                </b>
              </xsl:if>

              <xsl:if test="contains(ns1:LoadDetails/ns2:MaxPiecesPerMove, '##**##')=false()">
                <xsl:call-template name="parseString">
                  <xsl:with-param name="list" select="ns1:LoadDetails/ns2:MaxPiecesPerMove"/>
                </xsl:call-template>
              </xsl:if>
            </td>
          </tr>
        </table >
        <br></br>
        <table border = "0" width = "100%">
          <tr>
            <td>
              <b>Details of the vehicle</b>
            </td>
          </tr>
        </table >

        <xsl:variable name="PlateNoString">
          <xsl:for-each select="ns1:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:ConfigurationIdentityListPosition">
            <xsl:choose>
              <xsl:when test="ns3:ConfigurationIdentity/ns3:PlateNo!='' and  string-length(ns3:ConfigurationIdentity/ns3:PlateNo) &gt; 0">
                or <xsl:value-of select="ns3:ConfigurationIdentity/ns3:PlateNo"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:if test="ns3:ConfigurationIdentity/ns3:FleetNo !='' and  string-length(ns3:ConfigurationIdentity/ns3:FleetNo) &gt; 0">
                  or <xsl:value-of select="ns3:ConfigurationIdentity/ns3:FleetNo"/>
                </xsl:if >
              </xsl:otherwise>
            </xsl:choose>
          </xsl:for-each>
        </xsl:variable>

        <table style ="margin-left" width="100%" border = "1" align = "left">
          <tr>
            <td  valign="top">
              <b>
                Registration No. of vehicle or
                substitute
              </b>
            </td>
            <td valign="top">
              <b>Type of vehicle</b>
            </td>
          </tr>
          <tr>
            <td width="10%"  valign="top">
              <!--<xsl:value-of select="substring-after($PlateNoString,'or ')"/>-->
              <xsl:if test="substring-after($PlateNoString,'or ') !=''">
                <xsl:choose>
                  <xsl:when test="contains(substring-after($PlateNoString,'or '), '##**##')">
                    <xsl:value-of select="substring-after(substring-after($PlateNoString,'or '),' ##**##')"/>
                    <xsl:call-template name="split">
                      <xsl:with-param name="pText" select="substring-after($PlateNoString,'or ')"></xsl:with-param>
                      <xsl:with-param name="pDelim" select=" or"></xsl:with-param>
                    </xsl:call-template>
                    <!--<strike>
                        <xsl:value-of select="substring-before(substring-after($PlateNoString,'or '),'##**##')"/>
                      </strike>
                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                      <u>
                        <b>
                          <xsl:value-of select="substring-after(substring-after($PlateNoString,'or '),'##**##')"/>
                        </b>
                      </u>
                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>-->
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="substring-after($PlateNoString,'or ')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:if >
            </td>
            <td  width="40%"  valign="top">
              <xsl:for-each select="ns1:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:VehicleSummaryListPosition">
              <xsl:if test="contains(ns3:VehicleSummary/ns3:ConfigurationType, '##**##')">
                <strike>
                  <xsl:call-template name="CamelCase">
                    <xsl:with-param name="text" select="substring-before(ns3:VehicleSummary/ns3:ConfigurationType, '##**##')"/>
                  </xsl:call-template>
                </strike>
                <b>
                  <u>
                    <xsl:call-template name="CamelCase">
                      <xsl:with-param name="text" select="substring-after(ns3:VehicleSummary/ns3:ConfigurationType, '##**##')"/>
                    </xsl:call-template>
                  </u>
                </b>
              </xsl:if>

              <xsl:if test="contains(ns3:VehicleSummary/ns3:ConfigurationType, '##**##')=false()">
                <xsl:call-template name="CamelCase">
                  <xsl:with-param name="text" select="ns3:VehicleSummary/ns3:ConfigurationType"/>
                </xsl:call-template>

              </xsl:if>
                <xsl:if test="position() != last()"> or </xsl:if>
              </xsl:for-each>

            </td>
          </tr>
        </table >
        <br/>
        <br/>
        <br/>
        <br/>
        <br/>

        <xsl:variable name="ClassificationCategory" select="ns1:Classification"></xsl:variable>

        <xsl:for-each select="ns1:RouteParts/ns2:RoutePartListPosition">

          <!--1-->
          <xsl:variable name="IncludingProjectionsString">
            <xsl:for-each select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:OverallLengthListPosition">
              <xsl:choose>
                <xsl:when test="contains(ns3:OverallLength/ns3:IncludingProjections,'##**##')">
                  or <xsl:if test="substring-before(ns3:OverallLength/ns3:IncludingProjections,'##**##') !=''">
                    <xsl:value-of select="substring-before(ns3:OverallLength/ns3:IncludingProjections,'##**##')"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m**##**
                  </xsl:if >
                  <xsl:if test="substring-after(ns3:OverallLength/ns3:IncludingProjections,'##**##') !=''">
                    <xsl:value-of select="substring-after(ns3:OverallLength/ns3:IncludingProjections,'##**##')"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                  </xsl:if >
                </xsl:when>
                <xsl:otherwise>
                  <xsl:if test="ns3:OverallLength/ns3:IncludingProjections !=''">
                    or <xsl:value-of select="ns3:OverallLength/ns3:IncludingProjections"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                  </xsl:if >
                </xsl:otherwise>
              </xsl:choose>
            </xsl:for-each>
          </xsl:variable>
          <!--1 End-->
          <!--2-->
          <xsl:variable name="FrontOverhangListPositionString">
            <xsl:for-each select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:FrontOverhangListPosition">
              <xsl:choose>
                <xsl:when test="contains(ns3:FrontOverhang,'##**##')">
                  or <xsl:if test="substring-before(ns3:FrontOverhang,'##**##') !=''">
                    <xsl:value-of select="substring-before(ns3:FrontOverhang,'##**##')"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m**##**
                  </xsl:if >
                  <xsl:if test="substring-after(ns3:FrontOverhang,'##**##') !=''">
                    <xsl:value-of select="substring-after(ns3:FrontOverhang,'##**##')"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                  </xsl:if >
                </xsl:when>
                <xsl:otherwise>
                  <xsl:if test="ns3:FrontOverhang !=''">
                    or <xsl:value-of select="ns3:FrontOverhang"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                  </xsl:if >
                </xsl:otherwise>
              </xsl:choose>
            </xsl:for-each>
          </xsl:variable>
          <!--2 End-->
          <!--3-->
          <xsl:variable name="RearOverhangListPositionString">
            <xsl:for-each select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:RearOverhangListPosition">
              <xsl:choose>
                <xsl:when test="contains(ns3:RearOverhang,'##**##')">
                  or <xsl:if test="substring-before(ns3:RearOverhang,'##**##') !=''">
                    <xsl:value-of select="substring-before(ns3:RearOverhang,'##**##')"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m**##**
                  </xsl:if >
                  <xsl:if test="substring-after(ns3:RearOverhang,'##**##') !=''">
                    <xsl:value-of select="substring-after(ns3:RearOverhang,'##**##')"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                  </xsl:if >
                </xsl:when>
                <xsl:otherwise>
                  <xsl:if test="ns3:RearOverhang !=''">
                    or <xsl:value-of select="ns3:RearOverhang"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                  </xsl:if >
                </xsl:otherwise>
              </xsl:choose>
            </xsl:for-each>
          </xsl:variable>
          <!--3 End-->
          <!--4-->
          <xsl:variable name="RigidLengthListPositionString">
            <xsl:for-each select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:RigidLengthListPosition">
              <xsl:choose>
                <xsl:when test="contains(ns3:RigidLength,'##**##')">
                  or <xsl:if test="substring-before(ns3:RigidLength,'##**##') !=''">
                    <xsl:value-of select="substring-before(ns3:RigidLength,'##**##')"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m**##**
                  </xsl:if >
                  <xsl:if test="substring-after(ns3:RigidLength,'##**##') !=''">
                    <xsl:value-of select="substring-after(ns3:RigidLength,'##**##')"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                  </xsl:if >
                </xsl:when>
                <xsl:otherwise>
                  <xsl:if test="ns3:RigidLength !=''">
                    or <xsl:value-of select="ns3:RigidLength"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                  </xsl:if >
                </xsl:otherwise>
              </xsl:choose>
            </xsl:for-each>
          </xsl:variable>
          <!--4 End-->
          <!--5-->
          <xsl:variable name="OverallWidthListPositionString">
            <xsl:for-each select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:OverallWidthListPosition">
              <xsl:choose>
                <xsl:when test="contains(ns3:OverallWidth,'##**##')">
                  or <xsl:if test="substring-before(ns3:OverallWidth,'##**##') !=''">
                    <xsl:value-of select="substring-before(ns3:OverallWidth,'##**##')"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m**##**
                  </xsl:if >
                  <xsl:if test="substring-after(ns3:OverallWidth,'##**##') !=''">
                    <xsl:value-of select="substring-after(ns3:OverallWidth,'##**##')"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                  </xsl:if >
                </xsl:when>
                <xsl:otherwise>
                  <xsl:if test="ns3:OverallWidth !=''">
                    or <xsl:value-of select="ns3:OverallWidth"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                  </xsl:if >
                </xsl:otherwise>
              </xsl:choose>
            </xsl:for-each>
          </xsl:variable>
          <!--5 End-->
          <!--6-->
          <xsl:variable name="OverallHeightListPositionString">
            <xsl:for-each select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:OverallHeightListPosition">
              <xsl:choose>
                <xsl:when test="contains(ns3:OverallHeight/ns3:MaxHeight,'##**##')">
                  or <xsl:if test="substring-before(ns3:OverallHeight/ns3:MaxHeight,'##**##') !=''">
                    <xsl:value-of select="substring-before(ns3:OverallHeight/ns3:MaxHeight,'##**##')"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m**##**
                  </xsl:if >
                  <xsl:if test="substring-after(ns3:OverallHeight/ns3:MaxHeight,'##**##') !=''">
                    <xsl:value-of select="substring-after(ns3:OverallHeight/ns3:MaxHeight,'##**##')"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                  </xsl:if >
                </xsl:when>
                <xsl:otherwise>
                  <xsl:if test="ns3:OverallHeight/ns3:MaxHeight !=''">
                    or <xsl:value-of select="ns3:OverallHeight/ns3:MaxHeight"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                  </xsl:if >
                </xsl:otherwise>
              </xsl:choose>
            </xsl:for-each>
          </xsl:variable>
          <!--6 End-->
          <!--7-->
          <xsl:variable name="OverallHeightListPositionReducibleHeightString">
            <xsl:for-each select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:OverallHeightListPosition">
              <xsl:choose>
                <xsl:when test="contains(ns3:OverallHeight/ns3:ReducibleHeight,'##**##')">
                  or <xsl:if test="substring-before(ns3:OverallHeight/ns3:ReducibleHeight,'##**##') !=''">
                    <xsl:value-of select="substring-before(ns3:OverallHeight/ns3:ReducibleHeight,'##**##')"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m**##**
                  </xsl:if >
                  <xsl:if test="substring-after(ns3:OverallHeight/ns3:ReducibleHeight,'##**##') !=''">
                    <xsl:value-of select="substring-after(ns3:OverallHeight/ns3:ReducibleHeight,'##**##')"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                  </xsl:if >
                </xsl:when>
                <xsl:otherwise>
                  <xsl:if test="ns3:OverallHeight/ns3:ReducibleHeight !=''">
                    or <xsl:value-of select="ns3:OverallHeight/ns3:ReducibleHeight"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                  </xsl:if >
                </xsl:otherwise>
              </xsl:choose>
            </xsl:for-each>
          </xsl:variable>
          <!--7 End-->
          <!--8-->
          <xsl:variable name="GrossWeightListPositionString">
            <xsl:for-each select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:GrossWeightListPosition">
              <xsl:choose>
                <xsl:when test="contains(ns3:GrossWeight/ns3:Weight,'##**##')">
                  or <xsl:if test="substring-before(ns3:GrossWeight/ns3:Weight,'##**##') !=''">
                    <xsl:value-of select="substring-before(ns3:GrossWeight/ns3:Weight,'##**##')"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>kg**##**
                  </xsl:if >
                  <xsl:if test="substring-after(ns3:GrossWeight/ns3:Weight,'##**##') !=''">
                    <xsl:value-of select="substring-after(ns3:GrossWeight/ns3:Weight,'##**##')"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>kg
                  </xsl:if >
                </xsl:when>
                <xsl:otherwise>
                  <xsl:if test="ns3:GrossWeight/ns3:Weight !=''">
                    or <xsl:value-of select="ns3:GrossWeight/ns3:Weight"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>kg
                  </xsl:if >
                </xsl:otherwise>
              </xsl:choose>
            </xsl:for-each>
          </xsl:variable>
          <br/>
          <br/>
         
          <table border = "1" width = "100%">
            <tr>
              <td valign="top">
                <b>Overall length of vehicle</b>
              </td>
              <td valign="top">
                <b>Projection - front</b>
              </td>
              <td valign="top">
                <b>Projection - rear</b>
              </td>
              <td valign="top">
                <b>Rigid length </b>
              </td>
              <td valign="top">
                <b>Overall width of vehicle</b>
              </td>
              <td valign="top">
                <b>Maximum height</b>
              </td>
              <td valign="top">
                <b>Reducible height</b>
              </td>
              <td valign="top">
                <b>Gross weight</b>
              </td>
            </tr>
            <tr>
              <td valign="top">
                <!--TODO 1-->
                <xsl:if test="substring-after($IncludingProjectionsString,'or ') !=''">
                  <xsl:choose>
                    <xsl:when test="contains(substring-after($IncludingProjectionsString,'or '), '**##**')">
                      <xsl:value-of select="substring-after(substring-after($IncludingProjectionsString,'or '),' **##**')"/>
                      <strike>
                        <xsl:value-of select="substring-before(substring-after($IncludingProjectionsString,'or '),'**##**')"/>
                      </strike>
                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                      <u>
                        <b>
                          <xsl:value-of select="substring-after(substring-after($IncludingProjectionsString,'or '),'**##**')"/>
                        </b>
                      </u>
                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-after($IncludingProjectionsString,'or ')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:if >
                <xsl:if test="substring-after($IncludingProjectionsString,'or ') =false()">0</xsl:if>
              </td>
              <td valign="top">
                <!--TODO 2-->
                <xsl:if test="substring-after($FrontOverhangListPositionString,'or ') !=''">
                  <xsl:choose>
                    <xsl:when test="contains(substring-after($FrontOverhangListPositionString,'or '), '**##**')">
                      <xsl:value-of select="substring-after(substring-after($FrontOverhangListPositionString,'or '),' **##**')"/>
                      <strike>
                        <xsl:value-of select="substring-before(substring-after($FrontOverhangListPositionString,'or '),'**##**')"/>
                      </strike>
                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                      <u>
                        <b>
                          <xsl:value-of select="substring-after(substring-after($FrontOverhangListPositionString,'or '),'**##**')"/>
                        </b>
                      </u>
                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-after($FrontOverhangListPositionString,'or ')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:if >
                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
              </td>
              <td valign="top">
                <!--TODO 3-->
                <xsl:if test="substring-after($RearOverhangListPositionString,'or ') !=''">
                  <xsl:choose>
                    <xsl:when test="contains(substring-after($RearOverhangListPositionString,'or '), '**##**')">
                      <xsl:value-of select="substring-after(substring-after($RearOverhangListPositionString,'or '),' **##**')"/>
                      <strike>
                        <xsl:value-of select="substring-before(substring-after($RearOverhangListPositionString,'or '),'**##**')"/>
                      </strike>
                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                      <u>
                        <b>
                          <xsl:value-of select="substring-after(substring-after($RearOverhangListPositionString,'or '),'**##**')"/>
                        </b>
                      </u>
                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-after($RearOverhangListPositionString,'or ')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:if >
                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
              </td>
              <td valign="top">
                <!--TODO 4-->
                <xsl:if test="substring-after($RigidLengthListPositionString,'or ') !=''">
                  <xsl:choose>
                    <xsl:when test="contains(substring-after($RigidLengthListPositionString,'or '), '**##**')">
                      <xsl:value-of select="substring-after(substring-after($RigidLengthListPositionString,'or '),' **##**')"/>
                      <strike>
                        <xsl:value-of select="substring-before(substring-after($RigidLengthListPositionString,'or '),'**##**')"/>
                      </strike>
                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                      <u>
                        <b>
                          <xsl:value-of select="substring-after(substring-after($RigidLengthListPositionString,'or '),'**##**')"/>
                        </b>
                      </u>
                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-after($RigidLengthListPositionString,'or ')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:if >
                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
              </td>
              <td  valign="top">
                <!--TODO 5-->
                <xsl:if test="substring-after($OverallWidthListPositionString,'or ') !=''">
                  <xsl:choose>
                    <xsl:when test="contains(substring-after($OverallWidthListPositionString,'or '), '**##**')">
                      <xsl:value-of select="substring-after(substring-after($OverallWidthListPositionString,'or '),' **##**')"/>
                      <strike>
                        <xsl:value-of select="substring-before(substring-after($OverallWidthListPositionString,'or '),'**##**')"/>
                      </strike>
                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                      <u>
                        <b>
                          <xsl:value-of select="substring-after(substring-after($OverallWidthListPositionString,'or '),'**##**')"/>
                        </b>
                      </u>
                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-after($OverallWidthListPositionString,'or ')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:if >
                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
              </td>
              <td  valign="top">
                <!--TODO 6-->
                <xsl:if test="substring-after($OverallHeightListPositionString,'or ') !=''">
                  <xsl:choose>
                    <xsl:when test="contains(substring-after($OverallHeightListPositionString,'or '), '**##**')">
                      <xsl:value-of select="substring-after(substring-after($OverallHeightListPositionString,'or '),' **##**')"/>
                      <strike>
                        <xsl:value-of select="substring-before(substring-after($OverallHeightListPositionString,'or '),'**##**')"/>
                      </strike>
                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                      <u>
                        <b>
                          <xsl:value-of select="substring-after(substring-after($OverallHeightListPositionString,'or '),'**##**')"/>
                        </b>
                      </u>
                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-after($OverallHeightListPositionString,'or ')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:if >
                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
              </td>
              <td valign="top">
                <!--TODO 7-->
                <xsl:if test="substring-after($OverallHeightListPositionReducibleHeightString,'or ') !=''">
                  <xsl:choose>
                    <xsl:when test="contains(substring-after($OverallHeightListPositionReducibleHeightString,'or '), '**##**')">
                      <xsl:value-of select="substring-after(substring-after($OverallHeightListPositionReducibleHeightString,'or '),' **##**')"/>
                      <strike>
                        <xsl:value-of select="substring-before(substring-after($OverallHeightListPositionReducibleHeightString,'or '),'**##**')"/>
                      </strike>
                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                      <u>
                        <b>
                          <xsl:value-of select="substring-after(substring-after($OverallHeightListPositionReducibleHeightString,'or '),'**##**')"/>
                        </b>
                      </u>
                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-after($OverallHeightListPositionReducibleHeightString,'or ')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:if >
                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
              </td>
              <td  valign="top">
                <!--TODO 8-->
                <xsl:if test="substring-after($GrossWeightListPositionString,'or ') !=''">

                  <xsl:choose>
                    <xsl:when test="contains(substring-after($GrossWeightListPositionString,'or '), '**##**')">
                      <xsl:value-of select="substring-after(substring-after($GrossWeightListPositionString,'or '),' **##**')"/>
                      <strike>
                        <xsl:value-of select="substring-before(substring-after($GrossWeightListPositionString,'or '),'**##**')"/>
                      </strike>
                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                      <u>
                        <b>
                          <xsl:value-of select="substring-after(substring-after($GrossWeightListPositionString,'or '),'**##**')"/>
                        </b>
                      </u>
                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring-after($GrossWeightListPositionString,'or ')"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:if >
                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
              </td>
            </tr>
          </table>
          <br></br>

          <xsl:if test="$ClassificationCategory != ''">
            <!--For Semi Vehicles-->
              <xsl:for-each select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:VehicleSummaryListPosition">
				  <xsl:if test="ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:Summary != '' ">
                <table border = "0" width = "100%">
                  <tr>
                    <td  valign="top">
                      <b>
                        <xsl:choose>
                          <xsl:when test="contains(ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:Summary,'##**##')">
                            <xsl:value-of select="substring-after(ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:Summary,'##**##')"/>
                          </xsl:when>

                          <xsl:when test="contains(ns3:VehicleSummary/ns3:Configuration/ns3:TrackedVehicle/ns3:Summary,'##**##')">
                            <xsl:value-of select="substring-after(ns3:VehicleSummary/ns3:Configuration/ns3:TrackedVehicle/ns3:Summary,'##**##')"/>
                          </xsl:when>
                          <xsl:when test="ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:Summary != ''">
                            <xsl:value-of select="ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:Summary"/>
                          </xsl:when>

                          <xsl:when test="ns3:VehicleSummary/ns3:Configuration/ns3:TrackedVehicle/ns3:Summary != ''">
                            <xsl:value-of select="ns3:VehicleSummary/ns3:Configuration/ns3:TrackedVehicle/ns3:Summary"/>
                          </xsl:when>
                        </xsl:choose>
                      </b>
                    </td>
                  </tr>
                </table >
							</xsl:if>
				  <xsl:if test="ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle">
					  <table border = "1" width = "100%">
						  <tr>
							  <td width="25%" valign="top">
								  <b>Gross weight (kg) </b>
							  </td>
							  <td colspan="2" width="75%" valign="top">
								  <xsl:choose>
									  <xsl:when test="contains(ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:GrossWeight/ns3:Weight, '##**##')">
										  <xsl:if test="substring-before(ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:GrossWeight/ns3:Weight, '##**##') != ''">
											  <strike>
												  <xsl:value-of select="substring-before(ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:GrossWeight/ns3:Weight, '##**##')"/>
												  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>kg
											  </strike>
										  </xsl:if>
										  <xsl:if test="substring-after(ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:GrossWeight/ns3:Weight, '##**##') != ''">
											  <b>
												  <u>
													  <xsl:value-of select="substring-after(ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:GrossWeight/ns3:Weight, '##**##')"/>
													  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>kg
												  </u>
											  </b>
										  </xsl:if>
									  </xsl:when>
									  <xsl:when test="contains(ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:GrossWeight/ns3:Weight, '##**##')=false()">
										  <xsl:value-of select="ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:GrossWeight/ns3:Weight"/>
										  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>kg
									  </xsl:when>
								  </xsl:choose>
							  </td>
						  </tr>
						  <tr>
							  <td width="25%" valign="top">
								  <b>
									  No. of Wheels
									  (Wheels OR wheels x no of axles)
								  </b>
							  </td>
							  <td colspan="2" width="75%" valign="top">
								  <!--For Semi Vehicle Starts Here-->
								  <xsl:if test="ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:AxleConfiguration/ns3:WheelsPerAxleListPosition/ns3:WheelsPerAxle != ''">
									  <xsl:variable name="myConcatString">
										  <xsl:for-each select="ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:AxleConfiguration/ns3:WheelsPerAxleListPosition">
											  **#<xsl:value-of select="position()"/><xsl:value-of select="position()"/>#**
											  <!--Chirag=-->
											  <xsl:choose>
												  <xsl:when test="contains(ns3:WheelsPerAxle, '##**##')">
													  <xsl:if test="substring-before(ns3:WheelsPerAxle, '##**##') != ''">
														  <xsl:value-of select="substring-before(ns3:WheelsPerAxle, '##**##')"/>
													  </xsl:if>
													  <xsl:if test="substring-before(ns3:WheelsPerAxle, '##**##') =false()">
														  0
													  </xsl:if>
												  </xsl:when>
												  <xsl:otherwise>
													  <xsl:if test="ns3:WheelsPerAxle != ''">
														  <xsl:value-of select="ns3:WheelsPerAxle"/>
													  </xsl:if>
													  <xsl:if test="ns3:WheelsPerAxle =false()">
														  0
													  </xsl:if>
												  </xsl:otherwise>
											  </xsl:choose>
											  <xsl:text> x </xsl:text>
											  <xsl:choose>
												  <xsl:when test="contains(ns3:WheelsPerAxle/@AxleCount, '##**##')">
													  <xsl:if test="substring-before(ns3:WheelsPerAxle/@AxleCount, '##**##') != ''">
														  <xsl:value-of select="substring-before(ns3:WheelsPerAxle/@AxleCount, '##**##')"/>
													  </xsl:if>
													  <xsl:if test="substring-before(ns3:WheelsPerAxle/@AxleCount, '##**##') =false()">
														  <xsl:value-of select="ns3:WheelsPerAxle/@AxleCount"/>
													  </xsl:if>
												  </xsl:when>
												  <xsl:otherwise>
													  <xsl:if test="ns3:WheelsPerAxle/@AxleCount != ''">
														  <xsl:value-of select="ns3:WheelsPerAxle/@AxleCount"/>
													  </xsl:if>
													  <xsl:if test="ns3:WheelsPerAxle/@AxleCount =false()">
														  0
													  </xsl:if>
												  </xsl:otherwise>
											  </xsl:choose>
											  <xsl:choose>
												  <xsl:when test="contains(ns3:WheelsPerAxle, '##**##') or contains(ns3:WheelsPerAxle/@AxleCount, '##**##')">
													  <xsl:text>**##**</xsl:text>
													  <xsl:choose>
														  <xsl:when test="contains(ns3:WheelsPerAxle, '##**##')">
															  <xsl:if test="substring-after(ns3:WheelsPerAxle, '##**##') != ''">
																  <xsl:value-of select="substring-after(ns3:WheelsPerAxle, '##**##')"/>
															  </xsl:if>
															  <xsl:if test="substring-after(ns3:WheelsPerAxle, '##**##') =false()">
																  0
															  </xsl:if>
														  </xsl:when>
														  <xsl:otherwise>
															  <xsl:if test="ns3:WheelsPerAxle != ''">
																  <xsl:value-of select="ns3:WheelsPerAxle"/>
															  </xsl:if>
															  <xsl:if test="ns3:WheelsPerAxle =false()">
																  0
															  </xsl:if>
														  </xsl:otherwise>
													  </xsl:choose>
												  </xsl:when>
												  <xsl:otherwise>
												  </xsl:otherwise>
											  </xsl:choose>
											  <xsl:choose>
												  <xsl:when test="contains(ns3:WheelsPerAxle/@AxleCount, '##**##') or contains(ns3:WheelsPerAxle, '##**##')">
													  <xsl:text> x </xsl:text>
													  <xsl:if test="substring-after(ns3:WheelsPerAxle/@AxleCount, '##**##') != ''">
														  <xsl:value-of select="substring-after(ns3:WheelsPerAxle/@AxleCount, '##**##')"/>
													  </xsl:if>
													  <xsl:if test="substring-after(ns3:WheelsPerAxle/@AxleCount, '##**##') =false()">
														  <xsl:if test="ns3:WheelsPerAxle/@AxleCount != ''">
															  <xsl:value-of select="ns3:WheelsPerAxle/@AxleCount"/>
														  </xsl:if>
														  <xsl:if test="ns3:WheelsPerAxle/@AxleCount =false()">
															  0
														  </xsl:if>
													  </xsl:if>
												  </xsl:when>
												  <xsl:otherwise>
												  </xsl:otherwise>
											  </xsl:choose>
											  **#<xsl:value-of select="position()"/>#**
											  <!--Chirag=-->
										  </xsl:for-each>
									  </xsl:variable>
									  <xsl:variable name="valueLength" select="string-length($myConcatString)-1"/>
									  <xsl:for-each select="ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:AxleConfiguration/ns3:WheelsPerAxleListPosition">
										  <xsl:choose>
											  <xsl:when test="contains(substring($myConcatString,1,$valueLength), concat('**#',position(),position(),'#**'))">
												  <xsl:variable name="SubstringOfMainString" select="substring-before(substring-after(substring($myConcatString,1,$valueLength), concat('**#',position(),position(),'#**')),concat('**#',position(),'#**'))"/>
												  <xsl:choose>
													  <xsl:when test="contains($SubstringOfMainString, '**##**')">
														  <strike>
															  <xsl:value-of select="substring-before($SubstringOfMainString, '**##**')"/>
														  </strike>
														  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
														  <u>
															  <b>
																  <xsl:value-of select="substring-after($SubstringOfMainString, '**##**')"/>
															  </b>
														  </u>
														  <xsl:if test="position() != last()">, </xsl:if>
													  </xsl:when>
													  <xsl:otherwise>
														  <xsl:value-of select="$SubstringOfMainString"/>
														  <xsl:if test="position() != last()">, </xsl:if>
													  </xsl:otherwise>
												  </xsl:choose>
											  </xsl:when>
											  <xsl:otherwise>
											  </xsl:otherwise>
										  </xsl:choose>
									  </xsl:for-each>
								  </xsl:if>
								  <!--For Semi Vehicle Ends Here-->
							  </td>
						  </tr>
						  <xsl:if test="$ClassificationCategory != ''">
							  <tr>
								  <td width="25%" valign="top">
									  <b>Axle weight (kg) </b>
								  </td>
								  <td colspan="2" width="75%" valign="top">
									  <!--For Semi Vehicle Starts Here-->
									  <xsl:if test="ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:AxleConfiguration/ns3:AxleWeightListPosition/ns3:AxleWeight != ''">
										  <xsl:variable name="myConcatString">
											  <xsl:for-each select="ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:AxleConfiguration/ns3:AxleWeightListPosition">
												  **#<xsl:value-of select="position()"/><xsl:value-of select="position()"/>#**
												  <!--Chirag=-->
												  <xsl:choose>
													  <xsl:when test="contains(ns3:AxleWeight, '##**##')">
														  <xsl:if test="substring-before(ns3:AxleWeight, '##**##') != ''">
															  <xsl:value-of select="substring-before(ns3:AxleWeight, '##**##')"/> kg
														  </xsl:if>
														  <xsl:if test="substring-before(ns3:AxleWeight, '##**##') =false()">
															  0 kg
														  </xsl:if>
													  </xsl:when>
													  <xsl:otherwise>
														  <xsl:if test="ns3:AxleWeight != ''">
															  <xsl:value-of select="ns3:AxleWeight"/> kg
														  </xsl:if>
														  <xsl:if test="ns3:AxleWeight =false()">
															  0 kg
														  </xsl:if>
													  </xsl:otherwise>
												  </xsl:choose>
												  <xsl:text> x </xsl:text>
												  <xsl:choose>
													  <xsl:when test="contains(ns3:AxleWeight/@AxleCount, '##**##')">
														  <xsl:if test="substring-before(ns3:AxleWeight/@AxleCount, '##**##') != ''">
															  <xsl:value-of select="substring-before(ns3:AxleWeight/@AxleCount, '##**##')"/>
														  </xsl:if>
														  <xsl:if test="substring-before(ns3:AxleWeight/@AxleCount, '##**##') =false()">
															  <xsl:value-of select="ns3:AxleWeight/@AxleCount"/>
														  </xsl:if>
													  </xsl:when>
													  <xsl:otherwise>
														  <xsl:if test="ns3:AxleWeight/@AxleCount != ''">
															  <xsl:value-of select="ns3:AxleWeight/@AxleCount"/>
														  </xsl:if>
														  <xsl:if test="ns3:AxleWeight/@AxleCount =false()">
															  0
														  </xsl:if>
													  </xsl:otherwise>
												  </xsl:choose>
												  <xsl:choose>
													  <xsl:when test="contains(ns3:AxleWeight, '##**##') or contains(ns3:AxleWeight/@AxleCount, '##**##')">
														  <xsl:text>**##**</xsl:text>
														  <xsl:choose>
															  <xsl:when test="contains(ns3:AxleWeight, '##**##')">
																  <xsl:if test="substring-after(ns3:AxleWeight, '##**##') != ''">
																	  <xsl:value-of select="substring-after(ns3:AxleWeight, '##**##')"/> kg
																  </xsl:if>
																  <xsl:if test="substring-after(ns3:AxleWeight, '##**##') =false()">
																	  0 kg
																  </xsl:if>
															  </xsl:when>
															  <xsl:otherwise>
																  <xsl:if test="ns3:AxleWeight != ''">
																	  <xsl:value-of select="ns3:AxleWeight"/> kg
																  </xsl:if>
																  <xsl:if test="ns3:AxleWeight =false()">
																	  0 kg
																  </xsl:if>
															  </xsl:otherwise>
														  </xsl:choose>
													  </xsl:when>
													  <xsl:otherwise>
													  </xsl:otherwise>
												  </xsl:choose>
												  <xsl:choose>
													  <xsl:when test="contains(ns3:AxleWeight/@AxleCount, '##**##') or contains(ns3:AxleWeight, '##**##')">
														  <xsl:text> x </xsl:text>
														  <xsl:if test="substring-after(ns3:AxleWeight/@AxleCount, '##**##') != ''">
															  <xsl:value-of select="substring-after(ns3:AxleWeight/@AxleCount, '##**##')"/>
														  </xsl:if>
														  <xsl:if test="substring-after(ns3:AxleWeight/@AxleCount, '##**##') =false()">
															  <xsl:if test="ns3:AxleWeight/@AxleCount != ''">
																  <xsl:value-of select="ns3:AxleWeight/@AxleCount"/>
															  </xsl:if>
															  <xsl:if test="ns3:AxleWeight/@AxleCount =false()">
																  0
															  </xsl:if>
														  </xsl:if>
													  </xsl:when>
													  <xsl:otherwise>
													  </xsl:otherwise>
												  </xsl:choose>
												  **#<xsl:value-of select="position()"/>#**
												  <!--Chirag=-->
											  </xsl:for-each>
										  </xsl:variable>

										  <xsl:variable name="valueLength" select="string-length($myConcatString)-1"/>

										  <xsl:for-each select="ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:AxleConfiguration/ns3:AxleWeightListPosition">
											  <xsl:choose>
												  <xsl:when test="contains(substring($myConcatString,1,$valueLength), concat('**#',position(),position(),'#**'))">
													  <xsl:variable name="SubstringOfMainString" select="substring-before(substring-after(substring($myConcatString,1,$valueLength), concat('**#',position(),position(),'#**')),concat('**#',position(),'#**'))"/>
													  <xsl:choose>
														  <xsl:when test="contains($SubstringOfMainString, '**##**')">
															  <strike>
																  <xsl:value-of select="substring-before($SubstringOfMainString, '**##**')"/>
															  </strike>
															  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
															  <u>
																  <b>
																	  <xsl:value-of select="substring-after($SubstringOfMainString, '**##**')"/>
																  </b>
															  </u>
															  <xsl:if test="position() != last()">, </xsl:if>
														  </xsl:when>
														  <xsl:otherwise>
															  <xsl:value-of select="$SubstringOfMainString"/>
															  <xsl:if test="position() != last()">, </xsl:if>
														  </xsl:otherwise>
													  </xsl:choose>
												  </xsl:when>
												  <xsl:otherwise>
												  </xsl:otherwise>
											  </xsl:choose>
										  </xsl:for-each>
									  </xsl:if>
									  <!--For Semi Vehicle Ends Here-->
								  </td>
							  </tr>
						  </xsl:if>
						  <tr>
							  <td width="25%" valign="top">
								  <b>Axle spacing (m) </b>
							  </td>
							  <td colspan="2" width="75%" valign="top">
								  <!--For Semi Vehicle Starts Here-->
								  <xsl:if test="ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:AxleConfiguration/ns3:AxleSpacingListPosition/ns3:AxleSpacing != ''">
									  <xsl:variable name="myConcatString">
										  <xsl:for-each select="ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:AxleConfiguration/ns3:AxleSpacingListPosition">
											  **#<xsl:value-of select="position()"/><xsl:value-of select="position()"/>#**
											  <!--Chirag=-->
											  <xsl:choose>
												  <xsl:when test="contains(ns3:AxleSpacing, '##**##')">
													  <xsl:if test="substring-before(ns3:AxleSpacing, '##**##') != ''">
														  <xsl:value-of select="substring-before(ns3:AxleSpacing, '##**##')"/> m
													  </xsl:if>
													  <xsl:if test="substring-before(ns3:AxleSpacing, '##**##') =false()">
														  0 m
													  </xsl:if>
												  </xsl:when>
												  <xsl:otherwise>
													  <xsl:if test="ns3:AxleSpacing != ''">
														  <xsl:value-of select="ns3:AxleSpacing"/> m
													  </xsl:if>
													  <xsl:if test="ns3:AxleSpacing =false()">
														  0 m
													  </xsl:if>
												  </xsl:otherwise>
											  </xsl:choose>
											  <xsl:text> x </xsl:text>
											  <xsl:choose>
												  <xsl:when test="contains(ns3:AxleSpacing/@AxleCount, '##**##')">
													  <xsl:if test="substring-before(ns3:AxleSpacing/@AxleCount, '##**##') != ''">
														  <xsl:value-of select="substring-before(ns3:AxleSpacing/@AxleCount, '##**##')"/>
													  </xsl:if>
													  <xsl:if test="substring-before(ns3:AxleSpacing/@AxleCount, '##**##') =false()">
														  <xsl:value-of select="ns3:AxleSpacing/@AxleCount"/>
														  <xsl:if test="ns3:AxleSpacing/@AxleCount != ''">
															  <xsl:value-of select="ns3:AxleSpacing/@AxleCount"/>
														  </xsl:if>
														  <xsl:if test="ns3:AxleSpacing/@AxleCount =false()">
															  0
														  </xsl:if>
													  </xsl:if>
												  </xsl:when>
												  <xsl:otherwise>
													  <xsl:value-of select="ns3:AxleSpacing/@AxleCount"/>
												  </xsl:otherwise>
											  </xsl:choose>
											  <xsl:choose>
												  <xsl:when test="contains(ns3:AxleSpacing, '##**##') or contains(ns3:AxleSpacing/@AxleCount, '##**##')">
													  <xsl:text>**##**</xsl:text>
													  <xsl:choose>
														  <xsl:when test="contains(ns3:AxleSpacing, '##**##')">
															  <xsl:if test="substring-after(ns3:AxleSpacing, '##**##') != ''">
																  <xsl:value-of select="substring-after(ns3:AxleSpacing, '##**##')"/> m
															  </xsl:if>
															  <xsl:if test="substring-after(ns3:AxleSpacing, '##**##') =false()">
																  0 m
															  </xsl:if>
														  </xsl:when>
														  <xsl:otherwise>
															  <xsl:value-of select="ns3:AxleSpacing"/> m
														  </xsl:otherwise>
													  </xsl:choose>
												  </xsl:when>
												  <xsl:otherwise>
												  </xsl:otherwise>
											  </xsl:choose>
											  <xsl:choose>
												  <xsl:when test="contains(ns3:AxleSpacing/@AxleCount, '##**##') or contains(ns3:AxleSpacing, '##**##')">
													  <xsl:text> x </xsl:text>
													  <xsl:if test="substring-after(ns3:AxleSpacing/@AxleCount, '##**##') != ''">
														  <xsl:value-of select="substring-after(ns3:AxleSpacing/@AxleCount, '##**##')"/>
													  </xsl:if>
													  <xsl:if test="substring-after(ns3:AxleSpacing/@AxleCount, '##**##') =false()">
														  <xsl:value-of select="ns3:AxleSpacing/@AxleCount"/>
													  </xsl:if>
												  </xsl:when>
												  <xsl:otherwise>
												  </xsl:otherwise>
											  </xsl:choose>
											  **#<xsl:value-of select="position()"/>#**
											  <!--Chirag=-->
										  </xsl:for-each>
									  </xsl:variable>

									  <xsl:variable name="valueLength" select="string-length($myConcatString)-1"/>

									  <xsl:for-each select="ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:AxleConfiguration/ns3:AxleSpacingListPosition">
										  <xsl:choose>
											  <xsl:when test="contains(substring($myConcatString,1,$valueLength), concat('**#',position(),position(),'#**'))">
												  <xsl:variable name="SubstringOfMainString" select="substring-before(substring-after(substring($myConcatString,1,$valueLength), concat('**#',position(),position(),'#**')),concat('**#',position(),'#**'))"/>
												  <xsl:choose>
													  <xsl:when test="contains($SubstringOfMainString, '**##**')">
														  <strike>
															  <xsl:value-of select="substring-before($SubstringOfMainString, '**##**')"/>
														  </strike>
														  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
														  <u>
															  <b>
																  <xsl:value-of select="substring-after($SubstringOfMainString, '**##**')"/>
															  </b>
														  </u>
														  <xsl:if test="position() != last()">, </xsl:if>
													  </xsl:when>
													  <xsl:otherwise>
														  <xsl:value-of select="$SubstringOfMainString"/>
														  <xsl:if test="position() != last()">, </xsl:if>
													  </xsl:otherwise>
												  </xsl:choose>
											  </xsl:when>
											  <xsl:otherwise>
											  </xsl:otherwise>
										  </xsl:choose>
									  </xsl:for-each>
								  </xsl:if>
								  <!--For Semi Vehicle Ends Here-->
							  </td>
						  </tr>
					  </table>
				  </xsl:if>
                <br></br>
              </xsl:for-each>
            <!--For Semi Vehicles Ends here-->

            <!--For Non Semi Vehicles-->
              <xsl:for-each select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:Configuration/ns3:NonSemiVehicle/ns3:ComponentListPosition">
				  <xsl:if test="ns3:Component/ns3:DrawbarTractor/ns3:Summary != '' 
                  or ns3:Component/ns3:LoadBearing/ns3:Summary != '' ">
                <table border = "0" width = "100%">
                  <tr>
                    <td  valign="top">
                      <b>
                        <xsl:choose>

                          <xsl:when test="contains(ns3:Component/ns3:DrawbarTractor/ns3:Summary,'##**##')">
                            <xsl:value-of select="substring-after(ns3:Component/ns3:DrawbarTractor/ns3:Summary,'##**##')"/>
                          </xsl:when>
                          <xsl:when test="contains(ns3:Component/ns3:LoadBearing/ns3:Summary,'##**##')">
                            <xsl:value-of select="substring-after(ns3:Component/ns3:LoadBearing/ns3:Summary,'##**##')"/>
                          </xsl:when>

                          <xsl:when test="ns3:Component/ns3:DrawbarTractor/ns3:Summary != ''">
                            <xsl:value-of select="ns3:Component/ns3:DrawbarTractor/ns3:Summary"/>
                          </xsl:when>
                          <xsl:when test="ns3:Component/ns3:LoadBearing/ns3:Summary != ''">
                            <xsl:value-of select="ns3:Component/ns3:LoadBearing/ns3:Summary"/>
                          </xsl:when>

                        </xsl:choose>
                      </b>
                    </td>
                  </tr>
                </table>
				  </xsl:if>
                <table border = "1" width = "100%">
                  <tr>
                    <td width="25%" valign="top">
                      <b>Gross weight (kg) </b>
                    </td>
                    <td colspan="2" width="75%" valign="top">
                      <xsl:choose>
                        <xsl:when test="contains(ns3:Component/ns3:LoadBearing/ns3:Weight, '##**##')">

                          <xsl:if test="substring-before(ns3:Component/ns3:LoadBearing/ns3:Weight, '##**##') != ''">
                            <xsl:for-each select="ns3:Component">
                              <strike>
                                <xsl:value-of select="substring-before(ns3:LoadBearing/ns3:Weight, '##**##')"/>
                              </strike>
                            </xsl:for-each>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>kg
                          </xsl:if>

                          <xsl:if test="substring-after(ns3:Component/ns3:LoadBearing/ns3:Weight, '##**##') != ''">
                            <xsl:for-each select="ns3:Component">
                              <b>
                                <u>
                                  <xsl:value-of select="substring-after(ns3:LoadBearing/ns3:Weight, '##**##')"/>
                                </u>
                              </b>
                            </xsl:for-each>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>kg
                          </xsl:if>

                        </xsl:when>
                        <xsl:when test="contains(ns3:Component/ns3:LoadBearing/ns3:Weight, '##**##')">

                          <xsl:if test="substring-before(ns3:Component/ns3:DrawbarTractor/ns3:Weight, '##**##') != ''">
                            <xsl:for-each select="ns3:Component">
                              <strike>
                                <xsl:value-of select="substring-before(ns3:DrawbarTractor/ns3:Weight, '##**##')"/>
                              </strike>
                            </xsl:for-each>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>kg
                          </xsl:if>

                          <xsl:if test="substring-after(ns3:Component/ns3:DrawbarTractor/ns3:Weight, '##**##') != ''">
                            <xsl:for-each select="ns3:Component">
                              <b>
                                <u>
                                  <xsl:value-of select="substring-after(ns3:DrawbarTractor/ns3:Weight, '##**##')"/>
                                </u>
                              </b>
                            </xsl:for-each>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>kg
                          </xsl:if>
                        </xsl:when>

                        <xsl:when test="ns3:Component/ns3:LoadBearing/ns3:Weight != ''">
                          <xsl:for-each select="ns3:Component">
                            <xsl:value-of select="ns3:LoadBearing/ns3:Weight"/>
                          </xsl:for-each>
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>kg
                        </xsl:when>
                        <xsl:when test="ns3:Component/ns3:DrawbarTractor/ns3:Weight != ''">
                          <xsl:for-each select="ns3:Component">
                            <xsl:value-of select="ns3:DrawbarTractor/ns3:Weight"/>
                          </xsl:for-each>
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>kg
                        </xsl:when>
                      </xsl:choose>
                    </td>
                  </tr>
                  <tr>
                    <td width="25%" valign="top">
                      <b>
                        No. of Wheels
                        (Wheels OR wheels x no of axles)
                      </b>
                    </td>
                    <td colspan="2" width="75%" valign="top">

                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                      <!--For Non Semi Vehicle Drawbar Starts Here-->

                      <xsl:if test="ns3:Component/ns3:DrawbarTractor/ns3:AxleConfiguration/ns3:WheelsPerAxleListPosition/ns3:WheelsPerAxle != ''">
                        <xsl:variable name="myConcatString">
                          <xsl:for-each select="ns3:Component/ns3:DrawbarTractor/ns3:AxleConfiguration/ns3:WheelsPerAxleListPosition">
                            **#<xsl:value-of select="position()"/><xsl:value-of select="position()"/>#**
                            <!--Chirag=-->
                            <xsl:choose>
                              <xsl:when test="contains(ns3:WheelsPerAxle, '##**##')">
                                <xsl:if test="substring-before(ns3:WheelsPerAxle, '##**##') != ''">
                                  <xsl:value-of select="substring-before(ns3:WheelsPerAxle, '##**##')"/>
                                </xsl:if>
                                <xsl:if test="substring-before(ns3:WheelsPerAxle, '##**##') =false()">
                                  0
                                </xsl:if>
                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:if test="ns3:WheelsPerAxle != ''">
                                  <xsl:value-of select="ns3:WheelsPerAxle"/>
                                </xsl:if>
                                <xsl:if test="ns3:WheelsPerAxle =false()">
                                  0
                                </xsl:if>
                              </xsl:otherwise>
                            </xsl:choose>
                            <xsl:text> x </xsl:text>
                            <xsl:choose>
                              <xsl:when test="contains(ns3:WheelsPerAxle/@AxleCount, '##**##')">
                                <xsl:if test="substring-before(ns3:WheelsPerAxle/@AxleCount, '##**##') != ''">
                                  <xsl:value-of select="substring-before(ns3:WheelsPerAxle/@AxleCount, '##**##')"/>
                                </xsl:if>
                                <xsl:if test="substring-before(ns3:WheelsPerAxle/@AxleCount, '##**##') =false()">
                                  <xsl:value-of select="ns3:WheelsPerAxle/@AxleCount"/>
                                </xsl:if>
                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:if test="ns3:WheelsPerAxle/@AxleCount != ''">
                                  <xsl:value-of select="ns3:WheelsPerAxle/@AxleCount"/>
                                </xsl:if>
                                <xsl:if test="ns3:WheelsPerAxle/@AxleCount =false()">
                                  0
                                </xsl:if>
                              </xsl:otherwise>
                            </xsl:choose>
                            <xsl:choose>
                              <xsl:when test="contains(ns3:WheelsPerAxle, '##**##') or contains(ns3:WheelsPerAxle/@AxleCount, '##**##')">
                                <xsl:text>**##**</xsl:text>
                                <xsl:choose>
                                  <xsl:when test="contains(ns3:WheelsPerAxle, '##**##')">
                                    <xsl:if test="substring-after(ns3:WheelsPerAxle, '##**##') != ''">
                                      <xsl:value-of select="substring-after(ns3:WheelsPerAxle, '##**##')"/>
                                    </xsl:if>
                                    <xsl:if test="substring-after(ns3:WheelsPerAxle, '##**##') =false()">
                                      0
                                    </xsl:if>
                                  </xsl:when>
                                  <xsl:otherwise>
                                    <xsl:if test="ns3:WheelsPerAxle != ''">
                                      <xsl:value-of select="ns3:WheelsPerAxle"/>
                                    </xsl:if>
                                    <xsl:if test="ns3:WheelsPerAxle =false()">
                                      0
                                    </xsl:if>
                                  </xsl:otherwise>
                                </xsl:choose>
                              </xsl:when>
                              <xsl:otherwise>
                              </xsl:otherwise>
                            </xsl:choose>
                            <xsl:choose>
                              <xsl:when test="contains(ns3:WheelsPerAxle/@AxleCount, '##**##') or contains(ns3:WheelsPerAxle, '##**##')">
                                <xsl:text> x </xsl:text>
                                <xsl:if test="substring-after(ns3:WheelsPerAxle/@AxleCount, '##**##') != ''">
                                  <xsl:value-of select="substring-after(ns3:WheelsPerAxle/@AxleCount, '##**##')"/>
                                </xsl:if>
                                <xsl:if test="substring-after(ns3:WheelsPerAxle/@AxleCount, '##**##') =false()">
                                  <xsl:if test="ns3:WheelsPerAxle/@AxleCount != ''">
                                    <xsl:value-of select="ns3:WheelsPerAxle/@AxleCount"/>
                                  </xsl:if>
                                  <xsl:if test="ns3:WheelsPerAxle/@AxleCount =false()">
                                    0
                                  </xsl:if>
                                </xsl:if>
                              </xsl:when>
                              <xsl:otherwise>
                              </xsl:otherwise>
                            </xsl:choose>
                            **#<xsl:value-of select="position()"/>#**
                            <!--Chirag=-->
                          </xsl:for-each>
                        </xsl:variable>
                        <xsl:variable name="valueLength" select="string-length($myConcatString)-1"/>
                        <xsl:for-each select="ns3:Component/ns3:DrawbarTractor/ns3:AxleConfiguration/ns3:WheelsPerAxleListPosition">
                          <xsl:choose>
                            <xsl:when test="contains(substring($myConcatString,1,$valueLength), concat('**#',position(),position(),'#**'))">
                              <xsl:variable name="SubstringOfMainString" select="substring-before(substring-after(substring($myConcatString,1,$valueLength), concat('**#',position(),position(),'#**')),concat('**#',position(),'#**'))"/>
                              <xsl:choose>
                                <xsl:when test="contains($SubstringOfMainString, '**##**')">
                                  <strike>
                                    <xsl:value-of select="substring-before($SubstringOfMainString, '**##**')"/>
                                  </strike>
                                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                  <u>
                                    <b>
                                      <xsl:value-of select="substring-after($SubstringOfMainString, '**##**')"/>
                                    </b>
                                  </u>
                                  <xsl:if test="position() != last()">, </xsl:if>
                                </xsl:when>
                                <xsl:otherwise>
                                  <xsl:value-of select="$SubstringOfMainString"/>
                                  <xsl:if test="position() != last()">, </xsl:if>
                                </xsl:otherwise>
                              </xsl:choose>
                            </xsl:when>
                            <xsl:otherwise>
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:for-each>
                      </xsl:if>

                      <!--For Non Semi Vehicle Drawbar Ends Here-->

                      <!--For Non Semi Vehicle LoadBearing Starts Here-->

                      <xsl:if test="ns3:Component/ns3:LoadBearing/ns3:AxleConfiguration/ns3:WheelsPerAxleListPosition/ns3:WheelsPerAxle != ''">
                        <xsl:variable name="myConcatString">
                          <xsl:for-each select="ns3:Component/ns3:LoadBearing/ns3:AxleConfiguration/ns3:WheelsPerAxleListPosition">
                            **#<xsl:value-of select="position()"/><xsl:value-of select="position()"/>#**
                            <!--Chirag=-->
                            <xsl:choose>
                              <xsl:when test="contains(ns3:WheelsPerAxle, '##**##')">
                                <xsl:if test="substring-before(ns3:WheelsPerAxle, '##**##') != ''">
                                  <xsl:value-of select="substring-before(ns3:WheelsPerAxle, '##**##')"/>
                                </xsl:if>
                                <xsl:if test="substring-before(ns3:WheelsPerAxle, '##**##') =false()">
                                  0
                                </xsl:if>
                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:if test="ns3:WheelsPerAxle != ''">
                                  <xsl:value-of select="ns3:WheelsPerAxle"/>
                                </xsl:if>
                                <xsl:if test="ns3:WheelsPerAxle =false()">
                                  0
                                </xsl:if>
                              </xsl:otherwise>
                            </xsl:choose>
                            <xsl:text> x </xsl:text>
                            <xsl:choose>
                              <xsl:when test="contains(ns3:WheelsPerAxle/@AxleCount, '##**##')">
                                <xsl:if test="substring-before(ns3:WheelsPerAxle/@AxleCount, '##**##') != ''">
                                  <xsl:value-of select="substring-before(ns3:WheelsPerAxle/@AxleCount, '##**##')"/>
                                </xsl:if>
                                <xsl:if test="substring-before(ns3:WheelsPerAxle/@AxleCount, '##**##') =false()">
                                  <xsl:value-of select="ns3:WheelsPerAxle/@AxleCount"/>
                                </xsl:if>
                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:if test="ns3:WheelsPerAxle/@AxleCount != ''">
                                  <xsl:value-of select="ns3:WheelsPerAxle/@AxleCount"/>
                                </xsl:if>
                                <xsl:if test="ns3:WheelsPerAxle/@AxleCount =false()">
                                  0
                                </xsl:if>
                              </xsl:otherwise>
                            </xsl:choose>
                            <xsl:choose>
                              <xsl:when test="contains(ns3:WheelsPerAxle, '##**##') or contains(ns3:WheelsPerAxle/@AxleCount, '##**##')">
                                <xsl:text>**##**</xsl:text>
                                <xsl:choose>
                                  <xsl:when test="contains(ns3:WheelsPerAxle, '##**##')">
                                    <xsl:if test="substring-after(ns3:WheelsPerAxle, '##**##') != ''">
                                      <xsl:value-of select="substring-after(ns3:WheelsPerAxle, '##**##')"/>
                                    </xsl:if>
                                    <xsl:if test="substring-after(ns3:WheelsPerAxle, '##**##') =false()">
                                      0
                                    </xsl:if>
                                  </xsl:when>
                                  <xsl:otherwise>
                                    <xsl:if test="ns3:WheelsPerAxle != ''">
                                      <xsl:value-of select="ns3:WheelsPerAxle"/>
                                    </xsl:if>
                                    <xsl:if test="ns3:WheelsPerAxle =false()">
                                      0
                                    </xsl:if>
                                  </xsl:otherwise>
                                </xsl:choose>
                              </xsl:when>
                              <xsl:otherwise>
                              </xsl:otherwise>
                            </xsl:choose>
                            <xsl:choose>
                              <xsl:when test="contains(ns3:WheelsPerAxle/@AxleCount, '##**##') or contains(ns3:WheelsPerAxle, '##**##')">
                                <xsl:text> x </xsl:text>
                                <xsl:if test="substring-after(ns3:WheelsPerAxle/@AxleCount, '##**##') != ''">
                                  <xsl:value-of select="substring-after(ns3:WheelsPerAxle/@AxleCount, '##**##')"/>
                                </xsl:if>
                                <xsl:if test="substring-after(ns3:WheelsPerAxle/@AxleCount, '##**##') =false()">
                                  <xsl:if test="ns3:WheelsPerAxle/@AxleCount != ''">
                                    <xsl:value-of select="ns3:WheelsPerAxle/@AxleCount"/>
                                  </xsl:if>
                                  <xsl:if test="ns3:WheelsPerAxle/@AxleCount =false()">
                                    0
                                  </xsl:if>
                                </xsl:if>
                              </xsl:when>
                              <xsl:otherwise>
                              </xsl:otherwise>
                            </xsl:choose>
                            **#<xsl:value-of select="position()"/>#**
                            <!--Chirag=-->
                          </xsl:for-each>
                        </xsl:variable>
                        <xsl:variable name="valueLength" select="string-length($myConcatString)-1"/>
                        <xsl:for-each select="ns3:Component/ns3:LoadBearing/ns3:AxleConfiguration/ns3:WheelsPerAxleListPosition">
                          <xsl:choose>
                            <xsl:when test="contains(substring($myConcatString,1,$valueLength), concat('**#',position(),position(),'#**'))">
                              <xsl:variable name="SubstringOfMainString" select="substring-before(substring-after(substring($myConcatString,1,$valueLength), concat('**#',position(),position(),'#**')),concat('**#',position(),'#**'))"/>
                              <xsl:choose>
                                <xsl:when test="contains($SubstringOfMainString, '**##**')">
                                  <strike>
                                    <xsl:value-of select="substring-before($SubstringOfMainString, '**##**')"/>
                                  </strike>
                                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                  <u>
                                    <b>
                                      <xsl:value-of select="substring-after($SubstringOfMainString, '**##**')"/>
                                    </b>
                                  </u>
                                  <xsl:if test="position() != last()">, </xsl:if>
                                </xsl:when>
                                <xsl:otherwise>
                                  <xsl:value-of select="$SubstringOfMainString"/>
                                  <xsl:if test="position() != last()">, </xsl:if>
                                </xsl:otherwise>
                              </xsl:choose>
                            </xsl:when>
                            <xsl:otherwise>
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:for-each>
                      </xsl:if>

                      <!--For Non Semi Vehicle LoadBearing Ends Here-->
                    </td>
                  </tr>
                  <xsl:if test="$ClassificationCategory != ''">
                    <tr>
                      <td width="25%" valign="top">
                        <b>Axle weight (kg) </b>
                      </td>
                      <td colspan="2" width="75%" valign="top">

                        <xsl:if test="ns3:Component/ns3:DrawbarTractor/ns3:AxleConfiguration/ns3:AxleWeightListPosition/ns3:AxleWeight != ''">
                          <xsl:variable name="myConcatString">
                            <xsl:for-each select="ns3:Component/ns3:DrawbarTractor/ns3:AxleConfiguration/ns3:AxleWeightListPosition">
                              **#<xsl:value-of select="position()"/><xsl:value-of select="position()"/>#**
                              <!--Chirag=-->
                              <xsl:choose>
                                <xsl:when test="contains(ns3:AxleWeight, '##**##')">
                                  <xsl:if test="substring-before(ns3:AxleWeight, '##**##') != ''">
                                    <xsl:value-of select="substring-before(ns3:AxleWeight, '##**##')"/> kg
                                  </xsl:if>
                                  <xsl:if test="substring-before(ns3:AxleWeight, '##**##') =false()">
                                    0 kg
                                  </xsl:if>
                                </xsl:when>
                                <xsl:otherwise>
                                  <xsl:if test="ns3:AxleWeight != ''">
                                    <xsl:value-of select="ns3:AxleWeight"/> kg
                                  </xsl:if>
                                  <xsl:if test="ns3:AxleWeight =false()">
                                    0 kg
                                  </xsl:if>
                                </xsl:otherwise>
                              </xsl:choose>
                              <xsl:text> x </xsl:text>
                              <xsl:choose>
                                <xsl:when test="contains(ns3:AxleWeight/@AxleCount, '##**##')">
                                  <xsl:if test="substring-before(ns3:AxleWeight/@AxleCount, '##**##') != ''">
                                    <xsl:value-of select="substring-before(ns3:AxleWeight/@AxleCount, '##**##')"/>
                                  </xsl:if>
                                  <xsl:if test="substring-before(ns3:AxleWeight/@AxleCount, '##**##') =false()">
                                    <xsl:value-of select="ns3:AxleWeight/@AxleCount"/>
                                  </xsl:if>
                                </xsl:when>
                                <xsl:otherwise>
                                  <xsl:if test="ns3:AxleWeight/@AxleCount != ''">
                                    <xsl:value-of select="ns3:AxleWeight/@AxleCount"/>
                                  </xsl:if>
                                  <xsl:if test="ns3:AxleWeight/@AxleCount =false()">
                                    0
                                  </xsl:if>
                                </xsl:otherwise>
                              </xsl:choose>
                              <xsl:choose>
                                <xsl:when test="contains(ns3:AxleWeight, '##**##') or contains(ns3:AxleWeight/@AxleCount, '##**##')">
                                  <xsl:text>**##**</xsl:text>
                                  <xsl:choose>
                                    <xsl:when test="contains(ns3:AxleWeight, '##**##')">
                                      <xsl:if test="substring-after(ns3:AxleWeight, '##**##') != ''">
                                        <xsl:value-of select="substring-after(ns3:AxleWeight, '##**##')"/> kg
                                      </xsl:if>
                                      <xsl:if test="substring-after(ns3:AxleWeight, '##**##') =false()">
                                        0 kg
                                      </xsl:if>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:if test="ns3:AxleWeight != ''">
                                        <xsl:value-of select="ns3:AxleWeight"/> kg
                                      </xsl:if>
                                      <xsl:if test="ns3:AxleWeight =false()">
                                        0 kg
                                      </xsl:if>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                </xsl:when>
                                <xsl:otherwise>
                                </xsl:otherwise>
                              </xsl:choose>
                              <xsl:choose>
                                <xsl:when test="contains(ns3:AxleWeight/@AxleCount, '##**##') or contains(ns3:AxleWeight, '##**##')">
                                  <xsl:text> x </xsl:text>
                                  <xsl:if test="substring-after(ns3:AxleWeight/@AxleCount, '##**##') != ''">
                                    <xsl:value-of select="substring-after(ns3:AxleWeight/@AxleCount, '##**##')"/>
                                  </xsl:if>
                                  <xsl:if test="substring-after(ns3:AxleWeight/@AxleCount, '##**##') =false()">
                                    <xsl:if test="ns3:AxleWeight/@AxleCount != ''">
                                      <xsl:value-of select="ns3:AxleWeight/@AxleCount"/>
                                    </xsl:if>
                                    <xsl:if test="ns3:AxleWeight/@AxleCount =false()">
                                      0
                                    </xsl:if>
                                  </xsl:if>
                                </xsl:when>
                                <xsl:otherwise>
                                </xsl:otherwise>
                              </xsl:choose>
                              **#<xsl:value-of select="position()"/>#**
                              <!--Chirag=-->
                            </xsl:for-each>
                          </xsl:variable>

                          <xsl:variable name="valueLength" select="string-length($myConcatString)-1"/>

                          <xsl:for-each select="ns3:Component/ns3:DrawbarTractor/ns3:AxleConfiguration/ns3:AxleWeightListPosition">
                            <xsl:choose>
                              <xsl:when test="contains(substring($myConcatString,1,$valueLength), concat('**#',position(),position(),'#**'))">
                                <xsl:variable name="SubstringOfMainString" select="substring-before(substring-after(substring($myConcatString,1,$valueLength), concat('**#',position(),position(),'#**')),concat('**#',position(),'#**'))"/>
                                <xsl:choose>
                                  <xsl:when test="contains($SubstringOfMainString, '**##**')">
                                    <strike>
                                      <xsl:value-of select="substring-before($SubstringOfMainString, '**##**')"/>
                                    </strike>
                                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    <u>
                                      <b>
                                        <xsl:value-of select="substring-after($SubstringOfMainString, '**##**')"/>
                                      </b>
                                    </u>
                                    <xsl:if test="position() != last()">, </xsl:if>
                                  </xsl:when>
                                  <xsl:otherwise>
                                    <xsl:value-of select="$SubstringOfMainString"/>
                                    <xsl:if test="position() != last()">, </xsl:if>
                                  </xsl:otherwise>
                                </xsl:choose>
                              </xsl:when>
                              <xsl:otherwise>
                              </xsl:otherwise>
                            </xsl:choose>
                          </xsl:for-each>
                        </xsl:if>

                        <!--For Non Semi Vehicle Drawbar Ends Here-->

                        <!--For Non Semi Vehicle LoadBearing Starts Here-->

                        <xsl:if test="ns3:Component/ns3:LoadBearing/ns3:AxleConfiguration/ns3:AxleWeightListPosition/ns3:AxleWeight != ''">
                          <xsl:variable name="myConcatString">
                            <xsl:for-each select="ns3:Component/ns3:LoadBearing/ns3:AxleConfiguration/ns3:AxleWeightListPosition">
                              **#<xsl:value-of select="position()"/><xsl:value-of select="position()"/>#**
                              <!--Chirag=-->
                              <xsl:choose>
                                <xsl:when test="contains(ns3:AxleWeight, '##**##')">
                                  <xsl:if test="substring-before(ns3:AxleWeight, '##**##') != ''">
                                    <xsl:value-of select="substring-before(ns3:AxleWeight, '##**##')"/> kg
                                  </xsl:if>
                                  <xsl:if test="substring-before(ns3:AxleWeight, '##**##') =false()">
                                    0 kg
                                  </xsl:if>
                                </xsl:when>
                                <xsl:otherwise>
                                  <xsl:if test="ns3:AxleWeight != ''">
                                    <xsl:value-of select="ns3:AxleWeight"/> kg
                                  </xsl:if>
                                  <xsl:if test="ns3:AxleWeight =false()">
                                    0 kg
                                  </xsl:if>
                                </xsl:otherwise>
                              </xsl:choose>
                              <xsl:text> x </xsl:text>
                              <xsl:choose>
                                <xsl:when test="contains(ns3:AxleWeight/@AxleCount, '##**##')">
                                  <xsl:if test="substring-before(ns3:AxleWeight/@AxleCount, '##**##') != ''">
                                    <xsl:value-of select="substring-before(ns3:AxleWeight/@AxleCount, '##**##')"/>
                                  </xsl:if>
                                  <xsl:if test="substring-before(ns3:AxleWeight/@AxleCount, '##**##') =false()">
                                    <xsl:value-of select="ns3:AxleWeight/@AxleCount"/>
                                  </xsl:if>
                                </xsl:when>
                                <xsl:otherwise>
                                  <xsl:if test="ns3:AxleWeight/@AxleCount != ''">
                                    <xsl:value-of select="ns3:AxleWeight/@AxleCount"/>
                                  </xsl:if>
                                  <xsl:if test="ns3:AxleWeight/@AxleCount =false()">
                                    0
                                  </xsl:if>
                                </xsl:otherwise>
                              </xsl:choose>
                              <xsl:choose>
                                <xsl:when test="contains(ns3:AxleWeight, '##**##') or contains(ns3:AxleWeight/@AxleCount, '##**##')">
                                  <xsl:text>**##**</xsl:text>
                                  <xsl:choose>
                                    <xsl:when test="contains(ns3:AxleWeight, '##**##')">
                                      <xsl:if test="substring-after(ns3:AxleWeight, '##**##') != ''">
                                        <xsl:value-of select="substring-after(ns3:AxleWeight, '##**##')"/> kg
                                      </xsl:if>
                                      <xsl:if test="substring-after(ns3:AxleWeight, '##**##') =false()">
                                        0 kg
                                      </xsl:if>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:if test="ns3:AxleWeight != ''">
                                        <xsl:value-of select="ns3:AxleWeight"/> kg
                                      </xsl:if>
                                      <xsl:if test="ns3:AxleWeight =false()">
                                        0 kg
                                      </xsl:if>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                </xsl:when>
                                <xsl:otherwise>
                                </xsl:otherwise>
                              </xsl:choose>
                              <xsl:choose>
                                <xsl:when test="contains(ns3:AxleWeight/@AxleCount, '##**##') or contains(ns3:AxleWeight, '##**##')">
                                  <xsl:text> x </xsl:text>
                                  <xsl:if test="substring-after(ns3:AxleWeight/@AxleCount, '##**##') != ''">
                                    <xsl:value-of select="substring-after(ns3:AxleWeight/@AxleCount, '##**##')"/>
                                  </xsl:if>
                                  <xsl:if test="substring-after(ns3:AxleWeight/@AxleCount, '##**##') =false()">
                                    <xsl:if test="ns3:AxleWeight/@AxleCount != ''">
                                      <xsl:value-of select="ns3:AxleWeight/@AxleCount"/>
                                    </xsl:if>
                                    <xsl:if test="ns3:AxleWeight/@AxleCount =false()">
                                      0
                                    </xsl:if>
                                  </xsl:if>
                                </xsl:when>
                                <xsl:otherwise>
                                </xsl:otherwise>
                              </xsl:choose>
                              **#<xsl:value-of select="position()"/>#**
                              <!--Chirag=-->
                            </xsl:for-each>
                          </xsl:variable>

                          <xsl:variable name="valueLength" select="string-length($myConcatString)-1"/>

                          <xsl:for-each select="ns3:Component/ns3:LoadBearing/ns3:AxleConfiguration/ns3:AxleWeightListPosition">
                            <xsl:choose>
                              <xsl:when test="contains(substring($myConcatString,1,$valueLength), concat('**#',position(),position(),'#**'))">
                                <xsl:variable name="SubstringOfMainString" select="substring-before(substring-after(substring($myConcatString,1,$valueLength), concat('**#',position(),position(),'#**')),concat('**#',position(),'#**'))"/>
                                <xsl:choose>
                                  <xsl:when test="contains($SubstringOfMainString, '**##**')">
                                    <strike>
                                      <xsl:value-of select="substring-before($SubstringOfMainString, '**##**')"/>
                                    </strike>
                                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    <u>
                                      <b>
                                        <xsl:value-of select="substring-after($SubstringOfMainString, '**##**')"/>
                                      </b>
                                    </u>
                                    <xsl:if test="position() != last()">, </xsl:if>
                                  </xsl:when>
                                  <xsl:otherwise>
                                    <xsl:value-of select="$SubstringOfMainString"/>
                                    <xsl:if test="position() != last()">, </xsl:if>
                                  </xsl:otherwise>
                                </xsl:choose>
                              </xsl:when>
                              <xsl:otherwise>
                              </xsl:otherwise>
                            </xsl:choose>
                          </xsl:for-each>
                        </xsl:if>
                      </td>
                    </tr>
                  </xsl:if>
                  <tr>
                    <td width="25%" valign="top">
                      <b>Axle spacing (m) </b>
                    </td>
                    <td colspan="2" width="75%" valign="top">

                      <!--For Non Semi Vehicle Drawbar Starts Here-->

                      <xsl:if test="ns3:Component/ns3:DrawbarTractor/ns3:AxleConfiguration/ns3:AxleSpacingListPosition/ns3:AxleSpacing != ''">
                        <xsl:variable name="myConcatString">
                          <xsl:for-each select="ns3:Component/ns3:DrawbarTractor/ns3:AxleConfiguration/ns3:AxleSpacingListPosition">
                            **#<xsl:value-of select="position()"/><xsl:value-of select="position()"/>#**
                            <!--Chirag=-->
                            <xsl:choose>
                              <xsl:when test="contains(ns3:AxleSpacing, '##**##')">
                                <xsl:if test="substring-before(ns3:AxleSpacing, '##**##') != ''">
                                  <xsl:value-of select="substring-before(ns3:AxleSpacing, '##**##')"/> m
                                </xsl:if>
                                <xsl:if test="substring-before(ns3:AxleSpacing, '##**##') =false()">
                                  0 m
                                </xsl:if>
                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:if test="ns3:AxleSpacing != ''">
                                  <xsl:value-of select="ns3:AxleSpacing"/> m
                                </xsl:if>
                                <xsl:if test="ns3:AxleSpacing =false()">
                                  0 m
                                </xsl:if>
                              </xsl:otherwise>
                            </xsl:choose>
                            <xsl:text> x </xsl:text>
                            <xsl:choose>
                              <xsl:when test="contains(ns3:AxleSpacing/@AxleCount, '##**##')">
                                <xsl:if test="substring-before(ns3:AxleSpacing/@AxleCount, '##**##') != ''">
                                  <xsl:value-of select="substring-before(ns3:AxleSpacing/@AxleCount, '##**##')"/>
                                </xsl:if>
                                <xsl:if test="substring-before(ns3:AxleSpacing/@AxleCount, '##**##') =false()">
                                  <xsl:value-of select="ns3:AxleSpacing/@AxleCount"/>
                                  <xsl:if test="ns3:AxleSpacing/@AxleCount != ''">
                                    <xsl:value-of select="ns3:AxleSpacing/@AxleCount"/>
                                  </xsl:if>
                                  <xsl:if test="ns3:AxleSpacing/@AxleCount =false()">
                                    0
                                  </xsl:if>
                                </xsl:if>
                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:value-of select="ns3:AxleSpacing/@AxleCount"/>
                              </xsl:otherwise>
                            </xsl:choose>
                            <xsl:choose>
                              <xsl:when test="contains(ns3:AxleSpacing, '##**##') or contains(ns3:AxleSpacing/@AxleCount, '##**##')">
                                <xsl:text>**##**</xsl:text>
                                <xsl:choose>
                                  <xsl:when test="contains(ns3:AxleSpacing, '##**##')">
                                    <xsl:if test="substring-after(ns3:AxleSpacing, '##**##') != ''">
                                      <xsl:value-of select="substring-after(ns3:AxleSpacing, '##**##')"/> m
                                    </xsl:if>
                                    <xsl:if test="substring-after(ns3:AxleSpacing, '##**##') =false()">
                                      0 m
                                    </xsl:if>
                                  </xsl:when>
                                  <xsl:otherwise>
                                    <xsl:value-of select="ns3:AxleSpacing"/> m
                                  </xsl:otherwise>
                                </xsl:choose>
                              </xsl:when>
                              <xsl:otherwise>
                              </xsl:otherwise>
                            </xsl:choose>
                            <xsl:choose>
                              <xsl:when test="contains(ns3:AxleSpacing/@AxleCount, '##**##') or contains(ns3:AxleSpacing, '##**##')">
                                <xsl:text> x </xsl:text>
                                <xsl:if test="substring-after(ns3:AxleSpacing/@AxleCount, '##**##') != ''">
                                  <xsl:value-of select="substring-after(ns3:AxleSpacing/@AxleCount, '##**##')"/>
                                </xsl:if>
                                <xsl:if test="substring-after(ns3:AxleSpacing/@AxleCount, '##**##') =false()">
                                  <xsl:value-of select="ns3:AxleSpacing/@AxleCount"/>
                                </xsl:if>
                              </xsl:when>
                              <xsl:otherwise>
                              </xsl:otherwise>
                            </xsl:choose>
                            **#<xsl:value-of select="position()"/>#**
                            <!--Chirag=-->
                          </xsl:for-each>
                        </xsl:variable>

                        <xsl:variable name="valueLength" select="string-length($myConcatString)-1"/>

                        <xsl:for-each select="ns3:Component/ns3:DrawbarTractor/ns3:AxleConfiguration/ns3:AxleSpacingListPosition">
                          <xsl:choose>
                            <xsl:when test="contains(substring($myConcatString,1,$valueLength), concat('**#',position(),position(),'#**'))">
                              <xsl:variable name="SubstringOfMainString" select="substring-before(substring-after(substring($myConcatString,1,$valueLength), concat('**#',position(),position(),'#**')),concat('**#',position(),'#**'))"/>
                              <xsl:choose>
                                <xsl:when test="contains($SubstringOfMainString, '**##**')">
                                  <strike>
                                    <xsl:value-of select="substring-before($SubstringOfMainString, '**##**')"/>
                                  </strike>
                                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                  <u>
                                    <b>
                                      <xsl:value-of select="substring-after($SubstringOfMainString, '**##**')"/>
                                    </b>
                                  </u>
                                  <xsl:if test="position() != last()">, </xsl:if>
                                </xsl:when>
                                <xsl:otherwise>
                                  <xsl:value-of select="$SubstringOfMainString"/>
                                  <xsl:if test="position() != last()">, </xsl:if>
                                </xsl:otherwise>
                              </xsl:choose>
                            </xsl:when>
                            <xsl:otherwise>
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:for-each>
                      </xsl:if>

                      <!--For Non Semi Vehicle Drawbar Ends Here-->

                      <!--For Non Semi Vehicle LoadBearing Starts Here-->

                      <xsl:if test="ns3:Component/ns3:LoadBearing/ns3:AxleConfiguration/ns3:AxleSpacingListPosition/ns3:AxleSpacing != ''">
                        <xsl:variable name="myConcatString">
                          <xsl:for-each select="ns3:Component/ns3:LoadBearing/ns3:AxleConfiguration/ns3:AxleSpacingListPosition">
                            **#<xsl:value-of select="position()"/><xsl:value-of select="position()"/>#**
                            <!--Chirag=-->
                            <xsl:choose>
                              <xsl:when test="contains(ns3:AxleSpacing, '##**##')">
                                <xsl:if test="substring-before(ns3:AxleSpacing, '##**##') != ''">
                                  <xsl:value-of select="substring-before(ns3:AxleSpacing, '##**##')"/> m
                                </xsl:if>
                                <xsl:if test="substring-before(ns3:AxleSpacing, '##**##') =false()">
                                  0 m
                                </xsl:if>
                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:if test="ns3:AxleSpacing != ''">
                                  <xsl:value-of select="ns3:AxleSpacing"/> m
                                </xsl:if>
                                <xsl:if test="ns3:AxleSpacing =false()">
                                  0 m
                                </xsl:if>
                              </xsl:otherwise>
                            </xsl:choose>
                            <xsl:text> x </xsl:text>
                            <xsl:choose>
                              <xsl:when test="contains(ns3:AxleSpacing/@AxleCount, '##**##')">
                                <xsl:if test="substring-before(ns3:AxleSpacing/@AxleCount, '##**##') != ''">
                                  <xsl:value-of select="substring-before(ns3:AxleSpacing/@AxleCount, '##**##')"/>
                                </xsl:if>
                                <xsl:if test="substring-before(ns3:AxleSpacing/@AxleCount, '##**##') =false()">
                                  <xsl:value-of select="ns3:AxleSpacing/@AxleCount"/>
                                  <xsl:if test="ns3:AxleSpacing/@AxleCount != ''">
                                    <xsl:value-of select="ns3:AxleSpacing/@AxleCount"/>
                                  </xsl:if>
                                  <xsl:if test="ns3:AxleSpacing/@AxleCount =false()">
                                    0
                                  </xsl:if>
                                </xsl:if>
                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:value-of select="ns3:AxleSpacing/@AxleCount"/>
                              </xsl:otherwise>
                            </xsl:choose>
                            <xsl:choose>
                              <xsl:when test="contains(ns3:AxleSpacing, '##**##') or contains(ns3:AxleSpacing/@AxleCount, '##**##')">
                                <xsl:text>**##**</xsl:text>
                                <xsl:choose>
                                  <xsl:when test="contains(ns3:AxleSpacing, '##**##')">
                                    <xsl:if test="substring-after(ns3:AxleSpacing, '##**##') != ''">
                                      <xsl:value-of select="substring-after(ns3:AxleSpacing, '##**##')"/> m
                                    </xsl:if>
                                    <xsl:if test="substring-after(ns3:AxleSpacing, '##**##') =false()">
                                      0 m
                                    </xsl:if>
                                  </xsl:when>
                                  <xsl:otherwise>
                                    <xsl:value-of select="ns3:AxleSpacing"/> m
                                  </xsl:otherwise>
                                </xsl:choose>
                              </xsl:when>
                              <xsl:otherwise>
                              </xsl:otherwise>
                            </xsl:choose>
                            <xsl:choose>
                              <xsl:when test="contains(ns3:AxleSpacing/@AxleCount, '##**##') or contains(ns3:AxleSpacing, '##**##')">
                                <xsl:text> x </xsl:text>
                                <xsl:if test="substring-after(ns3:AxleSpacing/@AxleCount, '##**##') != ''">
                                  <xsl:value-of select="substring-after(ns3:AxleSpacing/@AxleCount, '##**##')"/>
                                </xsl:if>
                                <xsl:if test="substring-after(ns3:AxleSpacing/@AxleCount, '##**##') =false()">
                                  <xsl:value-of select="ns3:AxleSpacing/@AxleCount"/>
                                </xsl:if>
                              </xsl:when>
                              <xsl:otherwise>
                              </xsl:otherwise>
                            </xsl:choose>
                            **#<xsl:value-of select="position()"/>#**
                            <!--Chirag=-->
                          </xsl:for-each>
                        </xsl:variable>

                        <xsl:variable name="valueLength" select="string-length($myConcatString)-1"/>

                        <xsl:for-each select="ns3:Component/ns3:LoadBearing/ns3:AxleConfiguration/ns3:AxleSpacingListPosition">
                          <xsl:choose>
                            <xsl:when test="contains(substring($myConcatString,1,$valueLength), concat('**#',position(),position(),'#**'))">
                              <xsl:variable name="SubstringOfMainString" select="substring-before(substring-after(substring($myConcatString,1,$valueLength), concat('**#',position(),position(),'#**')),concat('**#',position(),'#**'))"/>
                              <xsl:choose>
                                <xsl:when test="contains($SubstringOfMainString, '**##**')">
                                  <strike>
                                    <xsl:value-of select="substring-before($SubstringOfMainString, '**##**')"/>
                                  </strike>
                                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                  <u>
                                    <b>
                                      <xsl:value-of select="substring-after($SubstringOfMainString, '**##**')"/>
                                    </b>
                                  </u>
                                  <xsl:if test="position() != last()">, </xsl:if>
                                </xsl:when>
                                <xsl:otherwise>
                                  <xsl:value-of select="$SubstringOfMainString"/>
                                  <xsl:if test="position() != last()">, </xsl:if>
                                </xsl:otherwise>
                              </xsl:choose>
                            </xsl:when>
                            <xsl:otherwise>
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:for-each>
                      </xsl:if>

                      <!--For Non Semi Vehicle LoadBearing Ends Here-->
                    </td>
                  </tr>

                  
                </table>
                <br></br>
              </xsl:for-each>
            <!--For Non Semi Vehicles Ends here-->
          </xsl:if>

          <!--For Tracked Vehicles-->
          <!--<xsl:if test="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:Configuration/ns3:TrackedVehicle/ns3:Summary != ''">
            <table border = "0" width = "100%">
              <tr>
                <td  valign="top">
                  <b>
                    <xsl:choose>
                      <xsl:when test="contains(ns3:VehicleSummary/ns3:Configuration/ns3:TrackedVehicle/ns3:Summary,'##**##')">
                        <xsl:value-of select="substring-after(ns3:VehicleSummary/ns3:Configuration/ns3:TrackedVehicle/ns3:Summary,'##**##')"/>
                      </xsl:when>

                      <xsl:when test="ns3:VehicleSummary/ns3:Configuration/ns3:TrackedVehicle/ns3:Summary != ''">
                        <xsl:value-of select="ns3:VehicleSummary/ns3:Configuration/ns3:TrackedVehicle/ns3:Summary"/>
                      </xsl:when>

                    </xsl:choose>
                  </b>
                </td>
              </tr>
            </table>
          </xsl:if>
			<xsl:if test="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:Configuration/ns3:TrackedVehicle/ns3:GrossWeight/ns3:Weight != ''">
            <table border = "1" width = "100%">
              <tr>
                <td width="25%" valign="top">
                  <b>Gross weight (kg) </b>
                </td>
                <td colspan="2" width="75%" valign="top">
                  <xsl:choose>
                    <xsl:when test="contains(ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:Configuration/ns3:TrackedVehicle/ns3:GrossWeight/ns3:Weight, '##**##')">
                      <xsl:if test="substring-after(ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:Configuration/ns3:TrackedVehicle/ns3:GrossWeight/ns3:Weight, '##**##') != ''">
                        <xsl:value-of select="substring-after(ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:Configuration/ns3:TrackedVehicle/ns3:GrossWeight/ns3:Weight, '##**##')"/>
                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>kg
                      </xsl:if>
                    </xsl:when>

                    <xsl:when test="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:Configuration/ns3:TrackedVehicle/ns3:GrossWeight/ns3:Weight != ''">
                      <xsl:value-of select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:Configuration/ns3:TrackedVehicle/ns3:GrossWeight/ns3:Weight"/>
                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>kg
                    </xsl:when>
                  </xsl:choose>
                </td>
              </tr>
            </table>
			</xsl:if>-->
          <!--For Tracked Vehicles Ends here-->

        </xsl:for-each>

        <br></br>
        <table border = "1" width = "100%">
          <tr>
            <td >
              <p align="center">
                <b>  List of Police Forces, Road Authorities and Bridge Authorities to which this form is sent </b>
              </p>

              <xsl:for-each select="ns1:Recipients/ns2:Contact">
                <p>
                  <xsl:if test="contains(ns2:ContactName, '##**##')">
                    <strike>
                      <xsl:value-of select="substring-before(ns2:ContactName, '##**##')"/>
                    </strike>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                    <b>
                      <u>
                        <xsl:value-of select="substring-after(ns2:ContactName, '##**##')"/>
                      </u>
                    </b>
                  </xsl:if>

                  <xsl:if test="contains(ns2:ContactName, '##**##')=false()">
                    <xsl:if test="@ContactId = $Contact_ID">
                      <b>
                        <xsl:value-of select="ns2:ContactName" />
                      </b>
                    </xsl:if>
                    <xsl:if test="@ContactId != $Contact_ID">
                      <xsl:value-of select="ns2:ContactName" />
                    </xsl:if>
                  </xsl:if>

                  , <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                  <xsl:if test="contains(ns2:OrganisationName, '##**##')">
                    <strike>
                      <xsl:value-of select="substring-before(ns2:OrganisationName, '##**##')"/>
                    </strike>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                    <b>
                      <u>
                        <xsl:value-of select="substring-after(ns2:OrganisationName, '##**##')"/>
                      </u>
                    </b>
                  </xsl:if>

                  <xsl:if test="contains(ns2:OrganisationName, '##**##')=false()">
                    <xsl:if test="@ContactId = $Contact_ID">
                      <b>
                        <xsl:value-of select="ns2:OrganisationName" />
                      </b>
                    </xsl:if>
                    <xsl:if test="@ContactId != $Contact_ID">
                      <xsl:value-of select="ns2:OrganisationName" />
                    </xsl:if>
                  </xsl:if>
                  <br/>
                </p>
              </xsl:for-each>

            </td>
          </tr>
        </table >
        <br></br>

        <br/>

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

  <xsl:template name="parseHtmlString">
    <xsl:param name="list"/>

    <xsl:if test="contains($list, '##**##')">
      <strike>
        <xsl:call-template name="tokenize">
          <xsl:with-param name="pText" select="substring-before($list, '##**##')"></xsl:with-param>
          <xsl:with-param name="pDelim" select="Leg"></xsl:with-param>
        </xsl:call-template>
      </strike>
      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
      <b>
        <u>
          <xsl:call-template name="tokenize">
            <xsl:with-param name="pText" select="substring-after($list, '##**##')"></xsl:with-param>
            <xsl:with-param name="pDelim" select="Leg"></xsl:with-param>
          </xsl:call-template>
        </u>
      </b>
    </xsl:if>

    <xsl:if test="contains($list, '##**##')=false()">
      <xsl:call-template name="tokenize">
        <xsl:with-param name="pText" select="$list"></xsl:with-param>
        <xsl:with-param name="pDelim" select="Leg"></xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template name="parseString">

    <xsl:param name="list"/>

    <xsl:if test="contains($list, '##**##')">
      <strike>
        <xsl:value-of select="substring-before($list, '##**##')"/>
      </strike>
      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
      <b>
        <u>
          <xsl:value-of select="substring-after($list, '##**##')"/>
        </u>
      </b>
    </xsl:if>

    <xsl:if test="contains($list, '##**##')=false()">
      <xsl:value-of select="$list"/>
    </xsl:if>


  </xsl:template>

  <xsl:template match="text()" name="tokenize">
    <xsl:param name="pText" select="."/>
    <xsl:param name="pDelim" select="'Leg'"/>

    <xsl:if test="string-length($pText) > 0">
      <xsl:variable name="vToken" select=
    "substring-before(concat($pText,'Leg'), 'Leg')"/>

      <option value="{$vToken}">
        <xsl:value-of select="$vToken"/>
      </option>

      <xsl:call-template name="tokenize">
        <xsl:with-param name="pText" select=
      "substring-after($pText,'Leg')"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

<xsl:template match="text()" name="split">
    <xsl:param name="pText" select="."/>
    <xsl:param name="pDelim" select="' or'"/>
    <xsl:param name="pCounter" select="1"/>
    <xsl:if test="string-length($pText) > 0">
      <xsl:variable name="vToken" select=
    "substring-before(concat($pText,' or'), ' or')"/>
      <!--<xsl:value-of select="$vToken"/>
      <xsl:value-of select="$pCounter"/>-->
       <xsl:if test="not($pCounter = 1)">or<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text></xsl:if>
      <xsl:if test="contains($vToken, '##**##')">
      <strike>
        <xsl:value-of select="substring-before($vToken,'##**##')"/>
      </strike>
      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
      <u>
        <b>
          <xsl:value-of select="substring-after($vToken,'##**##')"/>
        </b>
      </u>
      </xsl:if>
      <xsl:if test="contains($vToken, '##**##')=false()">
        <xsl:value-of select="$vToken"/>
      </xsl:if>
      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
      <xsl:call-template name="split">
        <xsl:with-param name="pText" select=
      "substring-after($pText,'or')"/>
      <xsl:with-param name="pCounter"
      select="$pCounter + 1"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>
