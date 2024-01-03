<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://www.esdal.com/schemas/common/movementversion" xmlns:b="http://www.esdal.com/schemas/core/formattedtext">
  <xsl:variable name="_crlf">
    <xsl:text>
</xsl:text>
  </xsl:variable>
  <xsl:variable name="crlf" select="string($_crlf)"/>
  <xsl:template match="/">
    <html xsl:version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://www.esdal.com/schemas/common/movementversion" xmlns:b="http://www.esdal.com/schemas/core/formattedtext">
      <body>
        <xsl:apply-templates/>
      </body>
    </html>
  </xsl:template>
  <xsl:template match="b:Div">
    <br/>
    <div>
      <xsl:apply-templates select="./* | ./text()"/>
    </div>
  </xsl:template>

  <xsl:template match="b:Para">
    <p>
      <xsl:apply-templates select="./* | ./text()"/>
    </p>
  </xsl:template>

  <xsl:template match="b:Bold">
    <xsl:if test="local-name(parent::*) = 'Underscore'" >
    </xsl:if>
    <b>
      <xsl:apply-templates select="./* | ./text()"/>
    </b>
    <xsl:if test="local-name(parent::*) = 'Underscore'" >
    </xsl:if>
  </xsl:template>

  <xsl:template match="b:Italic">
    <i>
      <xsl:apply-templates select="./* | ./text()"/>
    </i>
  </xsl:template>

  <xsl:template match="b:Underscore">
    <xsl:if test="local-name(parent::*) = 'Bold'" >
    </xsl:if>
    <u>
      <xsl:apply-templates select="./* | ./text()"/>
    </u>
    <xsl:if test="local-name(parent::*) = 'Bold'" >
    </xsl:if>
  </xsl:template>

  <xsl:template match="b:BulletedText">
    <!--<br/>-->
    <li>
      <xsl:choose>
        <xsl:when test="local-name(child::*) = 'Italic'">
          <i>
            <xsl:apply-templates select="./* | ./text()"/>
          </i>
        </xsl:when>
        <xsl:when test="local-name(child::*) = 'Bold'">
          <b>
            <xsl:apply-templates select="./* | ./text()"/>
          </b>
        </xsl:when>
        <xsl:when test="local-name(child::*) = 'Underscore'">
          <u>
            <xsl:apply-templates select="./* | ./text()"/>
          </u>
        </xsl:when>
        <xsl:otherwise>
          <xsl:call-template name="crlf-replace">
            <xsl:with-param name="textdata" select="."/>
          </xsl:call-template>
        </xsl:otherwise>
      </xsl:choose>
    </li>
  </xsl:template>

  <xsl:template match="b:Br">
    <br/>
  </xsl:template>

  <xsl:template match="Br">
    <br/>
  </xsl:template>

  <xsl:template match="text()">
    <xsl:call-template name="crlf-replace">
      <xsl:with-param name="textdata" select="."/>
    </xsl:call-template>
  </xsl:template>


  <xsl:template name="crlf-replace">
    <xsl:param name="textdata"/>
    <xsl:choose>
      <xsl:when test="contains($textdata, $crlf)">
        <xsl:value-of select="substring-before($textdata, $crlf)"/>
        <br/>
        <xsl:call-template name="crlf-replace">
          <xsl:with-param name="textdata" select="substring-after($textdata, $crlf)"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$textdata"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>