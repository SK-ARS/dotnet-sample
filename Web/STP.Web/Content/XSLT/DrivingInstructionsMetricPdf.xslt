<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:a="http://www.esdal.com/schemas/core/drivinginstruction"
                xmlns:b="http://www.esdal.com/schemas/core/formattedtext"
                xmlns:c="http://www.govtalk.gov.uk/people/bs7666"
                xmlns:d="http://www.esdal.com/schemas/core/caution"
                xmlns:e="http://www.esdal.com/schemas/core/contact"
                xmlns:f="http://www.esdal.com/schemas/core/esdalcommontypes"
                xmlns:g="http://www.esdal.com/schemas/core/annotation"
                >

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
        <table >
          <tbody>


            <xsl:for-each select="/a:RouteDrivingInstructions/a:RouteParts/a:DrivingInstructions">
              <!--<tr>
                <td>
                  <b>
                    Leg <xsl:text>&#xA0;&#xA0;&#xA0;</xsl:text>
                    <xsl:apply-templates select="a:LegNumber/text()"/><xsl:text>&#xA0;&#xA0;&#xA0;</xsl:text>:
                  </b>
                  <b>
                    <xsl:apply-templates select="a:Name/text()"/>
                  </b>
                </td>
               
              </tr>-->
              <tr>
                <td>
                  <table>
                    <tbody>
                      <tr>
                        <td>
                          <b>
                            Leg <xsl:text>&#xA0;&#xA0;&#xA0;</xsl:text>
                            <xsl:apply-templates select="a:LegNumber/text()"/><xsl:text>&#xA0;&#xA0;&#xA0;</xsl:text>:
                          </b>
                          <b>
                            <xsl:apply-templates select="a:Name/text()"/>
                          </b>
                        </td>
                        <td></td>
                      </tr>
                      <tr>
                        <th class="headgrad" >Route directions</th>
                        <th class="headgrad" >Cautions</th>
                      </tr>

                      <xsl:for-each select="a:SubPartListPosition/a:SubPart">

                        <xsl:variable name="set" select="a:AlternativeListPosition" />
                        <xsl:variable name="recCount" select="count($set)" />

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
                              <td style="font-weight:bold; padding-left: 25px;text-decoration: underline; text-align:center" >
                                Diverge
                              </td>
                              <td>
                                <!--empty cell-->
                              </td>
                            </tr>
                          </xsl:if>

                          <xsl:for-each select="a:Alternative/a:InstructionListPosition/a:Instruction">
                            <tr>
                              <td  style="width:40%;">
                                <xsl:if test="$recCount>1" >
                                  <div style="font-weight:bold; padding-left: 25px;text-decoration: underline; text-align:center">
                                    <xsl:if test="($mode='start' and position()=1)" >
                                      <xsl:value-of select="concat('Alternative ', 'start', ' # ',$AlternativeNo,':')" />
                                    </xsl:if>
                                    <xsl:if test="$mode='middle' and position()=1 and $recCount>1" >
                                      <xsl:value-of select="concat('Alternative ', ' # ',$AlternativeNo,':')" />
                                    </xsl:if>
                                  </div>
                                </xsl:if>
                                <xsl:apply-templates select="a:Navigation/a:Instruction/* | a:Navigation/a:Instruction/text()"/>
                                <xsl:apply-templates select="a:Navigation/a:Distance/a:DisplayMetric"/>
                                <xsl:if test="$recCount>1" >
                                  <div style="font-weight:bold; padding-left: 25px;text-decoration: underline; text-align:center">
                                    <xsl:if test="($mode='end' and position()=last())" >
                                      <xsl:value-of select="concat('Alternative ', 'end', ' # ',$AlternativeNo,':')" />
                                    </xsl:if>
                                  </div>
                                </xsl:if>
                              </td>
                              <td>
                                <xsl:for-each select="a:NoteListPosition">
                                  <xsl:if test="a:Note/a:Content/a:RoutePoint/@PointType='start'">
                                    <span>
                                      <b>Start point further details:</b>
                                    </span> <br/>
                                    <b>Grid Ref: </b>
                                    <xsl:apply-templates select="a:Note/a:GridReference/c:X/text()"/>,
                                    <xsl:apply-templates select="a:Note/a:GridReference/c:Y/text()"/><br/>
                                    <xsl:apply-templates select="a:Note/a:Content/a:RoutePoint/a:Description/text()"/>
                                  </xsl:if>

                                  <xsl:if test="a:Note/a:Content/a:RoutePoint/@PointType='end'">
                                    <span>
                                      <b>End point further details:</b>
                                    </span> <br/>
                                    <b>
                                      Grid Ref:
                                    </b><xsl:apply-templates select="a:Note/a:GridReference/c:X/text() "/>,
                                    <xsl:apply-templates select="a:Note/a:GridReference/c:Y/text()"/><br/>
                                    <xsl:apply-templates select="a:Note/a:Content/a:RoutePoint/a:Description/text()"/>
                                  </xsl:if>

                                  <xsl:if test="a:Note/a:Content/a:RoutePoint/@PointType='intermediate'">
                                    <span>
                                      <b>Via point reached, further details:</b>
                                    </span>
                                    <br/>
                                    <!--<b>Grid Ref: </b><xsl:apply-templates select="a:Note/a:GridReference/c:X/text()"/>, <xsl:apply-templates select="a:Note/a:GridReference/c:Y/text()"/><br/>-->
                                    <xsl:apply-templates select="a:Note/a:Content/a:RoutePoint/a:Description/text()"/>
                                  </xsl:if>

                                  <!--<xsl:for-each select="/a:Instruction/a:NoteListPosition">-->
                                  <xsl:if test="./a:Note/a:Content/a:Caution">
                                    <table>
                                      <tbody>
                                        <tr style="border-bottom:0px;">
                                          <td>
                                            <span style="font-weight:bold; ">CAUTION: </span>
                                            <span>After </span>
                                            <xsl:apply-templates select="a:Note/a:EncounteredAt/a:DisplayMetric"/>
                                          </td>
                                        </tr>
                                        <tr style="border-bottom:0px;">
                                          <xsl:if test="./a:Note/a:Content/a:Caution/d:CautionedEntity/d:Constraint">
                                            <td>
                                              <xsl:apply-templates select="a:Note/a:Content/a:Caution/d:CautionedEntity/d:Constraint/d:ECRN/text()"/>
                                              <xsl:text> </xsl:text>
                                              <xsl:apply-templates select="a:Note/a:Content/a:Caution/d:CautionedEntity/d:Constraint/d:Type/text()"/>
                                              <xsl:text> </xsl:text>
                                              <xsl:apply-templates select="a:Note/a:Content/a:Caution/d:CautionedEntity/d:Constraint/d:ConstraintName/text()"/>
                                              <!--<br/><br/>
                                              <span style="font-weight:bold; ">Grid Ref: </span><xsl:apply-templates select="a:Note/a:GridReference/c:X/text() "/>,  <xsl:apply-templates select="a:Note/a:GridReference/c:Y/text()"/>-->
                                            </td>
                                          </xsl:if>
                                        </tr>
                                        <tr>
                                          <xsl:if test="./a:Note/a:Content/a:Caution/d:CautionedEntity/d:Structure">
                                            <td>
                                              <xsl:apply-templates select="a:Note/a:Content/a:Caution/d:CautionedEntity/d:Structure/d:ESRN/text()"/>
                                              <xsl:text> </xsl:text>
                                              <xsl:apply-templates select="a:Note/a:Content/a:Caution/d:CautionedEntity/d:Structure/d:Type/text()"/>
                                              <xsl:text> </xsl:text>
                                              <xsl:apply-templates select="a:Note/a:Content/a:Caution/d:CautionedEntity/d:Structure/d:StructureName/text()"/>
                                              <!--<br/><br/>
                                              <span style="font-weight:bold; ">Grid Ref: </span><xsl:apply-templates select="a:Note/a:GridReference/c:X/text() "/>,  <xsl:apply-templates select="a:Note/a:GridReference/c:Y/text()"/>-->
                                            </td>
                                          </xsl:if>
                                        </tr>

                                        <!-- Bug Fix: SpecificAction not showing up. order adjusted to show the same above Grid Ref.-->
                                        <xsl:if test="./a:Note/a:Content/a:Caution/d:Action/d:SpecificAction">
                                          <tr>
                                            <td>
                                              <xsl:apply-templates select="a:Note/a:Content/a:Caution/d:Action/d:SpecificAction/node()"/>
                                            </td>
                                          </tr>
                                        </xsl:if>

                                        <tr>
                                          <td>
                                            <span style="font-weight:bold; ">Grid Ref: </span><xsl:apply-templates select="a:Note/a:GridReference/c:X/text() "/>,
                                            <xsl:apply-templates select="a:Note/a:GridReference/c:Y/text()"/>
                                          </td>
                                        </tr>

                                        <!--
                                        <xsl:if test="./a:Note/a:Content/a:Caution">
                                          <xsl:if test="./d:Action">
                                            <xsl:if test="./d:SpecificAction">
                                              <tr>
                                                <td>
                                                  <xsl:apply-templates select="a:Note/a:Content/a:Caution/d:Action/d:SpecificAction/node()"/>
                                                </td>
                                              </tr>
                                            </xsl:if>
                                          </xsl:if>
                                        </xsl:if>-->

                                        <xsl:if test="./a:Note/a:Content/a:Caution/d:Contact">
                                          <tr style="border-bottom:0px;">
                                            <td>
                                              <xsl:if test="./a:Note/a:Content/a:Caution/d:Contact/e:FullName">
                                                <span style="font-weight:bold;">Contact : </span>
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
                                                  <br/>
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

                                                <br/>

                                                <xsl:if test="./a:Note/a:Content/a:Caution/d:Contact/e:FaxNumber">
                                                  <span>Fax:</span>
                                                  <xsl:apply-templates select="a:Note/a:Content/a:Caution/d:Contact/e:FaxNumber/text()"/>
                                                  <br/>
                                                </xsl:if>

                                                <xsl:if test="./a:Note/a:Content/a:Caution/d:Contact/e:EmailAddress">
                                                  <span>Email:</span>
                                                  <xsl:apply-templates select="a:Note/a:Content/a:Caution/d:Contact/e:EmailAddress/text()"/>
                                                </xsl:if>
                                              </xsl:if>
                                            </td>
                                          </tr>
                                        </xsl:if>

                                      </tbody>
                                    </table>

                                  </xsl:if>

                                  <xsl:if test="./a:Note/a:Content/a:Annotation">
                                    <table>
                                      <tbody>
                                        <tr style="border-bottom:0px;">
                                          <td>
                                            <span>After </span>
                                            <xsl:apply-templates select="a:Note/a:EncounteredAt/a:DisplayMetric"/>
                                          </td>
                                        </tr>
                                        <tr style="border-bottom:0px;">
                                          <td>
                                            <span style="font-weight:bold; ">Grid Ref: </span><xsl:apply-templates select="a:Note/a:GridReference/c:X/text() "/>, <xsl:apply-templates select="a:Note/a:GridReference/c:Y/text()"/>
                                          </td>
                                        </tr>
                                        <tr style="border-bottom:0px;">
                                          <td>
                                            <xsl:variable name="AnnotationType" select="a:Note/a:Content/a:Annotation/@AnnotationType" />
                                            <xsl:if test="$AnnotationType!='generic'" >
                                              <span style="font-weight:bold;">
                                                <xsl:if test="$AnnotationType='special_manouevre'" >
                                                  Special manoeuvre:
                                                </xsl:if>
                                                <xsl:if test="$AnnotationType='caution'" >
                                                  Caution:
                                                </xsl:if>

                                              </span>
                                              <br/>
                                            </xsl:if>
                                            <xsl:apply-templates select="a:Note/a:Content/a:Annotation/g:Text/b:Bold/text()"/>
                                            <br/>
                                            <xsl:apply-templates select="a:Note/a:Content/a:Annotation/g:Contact"/>
                                            <!--<xsl:if test="./g:Text">
                                    <xsl:if test="./f:Bold">
                                      <xsl:apply-templates select="f:Bold/text()"/>
                                    </xsl:if>
                                  </xsl:if>-->
                                          </td>
                                        </tr>
                                      </tbody>
                                    </table>
                                  </xsl:if>

                                  <xsl:if test="./a:Note/a:Content/a:MotorwayCaution">
                                    <table>
                                      <tbody>
                                        <tr style="border-bottom:0px;">
                                          <td>
                                            <span>
                                              <B>Apply Motorway Caution</B>
                                            </span>
                                          </td>
                                        </tr>
                                      </tbody>
                                    </table>

                                  </xsl:if>

                                </xsl:for-each>
                              </td>
                            </tr>
                          </xsl:for-each>

                          <xsl:if test="($mode='start' or $mode='middle')  and $recCount>1 and position() = $recCount" >
                            <tr>
                              <td>
                                <!--<xsl:value-of select="concat('Alternative ', $mode, ' # ',$recCount,position())" />-->
                                <div style="font-weight:bold; padding-left: 25px;text-decoration: underline;text-align:center">Alternative Routes Merge</div>
                              </td>
                              <td>
                                <!--empty cell-->
                              </td>
                            </tr>
                          </xsl:if>
                        </xsl:for-each>
                      </xsl:for-each>
                    </tbody>
                  </table>

                </td>
              </tr>
            </xsl:for-each>
          </tbody>
        </table>
        <br></br>

        <xsl:if test="./a:RouteDrivingInstructions/a:PredefinedCautions">
          <table>
            <tbody>

              <xsl:if test="./a:RouteDrivingInstructions/a:PredefinedCautions/a:DockCautionDescription/text()!=null">
                <tr style="border-bottom:0px;">
                  <td>
                    <span>Dock Caution Description</span>
                  </td>
                </tr>
                <tr style="border-bottom:0px;">
                  <td>
                    <xsl:apply-templates select="a:RouteDrivingInstructions/a:PredefinedCautions/a:DockCautionDescription/text()"/>
                  </td>
                </tr>
              </xsl:if>

              <xsl:if test="./a:RouteDrivingInstructions/a:PredefinedCautions/a:MotorwayCautionDescription/text()!=null">
                <tr style="border-bottom:0px;">
                  <td>
                    <span>Motorway Caution Description</span>
                  </td>
                </tr>
                <tr style="border-bottom:0px;">
                  <td>
                    <xsl:apply-templates select="a:RouteDrivingInstructions/a:PredefinedCautions/a:MotorwayCautionDescription/text()"/>
                  </td>
                </tr>
              </xsl:if>

              <xsl:if test="./a:RouteDrivingInstructions/a:PredefinedCautions/a:HeightCautionDescription/text()!=null">
                <tr style="border-bottom:0px;">
                  <td>
                    <span>Height Caution Description</span>
                  </td>
                </tr>
                <tr style="border-bottom:0px;">
                  <td>
                    <xsl:apply-templates select="a:RouteDrivingInstructions/a:PredefinedCautions/a:HeightCautionDescription/text()"/>
                  </td>
                </tr>
              </xsl:if>

              <xsl:if test="./a:RouteDrivingInstructions/a:PredefinedCautions/a:StandardCautionDescription/text()!=null">
                <tr style="border-bottom:0px;">
                  <td>
                    <span>Standard Caution Description</span>
                  </td>
                </tr>
                <tr style="border-bottom:0px;">
                  <td>
                    <xsl:apply-templates select="a:RouteDrivingInstructions/a:PredefinedCautions/a:StandardCautionDescription/text()"/>
                  </td>
                </tr>
              </xsl:if>
            </tbody>
          </table>
        </xsl:if>

      </body>
    </html>
  </xsl:template>

  <xsl:template match="a:DisplayMetric">
    <xsl:if test="./text() &lt; 1000">
      <xsl:value-of select="./text()"/> m
    </xsl:if>
    <xsl:if test="./text() &gt;=  1000">
      <xsl:value-of select="format-number(./text() div 1000,'#.#')"/> km
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

  <xsl:template match="a:PointType">
    <b>
      <xsl:value-of select="."/>
    </b>
  </xsl:template>

  <xsl:template match="c:PointType">
    <u>
      <xsl:value-of select="."/>
    </u>
  </xsl:template>

</xsl:stylesheet>


