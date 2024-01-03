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
                    <input type="hidden" class="{$routediv}" name="RoutePartId" value="{$routePartId}" id="routePartId"/>
                    <!--<input type="hidden" id="routePartId" name="RoutePartsId"/>-->
                    <xsl:variable name="routePart" select="position()"/>
                    <input class="form-check-input route_select" assesscheck ="1" type="radio" id="routeRadio" name="route_select" value="{$routediv}"></input>
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
          <!--<div class="pl-0 main-sort-content">
            <div class="main-title">
              <span class="text-normal"> Affected Structures </span>
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
					<div class="main-table" style="border-radius:25px 0px 0px 0px">
						<table>
							<thead>
								<tr>
									<xsl:if test="$UserTypeId=696007">
										<th style="width:14%">
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
									<xsl:if test="$UserTypeId != 696007 and $UserTypeId != 696002">
									<th class="table-filter affected-structure-open-filter" style="width:10%;background-color: rgba(39, 87, 149, 0.1) !important;">
									  <img src="../Content/assets/images/filter-icon.svg" width="38"/>
									</th>
								  </xsl:if>
								</tr>
							</thead>
							<tbody>
                <xsl:choose>
									<xsl:when test="a:Structure">
								<xsl:variable name="set" select="./a:Structure" />
								<xsl:variable name="structCount" select="count($set)" />
								<input id="structuresCount" type="hidden" value="{$structCount}"/>
								<xsl:for-each select="$set">
									<xsl:variable name="structNo" select="position()"/>

									<tr id="StructureAssessment">
										<xsl:if test="$UserTypeId=696007">
											<td>
												<xsl:if test="./a:StructureResponsibility/a:StructureResponsibleParty/@OrganisationId=$OrgId">
													<xsl:if test="./@TraversalType='underbridge'">
														<input type="checkbox" id="{a:ESRN/text()}_{$routeId}" name="struct_select" class="struct_select affected-structure-list-struct-ids" structNo ="{$structNo}" ESRN="{a:ESRN/text()}" routeId ="{$routeId}" sectionid="{@StructureSectionId}" structCount="{$structCount}" divId="{$routediv}" />
													</xsl:if>
												</xsl:if>
											</td>
											<td>
												<div id="status_{a:ESRN/text()}_{$routeId}">
													<!--<img src="../Content/Images/Common/status_loading.gif" class="currently-loading"/>-->
												</div>
											</td>
										</xsl:if>
										<td class="text-color1 pl-2">
											<!--<a href="StructureGeneralDetails?StructureCode={a:ESRN/text()}" class="green">-->
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
										<td class="text-color1" style="text-align: center;">
											<xsl:if test="a:Appraisal/a:Suitability/text()='Suitable' or a:Appraisal/a:Suitability/text()='suitable'">
												<!--!='' Removed-->
												<img src="../Content/assets/images/yes-icon.svg" title="Suitable" width="20"/>
												<span class="suitability true" id="suitableConstraint"></span>
											</xsl:if>
											<xsl:if test="a:Appraisal/a:Suitability/text()='Unsuitable' or a:Appraisal/a:Suitability/text()='unsuitable' ">
												<!--!='' Removed-->
												<img src="../Content/assets/images/no-icon.svg" title="Unsuitable" width="20"/>
												<span class="suitability false" id="unsuitableStructure"></span>
											</xsl:if>
											<xsl:if test="a:Appraisal/a:Suitability/text()='Marginally suitable'or a:Appraisal/a:Suitability/text()='marginal'">
												<!--!='' Removed-->
												<span class="suitability question"></span>
												<span style="font-size: 30px;" title="Marginal structure" >?</span>
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
										<xsl:if test="$UserTypeId!=696007">
											<td></td>
										</xsl:if>
										<xsl:if test="$UserTypeId=696007">
											<td class="text-color1" style="text-align: center;">
												<xsl:if test="./a:StructureResponsibility/a:StructureResponsibleParty/@OrganisationId=$OrgId">
													<xsl:if test="a:AlsatAppraisal/a:ResultStructure/text()!=''">
														<xsl:choose>
															<xsl:when test="a:AlsatAppraisal/a:CommentsForHaulier/text()!=''">
																<a class="text-decoration-link affected-structure-show-note-to-haulier" hauliertext="{a:AlsatAppraisal/a:CommentsForHaulier/text()}" ESRN="{ a:ESRN/text()}">
																	<xsl:value-of select="a:AlsatAppraisal/a:ResultStructure/text()"/>
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
								</xsl:for-each>
                    </xsl:when>
                  <xsl:otherwise>										
											<tr>
												<td class="text-color1" colspan="5" style="text-align:center;">
                          <xsl:if test="$UserTypeId=696001">
                            No unsuitable affected structures
                          </xsl:if>
                          <xsl:if test="$UserTypeId!=696001">
                            No affected structures
                          </xsl:if>
												</td>
											</tr>										
									</xsl:otherwise>
                  </xsl:choose>
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