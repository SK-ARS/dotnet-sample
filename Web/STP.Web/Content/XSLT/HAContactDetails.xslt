<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://www.esdal.com/schemas/core/agreedroute" xmlns:b="http://www.esdal.com/schemas/core/movement" xmlns:c="http://www.esdal.com/schemas/core/esdalcommontypes" xmlns:d="http://www.esdal.com/schemas/core/formattedtext">
  <xsl:param name="uppercase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:param name="lowercase" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:template match="/">
    <html xsl:version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://www.esdal.com/schemas/core/agreedroute" xmlns:b="http://www.esdal.com/schemas/core/movement" xmlns:c="http://www.esdal.com/schemas/core/esdalcommontypes" xmlns:d="http://www.esdal.com/schemas/core/formattedtext">

      <body>

        <table class="margin0px frmbdr" style="margin-top: -13px !important;">
          <tbody>

            <xsl:for-each select="/a:AgreedRoute/b:HAContact">
              <tr style="border-top:1px border-bottom:1px solid #cccce2;">
                <td >Name</td>
                <td style="font-weight:bold;">
                  <span class="spantd1">:</span>
                  <span class="spantd">
                    <xsl:apply-templates select="b:Contact/text()"/>
                  </span>
                </td>
              </tr>
              <tr>
                <td style="vertical-align: top; width: 100px;">Address</td>
                <td style="font-weight:bold;">
                  <span class="spantd1" style="margin-right: -5px;">:</span>

                  <xsl:for-each select="./b:Address/c:Line">

                    <span class="marginleft9">
                      <xsl:apply-templates select="text()"/>
                    </span>
                    <br></br>
                  </xsl:for-each>
                </td>
              </tr>
              <tr>
                <td>
                  Postcode
                </td>
                <td style="font-weight:bold;">
                  <span class="spantd1">:</span>
                  <span class="spantd">
                    <xsl:apply-templates select="b:Address/c:PostCode/text()"/>
                  </span>
                </td>
              </tr>
              <tr>
                <td>
                  Country
                </td>
                <td style="font-weight:bold;">
                  <span class="spantd1">:</span>
                  <span class="spantd" style="text-transform:capitalize;">
                    <xsl:apply-templates select="b:Address/c:Country/text()"/>

                  </span>
                </td>
              </tr>
              <tr>
                <td>
                  Telephone
                </td>
                <td style="font-weight:bold;">
                  <span class="spantd1">:</span>
                  <span class="spantd">
                    <xsl:apply-templates select="b:TelephoneNumber/text()"/>
                  </span>
                </td>
              </tr>
              <tr>
                <td>
                  Fax
                </td>
                <td style="font-weight:bold;">
                  <span class="spantd1">:</span>
                  <span class="spantd">
                    <xsl:apply-templates select="b:FaxNumber/text()"/>
                  </span>
                </td>
              </tr>
              <tr>
                <td>
                  Email
                </td>
                <td style="font-weight:bold;">
                  <span class="spantd1">:</span>
                  <span class="spantd">
                    <xsl:apply-templates select="b:EmailAddress/text()"/>
                  </span>
                </td>
              </tr>
              <!--</tr>-->
            </xsl:for-each>
          </tbody>
        </table>
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


