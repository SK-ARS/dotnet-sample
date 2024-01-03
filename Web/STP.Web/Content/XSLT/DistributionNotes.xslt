<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://www.esdal.com/schemas/core/agreedroute" xmlns:b="http://www.esdal.com/schemas/core/movement" xmlns:c="http://www.esdal.com/schemas/core/esdalcommontypes" xmlns:d="http://www.esdal.com/schemas/core/formattedtext">
  <xsl:param name="uppercase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:param name="lowercase" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:template match="/">
    <html xsl:version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://www.esdal.com/schemas/core/agreedroute" xmlns:b="http://www.esdal.com/schemas/core/movement" xmlns:c="http://www.esdal.com/schemas/core/esdalcommontypes" xmlns:d="http://www.esdal.com/schemas/core/formattedtext">

      <body>


        <xsl:for-each select="/a:AgreedRoute">

          <xsl:apply-templates select="b:DistributionComments/text()"/>

        </xsl:for-each>


      </body>

    </html>
  </xsl:template>
  <xsl:template match="b:Bold">
    <b>
      <xsl:value-of select="."/>
    </b>
  </xsl:template>

  <xsl:template match="text()">
    <xsl:value-of select="."/>
  </xsl:template>

</xsl:stylesheet>