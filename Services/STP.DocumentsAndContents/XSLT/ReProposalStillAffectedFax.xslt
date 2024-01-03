<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="2.0"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:xs="http://www.w3.org/2001/XMLSchema"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ns1="http://www.esdal.com/schemas/core/proposedroute"
    xmlns:ns2="http://www.esdal.com/schemas/core/movement"
    xmlns:ns3="http://www.esdal.com/schemas/core/vehicle"
    xmlns:ns4="http://www.esdal.com/schemas/core/esdalcommontypes"
    xmlns:ns5="http://www.esdal.com/schemas/core/route"
    xmlns:ns6="http://www.esdal.com/schemas/core/drivinginstruction"
    xmlns:ns7="http://www.esdal.com/schemas/core/formattedtext"
    xmlns:ns8="http://www.govtalk.gov.uk/people/bs7666"
    xmlns:ns9="http://www.esdal.com/schemas/core/annotation"
    xmlns:ns10="http://www.esdal.com/schemas/core/caution"
    xmlns:ns11="http://www.esdal.com/schemas/common/movementversion"
    xmlns:ns12="http://www.esdal.com/schemas/core/contact">

  <xsl:param name="Contact_ID"></xsl:param>
  <xsl:param name="UnitType"></xsl:param>
  <xsl:param name="DocType"></xsl:param>

  <xsl:template match="/ns1:Proposal">

    <xsl:variable name="CountRouteSubList">
      <xsl:choose>
        <xsl:when test="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns6:DrivingInstructions/ns6:SubPartListPosition/ns6:SubPart/ns6:AlternativeListPosition/ns6:Alternative/ns6:AlternativeDescription/ns6:AlternativeNo != ''
                  and ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns6:DrivingInstructions/ns6:SubPartListPosition/ns6:SubPart/ns6:AlternativeListPosition/ns6:Alternative/ns6:AlternativeDescription/ns6:AlternativeNo &gt; 0">
          <xsl:for-each select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Roads/ns2:RouteSubPartListPosition">
            <xsl:number/>
          </xsl:for-each>
        </xsl:when>
        <xsl:otherwise>1</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <html>
      <head>
        <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta2/dist/css/bootstrap.min.css" rel="stylesheet"
           integrity="sha384-BmbxuPwQa2lc/FVzBcNJ7UAyJxM6wuqIj61tLrc4wSX0szH/Ev+nYRRuWlolflfl" crossorigin="anonymous" />
        <link rel="preconnect" href="https://fonts.gstatic.com" />
        <link href="https://fonts.googleapis.com/css2?family=Roboto+Condensed" rel="stylesheet" />
        <style>
          body {
          width: 230mm;
          height: 100%;
          margin: 0 auto;
          padding: 0;
          font-size: 12pt;
          background: rgb(204, 204, 204);
          }

          * {
          box-sizing: border-box;
          -moz-box-sizing: border-box;
          }

          @font-face {
          font-family: lato_light;
          src: url(assets/Lato-Light.ttf);
          }

          @font-face {
          font-family: lato_medium;
          src: url(assets/Lato-Medium.ttf);
          }

          @font-face {
          font-family: lato_bold;
          src: url(assets/Lato-Bold.ttf);
          }

          @font-face {
          font-family: lato_regular;
          src: url(assets/Lato-Regular.ttf);
          }

          @font-face {
          font-family: lato_semibold;
          src: url(assets/Lato-Semibold.ttf);
          }

          .main-page {
          width: 210mm;
          min-height: 297mm;
          margin: 10mm auto;
          background: white;
          box-shadow: 0 0 0.5cm rgba(0, 0, 0, 0.5);
          }

          .sub-page {
          padding: 1cm;
          height: 297mm;
          }

          .page-margin{
          padding: 1cm;
          }

          .text-head {
          color: black;
          font-size: 35px;
          font-family: lato_medium, Arial;
          font-style: normal;
          font-weight: 400;
          }

          .text-highlight {
          letter-spacing: 0.00px;
          color: black;
          font-size: 15.00px;
          font-weight: 700;
          font-style: normal;
          font-family: lato_medium, Arial;
          }

          .text-normal-hyperlink {
          line-height: 35.00px;
          letter-spacing: 0.00px;
          color: rgba(39, 87, 149, 1);
          -webkit-text-stroke: unset;
          font-size: 15.00px;
          font-weight: 500;
          font-style: normal;
          font-family: lato_medium, Arial;
          }

          .text-normal,
          p,
          li {
          line-height: 22.00px;
          letter-spacing: 0.00px;
          color: black;
          -webkit-text-stroke: unset;
          font-size: 14.00px;
          font-weight: 500;
          font-style: normal;
          font-family: lato_medium, Arial;
          text-align: justify;
          }

          ol {
          padding-left: 1rem !important;
          }

          ul {
          padding-left: 0.5rem !important;
          list-style-type: "*";
          }

          p {
          margin: 0px !important;
          padding: 0px !important;
          }

          .text-title {
          color: black;
          font-size: 25px;
          font-family: lato_light, Arial;
          font-style: normal;
          font-weight: 100;
          }

          .text-normal-light {
          color: black;
          font-size: 16px;
          font-family: lato_light, Arial;
          font-style: normal;
          font-weight: 300;
          }

          .text-normal-link {
          color: rgba(39, 87, 149, 1);
          font-size: 16px;
          font-family: lato_medium, Arial;
          font-style: normal;
          font-weight: 100;
          }

          .notes-highlight {
          background-color: rgba(39, 87, 149, 0.1);
          padding: 1.5rem 4rem;
          margin-left: 4rem;
          }

          .notes-highlight1 {
          background-color: rgba(39, 87, 149, 0.1);
          padding: 1.5rem 4rem;
          }

          .wd1 {
          flex: 0 0 auto;
          width: 16.6666666667%;
          }

          .wd1 {
          flex: 0 0 auto;
          width: 16.6666666667%;
          }

          .wd2 {
          flex: 0 0 auto;
          width: 50%;
          }

          .wd3 {
          flex: 0 0 auto;
          width: 83.3333333333%;
          }

          .wd4 {
          flex: 0 0 auto;
          width: 41.6666666667%;
          }

          .wd5 {
          flex: 0 0 auto;
          width: 58.3333333333%;
          }

          .wd6 {
          flex: 0 0 auto;
          width: 8.3333333333%;
          }

          .wd7 {
          flex: 0 0 auto;
          width: 33.3333333333%;
          }

          .wd8 {
          flex: 0 0 auto;
          width: 50%;
          }

          .wd9 {
          flex: 0 0 auto;
          width: 50%;
          }

          hr {
          margin: 0.2rem 0rem;
          }

          .pl-3 {
          padding-left: 1.5rem;
          }


          @page {
          size: A4;
          margin: 0;
          }

          @media print {

          html,
          body {
          width: 210mm;
          height: 297mm;
          -webkit-print-color-adjust: exact;
          }

          .main-page {
          margin: 0;
          border: initial;
          border-radius: initial;
          width: initial;
          min-height: initial;
          box-shadow: initial;
          background: initial;
          page-break-after: always;
          }
          }
        </style>


      </head>
      <body style="font-family:Arial;font-size:13px;background-color:white">

        <table width = "100%" cellspacing ="0" cellpadding ="0">
          <tr>
            <td>
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
              <br/>
            </td>
          </tr>
        </table>
        <br></br>
        <div class="text-normal-light pb-3" style="padding-left: 1cm;">
          <!--FACSIMILE MESSAGE-->
        </div>
        <div class="notes-highlight1">
          <span class="text-head">Amendment to provisional route</span>
          <br></br>
          <p class="text-normal-light">
            Classification: <span class="text-normal-link">special order</span>
          </p>
        </div>
        <br></br>
       
        <table style ="left-align;width:100%" cellspacing ="0" cellpadding ="0">
          
          <tr>
            <td colspan="16">
              <br></br>
              <B>Note</B>
            </td>
          </tr>
          <tr>
            <td colspan="16">
              Items that have been removed are shown with a line through them e.g. <strike>This line has been removed </strike>
              Items that have been added are shown in <b>bold</b> and <b>
                <u>underlined</u>
              </b> e.g. <b>
                <u>This line has been added</u>
              </b>
            </td>
          </tr>
        </table>
        <br />
       

        <table style ="left-align;width:100%;">

          <tr>
            <td align ="left" style="width:200px" valign="top">
              <b>Date sent:</b>
            </td>
            <td align ="left" style="width:700px;" valign="top">
              <table style="width:100%" >
                <tr>
                  <td>
                    <xsl:if test="contains(ns2:SentDateTime, '##**##')">
                      <xsl:element name="newdate">
                        <xsl:call-template name="FormatDate">
                          <xsl:with-param name="DateTime" select="substring-after(ns2:SentDateTime, '##**##')"/>
                          <xsl:with-param name="DateTime1" select="substring-after(ns2:SentDateTime, '##**##')"/>
                        </xsl:call-template>
                      </xsl:element>
                    </xsl:if>
                    <xsl:if test="contains(ns2:SentDateTime, '##**##')=false()">
                      <xsl:element name="newdate">
                        <xsl:call-template name="FormatDate">
                          <xsl:with-param name="DateTime" select="ns2:SentDateTime"/>
                          <xsl:with-param name="DateTime1" select="ns2:SentDateTime"/>
                        </xsl:call-template>
                      </xsl:element>
                    </xsl:if>
                  </td>
                </tr >
              </table >

            </td>
            <td border ="0" style="width:1px;">
            </td>

          </tr>
          <xsl:if test="$DocType = 'PDF'">
            <tr>
              <td align ="left" style="width:200px;" valign="top">
                <b>No of pages:</b>
              </td>
              <td align ="left" style="width:600px;" valign="top">
                ###Noofpages###
              </td>
              <td border ="0" style="width:1px;">
              </td>
            </tr>
          </xsl:if>
          <tr>
            <td align ="left" style="width:200px;" valign="top">
              <b>AIL CONTACT:</b>
            </td>
            <td align ="left" style="width:600px;" valign="top" colspan="2">

              <xsl:if test="ns2:HAContact/ns2:Contact != ''">
                <div>
                  <trim>
                    <xsl:call-template name="string-trim">
                      <xsl:with-param name="string" select="ns2:HAContact/ns2:Contact" />
                    </xsl:call-template>
                  </trim>
                </div>
              </xsl:if>

              <xsl:for-each select="ns2:HAContact/ns2:Address/ns4:Line">
                <xsl:if test=". != ''">
                  <div>
                    <xsl:value-of select="."/>
                  </div>
                </xsl:if>
              </xsl:for-each>
              <xsl:if test="ns2:HAContact/ns2:Address/ns4:PostCode != ''">
                <div>
                  <xsl:value-of select="ns2:HAContact/ns2:Address/ns4:PostCode" />
                </div>
              </xsl:if>
              <xsl:if test="ns2:HAContact/ns2:Address/ns4:Country != ''">
                <div>
                  <xsl:if test="contains(ns2:HAContact/ns2:Address/ns4:Country, '##**##')">
                    <xsl:call-template name="Capitalize">
                      <xsl:with-param name="word" select="substring-before(substring-before(concat(ns2:HAContact/ns2:Address/ns4:Country, ' '), ' '), '##**##')"/>
                    </xsl:call-template>
                    <xsl:call-template name="Capitalize">
                      <xsl:with-param name="word" select="substring-after(substring-before(concat(ns2:HAContact/ns2:Address/ns4:Country, ' '), ' '), '##**##')"/>
                    </xsl:call-template>
                  </xsl:if>

                  <xsl:if test="contains(ns2:HAContact/ns2:Address/ns4:Country, '##**##')=false()">
                    <xsl:call-template name="Capitalize">
                      <xsl:with-param name="word" select="substring-before(concat(ns2:HAContact/ns2:Address/ns4:Country, ' '), ' ')"/>
                    </xsl:call-template>

                  </xsl:if>
                </div>
              </xsl:if>
              <xsl:if test="ns2:HAContact/ns2:TelephoneNumber != ''">
                <div>
                  Direct tel:<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text> <xsl:value-of select="ns2:HAContact/ns2:TelephoneNumber" />
                </div>
              </xsl:if>
              <xsl:if test="ns2:HAContact/ns2:FaxNumber != ''">
                <div>
                  Fax number:<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:value-of select="ns2:HAContact/ns2:FaxNumber" />
                </div>
              </xsl:if>
              <xsl:if test="ns2:HAContact/ns2:EmailAddress != ''">
                <div>
                  E-mail address:<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:value-of select="ns2:HAContact/ns2:EmailAddress" />
                </div>
              </xsl:if>
            </td>
          </tr>
          <tr>
            <td align ="left" style="width:200px;" valign="top">
               <p>
          <b>To:</b>
        </p>
            </td>
            <td align ="left" style="width:800px;" valign="top">
               <xsl:for-each select="ns2:Recipients/ns2:Contact">
          <div>
            <xsl:if test="ns2:ContactName!=''">
              <xsl:choose>
                <xsl:when test="@Reason='newly affected' or @Reason='affected by change of route'">
                  <xsl:if test="contains(ns2:ContactName, '##**##')">
                    <strike>
                      <xsl:value-of select="substring-before(ns2:ContactName, '##**##')" />, <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                      <trim>
                        <xsl:call-template name="string-trim">
                          <xsl:with-param name="string" select="substring-before(ns2:OrganisationName, '##**##')" />
                        </xsl:call-template>
                      </trim>
                    </strike>
                    <u>
                      <b>
                        <xsl:value-of select="substring-after(ns2:ContactName, '##**##')" />, <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                        <trim>
                          <xsl:call-template name="string-trim">
                            <xsl:with-param name="string" select="substring-after(ns2:OrganisationName, '##**##')" />
                          </xsl:call-template>
                        </trim>
                      </b>
                    </u>
                  </xsl:if>
                  <xsl:if test="contains(ns2:ContactName, '##**##')=false()">
                    <!--start code change-->
                    <xsl:if test="@Reason='newly affected'">
                      <u>
                        <b>
                          <xsl:value-of select="ns2:ContactName" />, <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                          <trim>
                            <xsl:call-template name="string-trim">
                              <xsl:with-param name="string" select="ns2:OrganisationName" />
                            </xsl:call-template>
                          </trim>
                        </b>
                      </u>
                    </xsl:if>
                    <xsl:if test="@Reason='affected by change of route'">
                      <xsl:choose>
                        <xsl:when test="@ContactId = $Contact_ID">
                          <b>
                            <xsl:value-of select="ns2:ContactName" />, <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            <trim>
                              <xsl:call-template name="string-trim">
                                <xsl:with-param name="string" select="ns2:OrganisationName" />
                              </xsl:call-template>
                            </trim>
                          </b>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="ns2:ContactName" />, <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                          <trim>
                            <xsl:call-template name="string-trim">
                              <xsl:with-param name="string" select="ns2:OrganisationName" />
                            </xsl:call-template>
                          </trim>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:if>
                    <!--End code change-->


                  </xsl:if>
                </xsl:when>
                <!--start code change-->
                <xsl:when test="contains(@Reason, '##**##')">

                  <xsl:if test="substring-after(@Reason, '##**##')='no longer affected'">
                    <xsl:choose>
                      <!--new-->
                      <xsl:when test="contains(ns2:ContactName, '##**##')">
                        <strike>
                          <xsl:value-of select="substring-before(ns2:ContactName, '##**##')" />, <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                          <trim>
                            <xsl:call-template name="string-trim">
                              <xsl:with-param name="string" select="substring-before(ns2:OrganisationName, '##**##')" />
                            </xsl:call-template>
                          </trim>
                        </strike>
                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                        <u>
                          <b>
                            <xsl:value-of select="substring-after(ns2:ContactName, '##**##')" />, <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            <trim>
                              <xsl:call-template name="string-trim">
                                <xsl:with-param name="string" select="substring-after(ns2:OrganisationName, '##**##')" />
                              </xsl:call-template>
                            </trim>
                          </b>
                        </u>
                      </xsl:when>
                      <!--new-->
                      <xsl:otherwise>
                        <strike>
                          <xsl:value-of select="ns2:ContactName" />, <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                          <trim>
                            <xsl:call-template name="string-trim">
                              <xsl:with-param name="string" select="ns2:OrganisationName" />
                            </xsl:call-template>
                          </trim>
                        </strike>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:if>
                  <xsl:if test="substring-after(@Reason, '##**##')='affected by change of route' or substring-after(@Reason, '##**##')='still affected'">
                    <xsl:choose>
                      <!--new-->
                      <xsl:when test="contains(ns2:ContactName, '##**##')">
                        <strike>
                          <xsl:value-of select="substring-before(ns2:ContactName, '##**##')" />, <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                          <trim>
                            <xsl:call-template name="string-trim">
                              <xsl:with-param name="string" select="substring-before(ns2:OrganisationName, '##**##')" />
                            </xsl:call-template>
                          </trim>
                        </strike>
                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                        <u>
                          <b>
                            <xsl:value-of select="substring-after(ns2:ContactName, '##**##')" />, <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            <trim>
                              <xsl:call-template name="string-trim">
                                <xsl:with-param name="string" select="substring-after(ns2:OrganisationName, '##**##')" />
                              </xsl:call-template>
                            </trim>
                          </b>
                        </u>
                      </xsl:when>
                      <!--new-->
                      <xsl:when test="@ContactId = $Contact_ID">
                        <b>
                          <xsl:value-of select="ns2:ContactName" />, <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                          <trim>
                            <xsl:call-template name="string-trim">
                              <xsl:with-param name="string" select="ns2:OrganisationName" />
                            </xsl:call-template>
                          </trim>
                        </b>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="ns2:ContactName" />, <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                        <trim>
                          <xsl:call-template name="string-trim">
                            <xsl:with-param name="string" select="ns2:OrganisationName" />
                          </xsl:call-template>
                        </trim>
                      </xsl:otherwise>
                    </xsl:choose>

                  </xsl:if>
                </xsl:when>
                <!--End code change-->


                <xsl:when test="@Reason='no longer affected'">
                  <strike>
                    <xsl:value-of select="ns2:ContactName" />, <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                    <trim>
                      <xsl:call-template name="string-trim">
                        <xsl:with-param name="string" select="ns2:OrganisationName" />
                      </xsl:call-template>
                    </trim>
                  </strike>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:if test="@ContactId = $Contact_ID">
                    <b>
                      <xsl:call-template name="parseString">
                        <xsl:with-param name="list" select="ns2:ContactName"/>
                      </xsl:call-template>, <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                      <xsl:call-template name="parseString">
                        <xsl:with-param name="list" select="ns2:OrganisationName"/>
                      </xsl:call-template>
                    </b>
                  </xsl:if>
                  <xsl:if test="@ContactId != $Contact_ID">
                    <xsl:call-template name="parseString">
                      <xsl:with-param name="list" select="ns2:ContactName"/>
                    </xsl:call-template>, <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                    <xsl:call-template name="parseString">
                      <xsl:with-param name="list" select="ns2:OrganisationName"/>
                    </xsl:call-template>
                  </xsl:if>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:if>
          </div>
        </xsl:for-each>

            </td>
          </tr>
        </table>

       

       
        <xsl:if test="ns2:DistributionComments != ''">
          <br/>
          <table align="centre"  >
            <tr>
              <td width ="200"></td>
              <td border="1" width ="500">
				  <xsl:choose>
					  <xsl:when test="contains(ns2:DistributionComments, '##**##')">
						  <xsl:value-of select="substring-after(ns2:DistributionComments, '##**##')"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="ns2:DistributionComments" />
					  </xsl:otherwise>
				  </xsl:choose>
              </td>
              <td width ="200"></td>
            </tr>
          </table>
        </xsl:if>
        <br/>

        <p>
          <table style ="left-align;width:100%" >
            <tr>
              <td align ="left" style="width:200px" valign="top">
                <b>
                  Route summary for

                  <xsl:choose>
                    <xsl:when test="ns2:ESDALReferenceNumber/ns2:Mnemonic != '' and ns2:ESDALReferenceNumber/ns2:Mnemonic != 'SORT' and ns2:ESDALReferenceNumber/ns2:MovementProjectNumber!='' and ns2:ESDALReferenceNumber/ns2:MovementVersion!='' ">
                      <xsl:value-of select="ns2:ESDALReferenceNumber/ns2:Mnemonic"/>/<xsl:value-of select="ns2:ESDALReferenceNumber/ns2:MovementProjectNumber"/>/S<xsl:if test="contains(ns2:ESDALReferenceNumber/ns2:MovementVersion, '##**##')">
                        <xsl:value-of select="substring-after(ns2:ESDALReferenceNumber/ns2:MovementVersion, '##**##')"/>
                      </xsl:if><xsl:if test="contains(ns2:ESDALReferenceNumber/ns2:MovementVersion, '##**##')=false()">
                        <xsl:value-of select="ns2:ESDALReferenceNumber/ns2:MovementVersion"/>
                      </xsl:if>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="ns2:ESDALReferenceNumber/ns2:Mnemonic"/>/S<xsl:value-of select="ns2:ESDALReferenceNumber/ns2:MovementProjectNumber"/>/S<xsl:if test="contains(ns2:ESDALReferenceNumber/ns2:MovementVersion, '##**##')">
                        <xsl:value-of select="substring-after(ns2:ESDALReferenceNumber/ns2:MovementVersion, '##**##')"/>
                      </xsl:if><xsl:if test="contains(ns2:ESDALReferenceNumber/ns2:MovementVersion, '##**##')=false()">
                        <xsl:value-of select="ns2:ESDALReferenceNumber/ns2:MovementVersion"/>
                      </xsl:if>
                    </xsl:otherwise>
                  </xsl:choose>

                  <xsl:if test="ns2:JourneyFromToSummary/ns2:From != '' and  ns2:JourneyFromToSummary/ns2:To!=''">
                    <xsl:choose>
                      <xsl:when test="ns2:ESDALReferenceNumber/ns2:Mnemonic != '' and  ns2:ESDALReferenceNumber/ns2:MovementProjectNumber!='' and ns2:ESDALReferenceNumber/ns2:MovementVersion!='' ">
                        <xsl:if test="contains(ns2:JourneyFromToSummary/ns2:From, '##**##') and contains(ns2:JourneyFromToSummary/ns2:To, '##**##')">
                          <strike>
                            - <xsl:value-of select="substring-before(ns2:JourneyFromToSummary/ns2:From, '##**##')"/>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>to <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            <xsl:value-of select="substring-before(ns2:JourneyFromToSummary/ns2:To, '##**##')" />
                          </strike>
                          <b>
                            <u>
                              <xsl:value-of select="substring-after(ns2:JourneyFromToSummary/ns2:From, '##**##')"/>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>to <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                              <xsl:value-of select="substring-after(ns2:JourneyFromToSummary/ns2:To, '##**##')" />
                            </u>
                          </b>
                        </xsl:if>
                        <xsl:if test="contains(ns2:JourneyFromToSummary/ns2:From, '##**##')=false() and contains(ns2:JourneyFromToSummary/ns2:To, '##**##')=false()">
                          -  <xsl:value-of select="ns2:JourneyFromToSummary/ns2:From" /> <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>to <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text> <xsl:value-of select="ns2:JourneyFromToSummary/ns2:To" />
                        </xsl:if>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:if test="contains(ns2:JourneyFromToSummary/ns2:From, '##**##') and contains(ns2:JourneyFromToSummary/ns2:To, '##**##')">
                          <strike>
                            <xsl:value-of select="substring-before(ns2:JourneyFromToSummary/ns2:From, '##**##')"/>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>to <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                            <xsl:value-of select="substring-before(ns2:JourneyFromToSummary/ns2:To, '##**##')" />
                          </strike>
                          <b>
                            <u>
                              <xsl:value-of select="substring-after(ns2:JourneyFromToSummary/ns2:From, '##**##')"/>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>to <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                              <xsl:value-of select="substring-after(ns2:JourneyFromToSummary/ns2:To, '##**##')" />
                            </u>
                          </b>
                        </xsl:if>
                        <xsl:if test="contains(ns2:JourneyFromToSummary/ns2:From, '##**##')=false() and contains(ns2:JourneyFromToSummary/ns2:To, '##**##')=false()">
                          <xsl:value-of select="ns2:JourneyFromToSummary/ns2:From" /> <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>to <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text> <xsl:value-of select="ns2:JourneyFromToSummary/ns2:To" />
                        </xsl:if>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:if>
                </b>
              </td>
            </tr>
          </table>
        </p>
        <br/>
        <p>
          <table style ="left-align;width:100%" >
            <tr>
              <td align ="left" style="width:200px" valign="top">
                <b>Provisional route for movement of </b>
                <xsl:call-template name="parseString">
                  <xsl:with-param name="list" select="ns2:LoadDetails/ns2:Description"/>
                </xsl:call-template>
              </td>
            </tr>
          </table >
        </p>
        <table  style ="left-align;width:100%">
          <tr>
            <td align ="left" width ="100" valign="top">
              <table style="width:100%" >
                <tr>
                  <td>
                    <b>
                      ESDAL  reference:
                    </b>
                  </td>
                </tr>
              </table>
            </td>
            <td>
              <table style="width:100%">
                <tr>
                  <td>
                    <xsl:choose>
                      <xsl:when test="ns2:ESDALReferenceNumber/ns2:Mnemonic != '' and ns2:ESDALReferenceNumber/ns2:Mnemonic != 'SORT' and ns2:ESDALReferenceNumber/ns2:MovementProjectNumber!='' and ns2:ESDALReferenceNumber/ns2:MovementVersion!='' ">
                        <xsl:value-of select="ns2:ESDALReferenceNumber/ns2:Mnemonic"/>/<xsl:value-of select="ns2:ESDALReferenceNumber/ns2:MovementProjectNumber"/>/S<xsl:if test="contains(ns2:ESDALReferenceNumber/ns2:MovementVersion, '##**##')">
                          <xsl:value-of select="substring-after(ns2:ESDALReferenceNumber/ns2:MovementVersion, '##**##')"/>
                        </xsl:if><xsl:if test="contains(ns2:ESDALReferenceNumber/ns2:MovementVersion, '##**##')=false()">
                          <xsl:value-of select="ns2:ESDALReferenceNumber/ns2:MovementVersion"/>
                        </xsl:if>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="ns2:ESDALReferenceNumber/ns2:Mnemonic"/>/S<xsl:value-of select="ns2:ESDALReferenceNumber/ns2:MovementProjectNumber"/>/S<xsl:if test="contains(ns2:ESDALReferenceNumber/ns2:MovementVersion, '##**##')">
                          <xsl:value-of select="substring-after(ns2:ESDALReferenceNumber/ns2:MovementVersion, '##**##')"/>
                        </xsl:if><xsl:if test="contains(ns2:ESDALReferenceNumber/ns2:MovementVersion, '##**##')=false()">
                          <xsl:value-of select="ns2:ESDALReferenceNumber/ns2:MovementVersion"/>
                        </xsl:if>
                      </xsl:otherwise>
                    </xsl:choose>
                  </td>
                </tr>
              </table>
            </td>
            <td border ="0">
            </td>
          </tr>
          <!--<tr>
            <td align ="left" width ="100" valign="top">
              <table style="width:100%" >
                <tr>
                  <td>
                    <b>
                      HA reference:
                    </b>
                  </td>
                </tr>
              </table>
            </td>
            <td  align ="left" width ="60">
              <xsl:call-template name="parseString">
                <xsl:with-param name="list" select="ns2:JobFileReference"/>
              </xsl:call-template>
            </td>
            <td border ="0">
            </td>
          </tr>-->
          <xsl:if test="ns2:HauliersReference != ''">
            <tr>
              <td align ="left" width ="100" valign="top">
                <b>Hauliers Reference:</b>
              </td>
              <td align ="left"  width ="60" >
                <xsl:if test="contains(ns2:HauliersReference, '##**##')">
                  <xsl:value-of select="substring-after(ns2:HauliersReference, '##**##')"/>
                </xsl:if>
                <xsl:if test="contains(ns2:HauliersReference, '##**##')=false()">
                  <xsl:value-of select="ns2:HauliersReference"/>
                </xsl:if>
              </td>
              <td border ="0">
              </td>
            </tr>
          </xsl:if>
          <tr>
            <td align ="left" width ="100" valign="top">

              <b>
                From:
              </b>

            </td>
            <td align ="left"  width ="60" >

              <xsl:call-template name="parseString">
                <xsl:with-param name="list" select="ns2:JourneyFromTo/ns2:From"/>
              </xsl:call-template>

            </td>
            <td border ="0">
            </td>
          </tr>
          <tr>
            <td align ="left" width ="100" valign="top">

              <b>
                To:
              </b>

            </td>
            <td align ="left" width ="60">

              <xsl:call-template name="parseString">
                <xsl:with-param name="list" select="ns2:JourneyFromTo/ns2:To"/>
              </xsl:call-template>

            </td>
            <td border ="0">
            </td>
          </tr>
          <tr>
            <td align ="left" width ="100" valign="top">

              <b>
                Haulier:
              </b>

            </td>
            <td align ="left"  width ="60" >

              <xsl:call-template name="parseString">
                <xsl:with-param name="list" select="ns2:HaulierDetails/ns2:HaulierName"/>
              </xsl:call-template>

            </td>
            <td border ="0">
            </td>
          </tr>
          <tr>
            <td align ="left" width ="100" valign="top">
              <b>Haulier contact details:</b>
            </td>
            <td  align ="left" width ="60">
              <xsl:if test="ns2:HaulierDetails/ns2:HaulierContact != ''">
                <div>
                  <xsl:value-of select="ns2:HaulierDetails/ns2:HaulierContact" />
                </div>
              </xsl:if>
              <xsl:for-each select="ns2:HaulierDetails/ns2:HaulierAddress/ns4:Line">
                <xsl:if test=". != ''">
                  <div>
                    <xsl:value-of select="."/>
                  </div>
                </xsl:if>
              </xsl:for-each>
              <xsl:if test="ns2:HaulierDetails/ns2:HaulierAddress/ns4:PostCode != ''">
                <div>
                  <xsl:value-of select="ns2:HaulierDetails/ns2:HaulierAddress/ns4:PostCode" />
                </div>
              </xsl:if>
              <xsl:if test="ns2:HaulierDetails/ns2:HaulierAddress/ns4:Country != ''">
                <div>
                  <xsl:call-template name="Capitalize">
                    <xsl:with-param name="word" select="substring-before(concat(ns2:HaulierDetails/ns2:HaulierAddress/ns4:Country, ' '), ' ')"/>
                  </xsl:call-template>
                </div>
              </xsl:if>
            </td>
            <td border ="0">
            </td>
          </tr>
          <tr>
            <td align ="left" width ="100" valign="top">
              <b>
                Telephone number:
              </b>
            </td>
            <td align ="left" width ="60"  >
              <xsl:call-template name="parseString">
                <xsl:with-param name="list" select="ns2:HaulierDetails/ns2:TelephoneNumber"/>
              </xsl:call-template>
            </td>
            <td border ="0">
            </td>
          </tr>
        </table>
        <br/>
        <p>
          <b>
            Approximate date of first movement:
          </b>
          <xsl:if test="ns2:OldJourneyTiming/ns2:JourneyTiming/ns2:FirstMoveDate != '' and ns2:OldJourneyTiming/ns2:JourneyTiming/ns2:LastMoveDate!=''">
            <strike>
              <xsl:element name="newdate">
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="ns2:OldJourneyTiming/ns2:JourneyTiming/ns2:FirstMoveDate"/>
                </xsl:call-template>
              </xsl:element>
              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>to <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
              <xsl:element name="newdate">
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="ns2:OldJourneyTiming/ns2:JourneyTiming/ns2:LastMoveDate"/>
                </xsl:call-template>
              </xsl:element>
            </strike>
            <xsl:if test="ns2:JourneyTiming/ns2:FirstMoveDate != '' and ns2:JourneyTiming/ns2:LastMoveDate!=''">
              <u>
                <b>
                  <xsl:element name="newdate">
                    <xsl:call-template name="FormatDate">
                      <xsl:with-param name="DateTime" select="ns2:JourneyTiming/ns2:FirstMoveDate"/>
                    </xsl:call-template>
                  </xsl:element>
                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>to <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                  <xsl:element name="newdate">
                    <xsl:call-template name="FormatDate">
                      <xsl:with-param name="DateTime" select="ns2:JourneyTiming/ns2:LastMoveDate"/>
                    </xsl:call-template>
                  </xsl:element>
                </b>
              </u>
            </xsl:if>
          </xsl:if>

          <xsl:if test="ns2:JourneyTiming/ns2:FirstMoveDate != '' and ns2:JourneyTiming/ns2:LastMoveDate!='' and not(ns2:OldJourneyTiming/ns2:JourneyTiming/ns2:FirstMoveDate) and not(ns2:OldJourneyTiming/ns2:JourneyTiming/ns2:LastMoveDate)">
            <xsl:if test="contains(ns2:JourneyTiming/ns2:FirstMoveDate, '##**##') and contains(ns2:JourneyTiming/ns2:LastMoveDate, '##**##')">
              <strike>
                <xsl:element name="newdate">
                  <xsl:call-template name="FormatDate">
                    <xsl:with-param name="DateTime" select="substring-before(ns2:JourneyTiming/ns2:FirstMoveDate, '##**##')"/>
                  </xsl:call-template>
                </xsl:element>
                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>to <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                <xsl:element name="newdate">
                  <xsl:call-template name="FormatDate">
                    <xsl:with-param name="DateTime" select="substring-before(ns2:JourneyTiming/ns2:LastMoveDate, '##**##')"/>
                  </xsl:call-template>
                </xsl:element>
              </strike>
              <b>
                <u>
                  <xsl:element name="newdate">
                    <xsl:call-template name="FormatDate">
                      <xsl:with-param name="DateTime" select="substring-after(ns2:JourneyTiming/ns2:FirstMoveDate, '##**##')"/>
                    </xsl:call-template>
                  </xsl:element>
                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>to <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                  <xsl:element name="newdate">
                    <xsl:call-template name="FormatDate">
                      <xsl:with-param name="DateTime" select="substring-after(ns2:JourneyTiming/ns2:LastMoveDate, '##**##')"/>
                    </xsl:call-template>
                  </xsl:element>
                </u>
              </b>
            </xsl:if>

            <xsl:if test="contains(ns2:JourneyTiming/ns2:FirstMoveDate, '##**##')=false() and contains(ns2:JourneyTiming/ns2:LastMoveDate, '##**##')=false()">
              <xsl:element name="newdate">
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="ns2:JourneyTiming/ns2:FirstMoveDate"/>
                </xsl:call-template>
              </xsl:element>
              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>to <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
              <xsl:element name="newdate">
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="ns2:JourneyTiming/ns2:LastMoveDate"/>
                </xsl:call-template>
              </xsl:element>
            </xsl:if>

          </xsl:if>
        </p>
        <br/>
        <p >
          <table>
            <!--<tr>
              <td valign="top">1.</td>
              <td colspan="30">The Highways Agency (HA) has been requested to supply a route for the above mentioned movement(s).</td>
            </tr>-->
            <tr>
              <td valign="top">1.</td>
              <td colspan="30">
                <u>All highway, bridge and agent authorities should assess the adequacy of the route for the vehicle or the vehicle combination at the scheduled date of movement and confirm their acceptance / rejection via the Esdal2 on-line collaboration facility or by email.</u>
              </td>
            </tr>
            <tr>
              <td valign="top">2.</td>
              <td colspan="30">
                The assessment should take account of the capacity of the bridge structures, the strength of the
                carriageway, and headroom and ground clearances. The assessment should also take into account the
                possibility of damage to statutory undertakers' apparatus, which may be present under the road surface
                with shallow cover.
              </td>
            </tr>
            <tr>
              <td valign="top">3.</td>
              <td colspan="30">
                Would Network Rail also please provide the following information regarding level crossings and overhead wires.
              </td>
            </tr>
          </table>
          <table>
            <tr>
              <td valign="top"  align="right">
                i.
              </td>
              <td colspan="15">
                confirmation of the location of automatic half-barrier crossings indicated on the route and any
                additional locations.
              </td>
            </tr>
            <tr>
              <td valign="top"  align="right">
                ii.
              </td >
              <td colspan="15">
                the location of any other type of public or private road level crossings where special safety
                precautions and arrangements would be required.
              </td>
            </tr>
            <tr>
              <td valign="top"  align="right">
                iii.
              </td>
              <td colspan="15">
                locations of those level crossings where the track level could affect the road profile.
              </td>
            </tr>
            <tr>
              <td valign="top"  align="right">
                iv.
              </td>
              <td colspan="15">
                the safe clearance under any overhead Network Rail electrified wires crossing the route and details
                of any special arrangements that should be made.
              </td>
            </tr>
            <tr>
              <td valign="top"  align="right">
                v.
              </td>
              <td colspan="15">
                where circumstances require, the designation and telephone number of the local Railway Officer
                whom the haulier should contact before the passage of the vehicle over the crossing.
              </td>
            </tr>
          </table>

          <table>
            <tr>
              <td valign="top">4.</td>
              <td colspan="30">In those cases where the manoeuvrability is in doubt, arrangements will be made for a joint route survey by the haulier with the authority concerned.</td>
            </tr>
            <tr>
              <td valign="top">5.</td>
              <td colspan="30">Copies of all relevant observations from the authorities consulted for the route (and of any assessments involved) and any other comments you may have should be forwarded without delay.</td>
            </tr>
            <tr>
              <td valign="top">6.</td>
              <td colspan="30">(In cases where the date of movement is well in advance, your early observations are required to enable the manufacturer to know whether or not a route is available before the actual manufacture commences).</td>
            </tr>
          </table>
        </p>
        <br />
        <p>
          <b>
            <span class="text-title pb-3">
              ROUTE OVERVIEW
            </span> <br/>
            Number of movements:
            <xsl:if test="contains(ns2:LoadDetails/ns2:TotalMoves, '##**##')">
              <xsl:value-of select="substring-after(ns2:LoadDetails/ns2:TotalMoves, '##**##')"/>
            </xsl:if>
            <xsl:if test="contains(ns2:LoadDetails/ns2:TotalMoves, '##**##')=false()">
              <xsl:value-of select="ns2:LoadDetails/ns2:TotalMoves"/>
            </xsl:if>
            <br/>Number of pieces moved at one time:
            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
            <xsl:if test="contains(ns2:LoadDetails/ns2:MaxPiecesPerMove, '##**##')">
              <xsl:value-of select="substring-after(ns2:LoadDetails/ns2:MaxPiecesPerMove, '##**##')"/>
            </xsl:if>
            <xsl:if test="contains(ns2:LoadDetails/ns2:MaxPiecesPerMove, '##**##')=false()">
              <xsl:value-of select="ns2:LoadDetails/ns2:MaxPiecesPerMove"/>
            </xsl:if>
          </b>
        </p>

        <table  border="0" cellspacing="5"  cellpadding="5">
          <tr>
            <th colspan="2">
              <b>
                Leg
              </b>
            </th>
            <th colspan="3">
              <b>
                Route
              </b>
            </th>
            <th colspan="3">
              <b>
                Distance
              </b>

            </th>
          </tr>
          <tr>
            <td    colspan="8" style="border-top: 1px solid black">
            </td>
          </tr>
          <xsl:for-each select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart">
            <tr>
              <td colspan="2">
                <xsl:call-template name="parseString">
                  <xsl:with-param name="list" select="ns2:LegNumber"/>
                </xsl:call-template>
              </td>

              <td colspan="3">
                <xsl:call-template name="parseString">
                  <xsl:with-param name="list" select="ns2:Name"/>
                </xsl:call-template>
              </td>
              <td colspan="3">
                <xsl:if test="ns2:RoadPart/ns2:Distance/ns2:Imperial/ns2:Distance != ''">
                  <xsl:choose>
                    <xsl:when test="contains(ns2:RoadPart/ns2:Distance/ns2:Imperial/ns2:Distance, '##**##')">
                      <strike>
                        <xsl:if test="$UnitType=692001">
                          <xsl:variable name="var12" select="substring-before(ns2:RoadPart/ns2:Distance/ns2:Metric/ns2:Distance, '##**##')"/>
                          <xsl:variable name="var13" select="round(number($var12) div number(1000))"/>
                          <xsl:value-of select="$var13"/> <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                        </xsl:if>
                        <xsl:if test="$UnitType=692002">
                          <xsl:variable name="var12" select="substring-before(ns2:RoadPart/ns2:Distance/ns2:Imperial/ns2:Distance, '##**##')"/>
                          <xsl:variable name="var13" select="round(number($var12) div number(1760))"/>
                          <xsl:value-of select="$var13"/> <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                        </xsl:if>

                      </strike>
                      <b>
                        <u>
                          <xsl:if test="$UnitType=692001">
                            <xsl:variable name="var12" select="substring-after(ns2:RoadPart/ns2:Distance/ns2:Metric/ns2:Distance, '##**##')"/>
                            <xsl:variable name="var13" select="round(number($var12) div number(1000))"/>
                            <xsl:value-of select="$var13"/> <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                          </xsl:if>
                          <xsl:if test="$UnitType=692002">
                            <xsl:variable name="var12" select="substring-after(ns2:RoadPart/ns2:Distance/ns2:Imperial/ns2:Distance, '##**##')"/>
                            <xsl:variable name="var13" select="round(number($var12) div number(1760))"/>
                            <xsl:value-of select="$var13"/> <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                          </xsl:if>
                        </u>
                      </b>
                    </xsl:when>
                    <xsl:when test="ns2:RoadPart/ns2:OldDistance/ns2:Distance/ns2:Metric/ns2:Distance">
                      <xsl:if test="$UnitType=692001">

                        <strike>
                          <xsl:variable name="var12" select="ns2:RoadPart/ns2:OldDistance/ns2:Distance/ns2:Metric/ns2:Distance"/>
                          <xsl:variable name="var13" select="round(number($var12) div number(1000))"/>
                          <xsl:value-of select="$var13"/> <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                        </strike>
                        <u>
                          <b>
                            <xsl:variable name="var12" select="ns2:RoadPart/ns2:Distance/ns2:Metric/ns2:Distance"/>
                            <xsl:variable name="var13" select="round(number($var12) div number(1000))"/>
                            <xsl:value-of select="$var13"/> <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                          </b>
                        </u>
                      </xsl:if>

                      <xsl:if test="$UnitType=692002">
                        <strike>
                          <xsl:variable name="var12" select="ns2:RoadPart/ns2:OldDistance/ns2:Distance/ns2:Imperial/ns2:Distance"/>
                          <xsl:variable name="var13" select="round(number($var12) div number(1760))"/>
                          <xsl:value-of select="$var13"/> <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                        </strike>
                        <u>
                          <b>
                            <xsl:variable name="var12" select="ns2:RoadPart/ns2:Distance/ns2:Imperial/ns2:Distance"/>
                            <xsl:variable name="var13" select="round(number($var12) div number(1760))"/>
                            <xsl:value-of select="$var13"/> <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                          </b>
                        </u>
                      </xsl:if>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:if test="$UnitType=692001">
                        <xsl:if test="contains(ns2:RoadPart/ns2:Distance/ns2:Metric/ns2:Distance, '##**##')=false()">
                          <xsl:variable name="var12" select="ns2:RoadPart/ns2:Distance/ns2:Metric/ns2:Distance"/>
                          <xsl:variable name="var13" select="round(number($var12) div number(1000))"/>
                          <xsl:value-of select="$var13"/> <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                        </xsl:if>
                      </xsl:if>
                      <xsl:if test="$UnitType=692002">
                        <xsl:if test="contains(ns2:RoadPart/ns2:Distance/ns2:Imperial/ns2:Distance, '##**##')=false()">
                          <xsl:variable name="var12" select="ns2:RoadPart/ns2:Distance/ns2:Imperial/ns2:Distance"/>
                          <xsl:variable name="var13" select="round(number($var12) div number(1760))"/>
                          <xsl:value-of select="$var13"/> <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                        </xsl:if>
                      </xsl:if>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:if>
              </td>
            </tr>
            <tr>
              <td    colspan="8" style="border-top: 1px solid black">
              </td>
            </tr>
          </xsl:for-each>
        </table>

        <br/>

        <p>
          <xsl:for-each select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart">
            <b>
              LEG
              <xsl:value-of select="ns2:LegNumber" /><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>-

              <xsl:choose>
                <xsl:when test="contains(ns2:RoadPart/ns2:StartPointListPosition/ns2:StartPoint/ns5:Description, '##**##')">
                  <strike>
                    <xsl:value-of select="substring-before(ns2:RoadPart/ns2:StartPointListPosition/ns2:StartPoint/ns5:Description, '##**##')"/>
                  </strike>
                  <b>
                    <u>
                      <xsl:value-of select="substring-after(ns2:RoadPart/ns2:StartPointListPosition/ns2:StartPoint/ns5:Description, '##**##')"/>
                    </u>
                  </b>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="ns2:RoadPart/ns2:StartPointListPosition/ns2:StartPoint/ns5:Description"/>
                </xsl:otherwise>
              </xsl:choose>
              to <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
              <xsl:choose>
                <xsl:when test="contains(ns2:RoadPart/ns2:EndPointListPosition/ns2:EndPoint/ns5:Description, '##**##')">
                  <strike>
                    <xsl:value-of select="substring-before(ns2:RoadPart/ns2:EndPointListPosition/ns2:EndPoint/ns5:Description, '##**##')"/>
                  </strike>
                  <b>
                    <u>
                      <xsl:value-of select="substring-after(ns2:RoadPart/ns2:EndPointListPosition/ns2:EndPoint/ns5:Description, '##**##')"/>
                    </u>
                  </b>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="ns2:RoadPart/ns2:EndPointListPosition/ns2:EndPoint/ns5:Description"/>
                </xsl:otherwise>
              </xsl:choose>
            </b>
            <br/>
            <br/>

            <table style ="left-align" border ="0" cellspacing="5"  cellpadding="5">
              <tr>
                <th>
                  <b>Road</b>
                </th>
                <th>
                  <b>
                    Distance
                  </b>
                </th>
              </tr>
              <tr>
                <td    colspan="3" style="border-top: 1px solid black">
                </td>
              </tr>
              <xsl:variable name="CountDrivingInstruction">
                <xsl:choose>
                  <xsl:when test="ns2:RoadPart/ns6:DrivingInstructions/ns6:SubPartListPosition/ns6:SubPart/ns6:AlternativeListPosition/ns6:Alternative/ns6:AlternativeDescription/ns6:AlternativeNo != ''
                  and ns2:RoadPart/ns6:DrivingInstructions/ns6:SubPartListPosition/ns6:SubPart/ns6:AlternativeListPosition/ns6:Alternative/ns6:AlternativeDescription/ns6:AlternativeNo &gt; 0">
                    <xsl:for-each select="ns2:RoadPart/ns6:DrivingInstructions/ns6:SubPartListPosition/ns6:SubPart/ns6:AlternativeListPosition">
                      <xsl:choose>
                        <xsl:when test="ns6:Alternative/ns6:AlternativeDescription/ns6:AlternativeNo != ''">
                          <item>
                            <xsl:value-of select="ns6:Alternative/ns6:AlternativeDescription/ns6:AlternativeNo"/>
                          </item>
                        </xsl:when>
                        <xsl:otherwise>
                          <item>0</item>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:for-each>
                  </xsl:when>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="array" select="msxsl:node-set($CountDrivingInstruction)/item" />

              <xsl:for-each select="ns2:RoadPart/ns2:Roads/ns2:RouteSubPartListPosition/ns2:RouteSubPart/ns2:PathListPosition/ns2:Path">

                <xsl:variable name="getPosition" select="position()" />

                <xsl:if test="$CountRouteSubList &gt; 1">

                  <xsl:if test="position() &gt; 1 and position() = last()">
                    <xsl:choose>
                      <xsl:when test="$array[$getPosition] &gt; 0">
                        <xsl:choose>
                          <xsl:when test="$DocType = 'PDF' or $DocType = 'MovementPDF'">
                            <xsl:if test="$array[$getPosition] &gt; 0">
                              <tr>
                                <td colspan="3" align="center">
                                  <span>
                                    <b>
                                      Alternative #<xsl:value-of select="$array[$getPosition]" />
                                    </b>
                                  </span>
                                </td>
                              </tr>
                              <tr>
                                <td    colspan="3" style="border-top: 1px solid black">
                                </td>
                              </tr>
                            </xsl:if>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:if test="$array[$getPosition] &gt; 0">
                              <tr>
                                <td colspan="2" align="center">
                                  <span>
                                    <b>
                                      Alternative #<xsl:value-of select="$array[$getPosition]" />
                                    </b>
                                  </span>
                                </td>
                              </tr>
                              <tr>
                                <td    colspan="3" style="border-top: 1px solid black">
                                </td>
                              </tr>
                            </xsl:if>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:choose>
                          <xsl:when test="$DocType = 'PDF' or $DocType = 'MovementPDF'">
                            <tr>
                              <td colspan="3" border="0" align="center">
                                <span>
                                  <b>Alternatives Merge</b>
                                </span>
                              </td>
                            </tr>
                            <tr>
                              <td    colspan="3" style="border-top: 1px solid black">
                              </td>
                            </tr>
                          </xsl:when>
                          <xsl:otherwise>
                            <tr>
                              <td colspan="2" align="center">
                                <span>
                                  <b>Alternatives Merge</b>
                                </span>
                              </td>
                            </tr>
                            <tr>
                              <td    colspan="3" style="border-top: 1px solid black">
                              </td>
                            </tr>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:if>

                  <xsl:if test="position() != last()">
                    <xsl:choose>
                      <xsl:when test="$DocType = 'PDF' or $DocType = 'MovementPDF'">
                        <xsl:if test="$array[$getPosition] &gt; 0">
                          <tr>
                            <td colspan="3" align="center">
                              <span>
                                <b>
                                  Alternative #<xsl:value-of select="$array[$getPosition]" />
                                </b>
                              </span>
                            </td>
                          </tr>
                          <tr>
                            <td    colspan="3" style="border-top: 1px solid black">
                            </td>
                          </tr>
                        </xsl:if>
                        <xsl:if test="$array[$getPosition] = 0 and position() != 1">
                          <tr>
                            <td colspan="3" align="center">
                              <span>
                                <b>
                                  Alternative Route Merges
                                </b>
                              </span>
                            </td>
                          </tr>
                          <tr>
                            <td    colspan="3" style="border-top: 1px solid black">
                            </td>
                          </tr>
                        </xsl:if>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:if test="$array[$getPosition] &gt; 0">
                          <tr>
                            <td colspan="2" align="center">
                              <span>
                                <b>
                                  Alternative #<xsl:value-of select="$array[$getPosition]" />
                                </b>
                              </span>
                            </td>
                          </tr>
                          <tr>
                            <td    colspan="3" style="border-top: 1px solid black">
                            </td>
                          </tr>
                        </xsl:if>
                        <xsl:if test="$array[$getPosition] = 0 and position() != 1">
                          <tr>
                            <td colspan="3" align="center">
                              <span>
                                <b>
                                  Alternative Route Merges
                                </b>
                              </span>
                            </td>
                          </tr>
                          <tr>
                            <td    colspan="3" style="border-top: 1px solid black">
                            </td>
                          </tr>
                        </xsl:if>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:if>
                </xsl:if>

                <xsl:for-each select="ns2:RoadTraversalListPosition">
                  <tr>
                    <td>
                      <xsl:choose>
                        <xsl:when test="ns2:OldRoadTraversal/ns2:RoadTraversal/ns2:RoadIdentity">
                          <strike>
                            <xsl:value-of select="ns2:OldRoadTraversal/ns2:RoadTraversal/ns2:RoadIdentity"/>
                          </strike>
                        </xsl:when>
                        <xsl:when test="ns2:OldRoadTraversal">
                          <u>
                            <b>
                              <xsl:value-of select="ns2:RoadTraversal/ns2:RoadIdentity"/>
                            </b>
                          </u>
                        </xsl:when>
                        <xsl:when test="ns2:RoadTraversal/ns2:RoadIdentity/@Unidentified='true'">
                          <span>Unidentified</span>
                        </xsl:when>
                        <xsl:otherwise>
                          <!--RM#3646 - Start-->
                          <!--<xsl:value-of select="ns2:RoadTraversal/ns2:RoadIdentity"/>-->
                          <xsl:choose>
                            <xsl:when test="contains(ns2:RoadTraversal/ns2:RoadIdentity, '##**##')">
                              <strike>
                                <xsl:value-of select="substring-before(ns2:RoadTraversal/ns2:RoadIdentity, '##**##')"/>
                              </strike>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                              <b>
                                <u>
                                  <xsl:value-of select="substring-after(ns2:RoadTraversal/ns2:RoadIdentity, '##**##')"/>
                                </u>
                              </b>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="ns2:RoadTraversal/ns2:RoadIdentity"/>
                            </xsl:otherwise>
                          </xsl:choose>
                          <!--RM#3646 - End-->
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                    <td>
                      <xsl:choose>
                        <xsl:when test="$UnitType=692001">
                          <xsl:choose>
                            <xsl:when test="ns2:RoadTraversal/ns2:Distance/ns2:Metric &lt; 1000">
                              <!--RM#3646 - Start-->
                              <xsl:choose>
                                <xsl:when test="contains(ns2:RoadTraversal/ns2:Distance/ns2:Metric, '##**##')">
                                  <strike>
                                    <xsl:if test="ns2:RoadTraversal/ns2:Distance/ns2:Metric != ''">
                                      <xsl:call-template name="parseString">
                                        <xsl:with-param name="list" select="substring-before(ns2:RoadTraversal/ns2:Distance/ns2:Metric, '##**##')"/>
                                      </xsl:call-template>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                                    </xsl:if>
                                  </strike>
                                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                  <b>
                                    <u>
                                      <xsl:if test="ns2:RoadTraversal/ns2:Distance/ns2:Metric != ''">
                                        <xsl:call-template name="parseString">
                                          <xsl:with-param name="list" select="substring-after(ns2:RoadTraversal/ns2:Distance/ns2:Metric, '##**##')"/>
                                        </xsl:call-template>
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                                      </xsl:if>
                                    </u>
                                  </b>
                                </xsl:when>
                                <xsl:otherwise>
                                  <xsl:if test="ns2:RoadTraversal/ns2:Distance/ns2:Metric != ''">
                                    <xsl:call-template name="parseString">
                                      <xsl:with-param name="list" select="ns2:RoadTraversal/ns2:Distance/ns2:Metric"/>
                                    </xsl:call-template>
                                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                                  </xsl:if>
                                </xsl:otherwise>
                              </xsl:choose>
                              <!--RM#3646 - End-->
                            </xsl:when>
                            <xsl:when test="ns2:OldRoadTraversal/ns2:RoadTraversal/ns2:Distance/ns2:Metric &lt; 1000">
                              <strike>
                                <xsl:value-of select="ns2:OldRoadTraversal/ns2:RoadTraversal/ns2:Distance/ns2:Metric"/>
                              </strike>
                            </xsl:when>
                            <xsl:when test="ns2:OldRoadTraversal/ns2:RoadTraversal/ns2:Distance/ns2:Metric != ''">
                              <strike>
                                <xsl:variable name="var1" select="ns2:OldRoadTraversal/ns2:RoadTraversal/ns2:Distance/ns2:Metric" />
                                <xsl:variable name="var2" select="format-number($var1 div 1000, '#.##')"/>
                                <xsl:call-template name="parseString">
                                  <xsl:with-param name="list" select="$var2"/>
                                </xsl:call-template>
                                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                              </strike>
                            </xsl:when>
                            <xsl:when test="ns2:OldRoadTraversal">
                              <b>
                                <u>
                                  <xsl:if test="ns2:RoadTraversal/ns2:Distance/ns2:Metric != ''">
                                    <xsl:variable name="var1" select="ns2:RoadTraversal/ns2:Distance/ns2:Metric" />
                                    <xsl:variable name="var2" select="format-number($var1 div 1000, '#.##')"/>
                                    <xsl:call-template name="parseString">
                                      <xsl:with-param name="list" select="$var2"/>
                                    </xsl:call-template>
                                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                                  </xsl:if>
                                </u>
                              </b>
                            </xsl:when>
                            <xsl:otherwise>
                              <!--RM#3646 - Start-->
                              <xsl:choose>
                                <xsl:when test="contains(ns2:RoadTraversal/ns2:Distance/ns2:Metric, '##**##')">
                                  <strike>
                                    <xsl:if test="ns2:RoadTraversal/ns2:Distance/ns2:Metric != ''">
                                      <xsl:variable name="var1" select="substring-before(ns2:RoadTraversal/ns2:Distance/ns2:Metric, '##**##')" />
                                      <xsl:variable name="var2" select="format-number($var1 div 1000, '#.##')"/>
                                      <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                                    </xsl:if>
                                  </strike>
                                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                  <b>
                                    <u>
                                      <xsl:if test="ns2:RoadTraversal/ns2:Distance/ns2:Metric != ''">
                                        <xsl:variable name="var1" select="substring-after(ns2:RoadTraversal/ns2:Distance/ns2:Metric, '##**##')" />
                                        <xsl:variable name="var2" select="format-number($var1 div 1000, '#.##')"/>
                                        <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                                      </xsl:if>
                                    </u>
                                  </b>
                                </xsl:when>
                                <xsl:otherwise>
                                  <xsl:if test="ns2:RoadTraversal/ns2:Distance/ns2:Metric != ''">
                                    <xsl:variable name="var1" select="ns2:RoadTraversal/ns2:Distance/ns2:Metric" />
                                    <xsl:variable name="var2" select="format-number($var1 div 1000, '#.##')"/>
                                    <xsl:value-of select="$var2"/>
                                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                                  </xsl:if>
                                </xsl:otherwise>
                              </xsl:choose>
                              <!--RM#3646 - End-->
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:choose>
                            <xsl:when test="ns2:RoadTraversal/ns2:Distance/ns2:Imperial &lt; 700">
                              <!--RM#3646 - Start-->
                              <xsl:choose>
                                <xsl:when test="contains(ns2:RoadTraversal/ns2:Distance/ns2:Imperial, '##**##')">
                                  <strike>
                                    <xsl:if test="ns2:RoadTraversal/ns2:Distance/ns2:Imperial != ''">
                                      <xsl:call-template name="parseString">
                                        <xsl:with-param name="list" select="substring-before(ns2:RoadTraversal/ns2:Distance/ns2:Imperial, '##**##')"/>
                                      </xsl:call-template>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>yards
                                    </xsl:if>
                                  </strike>
                                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                  <b>
                                    <u>
                                      <xsl:if test="ns2:RoadTraversal/ns2:Distance/ns2:Imperial != ''">
                                        <xsl:call-template name="parseString">
                                          <xsl:with-param name="list" select="substring-after(ns2:RoadTraversal/ns2:Distance/ns2:Imperial, '##**##')"/>
                                        </xsl:call-template>
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>yards
                                      </xsl:if>
                                    </u>
                                  </b>
                                </xsl:when>
                                <xsl:otherwise>
                                  <xsl:if test="ns2:RoadTraversal/ns2:Distance/ns2:Imperial != ''">
                                    <xsl:call-template name="parseString">
                                      <xsl:with-param name="list" select="ns2:RoadTraversal/ns2:Distance/ns2:Imperial"/>
                                    </xsl:call-template>
                                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>yards
                                  </xsl:if>
                                </xsl:otherwise>
                              </xsl:choose>
                              <!--RM#3646 - End-->
                            </xsl:when>
                            <xsl:when test="ns2:OldRoadTraversal/ns2:RoadTraversal/ns2:Distance/ns2:Imperial &lt; 700">
                              <strike>
                                <xsl:value-of select="ns2:OldRoadTraversal/ns2:RoadTraversal/ns2:Distance/ns2:Imperial"/>
                                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>yards
                              </strike>
                            </xsl:when>
                            <xsl:when test="ns2:OldRoadTraversal/ns2:RoadTraversal/ns2:Distance/ns2:Imperial != ''">
                              <strike>
                                <xsl:variable name="var1" select="ns2:OldRoadTraversal/ns2:RoadTraversal/ns2:Distance/ns2:Imperial" />
                                <xsl:variable name="var2" select="format-number($var1 div 1760, '#.##')"/>
                                <xsl:call-template name="parseString">
                                  <xsl:with-param name="list" select="$var2"/>
                                </xsl:call-template>
                                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                              </strike>
                            </xsl:when>
                            <xsl:when test="ns2:OldRoadTraversal">
                              <b>
                                <u>
                                  <xsl:if test="ns2:RoadTraversal/ns2:Distance/ns2:Imperial != ''">
                                    <xsl:variable name="var1" select="ns2:RoadTraversal/ns2:Distance/ns2:Imperial" />
                                    <xsl:variable name="var2" select="format-number($var1 div 1760, '#.##')"/>
                                    <xsl:call-template name="parseString">
                                      <xsl:with-param name="list" select="$var2"/>
                                    </xsl:call-template>
                                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                                  </xsl:if>
                                </u>
                              </b>
                            </xsl:when>
                            <xsl:otherwise>
                              <!--RM#3646 - Start-->
                              <xsl:choose>
                                <xsl:when test="contains(ns2:RoadTraversal/ns2:Distance/ns2:Imperial, '##**##')">
                                  <strike>
                                    <xsl:if test="ns2:RoadTraversal/ns2:Distance/ns2:Imperial != ''">
                                      <xsl:variable name="var1" select="substring-before(ns2:RoadTraversal/ns2:Distance/ns2:Imperial, '##**##')" />
                                      <xsl:variable name="var2" select="format-number($var1 div 1760, '#.##')"/>
                                      <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                                    </xsl:if>
                                  </strike>
                                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                  <b>
                                    <u>
                                      <xsl:if test="ns2:RoadTraversal/ns2:Distance/ns2:Imperial != ''">
                                        <xsl:variable name="var1" select="substring-after(ns2:RoadTraversal/ns2:Distance/ns2:Imperial, '##**##')" />
                                        <xsl:variable name="var2" select="format-number($var1 div 1760, '#.##')"/>
                                        <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                                      </xsl:if>
                                    </u>
                                  </b>
                                </xsl:when>
                                <xsl:otherwise>
                                  <xsl:if test="ns2:RoadTraversal/ns2:Distance/ns2:Imperial != ''">
                                    <xsl:variable name="var1" select="ns2:RoadTraversal/ns2:Distance/ns2:Imperial" />
                                    <xsl:variable name="var2" select="format-number($var1 div 1760, '#.##')"/>
                                    <xsl:value-of select="$var2"/>
                                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                                  </xsl:if>
                                </xsl:otherwise>
                              </xsl:choose>
                              <!--RM#3646 - End-->
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                  </tr>
                  <tr>
                    <td    colspan="3" style="border-top: 1px solid black">
                    </td>
                  </tr>
                </xsl:for-each>
              </xsl:for-each>
            </table>
            <br></br>
          </xsl:for-each>

        </p>
        <br/>
        <!--Change Code Start RM#4604 21 july-->
        <xsl:variable name="FirstVehicle">

          <xsl:for-each select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles">
            <xsl:if test="ns3:ConfigurationSummaryListPosition/ns3:ConfigurationSummary != ''">
              <xsl:if test="position()=1">
                <xsl:choose>
                  <xsl:when test="contains(ns3:ConfigurationSummaryListPosition/ns3:ConfigurationSummary, '##**##')">
                    <xsl:value-of select="substring-after(ns3:ConfigurationSummaryListPosition/ns3:ConfigurationSummary,'##**##')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="ns3:ConfigurationSummaryListPosition/ns3:ConfigurationSummary"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:if>
            </xsl:if>
          </xsl:for-each>

        </xsl:variable>

        <xsl:variable name="StatusVehicle">
          <xsl:for-each select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles">
            <xsl:if test="ns3:ConfigurationSummaryListPosition/ns3:ConfigurationSummary != ''">

              <xsl:choose>
                <xsl:when test="contains(ns3:ConfigurationSummaryListPosition/ns3:ConfigurationSummary, '##**##')">

                  <xsl:if test="$FirstVehicle != substring-after(ns3:ConfigurationSummaryListPosition/ns3:ConfigurationSummary,'##**##')">
                    <xsl:value-of select="false()"/>
                  </xsl:if>

                </xsl:when>
                <xsl:otherwise>
                  <xsl:if test="$FirstVehicle != ns3:ConfigurationSummaryListPosition/ns3:ConfigurationSummary">
                    <xsl:value-of select="false()"/>
                  </xsl:if>
                </xsl:otherwise>
              </xsl:choose>

            </xsl:if>
          </xsl:for-each>
        </xsl:variable>

        <xsl:variable name="IsRouteNameChanged">
          <xsl:for-each select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart">
            <item>
              <xsl:choose>
                <xsl:when test="contains(ns2:Name, '##**##')">
                  <xsl:value-of select="true()"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="false()" />
                </xsl:otherwise>
              </xsl:choose>
            </item>
          </xsl:for-each>
        </xsl:variable>

        <xsl:variable name="OldLegName">
          <xsl:for-each select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart">
            <item>
              <xsl:choose>
                <xsl:when test="contains(ns2:Name, '##**##')">
                  <xsl:value-of select="substring-before(ns2:Name, '##**##')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="ns2:Name" />
                </xsl:otherwise>
              </xsl:choose>
            </item>
          </xsl:for-each>
        </xsl:variable>

        <xsl:variable name="NewLegName">
          <xsl:for-each select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart">
            <item>
              <xsl:choose>
                <xsl:when test="contains(ns2:Name, '##**##')">
                  <xsl:value-of select="substring-after(ns2:Name, '##**##')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="ns2:Name" />
                </xsl:otherwise>
              </xsl:choose>
            </item>
          </xsl:for-each>
        </xsl:variable>

        <xsl:variable name="OldDistance">
          <xsl:for-each select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart">

            <xsl:if test="$UnitType=692001">
              <item>
                <xsl:choose>
                  <!--Changes for RM#4998-->
                  <xsl:when test="contains(ns2:RoadPart/ns2:Distance/ns2:Metric/ns2:Distance, '##**##')">
                    <xsl:call-template name="SplitAlternative">
                      <xsl:with-param name="pText" select="substring-before(ns2:RoadPart/ns2:Distance/ns2:Metric/ns2:Distance, '##**##')"></xsl:with-param>
                      <xsl:with-param name="pDelim" select=" OR"></xsl:with-param>
                    </xsl:call-template>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:call-template name="SplitAlternative">
                      <xsl:with-param name="pText" select="ns2:RoadPart/ns2:Distance/ns2:Metric/ns2:Distance"></xsl:with-param>
                      <xsl:with-param name="pDelim" select=" OR"></xsl:with-param>
                    </xsl:call-template>
                  </xsl:otherwise>
                </xsl:choose>
              </item>
            </xsl:if>

            <xsl:if test="$UnitType=692002">
              <item>
                <xsl:choose>
                  <!--Changes for RM#4998-->
                  <xsl:when test="contains(ns2:RoadPart/ns2:Distance/ns2:Metric/ns2:Distance, '##**##')">
                    <xsl:call-template name="SplitAlternative">
                      <xsl:with-param name="pText" select="substring-before(ns2:RoadPart/ns2:Distance/ns2:Imperial/ns2:Distance, '##**##')"></xsl:with-param>
                      <xsl:with-param name="pDelim" select=" OR"></xsl:with-param>
                    </xsl:call-template>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:call-template name="SplitAlternative">
                      <xsl:with-param name="pText" select="ns2:RoadPart/ns2:Distance/ns2:Imperial/ns2:Distance"></xsl:with-param>
                      <xsl:with-param name="pDelim" select=" OR"></xsl:with-param>
                    </xsl:call-template>
                  </xsl:otherwise>
                </xsl:choose>
              </item>
            </xsl:if>
          </xsl:for-each>
        </xsl:variable>

        <xsl:variable name="NewDistance">
          <xsl:for-each select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart">

            <xsl:if test="$UnitType=692001">
              <item>
                <xsl:choose>
                  <!--Changes for RM#4998-->
                  <xsl:when test="contains(ns2:RoadPart/ns2:Distance/ns2:Metric/ns2:Distance, '##**##')">
                    <xsl:call-template name="SplitAlternative">
                      <xsl:with-param name="pText" select="substring-after(ns2:RoadPart/ns2:Distance/ns2:Metric/ns2:Distance, '##**##')"></xsl:with-param>
                      <xsl:with-param name="pDelim" select=" OR"></xsl:with-param>
                    </xsl:call-template>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:call-template name="SplitAlternative">
                      <xsl:with-param name="pText" select="ns2:RoadPart/ns2:Distance/ns2:Metric/ns2:Distance"></xsl:with-param>
                      <xsl:with-param name="pDelim" select=" OR"></xsl:with-param>
                    </xsl:call-template>
                  </xsl:otherwise>
                </xsl:choose>
              </item>
            </xsl:if>

            <xsl:if test="$UnitType=692002">
              <item>
                <xsl:choose>
                  <!--Changes for RM#4998-->
                  <xsl:when test="contains(ns2:RoadPart/ns2:Distance/ns2:Metric/ns2:Distance, '##**##')">
                    <xsl:call-template name="SplitAlternative">
                      <xsl:with-param name="pText" select="substring-after(ns2:RoadPart/ns2:Distance/ns2:Imperial/ns2:Distance, '##**##')"></xsl:with-param>
                      <xsl:with-param name="pDelim" select=" OR"></xsl:with-param>
                    </xsl:call-template>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:call-template name="SplitAlternative">
                      <xsl:with-param name="pText" select="ns2:RoadPart/ns2:Distance/ns2:Imperial/ns2:Distance"></xsl:with-param>
                      <xsl:with-param name="pDelim" select=" OR"></xsl:with-param>
                    </xsl:call-template>
                  </xsl:otherwise>
                </xsl:choose>
              </item>
            </xsl:if>
          </xsl:for-each>
        </xsl:variable>

        <xsl:variable name="CountVehicles">
          <xsl:for-each select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart">

            <xsl:if test="$UnitType=692001">
              <xsl:if test="$StatusVehicle = 'false'">
                <item>
                  Leg:
                  <xsl:choose>
                    <xsl:when test="contains(ns2:Name, '##**##')">
                      <xsl:value-of select="substring-after(ns2:Name, '##**##')"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="ns2:Name" />
                    </xsl:otherwise>
                  </xsl:choose>
                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                  <xsl:choose>
                    <xsl:when test="contains(ns2:RoadPart/ns2:Distance/ns2:Metric/ns2:Distance, '##**##')">
                      <!--Changes for RM#4998-->
                      <xsl:call-template name="SplitAlternative">
                        <xsl:with-param name="pText" select="substring-after(ns2:RoadPart/ns2:Distance/ns2:Metric/ns2:Distance, '##**##')"></xsl:with-param>
                        <xsl:with-param name="pDelim" select=" OR"></xsl:with-param>
                      </xsl:call-template>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:call-template name="SplitAlternative">
                        <!--Changes for RM#4998-->
                        <xsl:with-param name="pText" select="ns2:RoadPart/ns2:Distance/ns2:Metric/ns2:Distance"></xsl:with-param>
                        <xsl:with-param name="pDelim" select=" OR"></xsl:with-param>
                      </xsl:call-template>
                    </xsl:otherwise>
                  </xsl:choose>
                </item>
              </xsl:if>
            </xsl:if>

            <xsl:if test="$UnitType=692002">
              <xsl:if test="$StatusVehicle = 'false'">
                <item>
                  Leg:
                  <xsl:choose>
                    <xsl:when test="contains(ns2:Name, '##**##')">
                      <xsl:value-of select="substring-after(ns2:Name, '##**##')"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="ns2:Name" />
                    </xsl:otherwise>
                  </xsl:choose>
                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                  <xsl:choose>
                    <xsl:when test="contains(ns2:RoadPart/ns2:Distance/ns2:Metric/ns2:Distance, '##**##')">
                      <!--Changes for RM#4998-->
                      <xsl:call-template name="SplitAlternative">
                        <xsl:with-param name="pText" select="substring-after(ns2:RoadPart/ns2:Distance/ns2:Imperial/ns2:Distance, '##**##')"></xsl:with-param>
                        <xsl:with-param name="pDelim" select=" OR"></xsl:with-param>
                      </xsl:call-template>
                    </xsl:when>
                    <xsl:otherwise>
                      <!--Changes for RM#4998-->
                      <xsl:call-template name="SplitAlternative">
                        <xsl:with-param name="pText" select="ns2:RoadPart/ns2:Distance/ns2:Imperial/ns2:Distance"></xsl:with-param>
                        <xsl:with-param name="pDelim" select=" OR"></xsl:with-param>
                      </xsl:call-template>
                    </xsl:otherwise>
                  </xsl:choose>
                </item>
              </xsl:if>
            </xsl:if>
          </xsl:for-each>
        </xsl:variable>

        <xsl:variable name="VehiclesArray" select="msxsl:node-set($CountVehicles)/item" />
        <xsl:variable name="OldLegNameArray" select="msxsl:node-set($OldLegName)/item" />
        <xsl:variable name="NewLegNameArray" select="msxsl:node-set($NewLegName)/item" />
        <xsl:variable name="OldDistanceArray" select="msxsl:node-set($OldDistance)/item" />
        <xsl:variable name="NewDistanceArray" select="msxsl:node-set($NewDistance)/item" />
        <xsl:variable name="IsRouteNameChangedArray" select="msxsl:node-set($IsRouteNameChanged)/item" />

        <!--Change Code End RM#4604 21 july-->

        <p>
          <b>            

            VEHICLE DETAILS

            <xsl:if test="$StatusVehicle = 'false'">
              Leg:
           
            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
            

              <xsl:if test="contains(ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:Name, '##**##')">
                <strike>
                  <xsl:value-of select="substring-before(ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:Name, '##**##')"/>
                </strike>
                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                <u>
                  <b>
                    <xsl:value-of select="substring-after(ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:Name, '##**##')"/>
                  </b>
                </u>
              </xsl:if>

              <xsl:if test="contains(ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:Name, '##**##')=false()">
                <xsl:value-of select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:Name"/>
              </xsl:if>

              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>

              <xsl:if test="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Distance/ns2:Imperial/ns2:Distance != ''">
                <xsl:choose>
                  <xsl:when test="contains(ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Distance/ns2:Imperial/ns2:Distance, '##**##')">
                    <strike>
                      <xsl:if test="$UnitType=692001">
                        <xsl:variable name="var12" select="substring-before(ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Distance/ns2:Metric/ns2:Distance, '##**##')"/>
                        <xsl:variable name="var13" select="round(number($var12) div number(1000))"/>
                        <xsl:value-of select="$var13"/> <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                      </xsl:if>
                      <xsl:if test="$UnitType=692002">
                        <xsl:variable name="var12" select="substring-before(ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Distance/ns2:Imperial/ns2:Distance, '##**##')"/>
                        <xsl:variable name="var13" select="round(number($var12) div number(1760))"/>
                        <xsl:value-of select="$var13"/> <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                      </xsl:if>

                    </strike>
                    <b>
                      <u>
                        <xsl:if test="$UnitType=692001">
                          <xsl:variable name="var12" select="substring-after(ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Distance/ns2:Metric/ns2:Distance, '##**##')"/>
                          <xsl:variable name="var13" select="round(number($var12) div number(1000))"/>
                          <xsl:value-of select="$var13"/> <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                        </xsl:if>
                        <xsl:if test="$UnitType=692002">
                          <xsl:variable name="var12" select="substring-after(ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Distance/ns2:Imperial/ns2:Distance, '##**##')"/>
                          <xsl:variable name="var13" select="round(number($var12) div number(1760))"/>
                          <xsl:value-of select="$var13"/> <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                        </xsl:if>
                      </u>
                    </b>
                  </xsl:when>
                  <xsl:when test="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:OldDistance/ns2:Distance/ns2:Metric/ns2:Distance">
                    <xsl:if test="$UnitType=692001">

                      <strike>
                        <xsl:variable name="var12" select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:OldDistance/ns2:Distance/ns2:Metric/ns2:Distance"/>
                        <xsl:variable name="var13" select="round(number($var12) div number(1000))"/>
                        <xsl:value-of select="$var13"/> <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                      </strike>
                      <u>
                        <b>
                          <xsl:variable name="var12" select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Distance/ns2:Metric/ns2:Distance"/>
                          <xsl:variable name="var13" select="round(number($var12) div number(1000))"/>
                          <xsl:value-of select="$var13"/> <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                        </b>
                      </u>
                    </xsl:if>

                    <xsl:if test="$UnitType=692002">
                      <strike>
                        <xsl:variable name="var12" select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:OldDistance/ns2:Distance/ns2:Imperial/ns2:Distance"/>
                        <xsl:variable name="var13" select="round(number($var12) div number(1760))"/>
                        <xsl:value-of select="$var13"/> <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                      </strike>
                      <u>
                        <b>
                          <xsl:variable name="var12" select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Distance/ns2:Imperial/ns2:Distance"/>
                          <xsl:variable name="var13" select="round(number($var12) div number(1760))"/>
                          <xsl:value-of select="$var13"/> <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                        </b>
                      </u>
                    </xsl:if>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:if test="$UnitType=692001">
                      <xsl:if test="contains(ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Distance/ns2:Metric/ns2:Distance, '##**##')=false()">
                        <xsl:variable name="var12" select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Distance/ns2:Metric/ns2:Distance"/>
                        <xsl:variable name="var13" select="round(number($var12) div number(1000))"/>
                        <xsl:value-of select="$var13"/> <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                      </xsl:if>
                    </xsl:if>
                    <xsl:if test="$UnitType=692002">
                      <xsl:if test="contains(ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Distance/ns2:Imperial/ns2:Distance, '##**##')=false()">
                        <xsl:variable name="var12" select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Distance/ns2:Imperial/ns2:Distance"/>
                        <xsl:variable name="var13" select="round(number($var12) div number(1760))"/>
                        <xsl:value-of select="$var13"/> <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                      </xsl:if>
                    </xsl:if>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:if>
            </xsl:if>
          </b>
        </p>
        <br/>
        <br/>
        <table style ="margin-left" border = "1">

          <tr>
            <th>
              <b>
                Summary:
              </b>
            </th>
            <th colspan="2">
              <xsl:variable name="configurationSummaryList" select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/
          ns3:ConfigurationSummaryListPosition/ns3:ConfigurationSummary" />
                <xsl:for-each select="$configurationSummaryList">
                        <xsl:if test="generate-id() = generate-id($configurationSummaryList[. = current()][1])">
                          <b>
                          <xsl:if test="(last()=2 and position()=2)"> <br></br> </xsl:if>
                      <xsl:if test="position() > 1 and position() != last()"> or </xsl:if>
                          <xsl:value-of select="." />
                        </b>
                        </xsl:if>
                </xsl:for-each>
            </th>
          </tr>
          <tr>
            <td    colspan="3" style="border-top: 1px solid black">
            </td>
          </tr>
          <xsl:if test="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/
          ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:ConfigurationIdentityListPosition/ns3:ConfigurationIdentity/ns3:PlateNo != '' or
               ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/
          ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:ConfigurationIdentityListPosition/ns3:ConfigurationIdentity/ns3:FleetNo!=''">
            <tr>
              <td>
                Alternative vehicle:
              </td>
              <td colspan="2">
                <xsl:for-each select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/
          ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:ConfigurationIdentityListPosition/ns3:ConfigurationIdentity">
                  <p>
                    <xsl:if test="ns3:PlateNo != ''">
                      <xsl:call-template name="parseString">
                        <xsl:with-param name="list" select="ns3:PlateNo"/>
                      </xsl:call-template>
                      <xsl:if test="position() != last()">
                        ,
                      </xsl:if>
                    </xsl:if >
                    <xsl:if test="ns3:FleetNo != ''">
                      <xsl:call-template name="parseString">
                        <xsl:with-param name="list" select="ns3:FleetNo"/>
                      </xsl:call-template>
                      <xsl:if test="position() != last()">
                        ,
                      </xsl:if>
                    </xsl:if>
                  </p>
                </xsl:for-each>
              </td>
            </tr>
            <tr>
              <td    colspan="3" style="border-top: 1px solid black">
              </td>
            </tr>
          </xsl:if>
          <xsl:if test="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:OverallLengthListPosition != ''">
            <tr>
              <td>Overall length: </td>
              <td colspan="2">

                <xsl:variable name="OverallLengthList" select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:OverallLengthListPosition/ns3:OverallLength" />
                <xsl:for-each select="$OverallLengthList">
                  <xsl:if test="generate-id() = generate-id($OverallLengthList[. = current()][1])">
                    <xsl:if test="position() > 1 and position() != last()"> , </xsl:if>

                    <xsl:if test="./ns3:IncludingProjections != ''">
                      <xsl:choose>
                        <xsl:when test="contains(./ns3:IncludingProjections, '##**##')">

                          <xsl:value-of select="substring-after(./ns3:IncludingProjections, '##**##')"></xsl:value-of>

                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="./ns3:IncludingProjections"/>
                        </xsl:otherwise>
                      </xsl:choose>

                      <xsl:choose>
                        <xsl:when test="$UnitType='' or $UnitType=692001">
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>(including projections),
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>(including projections),
                        </xsl:otherwise>
                      </xsl:choose>

                    </xsl:if>

                    <xsl:if test="./ns3:ExcludingProjections!=''">
                      <xsl:choose>
                        <xsl:when test="contains(./ns3:ExcludingProjections, '##**##')">

                          <xsl:value-of select="substring-after(./ns3:ExcludingProjections, '##**##')"></xsl:value-of>

                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="./ns3:ExcludingProjections"/>
                        </xsl:otherwise>
                      </xsl:choose>
                      <xsl:choose>
                        <xsl:when test="$UnitType='' or $UnitType=692001">
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>(excluding projections),
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>(excluding projections),
                        </xsl:otherwise>
                      </xsl:choose>

                    </xsl:if>

                  </xsl:if>
                </xsl:for-each>
                <br></br>

              </td>
            </tr>
          </xsl:if>
          <tr>
            <td    colspan="3" style="border-top: 1px solid black">
            </td>
          </tr>

          <xsl:if test="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:RigidLengthListPosition != ''">
            <tr>
              <td>Rigid length:</td>
              <td colspan="2">
                <xsl:variable name="RigidLengthList" select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:RigidLengthListPosition" />
                <xsl:for-each select="$RigidLengthList">
                  <xsl:if test="generate-id() = generate-id($RigidLengthList[. = current()][1])">
                    
                    <xsl:if test="(last()=2 and position()=2)"> <br></br> </xsl:if>
                      <xsl:if test="position() > 1 and position() != last()"> or </xsl:if>

                    
                    <xsl:if test="./ns3:OldRigidLength/ns3:RigidLength != ''">
                      <strike>
                        <xsl:choose>
                          <xsl:when test="contains(./ns3:OldRigidLength/ns3:RigidLength, '##**##')">

                            <xsl:value-of select="substring-after(./ns3:OldRigidLength/ns3:RigidLength, '##**##')"></xsl:value-of>

                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="./ns3:OldRigidLength/ns3:RigidLength"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </strike>
                      <b>
                        <u>
                          <xsl:choose>
                            <xsl:when test="contains(./ns3:RigidLength, '##**##')">

                              <xsl:value-of select="substring-after(./ns3:RigidLength, '##**##')"></xsl:value-of>

                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="./ns3:RigidLength"/>
                            </xsl:otherwise>
                          </xsl:choose>
                        </u>
                      </b>

                        <xsl:choose>
                          <xsl:when test="$UnitType='' or $UnitType=692001">
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                          </xsl:otherwise>
                        </xsl:choose>

                      </xsl:if>

                    <xsl:if test="./ns3:OldRigidLength/ns3:RigidLength == ''">
                     <xsl:if test="./ns3:RigidLength != ''">
                      <xsl:choose>
                        <xsl:when test="contains(./ns3:RigidLength, '##**##')">

                          <xsl:value-of select="substring-after(./ns3:RigidLength, '##**##')"></xsl:value-of>

                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="./ns3:RigidLength"/>
                        </xsl:otherwise>
                      </xsl:choose>

                      <xsl:choose>
                        <xsl:when test="$UnitType='' or $UnitType=692001">
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                        </xsl:otherwise>
                      </xsl:choose>

                    </xsl:if>
                    </xsl:if>
                  </xsl:if>
                </xsl:for-each>
              </td>
            </tr>
          </xsl:if>

          
          <tr>
            <td    colspan="3" style="border-top: 1px solid black">
            </td>
          </tr>
          <!--Start Code for #4390-->
          <xsl:if test="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:FrontOverhangListPosition != ''">
            <tr>
              <td>Front overhang:</td>
              <td colspan="2">
                <xsl:variable name="FrontOverhangList" select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:FrontOverhangListPosition/ns3:FrontOverhang" />
                <xsl:for-each select="$FrontOverhangList">
                  <xsl:if test="generate-id() = generate-id($FrontOverhangList[. = current()][1])">
                    <xsl:if test="(last()=2 and position()=2)"> <br></br> </xsl:if>
                      <xsl:if test="position() > 1 and position() != last()"> or </xsl:if>

                    <xsl:if test=". != ''">
                      <xsl:choose>
                        <xsl:when test="contains(., '##**##')">
                          <strike>
                            <xsl:value-of select="substring-before(., '##**##')"></xsl:value-of>
                          </strike>
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                          <B>
                            <u>
                              <xsl:value-of select="substring-after(., '##**##')"></xsl:value-of>
                            </u>
                          </B>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="."/>
                        </xsl:otherwise>
                      </xsl:choose>

                      <xsl:choose>
                        <xsl:when test="$UnitType='' or $UnitType=692001">
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                        </xsl:otherwise>
                      </xsl:choose>

                    </xsl:if>
                  </xsl:if>
                </xsl:for-each>

              </td>
            </tr>
          </xsl:if>
          <!--End Code for #4390-->
          <tr>
            <td>Overall width:</td>
            <td colspan="2">
              <xsl:variable name="OverallWidthList" select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:OverallWidthListPosition" />
              <xsl:for-each select="$OverallWidthList">
                <xsl:if test="generate-id() = generate-id($OverallWidthList[. = current()][1])">

                  <xsl:if test="(last()=2 and position()=2)"> <br></br> </xsl:if>
                      <xsl:if test="position() > 1 and position() != last()"> or </xsl:if>


                  <xsl:if test="./ns3:OldOverallWidth/ns3:OverallWidth != ''">
                    <strike>
                      <xsl:choose>
                        <xsl:when test="contains(./ns3:OldOverallWidth/ns3:OverallWidth, '##**##')">

                          <xsl:value-of select="substring-after(./ns3:OldOverallWidth/ns3:OverallWidth, '##**##')"></xsl:value-of>

                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="./ns3:OldOverallWidth/ns3:OverallWidth"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </strike>
                    <b>
                      <u>
                        <xsl:choose>
                          <xsl:when test="contains(./ns3:OverallWidth, '##**##')">

                            <xsl:value-of select="substring-after(./ns3:OverallWidth, '##**##')"></xsl:value-of>

                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="./ns3:OverallWidth"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </u>
                    </b>

                    <xsl:choose>
                      <xsl:when test="$UnitType='' or $UnitType=692001">
                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                      </xsl:otherwise>
                    </xsl:choose>

                  </xsl:if>

                  <xsl:if test="./ns3:OldOverallWidth/ns3:OverallWidth == ''">
                    <xsl:if test="./ns3:OverallWidth != ''">
                      <xsl:choose>
                        <xsl:when test="contains(./ns3:OverallWidth, '##**##')">

                          <xsl:value-of select="substring-after(./ns3:OverallWidth, '##**##')"></xsl:value-of>

                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="./ns3:OverallWidth"/>
                        </xsl:otherwise>
                      </xsl:choose>

                      <xsl:choose>
                        <xsl:when test="$UnitType='' or $UnitType=692001">
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                        </xsl:otherwise>
                      </xsl:choose>

                    </xsl:if>
                  </xsl:if>
                </xsl:if>
              </xsl:for-each>
            </td>
          </tr>
          <tr>
            <td    colspan="3" style="border-top: 1px solid black">
            </td>
          </tr>
          <tr>
            <td>Overall height:</td>
            <td colspan="2">
              <xsl:variable name="OverallHeightList" select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:OverallHeightListPosition" />
              <xsl:for-each select="$OverallHeightList">
                <xsl:if test="generate-id() = generate-id($OverallHeightList[. = current()][1])">

                  <xsl:if test="(last()=2 and position()=2)"> <br></br> </xsl:if>
                      <xsl:if test="position() > 1 and position() != last()"> or </xsl:if>


                  <xsl:if test="./ns3:OldOverallHeight/ns3:OverallHeight/ns3:MaxHeight != ''">
                    <strike>
                      <xsl:choose>
                        <xsl:when test="contains(./ns3:OldOverallHeight/ns3:OverallHeight/ns3:MaxHeight, '##**##')">

                          <xsl:value-of select="substring-after(./ns3:OldOverallHeight/ns3:OverallHeight/ns3:MaxHeight, '##**##')"></xsl:value-of>

                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="./ns3:OldOverallHeight/ns3:OverallHeight/ns3:MaxHeight"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </strike>
                    <b>
                      <u>
                        <xsl:choose>
                          <xsl:when test="contains(./ns3:OverallHeight/ns3:MaxHeight, '##**##')">

                            <xsl:value-of select="substring-after(./ns3:OverallHeight/ns3:MaxHeight, '##**##')"></xsl:value-of>

                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="./ns3:OverallHeight/ns3:MaxHeight"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </u>
                    </b>

                    <xsl:choose>
                      <xsl:when test="$UnitType='' or $UnitType=692001">
                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                      </xsl:otherwise>
                    </xsl:choose>

                  </xsl:if>

                  <xsl:if test="./ns3:OldOverallHeight/ns3:OverallHeight/ns3:MaxHeight == ''">
                    <xsl:if test="./ns3:OverallHeight/ns3:MaxHeight != ''">
                      <xsl:choose>
                        <xsl:when test="contains(./ns3:OverallHeight/ns3:MaxHeight, '##**##')">

                          <xsl:value-of select="substring-after(./ns3:OverallHeight/ns3:MaxHeight, '##**##')"></xsl:value-of>

                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="./ns3:OverallHeight/ns3:MaxHeight"/>
                        </xsl:otherwise>
                      </xsl:choose>

                      <xsl:choose>
                        <xsl:when test="$UnitType='' or $UnitType=692001">
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                        </xsl:otherwise>
                      </xsl:choose>

                    </xsl:if>
                  </xsl:if>
                </xsl:if>
              </xsl:for-each>
            </td>
          </tr>
          <tr>
            <td    colspan="3" style="border-top: 1px solid black">
            </td>
          </tr>
          <tr>
            <td>Gross weight:</td>
            <td colspan="2">
              <xsl:variable name="GrossWeightList" select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:GrossWeightListPosition" />
              <xsl:for-each select="$GrossWeightList">
                <xsl:if test="generate-id() = generate-id($GrossWeightList[. = current()][1])">

                  <xsl:if test="(last()=2 and position()=2)"> <br></br> </xsl:if>
                      <xsl:if test="position() > 1 and position() != last()"> or </xsl:if>


                  <xsl:if test="./ns3:OldGrossWeight/ns3:GrossWeight/ns3:Weight != ''">
                    <strike>
                      <xsl:choose>
                        <xsl:when test="contains(./ns3:OldGrossWeight/ns3:GrossWeight/ns3:Weight, '##**##')">

                          <xsl:value-of select="substring-after(./ns3:OldGrossWeight/ns3:GrossWeight/ns3:Weight, '##**##')"></xsl:value-of>

                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="./ns3:OldGrossWeight/ns3:GrossWeight/ns3:Weight"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </strike>
                    <b>
                      <u>
                        <xsl:choose>
                          <xsl:when test="contains(./ns3:GrossWeight/ns3:Weight, '##**##')">

                            <xsl:value-of select="substring-after(./ns3:GrossWeight/ns3:Weight, '##**##')"></xsl:value-of>

                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="./ns3:GrossWeight/ns3:Weight"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </u>
                    </b>

                    <xsl:choose>
                      <xsl:when test="$UnitType='' or $UnitType=692001">
                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                      </xsl:otherwise>
                    </xsl:choose>

                  </xsl:if>

                  <xsl:if test="./ns3:OldGrossWeight/ns3:GrossWeight/ns3:Weight == ''">
                    <xsl:if test="./ns3:GrossWeight/ns3:Weight != ''">
                      <xsl:choose>
                        <xsl:when test="contains(./ns3:GrossWeight/ns3:Weight, '##**##')">

                          <xsl:value-of select="substring-after(./ns3:GrossWeight/ns3:Weight, '##**##')"></xsl:value-of>

                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="./ns3:GrossWeight/ns3:Weight"/>
                        </xsl:otherwise>
                      </xsl:choose>

                      <xsl:choose>
                        <xsl:when test="$UnitType='' or $UnitType=692001">
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                        </xsl:otherwise>
                      </xsl:choose>

                    </xsl:if>
                  </xsl:if>
                </xsl:if>
              </xsl:for-each>
            </td>
          </tr>
          <tr>
            <td    colspan="3" style="border-top: 1px solid black">
            </td>
          </tr>
          <tr>
            <td>Max axle weight:</td>
            <td colspan="2">
              <xsl:variable name="MaxAxleWeightList" select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:MaxAxleWeightListPosition" />
                <xsl:for-each select="$MaxAxleWeightList">
                  <xsl:if test="generate-id() = generate-id($MaxAxleWeightList[. = current()][1])">
                    
                    <xsl:if test="(last()=2 and position()=2)"> <br></br> </xsl:if>
                      <xsl:if test="position() > 1 and position() != last()"> or </xsl:if>

                    
                    <xsl:if test="./ns3:OldMaxAxleWeight/ns3:MaxAxleWeight/ns3:Weight != ''">
                      <strike>
                        <xsl:choose>
                          <xsl:when test="contains(./ns3:OldMaxAxleWeight/ns3:MaxAxleWeight/ns3:Weight, '##**##')">

                            <xsl:value-of select="substring-after(./ns3:OldMaxAxleWeight/ns3:MaxAxleWeight/ns3:Weight, '##**##')"></xsl:value-of>

                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="./ns3:OldMaxAxleWeight/ns3:MaxAxleWeight/ns3:Weight"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </strike>
                      <b>
                        <u>
                          <xsl:choose>
                            <xsl:when test="contains(./ns3:MaxAxleWeight/ns3:Weight, '##**##')">

                              <xsl:value-of select="substring-after(./ns3:MaxAxleWeight/ns3:Weight, '##**##')"></xsl:value-of>

                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="./ns3:MaxAxleWeight/ns3:Weight"/>
                            </xsl:otherwise>
                          </xsl:choose>
                        </u>
                      </b>

                        <xsl:choose>
                          <xsl:when test="$UnitType='' or $UnitType=692001">
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                          </xsl:otherwise>
                        </xsl:choose>

                      </xsl:if>

                    <xsl:if test="./ns3:OldMaxAxleWeight/ns3:MaxAxleWeight/ns3:Weight == ''">
                     <xsl:if test="./ns3:MaxAxleWeight/ns3:Weight != ''">
                      <xsl:choose>
                        <xsl:when test="contains(./ns3:MaxAxleWeight/ns3:Weight, '##**##')">

                          <xsl:value-of select="substring-after(./ns3:MaxAxleWeight/ns3:Weight, '##**##')"></xsl:value-of>

                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="./ns3:MaxAxleWeight/ns3:Weight"/>
                        </xsl:otherwise>
                      </xsl:choose>

                      <xsl:choose>
                        <xsl:when test="$UnitType='' or $UnitType=692001">
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                        </xsl:otherwise>
                      </xsl:choose>

                    </xsl:if>
                    </xsl:if>
                  </xsl:if>
                </xsl:for-each>
            </td>
          </tr>
          <tr>
            <td    colspan="3" style="border-top: 1px solid black">
            </td>
          </tr>

        </table>
        <br/>
        <xsl:for-each select="ns2:RouteParts/ns2:RoutePartListPosition">

          <xsl:variable name="getOuterPosition" select="position()" />

          <xsl:for-each select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle">
            <xsl:if test="ns3:Summary != ''">
              <xsl:variable name="getPosition" select="position()" />

              <xsl:if test="$getPosition = 1">
                <xsl:choose>
                  <xsl:when test="$IsRouteNameChangedArray[$getOuterPosition] = 'true'">
                    <b>
                      Leg <xsl:value-of select="$getOuterPosition"/> :
                    </b>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                    <strike>
                      <xsl:value-of select="$OldLegNameArray[$getOuterPosition]"/>
                    </strike>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                    <u>
                      <b>
                        <xsl:value-of select="$NewLegNameArray[$getOuterPosition]"/>
                      </b>
                    </u>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                    <strike>
                      <xsl:value-of select="$OldDistanceArray[$getOuterPosition]"/>
                    </strike>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                    <u>
                      <b>
                        <xsl:value-of select="$NewDistanceArray[$getOuterPosition]"/>
                      </b>
                    </u>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:if test="$NewLegNameArray[$getOuterPosition] != ''">
                      <b>
                        Leg <xsl:value-of select="$getOuterPosition"/> :
                      </b>
                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                      <b>
                        <xsl:value-of select="$NewLegNameArray[$getOuterPosition]"/>
                      </b>
                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                      <b>
                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                        <xsl:value-of select="$NewDistanceArray[$getOuterPosition]"/>
                      </b>
                    </xsl:if>
                  </xsl:otherwise>
                </xsl:choose>
                <br></br>

              </xsl:if>

              <table style ="margin-left" border = "1">
                <tr>
                  <td>
                    <b>
                      <!--TODO 1-->
                      Semi trailer
                    </b>
                  </td>
                  <td colspan="2">
                    <b>
                      <xsl:if test="ns3:Summary!='' ">
                        <xsl:value-of select="ns3:Summary"/>
                      </xsl:if>
                    </b>
                  </td>
                </tr>
                <!--</xsl:if >-->
                <xsl:if test="ns3:GrossWeight/ns3:Weight!=''">
                  <tr>
                    <td>Gross weight:</td>

                    <td colspan="2">
                      <xsl:if test="ns3:GrossWeight/ns3:Weight!=''">
                        <xsl:choose>
                          <xsl:when test="contains(ns3:GrossWeight/ns3:Weight, '##**##')">
                            <xsl:value-of select="substring-after(ns3:GrossWeight/ns3:Weight, '##**##')"/>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="ns3:GrossWeight/ns3:Weight"/>
                          </xsl:otherwise>
                        </xsl:choose>

                        <xsl:choose>
                          <xsl:when test="$UnitType='' or $UnitType=692001">
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>kg
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>kg
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:if>
                    </td>


                  </tr>
                </xsl:if>
                <xsl:if test="ns3:AxleConfiguration/ns3:AxleWeightListPosition/ns3:AxleWeight !=''">
                  <tr>
                    <td>Axle weight:</td>

                    <td colspan="2">
                      <xsl:for-each select="ns3:AxleConfiguration/ns3:AxleWeightListPosition">

                        <xsl:if test="ns3:AxleWeight!='' and ns3:AxleWeight/@AxleCount!=''">
                          <xsl:choose>
                            <xsl:when test="contains(ns3:AxleWeight, '##**##')">
                              <xsl:value-of select="substring-after(ns3:AxleWeight, '##**##')"/>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="ns3:AxleWeight"/>
                            </xsl:otherwise>
                          </xsl:choose>
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>kg<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text> x<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                          <xsl:choose>
                            <xsl:when test="contains(ns3:AxleWeight/@AxleCount, '##**##')">
                              <xsl:value-of select="substring-after(ns3:AxleWeight/@AxleCount, '##**##')"/>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="ns3:AxleWeight/@AxleCount"/>
                            </xsl:otherwise>
                          </xsl:choose>
                          <xsl:if test="position() != last()">
                            ,
                          </xsl:if>
                        </xsl:if>
                      </xsl:for-each>
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="ns3:AxleConfiguration/ns3:WheelsPerAxleListPosition/ns3:WheelsPerAxle !=''">
                  <tr>
                    <td>Wheel per axle:</td>

                    <td colspan="2">

                      <xsl:for-each select="ns3:AxleConfiguration/ns3:WheelsPerAxleListPosition">

                        <xsl:if test="ns3:WheelsPerAxle!='' and ns3:WheelsPerAxle/@AxleCount!=''">

                          <xsl:choose>
                            <xsl:when test="contains(ns3:WheelsPerAxle, '##**##')">
                              <xsl:value-of select="substring-after(ns3:WheelsPerAxle, '##**##')"/>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="ns3:WheelsPerAxle"/>
                            </xsl:otherwise>
                          </xsl:choose>

                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text> x<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>

                          <xsl:choose>
                            <xsl:when test="contains(ns3:WheelsPerAxle/@AxleCount, '##**##')">
                              <xsl:value-of select="substring-after(ns3:WheelsPerAxle/@AxleCount, '##**##')"/>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="ns3:WheelsPerAxle/@AxleCount"/>
                            </xsl:otherwise>
                          </xsl:choose>
                          <xsl:if test="position() != last()">
                            ,
                          </xsl:if>
                        </xsl:if>
                      </xsl:for-each>

                    </td>


                  </tr>
                </xsl:if>
                <xsl:if test="ns3:AxleConfiguration/ns3:AxleSpacingListPosition/ns3:AxleSpacing !=''">
                  <tr>
                    <td>Axle spacing:</td>

                    <td colspan="2">
                      <xsl:for-each select="ns3:AxleConfiguration/ns3:AxleSpacingListPosition">
                        <xsl:if test="ns3:AxleSpacing!='' and ns3:AxleSpacing/@AxleCount!=''">

                          <xsl:call-template name="parseStringForMeters">
                            <xsl:with-param name="list" select="ns3:AxleSpacing"/>
                          </xsl:call-template>


                          <xsl:choose>
                            <xsl:when test="$UnitType='' or $UnitType=692001">
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                            </xsl:otherwise>
                          </xsl:choose>
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text> x<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                          <xsl:choose>
                            <xsl:when test="contains(ns3:AxleSpacing/@AxleCount, '##**##')">
                              <xsl:value-of select="substring-after(ns3:AxleSpacing/@AxleCount, '##**##')"/>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="ns3:AxleSpacing/@AxleCount"/>
                            </xsl:otherwise>
                          </xsl:choose>
                          <xsl:if test="position() != last()">
                            ,
                          </xsl:if>
                        </xsl:if>
                      </xsl:for-each>
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="ns3:AxleConfiguration/ns3:TyreSizeListPosition/ns3:TyreSize !=''">
                  <tr>
                    <td>Tyre size:</td>
                    <td colspan="2">
                      <xsl:for-each select="ns3:AxleConfiguration/ns3:TyreSizeListPosition">

                        <xsl:if test="ns3:TyreSize!='' and ns3:TyreSize/@AxleCount!=''">
                          <xsl:choose>
                            <xsl:when test="contains(ns3:TyreSize, '##**##')">
                              <xsl:value-of select="substring-after(ns3:TyreSize, '##**##')"/>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="ns3:TyreSize"/>
                            </xsl:otherwise>
                          </xsl:choose>
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text> x<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                          <xsl:choose>
                            <xsl:when test="contains(ns3:TyreSize/@AxleCount, '##**##')">
                              <xsl:value-of select="substring-after(ns3:TyreSize/@AxleCount, '##**##')"/>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="ns3:TyreSize/@AxleCount"/>
                            </xsl:otherwise>
                          </xsl:choose>
                          <xsl:if test="position() != last()">
                            ,
                          </xsl:if>
                        </xsl:if>

                      </xsl:for-each>
                    </td>

                  </tr>
                </xsl:if>
                <xsl:if test="ns3:AxleConfiguration/ns3:WheelSpacingListPosition/ns3:WheelSpacing !=''">
                  <tr>
                    <td>Tyre centres:</td>
                    <td colspan="2">
                      <xsl:for-each select="ns3:AxleConfiguration/ns3:WheelSpacingListPosition">

                        <xsl:if test="ns3:WheelSpacing!='' and ns3:WheelSpacing/@AxleCount!=''">
                          <xsl:choose>
                            <xsl:when test="contains(ns3:WheelSpacing, '##**##')">
                              <xsl:value-of select="substring-after(ns3:WheelSpacing, '##**##')"/>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="ns3:WheelSpacing"/>
                            </xsl:otherwise>
                          </xsl:choose>
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text> x<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                          <xsl:choose>
                            <xsl:when test="contains(ns3:WheelSpacing/@AxleCount, '##**##')">
                              <xsl:value-of select="substring-after(ns3:WheelSpacing/@AxleCount, '##**##')"/>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="ns3:WheelSpacing/@AxleCount"/>
                            </xsl:otherwise>
                          </xsl:choose>
                          <xsl:if test="position() != last()">
                            ,
                          </xsl:if>
                        </xsl:if>
                      </xsl:for-each>
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="ns3:RigidLength!=''">
                  <tr>
                    <td>Length:</td>
                    <td colspan="2">
                      <xsl:choose>
                        <xsl:when test="ns3:RigidLength != ''">

                          <xsl:call-template name="parseStringForMeters">
                            <xsl:with-param name="list" select="ns3:RigidLength"/>
                          </xsl:call-template>

                          <xsl:choose>
                            <xsl:when test="$UnitType='' or $UnitType=692001">
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>
                          0
                          <xsl:choose>
                            <xsl:when test="$UnitType='' or $UnitType=692001">
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="ns3:Width!=''">
                  <tr>
                    <td>Width:</td>
                    <td colspan="2">
                      <xsl:choose>
                        <xsl:when test="ns3:Width != ''">

                          <xsl:call-template name="parseStringForMeters">
                            <xsl:with-param name="list" select="ns3:Width"/>
                          </xsl:call-template>

                          <xsl:choose>
                            <xsl:when test="$UnitType='' or $UnitType=692001">
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>
                          0<xsl:choose>
                            <xsl:when test="$UnitType='' or $UnitType=692001">
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="ns3:Height/ns3:MaxHeight!=''">
                  <tr>
                    <td>Max. height:</td>
                    <td colspan="2">

                      <xsl:call-template name="parseStringForMeters">
                        <xsl:with-param name="list" select="ns3:Height/ns3:MaxHeight"/>
                      </xsl:call-template>

                      <xsl:choose>
                        <xsl:when test="$UnitType='' or $UnitType=692001">
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="ns3:Height/ns3:ReducibleHeight!=''">
                  <tr>
                    <td>Reducible height:</td>
                    <td colspan="2">

                      <xsl:call-template name="parseStringForMeters">
                        <xsl:with-param name="list" select="ns3:Height/ns3:ReducibleHeight"/>
                      </xsl:call-template>

                      <xsl:choose>
                        <xsl:when test="$UnitType='' or $UnitType=692001">
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                  </tr>
                </xsl:if>
                <tr>
                  <td>Wheelbase:</td>
                  <td colspan="2">
                    <xsl:choose>
                      <xsl:when test="ns3:Wheelbase != ''">

                        <xsl:call-template name="parseStringForMeters">
                          <xsl:with-param name="list" select="ns3:Wheelbase"/>
                        </xsl:call-template>


                        <xsl:choose>
                          <xsl:when test="$UnitType='' or $UnitType=692001">
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:when>
                      <xsl:otherwise>
                        0<xsl:choose>
                          <xsl:when test="$UnitType='' or $UnitType=692001">
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:otherwise>
                    </xsl:choose>
                  </td>
                </tr>
                <tr>
                  <td>Left overhang:</td>
                  <td colspan="2">
                    <xsl:choose>
                      <xsl:when test="ns3:LeftOverhang != ''">

                        <xsl:call-template name="parseStringForMeters">
                          <xsl:with-param name="list" select="ns3:LeftOverhang"/>
                        </xsl:call-template>

                        <xsl:choose>
                          <xsl:when test="$UnitType='' or $UnitType=692001">
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:when>
                      <xsl:otherwise>
                        0<xsl:choose>
                          <xsl:when test="$UnitType='' or $UnitType=692001">
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:otherwise>
                    </xsl:choose>
                  </td>
                </tr>
                <tr>
                  <td>Right overhang:</td>
                  <td colspan="2">
                    <xsl:choose>
                      <xsl:when test="ns3:RightOverhang != ''">

                        <xsl:call-template name="parseStringForMeters">
                          <xsl:with-param name="list" select="ns3:RightOverhang"/>
                        </xsl:call-template>

                        <xsl:choose>
                          <xsl:when test="$UnitType='' or $UnitType=692001">
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:when>
                      <xsl:otherwise>
                        0<xsl:choose>
                          <xsl:when test="$UnitType='' or $UnitType=692001">
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:otherwise>
                    </xsl:choose>
                  </td>
                </tr>
                <tr>
                  <!--TODO 2-->
                  <td>Front overhang:</td>
                  <td colspan="2">
                    <xsl:choose>
                      <xsl:when test="ns3:FrontOverhang != ''">

                        <xsl:call-template name="parseStringForMeters">
                          <xsl:with-param name="list" select="ns3:FrontOverhang"/>
                        </xsl:call-template>

                        <xsl:choose>
                          <xsl:when test="$UnitType='' or $UnitType=692001">
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:when>
                      <xsl:otherwise>
                        0<xsl:choose>
                          <xsl:when test="$UnitType='' or $UnitType=692001">
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:otherwise>
                    </xsl:choose>
                  </td>
                </tr>
                <tr>
                  <!--TODO 3-->
                  <td>Rear overhang:</td>
                  <td colspan="2">
                    <xsl:choose>
                      <xsl:when test="ns3:RearOverhang != ''">

                        <xsl:call-template name="parseStringForMeters">
                          <xsl:with-param name="list" select="ns3:RearOverhang"/>
                        </xsl:call-template>

                        <xsl:choose>
                          <xsl:when test="$UnitType='' or $UnitType=692001">
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:when>
                      <xsl:otherwise>
                        0<xsl:choose>
                          <xsl:when test="$UnitType='' or $UnitType=692001">
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:otherwise>
                    </xsl:choose>
                  </td>
                </tr>
                <tr>
                  <!--TODO 5-->
                  <td>Ground clearance:</td>
                  <td colspan="2">
                    <xsl:choose>
                      <xsl:when test="ns3:GroundClearance != ''">

                        <xsl:call-template name="parseStringForMeters">
                          <xsl:with-param name="list" select="ns3:GroundClearance"/>
                        </xsl:call-template>

                        <xsl:choose>
                          <xsl:when test="$UnitType='' or $UnitType=692001">
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:when>
                      <xsl:otherwise>
                        0<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                      </xsl:otherwise>
                    </xsl:choose>
                  </td>
                </tr>
                <tr>
                  <!--TODO 6-->
                  <td>Reduced ground clearance:</td>
                  <td colspan="2">
                    <xsl:choose>
                      <xsl:when test="ns3:ReducedGroundClearance != ''">

                        <xsl:call-template name="parseStringForMeters">
                          <xsl:with-param name="list" select="ns3:ReducedGroundClearance"/>
                        </xsl:call-template>

                        <xsl:choose>
                          <xsl:when test="$UnitType='' or $UnitType=692001">
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:when>
                      <xsl:otherwise>
                        0<xsl:choose>
                          <xsl:when test="$UnitType='' or $UnitType=692001">
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:otherwise>
                    </xsl:choose>
                  </td>
                </tr>
                <tr>
                  <!--TODO 4-->
                  <td>Outside track:</td>
                  <td colspan="2">
                    <xsl:choose>
                      <xsl:when test="ns3:OutsideTrack != ''">

                        <xsl:call-template name="parseStringForMeters">
                          <xsl:with-param name="list" select="ns3:OutsideTrack"/>
                        </xsl:call-template>

                        <xsl:choose>
                          <xsl:when test="$UnitType='' or $UnitType=692001">
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:when>
                      <xsl:otherwise>
                        0
                        <xsl:choose>
                          <xsl:when test="$UnitType='' or $UnitType=692001">
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:otherwise>
                    </xsl:choose>
                  </td>
                </tr>
              </table>
              <br/>
            </xsl:if>
          </xsl:for-each>
          <!--For Semi Vehicles Ends here-->

          <!--For Non Semi Vehicles-->
          <xsl:for-each select="ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:Configuration/ns3:NonSemiVehicle/ns3:ComponentListPosition">
            <xsl:if test="ns3:Component/ns3:DrawbarTractor/ns3:Summary != '' 
                  or ns3:Component/ns3:LoadBearing/ns3:Summary != '' ">
              <xsl:variable name="getPosition" select="position()" />


              <xsl:if test="$getPosition = 1">
                <xsl:choose>
                  <xsl:when test="$IsRouteNameChangedArray[$getOuterPosition] = 'true'">
                    <b>
                      Leg <xsl:value-of select="$getOuterPosition"/> :
                    </b>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                    <strike>
                      <xsl:value-of select="$OldLegNameArray[$getOuterPosition]"/>
                    </strike>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                    <u>
                      <b>
                        <xsl:value-of select="$NewLegNameArray[$getOuterPosition]"/>
                      </b>
                    </u>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                    <strike>
                      <xsl:value-of select="$OldDistanceArray[$getOuterPosition]"/>
                    </strike>
                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                    <u>
                      <b>
                        <xsl:value-of select="$NewDistanceArray[$getOuterPosition]"/>
                      </b>
                    </u>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:if test="$NewLegNameArray[$getOuterPosition] != ''">
                      <b>
                        Leg <xsl:value-of select="$getOuterPosition"/> :
                      </b>
                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                      <b>
                        <xsl:value-of select="$NewLegNameArray[$getOuterPosition]"/>
                      </b>
                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                      <b>
                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                        <xsl:value-of select="$NewDistanceArray[$getOuterPosition]"/>
                      </b>
                    </xsl:if>
                  </xsl:otherwise>
                </xsl:choose>
                <br></br>

              </xsl:if>
              <br/>
              <table style ="margin-left" border = "1">
                <tr>
                  <td>
                    <b>
                      <!--TODO 1-->
                      <xsl:choose>
                        <xsl:when test="contains(ns3:Component/@ComponentType, 'tractor')">
                          Tractor
                        </xsl:when>
                        <xsl:when test="contains(ns3:Component/@ComponentType, 'trailer')">
                          Trailer
                        </xsl:when>
                        <xsl:when test="contains(ns3:Component/@ComponentType, 'spmt')">
                          SPMT
                        </xsl:when>
                        <xsl:otherwise>
                          Tractor
                        </xsl:otherwise>
                      </xsl:choose>
                    </b>
                  </td>
                  <td colspan="2">
                    <b>
                      <xsl:if test="contains(ns3:Component/ns3:DrawbarTractor/ns3:Summary, '##**##')=false()">
                        <xsl:value-of select="ns3:Component/ns3:DrawbarTractor/ns3:Summary"/>
                      </xsl:if>

                      <xsl:if test="contains(ns3:Component/ns3:DrawbarTractor/ns3:Summary, '##**##')=true()">
                        <xsl:value-of select="substring-after(ns3:Component/ns3:DrawbarTractor/ns3:Summary, '##**##')"/>
                      </xsl:if>

                      <xsl:if test="contains(ns3:Component/ns3:LoadBearing/ns3:Summary, '##**##')=false()">
                        <xsl:value-of select="ns3:Component/ns3:LoadBearing/ns3:Summary"/>
                      </xsl:if>

                      <xsl:if test="contains(ns3:Component/ns3:LoadBearing/ns3:Summary, '##**##')=true()">
                        <xsl:value-of select="substring-after(ns3:Component/ns3:LoadBearing/ns3:Summary, '##**##')"/>
                      </xsl:if>
                    </b>
                  </td>
                </tr>
                <!--</xsl:if >-->
                <xsl:if test="ns3:Component/ns3:DrawbarTractor/ns3:Weight!=''">
                  <tr>
                    <td>Gross weight:</td>
                    <td colspan="2">

                      <xsl:call-template name="parseString">
                        <xsl:with-param name="list" select="ns3:Component/ns3:DrawbarTractor/ns3:Weight"/>
                      </xsl:call-template>

                      <xsl:choose>
                        <xsl:when test="$UnitType='' or $UnitType=692001">
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>kg
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>kg
                        </xsl:otherwise>
                      </xsl:choose>

                    </td>
                  </tr>
                </xsl:if>
                <xsl:if test="ns3:Component/ns3:LoadBearing/ns3:Weight!=''">
                  <tr>
                    <td>Gross weight:</td>
                    <td colspan="2">

                      <xsl:call-template name="parseString">
                        <xsl:with-param name="list" select="ns3:Component/ns3:LoadBearing/ns3:Weight"/>
                      </xsl:call-template>


                      <xsl:choose>
                        <xsl:when test="$UnitType='' or $UnitType=692001">
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>kg
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>kg
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                  </tr>
                </xsl:if>

                <xsl:if test="ns3:Component/ns3:DrawbarTractor/ns3:AxleConfiguration/ns3:AxleWeightListPosition/ns3:AxleWeight !=''">
                  <tr>
                    <td>Axle weight:</td>

                    <td colspan="2">
                      <xsl:for-each select="ns3:Component/ns3:DrawbarTractor/ns3:AxleConfiguration/ns3:AxleWeightListPosition">

                        <xsl:if test="ns3:AxleWeight!='' and ns3:AxleWeight/@AxleCount!=''">

                          <xsl:call-template name="parseString">
                            <xsl:with-param name="list" select="ns3:AxleWeight"/>
                          </xsl:call-template>

                          <xsl:choose>
                            <xsl:when test="$UnitType='' or $UnitType=692001">
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>kg
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                            </xsl:otherwise>
                          </xsl:choose>
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text> x<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>

                          <xsl:call-template name="parseString">
                            <xsl:with-param name="list" select="ns3:AxleWeight/@AxleCount"/>
                          </xsl:call-template>
                          <xsl:if test="position() != last()">
                            ,
                          </xsl:if>
                        </xsl:if>
                      </xsl:for-each>

                    </td>


                  </tr>
                </xsl:if>

                <xsl:if test="ns3:Component/ns3:LoadBearing/ns3:AxleConfiguration/ns3:AxleWeightListPosition/ns3:AxleWeight !=''">
                  <tr>
                    <td>Axle weight:</td>
                    <td colspan="2">
                      <xsl:for-each select="ns3:Component/ns3:LoadBearing/ns3:AxleConfiguration/ns3:AxleWeightListPosition">
                        <xsl:if test="ns3:AxleWeight!='' and ns3:AxleWeight/@AxleCount!=''">

                          <xsl:call-template name="parseString">
                            <xsl:with-param name="list" select="ns3:AxleWeight"/>
                          </xsl:call-template>

                          <xsl:choose>
                            <xsl:when test="$UnitType='' or $UnitType=692001">
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>kg
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                            </xsl:otherwise>
                          </xsl:choose>
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text> x<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>

                          <xsl:call-template name="parseString">
                            <xsl:with-param name="list" select="ns3:AxleWeight/@AxleCount"/>
                          </xsl:call-template>

                          <xsl:if test="position() != last()">
                            ,
                          </xsl:if>
                        </xsl:if>
                      </xsl:for-each>
                    </td>
                  </tr>
                </xsl:if>

                <xsl:if test="ns3:Component/ns3:DrawbarTractor/ns3:AxleConfiguration/ns3:WheelsPerAxleListPosition/ns3:WheelsPerAxle !=''">
                  <tr>
                    <td>Wheel per axle:</td>

                    <td colspan="2">

                      <xsl:for-each select="ns3:Component/ns3:DrawbarTractor/ns3:AxleConfiguration/ns3:WheelsPerAxleListPosition">
                        <xsl:if test="ns3:WheelsPerAxle!='' and ns3:WheelsPerAxle/@AxleCount!=''">

                          <!--<xsl:value-of select="ns3:WheelsPerAxle"/>-->

                          <xsl:call-template name="parseString">
                            <xsl:with-param name="list" select="ns3:WheelsPerAxle"/>
                          </xsl:call-template>


                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text> x<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>

                          <xsl:call-template name="parseString">
                            <xsl:with-param name="list" select="ns3:WheelsPerAxle/@AxleCount"/>
                          </xsl:call-template>

                          <xsl:if test="position() != last()">
                            ,
                          </xsl:if>

                        </xsl:if>


                      </xsl:for-each>
                    </td>
                  </tr>
                </xsl:if>

                <xsl:if test="ns3:Component/ns3:LoadBearing/ns3:AxleConfiguration/ns3:WheelsPerAxleListPosition/ns3:WheelsPerAxle !=''">
                  <tr>
                    <td>Wheel per axle:</td>
                    <td colspan="2">
                      <xsl:for-each select="ns3:Component/ns3:LoadBearing/ns3:AxleConfiguration/ns3:WheelsPerAxleListPosition">
                        <xsl:if test="ns3:WheelsPerAxle!='' and ns3:WheelsPerAxle/@AxleCount!=''">

                          <xsl:call-template name="parseString">
                            <xsl:with-param name="list" select="ns3:WheelsPerAxle"/>
                          </xsl:call-template>

                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text> x<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>

                          <xsl:call-template name="parseString">
                            <xsl:with-param name="list" select="ns3:WheelsPerAxle/@AxleCount"/>
                          </xsl:call-template>

                          <xsl:if test="position() != last()">
                            ,
                          </xsl:if>
                        </xsl:if>


                      </xsl:for-each>
                    </td>
                  </tr>
                </xsl:if>

                <xsl:if test="ns3:Component/ns3:DrawbarTractor/ns3:AxleConfiguration/ns3:AxleSpacingListPosition/ns3:AxleSpacing !=''">
                  <tr>
                    <td>Axle spacing:</td>

                    <td colspan="2">
                      <xsl:for-each select="ns3:Component/ns3:DrawbarTractor/ns3:AxleConfiguration/ns3:AxleSpacingListPosition">
                        <xsl:if test="ns3:AxleSpacing!='' and ns3:AxleSpacing/@AxleCount!=''">

                          <!--<xsl:value-of select="ns3:AxleSpacing"/>-->

                          <xsl:call-template name="parseStringForMeters">
                            <xsl:with-param name="list" select="ns3:AxleSpacing"/>
                          </xsl:call-template>

                          <xsl:choose>
                            <xsl:when test="$UnitType='' or $UnitType=692001">
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                            </xsl:otherwise>
                          </xsl:choose>
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text> x<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>

                          <xsl:call-template name="parseString">
                            <xsl:with-param name="list" select="ns3:AxleSpacing/@AxleCount"/>
                          </xsl:call-template>
                          <xsl:if test="position() != last()">
                            ,
                          </xsl:if>
                        </xsl:if>

                      </xsl:for-each>
                    </td>
                  </tr>
                </xsl:if>

                <xsl:if test="ns3:Component/ns3:LoadBearing/ns3:AxleConfiguration/ns3:AxleSpacingListPosition/ns3:AxleSpacing !=''">
                  <tr>
                    <td>Axle spacing:</td>

                    <td colspan="2">
                      <xsl:for-each select="ns3:Component/ns3:LoadBearing/ns3:AxleConfiguration/ns3:AxleSpacingListPosition">
                        <xsl:if test="ns3:AxleSpacing!='' and ns3:AxleSpacing/@AxleCount!=''">

                          <!--<xsl:value-of select="ns3:AxleSpacing"/>-->

                          <xsl:call-template name="parseStringForMeters">
                            <xsl:with-param name="list" select="ns3:AxleSpacing"/>
                          </xsl:call-template>

                          <xsl:choose>
                            <xsl:when test="$UnitType='' or $UnitType=692001">
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                            </xsl:otherwise>
                          </xsl:choose>
                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text> x<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                          <xsl:call-template name="parseString">
                            <xsl:with-param name="list" select="ns3:AxleSpacing/@AxleCount"/>
                          </xsl:call-template>
                          <xsl:if test="position() != last()">
                            ,
                          </xsl:if>
                        </xsl:if>

                      </xsl:for-each>
                    </td>
                  </tr>
                </xsl:if>

                <xsl:if test="ns3:Component/ns3:DrawbarTractor/ns3:AxleConfiguration/ns3:TyreSizeListPosition/ns3:TyreSize !=''">
                  <tr>
                    <td>Tyre size:</td>
                    <td colspan="2">
                      <xsl:for-each select="ns3:Component/ns3:DrawbarTractor/ns3:AxleConfiguration/ns3:TyreSizeListPosition">

                        <xsl:if test="ns3:TyreSize!='' and ns3:TyreSize/@AxleCount!=''">

                          <!--<xsl:call-template name="parseString">
                            <xsl:with-param name="list" select="ns3:TyreSize"/>
                          </xsl:call-template>-->

                          <!--<xsl:value-of select="ns3:TyreSize"/>-->

                          <xsl:if test="contains(ns3:TyreSize, '##**##')=false()">
                            <xsl:value-of select="ns3:TyreSize"/>
                          </xsl:if>
                          <xsl:if test="contains(ns3:TyreSize, '##**##')=true()">
                            <xsl:value-of select="substring-after(ns3:TyreSize, '##**##')"/>"/>
                          </xsl:if>

                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text> x<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>

                          <xsl:call-template name="parseString">
                            <xsl:with-param name="list" select="ns3:TyreSize/@AxleCount"/>
                          </xsl:call-template>

                          <xsl:if test="position() != last()">
                            ,
                          </xsl:if>
                        </xsl:if>

                      </xsl:for-each>
                    </td>

                  </tr>
                </xsl:if>

                <xsl:if test="ns3:Component/ns3:LoadBearing/ns3:AxleConfiguration/ns3:TyreSizeListPosition/ns3:TyreSize !=''">
                  <tr>
                    <td>Tyre size:</td>
                    <td colspan="2">
                      <xsl:for-each select="ns3:Component/ns3:LoadBearing/ns3:AxleConfiguration/ns3:TyreSizeListPosition">

                        <xsl:if test="ns3:TyreSize!='' and ns3:TyreSize/@AxleCount!=''">
                          <!--<xsl:value-of select="ns3:TyreSize"/>-->

                          <xsl:call-template name="parseString">
                            <xsl:with-param name="list" select="ns3:TyreSize"/>
                          </xsl:call-template>

                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text> x<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>

                          <xsl:call-template name="parseString">
                            <xsl:with-param name="list" select="ns3:TyreSize/@AxleCount"/>
                          </xsl:call-template>
                          <xsl:if test="position() != last()">
                            ,
                          </xsl:if>
                        </xsl:if>

                      </xsl:for-each>
                    </td>

                  </tr>
                </xsl:if>

                <xsl:if test="ns3:Component/ns3:DrawbarTractor/ns3:AxleConfiguration/ns3:WheelSpacingListPosition/ns3:WheelSpacing !=''">
                  <tr>
                    <td>Tyre centres:</td>
                    <td colspan="2">
                      <xsl:for-each select="ns3:Component/ns3:DrawbarTractor/ns3:AxleConfiguration/ns3:WheelSpacingListPosition">

                        <xsl:if test="ns3:WheelSpacing!='' and ns3:WheelSpacing/@AxleCount!=''">

                          <xsl:call-template name="parseString">
                            <xsl:with-param name="list" select="ns3:WheelSpacing"/>
                          </xsl:call-template>

                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text> x<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>

                          <xsl:call-template name="parseString">
                            <xsl:with-param name="list" select="ns3:WheelSpacing/@AxleCount"/>
                          </xsl:call-template>

                          <xsl:if test="position() != last()">
                            ,
                          </xsl:if>

                        </xsl:if>

                      </xsl:for-each>
                    </td>
                  </tr>
                </xsl:if>

                <xsl:if test="ns3:Component/ns3:LoadBearing/ns3:AxleConfiguration/ns3:WheelSpacingListPosition/ns3:WheelSpacing !=''">
                  <tr>
                    <td>Tyre centres:</td>
                    <td colspan="2">
                      <xsl:for-each select="ns3:Component/ns3:LoadBearing/ns3:AxleConfiguration/ns3:WheelSpacingListPosition">

                        <xsl:if test="ns3:WheelSpacing!='' and ns3:WheelSpacing/@AxleCount!=''">

                          <xsl:call-template name="parseString">
                            <xsl:with-param name="list" select="ns3:WheelSpacing"/>
                          </xsl:call-template>

                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text> x<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>

                          <xsl:call-template name="parseString">
                            <xsl:with-param name="list" select="ns3:WheelSpacing/@AxleCount"/>
                          </xsl:call-template>

                          <xsl:if test="position() != last()">
                            ,
                          </xsl:if>

                        </xsl:if>

                      </xsl:for-each>
                    </td>
                  </tr>
                </xsl:if>

                <xsl:if test="ns3:Component/ns3:LoadBearing/ns3:RigidLength != ''">
                  <tr>
                    <td>Length:</td>
                    <td colspan="2">
                      <xsl:if test="ns3:Component/ns3:LoadBearing/ns3:RigidLength != ''">

                        <xsl:call-template name="parseStringForMeters">
                          <xsl:with-param name="list" select="ns3:Component/ns3:LoadBearing/ns3:RigidLength"/>
                        </xsl:call-template>

                        <xsl:choose>
                          <xsl:when test="$UnitType='' or $UnitType=692001">
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:if>
                    </td>
                  </tr>
                </xsl:if>

                <xsl:if test="ns3:Component/ns3:DrawbarTractor/ns3:Length != ''">
                  <tr>
                    <td>Length:</td>
                    <td colspan="2">
                      <xsl:if test="ns3:Component/ns3:DrawbarTractor/ns3:Length != ''">

                        <xsl:call-template name="parseStringForMeters">
                          <xsl:with-param name="list" select="ns3:Component/ns3:DrawbarTractor/ns3:Length"/>
                        </xsl:call-template>

                        <xsl:choose>
                          <xsl:when test="$UnitType='' or $UnitType=692001">
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:if>
                    </td>
                  </tr>
                </xsl:if>

                <xsl:if test="ns3:Component/ns3:LoadBearing/ns3:Width!=''">
                  <tr>
                    <td>Width:</td>
                    <td colspan="2">
                      <xsl:if test="ns3:Component/ns3:LoadBearing/ns3:Width != ''">

                        <xsl:call-template name="parseStringForMeters">
                          <xsl:with-param name="list" select="ns3:Component/ns3:LoadBearing/ns3:Width"/>
                        </xsl:call-template>


                        <xsl:choose>
                          <xsl:when test="$UnitType='' or $UnitType=692001">
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:if>
                    </td>
                  </tr>
                </xsl:if>

                <xsl:if test="ns3:Component/ns3:LoadBearing/ns3:Height/ns3:MaxHeight!=''">
                  <tr>
                    <td>Max. height:</td>
                    <td colspan="2">
                      <xsl:if test="ns3:Component/ns3:LoadBearing/ns3:Height/ns3:MaxHeight != ''">

                        <xsl:call-template name="parseStringForMeters">
                          <xsl:with-param name="list" select="ns3:Component/ns3:LoadBearing/ns3:Height/ns3:MaxHeight"/>
                        </xsl:call-template>


                        <xsl:choose>
                          <xsl:when test="$UnitType='' or $UnitType=692001">
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:if>
                    </td>
                  </tr>
                </xsl:if>

                <xsl:if test="ns3:Component/ns3:LoadBearing/ns3:Height/ns3:ReducibleHeight!=''">
                  <tr>
                    <td>Reducible height:</td>
                    <td colspan="2">
                      <xsl:if test="ns3:Component/ns3:LoadBearing/ns3:Height/ns3:ReducibleHeight != ''">

                        <!--<xsl:value-of select="ns3:Component/ns3:LoadBearing/ns3:Height/ns3:ReducibleHeight"/>-->

                        <xsl:call-template name="parseStringForMeters">
                          <xsl:with-param name="list" select="ns3:Component/ns3:LoadBearing/ns3:Height/ns3:ReducibleHeight"/>
                        </xsl:call-template>


                        <xsl:choose>
                          <xsl:when test="$UnitType='' or $UnitType=692001">
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:if>
                    </td>
                  </tr>
                </xsl:if>

                <xsl:if test="ns3:Component/ns3:LoadBearing/ns3:Wheelbase != ''">
                  <tr>
                    <td>Wheelbase:</td>
                    <td colspan="2">
                      <xsl:choose>
                        <xsl:when test="ns3:Component/ns3:LoadBearing/ns3:Wheelbase != ''">

                          <!--<xsl:value-of select="ns3:Component/ns3:LoadBearing/ns3:Wheelbase"/>-->

                          <xsl:call-template name="parseStringForMeters">
                            <xsl:with-param name="list" select="ns3:Component/ns3:LoadBearing/ns3:Wheelbase"/>
                          </xsl:call-template>

                          <xsl:choose>
                            <xsl:when test="$UnitType='' or $UnitType=692001">
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:choose>
                            <xsl:when test="$UnitType='' or $UnitType=692001">
                              0<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                            </xsl:when>
                            <xsl:otherwise>
                              0<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                  </tr>
                </xsl:if>

                <xsl:if test="ns3:Component/ns3:LoadBearing/ns3:LeftOverhang != ''">
                  <tr>
                    <td>Left overhang:</td>
                    <td colspan="2">
                      <xsl:choose>
                        <xsl:when test="ns3:Component/ns3:LoadBearing/ns3:LeftOverhang != ''">

                          <!--<xsl:value-of select="ns3:Component/ns3:LoadBearing/ns3:LeftOverhang"/>-->

                          <xsl:call-template name="parseStringForMeters">
                            <xsl:with-param name="list" select="ns3:Component/ns3:LoadBearing/ns3:LeftOverhang"/>
                          </xsl:call-template>

                          <xsl:choose>
                            <xsl:when test="$UnitType='' or $UnitType=692001">
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:choose>
                            <xsl:when test="$UnitType='' or $UnitType=692001">
                              0<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                            </xsl:when>
                            <xsl:otherwise>
                              0<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                  </tr>
                </xsl:if>

                <xsl:if test="ns3:Component/ns3:LoadBearing/ns3:RightOverhang != ''">
                  <tr>
                    <td>Right overhang:</td>
                    <td colspan="2">
                      <xsl:choose>
                        <xsl:when test="ns3:Component/ns3:LoadBearing/ns3:RightOverhang != ''">

                          <xsl:call-template name="parseStringForMeters">
                            <xsl:with-param name="list" select="ns3:Component/ns3:LoadBearing/ns3:RightOverhang"/>
                          </xsl:call-template>

                          <xsl:choose>
                            <xsl:when test="$UnitType='' or $UnitType=692001">
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:choose>
                            <xsl:when test="$UnitType='' or $UnitType=692001">
                              0<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                            </xsl:when>
                            <xsl:otherwise>
                              0<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                  </tr>
                </xsl:if>

                <xsl:if test="ns3:Component/ns3:LoadBearing/ns3:FrontOverhang != ''">
                  <tr>
                    <!--TODO 2-->
                    <td>Front overhang:</td>
                    <td colspan="2">
                      <xsl:choose>
                        <xsl:when test="ns3:Component/ns3:LoadBearing/ns3:FrontOverhang != ''">

                          <xsl:call-template name="parseStringForMeters">
                            <xsl:with-param name="list" select="ns3:Component/ns3:LoadBearing/ns3:FrontOverhang"/>
                          </xsl:call-template>

                          <xsl:choose>
                            <xsl:when test="$UnitType='' or $UnitType=692001">
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:choose>
                            <xsl:when test="$UnitType='' or $UnitType=692001">
                              0<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                            </xsl:when>
                            <xsl:otherwise>
                              0<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                  </tr>
                </xsl:if>

                <xsl:if test="ns3:Component/ns3:LoadBearing/ns3:RearOverhang != ''">
                  <tr>
                    <!--TODO 3-->
                    <td>Rear overhang:</td>
                    <td colspan="2">
                      <xsl:choose>
                        <xsl:when test="ns3:Component/ns3:LoadBearing/ns3:RearOverhang != ''">

                          <xsl:call-template name="parseStringForMeters">
                            <xsl:with-param name="list" select="ns3:Component/ns3:LoadBearing/ns3:RearOverhang"/>
                          </xsl:call-template>

                          <xsl:choose>
                            <xsl:when test="$UnitType='' or $UnitType=692001">
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:choose>
                            <xsl:when test="$UnitType='' or $UnitType=692001">
                              0<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                            </xsl:when>
                            <xsl:otherwise>
                              0<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                  </tr>
                </xsl:if>

                <xsl:if test="ns3:Component/ns3:LoadBearing/ns3:GroundClearance != ''">
                  <tr>
                    <!--TODO 5-->
                    <td>Ground clearance:</td>
                    <td colspan="2">
                      <xsl:choose>
                        <xsl:when test="ns3:Component/ns3:LoadBearing/ns3:GroundClearance != ''">

                          <xsl:call-template name="parseStringForMeters">
                            <xsl:with-param name="list" select="ns3:Component/ns3:LoadBearing/ns3:GroundClearance"/>
                          </xsl:call-template>

                          <xsl:choose>
                            <xsl:when test="$UnitType='' or $UnitType=692001">
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:choose>
                            <xsl:when test="$UnitType='' or $UnitType=692001">
                              0<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                            </xsl:when>
                            <xsl:otherwise>
                              0<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                  </tr>
                </xsl:if>

                <xsl:if test="ns3:Component/ns3:LoadBearing/ns3:ReducedGroundClearance != ''">
                  <tr>
                    <!--TODO 6-->
                    <td>Reduced ground clearance:</td>
                    <td colspan="2">
                      <xsl:choose>
                        <xsl:when test="ns3:Component/ns3:LoadBearing/ns3:ReducedGroundClearance != ''">

                          <!--<xsl:value-of select="ns3:Component/ns3:LoadBearing/ns3:ReducedGroundClearance"/>-->

                          <xsl:call-template name="parseStringForMeters">
                            <xsl:with-param name="list" select="ns3:Component/ns3:LoadBearing/ns3:ReducedGroundClearance"/>
                          </xsl:call-template>

                          <xsl:choose>
                            <xsl:when test="$UnitType='' or $UnitType=692001">
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:choose>
                            <xsl:when test="$UnitType='' or $UnitType=692001">
                              0<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                            </xsl:when>
                            <xsl:otherwise>
                              0<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                  </tr>
                </xsl:if>

                <xsl:if test="ns3:Component/ns3:LoadBearing/ns3:OutsideTrack != ''">
                  <tr>
                    <!--TODO 4-->
                    <td>Outside track:</td>
                    <td colspan="2">
                      <xsl:choose>
                        <xsl:when test="ns3:Component/ns3:LoadBearing/ns3:OutsideTrack != ''">

                          <xsl:call-template name="parseStringForMeters">
                            <xsl:with-param name="list" select="ns3:Component/ns3:LoadBearing/ns3:OutsideTrack"/>
                          </xsl:call-template>


                          <xsl:choose>
                            <xsl:when test="$UnitType='' or $UnitType=692001">
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:choose>
                            <xsl:when test="$UnitType='' or $UnitType=692001">
                              0<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                            </xsl:when>
                            <xsl:otherwise>
                              0<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>ft
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                  </tr>
                </xsl:if>

              </table>
              <br/>
            </xsl:if>
          </xsl:for-each>
        </xsl:for-each>
        <br/>

        <xsl:if test="$DocType = 'PDF' or $DocType = 'MovementPDF'">
          <xsl:for-each select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart">
            <b>
              LEG
              <xsl:value-of select="ns2:LegNumber" /><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>-
              <xsl:choose>
                <xsl:when test="contains(ns2:RoadPart/ns2:StartPointListPosition/ns2:StartPoint/ns5:Description, '##**##')">
                  <xsl:value-of select="substring-after(ns2:RoadPart/ns2:StartPointListPosition/ns2:StartPoint/ns5:Description, '##**##')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="ns2:RoadPart/ns2:StartPointListPosition/ns2:StartPoint/ns5:Description"/>
                </xsl:otherwise>
              </xsl:choose>
              to <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
              <xsl:choose>
                <xsl:when test="contains(ns2:RoadPart/ns2:EndPointListPosition/ns2:EndPoint/ns5:Description, '##**##')">
                  <xsl:value-of select="substring-after(ns2:RoadPart/ns2:EndPointListPosition/ns2:EndPoint/ns5:Description, '##**##')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="ns2:RoadPart/ns2:EndPointListPosition/ns2:EndPoint/ns5:Description"/>
                </xsl:otherwise>
              </xsl:choose>
            </b>
            <br/>
            <br/>
            <table border = "0" style="width:100%;" cellspacing="5" cellpadding="5">

              <tr>
                <th style="width:37%;">
                  <b>Route directions</b>
                </th>
                <th style="width:63%;">
                  <b>Cautions</b>
                </th>
              </tr>
              <tr>
                <td    colspan="3" style="border-top: 1px solid black">
                </td>
              </tr>
              <xsl:for-each select="ns2:RoadPart/ns6:DrivingInstructions/ns6:SubPartListPosition/ns6:SubPart">

                <xsl:variable name="set" select="ns6:AlternativeListPosition" />
                <xsl:variable name="recCount" select="count($set)" />

                <xsl:for-each select="ns6:AlternativeListPosition">
                  <xsl:variable name="AlternativeNo" select="ns6:Alternative/ns6:AlternativeDescription/ns6:AlternativeNo/text()" />
                  <xsl:variable name="startmode" select="(ns6:Alternative/ns6:InstructionListPosition/ns6:Instruction/ns6:NoteListPosition/ns6:Note/ns6:Content/ns6:RoutePoint/@PointType)[1]" />
                  <xsl:variable name="endmode" select="(ns6:Alternative/ns6:InstructionListPosition/ns6:Instruction/ns6:NoteListPosition/ns6:Note/ns6:Content/ns6:RoutePoint/@PointType)[position()=last()]" />

                  <xsl:variable name="mode">
                    <xsl:choose>
                      <xsl:when test="$startmode='start'">
                        <xsl:value-of select="'start'"/>
                      </xsl:when>
                      <xsl:when test="$endmode='end'">
                        <xsl:value-of select="'end'"/>
                      </xsl:when>
                      <xsl:otherwise>middle</xsl:otherwise>
                    </xsl:choose>
                  </xsl:variable>

                  <xsl:if test="($mode='end' or $mode='middle') and $recCount>1 and position()=1" >
                    <tr>
                      <th colspan="2" align="center">
                        <span>
                          <b>
                            Diverge
                          </b>
                        </span>
                      </th>
                    </tr>
                    <tr>
                      <td    colspan="3" style="border-top: 1px solid black">
                      </td>
                    </tr>
                  </xsl:if>                

                  

                  <xsl:for-each select="ns6:Alternative/ns6:InstructionListPosition">

                    <xsl:if test="$recCount &gt; 1" >

                      <xsl:if test="($mode='start' and position()=1)" >
                        <tr>
                          <th colspan="2" align="center">
                            <span>
                              <b>
                                <xsl:value-of select="concat('Alternative ', 'start', ' # ',$AlternativeNo,':')" />
                              </b>
                            </span>
                          </th>
                        </tr>
                        <tr>
                          <td    colspan="3" style="border-top: 1px solid black">
                          </td>
                        </tr>
                      </xsl:if>

                      <xsl:if test="$mode='middle' and position()=1 and $recCount>1" >
                        <tr>
                          <th colspan="2" align="center">
                            <span>
                              <b>
                                <xsl:value-of select="concat('Alternative ', ' # ',$AlternativeNo,':')" />
                              </b>
                            </span>
                          </th>
                        </tr>
                        <tr>
                          <td    colspan="3" style="border-top: 1px solid black">
                          </td>
                        </tr>
                      </xsl:if>

                    </xsl:if>
                    <tr>
                      <xsl:if test="$UnitType=692001">
                        <xsl:choose>
                          <xsl:when test="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric &lt; 1000 or ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric &lt; 1000">
                            <td>
                              <xsl:choose>
                                <xsl:when test="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Instruction">
                                  <strike>
                                    <xsl:value-of select="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Instruction"/>
                                    <xsl:value-of select="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                                  </strike>
                                </xsl:when>
                                <xsl:when test="ns6:OldInstruction">
                                  <b>
                                    <u>
                                      <xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Instruction"/>
                                      <xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                                    </u>
                                  </b>
                                </xsl:when>
                                <xsl:otherwise>
                                  <!--RM#3646 - Start-->
                                  <!--<xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Instruction"/>
                            <xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m-->
                                  <xsl:choose>
                                    <xsl:when test="contains(ns6:Instruction/ns6:Navigation/ns6:Instruction, '##**##')">
                                      <strike>
                                        <xsl:value-of select="substring-before(ns6:Instruction/ns6:Navigation/ns6:Instruction, '##**##')"/>
                                      </strike>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      <b>
                                        <u>
                                          <xsl:value-of select="substring-after(ns6:Instruction/ns6:Navigation/ns6:Instruction, '##**##')"/>
                                        </u>
                                      </b>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Instruction"/>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                  <xsl:choose>
                                    <xsl:when test="contains(ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric, '##**##')">
                                      <strike>
                                        <xsl:value-of select="substring-before(ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric, '##**##')"/>
                                      </strike>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      <b>
                                        <u>
                                          <xsl:value-of select="substring-after(ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric, '##**##')"/>
                                        </u>
                                      </b>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric"/>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                                  <!--RM#3646 - End-->
                                </xsl:otherwise>
                              </xsl:choose>
                            </td>
                          </xsl:when>
                          <xsl:otherwise>
                            <td>
                              <xsl:choose>
                                <xsl:when test="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Instruction">
                                  <strike>
                                    <xsl:value-of select="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Instruction"/>
                                    <xsl:if test="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric != '' ">
                                      <xsl:variable name="var1" select="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric" />
                                      <xsl:variable name="var2" select="format-number($var1 div 1000, '#.##')"/>
                                      <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                                    </xsl:if>
                                  </strike>
                                </xsl:when>
                                <xsl:when test="ns6:OldInstruction">
                                  <b>
                                    <u>
                                      <xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Instruction"/>
                                    </u>
                                  </b>
                                </xsl:when>
                                <xsl:otherwise>
                                  <xsl:choose>
                                    <xsl:when test="contains(ns6:Instruction/ns6:Navigation/ns6:Instruction, '##**##')">
                                      <strike>
                                        <xsl:value-of select="substring-before(ns6:Instruction/ns6:Navigation/ns6:Instruction, '##**##')"/>
                                      </strike>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      <b>
                                        <u>
                                          <xsl:value-of select="substring-after(ns6:Instruction/ns6:Navigation/ns6:Instruction, '##**##')"/>
                                        </u>
                                      </b>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Instruction"/>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                  <!--RM#3646 - End-->
                                </xsl:otherwise>
                              </xsl:choose>
                              <xsl:if test="ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric != '' ">
                                <!--RM#3646 - Start-->
                                <xsl:choose>
                                  <xsl:when test="contains(ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric, '##**##')">
                                    <strike>
                                      <xsl:variable name="var1" select="substring-before(ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric, '##**##')" />
                                      <xsl:variable name="var2" select="format-number($var1 div 1000, '#.##')"/>
                                      <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                                    </strike>
                                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    <b>
                                      <u>
                                        <xsl:variable name="var1" select="substring-after(ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric, '##**##')" />
                                        <xsl:variable name="var2" select="format-number($var1 div 1000, '#.##')"/>
                                        <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                                      </u>
                                    </b>
                                  </xsl:when>
                                  <xsl:otherwise>
                                    <xsl:variable name="var1" select="ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric" />
                                    <xsl:variable name="var2" select="format-number($var1 div 1000, '#.##')"/>
                                    <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                                  </xsl:otherwise>
                                </xsl:choose>
                                <!--RM#3646 - End-->
                              </xsl:if>
                            </td>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:if>

                      <xsl:if test="$UnitType=692002">
                        <xsl:choose>
                          <xsl:when test="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial &lt; 1760 or ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial &lt; 1760">
                            <td>
                              <xsl:choose>
                                <xsl:when test="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Instruction">
                                  <strike>
                                    <xsl:value-of select="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Instruction"/>
                                    <xsl:value-of select="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>yards
                                  </strike>
                                </xsl:when>
                                <xsl:when test="ns6:OldInstruction">
                                  <b>
                                    <u>
                                      <xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Instruction"/>
                                      <xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                                    </u>
                                  </b>
                                </xsl:when>
                                <xsl:otherwise>
                                  <!--RM#3646 - Start-->
                                  <!--<xsl:value-of select="ns6:Navigation/ns6:Instruction"/>-->
                                  <xsl:choose>
                                    <xsl:when test="contains(ns6:Instruction/ns6:Navigation/ns6:Instruction, '##**##')">
                                      <strike>
                                        <xsl:value-of select="substring-before(ns6:Instruction/ns6:Navigation/ns6:Instruction, '##**##')"/>
                                      </strike>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      <b>
                                        <u>
                                          <xsl:value-of select="substring-after(ns6:Instruction/ns6:Navigation/ns6:Instruction, '##**##')"/>
                                        </u>
                                      </b>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Instruction"/>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                  <!--RM#3646 - End-->
                                </xsl:otherwise>
                              </xsl:choose>
                              <!--RM#3646 - Start-->
                              <!--<xsl:value-of select="ns6:Navigation/ns6:Distance/ns6:DisplayImperial"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>yards-->
                              <xsl:choose>
                                <xsl:when test="contains(ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial, '##**##')">
                                  <strike>
                                    <xsl:value-of select="substring-before(ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial, '##**##')"/>
                                  </strike>
                                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                  <b>
                                    <u>
                                      <xsl:value-of select="substring-after(ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial, '##**##')"/>
                                    </u>
                                  </b>
                                </xsl:when>
                                <xsl:otherwise>
                                  <xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial"/>
                                </xsl:otherwise>
                              </xsl:choose>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>yards
                              <!--RM#3646 - End-->
                            </td>
                          </xsl:when>
                          <xsl:otherwise>
                            <td>
                              <xsl:choose>
                                <xsl:when test="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Instruction">
                                  <strike>
                                    <xsl:value-of select="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Instruction"/>
                                    <xsl:if test="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial != '' ">
                                      <xsl:variable name="var1" select="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial" />
                                      <xsl:variable name="var2" select="format-number($var1 div 1760, '#.##')"/>
                                      <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                                    </xsl:if>
                                  </strike>
                                </xsl:when>
                                <xsl:when test="ns6:OldInstruction">
                                  <b>
                                    <u>
                                      <xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Instruction"/>
                                    </u>
                                  </b>
                                </xsl:when>
                                <xsl:otherwise>
                                  <!--RM#3646 - Start-->
                                  <!--<xsl:value-of select="ns6:Navigation/ns6:Instruction"/>-->
                                  <xsl:choose>
                                    <xsl:when test="contains(ns6:Instruction/ns6:Navigation/ns6:Instruction, '##**##')">
                                      <strike>
                                        <xsl:value-of select="substring-before(ns6:Instruction/ns6:Navigation/ns6:Instruction, '##**##')"/>
                                      </strike>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      <b>
                                        <u>
                                          <xsl:value-of select="substring-after(ns6:Instruction/ns6:Navigation/ns6:Instruction, '##**##')"/>
                                        </u>
                                      </b>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Instruction"/>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                  <!--RM#3646 - End-->
                                </xsl:otherwise>
                              </xsl:choose>
                              <!--RM#3646 - Start-->
                              <xsl:choose>
                                <xsl:when test="contains(ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial, '##**##')">
                                  <strike>
                                    <xsl:variable name="var1" select="substring-before(ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial, '##**##')" />
                                    <xsl:variable name="var2" select="format-number($var1 div 1760, '#.##')"/>
                                    <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                                  </strike>
                                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                  <b>
                                    <u>
                                      <xsl:variable name="var1" select="substring-after(ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial, '##**##')" />
                                      <xsl:variable name="var2" select="format-number($var1 div 1760, '#.##')"/>
                                      <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                                    </u>
                                  </b>
                                </xsl:when>
                                <xsl:otherwise>
                                  <xsl:if test="ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial != '' ">
                                    <xsl:variable name="var1" select="ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial" />
                                    <xsl:variable name="var2" select="format-number($var1 div 1760, '#.##')"/>
                                    <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                                  </xsl:if>
                                </xsl:otherwise>
                              </xsl:choose>
                              <!--RM#3646 - End-->
                            </td>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:if>

                      <td valign ="top">

                        <xsl:choose>
                          <xsl:when test="ns6:Instruction/ns6:NoteListPosition/ns6:Note">
                            <xsl:for-each select="ns6:Instruction/ns6:NoteListPosition/ns6:Note">

                              <xsl:if test="ns6:Content/ns6:RoutePoint/@PointType = 'start'">
                                <b>Start point further details:</b>
                              </xsl:if>

                              <xsl:if test="ns6:Content/ns6:RoutePoint/@PointType = 'end'">
                                <b>End point further details:</b>
                              </xsl:if>

                              <xsl:choose>
                                <!--Code start here for PDF RM#4613-->
                                <xsl:when test="ns6:Content/ns6:Caution/ns10:Action/ns10:Standard != '' or 
                                ns6:Content/ns6:Caution/ns10:Action/ns10:SpecificAction != ''">

                                  <xsl:if test="ns6:Content/ns6:Caution/ns10:Action/ns10:Standard != ''">
                                    <xsl:choose>
                                      <xsl:when test="contains(ns6:Content/ns6:Caution/ns10:Action/ns10:Standard, '##**##')">
                                        <strike>
                                          <xsl:value-of select="substring-before(ns6:Content/ns6:Caution/ns10:Action/ns10:Standard, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </strike>
                                        <u>
                                          <b>
                                            <xsl:value-of select="substring-after(ns6:Content/ns6:Caution/ns10:Action/ns10:Standard, '##**##')" />
                                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                          </b>
                                        </u>
                                      </xsl:when>

                                      <xsl:otherwise>
                                        <xsl:value-of select="ns6:Content/ns6:Caution/ns10:Action/ns10:Standard"/>
                                      </xsl:otherwise>

                                    </xsl:choose>
                                  </xsl:if>

                                  <xsl:if test="ns6:Content/ns6:Caution/ns10:Action/ns10:SpecificAction != ''">
                                    <xsl:choose>
                                      <xsl:when test="contains(ns6:Content/ns6:Caution/ns10:Action/ns10:SpecificAction, '##**##')">
                                        <strike>
                                          <xsl:value-of select="substring-before(ns6:Content/ns6:Caution/ns10:Action/ns10:SpecificAction, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </strike>
                                        <u>
                                          <b>
                                            <xsl:value-of select="substring-after(ns6:Content/ns6:Caution/ns10:Action/ns10:SpecificAction, '##**##')" />
                                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                          </b>
                                        </u>
                                      </xsl:when>

                                      <xsl:otherwise>
                                        <xsl:value-of select="ns6:Content/ns6:Caution/ns10:Action/ns10:SpecificAction"/>
                                      </xsl:otherwise>
                                    </xsl:choose>
                                  </xsl:if>

                                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text> <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>

                                  <xsl:if test="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN !=''">
                                    <xsl:choose>
                                      <xsl:when test="contains(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN, '##**##')">
                                        <strike>
                                          <xsl:value-of select="substring-before(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </strike>
                                        <u>
                                          <b>
                                            <xsl:value-of select="substring-after(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN, '##**##')" />
                                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                          </b>
                                        </u>
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </xsl:when>

                                      <xsl:otherwise>
                                        <xsl:value-of select="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN"/>
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </xsl:otherwise>
                                    </xsl:choose>
                                  </xsl:if>

                                  <xsl:if test="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN !=''">
                                    <xsl:choose>
                                      <xsl:when test="contains(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN, '##**##')">
                                        <strike>
                                          <xsl:value-of select="substring-before(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </strike>
                                        <u>
                                          <b>
                                            <xsl:value-of select="substring-after(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN, '##**##')" />
                                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                          </b>
                                        </u>
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </xsl:when>

                                      <xsl:otherwise>
                                        <xsl:value-of select="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN"/>
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </xsl:otherwise>
                                    </xsl:choose>
                                  </xsl:if>


                                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                  <br/>
                                  <br/>
                                  CAUTION: After<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>

                                  <xsl:choose>
                                    <xsl:when test="$UnitType='' or $UnitType=692001">

                                      <xsl:choose>
                                        <xsl:when test="ns6:EncounteredAt/ns6:DisplayMetric != ''">
                                          <xsl:choose>
                                            <xsl:when test="ns6:EncounteredAt/ns6:DisplayMetric &lt; 1000">

                                              <xsl:choose>
                                                <xsl:when test="contains(ns6:EncounteredAt/ns6:DisplayMetric, '##**##')">
                                                  <strike>
                                                    <xsl:value-of select="substring-before(ns6:EncounteredAt/ns6:DisplayMetric, '##**##')" />m <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                                  </strike>
                                                  <u>
                                                    <b>
                                                      <xsl:value-of select="substring-after(ns6:EncounteredAt/ns6:DisplayMetric, '##**##')" />m <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                                    </b>
                                                  </u>
                                                </xsl:when>

                                                <xsl:otherwise>
                                                  <xsl:value-of select="ns6:EncounteredAt/ns6:DisplayMetric"/>m
                                                </xsl:otherwise>
                                              </xsl:choose>

                                            </xsl:when>
                                            <xsl:otherwise>
                                              <xsl:variable name="var1" select="ns6:EncounteredAt/ns6:DisplayMetric" />
                                              <xsl:variable name="var2" select="format-number($var1 div 1000, '#.##')"/>


                                              <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                                            </xsl:otherwise>
                                          </xsl:choose>
                                        </xsl:when>
                                        <xsl:otherwise>0 m</xsl:otherwise>
                                      </xsl:choose>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:choose>
                                        <xsl:when test="ns6:EncounteredAt/ns6:DisplayImperial != ''">
                                          <xsl:choose>
                                            <xsl:when test="ns6:EncounteredAt/ns6:DisplayImperial &lt; 1760">

                                              <xsl:choose>
                                                <xsl:when test="contains(ns6:EncounteredAt/ns6:DisplayImperial, '##**##')">
                                                  <strike>
                                                    <xsl:value-of select="substring-before(ns6:EncounteredAt/ns6:DisplayImperial, '##**##')" />yards <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                                  </strike>
                                                  <u>
                                                    <b>
                                                      <xsl:value-of select="substring-after(ns6:EncounteredAt/ns6:DisplayImperial, '##**##')" />yards <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                                    </b>
                                                  </u>
                                                </xsl:when>

                                                <xsl:otherwise>
                                                  <xsl:value-of select="ns6:EncounteredAt/ns6:DisplayImperial"/>yards
                                                </xsl:otherwise>
                                              </xsl:choose>

                                            </xsl:when>
                                            <xsl:otherwise>
                                              <xsl:variable name="var1" select="ns6:EncounteredAt/ns6:DisplayImperial" />
                                              <xsl:variable name="var2" select="format-number($var1 div 1760, '#.##')"/>
                                              <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                                            </xsl:otherwise>
                                          </xsl:choose>
                                        </xsl:when>
                                        <xsl:otherwise>0 yards</xsl:otherwise>
                                      </xsl:choose>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                  <br/>
                                  <br/>


                                  <xsl:if test="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN !=''">
                                    <xsl:choose>
                                      <xsl:when test="contains(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN, '##**##')">
                                        <strike>
                                          <xsl:value-of select="substring-before(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </strike>
                                        <u>
                                          <b>
                                            <xsl:value-of select="substring-after(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN, '##**##')" />
                                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                          </b>
                                        </u>
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </xsl:when>

                                      <xsl:otherwise>
                                        <xsl:value-of select="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN"/>
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </xsl:otherwise>
                                    </xsl:choose>
                                  </xsl:if>

                                  <xsl:if test="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN !=''">
                                    <xsl:choose>
                                      <xsl:when test="contains(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN, '##**##')">
                                        <strike>
                                          <xsl:value-of select="substring-before(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </strike>
                                        <u>
                                          <b>
                                            <xsl:value-of select="substring-after(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN, '##**##')" />
                                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                          </b>
                                        </u>
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </xsl:when>

                                      <xsl:otherwise>
                                        <xsl:value-of select="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN"/>
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </xsl:otherwise>
                                    </xsl:choose>
                                  </xsl:if>

                                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>

                                  <xsl:choose>
                                    <xsl:when test="contains(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ConstraintName, '##**##')">
                                      <strike>
                                        <xsl:value-of select="substring-before(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ConstraintName, '##**##')" />
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </strike>
                                      <u>
                                        <b>
                                          <xsl:value-of select="substring-after(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ConstraintName, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </b>
                                      </u>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:when>

                                    <xsl:otherwise>
                                      <xsl:value-of select="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ConstraintName"/>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:otherwise>
                                  </xsl:choose>

                                  <xsl:choose>
                                    <xsl:when test="contains(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:StructureName, '##**##')">
                                      <strike>
                                        <xsl:value-of select="substring-before(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:StructureName, '##**##')" />
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </strike>
                                      <u>
                                        <b>
                                          <xsl:value-of select="substring-after(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:StructureName, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </b>
                                      </u>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:when>

                                    <xsl:otherwise>
                                      <xsl:value-of select="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:StructureName"/>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:otherwise>
                                  </xsl:choose>


                                  <BR/>
                                  <div align="left" valign ="top">
                                    <b>
                                      GridRef:
                                    </b>
                                    <xsl:choose>
                                      <xsl:when test="ns6:GridReference/ns8:X != ''">

                                        <xsl:choose>
                                          <xsl:when test="contains(ns6:GridReference/ns8:X, '##**##')">
                                            <strike>
                                              <xsl:value-of select="substring-before(ns6:GridReference/ns8:X, '##**##')" />
                                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                            </strike>
                                            <u>
                                              <b>
                                                <xsl:value-of select="substring-after(ns6:GridReference/ns8:X, '##**##')" />
                                                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                              </b>
                                            </u>
                                          </xsl:when>
                                          <xsl:otherwise>
                                            <xsl:value-of select="ns6:GridReference/ns8:X"/>
                                          </xsl:otherwise>
                                        </xsl:choose>

                                      </xsl:when>
                                      <xsl:otherwise>
                                        0
                                      </xsl:otherwise>
                                    </xsl:choose>,
                                    <xsl:choose>
                                      <xsl:when test="ns6:GridReference/ns8:Y != ''">

                                        <xsl:choose>
                                          <xsl:when test="contains(ns6:GridReference/ns8:Y, '##**##')">
                                            <strike>
                                              <xsl:value-of select="substring-before(ns6:GridReference/ns8:Y, '##**##')" />
                                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                            </strike>
                                            <u>
                                              <b>
                                                <xsl:value-of select="substring-after(ns6:GridReference/ns8:Y, '##**##')" />
                                                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                              </b>
                                            </u>
                                          </xsl:when>
                                          <xsl:otherwise>
                                            <xsl:value-of select="ns6:GridReference/ns8:Y"/>
                                          </xsl:otherwise>
                                        </xsl:choose>
                                      </xsl:when>
                                      <xsl:otherwise>
                                        0
                                      </xsl:otherwise>
                                    </xsl:choose>
                                  </div>
                                  <xsl:if test="ns6:Content/ns6:MotorwayCaution/ns6:Description != ''">
                                    <b>Apply motorway caution</b>
                                  </xsl:if>

                                  <xsl:choose>
                                    <xsl:when test="contains(ns6:Content/ns6:Annotation/ns9:Text, '##**##')">
                                      <strike>
                                        <xsl:value-of select="substring-before(ns6:Content/ns6:Annotation/ns9:Text, '##**##')" />
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </strike>
                                      <u>
                                        <b>
                                          <xsl:value-of select="substring-after(ns6:Content/ns6:Annotation/ns9:Text, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </b>
                                      </u>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:when>

                                    <xsl:otherwise>
                                      <xsl:value-of select="ns6:Content/ns6:Annotation/ns9:Text"/>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:otherwise>
                                  </xsl:choose>


                                </xsl:when>

                                <xsl:when test="ns6:Content/ns6:Annotation != ''">
                                  <!--Code start 30 june-->
                                  <br/>
                                  After<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                  <xsl:choose>
                                    <xsl:when test="$UnitType='' or $UnitType=692001">

                                      <xsl:choose>
                                        <xsl:when test="ns6:EncounteredAt/ns6:DisplayMetric != ''">
                                          <xsl:choose>
                                            <xsl:when test="ns6:EncounteredAt/ns6:DisplayMetric &lt; 1000">
                                              <xsl:value-of select="ns6:EncounteredAt/ns6:DisplayMetric"/> m
                                            </xsl:when>
                                            <xsl:otherwise>
                                              <xsl:variable name="var1" select="ns6:EncounteredAt/ns6:DisplayMetric" />
                                              <xsl:variable name="var2" select="format-number($var1 div 1000, '#.##')"/>
                                              <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                                            </xsl:otherwise>
                                          </xsl:choose>
                                        </xsl:when>
                                        <xsl:otherwise>0 m</xsl:otherwise>
                                      </xsl:choose>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:choose>
                                        <xsl:when test="ns6:EncounteredAt/ns6:DisplayImperial != ''">
                                          <xsl:choose>
                                            <xsl:when test="ns6:EncounteredAt/ns6:DisplayImperial &lt; 1760">
                                              <xsl:value-of select="ns6:EncounteredAt/ns6:DisplayImperial"/> yards
                                            </xsl:when>
                                            <xsl:otherwise>
                                              <xsl:variable name="var1" select="ns6:EncounteredAt/ns6:DisplayImperial" />
                                              <xsl:variable name="var2" select="format-number($var1 div 1760, '#.##')"/>
                                              <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                                            </xsl:otherwise>
                                          </xsl:choose>
                                        </xsl:when>
                                        <xsl:otherwise>0 yards</xsl:otherwise>
                                      </xsl:choose>
                                    </xsl:otherwise>
                                  </xsl:choose>

                                  <br/>
                                  <br/>
                                  <div align="left" valign ="top">
                                    <b>
                                      GridRef:
                                    </b>
                                    <xsl:choose>
                                      <xsl:when test="ns6:GridReference/ns8:X != ''">

                                        <xsl:choose>
                                          <xsl:when test="contains(ns6:GridReference/ns8:X, '##**##')">
                                            <strike>
                                              <xsl:value-of select="substring-before(ns6:GridReference/ns8:X, '##**##')" />
                                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                            </strike>
                                            <u>
                                              <b>
                                                <xsl:value-of select="substring-after(ns6:GridReference/ns8:X, '##**##')" />
                                                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                              </b>
                                            </u>
                                          </xsl:when>
                                          <xsl:otherwise>
                                            <xsl:value-of select="ns6:GridReference/ns8:X"/>
                                          </xsl:otherwise>
                                        </xsl:choose>

                                      </xsl:when>
                                      <xsl:otherwise>
                                        0
                                      </xsl:otherwise>
                                    </xsl:choose>,
                                    <xsl:choose>
                                      <xsl:when test="ns6:GridReference/ns8:Y != ''">

                                        <xsl:choose>
                                          <xsl:when test="contains(ns6:GridReference/ns8:Y, '##**##')">
                                            <strike>
                                              <xsl:value-of select="substring-before(ns6:GridReference/ns8:Y, '##**##')" />
                                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                            </strike>
                                            <u>
                                              <b>
                                                <xsl:value-of select="substring-after(ns6:GridReference/ns8:Y, '##**##')" />
                                                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                              </b>
                                            </u>
                                          </xsl:when>
                                          <xsl:otherwise>
                                            <xsl:value-of select="ns6:GridReference/ns8:Y"/>
                                          </xsl:otherwise>
                                        </xsl:choose>
                                      </xsl:when>
                                      <xsl:otherwise>
                                        0
                                      </xsl:otherwise>
                                    </xsl:choose>
                                  </div>

                                  <br/>

                                  <xsl:variable name="AnnotationType" select="ns6:Content/ns6:Annotation/@AnnotationType" />
                                  <xsl:if test="$AnnotationType!='generic'" >
                                    <xsl:if test="$AnnotationType='special manouevre'" >
                                      <b>Special manouevre:</b>
                                    </xsl:if>
                                    <xsl:if test="$AnnotationType='caution'" >
                                      <b>Caution:</b>
                                    </xsl:if>
                                  </xsl:if>

                                  <xsl:choose>
                                    <xsl:when test="contains(ns6:Content/ns6:Annotation/ns9:Text, '##**##')">
                                      <strike>
                                        <xsl:value-of select="substring-before(ns6:Content/ns6:Annotation/ns9:Text, '##**##')" />
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </strike>
                                      <u>
                                        <b>
                                          <xsl:value-of select="substring-after(ns6:Content/ns6:Annotation/ns9:Text, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </b>
                                      </u>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:when>

                                    <xsl:otherwise>
                                      <xsl:value-of select="ns6:Content/ns6:Annotation/ns9:Text"/>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                  <!--Code end 30 june-->
                                </xsl:when>

                                <xsl:when test="ns6:Content/ns6:Caution/ns10:CautionedEntity != ''">
                                  <br/>
                                  CAUTION: After<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>

                                  <xsl:choose>
                                    <xsl:when test="$UnitType='' or $UnitType=692001">

                                      <xsl:choose>
                                        <xsl:when test="ns6:EncounteredAt/ns6:DisplayMetric != ''">
                                          <xsl:choose>
                                            <xsl:when test="ns6:EncounteredAt/ns6:DisplayMetric &lt; 1000">

                                              <xsl:choose>
                                                <xsl:when test="contains(ns6:EncounteredAt/ns6:DisplayMetric, '##**##')">
                                                  <strike>
                                                    <xsl:value-of select="substring-before(ns6:EncounteredAt/ns6:DisplayMetric, '##**##')" />m <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                                  </strike>
                                                  <u>
                                                    <b>
                                                      <xsl:value-of select="substring-after(ns6:EncounteredAt/ns6:DisplayMetric, '##**##')" />m <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                                    </b>
                                                  </u>
                                                </xsl:when>

                                                <xsl:otherwise>
                                                  <xsl:value-of select="ns6:EncounteredAt/ns6:DisplayMetric"/>m
                                                </xsl:otherwise>
                                              </xsl:choose>

                                            </xsl:when>
                                            <xsl:otherwise>
                                              <xsl:variable name="var1" select="ns6:EncounteredAt/ns6:DisplayMetric" />
                                              <xsl:variable name="var2" select="format-number($var1 div 1000, '#.##')"/>


                                              <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                                            </xsl:otherwise>
                                          </xsl:choose>
                                        </xsl:when>
                                        <xsl:otherwise>0 m</xsl:otherwise>
                                      </xsl:choose>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:choose>
                                        <xsl:when test="ns6:EncounteredAt/ns6:DisplayImperial != ''">
                                          <xsl:choose>
                                            <xsl:when test="ns6:EncounteredAt/ns6:DisplayImperial &lt; 1760">

                                              <xsl:choose>
                                                <xsl:when test="contains(ns6:EncounteredAt/ns6:DisplayImperial, '##**##')">
                                                  <strike>
                                                    <xsl:value-of select="substring-before(ns6:EncounteredAt/ns6:DisplayImperial, '##**##')" />yards <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                                  </strike>
                                                  <u>
                                                    <b>
                                                      <xsl:value-of select="substring-after(ns6:EncounteredAt/ns6:DisplayImperial, '##**##')" />yards <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                                    </b>
                                                  </u>
                                                </xsl:when>

                                                <xsl:otherwise>
                                                  <xsl:value-of select="ns6:EncounteredAt/ns6:DisplayImperial"/>yards
                                                </xsl:otherwise>
                                              </xsl:choose>

                                            </xsl:when>
                                            <xsl:otherwise>
                                              <xsl:variable name="var1" select="ns6:EncounteredAt/ns6:DisplayImperial" />
                                              <xsl:variable name="var2" select="format-number($var1 div 1760, '#.##')"/>
                                              <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                                            </xsl:otherwise>
                                          </xsl:choose>
                                        </xsl:when>
                                        <xsl:otherwise>0 yards</xsl:otherwise>
                                      </xsl:choose>
                                    </xsl:otherwise>
                                  </xsl:choose>

                                  <br/>
                                  <br/>

                                  <xsl:if test="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN !=''">
                                    <xsl:choose>
                                      <xsl:when test="contains(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN, '##**##')">
                                        <strike>
                                          <xsl:value-of select="substring-before(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </strike>
                                        <u>
                                          <b>
                                            <xsl:value-of select="substring-after(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN, '##**##')" />
                                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                          </b>
                                        </u>
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </xsl:when>

                                      <xsl:otherwise>
                                        <xsl:value-of select="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN"/>
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </xsl:otherwise>
                                    </xsl:choose>
                                  </xsl:if>

                                  <xsl:if test="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN !=''">
                                    <xsl:choose>
                                      <xsl:when test="contains(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN, '##**##')">
                                        <strike>
                                          <xsl:value-of select="substring-before(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </strike>
                                        <u>
                                          <b>
                                            <xsl:value-of select="substring-after(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN, '##**##')" />
                                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                          </b>
                                        </u>
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </xsl:when>

                                      <xsl:otherwise>
                                        <xsl:value-of select="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN"/>
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </xsl:otherwise>
                                    </xsl:choose>
                                  </xsl:if>

                                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>

                                  <!--For structure name start-->
                                  <xsl:choose>
                                    <xsl:when test="contains(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ConstraintName, '##**##')">
                                      <strike>
                                        <xsl:value-of select="substring-before(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ConstraintName, '##**##')" />
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </strike>
                                      <u>
                                        <b>
                                          <xsl:value-of select="substring-after(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ConstraintName, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </b>
                                      </u>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:when>

                                    <xsl:otherwise>
                                      <xsl:value-of select="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ConstraintName"/>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:otherwise>
                                  </xsl:choose>

                                  <xsl:choose>
                                    <xsl:when test="contains(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:StructureName, '##**##')">
                                      <strike>
                                        <xsl:value-of select="substring-before(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:StructureName, '##**##')" />
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </strike>
                                      <u>
                                        <b>
                                          <xsl:value-of select="substring-after(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:StructureName, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </b>
                                      </u>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:when>

                                    <xsl:otherwise>
                                      <xsl:value-of select="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:StructureName"/>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                  <!--End-->

                                  <br/>
                                  <br/>
                                  <div align="left" valign ="top">
                                    <b>
                                      GridRef:
                                    </b>
                                    <xsl:choose>
                                      <xsl:when test="ns6:GridReference/ns8:X != ''">

                                        <xsl:choose>
                                          <xsl:when test="contains(ns6:GridReference/ns8:X, '##**##')">
                                            <strike>
                                              <xsl:value-of select="substring-before(ns6:GridReference/ns8:X, '##**##')" />
                                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                            </strike>
                                            <u>
                                              <b>
                                                <xsl:value-of select="substring-after(ns6:GridReference/ns8:X, '##**##')" />
                                                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                              </b>
                                            </u>
                                          </xsl:when>
                                          <xsl:otherwise>
                                            <xsl:value-of select="ns6:GridReference/ns8:X"/>,
                                          </xsl:otherwise>
                                        </xsl:choose>

                                      </xsl:when>
                                      <xsl:otherwise>
                                        0
                                      </xsl:otherwise>
                                    </xsl:choose>,
                                    <xsl:choose>
                                      <xsl:when test="ns6:GridReference/ns8:Y != ''">

                                        <xsl:choose>
                                          <xsl:when test="contains(ns6:GridReference/ns8:Y, '##**##')">
                                            <strike>
                                              <xsl:value-of select="substring-before(ns6:GridReference/ns8:Y, '##**##')" />
                                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                            </strike>
                                            <u>
                                              <b>
                                                <xsl:value-of select="substring-after(ns6:GridReference/ns8:Y, '##**##')" />
                                                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                              </b>
                                            </u>
                                          </xsl:when>
                                          <xsl:otherwise>
                                            <xsl:value-of select="ns6:GridReference/ns8:Y"/>,
                                          </xsl:otherwise>
                                        </xsl:choose>
                                      </xsl:when>
                                      <xsl:otherwise>
                                        0
                                      </xsl:otherwise>
                                    </xsl:choose>
                                  </div>

                                </xsl:when>

                                <xsl:otherwise>
                                  <div align="left" valign ="top">
                                    <b>
                                      <xsl:if test="ns6:GridReference/ns8:X != '' or ns6:GridReference/ns8:Y != ''">
                                        <br/>
                                        GridRef:
                                      </xsl:if>
                                    </b>
                                    <xsl:choose>

                                      <xsl:when test="ns6:GridReference/ns8:X != ''">

                                        <xsl:choose>
                                          <xsl:when test="contains(ns6:GridReference/ns8:X, '##**##')">
                                            <strike>
                                              <xsl:value-of select="substring-before(ns6:GridReference/ns8:X, '##**##')" />
                                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                            </strike>
                                            <u>
                                              <b>
                                                <xsl:value-of select="substring-after(ns6:GridReference/ns8:X, '##**##')" />, <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                              </b>
                                            </u>
                                          </xsl:when>
                                          <xsl:otherwise>
                                            <xsl:value-of select="ns6:GridReference/ns8:X"/>,
                                          </xsl:otherwise>
                                        </xsl:choose>


                                      </xsl:when>
                                      <xsl:otherwise>

                                      </xsl:otherwise>
                                    </xsl:choose>
                                    <xsl:choose>
                                      <xsl:when test="ns6:GridReference/ns8:Y != ''">

                                        <xsl:choose>
                                          <xsl:when test="contains(ns6:GridReference/ns8:Y, '##**##')">
                                            <strike>
                                              <xsl:value-of select="substring-before(ns6:GridReference/ns8:Y, '##**##')" />
                                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                            </strike>
                                            <u>
                                              <b>
                                                <xsl:value-of select="substring-after(ns6:GridReference/ns8:Y, '##**##')" />
                                                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                              </b>
                                            </u>
                                          </xsl:when>
                                          <xsl:otherwise>
                                            <xsl:value-of select="ns6:GridReference/ns8:Y"/>
                                          </xsl:otherwise>
                                        </xsl:choose>


                                      </xsl:when>

                                      <xsl:otherwise>

                                      </xsl:otherwise>
                                    </xsl:choose>
                                  </div>

                                  <xsl:if test="ns6:Content/ns6:MotorwayCaution/ns6:Description != ''">
                                    <b>Apply motorway caution</b>
                                  </xsl:if>

                                  <xsl:choose>
                                    <xsl:when test="contains(ns6:Content/ns6:Annotation/ns9:Text, '##**##')">
                                      <strike>
                                        <xsl:value-of select="substring-before(ns6:Content/ns6:Annotation/ns9:Text, '##**##')" />
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </strike>
                                      <u>
                                        <b>
                                          <xsl:value-of select="substring-after(ns6:Content/ns6:Annotation/ns9:Text, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </b>
                                      </u>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:when>

                                    <xsl:otherwise>
                                      <xsl:value-of select="ns6:Content/ns6:Annotation/ns9:Text"/>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                </xsl:otherwise>

                              </xsl:choose>
                              <!--Code End here for PDF RM#4613-->

                              <xsl:call-template name="parseString">
                                <xsl:with-param name="list" select="ns6:Content/ns6:RoutePoint/ns6:Description"/>
                              </xsl:call-template>
                              <xsl:call-template name="parseString">
                                <xsl:with-param name="list" select="ns6:Contact/ns12:FullName"/>
                              </xsl:call-template>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                              <xsl:call-template name="parseString">
                                <xsl:with-param name="list" select="ns6:Contact/ns12:OrganisationName"/>
                              </xsl:call-template>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                              <xsl:call-template name="parseString">
                                <xsl:with-param name="list" select="ns6:Contact/ns12:Address"/>
                              </xsl:call-template>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                              <xsl:call-template name="parseString">
                                <xsl:with-param name="list" select="ns6:Contact/ns12:TelephoneNumber"/>
                              </xsl:call-template>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                              <xsl:call-template name="parseString">
                                <xsl:with-param name="list" select="ns6:Contact/ns12:FaxNumber"/>
                              </xsl:call-template>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                              <xsl:call-template name="parseString">
                                <xsl:with-param name="list" select="ns6:Contact/ns12:EmailAddress"/>
                              </xsl:call-template>

                              <xsl:if test="position() != last()">
                                <br/>
                                <table  width="100%" cellpadding="0" cellspacing="0">
                                  <tr>
                                    <td style="border-top: 1px solid black"></td>
                                  </tr>
                                </table>

                              </xsl:if>

                            </xsl:for-each>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                          </xsl:otherwise>
                        </xsl:choose>
                      </td>
                    </tr>
                    <tr>
                      <td    colspan="3" style="border-top: 1px solid black">
                      </td>
                    </tr>
                    <xsl:if test="$recCount &gt; 1" >
                      <xsl:if test="($mode='end' and position()=last())" >
                        <tr>
                          <th colspan="2" align="center">
                            <span>
                              <b>
                                <xsl:value-of select="concat('Alternative ', 'end', ' # ',$AlternativeNo,':')" />
                              </b>
                            </span>
                          </th>
                        </tr>
                        <tr>
                          <td    colspan="3" style="border-top: 1px solid black">
                          </td>
                        </tr>
                      </xsl:if>
                    </xsl:if>
                    
                  </xsl:for-each>

                  <xsl:if test="($mode='start' or $mode='middle') and $recCount > 1 and position() = $recCount">
                    <tr>
                      <th colspan="2" align="center">
                        <span>
                          <b>Alternative Routes Merge</b>
                        </span>
                      </th>
                    </tr>
                    <tr>
                      <td    colspan="3" style="border-top: 1px solid black">
                      </td>
                    </tr>
                  </xsl:if>

                </xsl:for-each>
              </xsl:for-each>
            </table>
            <br></br>
          </xsl:for-each>
        </xsl:if>
        <xsl:if test="$DocType = 'EMAIL'">
          <xsl:for-each select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart">
            <b>
              LEG
              <xsl:value-of select="ns2:LegNumber" /><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>-
              <xsl:choose>
                <xsl:when test="contains(ns2:RoadPart/ns2:StartPointListPosition/ns2:StartPoint/ns5:Description, '##**##')">
                  <xsl:value-of select="substring-after(ns2:RoadPart/ns2:StartPointListPosition/ns2:StartPoint/ns5:Description, '##**##')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="ns2:RoadPart/ns2:StartPointListPosition/ns2:StartPoint/ns5:Description"/>
                </xsl:otherwise>
              </xsl:choose>
              to <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
              <xsl:choose>
                <xsl:when test="contains(ns2:RoadPart/ns2:EndPointListPosition/ns2:EndPoint/ns5:Description, '##**##')">
                  <xsl:value-of select="substring-after(ns2:RoadPart/ns2:EndPointListPosition/ns2:EndPoint/ns5:Description, '##**##')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="ns2:RoadPart/ns2:EndPointListPosition/ns2:EndPoint/ns5:Description"/>
                </xsl:otherwise>
              </xsl:choose>
            </b>
            <table border = "0" cellpadding="5" cellspacing="5">

              <tr>
                <th style="width:37%;">
                  <b>Route directions</b>
                </th>
                <th style="width:63%;">
                  <b>Cautions</b>
                </th>
              </tr>
              <tr>
                <td    colspan="3" style="border-top: 1px solid black">
                </td>
              </tr>
              <xsl:for-each select="ns2:RoadPart/ns6:DrivingInstructions/ns6:SubPartListPosition/ns6:SubPart">

                <xsl:variable name="set" select="ns6:AlternativeListPosition" />
                <xsl:variable name="recCount" select="count($set)" />

                <xsl:for-each select="ns6:AlternativeListPosition">
                  <xsl:variable name="AlternativeNo" select="ns6:Alternative/ns6:AlternativeDescription/ns6:AlternativeNo/text()" />
                  <xsl:variable name="startmode" select="(ns6:Alternative/ns6:InstructionListPosition/ns6:Instruction/ns6:NoteListPosition/ns6:Note/ns6:Content/ns6:RoutePoint/@PointType)[1]" />
                  <xsl:variable name="endmode" select="(ns6:Alternative/ns6:InstructionListPosition/ns6:Instruction/ns6:NoteListPosition/ns6:Note/ns6:Content/ns6:RoutePoint/@PointType)[position()=last()]" />

                  <xsl:variable name="mode">
                    <xsl:choose>
                      <xsl:when test="$startmode='start'">
                        <xsl:value-of select="'start'"/>
                      </xsl:when>
                      <xsl:when test="$endmode='end'">
                        <xsl:value-of select="'end'"/>
                      </xsl:when>
                      <xsl:otherwise>middle</xsl:otherwise>
                    </xsl:choose>
                  </xsl:variable>

                  <xsl:if test="($mode='end' or $mode='middle') and $recCount>1 and position()=1" >
                    <tr>
                      <th colspan="2" align="center">
                        <span>
                          <b>
                            Diverge
                          </b>
                        </span>
                      </th>
                    </tr>
                    <tr>
                      <td    colspan="3" style="border-top: 1px solid black">
                      </td>
                    </tr>
                  </xsl:if>
             

                  <xsl:for-each select="ns6:Alternative/ns6:InstructionListPosition">

                    <xsl:if test="$recCount &gt; 1" >

                      <xsl:if test="($mode='start' and position()=1)" >
                        <tr>
                          <th colspan="2" align="center">
                            <span>
                              <b>
                                <xsl:value-of select="concat('Alternative ', 'start', ' # ',$AlternativeNo,':')" />
                              </b>
                            </span>
                          </th>
                        </tr>
                        <tr>
                          <td    colspan="3" style="border-top: 1px solid black">
                          </td>
                        </tr>
                      </xsl:if>

                      <xsl:if test="$mode='middle' and position()=1 and $recCount>1" >
                        <tr>
                          <th colspan="2" align="center">
                            <span>
                              <b>
                                <xsl:value-of select="concat('Alternative ', ' # ',$AlternativeNo,':')" />
                              </b>
                            </span>
                          </th>
                        </tr>
                        <tr>
                          <td    colspan="3" style="border-top: 1px solid black">
                          </td>
                        </tr>
                      </xsl:if>

                    </xsl:if>
                    
                    <tr>
                      <xsl:if test="$UnitType=692001">
                        <xsl:choose>
                          <xsl:when test="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric &lt; 1000 or ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric &lt; 1000">
                            <td>
                              <xsl:choose>
                                <xsl:when test="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Instruction">
                                  <strike>
                                    <xsl:value-of select="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Instruction"/>
                                    <xsl:value-of select="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                                  </strike>
                                </xsl:when>
                                <xsl:when test="ns6:OldInstruction">
                                  <b>
                                    <u>
                                      <xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Instruction"/>
                                      <xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                                    </u>
                                  </b>
                                </xsl:when>
                                <xsl:otherwise>
                                  <!--RM#3646 - Start-->
                                  <!--<xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Instruction"/>
                            <xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m-->
                                  <xsl:choose>
                                    <xsl:when test="contains(ns6:Instruction/ns6:Navigation/ns6:Instruction, '##**##')">
                                      <strike>
                                        <xsl:value-of select="substring-before(ns6:Instruction/ns6:Navigation/ns6:Instruction, '##**##')"/>
                                      </strike>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      <b>
                                        <u>
                                          <xsl:value-of select="substring-after(ns6:Instruction/ns6:Navigation/ns6:Instruction, '##**##')"/>
                                        </u>
                                      </b>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Instruction"/>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                  <xsl:choose>
                                    <xsl:when test="contains(ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric, '##**##')">
                                      <strike>
                                        <xsl:value-of select="substring-before(ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric, '##**##')"/>
                                      </strike>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      <b>
                                        <u>
                                          <xsl:value-of select="substring-after(ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric, '##**##')"/>
                                        </u>
                                      </b>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric"/>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                                  <!--RM#3646 - End-->
                                </xsl:otherwise>
                              </xsl:choose>
                            </td>
                          </xsl:when>
                          <xsl:otherwise>
                            <td>
                              <xsl:choose>
                                <xsl:when test="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Instruction">
                                  <strike>
                                    <xsl:value-of select="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Instruction"/>
                                    <xsl:if test="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric != '' ">
                                      <xsl:variable name="var1" select="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric" />
                                      <xsl:variable name="var2" select="format-number($var1 div 1000, '#.##')"/>
                                      <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                                    </xsl:if>
                                  </strike>
                                </xsl:when>
                                <xsl:when test="ns6:OldInstruction">
                                  <b>
                                    <u>
                                      <xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Instruction"/>
                                    </u>
                                  </b>
                                </xsl:when>
                                <xsl:otherwise>
                                  <xsl:choose>
                                    <xsl:when test="contains(ns6:Instruction/ns6:Navigation/ns6:Instruction, '##**##')">
                                      <strike>
                                        <xsl:value-of select="substring-before(ns6:Instruction/ns6:Navigation/ns6:Instruction, '##**##')"/>
                                      </strike>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      <b>
                                        <u>
                                          <xsl:value-of select="substring-after(ns6:Instruction/ns6:Navigation/ns6:Instruction, '##**##')"/>
                                        </u>
                                      </b>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Instruction"/>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                  <!--RM#3646 - End-->
                                </xsl:otherwise>
                              </xsl:choose>
                              <xsl:if test="ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric != '' ">
                                <!--RM#3646 - Start-->
                                <xsl:choose>
                                  <xsl:when test="contains(ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric, '##**##')">
                                    <strike>
                                      <xsl:variable name="var1" select="substring-before(ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric, '##**##')" />
                                      <xsl:variable name="var2" select="format-number($var1 div 1000, '#.##')"/>
                                      <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                                    </strike>
                                    <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    <b>
                                      <u>
                                        <xsl:variable name="var1" select="substring-after(ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric, '##**##')" />
                                        <xsl:variable name="var2" select="format-number($var1 div 1000, '#.##')"/>
                                        <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                                      </u>
                                    </b>
                                  </xsl:when>
                                  <xsl:otherwise>
                                    <xsl:variable name="var1" select="ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric" />
                                    <xsl:variable name="var2" select="format-number($var1 div 1000, '#.##')"/>
                                    <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                                  </xsl:otherwise>
                                </xsl:choose>
                                <!--RM#3646 - End-->
                              </xsl:if>
                            </td>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:if>

                      <xsl:if test="$UnitType=692002">
                        <xsl:choose>
                          <xsl:when test="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial &lt; 1760 or ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial &lt; 1760">
                            <td>
                              <xsl:choose>
                                <xsl:when test="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Instruction">
                                  <strike>
                                    <xsl:value-of select="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Instruction"/>
                                    <xsl:value-of select="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>yards
                                  </strike>
                                </xsl:when>
                                <xsl:when test="ns6:OldInstruction">
                                  <b>
                                    <u>
                                      <xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Instruction"/>
                                      <xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayMetric"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>m
                                    </u>
                                  </b>
                                </xsl:when>
                                <xsl:otherwise>
                                  <!--RM#3646 - Start-->
                                  <!--<xsl:value-of select="ns6:Navigation/ns6:Instruction"/>-->
                                  <xsl:choose>
                                    <xsl:when test="contains(ns6:Instruction/ns6:Navigation/ns6:Instruction, '##**##')">
                                      <strike>
                                        <xsl:value-of select="substring-before(ns6:Instruction/ns6:Navigation/ns6:Instruction, '##**##')"/>
                                      </strike>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      <b>
                                        <u>
                                          <xsl:value-of select="substring-after(ns6:Instruction/ns6:Navigation/ns6:Instruction, '##**##')"/>
                                        </u>
                                      </b>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Instruction"/>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                  <!--RM#3646 - End-->
                                </xsl:otherwise>
                              </xsl:choose>
                              <!--RM#3646 - Start-->
                              <!--<xsl:value-of select="ns6:Navigation/ns6:Distance/ns6:DisplayImperial"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>yards-->
                              <xsl:choose>
                                <xsl:when test="contains(ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial, '##**##')">
                                  <strike>
                                    <xsl:value-of select="substring-before(ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial, '##**##')"/>
                                  </strike>
                                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                  <b>
                                    <u>
                                      <xsl:value-of select="substring-after(ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial, '##**##')"/>
                                    </u>
                                  </b>
                                </xsl:when>
                                <xsl:otherwise>
                                  <xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial"/>
                                </xsl:otherwise>
                              </xsl:choose>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>yards
                              <!--RM#3646 - End-->
                            </td>
                          </xsl:when>
                          <xsl:otherwise>
                            <td>
                              <xsl:choose>
                                <xsl:when test="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Instruction">
                                  <strike>
                                    <xsl:value-of select="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Instruction"/>
                                    <xsl:if test="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial != '' ">
                                      <xsl:variable name="var1" select="ns6:OldInstruction/ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial" />
                                      <xsl:variable name="var2" select="format-number($var1 div 1760, '#.##')"/>
                                      <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                                    </xsl:if>
                                  </strike>
                                </xsl:when>
                                <xsl:when test="ns6:OldInstruction">
                                  <b>
                                    <u>
                                      <xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Instruction"/>
                                    </u>
                                  </b>
                                </xsl:when>
                                <xsl:otherwise>
                                  <!--RM#3646 - Start-->
                                  <!--<xsl:value-of select="ns6:Navigation/ns6:Instruction"/>-->
                                  <xsl:choose>
                                    <xsl:when test="contains(ns6:Instruction/ns6:Navigation/ns6:Instruction, '##**##')">
                                      <strike>
                                        <xsl:value-of select="substring-before(ns6:Instruction/ns6:Navigation/ns6:Instruction, '##**##')"/>
                                      </strike>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      <b>
                                        <u>
                                          <xsl:value-of select="substring-after(ns6:Instruction/ns6:Navigation/ns6:Instruction, '##**##')"/>
                                        </u>
                                      </b>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:value-of select="ns6:Instruction/ns6:Navigation/ns6:Instruction"/>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                  <!--RM#3646 - End-->
                                </xsl:otherwise>
                              </xsl:choose>
                              <!--RM#3646 - Start-->
                              <xsl:choose>
                                <xsl:when test="contains(ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial, '##**##')">
                                  <strike>
                                    <xsl:variable name="var1" select="substring-before(ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial, '##**##')" />
                                    <xsl:variable name="var2" select="format-number($var1 div 1760, '#.##')"/>
                                    <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                                  </strike>
                                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                  <b>
                                    <u>
                                      <xsl:variable name="var1" select="substring-after(ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial, '##**##')" />
                                      <xsl:variable name="var2" select="format-number($var1 div 1760, '#.##')"/>
                                      <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                                    </u>
                                  </b>
                                </xsl:when>
                                <xsl:otherwise>
                                  <xsl:if test="ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial != '' ">
                                    <xsl:variable name="var1" select="ns6:Instruction/ns6:Navigation/ns6:Distance/ns6:DisplayImperial" />
                                    <xsl:variable name="var2" select="format-number($var1 div 1760, '#.##')"/>
                                    <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                                  </xsl:if>
                                </xsl:otherwise>
                              </xsl:choose>
                              <!--RM#3646 - End-->
                            </td>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:if>

                      <td valign ="top">

                        <xsl:choose>
                          <xsl:when test="ns6:Instruction/ns6:NoteListPosition/ns6:Note">
                            <xsl:for-each select="ns6:Instruction/ns6:NoteListPosition/ns6:Note">

                              <xsl:if test="ns6:Content/ns6:RoutePoint/@PointType = 'start'">
                                <b>Start point further details:</b>
                              </xsl:if>

                              <xsl:if test="ns6:Content/ns6:RoutePoint/@PointType = 'end'">
                                <b>End point further details:</b>
                              </xsl:if>

                              <xsl:choose>
                                <!--Code start here for EMAIL RM#4613-->
                                <xsl:when test="ns6:Content/ns6:Caution/ns10:Action/ns10:Standard != '' or 
                                ns6:Content/ns6:Caution/ns10:Action/ns10:SpecificAction != ''">

                                  <xsl:if test="ns6:Content/ns6:Caution/ns10:Action/ns10:Standard != ''">
                                    <xsl:choose>
                                      <xsl:when test="contains(ns6:Content/ns6:Caution/ns10:Action/ns10:Standard, '##**##')">
                                        <strike>
                                          <xsl:value-of select="substring-before(ns6:Content/ns6:Caution/ns10:Action/ns10:Standard, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </strike>
                                        <u>
                                          <b>
                                            <xsl:value-of select="substring-after(ns6:Content/ns6:Caution/ns10:Action/ns10:Standard, '##**##')" />
                                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                          </b>
                                        </u>
                                      </xsl:when>

                                      <xsl:otherwise>
                                        <xsl:value-of select="ns6:Content/ns6:Caution/ns10:Action/ns10:Standard"/>
                                      </xsl:otherwise>

                                    </xsl:choose>
                                  </xsl:if>

                                  <xsl:if test="ns6:Content/ns6:Caution/ns10:Action/ns10:SpecificAction != ''">
                                    <xsl:choose>
                                      <xsl:when test="contains(ns6:Content/ns6:Caution/ns10:Action/ns10:SpecificAction, '##**##')">
                                        <strike>
                                          <xsl:value-of select="substring-before(ns6:Content/ns6:Caution/ns10:Action/ns10:SpecificAction, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </strike>
                                        <u>
                                          <b>
                                            <xsl:value-of select="substring-after(ns6:Content/ns6:Caution/ns10:Action/ns10:SpecificAction, '##**##')" />
                                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                          </b>
                                        </u>
                                      </xsl:when>

                                      <xsl:otherwise>
                                        <xsl:value-of select="ns6:Content/ns6:Caution/ns10:Action/ns10:SpecificAction"/>
                                      </xsl:otherwise>
                                    </xsl:choose>
                                  </xsl:if>

                                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text> <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>

                                  <xsl:if test="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN !=''">
                                    <xsl:choose>
                                      <xsl:when test="contains(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN, '##**##')">
                                        <strike>
                                          <xsl:value-of select="substring-before(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </strike>
                                        <u>
                                          <b>
                                            <xsl:value-of select="substring-after(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN, '##**##')" />
                                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                          </b>
                                        </u>
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </xsl:when>

                                      <xsl:otherwise>
                                        <xsl:value-of select="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN"/>
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </xsl:otherwise>
                                    </xsl:choose>
                                  </xsl:if>

                                  <xsl:if test="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN !=''">
                                    <xsl:choose>
                                      <xsl:when test="contains(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN, '##**##')">
                                        <strike>
                                          <xsl:value-of select="substring-before(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </strike>
                                        <u>
                                          <b>
                                            <xsl:value-of select="substring-after(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN, '##**##')" />
                                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                          </b>
                                        </u>
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </xsl:when>

                                      <xsl:otherwise>
                                        <xsl:value-of select="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN"/>
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </xsl:otherwise>
                                    </xsl:choose>
                                  </xsl:if>


                                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                  <br/>
                                  <br/>
                                  CAUTION: After<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>

                                  <xsl:choose>
                                    <xsl:when test="$UnitType='' or $UnitType=692001">

                                      <xsl:choose>
                                        <xsl:when test="ns6:EncounteredAt/ns6:DisplayMetric != ''">
                                          <xsl:choose>
                                            <xsl:when test="ns6:EncounteredAt/ns6:DisplayMetric &lt; 1000">

                                              <xsl:choose>
                                                <xsl:when test="contains(ns6:EncounteredAt/ns6:DisplayMetric, '##**##')">
                                                  <strike>
                                                    <xsl:value-of select="substring-before(ns6:EncounteredAt/ns6:DisplayMetric, '##**##')" />m <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                                  </strike>
                                                  <u>
                                                    <b>
                                                      <xsl:value-of select="substring-after(ns6:EncounteredAt/ns6:DisplayMetric, '##**##')" />m <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                                    </b>
                                                  </u>
                                                </xsl:when>

                                                <xsl:otherwise>
                                                  <xsl:value-of select="ns6:EncounteredAt/ns6:DisplayMetric"/>m
                                                </xsl:otherwise>
                                              </xsl:choose>

                                            </xsl:when>
                                            <xsl:otherwise>
                                              <xsl:variable name="var1" select="ns6:EncounteredAt/ns6:DisplayMetric" />
                                              <xsl:variable name="var2" select="format-number($var1 div 1000, '#.##')"/>


                                              <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                                            </xsl:otherwise>
                                          </xsl:choose>
                                        </xsl:when>
                                        <xsl:otherwise>0 m</xsl:otherwise>
                                      </xsl:choose>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:choose>
                                        <xsl:when test="ns6:EncounteredAt/ns6:DisplayImperial != ''">
                                          <xsl:choose>
                                            <xsl:when test="ns6:EncounteredAt/ns6:DisplayImperial &lt; 1760">

                                              <xsl:choose>
                                                <xsl:when test="contains(ns6:EncounteredAt/ns6:DisplayImperial, '##**##')">
                                                  <strike>
                                                    <xsl:value-of select="substring-before(ns6:EncounteredAt/ns6:DisplayImperial, '##**##')" />yards <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                                  </strike>
                                                  <u>
                                                    <b>
                                                      <xsl:value-of select="substring-after(ns6:EncounteredAt/ns6:DisplayImperial, '##**##')" />yards <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                                    </b>
                                                  </u>
                                                </xsl:when>

                                                <xsl:otherwise>
                                                  <xsl:value-of select="ns6:EncounteredAt/ns6:DisplayImperial"/>yards
                                                </xsl:otherwise>
                                              </xsl:choose>

                                            </xsl:when>
                                            <xsl:otherwise>
                                              <xsl:variable name="var1" select="ns6:EncounteredAt/ns6:DisplayImperial" />
                                              <xsl:variable name="var2" select="format-number($var1 div 1760, '#.##')"/>
                                              <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                                            </xsl:otherwise>
                                          </xsl:choose>
                                        </xsl:when>
                                        <xsl:otherwise>0 yards</xsl:otherwise>
                                      </xsl:choose>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                  <br/>
                                  <br/>


                                  <xsl:if test="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN !=''">
                                    <xsl:choose>
                                      <xsl:when test="contains(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN, '##**##')">
                                        <strike>
                                          <xsl:value-of select="substring-before(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </strike>
                                        <u>
                                          <b>
                                            <xsl:value-of select="substring-after(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN, '##**##')" />
                                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                          </b>
                                        </u>
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </xsl:when>

                                      <xsl:otherwise>
                                        <xsl:value-of select="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN"/>
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </xsl:otherwise>
                                    </xsl:choose>
                                  </xsl:if>

                                  <xsl:if test="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN !=''">
                                    <xsl:choose>
                                      <xsl:when test="contains(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN, '##**##')">
                                        <strike>
                                          <xsl:value-of select="substring-before(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </strike>
                                        <u>
                                          <b>
                                            <xsl:value-of select="substring-after(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN, '##**##')" />
                                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                          </b>
                                        </u>
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </xsl:when>

                                      <xsl:otherwise>
                                        <xsl:value-of select="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN"/>
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </xsl:otherwise>
                                    </xsl:choose>
                                  </xsl:if>

                                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>

                                  <xsl:choose>
                                    <xsl:when test="contains(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ConstraintName, '##**##')">
                                      <strike>
                                        <xsl:value-of select="substring-before(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ConstraintName, '##**##')" />
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </strike>
                                      <u>
                                        <b>
                                          <xsl:value-of select="substring-after(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ConstraintName, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </b>
                                      </u>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:when>

                                    <xsl:otherwise>
                                      <xsl:value-of select="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ConstraintName"/>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:otherwise>
                                  </xsl:choose>

                                  <xsl:choose>
                                    <xsl:when test="contains(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:StructureName, '##**##')">
                                      <strike>
                                        <xsl:value-of select="substring-before(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:StructureName, '##**##')" />
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </strike>
                                      <u>
                                        <b>
                                          <xsl:value-of select="substring-after(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:StructureName, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </b>
                                      </u>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:when>

                                    <xsl:otherwise>
                                      <xsl:value-of select="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:StructureName"/>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:otherwise>
                                  </xsl:choose>


                                  <BR/>
                                  <div align="left" valign ="top">
                                    <b>
                                      GridRef:
                                    </b>
                                    <xsl:choose>
                                      <xsl:when test="ns6:GridReference/ns8:X != ''">

                                        <xsl:choose>
                                          <xsl:when test="contains(ns6:GridReference/ns8:X, '##**##')">
                                            <strike>
                                              <xsl:value-of select="substring-before(ns6:GridReference/ns8:X, '##**##')" />
                                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                            </strike>
                                            <u>
                                              <b>
                                                <xsl:value-of select="substring-after(ns6:GridReference/ns8:X, '##**##')" />
                                                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                              </b>
                                            </u>
                                          </xsl:when>
                                          <xsl:otherwise>
                                            <xsl:value-of select="ns6:GridReference/ns8:X"/>
                                          </xsl:otherwise>
                                        </xsl:choose>

                                      </xsl:when>
                                      <xsl:otherwise>
                                        0
                                      </xsl:otherwise>
                                    </xsl:choose>,
                                    <xsl:choose>
                                      <xsl:when test="ns6:GridReference/ns8:Y != ''">

                                        <xsl:choose>
                                          <xsl:when test="contains(ns6:GridReference/ns8:Y, '##**##')">
                                            <strike>
                                              <xsl:value-of select="substring-before(ns6:GridReference/ns8:Y, '##**##')" />
                                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                            </strike>
                                            <u>
                                              <b>
                                                <xsl:value-of select="substring-after(ns6:GridReference/ns8:Y, '##**##')" />
                                                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                              </b>
                                            </u>
                                          </xsl:when>
                                          <xsl:otherwise>
                                            <xsl:value-of select="ns6:GridReference/ns8:Y"/>
                                          </xsl:otherwise>
                                        </xsl:choose>
                                      </xsl:when>
                                      <xsl:otherwise>
                                        0
                                      </xsl:otherwise>
                                    </xsl:choose>
                                  </div>
                                  <xsl:if test="ns6:Content/ns6:MotorwayCaution/ns6:Description != ''">
                                    <b>Apply motorway caution</b>
                                  </xsl:if>

                                  <xsl:choose>
                                    <xsl:when test="contains(ns6:Content/ns6:Annotation/ns9:Text, '##**##')">
                                      <strike>
                                        <xsl:value-of select="substring-before(ns6:Content/ns6:Annotation/ns9:Text, '##**##')" />
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </strike>
                                      <u>
                                        <b>
                                          <xsl:value-of select="substring-after(ns6:Content/ns6:Annotation/ns9:Text, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </b>
                                      </u>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:when>

                                    <xsl:otherwise>
                                      <xsl:value-of select="ns6:Content/ns6:Annotation/ns9:Text"/>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:otherwise>
                                  </xsl:choose>


                                </xsl:when>

                                <xsl:when test="ns6:Content/ns6:Annotation != ''">
                                  <!--Code start 30 june-->
                                  <br/>
                                  After<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                  <xsl:choose>
                                    <xsl:when test="$UnitType='' or $UnitType=692001">

                                      <xsl:choose>
                                        <xsl:when test="ns6:EncounteredAt/ns6:DisplayMetric != ''">
                                          <xsl:choose>
                                            <xsl:when test="ns6:EncounteredAt/ns6:DisplayMetric &lt; 1000">
                                              <xsl:value-of select="ns6:EncounteredAt/ns6:DisplayMetric"/> m
                                            </xsl:when>
                                            <xsl:otherwise>
                                              <xsl:variable name="var1" select="ns6:EncounteredAt/ns6:DisplayMetric" />
                                              <xsl:variable name="var2" select="format-number($var1 div 1000, '#.##')"/>
                                              <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                                            </xsl:otherwise>
                                          </xsl:choose>
                                        </xsl:when>
                                        <xsl:otherwise>0 m</xsl:otherwise>
                                      </xsl:choose>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:choose>
                                        <xsl:when test="ns6:EncounteredAt/ns6:DisplayImperial != ''">
                                          <xsl:choose>
                                            <xsl:when test="ns6:EncounteredAt/ns6:DisplayImperial &lt; 1760">
                                              <xsl:value-of select="ns6:EncounteredAt/ns6:DisplayImperial"/> yards
                                            </xsl:when>
                                            <xsl:otherwise>
                                              <xsl:variable name="var1" select="ns6:EncounteredAt/ns6:DisplayImperial" />
                                              <xsl:variable name="var2" select="format-number($var1 div 1760, '#.##')"/>
                                              <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                                            </xsl:otherwise>
                                          </xsl:choose>
                                        </xsl:when>
                                        <xsl:otherwise>0 yards</xsl:otherwise>
                                      </xsl:choose>
                                    </xsl:otherwise>
                                  </xsl:choose>

                                  <br/>
                                  <br/>
                                  <div align="left" valign ="top">
                                    <b>
                                      GridRef:
                                    </b>
                                    <xsl:choose>
                                      <xsl:when test="ns6:GridReference/ns8:X != ''">

                                        <xsl:choose>
                                          <xsl:when test="contains(ns6:GridReference/ns8:X, '##**##')">
                                            <strike>
                                              <xsl:value-of select="substring-before(ns6:GridReference/ns8:X, '##**##')" />
                                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                            </strike>
                                            <u>
                                              <b>
                                                <xsl:value-of select="substring-after(ns6:GridReference/ns8:X, '##**##')" />
                                                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                              </b>
                                            </u>
                                          </xsl:when>
                                          <xsl:otherwise>
                                            <xsl:value-of select="ns6:GridReference/ns8:X"/>
                                          </xsl:otherwise>
                                        </xsl:choose>

                                      </xsl:when>
                                      <xsl:otherwise>
                                        0
                                      </xsl:otherwise>
                                    </xsl:choose>,
                                    <xsl:choose>
                                      <xsl:when test="ns6:GridReference/ns8:Y != ''">

                                        <xsl:choose>
                                          <xsl:when test="contains(ns6:GridReference/ns8:Y, '##**##')">
                                            <strike>
                                              <xsl:value-of select="substring-before(ns6:GridReference/ns8:Y, '##**##')" />
                                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                            </strike>
                                            <u>
                                              <b>
                                                <xsl:value-of select="substring-after(ns6:GridReference/ns8:Y, '##**##')" />
                                                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                              </b>
                                            </u>
                                          </xsl:when>
                                          <xsl:otherwise>
                                            <xsl:value-of select="ns6:GridReference/ns8:Y"/>
                                          </xsl:otherwise>
                                        </xsl:choose>
                                      </xsl:when>
                                      <xsl:otherwise>
                                        0
                                      </xsl:otherwise>
                                    </xsl:choose>
                                  </div>

                                  <br/>

                                  <xsl:variable name="AnnotationType" select="ns6:Content/ns6:Annotation/@AnnotationType" />
                                  <xsl:if test="$AnnotationType!='generic'" >
                                    <xsl:if test="$AnnotationType='special manouevre'" >
                                      <b>Special manouevre:</b>
                                    </xsl:if>
                                    <xsl:if test="$AnnotationType='caution'" >
                                      <b>Caution:</b>
                                    </xsl:if>
                                  </xsl:if>

                                  <xsl:choose>
                                    <xsl:when test="contains(ns6:Content/ns6:Annotation/ns9:Text, '##**##')">
                                      <strike>
                                        <xsl:value-of select="substring-before(ns6:Content/ns6:Annotation/ns9:Text, '##**##')" />
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </strike>
                                      <u>
                                        <b>
                                          <xsl:value-of select="substring-after(ns6:Content/ns6:Annotation/ns9:Text, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </b>
                                      </u>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:when>

                                    <xsl:otherwise>
                                      <xsl:value-of select="ns6:Content/ns6:Annotation/ns9:Text"/>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                  <!--Code end 30 june-->
                                </xsl:when>

                                <xsl:when test="ns6:Content/ns6:Caution/ns10:CautionedEntity != ''">
                                  <br/>
                                  CAUTION: After<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>

                                  <xsl:choose>
                                    <xsl:when test="$UnitType='' or $UnitType=692001">

                                      <xsl:choose>
                                        <xsl:when test="ns6:EncounteredAt/ns6:DisplayMetric != ''">
                                          <xsl:choose>
                                            <xsl:when test="ns6:EncounteredAt/ns6:DisplayMetric &lt; 1000">

                                              <xsl:choose>
                                                <xsl:when test="contains(ns6:EncounteredAt/ns6:DisplayMetric, '##**##')">
                                                  <strike>
                                                    <xsl:value-of select="substring-before(ns6:EncounteredAt/ns6:DisplayMetric, '##**##')" />m <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                                  </strike>
                                                  <u>
                                                    <b>
                                                      <xsl:value-of select="substring-after(ns6:EncounteredAt/ns6:DisplayMetric, '##**##')" />m <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                                    </b>
                                                  </u>
                                                </xsl:when>

                                                <xsl:otherwise>
                                                  <xsl:value-of select="ns6:EncounteredAt/ns6:DisplayMetric"/>m
                                                </xsl:otherwise>
                                              </xsl:choose>

                                            </xsl:when>
                                            <xsl:otherwise>
                                              <xsl:variable name="var1" select="ns6:EncounteredAt/ns6:DisplayMetric" />
                                              <xsl:variable name="var2" select="format-number($var1 div 1000, '#.##')"/>


                                              <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
                                            </xsl:otherwise>
                                          </xsl:choose>
                                        </xsl:when>
                                        <xsl:otherwise>0 m</xsl:otherwise>
                                      </xsl:choose>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:choose>
                                        <xsl:when test="ns6:EncounteredAt/ns6:DisplayImperial != ''">
                                          <xsl:choose>
                                            <xsl:when test="ns6:EncounteredAt/ns6:DisplayImperial &lt; 1760">

                                              <xsl:choose>
                                                <xsl:when test="contains(ns6:EncounteredAt/ns6:DisplayImperial, '##**##')">
                                                  <strike>
                                                    <xsl:value-of select="substring-before(ns6:EncounteredAt/ns6:DisplayImperial, '##**##')" />yards <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                                  </strike>
                                                  <u>
                                                    <b>
                                                      <xsl:value-of select="substring-after(ns6:EncounteredAt/ns6:DisplayImperial, '##**##')" />yards <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                                    </b>
                                                  </u>
                                                </xsl:when>

                                                <xsl:otherwise>
                                                  <xsl:value-of select="ns6:EncounteredAt/ns6:DisplayImperial"/>yards
                                                </xsl:otherwise>
                                              </xsl:choose>

                                            </xsl:when>
                                            <xsl:otherwise>
                                              <xsl:variable name="var1" select="ns6:EncounteredAt/ns6:DisplayImperial" />
                                              <xsl:variable name="var2" select="format-number($var1 div 1760, '#.##')"/>
                                              <xsl:value-of select="$var2"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
                                            </xsl:otherwise>
                                          </xsl:choose>
                                        </xsl:when>
                                        <xsl:otherwise>0 yards</xsl:otherwise>
                                      </xsl:choose>
                                    </xsl:otherwise>
                                  </xsl:choose>

                                  <br/>
                                  <br/>

                                  <xsl:if test="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN !=''">
                                    <xsl:choose>
                                      <xsl:when test="contains(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN, '##**##')">
                                        <strike>
                                          <xsl:value-of select="substring-before(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </strike>
                                        <u>
                                          <b>
                                            <xsl:value-of select="substring-after(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN, '##**##')" />
                                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                          </b>
                                        </u>
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </xsl:when>

                                      <xsl:otherwise>
                                        <xsl:value-of select="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ECRN"/>
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </xsl:otherwise>
                                    </xsl:choose>
                                  </xsl:if>

                                  <xsl:if test="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN !=''">
                                    <xsl:choose>
                                      <xsl:when test="contains(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN, '##**##')">
                                        <strike>
                                          <xsl:value-of select="substring-before(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </strike>
                                        <u>
                                          <b>
                                            <xsl:value-of select="substring-after(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN, '##**##')" />
                                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                          </b>
                                        </u>
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </xsl:when>

                                      <xsl:otherwise>
                                        <xsl:value-of select="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:ESRN"/>
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </xsl:otherwise>
                                    </xsl:choose>
                                  </xsl:if>

                                  <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>

                                  <!--For structure name start-->
                                  <xsl:choose>
                                    <xsl:when test="contains(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ConstraintName, '##**##')">
                                      <strike>
                                        <xsl:value-of select="substring-before(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ConstraintName, '##**##')" />
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </strike>
                                      <u>
                                        <b>
                                          <xsl:value-of select="substring-after(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ConstraintName, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </b>
                                      </u>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:when>

                                    <xsl:otherwise>
                                      <xsl:value-of select="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Constraint/ns10:ConstraintName"/>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:otherwise>
                                  </xsl:choose>

                                  <xsl:choose>
                                    <xsl:when test="contains(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:StructureName, '##**##')">
                                      <strike>
                                        <xsl:value-of select="substring-before(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:StructureName, '##**##')" />
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </strike>
                                      <u>
                                        <b>
                                          <xsl:value-of select="substring-after(ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:StructureName, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </b>
                                      </u>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:when>

                                    <xsl:otherwise>
                                      <xsl:value-of select="ns6:Content/ns6:Caution/ns10:CautionedEntity/ns10:Structure/ns10:StructureName"/>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                  <!--End-->

                                  <br/>
                                  <br/>
                                  <div align="left" valign ="top">
                                    <b>
                                      GridRef:
                                    </b>
                                    <xsl:choose>
                                      <xsl:when test="ns6:GridReference/ns8:X != ''">

                                        <xsl:choose>
                                          <xsl:when test="contains(ns6:GridReference/ns8:X, '##**##')">
                                            <strike>
                                              <xsl:value-of select="substring-before(ns6:GridReference/ns8:X, '##**##')" />
                                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                            </strike>
                                            <u>
                                              <b>
                                                <xsl:value-of select="substring-after(ns6:GridReference/ns8:X, '##**##')" />
                                                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                              </b>
                                            </u>
                                          </xsl:when>
                                          <xsl:otherwise>
                                            <xsl:value-of select="ns6:GridReference/ns8:X"/>,
                                          </xsl:otherwise>
                                        </xsl:choose>

                                      </xsl:when>
                                      <xsl:otherwise>
                                        0
                                      </xsl:otherwise>
                                    </xsl:choose>,
                                    <xsl:choose>
                                      <xsl:when test="ns6:GridReference/ns8:Y != ''">

                                        <xsl:choose>
                                          <xsl:when test="contains(ns6:GridReference/ns8:Y, '##**##')">
                                            <strike>
                                              <xsl:value-of select="substring-before(ns6:GridReference/ns8:Y, '##**##')" />
                                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                            </strike>
                                            <u>
                                              <b>
                                                <xsl:value-of select="substring-after(ns6:GridReference/ns8:Y, '##**##')" />
                                                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                              </b>
                                            </u>
                                          </xsl:when>
                                          <xsl:otherwise>
                                            <xsl:value-of select="ns6:GridReference/ns8:Y"/>,
                                          </xsl:otherwise>
                                        </xsl:choose>
                                      </xsl:when>
                                      <xsl:otherwise>
                                        0
                                      </xsl:otherwise>
                                    </xsl:choose>
                                  </div>

                                </xsl:when>

                                <xsl:otherwise>
                                  <div align="left" valign ="top">
                                    <b>
                                      <xsl:if test="ns6:GridReference/ns8:X != '' or ns6:GridReference/ns8:Y != ''">
                                        <br/>
                                        GridRef:
                                      </xsl:if>
                                    </b>
                                    <xsl:choose>

                                      <xsl:when test="ns6:GridReference/ns8:X != ''">

                                        <xsl:choose>
                                          <xsl:when test="contains(ns6:GridReference/ns8:X, '##**##')">
                                            <strike>
                                              <xsl:value-of select="substring-before(ns6:GridReference/ns8:X, '##**##')" />
                                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                            </strike>
                                            <u>
                                              <b>
                                                <xsl:value-of select="substring-after(ns6:GridReference/ns8:X, '##**##')" />, <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                              </b>
                                            </u>
                                          </xsl:when>
                                          <xsl:otherwise>
                                            <xsl:value-of select="ns6:GridReference/ns8:X"/>,
                                          </xsl:otherwise>
                                        </xsl:choose>


                                      </xsl:when>
                                      <xsl:otherwise>

                                      </xsl:otherwise>
                                    </xsl:choose>
                                    <xsl:choose>
                                      <xsl:when test="ns6:GridReference/ns8:Y != ''">

                                        <xsl:choose>
                                          <xsl:when test="contains(ns6:GridReference/ns8:Y, '##**##')">
                                            <strike>
                                              <xsl:value-of select="substring-before(ns6:GridReference/ns8:Y, '##**##')" />
                                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                            </strike>
                                            <u>
                                              <b>
                                                <xsl:value-of select="substring-after(ns6:GridReference/ns8:Y, '##**##')" />
                                                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                              </b>
                                            </u>
                                          </xsl:when>
                                          <xsl:otherwise>
                                            <xsl:value-of select="ns6:GridReference/ns8:Y"/>
                                          </xsl:otherwise>
                                        </xsl:choose>


                                      </xsl:when>

                                      <xsl:otherwise>

                                      </xsl:otherwise>
                                    </xsl:choose>
                                  </div>

                                  <xsl:if test="ns6:Content/ns6:MotorwayCaution/ns6:Description != ''">
                                    <b>Apply motorway caution</b>
                                  </xsl:if>

                                  <xsl:choose>
                                    <xsl:when test="contains(ns6:Content/ns6:Annotation/ns9:Text, '##**##')">
                                      <strike>
                                        <xsl:value-of select="substring-before(ns6:Content/ns6:Annotation/ns9:Text, '##**##')" />
                                        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                      </strike>
                                      <u>
                                        <b>
                                          <xsl:value-of select="substring-after(ns6:Content/ns6:Annotation/ns9:Text, '##**##')" />
                                          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                        </b>
                                      </u>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:when>

                                    <xsl:otherwise>
                                      <xsl:value-of select="ns6:Content/ns6:Annotation/ns9:Text"/>
                                      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                </xsl:otherwise>

                              </xsl:choose>
                              <!--Code End here for EMAIL RM#4613-->

                              <xsl:call-template name="parseString">
                                <xsl:with-param name="list" select="ns6:Content/ns6:RoutePoint/ns6:Description"/>
                              </xsl:call-template>
                              <xsl:call-template name="parseString">
                                <xsl:with-param name="list" select="ns6:Contact/ns12:FullName"/>
                              </xsl:call-template>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                              <xsl:call-template name="parseString">
                                <xsl:with-param name="list" select="ns6:Contact/ns12:OrganisationName"/>
                              </xsl:call-template>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                              <xsl:call-template name="parseString">
                                <xsl:with-param name="list" select="ns6:Contact/ns12:Address"/>
                              </xsl:call-template>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                              <xsl:call-template name="parseString">
                                <xsl:with-param name="list" select="ns6:Contact/ns12:TelephoneNumber"/>
                              </xsl:call-template>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                              <xsl:call-template name="parseString">
                                <xsl:with-param name="list" select="ns6:Contact/ns12:FaxNumber"/>
                              </xsl:call-template>
                              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                              <xsl:call-template name="parseString">
                                <xsl:with-param name="list" select="ns6:Contact/ns12:EmailAddress"/>
                              </xsl:call-template>

                              <xsl:if test="position() != last()">
                                <br/>
                                <table  width="100%" cellpadding="0" cellspacing="0">
                                  <tr>
                                    <td style="border-top: 1px solid black"></td>
                                  </tr>
                                </table>

                              </xsl:if>

                            </xsl:for-each>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                          </xsl:otherwise>
                        </xsl:choose>
                      </td>
                    </tr>
                    <tr>
                      <td    colspan="3" style="border-top: 1px solid black">
                      </td>
                    </tr>
                    <xsl:if test="$recCount &gt; 1" >
                      <xsl:if test="($mode='end' and position()=last())" >
                        <tr>
                          <th colspan="2" align="center">
                            <span>
                              <b>
                                <xsl:value-of select="concat('Alternative ', 'end', ' # ',$AlternativeNo,':')" />
                              </b>
                            </span>
                          </th>
                        </tr>
                        <tr>
                          <td    colspan="3" style="border-top: 1px solid black">
                          </td>
                        </tr>
                      </xsl:if>
                    </xsl:if>
                  </xsl:for-each>

                  <xsl:if test="($mode='start' or $mode='middle') and $recCount > 1 and position() = $recCount">
                    <tr>
                      <th colspan="2" align="center">
                        <span>
                          <b>Alternative Routes Merge</b>
                        </span>
                      </th>
                    </tr>
                    <tr>
                      <td    colspan="3" style="border-top: 1px solid black">
                      </td>
                    </tr>
                  </xsl:if>

                </xsl:for-each>
              </xsl:for-each>
              <br></br>
            </table>
            <br></br>
          </xsl:for-each>
        </xsl:if>
        <br/>
        <div style="text-decoration:none !important;">
          <xsl:choose>
            <xsl:when test="$DocType = 'PDF'">
              <b>
                AFFECTED STRUCTURES (<xsl:value-of select="ns1:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:Name" />)
              </b>
              <br/>
              <b>
                Structures you are responsible for have been affected. Details of these structures can be found in ESDAL.
              </b>
            </xsl:when>
            <xsl:otherwise>
              <xsl:for-each select="ns1:StructureDetails/ns1:div/ns1:table/ns1:tbody/ns1:tr">
                <b>
                  <xsl:value-of select="ns1:th"/>
                </b>
                <p>
                  <xsl:value-of select="ns1:td" />
                </p>
              </xsl:for-each>
            </xsl:otherwise>
          </xsl:choose>
          <br/>
          <b>
            <u> NOTES FOR HAULIER</u>
          </b>

          <xsl:choose>
            <xsl:when test="ns2:NotesForHaulier !=''">
              <xsl:call-template name="parseString">
                <xsl:with-param name="list" select="ns2:NotesForHaulier"/>
              </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
              There are no notes.
            </xsl:otherwise>
          </xsl:choose>
          <br/>
          <br/>

          <xsl:choose>
            <xsl:when test="contains(ns2:PredefinedCautions/ns6:DockCautionDescription, '##**##')">
              <xsl:if test="substring-after(ns2:PredefinedCautions/ns6:DockCautionDescription, '##**##') = '1'">                
                <p style="padding:top:6px;">
                  <b>
                    <u>
                      DOCK CAUTION
                    </u>
                  </b>
                  <br/>
                  The manufacturer and/or hauliers are reminded that it is their responsibility to negotiate with the appropriate dock
                  manager for the movements within the dock area.
                </p>

              </xsl:if>

            </xsl:when>
            <xsl:otherwise>
              <xsl:if test="ns2:PredefinedCautions/ns6:DockCautionDescription = '1'">               
                <p style="padding:top:6px;">
                  <b>
                    <u>
                      DOCK CAUTION
                    </u>
                  </b>
                  <br/>
                  The manufacturer and/or hauliers are reminded that it is their responsibility to negotiate with the appropriate dock
                  manager for the movements within the dock area.
                </p>
              </xsl:if>
            </xsl:otherwise>
          </xsl:choose>          
          
          <br/>
          <xsl:if test="ns2:PredefinedCautions/ns6:MotorwayCautionDescription">
            <b>
              <u>
                MOTORWAY CAUTIONS
              </u>
            </b>
            <br/>
            <table style="font-family:Arial;font-size:10px;">
              <tr>
                <td valign="top">(a)</td>
                <td colspan="28">
                  When travelling on the motorway, vehicles must not travel on the hard shoulder or in the right hand lane.
                </td>
              </tr>
              <tr>
                <td valign="top">(b)</td>
                <td colspan="28" >
                  When crossing motorway bridges, vehicles must travel in the left hand lane.
                </td>
              </tr>
              <tr>
                <td valign="top">(c)</td>
                <td colspan="28">
                  If over 4.877 m in height, height of load must be reduced to a minimum when travelling under motorway
                  bridges.
                </td>
              </tr>
              <tr>
                <td valign="top">(d)</td>
                <td colspan="28">
                  If under but near 4.877 m in height, extreme caution must be exercised when travelling under motorway
                  bridges.
                </td>
              </tr>
            </table>
          </xsl:if>
        </div>
      </body>

    </html>

  </xsl:template>



  <xsl:template name="remove">
    <xsl:param name="value" select="."/>
    <xsl:param name="removeString" select="."/>
    <xsl:value-of select="concat(substring-before($value, $removeString), substring-after($value, $removeString))"/>
  </xsl:template>

  <xsl:template name="letters">
    <xsl:param name="text" select="." />
    <xsl:if test="$text != ''">
      <xsl:variable name="letter" select="substring($text, 1, 2)" />
      <xsl:value-of select="$letter" />
      <xsl:call-template name="letters">
        <xsl:with-param name="text" select="substring-after($text, $letter)" />
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


  <xsl:template name="tokenize">
    <xsl:param name="pText"/>

    <xsl:if test="string-length($pText)">
      <tag>
        <xsl:value-of select=
         "substring-before($pText, ',')"/>
      </tag>

      <xsl:call-template name="tokenize">
        <xsl:with-param name="pText" select=
         "substring-after($pText, ',')"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <!--Changes for RM#4998 start-->
  <xsl:template match="text()" name="SplitAlternative">
    <xsl:param name="pText" select="."/>
    <xsl:param name="pDelim" select="' OR'"/>
    <xsl:param name="pCounter" select="1"/>
    <xsl:if test="string-length($pText) > 0">
      <xsl:variable name="vToken" select=
    "substring-before(concat($pText,' OR'), ' OR')"/>
      <!--<xsl:value-of select="$vToken"/>
      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;,&nbsp;]]></xsl:text>
      <xsl:value-of select="$pCounter"/>-->

      <xsl:if test="not($pCounter = 1)">
        or<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
      </xsl:if>
      <xsl:if test="$UnitType='' or $UnitType=692001">
        <xsl:variable name="varKM" select="round(number($vToken) div number(1000))"/>
        <xsl:value-of select="$varKM"/> <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>km
      </xsl:if>
      <xsl:if test="$UnitType=692002">
        <xsl:variable name="varMiles" select="round(number($vToken) div number(1760))"/>
        <xsl:value-of select="$varMiles"/><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>miles
      </xsl:if>
      <xsl:call-template name="SplitAlternative">
        <xsl:with-param name="pText" select=
      "substring-after($pText,'OR')"/>
        <xsl:with-param name="pCounter"
        select="$pCounter + 1"/>
      </xsl:call-template>

    </xsl:if>
  </xsl:template>
  <!--Changes for RM#4998 end-->


  <xsl:template name="parseStringForMeters">

    <xsl:param name="list"/>

    <xsl:if test="contains($list, '##**##')">
      <xsl:if test="$UnitType='' or $UnitType=692001">
        <xsl:value-of select="substring-after($list, '##**##')"/>
        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
      </xsl:if>

      <xsl:if test="$UnitType=692002">
        <xsl:value-of select="substring-after($list, '##**##')"/>
        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
      </xsl:if>
    </xsl:if>

    <xsl:if test="contains($list, '##**##')=false()">

      <xsl:if test="$UnitType='' or $UnitType=692001">
        <xsl:value-of select="$list"/>
        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
      </xsl:if>

      <xsl:if test="$UnitType=692002">
        <xsl:value-of select="$list"/>
        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
      </xsl:if>

    </xsl:if>


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

            <xsl:value-of select="floor($Feet)"/>' <xsl:value-of select="$Inches"/>"
          </xsl:when>
          <xsl:otherwise>
            <xsl:variable name="Feet" select="number($metreInches) div 12"/>
            <xsl:variable name="Inches" select="floor(number($metreInches)) mod 12"/>
            <!--<xsl:variable name="Inches" select="floor(number($InchesValue))"/>-->
            <xsl:value-of select="floor($Feet)"/>' <xsl:value-of select="$Inches"/>"
          </xsl:otherwise>
        </xsl:choose>

        <!--or <xsl:value-of select="$needRoundOff"/>needRoundOff-->

      </xsl:when>

      <xsl:otherwise>
        <xsl:value-of select="0"/>
      </xsl:otherwise>

    </xsl:choose>

  </xsl:template>

</xsl:stylesheet>

