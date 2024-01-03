<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://www.esdal.com/schemas/core/agreedroute" xmlns:b="http://www.esdal.com/schemas/core/movement" xmlns:c="http://www.esdal.com/schemas/core/esdalcommontypes" xmlns:d="http://www.esdal.com/schemas/core/formattedtext">
  <xsl:param name="uppercase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
  <xsl:param name="lowercase" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:template match="/">
    <html xsl:version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://www.esdal.com/schemas/core/agreedroute" xmlns:b="http://www.esdal.com/schemas/core/movement" xmlns:c="http://www.esdal.com/schemas/core/esdalcommontypes" xmlns:d="http://www.esdal.com/schemas/core/formattedtext">

      <body>
        <div id="contact-details">
          <div class="col-lg-7 col-sm-12 col-md-5 pl-6 pb-6">
            <div class="heading-medium pt-3 pb-3">
              Contact details
            </div>
            <xsl:for-each select="/a:AgreedRoute/b:HAContact">
              <div class="row pb-3">
                <div class="col-sm-4 col-md-6 col-lg-3 text-normal">
                  Contact name
                </div>
                <div
                    class="col-sm-4 col-md-5 col-lg-6 input-field edit-normal pt-0  pb-0 pl-0 pr-0">
                  <xsl:apply-templates select="b:Contact/text()"/>
                </div>
              </div>
              <div class="row pb-3">
                <div class="col-sm-4 col-md-6 col-lg-3 text-normal">
                  Address
                </div>
                <div
                    class="col-sm-4 col-md-5 col-lg-6 input-field edit-normal pt-0  pb-0 pl-0 pr-0">
                  <xsl:for-each select="./b:Address/c:Line">

                    <span class="marginleft9">
                      <xsl:apply-templates select="text()"/>
                    </span>
                    <br></br>
                  </xsl:for-each>
                </div>
              </div>
              <div class="row pb-1">
                <div class="col-sm-4 col-md-6 col-lg-3 text-normal">
                  Country
                </div>
                <div class="col-sm-4 col-md-5 col-lg-6 input-field edit-normal pt-0  pb-0 pl-0 pr-0" style="text-transform:capitalize;">

                  <xsl:apply-templates select="b:Address/c:Country/text()"/>

                </div>
              </div>
              <div class="row pb-1">
                <div class="col-sm-4 col-md-6 col-lg-3 text-normal">
                  Postcode
                </div>
                <div
                    class="col-sm-4 col-md-5 col-lg-6 input-field edit-normal pt-0  pb-0 pl-0 pr-0">
                  <xsl:apply-templates select="b:Address/c:PostCode/text()"/>
                </div>
              </div>
              <div class="row pb-1">
                <div class="col-sm-4 col-md-6 col-lg-3 text-normal">
                  Telephone
                </div>
                <div
                    class="col-sm-4 col-md-5 col-lg-6 input-field edit-normal pt-0  pb-0 pl-0 pr-0">
                  <xsl:apply-templates select="b:TelephoneNumber/text()"/>
                </div>
              </div>
              <div class="row pb-1">
                <div class="col-sm-4 col-md-6 col-lg-3 text-normal">
                  Fax
                </div>
                <div
                    class="col-sm-4 col-md-5 col-lg-6 input-field edit-normal pt-0  pb-0 pl-0 pr-0">
                  <xsl:apply-templates select="b:FaxNumber/text()"/>
                </div>
              </div>
              <div class="row pb-1">
                <div class="col-sm-4 col-md-6 col-lg-3 text-normal">
                  E-mail
                </div>
                <div
                    class="col-sm-4 col-md-5 col-lg-6 input-field edit-normal pt-0  pb-0 pl-0 pr-0">
                  <xsl:apply-templates select="b:EmailAddress/text()"/>
                </div>
              </div>
            </xsl:for-each>
          </div>
        </div>
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
