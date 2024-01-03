<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://www.esdal.com/schemas/core/routeanalysis" xmlns:b="http://www.esdal.com/schemas/core/constraint" xmlns:c="http://www.esdal.com/schemas/core/caution" xmlns:d="http://www.esdal.com/schemas/core/contact" xmlns:g="http://www.esdal.com/schemas/core/esdalcommontypes" xmlns:f="http://www.esdal.com/schemas/core/formattedtext">
  <xsl:param name="OrgId"></xsl:param>
  <xsl:param name="UserTypeId"></xsl:param>
  <xsl:template match="/">
    <html xsl:version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://www.esdal.com/schemas/core/routeanalysis" xmlns:b="http://www.esdal.com/schemas/core/constraint" xmlns:c="http://www.esdal.com/schemas/core/caution" xmlns:d="http://www.esdal.com/schemas/core/contact" xmlns:g="http://www.esdal.com/schemas/core/esdalcommontypes" xmlns:f="http://www.esdal.com/schemas/core/formattedtext">
      <body>

        <div class="row pl-3">
          <div class="cardSOA1 mt-0">
            <h6 class="text-color3 stHeading1 m-0">Select route part</h6>
          </div>
          <div class="col-9 c9p40Helper c9H1 pl-0">
            <div id="dlfId" class="flexer radioSOA mt-4 ml-0">
              <xsl:for-each select="/a:AnalysedConstraints/a:AnalysedConstraintsPart">
                <xsl:variable name="routedivcon" select="concat('routedivcon',position())"/>
                <div class="form-check">
                  <input type="radio" name="route_select" class="route_select form-check-input" value="{$routedivcon}"/>
                  <label class="form-check-label" for="flexRadioDefault11">
                    <span class="text-highlight">
                      Route part <countNo>
                        <xsl:value-of select="position()"/>
                      </countNo> -
                    </span>
                    <span class="details1">
                      <xsl:apply-templates select="a:Name/text()"/>
                    </span>
                  </label>
                </div>

              </xsl:for-each>
            </div>
          </div>

          <div class="row pl-0 pr-0">
            <xsl:for-each select="/a:AnalysedConstraints/a:AnalysedConstraintsPart">
              <xsl:variable name="routedivcon" select="concat('routedivcon',position())"/>
              <div id="{$routedivcon}" class="routedivclasscon pl-0 pr-0">
                <div class="cardSOA1">
                  <h6 class="text-color3 stHeading1 m-0">
                    Route part <countNo>
                      <xsl:value-of select="position()"/>
                    </countNo>
                    - (<xsl:apply-templates select="a:Name/text()"/>)
                  </h6>
                </div>
                <xsl:choose>
                  <xsl:when test="./a:Constraint">
                    <div class="mt-2 pl-0">
                      <xsl:for-each select="./a:Constraint">
                        <div class="row mt-2 ml-0">
                          <div class="col-lg-2 col-sm-3 col-md-3  pl-0">
                            <span class="details">ECRN</span>
                          </div>
                          <div class="col-lg-3 col-sm-3 col-md-3">
                            <span class="details1">
                              <xsl:apply-templates select="b:ECRN/text()"/>
                            </span>
                          </div>
                        </div>
                        <div class="row mt-2 ml-0">
                          <div class="col-lg-2 col-sm-3 col-md-3 pl-0">
                            <span class="details">Name</span>
                          </div>
                          <div class="col-lg-3 col-sm-3 col-md-3">
                            <span class="details1">
                              <xsl:apply-templates select="b:Name/text()"/>
                            </span>
                          </div>
                        </div>
                        <div class="row mt-2 ml-0">
                          <div class="col-lg-2 col-sm-3 col-md-3 pl-0">
                            <span class="details">Type</span>
                          </div>
                          <div class="col-lg-3 col-sm-3 col-md-3">
                            <span class="detailsFormatter">
                              <xsl:apply-templates select="b:Type/text()"/>
                            </span>
                          </div>
                          <div class="col-lg-1 col-sm-3 col-md-3">
                            <img src="../Content/assets/images/Group 1077.svg" width="28"></img>
                          </div>
                          <div class="col-lg-2 col-sm-3 col-md-3">
                            <a class="text-normal-hyperlink btn_ViewConstraints" data-ecrn="{b:ECRN/text()}" style="cursor:pointer;">View details</a>
                          </div>
                        </div>
                        <div class="row mt-2 ml-0">
                          <div class="col-lg-2 col-sm-3 col-md-3 pl-0">
                            <span class="details">Suitability</span>
                          </div>
                          <div class="col-lg-3 col-sm-3 col-md-3">
                            <xsl:if test="a:Appraisal/a:Suitability/text()='Unsuitable'">
                              <span id="unsuitableConstraint"></span>
                            </xsl:if>
                            <span class="details1" >
                              <xsl:apply-templates select="a:Appraisal/a:Suitability/text()"/>
                            </span>
                          </div>
                        </div>
                        <hr/>
                      </xsl:for-each>
                    </div>
                  </xsl:when>
                  <xsl:otherwise>
                    <div class="flexer radioSOA mt-4 ml-0">
                      <span class="text-highlight">
                        <b style="color:#414193;">
                          <xsl:if test="$UserTypeId=696001">
                            No Unsuitable Constraints
                          </xsl:if>
                          <xsl:if test="$UserTypeId!=696001">
                            No Constraints
                          </xsl:if>
                        </b>
                      </span>
                    </div>
                  </xsl:otherwise>
                </xsl:choose>
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


