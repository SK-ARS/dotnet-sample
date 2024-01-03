<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:ns1="http://www.esdal.com/schemas/core/nolongeraffected"
xmlns:ns2="http://www.esdal.com/schemas/notification/common/outbounddocument"
xmlns:ns3="http://www.esdal.com/schemas/core/movement"
xmlns:ns4="http://www.esdal.com/schemas/core/esdalcommontypes">

  <xsl:param name="DocType"></xsl:param>

  <xsl:template match="/ns1:NoLongerAffected">
    <html>
      <body style="font-family:Arial;font-size:13px;">
        <table width = "100%" cellspacing ="0" cellpadding ="0">
          <tr>
            <td>
              <xsl:choose>
                <xsl:when test="$DocType = 'EMAIL'">
                  <img align="left" width="540" height="80" src="https://esdal.dft.gov.uk/Content/Images/ESDAL 2 Logo_org.png"/>
                </xsl:when>
                <xsl:otherwise>
                  <img align="left" width="540" height="80" id="hdr_img"/>
                </xsl:otherwise>
              </xsl:choose>
            </td>
          </tr>
        </table>
        <div>
          <!--FACSIMILE MESSAGE-->
        </div>
        <br></br>
        <table  bgcolor="#e9eef4">
          <tr>
            <td>
              <h2>No longer affected by route</h2>
            </td>
          </tr>
          <br>

          </br>
        </table>
        <br />
        <br />
        <br />
        <table width = "95%" cellspacing ="0" cellpadding ="0">
        
          <tr>
            <td></td>
            <td colspan="3">
              <br></br>
            </td>
          </tr>
          <tr>
            <td>
              <b>Date Sent:</b>
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
            <td></td>
            <td colspan="3">
              <br></br>
            </td>
          </tr>
          <xsl:if test="$DocType = 'PDF'">
            <tr>
              <td>
                <b>No of Pages:</b>
              </td>
              <td colspan="3">###Noofpages###</td>
            </tr>
            <tr>
              <td></td>
              <td colspan="3">
                <br></br>
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
                  <xsl:call-template name="Capitalize">
                    <xsl:with-param name="word" select="substring-before(concat(ns2:HAContact/ns2:Address/ns4:Country, ' '), ' ')"/>
                  </xsl:call-template>
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
                  Email address:<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:value-of select="ns2:HAContact/ns2:EmailAddress" />
                </div>
              </xsl:if>
            </td>
          </tr>
          <tr>
            <td></td>
            <td colspan="3">
              <br></br>
            </td>
          </tr>

        </table>
        <table width = "95%">
          <tr>
            <td>
              <p align="left">
                <b>  To </b>
              </p >
              <xsl:for-each select="ns1:Recipients/ns3:Contact">
                <p>
                  <xsl:value-of select="ns3:ContactName" />,<xsl:value-of select="ns3:OrganisationName" /><br/>
                </p>
              </xsl:for-each>
            </td>
          </tr>
          <tr>
            <td>
              <br></br>
              <b>test to test</b>
              <br></br>
              <br></br>
              You are no longer affected by the above route
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
</xsl:stylesheet>
