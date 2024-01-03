<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:a="http://www.esdal.com/schemas/common/routeappraisal" xmlns:b="http://www.esdal.com/schemas/core/formattedtext" xmlns:c="http://www.esdal.com/schemas/core/routedescription" xmlns:d="http://www.esdal.com/schemas/core/esdalcommontypes">

  <xsl:template match="/">
    <html xsl:version="1.0" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://www.esdal.com/schemas/common/routeappraisal" xmlns:b="http://www.esdal.com/schemas/core/formattedtext" xmlns:c="http://www.esdal.com/schemas/core/routedescription" xmlns:d="http://www.esdal.com/schemas/core/esdalcommontypes">
      <body>
        <xsl:for-each select="/a:RoutePartsDescription/a:RoutePartDescription">
          <xsl:variable name="i" select="position()" />

          <xsl:variable name="splitRoute">
            <xsl:for-each select="c:SubpartListPosition/c:Subpart">

              <xsl:variable name="PathListPositionData" select="c:PathListPosition" />
              <xsl:variable name="PathListPositionDataCount" select="count($PathListPositionData)" />
              <item>
                <xsl:value-of select="$PathListPositionDataCount"/>
              </item>

            </xsl:for-each>
          </xsl:variable>

          <!--<xsl:value-of select="$splitRoute" />-->

          <table>
            <tbody>
              <tr >
                <th  class="headgrad white" style="text-align:centre">

                  <xsl:for-each select="c:SubpartListPosition/c:Subpart/c:PathListPosition/c:Path/c:SegmentListPosition/c:Segment/c:InstructionListPosition/c:Instruction">

                    <xsl:if test="position() = 1">
                      <xsl:if test="c:DirectionInstruction">
                        Split
                        Leg
                      </xsl:if>
                      <xsl:if test="c:ManoeuvreInstruction">
                        Split
                        Leg
                      </xsl:if>
                      <xsl:if test="c:JunctionInstruction">
                        Split
                        Leg
                      </xsl:if>
                    </xsl:if>
                    <!--<xsl:if test="position() = last()">
                      <xsl:if test="c:DirectionInstruction">
                        <xsl:variable name="end1" select="(c:DirectionInstruction/c:RoadIdentity)" />
                        <xsl:text>&#xA0;&#xA0;&#xA0;</xsl:text>
                        <xsl:value-of select="$end1"/>
                      </xsl:if>
                      <xsl:if test="c:ManoeuvreInstruction">
                        <xsl:variable name="end1" select="(c:ManoeuvreInstruction/c:RoadIdentity)" />
                        <xsl:text>&#xA0;&#xA0;&#xA0;</xsl:text>
                        <xsl:value-of select="$end1"/>
                      </xsl:if>
                      <xsl:if test="c:JunctionInstruction">
                        <xsl:variable name="end1" select="(c:JunctionInstruction/c:RoadIdentity)" />
                        <xsl:text>&#xA0;&#xA0;&#xA0;</xsl:text>
                        <xsl:value-of select="$end1"/>
                      </xsl:if>
                    </xsl:if>

                    -<xsl:apply-templates select="c:DirectionInstruction/c:Distance/c:DisplayMetric"/>-->
                  </xsl:for-each>
                </th>
              </tr>

              <tr>
                <td>
                  Start

                  <xsl:for-each select="c:SubpartListPosition/c:Subpart">

                    <xsl:variable name="PathListPositionvalue" select="c:PathListPosition" />
                    <xsl:variable name="PathListPositionvalueCount" select="count($PathListPositionvalue)" />
                    <xsl:variable name="Previous" select="position()-1"></xsl:variable>
                    <xsl:variable name="getposition" select="position()"></xsl:variable>
                    <xsl:variable name="getpositionLast" select="last()"></xsl:variable>
                    <xsl:variable name="AlternativeNoPart" select="c:PathListPosition/c:Path/c:Description/c:AlternativeNumber/text()" />

                    <!--<xsl:value-of select="$PathListPositionvalueCount" />-->

                    <!--<xsl:variable name="PathListPositionData" select="c:PathListPosition" />
                    <xsl:variable name="PathListPositionDataCount" select="count($PathListPositionData)" />-->
                    <!--<xsl:value-of select="$PathListPositionDataCount" />-->



                    <xsl:for-each select="c:PathListPosition/c:Path">

                      <xsl:variable name="PathListPosition" select="c:PathListPosition" />
                      <xsl:variable name="PathListPositionCount" select="count($PathListPositionvalue)" />

                      <xsl:variable name="arrayRoute" select="msxsl:node-set($splitRoute)/item" />
                      <xsl:variable name="AlternativeNo" select="c:Description/c:AlternativeNumber/text()" />

                      <xsl:if test="($PathListPositionvalueCount > 1 and position()=1 and $getposition > 1 and $getposition != $getpositionLast)">
                        <b>
                          <u>
                            Diverge
                          </u>
                        </b>
                        <!--<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>-->
                      </xsl:if>

                      <xsl:choose>
                        <xsl:when test="$getposition = 1 and $PathListPositionCount > 1">
                          <!--Condition added for showing "Alternative start" only when there are 2 PathListPosition in first loop of Subpart-->
                          <!--<xsl:value-of select="concat('Alternative ', ' # ',$AlternativeNo,':')" />-->
                          <b>
                            <u>
                              <xsl:value-of select="concat('Alternative ', 'start', ' # ',$AlternativeNo,':')" />
                            </u>
                          </b>
                        </xsl:when>

                        <xsl:when test="$getposition = $getpositionLast">


                          <xsl:choose>

                            <xsl:when test="contains(c:Description/c:Description, '1') and $getposition > 1 and $PathListPositionCount > 1" > <!--Changes code 18 sept-->
                              <!--condition added $PathListPositionCount > 1 bec diverge will come if there is PathListPositionCount > 1 in this loop-->
                              <b>
                                <u>
                                  Diverge<!--<xsl:value-of select="concat('Alternative ', 'end', ' # ',$AlternativeNo,':')" />-->
                                </u>
                              </b>
                              <!--<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>-->
                            </xsl:when>
                            <xsl:when test="contains(c:Description/c:Description, '2') and  $PathListPositionCount > 1">
                              <b>
                                <u>
                                  <xsl:value-of select="concat('Alternative ', 'end', ' # ',1,':')" />
                                </u>
                              </b>

                            </xsl:when>

                            <xsl:when test="$PathListPositionCount >= 3">
                              <b>
                                <u>
                                  <xsl:value-of select="concat('Alternative ', 'end', ' # ',2,':')" />
                                </u>
                              </b>
                            </xsl:when>

                          </xsl:choose>

                        </xsl:when>

                        <xsl:when test="$PathListPositionvalueCount > 1 and $PathListPositionCount >1" >
                          <!--Added-->
                          <b>
                            <u>
                              <xsl:value-of select="concat('Alternative ', ' # ',$AlternativeNo,':')" />
                            </u>
                          </b>
                        </xsl:when>

                      </xsl:choose>

                      <!--<xsl:if test="contains(c:Description/c:Description, '1') and position()=3 and $PathListPositionCount = 3">
                        <b>
                          <u>
                            <xsl:value-of select="concat('Alternative ', 'end', ' # ',2,':')" />
                          </u>
                        </b>
                      </xsl:if>-->


                      <xsl:choose>
                        <xsl:when test="$PathListPositionvalueCount = 1 and $getposition != 1">

                          <xsl:if test="$arrayRoute[$Previous] > 1 and contains(c:Description/c:Description, 'Alternative')">
                            <b>
                              <u>Alternative Routes Merge</u>
                            </b>
                          </xsl:if>

                        </xsl:when>

                      </xsl:choose>

                      <xsl:if test="$PathListPositionvalueCount = 1 and $getposition = 1 and $PathListPositionCount >1">
                        <xsl:if test="$arrayRoute[$Previous] > 1">
                          <b>
                            <u>
                              <xsl:value-of select="concat('Alternative ', ' # ',$AlternativeNo,':')" />
                            </u>
                          </b>
                        </xsl:if>
                      </xsl:if>

                      <!--<b>
                      <xsl:value-of select="$Previous" />
                        </b>-->

                      <xsl:apply-templates select="c:SegmentListPosition/c:Segment/c:InstructionListPosition/c:Instruction/c:DirectionInstruction"/>
                      <xsl:apply-templates select="c:SegmentListPosition/c:Segment/c:InstructionListPosition/c:Instruction/c:ManoeuvreInstruction"/>
                      <xsl:apply-templates select="c:SegmentListPosition/c:Segment/c:InstructionListPosition/c:Instruction/c:JunctionInstruction"/>
                      <!---<xsl:apply-templates select="c:DirectionInstruction/c:Distance/c:DisplayMetric"/>-->

                    </xsl:for-each>

                  </xsl:for-each>



                  arrive at destination.
                </td>
              </tr>


            </tbody>
          </table>
          <br/>
        </xsl:for-each>
      </body>
    </html>
  </xsl:template>

  <xsl:template match="c:DirectionInstruction">
    <xsl:apply-templates select="c:RoadIdentity"/><xsl:text> </xsl:text>
    (<xsl:apply-templates select="c:Distance"/>
    <!--<xsl:apply-templates select="c:Direction"/>-->  
    <!--<xsl:text> </xsl:text>-->)<!--<xsl:text> </xsl:text>-->
    <xsl:apply-templates select="c:Annotation"/>,
  </xsl:template>

  <xsl:template match="c:ManoeuvreInstruction">
    <xsl:apply-templates select="c:Manoeuvre"/>
    <xsl:apply-templates select="c:RoadIdentity"/><xsl:text> </xsl:text>
    <xsl:apply-templates select="c:Distance"/>
    <xsl:apply-templates select="c:Annotation"/>,
  </xsl:template>

  <xsl:template match="c:JunctionInstruction">
    <!--<xsl:text>Exit at </xsl:text>-->
    <xsl:text>J</xsl:text>
    <xsl:apply-templates select="c:ExitJunction"/>
    <xsl:text>, </xsl:text>
    <!--<xsl:text> Proceed to </xsl:text>-->
  </xsl:template>


  <xsl:template match="c:RoadIdentity">
    <xsl:choose>
      <xsl:when test="@Unidentified='true'">
        Unclassified
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="concat(d:Number/text(),' ' )"/>
        <xsl:value-of select="d:Name/text()"/>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>

  <xsl:template match="c:Annotation">
    after
    <xsl:apply-templates select="c:Distance"/><xsl:text> </xsl:text>***
    <xsl:value-of select ="c:Description"/>***
  </xsl:template>

  <xsl:template match="c:Distance">
    <xsl:if test="./c:DisplayMetric/text() &lt; 1000">
      <!--<xsl:text> </xsl:text>--><xsl:value-of select="format-number(./c:DisplayMetric/text(),'#.#')"/> m
    </xsl:if>
    <xsl:if test="./c:DisplayMetric/text() = 1000">
      <!--<xsl:text> </xsl:text>--><xsl:value-of select="format-number(./c:DisplayMetric/text() div 1000,'#.#')"/> km
    </xsl:if>
    <xsl:if test="./c:DisplayMetric/text() &gt; 1000">
      <!--<xsl:text> </xsl:text>--><xsl:value-of select="format-number(./c:DisplayMetric/text() div 1000,'#.#')"/> km
    </xsl:if>
  </xsl:template>

  <xsl:template match="c:Direction">  
    <xsl:if test="./text()= 'EAST'">E</xsl:if>
    <xsl:if test="./text()= 'WEST'">W</xsl:if>
    <xsl:if test="./text()= 'NORTH'">N</xsl:if>
    <xsl:if test="./text()= 'SOUTH'">S</xsl:if>
    <xsl:if test="./text()= 'SE'">SE</xsl:if>
    <xsl:if test="./text()= 'SW'">SW</xsl:if>
    <xsl:if test="./text()= 'NE'">NE</xsl:if>
    <xsl:if test="./text()= 'NW'">NW</xsl:if>
  </xsl:template>

  <xsl:template match="c:Manoeuvre">
    <xsl:if test="c:TurnLeft"> Turn Left </xsl:if>
    <xsl:if test="c:Exit"> Exit </xsl:if>
    <xsl:if test="c:Continue"> Continue </xsl:if>
    <xsl:if test="c:TurnRight"> Turn right </xsl:if>
    <xsl:if test="c:Roundabout">
      <xsl:apply-templates select="c:Roundabout"/>
    </xsl:if>
  </xsl:template>

  <xsl:template match="c:Roundabout">
    R/A <xsl:if test="c:ExitNumber">
      <xsl:value-of select="./c:ExitNumber/text()"/>X
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