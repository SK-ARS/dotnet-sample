<?xml version="1.0" encoding="UTF-8"?>

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
              <img align="left" width="540" height="80" id="hdr_img"/>
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
            <td colspan="4">
              <b><!--FACSIMILE MESSAGE--></b>
              <br/>
            </td>
          </tr>
          <tr>
            <td valign="bottom">
              <b>
                ESDAL reference:
              </b>
            </td>
            <td valign="top" colspan="3">
              <xsl:value-of select="ns2:ESDALReferenceNumber/ns2:ESDALReferenceNo"/>
            </td>
          </tr>
          <tr>
            <td valign="bottom">
              <b>Notification of movement:</b>
            </td>
            <td valign="top" colspan="3">
              <xsl:if test="ns1:JourneyFromToSummary/ns2:From != '' and  ns1:JourneyFromToSummary/ns2:To!=''">
                <xsl:value-of select="ns1:JourneyFromToSummary/ns2:From" /> <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>to<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:value-of select="ns1:JourneyFromToSummary/ns2:To" />
              </xsl:if>
            </td>
          </tr>
          <tr>
            <td>
              <b>Date sent:</b>
            </td>
            <td colspan="3">
              <xsl:element name="newdate">
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="ns1:SentDateTime"/>
                  <xsl:with-param name="DateTime1" select="ns1:SentDateTime"/>
                </xsl:call-template>
              </xsl:element>
            </td>
          </tr>
          <tr>
            <td>
              <b>No of pages:</b>
            </td>
            <td colspan="3"></td>
          </tr>
          <tr>
            <td></td>
            <td colspan="3">
              <br></br>
            </td>
          </tr>

          <tr>
            <td>
              <b>NH reference:</b>
            </td>
            <td colspan="3">
              <xsl:value-of select="ns1:JobFileReference" />
            </td>
          </tr>
          <tr>
            <td>
              <b>Classification:</b>
            </td>
            <td colspan="3">
              <xsl:value-of select="translate(ns1:Classification, 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
            </td>
          </tr>

          <!--Start code for RM#3846-->
          <xsl:if test="ns1:VR1Information/ns1:Numbers/ns2:Scottish !=''">
            <tr>
              <td>
                <b>VR1 number:</b>
              </td>
              <td colspan="3" align="left">
                <xsl:choose>
                  <xsl:when test="contains(ns1:VR1Information/ns1:Numbers/ns2:Scottish, '##**##')">
                    <xsl:value-of select="substring-after(ns1:VR1Information/ns1:Numbers/ns2:Scottish, '##**##')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="ns1:VR1Information/ns1:Numbers/ns2:Scottish"/>
                  </xsl:otherwise>
                </xsl:choose>
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
                <xsl:for-each select="ns1:SOInformation/ns1:Summary/ns2:OrderSummary">
                  <xsl:value-of select="ns2:CurrentOrder/ns2:OrderNumber" />
                  <xsl:if test="position() != last()">
                    , <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                  </xsl:if>
                </xsl:for-each>
              </td>
            </tr>
          </xsl:if >
			<xsl:if test="ns1:Classification = 'vehicle special order' or ns1:Classification = 'Vehicle special order'">
				<tr>
					<td valign="bottom">
						<b>VSO no:</b>
					</td>
					<td valign ="top" colspan="3">
						<xsl:value-of select="ns2:ESDALReferenceNumber/ns2:VSONo"/>
					</td>
				</tr>
			</xsl:if>
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
		  <xsl:choose>
			  <xsl:when test="ns1:Classification = 'WHEELED CONSTRUCTION AND USE' or ns1:Classification = 'Wheeled construction and use' or ns1:Classification = 'wheeled construction and use'">
				  <p align="center">
					  <b> Form of notice to Police </b>
				  </p>
				  <p align="center">
					  <b>under the Road Vehicles (Construction and Use)</b>
				  </p>
				  <p align="center">
					  <b>Regulations 1986</b>
				  </p>
			  </xsl:when>
			  <xsl:otherwise>

				  <p align="center">
					  <b> Form of notice to Road and Bridge Authorities</b>
				  </p>
				  <p align="center">
					  <b>The Road Vehicles (Authorisation of Special Types)</b>
				  </p>
				  <p align="center">
					  <b>(General) Order, 2003 Schedule 9 Part 1</b>
				  </p>
			  </xsl:otherwise>
		  </xsl:choose>
		
        <br></br>
        <table border = "1" width = "100%">
          <tr>
            <td>
              <table border = "0" width = "100%">
                <tr>
                  <td style="width:35%;">
                    <table width = "100%">
                      <tr>
                        <td colspan="2" valign="top">
                          <b>Operator:</b>
                        </td>
                        <td colspan="3" valign="top">
                          <xsl:value-of select="ns1:HaulierDetails/ns2:HaulierName" />
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
                          <xsl:value-of select="ns1:HaulierDetails/ns2:HaulierContact" />
                        </td>
                      </tr>
                      <tr>
                        <td colspan="2" valign="top">
                          <b>Address:</b>
                        </td>
                        <td colspan="3" valign="top">
                          <xsl:for-each select="ns1:HaulierDetails/ns2:HaulierAddress/ns4:Line">
                            <div>
                              <xsl:if test=". != ''">
                                <xsl:value-of select="."/>
                              </xsl:if>
                            </div>
                          </xsl:for-each>
                        </td>
                      </tr>
                      <tr>
                        <td colspan="2" valign="top">
                          <b>Postcode:</b>
                        </td>
                        <td colspan="3" valign="top">
                          <xsl:value-of select="ns1:HaulierDetails/ns2:HaulierAddress/ns4:PostCode" />
                        </td>
                      </tr>
                    </table>
                  </td>
                  <td style="width:65%;vertical-align: top;">
                    <table width = "100%">
                      <tr>
                        <td colspan="3" valign="top">
                          <b>Telephone no:</b>
                        </td>
                        <td colspan="3" valign="top">
                          <xsl:value-of select="ns1:HaulierDetails/ns2:TelephoneNumber" />
                        </td>
                      </tr>
                      <tr>
                        <td colspan="3" valign="top">
                          <b>Fax no:</b>
                        </td>
                        <td colspan="3" valign="top">
                          <xsl:value-of select="ns1:HaulierDetails/ns2:FaxNumber" />
                        </td>
                      </tr>
                      <tr>
                        <td colspan="3" valign="top">
                          <b>E-mail address:</b>
                        </td>
                        <td colspan="3" valign="top">
                          <xsl:value-of select="ns1:HaulierDetails/ns2:EmailAddress" />
                        </td>
                      </tr>
                      <tr>
                        <td colspan="3" valign="top">
                          <b>Operator licence no:</b>
                        </td>
                        <td colspan="3" valign="top">
                          <xsl:value-of select="ns1:HaulierDetails/ns2:Licence" />
                        </td>
                      </tr>
                      <tr>
                        <td colspan="3" valign="top">
                          <b>Operator reference no:</b>
                        </td>
                        <td colspan="3" valign="top">
                          <xsl:value-of select="ns1:HauliersReference" />
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
              </table>
            </td>
          </tr>
        </table>

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
        </table>
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
              <xsl:value-of select="ns1:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:StartPointListPosition/ns2:StartPoint/ns5:Description" />
            </td>
            <td align="center" valign="top">
              <xsl:element name="newdate">
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="ns1:JourneyTiming/ns2:FirstMoveDate"/>
                </xsl:call-template>
              </xsl:element>
              <xsl:value-of select="ns1:JourneyTiming/ns1:StartTime" />
            </td>
            <td align="center" valign="top">
              <xsl:value-of select="ns1:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:EndPointListPosition/ns2:EndPoint/ns5:Description" />
            </td>
            <td align="center" valign="top">
              <xsl:element name="newdate">
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="ns1:JourneyTiming/ns2:LastMoveDate"/>
                </xsl:call-template>
              </xsl:element>
              <xsl:value-of select="ns1:JourneyTiming/ns1:EndTime" />
            </td>
          </tr>
          <tr>
            <td colspan="4" valign="top">
              <b>Route: </b>              
              <xsl:variable name="valueLength" select="string-length(ns1:RouteParts/ns2:RoutePartListPosition/ns2:Route)-1"/>
              <xsl:copy-of select="substring(ns1:RouteParts/ns2:RoutePartListPosition/ns2:Route,1,$valueLength)"/>
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

              <xsl:value-of  select="ns1:NotificationOnEscort"/>
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
              <!--<xsl:value-of select="ns1:NotificationNotesFromHaulier"/>-->
              <xsl:call-template name="newLineBySeperator">
                <xsl:with-param name="text" select="ns1:NotificationNotesFromHaulier"/>
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
              <xsl:value-of select="ns1:LoadDetails/ns2:Description"/>
            </td>
          </tr>
          <tr>
            <td width="30%" valign="top">
              <b> No. of movements</b>
            </td>
            <td colspan="2" width="70%" valign="top">
              <xsl:value-of select="ns1:LoadDetails/ns2:TotalMoves" />
            </td>
          </tr>
          <tr>
            <td width="30%" valign="top">
              <b>No. of pieces moved at one time</b>
            </td>
            <td colspan="2" width="70%" valign="top">
              <xsl:value-of select="ns1:LoadDetails/ns2:MaxPiecesPerMove" />
            </td>
          </tr>
        </table >
        <br></br>
        <xsl:for-each select="ns1:RouteParts/ns2:RoutePartListPosition">
          <table border = "0" width = "100%">
            <tr>
              <td>
                <b> Details of the vehicle</b>
              </td>
            </tr>
          </table >
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
                <xsl:choose>
                  <xsl:when test="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:ConfigurationIdentityListPosition/ns3:ConfigurationIdentity/ns3:PlateNo !=''">

                    <xsl:value-of select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:ConfigurationIdentityListPosition/ns3:ConfigurationIdentity/ns3:PlateNo"/>

                  </xsl:when>
                  <xsl:otherwise>


                    <xsl:if test="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:ConfigurationIdentityListPosition/ns3:ConfigurationIdentity/ns3:FleetNo !=''">
                      <xsl:value-of select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:ConfigurationIdentityListPosition/ns3:ConfigurationIdentity/ns3:FleetNo"/>
                    </xsl:if>

                  </xsl:otherwise>

                </xsl:choose>


              </td>
              <td  width="40%"  valign="top">
                <xsl:call-template name="CamelCase">
                  <xsl:with-param name="text">
                    <xsl:value-of select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:ConfigurationType"/>
                  </xsl:with-param>
                </xsl:call-template>
              </td>
            </tr>
          </table >
          <br></br>
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
                <xsl:choose>
                  <xsl:when test="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:OverallLengthListPosition/ns3:OverallLength/ns3:IncludingProjections!=''">
                    <xsl:value-of select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:OverallLengthListPosition/ns3:OverallLength/ns3:IncludingProjections"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                  </xsl:when>
                  <xsl:otherwise>-</xsl:otherwise>
                </xsl:choose>
              </td>
              <td valign="top">
                <xsl:choose>
                  <xsl:when test="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:FrontOverhangListPosition/ns3:FrontOverhang!=''">
                    <xsl:value-of select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:FrontOverhangListPosition/ns3:FrontOverhang"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                  </xsl:when>
                  <xsl:otherwise>-</xsl:otherwise>
                </xsl:choose>
              </td>
              <td valign="top">
                <xsl:choose>
                  <xsl:when test="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:RearOverhangListPosition/ns3:RearOverhang!=''">
                  <xsl:value-of select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:RearOverhangListPosition/ns3:RearOverhang"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                </xsl:when>
                <xsl:otherwise>-</xsl:otherwise>
                </xsl:choose>
              </td>
              <td valign="top">
                <xsl:choose>
                  <xsl:when test="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:RigidLengthListPosition/ns3:RigidLength!=''">
                    <xsl:value-of select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:RigidLengthListPosition/ns3:RigidLength"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                  </xsl:when>
                  <xsl:otherwise>-</xsl:otherwise>
                </xsl:choose>
              </td>
              <td valign="top">
                <xsl:choose>
                  <xsl:when test="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:OverallWidthListPosition/ns3:OverallWidth!=''">
                    <xsl:value-of select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:OverallWidthListPosition/ns3:OverallWidth"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                  </xsl:when>
                  <xsl:otherwise>-</xsl:otherwise>
                </xsl:choose>
              </td>
              <td valign="top">
                <xsl:choose>
                  <xsl:when test="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:OverallHeightListPosition/ns3:OverallHeight/ns3:MaxHeight!=''">
                    <xsl:value-of select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:OverallHeightListPosition/ns3:OverallHeight/ns3:MaxHeight"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                  </xsl:when>
                  <xsl:otherwise>-</xsl:otherwise>
                </xsl:choose>
              </td>
              <td valign="top">
                <xsl:choose>
                  <xsl:when test="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:OverallHeightListPosition/ns3:OverallHeight/ns3:ReducibleHeight!=''">
                    <xsl:value-of select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:OverallHeightListPosition/ns3:OverallHeight/ns3:ReducibleHeight"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                  </xsl:when>
                  <xsl:otherwise>-</xsl:otherwise>
                </xsl:choose>
              </td>
              <td valign="top">
                <xsl:choose>
                  <xsl:when test="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:GrossWeightListPosition/ns3:GrossWeight/ns3:Weight!=''">
                    <xsl:value-of select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:GrossWeightListPosition/ns3:GrossWeight/ns3:Weight"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>kg
                  </xsl:when>
                  <xsl:otherwise>-</xsl:otherwise>
                </xsl:choose>
              </td>
            </tr>
          </table>
          <br></br>

          <xsl:for-each select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/
              ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle">
            <xsl:if test="position() = 1">

              <table border = "0" width = "100%">
                <tr>
                  <td  valign="top">
                    <b>
                      <xsl:if test="ns3:Summary != ''">
                        <xsl:value-of select="ns3:Summary"/>
                      </xsl:if>
                    </b>
                  </td>
                </tr>
              </table >
              <table border = "1" width = "100%">
                <tr>
                  <td width="25%" valign="top">
                    <b>Gross weight (kg) </b>
                  </td>
                  <td colspan="2" width="75%" valign="top">
                    <xsl:if test="ns3:GrossWeight/ns3:Weight != ''">
                      <xsl:value-of select="ns3:GrossWeight/ns3:Weight"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>kg
                    </xsl:if>
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

                    <xsl:variable name="myConcatString">
                      <xsl:for-each select="ns3:AxleConfiguration/ns3:WheelsPerAxleListPosition">
                        <xsl:if test="ns3:WheelsPerAxle != '' and ns3:WheelsPerAxle/@AxleCount!=''">
                          <xsl:value-of select="ns3:WheelsPerAxle"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text> * <xsl:value-of select="ns3:WheelsPerAxle/@AxleCount"/>
                          <xsl:text>,</xsl:text>
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                        </xsl:if>
                      </xsl:for-each>
                    </xsl:variable>

                    <xsl:variable name="valueLength" select="string-length($myConcatString)-7"/>
                    <xsl:value-of select="substring($myConcatString,1,$valueLength)"/>

                    <!--<xsl:if test="ns3:AxleConfiguration/ns3:NumberOfWheel != ''">
                      <xsl:value-of select="ns3:AxleConfiguration/ns3:NumberOfWheel"/>
                    </xsl:if>-->
                  </td>
                </tr>
                <tr>
                  <td width="25%" valign="top">
                    <b>Axle weight (kg) </b>
                  </td>
                  <td colspan="2" width="75%" valign="top">
                    <xsl:variable name="myConcatString">
                      <xsl:for-each select="ns3:AxleConfiguration/ns3:AxleWeightListPosition">
                        <xsl:if test="ns3:AxleWeight != '' and ns3:AxleWeight/@AxleCount!=''">
                          <xsl:value-of select="ns3:AxleWeight"/>
                          kg * <xsl:value-of select="ns3:AxleWeight/@AxleCount"/>
                          <xsl:text>,</xsl:text>
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                        </xsl:if>
                      </xsl:for-each>
                    </xsl:variable>

                    <xsl:variable name="valueLength" select="string-length($myConcatString)-7"/>
                    <xsl:value-of select="substring($myConcatString,1,$valueLength)"/>

                  </td>
                </tr>

                <tr>
                  <td width="25%" valign="top">
                    <b>Axle spacing (m) </b>
                  </td>
                  <td colspan="2" width="75%" valign="top">
                    <xsl:variable name="myConcatString">
                      <xsl:for-each select="ns3:AxleConfiguration/ns3:AxleSpacingListPosition">
                        <xsl:if test="ns3:AxleSpacing != '' and ns3:AxleSpacing/@AxleCount!=''">
                          <xsl:value-of select="ns3:AxleSpacing"/>
                          m * <xsl:value-of select="ns3:AxleSpacing/@AxleCount"/>
                          <xsl:text>,</xsl:text>
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                        </xsl:if>
                      </xsl:for-each>
                    </xsl:variable>

                    <xsl:variable name="valueLength" select="string-length($myConcatString)-7"/>
                    <xsl:value-of select="substring($myConcatString,1,$valueLength)"/>
                  </td>
                </tr>
                <!--For showing AxleSpacing to Following for Non Semi Vehicles-->
                
                <!--For showing AxleSpacing to Following for Non Semi Vehicles ends here-->
              </table >
              <br></br>
            </xsl:if >
          </xsl:for-each>
        </xsl:for-each>
        <p>
          <br/>
          <xsl:for-each select="ns1:StructureDetails/ns1:div/ns1:table/ns1:tbody/ns1:tr">
            <b>
              <xsl:value-of select="ns1:th"/>
            </b>
            <p>
              <xsl:value-of select="ns1:td" />
            </p>
          </xsl:for-each>
          <br></br>
        </p>
        <br></br>

        <xsl:variable name="logInOrganisationValue">
          <xsl:for-each select="ns1:Recipients/ns2:Contact">
            <xsl:if test="@ContactId = $Contact_ID">
              <xsl:value-of select="ns2:OrganisationName"/>
            </xsl:if>
          </xsl:for-each>
        </xsl:variable>

        <table border = "1" width = "100%">
          <tr>
            <td >
              <p align="center">
                <b>  List of Police Forces, Road Authorities and Bridge Authorities to which this form is sent </b>
              </p >

              <xsl:for-each select="ns1:Recipients/ns2:Contact">
                <p>
                  <xsl:if test="@ContactId = $Contact_ID">
                    <b>
                      <xsl:value-of select="ns2:ContactName" />,<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:value-of select="ns2:OrganisationName" /><br/>
                    </b>
                  </xsl:if>
                  <xsl:if test="@ContactId != $Contact_ID">
                    <xsl:value-of select="ns2:ContactName" />,<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:value-of select="ns2:OrganisationName" /><br/>
                  </xsl:if>
                </p>
              </xsl:for-each>

            </td>
          </tr>
        </table >
        <br></br>
        <table border = "0" >
          <tr>
            <td colspan="75" >
              <b>Form of Indemnity</b>
            </td>
          </tr>
          <tr>
            <td colspan="75">
              <b>
                THE INDEMNITY
              </b>
            </td>
          </tr>
          <tr>
            <td valign="top">
              1.
            </td>
            <td colspan="74" >
              We<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:value-of select="ns1:HaulierDetails/ns2:HaulierName" />
              <xsl:if test="ns1:OnBehalfOf">
                (on behalf of <xsl:value-of select="ns1:OnBehalfOf" />)
              </xsl:if>
              agree to indemnify you <xsl:value-of select="$logInOrganisationValue"/>, in respect of any damage that is caused in the course of a journey of which
              you have been notified under the Road Vehicles (Authorisation of Special Types)(General) Order 2003 (which is
              referred to below as "the 2003 Order").

            </td>
          </tr>
          <tr>
            <td valign="top" >
              2.
            </td>
            <td colspan="74">
              This relates to the journey scheduled to take place on
              <xsl:element name="newdate">
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="ns1:JourneyTiming/ns2:FirstMoveDate"/>
                </xsl:call-template>
              </xsl:element> starting with the date on which the
              indemnity was signed.
            </td>
          </tr>
          <tr>
            <td colspan="75" >
              <b>
                The damage covered:
              </b>
            </td>
          </tr>
          <tr>
            <td valign="top">
              3.
            </td>
            <td colspan="74">
              Except as stated in paragraph 4, the damage in respect of which this indemnity is given is limited to any
              damage caused to any road or bridge for the maintenance of which you are responsible.
            </td>
          </tr>

          <tr>
            <td valign="top">
              4.
            </td>
            <td colspan="74">
              This indemnity also extends to any damage caused to any other road or bridge that is used in the course of
              any journey to which the indemnity relates, in any case where a separate indemnity required by the 2003 Order
              has not been given to, or received by, the authority, body or person ("third party") which is responsible for the
              maintenance of that other road or bridge.
            </td>
          </tr>
          <tr>
            <td colspan="75" >
              <b>
                The cause of damage:
              </b>
            </td>
          </tr>
          <tr>
            <td valign="top">
              5.
            </td>
            <td colspan="74">
              The damage covered in this indemnity is limited to damage caused by - (a) the construction of any vehicle
              used; (b) the weight transmitted to the road surface by any vehicle used; (c) the dimensions, distribution or
              adjustment of the load carried on any vehicle used in the carriage of an abnormal indivisible load; (d) any vehicle
              other than the vehicle used in any case where that damage results from the vehicle used (but excluding any
              damage caused, or contributed to, by the negligence of the driver of the other vehicle).
            </td>
          </tr>
          <tr>
            <td colspan="75" >
              <b>
                Enforcement of indemnity:
              </b>
            </td>
          </tr>
          <tr>
            <td valign="top">
              6.
            </td>
            <td colspan="74">
              This indemnity is enforceable by you, to the extent of the damage specified in paragraph 3.
            </td>
          </tr>
          <tr>
            <td  valign="top">
              7.
            </td>
            <td colspan="74">
              This indemnity is enforceable by any third party referred to in paragraph 4, in its own right, to the extent of any
              damage caused to any road or bridge for the maintenance of which it is responsible (but only if it has not already
              recovered payment in respect of that damage by virtue of a claim made by it under the equivalent provision in
              another indemnity given under the 2003 Order).
            </td>
          </tr>
          <tr>
            <td valign="top">
              8.
            </td>
            <td colspan="74">
              A claim in respect of damage covered by this indemnity will only be entertained if the claim - (a) states the
              occasion and place of the damage; and (b) is made before the end of the period of 12 months starting with the
              date on which the vehicle was last used in the course of the journey during which the damage occurred.

            </td>
          </tr>

        </table >
        <br/>
        <table width="100%" >
          <tr>
            <td width ="60%">
              <b>
                Date:
              </b>
              <xsl:choose>
                <xsl:when test="contains(ns1:SentDateTime, '##**##')">
                  <xsl:element name="newdate">
                    <xsl:call-template name="FormatDate">
                      <xsl:with-param name="DateTime" select="substring-after(ns1:SentDateTime, '##**##')"/>
                      <xsl:with-param name="DateTime1" select="substring-after(ns1:SentDateTime, '##**##')"/>
                    </xsl:call-template>
                  </xsl:element>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:element name="newdate">
                    <xsl:call-template name="FormatDate">
                      <xsl:with-param name="DateTime" select="ns1:SentDateTime"/>
                      <xsl:with-param name="DateTime1" select="ns1:SentDateTime"/>
                    </xsl:call-template>
                  </xsl:element>
                </xsl:otherwise>
              </xsl:choose>
            </td>
            <td width ="40%">
              <b>Signed: </b>
              <xsl:value-of select="ns1:HaulierDetails/ns2:HaulierContact" />
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
  <xsl:template name="newLineBySeperator">
    <xsl:param name="text"/>
    <xsl:param name="delimiter" select="'|'"/>
    <xsl:choose>
      <xsl:when test="contains($text, $delimiter)">
        <xsl:value-of select="substring-before($text, $delimiter)"/>
        <br/>
        <!-- recursive call -->
        <xsl:call-template name="newLineBySeperator">
          <xsl:with-param name="text" select="substring-after($text, $delimiter)"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$text"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
