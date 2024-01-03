<?xml version="1.0" encoding="UTF-8" ?>

<xsl:stylesheet version="2.0"
    xmlns:exsl="http://exslt.org/common"
    xmlns:math="http://exslt.org/math"
    extension-element-prefixes="exsl math"
    xmlns:o="urn:schemas-microsoft-com:office:office"
    xmlns:w="urn:schemas-microsoft-com:office:word"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ns1="http://www.esdal.com/schemas/core/specialorder"
    xmlns:ns2="http://www.esdal.com/schemas/core/movement"
    xmlns:ns3="http://www.esdal.com/schemas/core/esdalcommontypes"
    xmlns:ns4="http://www.govtalk.gov.uk/people/AddressAndPersonalDetails">

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
      <body style="font-family:Arial;font-size:12px">
        <p>
          <b>
            ORDER No.
            <xsl:value-of select="ns2:OrderNumber"/>
          </b>
        </p>
        <p>
          <b>ROAD TRAFFIC ACT 1988</b>
        </p>
        <p>
          <b>ORDER OF THE SECRETARY OF STATE UNDER SECTION 44(1)</b>
        </p>
        <p>
          The Secretary of State in exercise of his powers under sub-sections (1) and (3) of section 44 of the Road Traffic Act 1988 hereby makes the following Order.
        </p>
        <P>
          <BR></BR>
          <BR></BR>
        </P>
        <P>
          <B>
            <U>
              Place holder for text to be entered by SORT
            </U>
          </B>
        </P>
        <P>
          <BR></BR>
          <BR></BR>
        </P>
        <p>
          *Bank Holiday Dates*:
          <br/> <br/>
        </p>
        <table style="font-family:Arial;font-size:12px">
          <xsl:for-each select="ns1:BankHolidayExclusion">
            <xsl:if test="ns1:End/ns1:Time!='' and ns1:Start/ns1:Day!='' and ns1:Start/ns1:Date!='' and ns1:End/ns1:Day!='' and ns1:End/ns1:Date!=''">
              <p style="padding-left: 18px;">
                from
                <xsl:value-of select="ns1:End/ns1:Time"/>
                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                <xsl:value-of select="ns1:Start/ns1:CustomStartDate"/>
                to
                <xsl:value-of select="ns1:End/ns1:Time"/>
                <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
                <xsl:value-of select="ns1:End/ns1:CustomEndDate"/>
              </p >
            </xsl:if >
          </xsl:for-each>
        </table>
        <p>
          Signed by authority of the Secretary of State on
          <xsl:value-of select="ns1:SigningDetails/ns2:CustomSigningDate"/>
        </p>
        <p>
          <xsl:call-template name="CamelCase">
            <xsl:with-param name="text">
              <xsl:value-of select="ns1:SigningDetails/ns2:Signatory"/>
            </xsl:with-param>
          </xsl:call-template>
        </p>
        <p>
          <xsl:value-of select="ns1:SigningDetails/ns2:SignatoryRole"/>
        </p>
        <br/>
        <br/>
        Ref:<xsl:value-of select="ns2:JobFileReferenceNumber"/>

        <br/>
        <br/>
        <br/>
        <xsl:value-of select="ns2:ESDALReferenceNumber/ns2:Mnemonic"/>/<xsl:value-of select="ns2:ESDALReferenceNumber/ns2:MovementProjectNumber"/>/S<xsl:value-of select="ns2:ESDALReferenceNumber/ns2:MovementVersion"/>
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
