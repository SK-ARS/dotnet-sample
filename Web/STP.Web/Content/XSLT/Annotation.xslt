<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://www.esdal.com/schemas/core/routeanalysis" xmlns:b="http://www.esdal.com/schemas/core/formattedtext" xmlns:c="http://www.esdal.com/schemas/core/annotation" xmlns:d="http://www.esdal.com/schemas/core/esdalcommontypes">

  <xsl:template match="/">
    <html xsl:version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://www.esdal.com/schemas/core/routeanalysis" xmlns:b="http://www.esdal.com/schemas/core/formattedtext" xmlns:c="http://www.esdal.com/schemas/core/annotation" xmlns:d="http://www.esdal.com/schemas/core/esdalcommontypes">
      <body>

        <div class="row">
          <div class="cardSOA1 mt-0">
            <h6 class="text-color3 stHeading1 m-0">Select route part</h6>
          </div>
          <div class="col-9 c9p40Helper c9H1 pl-0">
            <div id="dlfId" class="flexer radioSOA mt-4 ml-0">
              <xsl:for-each select="/a:AnalysedAnnotations/a:AnalysedAnnotationsPart">
                <xsl:variable name="routedivannot" select="concat('routedivannot',position())"/>

                <div class="form-check">
                  <input  type="radio" id="routeRadio" name="route_selectannot" class="form-check-input route_selectannot" value="{$routedivannot}"></input>
                  <label class="form-check-label" for="flexRadioDefault11">
                    <span class="text-highlight">
                      Route part <countNo>
                        <xsl:value-of select="position()"/>
                      </countNo>
                    </span>
                    <span class="details1">
                      - (<xsl:apply-templates select="a:Name/text()"/>)
                    </span>
                  </label>
                </div>
              </xsl:for-each>
            </div>
          </div>
        </div>

        <div class="row">
          <xsl:for-each select="/a:AnalysedAnnotations/a:AnalysedAnnotationsPart">
            <xsl:variable name="routedivannot" select="concat('routedivannot',position())"/>
            <div id="{$routedivannot}" class="routedivclassannot pl-0 pr-0" >
              <div class="cardSOA1">
                <h6 class="text-color3 stHeading1 m-0">
                  Route part <countNo>
                    <xsl:value-of select="position()"/>
                  </countNo>
                  - (<xsl:apply-templates select="a:Name/text()"/>)
                </h6>
              </div>

              <xsl:choose>
                <xsl:when test="./a:Annotation">
                  <div class="mt-2 pl-0">
                    <xsl:for-each select="a:Annotation">
                      <div class="pb-4">
                        <span class="details1">
                          Grid Reference  : <xsl:apply-templates select="a:AnnotatedEntity/a:Road/a:OSGridRef/text()"/>
                        </span>
                      </div>
                      <div class="textFont">
                        <p class="details1 pb-2">
                          <xsl:if test="a:Road/d:Number/text() != ''">
                            Road number   : <xsl:apply-templates select="a:Road/d:Number/text()"/>
                          </xsl:if>
                        </p>
                        <p class="details1 pb-2">
                          <xsl:if test ="a:Road/d:Name/text() != ''">
                            Road name     : <xsl:apply-templates select="a:Road/d:Name/text()"/>
                          </xsl:if>
                          <br/>
                          <xsl:apply-templates select="c:Text/text() "/>
                        </p>
                        <hr/>

                      </div>
                    </xsl:for-each>
                  </div>
                </xsl:when>
                <xsl:otherwise>
                  <div class="flexer radioSOA mt-4 ml-0">
                    <span class="text-highlight">
                      <b style="color:#414193;">
                        No Annotations
                      </b>
                    </span>
                  </div>
                </xsl:otherwise>
              </xsl:choose>
            </div>
          </xsl:for-each>
        </div>
        <br/>
      </body>
    </html>
  </xsl:template>

  <xsl:template match="a:DisplayMetric">
    <xsl:if test="./text() div 1000>=0">
      <xsl:value-of select="./text() div 1000"/> Km
    </xsl:if>
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


