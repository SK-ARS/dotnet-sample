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
xmlns:ns5="http://www.esdal.com/schemas/core/route"
 xmlns:ns6="http://www.esdal.com/schemas/core/routeanalysis" >
	
	<xsl:param name="Contact_ID"></xsl:param>
	<xsl:param name="DocType"></xsl:param>
	<xsl:param name="OrganisationName"></xsl:param>
	<xsl:param name="HAReferenceNumber"></xsl:param>
	<xsl:param name="UnitType"></xsl:param>
	<xsl:param name="OrgId"></xsl:param>
	<xsl:param name="RoutePartId"></xsl:param>
	<xsl:param name="ContactPhoneNumber"></xsl:param>
	<xsl:param name="ContactEmail"></xsl:param>
	
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
			<body style="font-family:Arial;font-size:10.5px;">

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

				<br/>

				 <table width = "100%" cellspacing ="0" cellpadding ="0">
          <tr>
            <td colspan="4" align="left">
              <b><!--FACSIMILE MESSAGE--></b>
            </td>
          </tr>
          <tr>
            <td valign="bottom">
              <b>
                ESDAL reference:
              </b>
            </td>
            <td valign="top" colspan="3" align="left">
              <xsl:choose>
                <xsl:when test="contains(ns2:ESDALReferenceNumber/ns2:ESDALReferenceNo, '##**##')">
                  <xsl:value-of select="substring-after(ns2:ESDALReferenceNumber/ns2:ESDALReferenceNo, '##**##')"/>
                </xsl:when>
                <xsl:when test="ns2:ESDALReferenceNumber/ns2:ESDALReferenceNo != ''">
                  <xsl:value-of select="ns2:ESDALReferenceNumber/ns2:ESDALReferenceNo"/>
                </xsl:when>
                <xsl:when test="ns2:ESDALReferenceNumber/ns2:Mnemonic != '' and ns2:ESDALReferenceNumber/ns2:MovementProjectNumber != ''">
                  <xsl:value-of select="ns2:ESDALReferenceNumber/ns2:Mnemonic" />/<xsl:value-of select="ns2:ESDALReferenceNumber/ns2:MovementProjectNumber" />/S<xsl:value-of select=" ns2:ESDALReferenceNumber/ns2:MovementVersion" />#<xsl:value-of select="ns2:ESDALReferenceNumber/ns2:NotificationNumber" />
                </xsl:when>
                <xsl:when test="ns2:ESDALReferenceNumber/ns2:Mnemonic != '' and ns2:ESDALReferenceNumber/ns2:MovementProjectNumber != ''
                    and ns2:ESDALReferenceNumber/ns2:MovementVersion != '' and ns2:ESDALReferenceNumber/ns2:NotificationNumber!='' and ns2:ESDALReferenceNumber/ns2:MovementVersion > 1">
                  <xsl:value-of select="ns2:ESDALReferenceNumber/ns2:Mnemonic" />/<xsl:value-of select="ns2:ESDALReferenceNumber/ns2:MovementProjectNumber" />/<xsl:value-of select=" ns2:ESDALReferenceNumber/ns2:MovementVersion" />#<xsl:value-of select="ns2:ESDALReferenceNumber/ns2:NotificationNumber" />(<xsl:value-of select=" ns2:ESDALReferenceNumber/ns2:MovementVersion" />)
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="ns2:ESDALReferenceNumber/ns2:Mnemonic"/>/<xsl:value-of select="ns2:ESDALReferenceNumber/ns2:MovementProjectNumber"/>/<xsl:value-of select="ns2:ESDALReferenceNumber/ns2:MovementVersion"/>#<xsl:value-of select="ns2:ESDALReferenceNumber/ns2:NotificationNumber"/>
                </xsl:otherwise>
              </xsl:choose>
            </td>
          </tr>
          <tr>
            <td valign="bottom">
              <b>Notification of movement:</b>
            </td>
            <td valign="top" colspan="3" align="left">
              <xsl:if test="ns1:JourneyFromToSummary/ns2:From != '' and  ns1:JourneyFromToSummary/ns2:To!=''">
                <xsl:if test="contains(ns1:JourneyFromToSummary/ns2:From, '##**##')">
                  <xsl:value-of select="substring-after(ns1:JourneyFromToSummary/ns2:From, '##**##')"/>
                </xsl:if>

                <xsl:if test="contains(ns1:JourneyFromToSummary/ns2:From, '##**##')=false()">
                  <xsl:value-of select="ns1:JourneyFromToSummary/ns2:From" />
                </xsl:if>
                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>to<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                <xsl:if test="contains(ns1:JourneyFromToSummary/ns2:To, '##**##')">
                  <xsl:value-of select="substring-after(ns1:JourneyFromToSummary/ns2:To, '##**##')"/>
                </xsl:if>

                <xsl:if test="contains(ns1:JourneyFromToSummary/ns2:To, '##**##')=false()">
                  <xsl:value-of select="ns1:JourneyFromToSummary/ns2:To" />
                </xsl:if>
              </xsl:if>
            </td>
          </tr>
          <tr>
            <td>
              <b>Date sent:</b>
            </td>
            <td colspan="3" align="left">
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
          </tr>
          <xsl:if test="$DocType = 'PDF'">
            <tr>
              <td>
                <b>No of pages:</b>
              </td>
              <td colspan="3">###Noofpages###</td>
            </tr>
          </xsl:if>
          <tr>
            <td>
              <b>NH reference:</b>
            </td>
            <td colspan="3" align="left">
              <xsl:value-of select="$HAReferenceNumber"/>
            </td>
          </tr>
          <tr>
            <td>
              <b>Classification:</b>
            </td>
            <td colspan="3" align="left">

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
                    <xsl:choose>
                      <xsl:when test="contains(ns1:DftReference, '##**##')">
                        <xsl:value-of select="substring-after(ns1:DftReference, '##**##')"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="ns1:DftReference"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
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
<xsl:for-each select="ns1:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart">
					<table border = "0" width = "100%">
						<tr>
							<th class="headgrad">
								AFFECTED STRUCTURE (<xsl:apply-templates select="ns2:Name/text()"/>)
							</th>
						</tr>
					</table >
					<br/>
					
					<!-- Code changes needs to be modified over here TODO-->
					<table border="1" width = "100%" >
						<tr>
							<td valign="top">
								<b>Structure Code</b>
							</td>
							<td valign="top">
								<b>Structure Name</b>
							</td>
							<td valign="top">
								<b>Structure Class</b>
							</td>
							<td valign="top">
								<b>Organisation Name</b>
							</td>
							
							<td valign="top">
								<b>Contact Name</b>
							</td>
							<td valign="top">
								<b>Email</b>
							</td>
						<td valign="top">
								<b>Phone Number</b>
							</td>
						</tr>
						<xsl:for-each select="ns2:RoadPart/ns2:Structures/ns2:Structure">


							<xsl:if test="ns2:Appraisal/ns6:Organisation=$OrganisationName">
								<tr>
									<td>
										
									<xsl:choose>
											<xsl:when test="contains(ns2:ESRN, '##**##')">
												<xsl:value-of select="substring-after(ns2:ESRN, '##**##')"/>
											</xsl:when>										
											<xsl:otherwise>
												<xsl:value-of select="ns2:ESRN/text()"/>
											</xsl:otherwise>
										</xsl:choose>	
									</td>
									<td>
										<xsl:choose>
											<xsl:when test="contains(ns2:Name, '##**##')">
												<xsl:value-of select="substring-after(ns2:Name, '##**##')"/>
											</xsl:when>										
											<xsl:otherwise>
												<xsl:value-of select="ns2:Name/text()"/>
											</xsl:otherwise>
										</xsl:choose>										
									</td>
									<td>
										
									
									  <xsl:choose>
                      <xsl:when test="./@TraversalType='overbridge'">
                        <span>Overbridge</span>
                      </xsl:when>
                      <xsl:otherwise>
                        <span>Underbridge</span>
                      </xsl:otherwise>
                    </xsl:choose>
									</td>
									<td>
										<xsl:value-of select="ns2:Appraisal/ns6:Organisation/text()"/>
									</td>

									

										
												<xsl:for-each select="//ns1:Recipients/ns2:Contact">
										<xsl:if test="ns2:OrganisationName=$OrganisationName">
												<td>
												<xsl:value-of select="ns2:ContactName/text()"/>
															</td>
										<td>
											<xsl:value-of select="$ContactEmail"/>
											<xsl:variable name="structure-contactid" select="./@ContactId"/>

										</td>
											
											</xsl:if>
									</xsl:for-each>
									
											

										<td>
										<xsl:value-of select="$ContactPhoneNumber"/>
											</td>


								</tr>
							</xsl:if>



						</xsl:for-each>
					</table>
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
