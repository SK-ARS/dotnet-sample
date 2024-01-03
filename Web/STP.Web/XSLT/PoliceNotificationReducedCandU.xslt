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
	<xsl:param name="DocType"></xsl:param>
	<xsl:param name="OrganisationName"></xsl:param>
	<xsl:param name="HAReferenceNumber"></xsl:param>
	<xsl:param name="UnitType"></xsl:param>

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
			<body style="font-family:Arial;font-size:8px;">

				<div>
					<div style="text-align: end;padding-top: 1cm;padding-right: 2cm;">
						<img align="right" width="540"  id="hdr_img"/>
					</div>
				</div>
				<br />
				<table  bgcolor="#e9eef4">
					<tr>
						<td style="padding-left:3em;padding-bottom:3em;">
							<h2>Notification – Reduced Version</h2>

						</td>
					</tr>
					<tr>
						<td style="padding-left:3em">

							<h4>
								Classification: <span style="color:#275795">
									<xsl:choose>
										<xsl:when test="contains(ns1:Classification, '##**##')">
                      <xsl:value-of select="translate(substring-after(ns1:Classification, '##**##'), 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="translate(ns1:Classification, 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
                    </xsl:otherwise>
									</xsl:choose>
								</span>
							</h4>
							<br />
						</td>
					</tr>
					<br>
					</br>
				</table>
				<br />

				<table border = "0" width = "100%">
					<tr>
						<td>
							<b>
								<xsl:choose>
									<xsl:when test="contains(ns2:ESDALReferenceNumber/ns2:ESDALReferenceNo, '##**##')">
										<xsl:value-of select="substring-after(ns2:ESDALReferenceNumber/ns2:ESDALReferenceNo, '##**##')"/>
									</xsl:when>
									<xsl:when test="contains(ns2:ESDALReferenceNumber/ns2:ESDALReferenceNo, '/S')">
										<xsl:value-of select="ns2:ESDALReferenceNumber/ns2:Mnemonic" />/<xsl:value-of select="ns2:ESDALReferenceNumber/ns2:MovementProjectNumber" />/S<xsl:value-of select=" ns2:ESDALReferenceNumber/ns2:MovementVersion" />#<xsl:value-of select="ns2:ESDALReferenceNumber/ns2:NotificationNumber" />
									</xsl:when>
									<xsl:when test="ns2:ESDALReferenceNumber/ns2:ESDALReferenceNo != ''">
										<xsl:value-of select="ns2:ESDALReferenceNumber/ns2:ESDALReferenceNo"/>
									</xsl:when>
									<xsl:when test="ns2:ESDALReferenceNumber/ns2:Mnemonic != '' and ns2:ESDALReferenceNumber/ns2:MovementProjectNumber != ''
                    and ns2:ESDALReferenceNumber/ns2:MovementVersion != '' and ns2:ESDALReferenceNumber/ns2:NotificationNumber!='' and ns2:ESDALReferenceNumber/ns2:MovementVersion > 1">
										<xsl:value-of select="ns2:ESDALReferenceNumber/ns2:Mnemonic" />/<xsl:value-of select="ns2:ESDALReferenceNumber/ns2:MovementProjectNumber" />/<xsl:value-of select=" ns2:ESDALReferenceNumber/ns2:MovementVersion" />#<xsl:value-of select="ns2:ESDALReferenceNumber/ns2:NotificationNumber" />(<xsl:value-of select=" ns2:ESDALReferenceNumber/ns2:MovementVersion" />)
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="ns2:ESDALReferenceNumber/ns2:Mnemonic" />/<xsl:value-of select="ns2:ESDALReferenceNumber/ns2:MovementProjectNumber" />/<xsl:value-of select=" ns2:ESDALReferenceNumber/ns2:MovementVersion" />#<xsl:value-of select="ns2:ESDALReferenceNumber/ns2:NotificationNumber" />
									</xsl:otherwise>
								</xsl:choose>

								<xsl:if test="ns1:Classification != ''">
									<!--Changes for RM#4931-->
									<xsl:choose>
										<xsl:when test="contains(ns2:ESDALReferenceNumber/ns2:ESDALReferenceNo, '##**##')">
											- Re-Notification - for 
										</xsl:when>
										<xsl:otherwise>
											- Notification - for 
										</xsl:otherwise>
									</xsl:choose>
									<xsl:choose>
										<xsl:when test="contains(ns1:Classification, '##**##')">
                      <xsl:value-of select="translate(substring-after(ns1:Classification, '##**##'), 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="translate(ns1:Classification, 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
                    </xsl:otherwise>
									</xsl:choose>
								</xsl:if >
								- PRINTABLE NOTIFICATION


							</b>
						</td>
					</tr>
				</table>

				<br/>
				<p align="center">
					<b>Form of notice to Police under the Road Vehicles (Construction and Use) Regulations 1986</b>
				</p>
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
									<td style="width:65%">
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
													<b>Email address:</b>
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
													<xsl:if test="contains(ns1:HaulierDetails/ns2:Licence, '##**##')">
														<xsl:value-of select="substring-after(ns1:HaulierDetails/ns2:Licence, '##**##')"/>
													</xsl:if>

													<xsl:if test="contains(ns1:HaulierDetails/ns2:Licence, '##**##')=false()">
														<xsl:value-of select="ns1:HaulierDetails/ns2:Licence" />
													</xsl:if>
												</td>
											</tr>
											<tr>
												<td colspan="3" valign="top">
													<b>Operator reference no:</b>
												</td>
												<td colspan="3" valign="top">
													<xsl:if test="contains(ns1:HauliersReference, '##**##')">
														<xsl:value-of select="substring-after(ns1:HauliersReference, '##**##')"/>
													</xsl:if>

													<xsl:if test="contains(ns1:HauliersReference, '##**##')=false()">
														<xsl:value-of select="ns1:HauliersReference"/>
													</xsl:if>
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
							<b>Particulars of the journey</b>
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
							<xsl:if test="contains(ns1:JourneyFromTo/ns2:From, '##**##')">
								<xsl:value-of select="substring-after(ns1:JourneyFromTo/ns2:From, '##**##')"/>
							</xsl:if>

							<xsl:if test="contains(ns1:JourneyFromTo/ns2:From, '##**##')=false()">
								<xsl:value-of select="ns1:JourneyFromTo/ns2:From" />
							</xsl:if>
						</td>
						<td align="center" valign="top">
							<xsl:if test="contains(ns1:JourneyTiming/ns2:FirstMoveDate, '##**##')">
								<xsl:element name="newdate">
									<xsl:call-template name="FormatDate">
										<xsl:with-param name="DateTime" select="substring-after(ns1:JourneyTiming/ns2:FirstMoveDate, '##**##')"/>
									</xsl:call-template>
								</xsl:element>
							</xsl:if>

							<xsl:if test="contains(ns1:JourneyTiming/ns2:FirstMoveDate, '##**##')=false()">
								<xsl:element name="newdate">
									<xsl:call-template name="FormatDate">
										<xsl:with-param name="DateTime" select="ns1:JourneyTiming/ns2:FirstMoveDate"/>
									</xsl:call-template>
								</xsl:element>
							</xsl:if>

							<xsl:if test="contains(ns1:JourneyTiming/ns1:StartTime, '##**##')">
								<xsl:value-of select="substring-after(ns1:JourneyTiming/ns1:StartTime, '##**##')"/>
							</xsl:if>

							<xsl:if test="contains(ns1:JourneyTiming/ns1:StartTime, '##**##')=false()">
								<xsl:value-of select="ns1:JourneyTiming/ns1:StartTime" />
							</xsl:if>
						</td>
						<td align="center" valign="top">

							<xsl:if test="contains(ns1:JourneyFromTo/ns2:To, '##**##')">
								<xsl:value-of select="substring-after(ns1:JourneyFromTo/ns2:To, '##**##')"/>
							</xsl:if>

							<xsl:if test="contains(ns1:JourneyFromTo/ns2:To, '##**##')=false()">
								<xsl:value-of select="ns1:JourneyFromTo/ns2:To" />
							</xsl:if>
						</td>
						<td align="center" valign="top">
							<xsl:if test="contains(ns1:JourneyTiming/ns2:LastMoveDate, '##**##')">
								<xsl:element name="newdate">
									<xsl:call-template name="FormatDate">
										<xsl:with-param name="DateTime" select="substring-after(ns1:JourneyTiming/ns2:LastMoveDate, '##**##')"/>
									</xsl:call-template>
								</xsl:element>
							</xsl:if>

							<xsl:if test="contains(ns1:JourneyTiming/ns2:LastMoveDate, '##**##')=false()">
								<xsl:element name="newdate">
									<xsl:call-template name="FormatDate">
										<xsl:with-param name="DateTime" select="ns1:JourneyTiming/ns2:LastMoveDate"/>
									</xsl:call-template>
								</xsl:element>
							</xsl:if>

							<xsl:if test="contains(ns1:JourneyTiming/ns1:EndTime, '##**##')">
								<xsl:value-of select="substring-after(ns1:JourneyTiming/ns1:EndTime, '##**##')"/>
							</xsl:if>

							<xsl:if test="contains(ns1:JourneyTiming/ns1:EndTime, '##**##')=false()">
								<xsl:value-of select="ns1:JourneyTiming/ns1:EndTime" />
							</xsl:if>
						</td>
					</tr>

					<xsl:variable name="splitRouteDescription">

						<xsl:if test="$UnitType='' or $UnitType=692001">
							<xsl:if test="contains(ns1:RouteDescription, '##**##')">
								<xsl:call-template name="tokenize">
									<xsl:with-param name="pText" select="substring-after(ns1:RouteDescription, '##**##')"></xsl:with-param>
									<xsl:with-param name="pDelim" select="Leg"></xsl:with-param>
								</xsl:call-template>
							</xsl:if>
							<xsl:if test="contains(ns1:RouteDescription, '##**##')=false()">
								<xsl:call-template name="tokenize">
									<xsl:with-param name="pText" select="ns1:RouteDescription"></xsl:with-param>
									<xsl:with-param name="pDelim" select="Leg"></xsl:with-param>
								</xsl:call-template>
							</xsl:if>
						</xsl:if>

						<xsl:if test="$UnitType=692002">
							<xsl:choose>
								<xsl:when test="contains(ns1:RouteParts/ns2:RoutePartListPosition/ns2:RouteImperial, '##**##')">
									<xsl:variable name="valueLength" select="string-length(substring-after(ns1:RouteParts/ns2:RoutePartListPosition/ns2:RouteImperial, '##**##'))-1"/>
									<xsl:call-template name="parseHtmlString">
										<xsl:with-param name="list" select="substring(substring-after(ns1:RouteParts/ns2:RoutePartListPosition/ns2:RouteImperial, '##**##'),1,$valueLength)"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:otherwise>
									<xsl:variable name="valueLength" select="string-length(ns1:RouteParts/ns2:RoutePartListPosition/ns2:RouteImperial)-1"/>
									<xsl:call-template name="parseHtmlString">
										<xsl:with-param name="list" select="substring(ns1:RouteParts/ns2:RoutePartListPosition/ns2:RouteImperial,1,$valueLength)"/>
									</xsl:call-template>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:if>

					</xsl:variable>

					<xsl:variable name="arrayRouteDescription" select="msxsl:node-set($splitRouteDescription)/option" />

					<tr>
						<td colspan="4" valign="top">
							<b>Route: </b>
							<br/>

							<xsl:for-each select="ns1:RouteParts/ns2:RoutePartListPosition">

								<xsl:variable name="getPosition" select="position() + 1" />

								<b>
									Leg <xsl:number/> :
								</b>
								<br/>
								<xsl:choose>
									<xsl:when test="contains(ns2:RoutePart/ns2:RoadPart/ns2:StartPointListPosition/ns2:StartPoint/ns5:Description, '##**##')">
										<xsl:value-of select="substring-after(ns2:RoutePart/ns2:RoadPart/ns2:StartPointListPosition/ns2:StartPoint/ns5:Description, '##**##')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="ns2:RoutePart/ns2:RoadPart/ns2:StartPointListPosition/ns2:StartPoint/ns5:Description"/>
									</xsl:otherwise>
								</xsl:choose>
								to <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
								<xsl:choose>
									<xsl:when test="contains(ns2:RoutePart/ns2:RoadPart/ns2:EndPointListPosition/ns2:EndPoint/ns5:Description, '##**##')">
										<xsl:value-of select="substring-after(ns2:RoutePart/ns2:RoadPart/ns2:EndPointListPosition/ns2:EndPoint/ns5:Description, '##**##')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="ns2:RoutePart/ns2:RoadPart/ns2:EndPointListPosition/ns2:EndPoint/ns5:Description"/>
									</xsl:otherwise>
								</xsl:choose>
								<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>:

								<xsl:value-of select="$arrayRouteDescription[$getPosition]" />

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


				<table border = "1" width = "100%">
					<tr>
						<td align="left" valign="top">
							<b>Notes supplied by haulier at time of notification: </b>

							<xsl:choose>
								<xsl:when test="ns1:NotificationNotesFromHaulier !=''">

									<xsl:if test="contains(ns1:NotificationNotesFromHaulier, '##**##')">
										<xsl:call-template name="newLineBySeperator">
											<xsl:with-param name="text" select="substring-after(ns1:NotificationNotesFromHaulier, '##**##')"/>
										</xsl:call-template>
									</xsl:if>

									<xsl:if test="contains(ns1:NotificationNotesFromHaulier, '##**##')=false()">
										<xsl:call-template name="newLineBySeperator">
											<xsl:with-param name="text" select="ns1:NotificationNotesFromHaulier"/>
										</xsl:call-template>
									</xsl:if>

								</xsl:when>
								<xsl:otherwise>
									There are no notes
								</xsl:otherwise>
							</xsl:choose>
						</td>
					</tr >
				</table >
				<table border = "0" width = "100%">
					<tr>
						<td>
							<b>Particulars of the load</b>
						</td>
					</tr>
				</table >
				<table border = "1" width = "100%">
					<tr>
						<td width="30%" valign="top">
							<b>Description of load</b>
						</td>
						<td colspan="2" width="70%" valign="top">
							<xsl:if test="contains(ns1:LoadDetails/ns2:Description, '##**##')">
								<xsl:value-of select="substring-after(ns1:LoadDetails/ns2:Description, '##**##')"/>
							</xsl:if>

							<xsl:if test="contains(ns1:LoadDetails/ns2:Description, '##**##')=false()">
								<xsl:value-of select="ns1:LoadDetails/ns2:Description" />
							</xsl:if>
						</td>
					</tr>
					<tr>
						<td width="30%" valign="top">
							<b>No. of movements</b>
						</td>
						<td colspan="2" width="70%" valign="top">
							<xsl:if test="contains(ns1:LoadDetails/ns2:TotalMoves, '##**##')">
								<xsl:value-of select="substring-after(ns1:LoadDetails/ns2:TotalMoves, '##**##')"/>
							</xsl:if>

							<xsl:if test="contains(ns1:LoadDetails/ns2:TotalMoves, '##**##')=false()">
								<xsl:value-of select="ns1:LoadDetails/ns2:TotalMoves" />
							</xsl:if>

						</td>
					</tr>
					<tr>
						<td width="30%" valign="top">
							<b>No. of pieces moved at one time</b>
						</td>
						<td colspan="2" width="70%" valign="top">
							<xsl:if test="contains(ns1:LoadDetails/ns2:MaxPiecesPerMove, '##**##')">
								<xsl:value-of select="substring-after(ns1:LoadDetails/ns2:MaxPiecesPerMove, '##**##')"/>
							</xsl:if>

							<xsl:if test="contains(ns1:LoadDetails/ns2:MaxPiecesPerMove, '##**##')=false()">
								<xsl:value-of select="ns1:LoadDetails/ns2:MaxPiecesPerMove" />
							</xsl:if>
						</td>
					</tr>
				</table >
				<!-- Code changes needs to be modified over here TODO-->
				<xsl:variable name="PlateNoString">
					<xsl:variable name="ConfigurationIdentityListPositionPlateNo" select="ns1:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:ConfigurationIdentityListPosition/ns3:ConfigurationIdentity/ns3:PlateNo" />
					<xsl:variable name="ConfigurationIdentityListPositionFleetNo" select="ns1:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:ConfigurationIdentityListPosition/ns3:ConfigurationIdentity/ns3:FleetNumber" />
					<xsl:for-each select="$ConfigurationIdentityListPositionPlateNo">
						<xsl:variable name="getPositionPlateNo" select="position()" />
						<xsl:choose>
							<xsl:when test=".!='' and  string-length(.) &gt; 0">
								<xsl:if test="generate-id() = generate-id($ConfigurationIdentityListPositionPlateNo[. = current()][1])">
									<xsl:if test="(last()=2 and position()=2)">
										<br></br>
									</xsl:if>
									<xsl:if test="position() > 1 and position() != last()"> or </xsl:if>
									<xsl:if test=". != ''">
										<xsl:choose>
											<xsl:when test="contains(., '##**##')">
												<xsl:value-of select="substring-after(., '##**##')"></xsl:value-of>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="."/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:if>
								</xsl:if>
							</xsl:when>
							<xsl:otherwise>
								<xsl:variable name="FleetNo" select="$ConfigurationIdentityListPositionFleetNo[$getPositionPlateNo]" />
								<xsl:choose>
									<xsl:when test="$FleetNo!='' and  string-length($FleetNo) &gt; 0">
										<xsl:value-of select="$FleetNo" />
									</xsl:when>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:for-each>
				</xsl:variable>
				<br></br>
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
							<xsl:value-of select="$PlateNoString"/>
						</td>
						<td  width="40%"  valign="top">
							<xsl:for-each select="ns1:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:VehicleSummaryListPosition">
								<xsl:if test="contains(ns3:VehicleSummary/ns3:ConfigurationType, '##**##')">
									<xsl:call-template name="CamelCase">
										<xsl:with-param name="text" select="substring-after(ns3:VehicleSummary/ns3:ConfigurationType, '##**##')"/>
									</xsl:call-template>
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
				</table>

				<xsl:variable name="ClassificationCategory" select="ns1:Classification"></xsl:variable>

				<xsl:for-each select="ns1:RouteParts/ns2:RoutePartListPosition">
					<!--TODO 1-->

					<!--TODO 1 ENd-->
					<!--Create string of variable-->
					<!--1-->
					<xsl:variable name="IncludingProjectionsString">
						<xsl:for-each select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:OverallLengthListPosition">
							<xsl:choose>
								<xsl:when test="contains(ns3:OverallLength/ns3:IncludingProjections,'##**##')">
									<xsl:if test="substring-after(ns3:OverallLength/ns3:IncludingProjections,'##**##') !=''">

										<xsl:if test="$UnitType='' or $UnitType=692001">
											or <xsl:value-of select="substring-after(ns3:OverallLength/ns3:IncludingProjections,'##**##')"/>
											<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
										</xsl:if>

										<xsl:if test="$UnitType=692002">
											<xsl:call-template name="ConvertToFeet">
												<xsl:with-param name="pText" select="substring-after(ns3:OverallLength/ns3:IncludingProjections,'##**##')"></xsl:with-param>
												<!--or <xsl:value-of select="ns3:OverallLength/ns3:IncludingProjections"/>-->
											</xsl:call-template>
										</xsl:if>

									</xsl:if >
								</xsl:when>
								<xsl:otherwise>
									<xsl:if test="ns3:OverallLength/ns3:IncludingProjections !=''">


										<xsl:if test="$UnitType='' or $UnitType=692001">
											or <xsl:value-of select="ns3:OverallLength/ns3:IncludingProjections"/>
											<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
										</xsl:if>

										<xsl:if test="$UnitType=692002">
											<xsl:call-template name="ConvertToFeet">

												<xsl:with-param name="pText" select="ns3:OverallLength/ns3:IncludingProjections"></xsl:with-param>
											</xsl:call-template>
										</xsl:if>

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
									<xsl:if test="substring-after(ns3:FrontOverhang,'##**##') !=''">

										<xsl:if test="$UnitType='' or $UnitType=692001">
											or <xsl:value-of select="substring-after(ns3:FrontOverhang,'##**##')"/>
											<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
										</xsl:if>

										<xsl:if test="$UnitType=692002">
											<xsl:call-template name="ConvertToFeet">
												<xsl:with-param name="pText" select="substring-after(ns3:FrontOverhang,'##**##')"></xsl:with-param>
											</xsl:call-template>
										</xsl:if>

									</xsl:if >
								</xsl:when>
								<xsl:otherwise>
									<xsl:if test="ns3:FrontOverhang !=''">

										<xsl:if test="$UnitType='' or $UnitType=692001">
											or <xsl:value-of select="ns3:FrontOverhang"/>
											<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
										</xsl:if>

										<xsl:if test="$UnitType=692002">
											<xsl:call-template name="ConvertToFeet">
												<xsl:with-param name="pText" select="ns3:FrontOverhang"></xsl:with-param>
											</xsl:call-template>
										</xsl:if>

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
									<xsl:if test="substring-after(ns3:RearOverhang,'##**##') !=''">

										<xsl:if test="$UnitType='' or $UnitType=692001">
											or <xsl:value-of select="substring-after(ns3:RearOverhang,'##**##')"/>
											<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
										</xsl:if>

										<xsl:if test="$UnitType=692002">
											<xsl:call-template name="ConvertToFeet">
												<xsl:with-param name="pText" select="substring-after(ns3:RearOverhang,'##**##')"></xsl:with-param>
											</xsl:call-template>
										</xsl:if>

									</xsl:if >
								</xsl:when>
								<xsl:otherwise>
									<xsl:if test="ns3:RearOverhang !=''">

										<xsl:if test="$UnitType='' or $UnitType=692001">
											or <xsl:value-of select="ns3:RearOverhang"/>
											<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
										</xsl:if>

										<xsl:if test="$UnitType=692002">
											<xsl:call-template name="ConvertToFeet">
												<xsl:with-param name="pText" select="ns3:RearOverhang"></xsl:with-param>
											</xsl:call-template>
										</xsl:if>

									</xsl:if >
								</xsl:otherwise>
							</xsl:choose>
						</xsl:for-each>
					</xsl:variable>
					<!--3 End-->
					<!--Left-->
					<xsl:variable name="LeftOverhangListPositionString">
						<xsl:for-each select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:LeftOverhangListPosition">
							<xsl:choose>
								<xsl:when test="contains(ns3:LeftOverhang,'##**##')">
									<xsl:if test="substring-after(ns3:LeftOverhang,'##**##') !=''">

										<xsl:if test="$UnitType='' or $UnitType=692001">
											or <xsl:value-of select="substring-after(ns3:LeftOverhang,'##**##')"/>
											<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
										</xsl:if>

										<xsl:if test="$UnitType=692002">
											<xsl:call-template name="ConvertToFeet">
												<xsl:with-param name="pText" select="substring-after(ns3:LeftOverhang,'##**##')"></xsl:with-param>
											</xsl:call-template>
										</xsl:if>

									</xsl:if >
								</xsl:when>
								<xsl:otherwise>
									<xsl:if test="ns3:LeftOverhang !=''">

										<xsl:if test="$UnitType='' or $UnitType=692001">
											or <xsl:value-of select="ns3:LeftOverhang"/>
											<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
										</xsl:if>

										<xsl:if test="$UnitType=692002">
											<xsl:call-template name="ConvertToFeet">
												<xsl:with-param name="pText" select="ns3:LeftOverhang"></xsl:with-param>
											</xsl:call-template>
										</xsl:if>


									</xsl:if >
								</xsl:otherwise>
							</xsl:choose>
						</xsl:for-each>
					</xsl:variable>
					<!--Left End-->
					<!--Right-->
					<xsl:variable name="RightOverhangListPositionString">
						<xsl:for-each select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:RightOverhangListPosition">
							<xsl:choose>
								<xsl:when test="contains(ns3:RightOverhang,'##**##')">
									<xsl:if test="substring-after(ns3:RightOverhang,'##**##') !=''">

										<xsl:if test="$UnitType='' or $UnitType=692001">
											or <xsl:value-of select="substring-after(ns3:RightOverhang,'##**##')"/>
											<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
										</xsl:if>

										<xsl:if test="$UnitType=692002">
											<xsl:call-template name="ConvertToFeet">
												<xsl:with-param name="pText" select="substring-after(ns3:RightOverhang,'##**##')"></xsl:with-param>
											</xsl:call-template>
										</xsl:if>

									</xsl:if >
								</xsl:when>
								<xsl:otherwise>
									<xsl:if test="ns3:RightOverhang !=''">

										<xsl:if test="$UnitType='' or $UnitType=692001">
											or <xsl:value-of select="ns3:RightOverhang"/>
											<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
										</xsl:if>

										<xsl:if test="$UnitType=692002">
											<xsl:call-template name="ConvertToFeet">
												<xsl:with-param name="pText" select="ns3:RightOverhang"></xsl:with-param>
											</xsl:call-template>
										</xsl:if>


									</xsl:if >
								</xsl:otherwise>
							</xsl:choose>
						</xsl:for-each>
					</xsl:variable>
					<!--Right End-->
					<!--4-->
					<xsl:variable name="RigidLengthListPositionString">
						<xsl:for-each select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:RigidLengthListPosition">
							<xsl:choose>
								<xsl:when test="contains(ns3:RigidLength,'##**##')">
									<xsl:if test="substring-after(ns3:RigidLength,'##**##') !=''">

										<xsl:if test="$UnitType='' or $UnitType=692001">
											or <xsl:value-of select="substring-after(ns3:RigidLength,'##**##')"/>
											<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
										</xsl:if>

										<xsl:if test="$UnitType=692002">
											<xsl:call-template name="ConvertToFeet">
												<xsl:with-param name="pText" select="substring-after(ns3:RigidLength,'##**##')"></xsl:with-param>
											</xsl:call-template>
										</xsl:if>

									</xsl:if >
								</xsl:when>
								<xsl:otherwise>
									<xsl:if test="ns3:RigidLength !=''">

										<xsl:if test="$UnitType='' or $UnitType=692001">
											or <xsl:value-of select="ns3:RigidLength"/>
											<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
										</xsl:if>

										<xsl:if test="$UnitType=692002">
											<xsl:call-template name="ConvertToFeet">
												<xsl:with-param name="pText" select="ns3:RigidLength"></xsl:with-param>
											</xsl:call-template>
										</xsl:if>

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
									<xsl:if test="substring-after(ns3:OverallWidth,'##**##') !=''">

										<xsl:if test="$UnitType='' or $UnitType=692001">
											or <xsl:value-of select="substring-after(ns3:OverallWidth,'##**##')"/>
											<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
										</xsl:if>

										<xsl:if test="$UnitType=692002">
											<xsl:call-template name="ConvertToFeet">
												<xsl:with-param name="pText" select="substring-after(ns3:OverallWidth,'##**##')"></xsl:with-param>
											</xsl:call-template>
										</xsl:if>

									</xsl:if >
								</xsl:when>
								<xsl:otherwise>
									<xsl:if test="ns3:OverallWidth !=''">

										<xsl:if test="$UnitType='' or $UnitType=692001">
											or <xsl:value-of select="ns3:OverallWidth"/>
											<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
										</xsl:if>

										<xsl:if test="$UnitType=692002">
											<xsl:call-template name="ConvertToFeet">
												<xsl:with-param name="pText" select="ns3:OverallWidth"></xsl:with-param>
											</xsl:call-template>
										</xsl:if>

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
									<xsl:if test="contains(ns3:OverallHeight/ns3:MaxHeight,'##**##') !=''">

										<xsl:if test="$UnitType='' or $UnitType=692001">
											or <xsl:value-of select="substring-after(ns3:OverallHeight/ns3:MaxHeight,'##**##')"/>
											<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
										</xsl:if>

										<xsl:if test="$UnitType=692002">
											<xsl:call-template name="ConvertToFeet">
												<xsl:with-param name="pText" select="substring-after(ns3:OverallHeight/ns3:MaxHeight,'##**##')"></xsl:with-param>
											</xsl:call-template>
										</xsl:if>

									</xsl:if >
								</xsl:when>
								<xsl:otherwise>
									<xsl:if test="ns3:OverallHeight/ns3:MaxHeight !=''">

										<xsl:if test="$UnitType='' or $UnitType=692001">
											or <xsl:value-of select="ns3:OverallHeight/ns3:MaxHeight"/>
											<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
										</xsl:if>

										<xsl:if test="$UnitType=692002">
											<xsl:call-template name="ConvertToFeet">
												<xsl:with-param name="pText" select="ns3:OverallHeight/ns3:MaxHeight"></xsl:with-param>
											</xsl:call-template>
										</xsl:if>

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
									<xsl:if test="contains(ns3:OverallHeight/ns3:ReducibleHeight,'##**##') !=''">
										or <xsl:value-of select="substring-after(ns3:OverallHeight/ns3:ReducibleHeight, '##**##')"/>
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
									<xsl:if test="substring-after(ns3:GrossWeight/ns3:Weight,'##**##') !=''">
										or <xsl:value-of select="substring-after(ns3:GrossWeight/ns3:Weight,'##**##')"/>
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
					<!--8 End-->
					<!--9-->
					<xsl:variable name="GrossTrainWeightListPositionString">
						<xsl:for-each select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:GrossTrainWeightListPosition">
							<xsl:choose>
								<xsl:when test="contains(ns3:GrossTrainWeight,'##**##')">
									<xsl:if test="substring-after(ns3:GrossTrainWeight,'##**##') !=''">
										or <xsl:value-of select="substring-after(ns3:GrossTrainWeight,'##**##')"/>
										<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>kg
									</xsl:if >
								</xsl:when>
								<xsl:otherwise>
									<xsl:if test="ns3:GrossTrainWeight !=''">
										or <xsl:value-of select="ns3:GrossTrainWeight"/>
										<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>kg
									</xsl:if >
								</xsl:otherwise>
							</xsl:choose>
						</xsl:for-each>
					</xsl:variable>
					<!--9 End-->
					<!--Create string of variable end-->
					<!--TODO 2-->
					<xsl:for-each select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:OverallLengthListPosition">
						<xsl:if test="position() = 1">
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
										<b>Projection - left</b>
									</td>
									<td valign="top">
										<b>Projection - right</b>
									</td>
									<td valign="top">
										<b>Rigid length</b>
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
									<xsl:choose>
										<xsl:when test="substring-after($GrossTrainWeightListPositionString,'or ') !=''">
											<td valign="top">
												<b>Gross train weight</b>
											</td>
										</xsl:when>
									</xsl:choose>
								</tr>
								<tr>
									<td valign="top">
										<!--TODO 1-->
										<xsl:choose>
											<xsl:when test="substring-after($IncludingProjectionsString,'or ') !=''">
												<xsl:value-of select="substring-after($IncludingProjectionsString,'or ')"/>
											</xsl:when>
											<xsl:otherwise>-</xsl:otherwise>
										</xsl:choose>
									</td>
									<td valign="top">
										<!--TODO 2-->
										<xsl:choose>
											<xsl:when test="substring-after($FrontOverhangListPositionString,'or ') !=''">
												<xsl:value-of select="substring-after($FrontOverhangListPositionString,'or ')"/>
											</xsl:when>
											<xsl:otherwise>-</xsl:otherwise>
										</xsl:choose>
									</td>
									<td valign="top">
										<!--TODO 3-->
										<xsl:choose>
											<xsl:when test="substring-after($RearOverhangListPositionString,'or ') !=''">
												<xsl:value-of select="substring-after($RearOverhangListPositionString,'or ')"/>
											</xsl:when>
											<xsl:otherwise>-</xsl:otherwise>
										</xsl:choose>
									</td>
									<td valign="top">
										<!--TODO Left-->
										<xsl:choose>
											<xsl:when test="substring-after($LeftOverhangListPositionString,'or ') !=''">
												<xsl:value-of select="substring-after($LeftOverhangListPositionString,'or ')"/>
											</xsl:when>
											<xsl:otherwise>-</xsl:otherwise>
										</xsl:choose>
									</td>
									<td valign="top">
										<!--TODO Right-->
										<xsl:choose>
											<xsl:when test="substring-after($RightOverhangListPositionString,'or ') !=''">
												<xsl:value-of select="substring-after($RightOverhangListPositionString,'or ')"/>
											</xsl:when>
											<xsl:otherwise>-</xsl:otherwise>
										</xsl:choose>
									</td>
									<td valign="top">
										<!--TODO 4-->
										<xsl:choose>
											<xsl:when test="substring-after($RigidLengthListPositionString,'or ') !=''">
												<xsl:value-of select="substring-after($RigidLengthListPositionString,'or ')"/>
											</xsl:when>
											<xsl:otherwise>-</xsl:otherwise>
										</xsl:choose>
									</td>
									<td  valign="top">
										<!--TODO 5-->
										<xsl:choose>
											<xsl:when test="substring-after($OverallWidthListPositionString,'or ') !=''">
												<xsl:value-of select="substring-after($OverallWidthListPositionString,'or ')"/>
											</xsl:when>
											<xsl:otherwise>-</xsl:otherwise>
										</xsl:choose>
									</td>
									<td  valign="top">
										<!--TODO 6-->
										<xsl:choose>
											<xsl:when test="substring-after($OverallHeightListPositionString,'or ') !=''">
												<xsl:value-of select="substring-after($OverallHeightListPositionString,'or ')"/>
											</xsl:when>
											<xsl:otherwise>-</xsl:otherwise>
										</xsl:choose>
									</td>
									<td valign="top">
										<!--TODO 7-->
										<xsl:choose>
											<xsl:when test="substring-after($OverallHeightListPositionReducibleHeightString,'or ') !=''">
												<xsl:value-of select="substring-after($OverallHeightListPositionReducibleHeightString,'or ')"/>
											</xsl:when>
											<xsl:otherwise>-</xsl:otherwise>
										</xsl:choose>
									</td>
									<td  valign="top">
										<!--TODO 8-->
										<xsl:choose>
											<xsl:when test="substring-after($GrossWeightListPositionString,'or ') !=''">
												<xsl:value-of select="substring-after($GrossWeightListPositionString,'or ')"/>
											</xsl:when>
											<xsl:otherwise>-</xsl:otherwise>
										</xsl:choose>
									</td>
									<xsl:choose>
										<xsl:when test="substring-after($GrossTrainWeightListPositionString,'or ') !=''">
											<td  valign="top">
												<!--TODO 9-->
												<xsl:value-of select="substring-after($GrossTrainWeightListPositionString,'or ')"/>
											</td>
										</xsl:when>
									</xsl:choose>
								</tr>
							</table>
						</xsl:if >
					</xsl:for-each>
					<!--TODO 2 ENd2-->
				</xsl:for-each>

				<xsl:for-each select="ns1:StructureDetails/ns1:div/ns1:table/ns1:tbody/ns1:tr">
					<b>
						<xsl:value-of select="ns1:th"/>
					</b>
					<p>
						<xsl:value-of select="ns1:td" />
					</p>
				</xsl:for-each>
			</body>
		</html>
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

	<xsl:template name="CamelCaseWord">
		<xsl:param name="text"/>
		<xsl:value-of select="translate(substring($text,1,1),'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
		<xsl:value-of select="translate(substring($text,2,string-length($text)-1),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')" />
	</xsl:template>

	<xsl:variable name="whitespace" select="'&#09;&#10;&#13; '" />

	<!-- Strips trailing whitespace characters from 'string' -->
	<xsl:template name="string-rtrim">
		<xsl:param name="string" />
		<xsl:param name="trim" select="$whitespace" />

		<xsl:variable name="length" select="string-length($string)" />

		<xsl:if test="$length &gt; 0">
			<xsl:choose>
				<xsl:when test="contains($trim, substring($string, $length, 1))">
					<xsl:call-template name="string-rtrim">
						<xsl:with-param name="string" select="substring($string, 1, $length - 1)" />
						<xsl:with-param name="trim"   select="$trim" />
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$string" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<!-- Strips leading whitespace characters from 'string' -->
	<xsl:template name="string-ltrim">
		<xsl:param name="string" />
		<xsl:param name="trim" select="$whitespace" />

		<xsl:if test="string-length($string) &gt; 0">
			<xsl:choose>
				<xsl:when test="contains($trim, substring($string, 1, 1))">
					<xsl:call-template name="string-ltrim">
						<xsl:with-param name="string" select="substring($string, 2)" />
						<xsl:with-param name="trim"   select="$trim" />
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$string" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<!-- Strips leading and trailing whitespace characters from 'string' -->
	<xsl:template name="string-trim">
		<xsl:param name="string" />
		<xsl:param name="trim" select="$whitespace" />
		<xsl:call-template name="string-rtrim">
			<xsl:with-param name="string">
				<xsl:call-template name="string-ltrim">
					<xsl:with-param name="string" select="$string" />
					<xsl:with-param name="trim"   select="$trim" />
				</xsl:call-template>
			</xsl:with-param>
			<xsl:with-param name="trim"   select="$trim" />
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="string-replace-all">
		<xsl:param name="text" />
		<xsl:param name="replace" />
		<xsl:param name="by" />
		<xsl:choose>
			<xsl:when test="contains($text, $replace)">
				<xsl:value-of select="substring-before($text,$replace)" />
				<xsl:value-of select="$by" />
				<xsl:call-template name="string-replace-all">
					<xsl:with-param name="text"
					select="substring-after($text,$replace)" />
					<xsl:with-param name="replace" select="$replace" />
					<xsl:with-param name="by" select="$by" />
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$text" />
			</xsl:otherwise>
		</xsl:choose>
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

	<xsl:template match="text()" name="ConvertToFeetNoOR">
		<xsl:param name="pText" select="."/>
		<!--<xsl:if test="string-length($pText) > 0">-->

		<xsl:choose>

			<xsl:when test="number($pText) != 0">
				<xsl:variable name="metreInches" select="number($pText) * number(39.370078740157477)"/>
				<xsl:variable name="needRoundOff" select="number($metreInches) mod 1"/>

				<xsl:choose>
					<xsl:when test="$needRoundOff &gt;= 0.99">
						<xsl:variable name="needRoundOffValue"  select="ceiling(number($metreInches))"/>

						<xsl:variable name="Feet" select="number($needRoundOffValue) div 12"/>
						<xsl:variable name="Inches" select="floor(number($needRoundOffValue)) mod 12"/>
						<!--<xsl:variable name="Inches" select="floor(number($InchesValue))"/>-->

						<xsl:value-of select="floor($Feet)"/>' <xsl:value-of select="$Inches"/>" ft
					</xsl:when>
					<xsl:otherwise>
						<xsl:variable name="Feet" select="number($metreInches) div 12"/>
						<xsl:variable name="Inches" select="floor(number($metreInches)) mod 12"/>
						<!--<xsl:variable name="Inches" select="floor(number($InchesValue))"/>-->
						<xsl:value-of select="floor($Feet)"/>' <xsl:value-of select="$Inches"/>" ft
					</xsl:otherwise>
				</xsl:choose>

				<!--or <xsl:value-of select="$needRoundOff"/>needRoundOff-->

			</xsl:when>

			<xsl:otherwise>
				<xsl:value-of select="0"/> ft
			</xsl:otherwise>

		</xsl:choose>

	</xsl:template>

	<xsl:template match="text()" name="ConvertToFeet">
		<xsl:param name="pText" select="."/>
		<!--<xsl:if test="string-length($pText) > 0">-->

		<xsl:choose>

			<xsl:when test="number($pText) != 0">
				<xsl:variable name="metreInches" select="number($pText) * number(39.370078740157477)"/>
				<xsl:variable name="needRoundOff" select="number($metreInches) mod 1"/>

				<xsl:choose>
					<xsl:when test="$needRoundOff &gt;= 0.99">
						<xsl:variable name="needRoundOffValue"  select="ceiling(number($metreInches))"/>

						<xsl:variable name="Feet" select="number($needRoundOffValue) div 12"/>
						<xsl:variable name="Inches" select="floor(number($needRoundOffValue)) mod 12"/>
						<!--<xsl:variable name="Inches" select="floor(number($InchesValue))"/>-->

						or <xsl:value-of select="floor($Feet)"/>' <xsl:value-of select="$Inches"/>" ft
					</xsl:when>
					<xsl:otherwise>
						<xsl:variable name="Feet" select="number($metreInches) div 12"/>
						<xsl:variable name="Inches" select="floor(number($metreInches)) mod 12"/>
						<!--<xsl:variable name="Inches" select="floor(number($InchesValue))"/>-->
						or <xsl:value-of select="floor($Feet)"/>' <xsl:value-of select="$Inches"/>" ft
					</xsl:otherwise>
				</xsl:choose>

			</xsl:when>

			<xsl:otherwise>
				or <xsl:value-of select="0"/> ft
			</xsl:otherwise>

		</xsl:choose>

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
