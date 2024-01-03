<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"  xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="urn:my-scripts" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://www.esdal.com/schemas/core/routeanalysis" xmlns:b="http://www.esdal.com/schemas/core/structure" xmlns:c="http://www.esdal.com/schemas/core/formattedtext">
  <xsl:param name="OrgId"></xsl:param>
  <xsl:param name="UserTypeId"></xsl:param>
  <xsl:template match="/">
    <html xsl:version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://www.esdal.com/schemas/core/routeanalysis" xmlns:b="http://www.esdal.com/schemas/core/structure" xmlns:c="http://www.esdal.com/schemas/core/formattedtext">
      <msxsl:script implements-prefix='user' language='CSharp'>
        <![CDATA[
       ]]>
      </msxsl:script>
      <body>
        <div id='AffectedStructureXslt' class='mt-0 ml-0 mb-0'>
          <div>
            <div class="cardSOA1 mt-0" style="margin:0 !important">
              <h6 class="text-color3 stHeading1 m-0">Select route part</h6>
            </div>
            <div class="col-9 c9p40Helper c9H1 pl-3">
              <div id="dlfId" class="flexer radioSOA mt-4 ml-0">
                <xsl:for-each select="/a:AnalysedStructures/a:AnalysedStructuresPart">
                  <xsl:variable name="routediv" select="concat('routediv',position())"/>

                  <div class="form-check">
                    <xsl:variable name="routePartId" select="@Id"/>
                    <input type="hidden" class="{$routediv}" name="RoutePartId" id="routePartId" value="{$routePartId}"/>
                    <!--<input type="hidden" id="routePartId" name="RoutePartsId"/>-->
                    <xsl:variable name="routePart" select="position()"/>
                    <input class="form-check-input route_select" assesscheck ="0" type="radio" id="routeRadio" name="route_select" value="{$routediv}"></input>
                    <label class="form-check-label">
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
          <br/>
          <div>
            <xsl:for-each select="/a:AnalysedStructures/a:AnalysedStructuresPart">
              <xsl:variable name="routediv" select="concat('routediv',position())"/>
              <xsl:variable name="routeId" select="@Id"/>
              <div id="{$routediv}" class="routedivclass" style="padding-left: 0px;" >
                <input type="hidden" name="RouteId" value="{$routeId}"/>
                <div>
                  <div class="cardSOA1 mt-0" style="margin:0 !important">
                    <h6 class="text-color3 stHeading1 m-0">
                      <label class="form-check-label">
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
                        <xsl:if test="$UserTypeId=696007">
                          <th style="width:13%" class="chk_SelectAll">
                            <xsl:variable name="chk_SelectAll" select="concat('chk_SelectAll',position())"/>
                            <input type="checkbox" id="{$chk_SelectAll}" divid="{$routediv}" class="checkbox mr-2 affected-structure-select-unselect-all" name="submitted" value="submitted" width="20"></input>
                            <span class="text-normal"
                                style="vertical-align: super;color:white">Select all</span>
                          </th>
                          <th></th>
                        </xsl:if>
                        <th>Structure</th>
                        <th>Type</th>
                        <th>Suitability</th>
                        <xsl:if test="$UserTypeId=696007">
                          <th style="width:12%">Assessment Result</th>
                          <th style="width:7%">Remarks</th>
                        </xsl:if>
                      </tr>
                    </thead>
                    <tbody>
                      <xsl:variable name="set" select="./a:Structure" />
                      <xsl:variable name="structCount" select="count($set)" />
                      <input id="structuresCount" type="hidden" value="{$structCount}"/>
                      <xsl:for-each select="$set">
                        <xsl:variable name="sectionId" select="@StructureSectionId"/>
                        <xsl:if test="./a:StructureResponsibility/a:StructureResponsibleParty/@OrganisationId=$OrgId">
                          <xsl:variable name="structNo" select="position()"/>
                          <tr id="StructureAssessment">
                            <xsl:if test="$UserTypeId=696007">
                              <td class="chk_SelectAll">
                                <xsl:if test="./@TraversalType='underbridge'">
                                  <input type="checkbox" id="{a:ESRN/text()}_{$routeId}" name="struct_select" class="struct_select affected-structure-list-struct-ids" structNo ="{$structNo}" ESRN="{a:ESRN/text()}" routeId ="{$routeId}" sectionid="{@StructureSectionId}" structCount="{$structCount}" divId="{$routediv}" />
                                </xsl:if>
                              </td>
                              <td>
                                <div id="status_{a:ESRN/text()}_{$routeId}">
                                  <!--<img src="../Content/Images/Common/status_loading.gif" class="currently-loading"/>-->
                                </div>
                              </td>
                            </xsl:if>
                            <td class="text-color1 pl-2">
                              <input type="hidden" name="SectionId" value="{$sectionId}"/>
                              <a class="text-decoration-link affected-structure-display-structure-gent" ESRN ="{a:ESRN/text()}" routeid="{$routeId}" sectionid="{@StructureSectionId}">
                                <xsl:value-of select="a:ESRN/text()"/>-
                                <xsl:apply-templates select="a:Name/text()"/>
                              </a>
                              <!--</a>-->
                            </td>
                            <td class="text-color1">
                              <xsl:if test="./@TraversalType='underbridge'">
                                <!--<span class="spanimg underbridge"></span>-->
                                <img src="../Content/assets/images/Group 1719.svg" title="Underbridge" width="30"/>
                              </xsl:if>
                              <xsl:if test="./@TraversalType='overbridge'">
                                <!--<span class="spanimg overbridge"></span>-->
                                <img src="../Content/assets/images/Group 1716.svg" title="Overbridge" width="30"/>
                              </xsl:if>
                              <xsl:if test="./@TraversalType='level crossing'">
                                <span class="spanimg levelcross"></span>
                              </xsl:if>
                            </td>
                            <td class="text-color1">
                              <xsl:if test="a:Appraisal/a:Suitability/text()='Suitable' or a:Appraisal/a:Suitability/text()='suitable'">
                                <!--!='' Removed-->
                                <img src="../Content/assets/images/yes-icon.svg" width="20" title="Suitable" />
                                <!--<span class="suitability true"></span>-->
                              </xsl:if>
                              <xsl:if test="a:Appraisal/a:Suitability/text()='Unsuitable' or a:Appraisal/a:Suitability/text()='unsuitable' ">
                                <!--!='' Removed-->
                                <img src="../Content/assets/images/no-icon.svg" width="20" title="Unsuitable"/>
                                <!--<span class="suitability false"></span>-->
                              </xsl:if>
                              <xsl:if test="a:Appraisal/a:Suitability/text()='Marginally suitable'or a:Appraisal/a:Suitability/text()='marginal'">
                                <!--!='' Removed-->
                                <!--<span class="suitability question"></span>-->
                                <span style="font-size: 30px;" title="Marginal structure">?</span>
                              </xsl:if>

                              <!-- exclamation-->
                              <xsl:if test="a:Appraisal/a:Suitability/text()='Cannot be performed for side-by-side configuration'">
                                <!--!='' Removed-->
                                <span style="font-size: 30px; color: red;" title="Cannot be performed for side-by-side configuration">!</span>
                              </xsl:if>
                              <xsl:if test="a:Appraisal/a:Suitability/text()='Cannot be performed for 3 or more components'">
                                <!--!='' Removed-->
                                <span style="font-size: 30px; color: red;" title="Cannot be performed for 3 or more components">!</span>
                              </xsl:if>
                              <xsl:if test="a:Appraisal/a:Suitability/text()='Minimum axle spacing not found'">
                                <!--!='' Removed-->
                                <span style="font-size: 30px; color: red;" title="Minimum axle spacing not found">!</span>
                              </xsl:if>
                              <xsl:if test="a:Appraisal/a:Suitability/text()='Cannot be performed for tractors with 4 or more axles'">
                                <!--!='' Removed-->
                                <span style="font-size: 30px; color: red;" title="Cannot be performed for tractors with 4 or more axles">!</span>
                              </xsl:if>
                              <xsl:if test="a:Appraisal/a:Suitability/text()='Can be performed only for underbridges'">
                                <!--!='' Removed-->
                                <span style="font-size: 30px; color: red;" title="Can be performed only for underbridges">!</span>
                              </xsl:if>
                              <xsl:if test="a:Appraisal/a:Suitability/text()='Gross weight or axle weight capacity of structure not available' or a:Appraisal/a:Suitability/text()='unknown'">
                                <!--!='' Removed-->
                                <span style="font-size: 30px; color: red;" title="Gross weight or axle weight capacity of structure not available">!</span>
                              </xsl:if>
                              <xsl:if test="a:Appraisal/a:Suitability/text()='Vehicle not suitable for SV Train'">
                                <!--!='' Removed-->
                                <span style="font-size: 30px; color: red;" title="Vehicle not suitable for SV Train">!</span>
                              </xsl:if>
                              <xsl:if test="a:Appraisal/a:Suitability/text()='Vehicle does not belong to STGO category 1'">
                                <!--!='' Removed-->
                                <span style="font-size: 30px; color: red;" title="Vehicle does not belong to STGO category 1">!</span>
                              </xsl:if>
                              <xsl:if test="a:Appraisal/a:Suitability/text()='Axle weight capacity of structure is not available'">
                                <!--!='' Removed-->
                                <span style="font-size: 30px; color: red;" title="Axle weight capacity of structure is not available">!</span>
                              </xsl:if>
                              <xsl:if test="a:Appraisal/a:Suitability/text()='Cannot perform axle weight screening'">
                                <!--!='' Removed-->
                                <span style="font-size: 30px; color: red;" title="Cannot perform axle weight screening">!</span>
                              </xsl:if>
                              <xsl:if test="a:Appraisal/a:Suitability/text()='Structure width or length is not available'">
                                <!--!='' Removed-->
                                <span style="font-size: 30px; color: red;" title="Structure width or length is not available">!</span>
                              </xsl:if>
                              <xsl:if test="a:Appraisal/a:Suitability/text()='Gross weight capacity of structure is not available'">
                                <!--!='' Removed-->
                                <span style="font-size: 30px; color: red;" title="Gross weight capacity of structure is not available">!</span>
                              </xsl:if>
                              <xsl:if test="a:Appraisal/a:Suitability/text()='Cannot perform gross weight screening'">
                                <!--!='' Removed-->
                                <span style="font-size: 30px; color: red;" title="Cannot perform gross weight screening">!</span>
                              </xsl:if>
                            </td>
                            <xsl:if test="$UserTypeId=696007">
                              <td class="text-color1" style="text-align: center;">
                                <xsl:if test="./a:StructureResponsibility/a:StructureResponsibleParty/@OrganisationId=$OrgId">
                                  <xsl:if test="a:AlsatAppraisal/a:ResultStructure/text()!=''">
                                    <xsl:choose>
                                      <xsl:when test="a:AlsatAppraisal/a:CommentsForHaulier/text()!=''">
                                        <a class="text-decoration-link affected-structure-show-note-to-haulier" hauliertext="{a:AlsatAppraisal/a:CommentsForHaulier/text()}" ESRN="{ a:ESRN/text()}">
                                          <div class="text-uppercase">
                                            <xsl:value-of select="a:AlsatAppraisal/a:ResultStructure/text()"/>
                                          </div>
                                        </a>
                                      </xsl:when>
                                      <xsl:otherwise>
                                        <xsl:value-of select="a:AlsatAppraisal/a:ResultStructure/text()"/>
                                      </xsl:otherwise>
                                    </xsl:choose>
                                  </xsl:if>
                                </xsl:if>
                              </td>
                              <td class="text-color1" style="text-align: center;">
                                <xsl:if test="./a:StructureResponsibility/a:StructureResponsibleParty/@OrganisationId=$OrgId">
                                  <xsl:if test="a:AlsatAppraisal/a:AssessmentComments/text()!=''">
                                    <a class="text-decoration-link affected-structure-view-assess-comment" assesscomments ="{a:AlsatAppraisal/a:AssessmentComments/text()}" ESRN ="{a:ESRN/text()}">
                                      View
                                    </a>
                                  </xsl:if>
                                </xsl:if>
                              </td>
                            </xsl:if>

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


