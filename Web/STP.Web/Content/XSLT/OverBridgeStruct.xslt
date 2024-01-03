<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"  xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="urn:my-scripts" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://www.esdal.com/schemas/core/routeanalysis" xmlns:b="http://www.esdal.com/schemas/core/structure" xmlns:c="http://www.esdal.com/schemas/core/formattedtext">

  <xsl:template match="/">
    <html xsl:version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://www.esdal.com/schemas/core/routeanalysis" xmlns:b="http://www.esdal.com/schemas/core/structure" xmlns:c="http://www.esdal.com/schemas/core/formattedtext">
      <msxsl:script implements-prefix='user' language='CSharp'>
        <![CDATA[
       ]]>
      </msxsl:script>
      <body>
        <div id='AffectedStructureXslt'>
          <div>
            <div class="cardSOA1 mt-0" style="margin:0 !important">
              <h6 class="text-color3 stHeading1 m-0">Select route part</h6>
            </div>
            <div class="col-9 c9p40Helper c9H1 pl-3">
              <div id="dlfId" class="flexer radioSOA mt-4 ml-0">
                <xsl:for-each select="/a:AnalysedStructures/a:AnalysedStructuresPart">
                  <xsl:variable name="routediv" select="concat('routediv',position())"/>

                  <div class="form-check">
                    <input class="form-check-input route_select" assesscheck ="0" type="radio" id="routeRadio" name="route_select" value="{$routediv}"></input>
                    <label class="form-check-label" for="flexRadioDefault11">
                      <span class="text-highlight">
                        Route part <countNo>
                          <xsl:value-of select="position()"/>
                        </countNo>
                      </span>
                      <span class="details1">
                        - <xsl:apply-templates select="a:Name/text()"/>
                      </span>
                    </label>
                  </div>
                </xsl:for-each>
              </div>
            </div>
          </div>
          <!--<div class="pl-0 main-sort-content">
				<div class="main-title">
					<span class="text-normal"> Affected </span>
				</div>
			</div>-->
          <br/>
          <div>
            <xsl:for-each select="/a:AnalysedStructures/a:AnalysedStructuresPart">
              <xsl:variable name="routediv" select="concat('routediv',position())"/>
              <xsl:variable name="routeId" select="@Id"/>
              <div id="{$routediv}" class="routedivclass" style="padding-left: 0px;" >
                <div>
                  <div class="cardSOA1 mt-0" style="margin:0 !important">
                    <h6 class="text-color3 stHeading1 m-0">
                      <label class="form-check-label" for="flexRadioDefault11">
                        <span class="text-color3 stHeading1">
                          Route part <countNo>
                            <xsl:value-of select="position()"/>
                          </countNo>
                        </span>
                        <span class="text-color3 stHeading1">
                          - <xsl:apply-templates select="a:Name/text()"/>
                        </span>
                      </label>
                    </h6>
                  </div>
                </div>
                <div class="main-table">
                  <table>
                    <thead>
                      <tr>
                        <th>Structure</th>
                        <th>Type</th>
                        <th>Suitability</th>
                        <th class="table-filter affected-structure-open-filter" style="width:10%;background-color: rgba(39, 87, 149, 0.1) !important;">
                          <img src="../Content/assets/images/filter-icon.svg" width="38"/>
                        </th>
                      </tr>
                    </thead>
                    <tbody>

                      <xsl:for-each select="./a:Structure">
                        <xsl:if test="./@TraversalType='overbridge'">
                          <tr>
                            <td class="text-color1">
                              <!--<a href="StructureGeneralDetails?StructureCode={a:ESRN/text()}" class="green">-->
                              <a class="text-decoration-link affected-structure-display-structure-gent" ESRN ="{a:ESRN/text()}" routeid="{$routeId}" sectionid="{@StructureSectionId}">
                                <xsl:value-of select="a:ESRN/text()"/>-
                                <xsl:apply-templates select="a:Name/text()"/>

                              </a>
                              <!--</a>-->
                            </td>
                            <td class="text-color1">
                              <!--<span class="spanimg overbridge"></span>-->
                              <img src="../Content/assets/images/Group 1716.svg" title="Overbridge" width="30"/>
                            </td>
                            <td class="text-color1">
                              <xsl:if test="a:Appraisal/a:Suitability/text()='Suitable' or a:Appraisal/a:Suitability/text()='suitable'">
                                <!--!='' Removed-->
                                <img src="../Content/assets/images/yes-icon.svg" width="20"/>
                                <span class="suitability true"></span>
                              </xsl:if>
                              <xsl:if test="a:Appraisal/a:Suitability/text()='Unsuitable' or a:Appraisal/a:Suitability/text()='unsuitable' ">
                                <!--!='' Removed-->
                                <img src="../Content/assets/images/no-icon.svg" width="20"/>
                                <span class="suitability false"></span>
                              </xsl:if>
                              <xsl:if test="a:Appraisal/a:Suitability/text()='Marginally suitable'or a:Appraisal/a:Suitability/text()='marginal'">
                                <!--!='' Removed-->
                                <span class="suitability question"></span>
                                <span style="font-size: 30px;">?</span>
                              </xsl:if>
                              <xsl:if test="a:Appraisal/a:Suitability/text()='unknown'">
                                <!--!='' Removed-->
                                <span></span>
                              </xsl:if>
                              <xsl:if test="a:Appraisal/a:Suitability/text()='erroneous'">
                                <!--!='' Removed-->
                                <span></span>
                              </xsl:if>
                            </td>
                          </tr>
                        </xsl:if>
                      </xsl:for-each>
                    </tbody>
                  </table>
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


