<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="2.0"
xmlns:o="urn:schemas-microsoft-com:office:office"
xmlns:w="urn:schemas-microsoft-com:office:word"
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

        <table border = "0" >
          <tr>
            <td colspan="65" >
              <b>Form of Indemnity</b>
            </td>
          </tr>
          <tr>
            <td colspan="65">
              <b>
                THE INDEMNITY
              </b>
            </td>
          </tr>
          <tr>
            <td colspan="2" valign="top">
              1.
            </td>
            <td colspan="63">


              We<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:value-of select="ns1:HaulierDetails/ns2:HaulierName" />
              <xsl:if test="(ns1:OnBehalfOf != '?' and ns1:OnBehalfOf != '')">
                
                <xsl:choose>
                  <xsl:when test="contains(ns1:OnBehalfOf, '##**##')">
                    (on behalf of <xsl:value-of select="substring-after(ns1:OnBehalfOf, '##**##')"/>)
                  </xsl:when>
                  <xsl:otherwise>
                    (on behalf of <xsl:value-of select="ns1:OnBehalfOf" />)
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:if>
              agree to indemnify
              you <xsl:value-of select="$OrganisationName"/>, in respect of any damage that is caused in the course of a journey of which
              you have been notified under the Road Vehicles (Authorisation of Special Types)(General) Order 2003 (which is
              referred to below as "the 2003 Order").
            </td>
          </tr>
          <tr>
            <td colspan="2" valign="top" >
              2.
            </td>
            <td colspan="63">
              This indemnity relates to the journey scheduled to take place between 
              <xsl:element name="newdate">
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="ns1:JourneyTiming/ns2:FirstMoveDate"/>
                </xsl:call-template>
              </xsl:element> and <xsl:element name="newdate">
                <xsl:call-template name="FormatDate">
                  <xsl:with-param name="DateTime" select="ns1:JourneyTiming/ns2:LastMoveDate"/>
                </xsl:call-template>
              </xsl:element> starting with the date on which the indemnity was signed.
            </td>
          </tr>
          <tr>
            <td colspan="65" >
              <b>
                The damage covered:
              </b>
            </td>
          </tr>
          <tr>
            <td colspan="2" valign="top">
              3.
            </td>
            <td colspan="63">
              Except as stated in paragraph 4, the damage in respect of which this indemnity is given is limited to any
              damage caused to any road or bridge for the maintenance of which you are responsible.
            </td>
          </tr>

          <tr>
            <td colspan="2" valign="top">
              4.
            </td>
            <td colspan="63">
              This indemnity also extends to any damage caused to any other road or bridge that is used in the course of
              any journey to which the indemnity relates, in any case where a separate indemnity required by the 2003 Order
              has not been given to, or received by, the authority, body or person ("third party") which is responsible for the
              maintenance of that other road or bridge.
            </td>
          </tr>
          <tr>
            <td colspan="65" >
              <b>
                The cause of damage:
              </b>
            </td>
          </tr>
          <tr>
            <td colspan="2" valign="top">
              5.
            </td>
            <td colspan="63">
              The damage covered in this indemnity is limited to damage caused by - (a) the construction of any vehicle
              used; (b) the weight transmitted to the road surface by any vehicle used; (c) the dimensions, distribution or
              adjustment of the load carried on any vehicle used in the carriage of an abnormal indivisible load; (d) any vehicle
              other than the vehicle used in any case where that damage results from the vehicle used (but excluding any
              damage caused, or contributed to, by the negligence of the driver of the other vehicle).
            </td>
          </tr>
          <tr>
            <td colspan="65" >
              <b>
                Enforcement of indemnity:
              </b>
            </td>
          </tr>
          <tr>
            <td colspan="2" valign="top">
              6.
            </td>
            <td colspan="63">
              This indemnity is enforceable by you, to the extent of the damage specified in paragraph 3.
            </td>
          </tr>
          <tr>
            <td colspan="2" valign="top">
              7.
            </td>
            <td colspan="63">
              This indemnity is enforceable by any third party referred to in paragraph 4, in its own right, to the extent of any
              damage caused to any road or bridge for the maintenance of which it is responsible (but only if it has not already
              recovered payment in respect of that damage by virtue of a claim made by it under the equivalent provision in
              another indemnity given under the 2003 Order).
            </td>
          </tr>
          <tr>
            <td colspan="2" valign="top">
              8.
            </td>
            <td colspan="63">
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
              <b>Signed:</b>
              <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
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
</xsl:stylesheet>
