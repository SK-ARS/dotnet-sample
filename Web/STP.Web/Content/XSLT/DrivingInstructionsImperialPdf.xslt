<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:a="http://www.esdal.com/schemas/core/drivinginstruction"
                xmlns:b="http://www.esdal.com/schemas/core/formattedtext"
                xmlns:c="http://www.govtalk.gov.uk/people/bs7666"
                xmlns:d="http://www.esdal.com/schemas/core/caution"
                xmlns:e="http://www.esdal.com/schemas/core/contact"
                xmlns:f="http://www.esdal.com/schemas/core/esdalcommontypes"
                xmlns:g="http://www.esdal.com/schemas/core/annotation">

  <xsl:template match="/">
    <html xsl:version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
          xmlns:a="http://www.esdal.com/schemas/core/drivinginstruction"
          xmlns:b="http://www.esdal.com/schemas/core/formattedtext"
          xmlns:c="http://www.govtalk.gov.uk/people/bs7666"
          xmlns:d="http://www.esdal.com/schemas/core/caution"
          xmlns:e="http://www.esdal.com/schemas/core/contact"
          xmlns:f="http://www.esdal.com/schemas/core/esdalcommontypes"
          xmlns:g="http://www.esdal.com/schemas/core/annotation">

      <body style="font-size:10.5px;">
        <xsl:for-each select="/a:RouteDrivingInstructions/a:RouteParts/a:DrivingInstructions">
          <xsl:variable name="legCount" select="a:LegNumber/text()" />

          <div class="colHeader pl-3">
            <span class="text-normal" id="{$legCount}">
              Leg <xsl:text>&#xA0;&#xA0;&#xA0;</xsl:text>
              <xsl:apply-templates select="a:LegNumber/text()"/><xsl:text>&#xA0;&#xA0;&#xA0;</xsl:text>:
            </span>
            <span class="text-normal">
              <xsl:apply-templates select="a:Name/text()"/>
            </span>
          </div>
          <div id="showDrivingdetails">
            <div class="main-table" style="border-radius: 0px 0px 0px 0px;">
              <table>
                <tbody>
                  <tr>
                    <th width="50%" class="pl-3">Route directions</th>
                    <th class="pl-1">Cautions</th>
                  </tr>
                </tbody>
              </table>
            </div>
            <xsl:for-each select="a:SubPartListPosition/a:SubPart">

              <xsl:variable name="set" select="a:AlternativeListPosition" />
              <xsl:variable name="recCount" select="count($set)" />
              <xsl:variable name="mergeCount" select="position()" />

              <xsl:for-each select="a:AlternativeListPosition">

                <xsl:variable name="altLstPositionCount" select="position()" />
                <xsl:variable name="AlternativeNo" select="a:Alternative/a:AlternativeDescription/a:AlternativeNo/text()" />
                <xsl:variable name="startmode" select="(a:Alternative/a:InstructionListPosition/a:Instruction/a:NoteListPosition/a:Note/a:Content/a:RoutePoint/@PointType)[1]" />
                <xsl:variable name="endmode" select="(a:Alternative/a:InstructionListPosition/a:Instruction/a:NoteListPosition/a:Note/a:Content/a:RoutePoint/@PointType)[position()=last()]" />

                <xsl:variable name="mode">
                  <xsl:choose>
                    <xsl:when test="$startmode='start'">
                      <xsl:value-of select="'start'"/>
                    </xsl:when>
                    <xsl:when test="$endmode='end'">
                      <xsl:value-of select="'end'"/>
                    </xsl:when>
                    <xsl:otherwise>middle</xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>

                <xsl:if test="($mode='end' or $mode='middle') and $recCount>1 and position()=1" >
                  <tr>
                    <div class="details-below">
                      <span class="details">Diverge</span>
                    </div>
                    <td>
                      <!--empty cell-->
                    </td>
                  </tr>
                </xsl:if>
                <xsl:variable name="altCount" select="position()" />
                <xsl:for-each select="a:Alternative/a:InstructionListPosition/a:Instruction">
                  <xsl:variable name="count" select="concat('viewmoredetails',position(),$altCount,$mergeCount,$legCount)"/>
                  <div class="details-below" style="cursor: pointer;" onclick="viewmoredetails('{$count}')">
                    <span class="details">
                      <xsl:if test="$recCount>1" >
                        <xsl:if test="($mode='start' and position()=1)" >
                          <xsl:value-of select="concat('Alternative ', 'start', ' # ',$AlternativeNo,':')" />
                        </xsl:if>
                        <xsl:if test="$mode='middle' and position()=1 and $recCount>1" >
                          <xsl:value-of select="concat('Alternative ', ' # ',$AlternativeNo,':')" />
                        </xsl:if>
                      </xsl:if>
                      <xsl:apply-templates select="a:Navigation/a:Instruction/* | a:Navigation/a:Instruction/text()"/>
                      <xsl:apply-templates select="a:Navigation/a:Distance/a:DisplayImperial"/>
                      <xsl:if test="$recCount>1" >
                        <xsl:if test="($mode='end' and position()=last())" >
                          <xsl:value-of select="concat('Alternative ', 'end', ' # ',$AlternativeNo,':')" />
                        </xsl:if>
                      </xsl:if>
                    </span>

                    <xsl:for-each select="a:NoteListPosition">
                      <xsl:variable name="iterationCount" select="position()"/>
                      <xsl:if test="$iterationCount=1" >
                        <!--<img id="chevlon-up-icon1{$count}" src="~/Content/Images/logo.png"
                                 
                     width="15" style="display: none;"></img>
                        <img id="chevlon-down-icon1{$count}" src="~/Content/Images/logo.png" width="15"
                         style="display: block;"></img>-->
                      
                      </xsl:if>
                      <div id='{$count}' style="display: none;">
                        <div class="row">
                          <div class="col-sm-6 col-md-6 col-lg-6 pr-0">
                          </div>
                          <div class="col-6">

                            <p class="edit-normal pl-0 pr-0 pb-0 pt-0 mt-0 mb-0 ml-0 mr-0" style="text-align: justify;">
                              <p class="edit-normal pl-0 pr-0 pb-0 pt-0 mt-0 mb-0 ml-0 mr-0" style="text-align: justify;">
                                <xsl:if test="a:Note/a:Content/a:RoutePoint/@PointType='start'">
                                  <div class="text-normal pt-1">
                                    <span>
                                      <b>Start point further details:</b>
                                    </span> <br></br>
                                    Grid Ref:
                                    <xsl:apply-templates select="a:Note/a:GridReference/c:X/text()"/>,
                                    <xsl:apply-templates select="a:Note/a:GridReference/c:Y/text()"/><br></br>
                                    <xsl:apply-templates select="a:Note/a:Content/a:RoutePoint/a:Description/text()"/>
                                  </div>
                                </xsl:if>
                              </p>

                              <p class="edit-normal pl-0 pr-0 pb-0 pt-0 mt-0 mb-0 ml-0 mr-0" style="text-align: justify;">
                                <xsl:if test="a:Note/a:Content/a:RoutePoint/@PointType='end'">
                                  <div class="text-normal pt-1">
                                    <span>
                                      <b>End point further details:</b>
                                    </span> <br></br>
                                    Grid Ref:
                                    <xsl:apply-templates select="a:Note/a:GridReference/c:X/text() "/>,
                                    <xsl:apply-templates select="a:Note/a:GridReference/c:Y/text()"/><br></br>
                                    <xsl:apply-templates select="a:Note/a:Content/a:RoutePoint/a:Description/text()"/>
                                  </div>
                                </xsl:if>
                              </p>
                              <p class="edit-normal pl-0 pr-0 pb-0 pt-0 mt-0 mb-0 ml-0 mr-0" style="text-align: justify;">
                                <xsl:if test="a:Note/a:Content/a:RoutePoint/@PointType='intermediate'">
                                  <span>
                                    <b>Via point reached, further details:</b>
                                  </span>
                                  <br></br>
                                  <!--<b>Grid Ref: </b><xsl:apply-templates select="a:Note/a:GridReference/c:X/text()"/>, <xsl:apply-templates select="a:Note/a:GridReference/c:Y/text()"/><br/>-->
                                  <xsl:apply-templates select="a:Note/a:Content/a:RoutePoint/a:Description/text()"/>
                                </xsl:if>
                              </p>
                              <p class="edit-normal pl-0 pr-0 pb-0 pt-0 mt-0 mb-0 ml-0 mr-0" style="text-align: justify;">
                                <xsl:if test="./a:Note/a:Content/a:Caution">
                                  <div class="text-normal" style="font-family: 700px;">
                                    <span>
                                      <b>CAUTION: </b>
                                    </span>
                                    <span>
                                      <b>After </b>
                                    </span>
                                    <b>
                                      <xsl:apply-templates select="a:Note/a:EncounteredAt/a:DisplayImperial"/>
                                    </b>
                                    <br></br>
                                  </div>


                                  <xsl:if test="./a:Note/a:Content/a:Caution/d:CautionedEntity/d:Constraint">
                                    <div class="text-normal pt-1">
                                      <xsl:apply-templates select="a:Note/a:Content/a:Caution/d:CautionedEntity/d:Constraint/d:ECRN/text()"/>
                                      <xsl:text> </xsl:text>
                                      <xsl:apply-templates select="a:Note/a:Content/a:Caution/d:CautionedEntity/d:Constraint/d:Type/text()"/>
                                      <xsl:text> </xsl:text>
                                      <xsl:apply-templates select="a:Note/a:Content/a:Caution/d:CautionedEntity/d:Constraint/d:ConstraintName/text()"/>
                                    </div>
                                    <br></br>
                                  </xsl:if>

                                  <xsl:if test="./a:Note/a:Content/a:Caution/d:CautionedEntity/d:Structure">
                                    <div class="text-normal pt-1">
                                      <xsl:apply-templates select="a:Note/a:Content/a:Caution/d:CautionedEntity/d:Structure/d:ESRN/text()"/>
                                      <xsl:text> </xsl:text>
                                      <xsl:apply-templates select="a:Note/a:Content/a:Caution/d:CautionedEntity/d:Structure/d:Type/text()"/>
                                      <xsl:text> </xsl:text>
                                      <xsl:apply-templates select="a:Note/a:Content/a:Caution/d:CautionedEntity/d:Structure/d:StructureName/text()"/>
                                    </div>
                                    <br></br>
                                  </xsl:if>
                                  <!-- Bug Fix: SpecificAction not showing up. order adjusted to show the same above Grid Ref.-->
                                  <xsl:if test="./a:Note/a:Content/a:Caution/d:Action/d:SpecificAction">
                                    <div class="text-normal pt-1">
                                      <xsl:apply-templates select="a:Note/a:Content/a:Caution/d:Action/d:SpecificAction/node()"/>
                                    </div>
                                  </xsl:if>
                                  <div class="text-normal pt-1">
                                    Grid Ref:<xsl:apply-templates select="a:Note/a:GridReference/c:X/text() "/>,
                                    <xsl:apply-templates select="a:Note/a:GridReference/c:Y/text()"/>
                                  </div>

                                  <br></br>
                                  <xsl:if test="./a:Note/a:Content/a:Caution/d:Contact">
                                    <xsl:if test="./a:Note/a:Content/a:Caution/d:Contact/e:FullName">
                                      <span>Contact : </span>
                                      <xsl:apply-templates select="a:Note/a:Content/a:Caution/d:Contact/e:FullName/text()"/>
                                      <xsl:text>, </xsl:text>

                                      <xsl:if test="./a:Note/a:Content/a:Caution/d:Contact/e:OrganisationName">
                                        <xsl:apply-templates select="a:Note/a:Content/a:Caution/d:Contact/e:OrganisationName/text()"/>
                                        <xsl:text>, </xsl:text>
                                      </xsl:if>

                                      <xsl:if test="./a:Note/a:Content/a:Caution/d:Contact/e:Address/f:Line">
                                        <xsl:for-each select="./a:Note/a:Content/a:Caution/d:Contact/e:Address/f:Line">
                                          <xsl:text> </xsl:text>
                                          <xsl:apply-templates select="text()"/>
                                        </xsl:for-each>
                                        <br></br>
                                      </xsl:if>

                                      <xsl:if test="./a:Note/a:Content/a:Caution/d:Contact/e:Address/f:PostCode">
                                        <xsl:apply-templates select="a:Note/a:Content/a:Caution/d:Contact/e:Address/f:PostCode/text()"/>
                                        <xsl:text>, </xsl:text>
                                      </xsl:if>

                                      <xsl:if test="./a:Note/a:Content/a:Caution/d:Contact/e:Address/f:Country">
                                        <xsl:apply-templates select="a:Note/a:Content/a:Caution/d:Contact/e:Address/f:Country/text()"/>
                                        <xsl:text>, </xsl:text>
                                      </xsl:if>

                                      <xsl:if test="./a:Note/a:Content/a:Caution/d:Contact/e:TelephoneNumber">
                                        <span>Tel:</span>
                                        <xsl:apply-templates select="a:Note/a:Content/a:Caution/d:Contact/e:TelephoneNumber/text()"/>
                                      </xsl:if>

                                      <br></br>

                                      <xsl:if test="./a:Note/a:Content/a:Caution/d:Contact/e:FaxNumber">
                                        <span>Fax:</span>
                                        <xsl:apply-templates select="a:Note/a:Content/a:Caution/d:Contact/e:FaxNumber/text()"/>
                                        <br></br>
                                      </xsl:if>

                                      <xsl:if test="./a:Note/a:Content/a:Caution/d:Contact/e:EmailAddress">
                                        <span>Email:</span>
                                        <xsl:apply-templates select="a:Note/a:Content/a:Caution/d:Contact/e:EmailAddress/text()"/>
                                      </xsl:if>
                                    </xsl:if>
                                  </xsl:if>
                                </xsl:if>
                              </p>
                              <p class="edit-normal pl-0 pr-0 pb-0 pt-0 mt-0 mb-0 ml-0 mr-0" style="text-align: justify;">
                                <xsl:if test="./a:Note/a:Content/a:Annotation">
                                  <div class="text-normal pt-1">
                                    After
                                    <xsl:apply-templates select="a:Note/a:EncounteredAt/a:DisplayImperial"/>
                                    <br></br>
                                    Grid Ref: <xsl:apply-templates select="a:Note/a:GridReference/c:X/text() "/>, <xsl:apply-templates select="a:Note/a:GridReference/c:Y/text()"/><br/>
                                  </div>
                                  <xsl:variable name="AnnotationType" select="a:Note/a:Content/a:Annotation/@AnnotationType" />
                                  <xsl:if test="$AnnotationType!='generic'" >

                                    <span>
                                      <xsl:if test="$AnnotationType='special_manouevre'" >
                                        <div class="text-normal pt-1">
                                          <b>Special manoeuvre:</b>
                                        </div>

                                      </xsl:if>
                                      <xsl:if test="$AnnotationType='caution'" >
                                        <div class="text-normal pt-1">
                                          <b>Caution:</b>
                                        </div>

                                      </xsl:if>
                                    </span>
                                  </xsl:if>
                                  <div class="text-normal pt-1">
                                    <xsl:apply-templates select="a:Note/a:Content/a:Annotation/g:Text/b:Bold/text()"/>
                                    <br></br>
                                    <xsl:apply-templates select="a:Note/a:Content/a:Annotation/g:Contact"/>
                                  </div>
                                </xsl:if>
                              </p>
                              <p class="edit-normal pl-0 pr-0 pb-0 pt-0 mt-0 mb-0 ml-0 mr-0" style="text-align: justify;">
                                <xsl:if test="./a:Note/a:Content/a:MotorwayCaution">
                                  <div class="text-normal pt-1">
                                    Apply Motorway Caution
                                  </div>
                                </xsl:if>
                              </p>

                            </p>
                          </div>

                        </div>
                      </div>

                    </xsl:for-each>

                  </div>
                  <br></br>
                </xsl:for-each>

                <xsl:if test="($mode='start' or $mode='middle')  and $recCount>1 and position() = $recCount" >
                  <div class="details-below">
                    <span class="details">Alternative Routes Merge</span>
                  </div>
                </xsl:if>
              </xsl:for-each>
            </xsl:for-each>
          </div>
        </xsl:for-each>

        <xsl:if test="./a:RouteDrivingInstructions/a:PredefinedCautions">
          <xsl:if test="./a:RouteDrivingInstructions/a:PredefinedCautions/a:DockCautionDescription/text()!=null">
            <p class="edit-normal pl-0 pr-0 pb-0 pt-0 mt-0 mb-0 ml-0 mr-0" style="text-align: justify;">
              <span>Dock Caution Description</span>
              <br></br>
              <xsl:apply-templates select="a:RouteDrivingInstructions/a:PredefinedCautions/a:DockCautionDescription/text()"/>
            </p>
          </xsl:if>
          <xsl:if test="./a:RouteDrivingInstructions/a:PredefinedCautions/a:MotorwayCautionDescription/text()!=null">
            <p class="edit-normal pl-0 pr-0 pb-0 pt-0 mt-0 mb-0 ml-0 mr-0" style="text-align: justify;">
              <span>Motorway Caution Description</span>
              <br></br>
              <xsl:apply-templates select="a:RouteDrivingInstructions/a:PredefinedCautions/a:MotorwayCautionDescription/text()"/>
            </p>
          </xsl:if>
          <xsl:if test="./a:RouteDrivingInstructions/a:PredefinedCautions/a:HeightCautionDescription/text()!=null">
            <p class="edit-normal pl-0 pr-0 pb-0 pt-0 mt-0 mb-0 ml-0 mr-0" style="text-align: justify;">
              <span>Height Caution Description</span>
              <br></br>
              <xsl:apply-templates select="a:RouteDrivingInstructions/a:PredefinedCautions/a:HeightCautionDescription/text()"/>
            </p>
          </xsl:if>
          <xsl:if test="./a:RouteDrivingInstructions/a:PredefinedCautions/a:StandardCautionDescription/text()!=null">
            <p class="edit-normal pl-0 pr-0 pb-0 pt-0 mt-0 mb-0 ml-0 mr-0" style="text-align: justify;">
              <span>Standard Caution Description</span>
              <br></br>
              <xsl:apply-templates select="a:RouteDrivingInstructions/a:PredefinedCautions/a:StandardCautionDescription/text()"/>
            </p>
          </xsl:if>
        </xsl:if>
      </body>
    </html>
  </xsl:template>

  <xsl:template match="a:DisplayImperial">
    <xsl:if test="./text() &lt; 1760">
      <xsl:value-of select="./text()"/> yards
    </xsl:if>
    <xsl:if test="./text() &gt; 1760">
      <xsl:value-of select="format-number(./text() div 1760,'#.#')"/> miles
    </xsl:if>
  </xsl:template>



  <xsl:template match="text()">
    <xsl:value-of select="."/>
  </xsl:template>

  <xsl:template match="a:PointType">
    <b>
      <xsl:value-of select="."/>
    </b>
  </xsl:template>
</xsl:stylesheet>


