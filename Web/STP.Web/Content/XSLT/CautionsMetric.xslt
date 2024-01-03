<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://www.esdal.com/schemas/core/routeanalysis" xmlns:b="http://www.esdal.com/schemas/core/caution" xmlns:c="http://www.esdal.com/schemas/core/contact" xmlns:d="http://www.esdal.com/schemas/core/esdalcommontypes" xmlns:e="http://www.esdal.com/schemas/core/formattedtext"  xmlns:f="http://www.govtalk.gov.uk/people/AddressAndPersonalDetails">

  <xsl:template match="/">
    <html xsl:version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://www.esdal.com/schemas/core/routeanalysis" xmlns:b="http://www.esdal.com/schemas/core/caution" xmlns:c="http://www.esdal.com/schemas/core/contact" xmlns:d="http://www.esdal.com/schemas/core/esdalcommontypes" xmlns:e="http://www.esdal.com/schemas/core/formattedtext"  xmlns:f="http://www.govtalk.gov.uk/people/AddressAndPersonalDetails">
      <style>
        .ctest{width:98%; float:left;margin:1%; border:1px solid #cccce2;}

        .ctest_inner{width:100%; float:left;}

        .ctest_title{width: 19.5%;
        float: left;
        font-weight: normal !important;
        padding-left: 0.5%;}

        .ctest_content{width: 80%;
        float: left;
        font-weight: bold;}

        .cborder{border-top:1px solid #cccce2;}

      </style>

      <body>
        <div class="row pl-1">
          <div class="cardSOA1 mt-0">
            <h6 class="text-color3 stHeading1 m-0">Select route part</h6>
          </div>
          <div class="col-9 c9p40Helper c9H1 pl-0">
            <div id="dlfId" class="flexer radioSOA mt-4 ml-0">
              <xsl:for-each select="/a:AnalysedCautions/a:AnalysedCautionsPart">
                <xsl:variable name="routedivcau" select="concat('routedivcau',position())"/>

                <div class="form-check">
                  <input type="radio" name="route_selectcau" class="form-check-input route_selectcau" value="{$routedivcau}"/>
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
        </div>
        <div class="row">
          <xsl:for-each select="/a:AnalysedCautions/a:AnalysedCautionsPart">
            <xsl:variable name="routedivcau" select="concat('routedivcau',position())"/>
            <div id="{$routedivcau}" class="routedivclasscau pl-0 pr-0" >
              <div class="cardSOA1">
                <h6 class="text-color3 stHeading1 m-0">
                  Route part <countNo>
                    <xsl:value-of select="position()"/>
                  </countNo>
                  - (<xsl:apply-templates select="a:Name/text()"/>)
                </h6>
              </div>

              <xsl:choose>
                <xsl:when test="./a:Caution">
                  <div class="pl-0">
                    <span  id="cautionExist"></span>
                    <xsl:for-each select="./a:Caution">
                      <div class="pt-4 pb-2">
                        <span class="text-highlight" style="font-size: 25px;">
                          <!--Caution-->
                          <xsl:choose>
                            <xsl:when test="b:Name/text()=''">
                              <xsl:text> </xsl:text>No name
                            </xsl:when>
                            <xsl:when test="b:Name/text()!=''">
                              <xsl:text> </xsl:text>
                              <xsl:apply-templates select="b:Name/text()"/>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text> </xsl:text>No name
                            </xsl:otherwise>
                          </xsl:choose>
                        </span>
                      </div>

                      <xsl:if test="./b:Contact">
                        <xsl:for-each select="./b:Contact">
                          <div class="row mt-2 ml-0">
                            <div class="col-lg-3 col-sm-3 col-md-3  pl-0">
                              <span class="details">Name</span>
                            </div>
                            <div class="col-lg-9 col-sm-6 col-md-6">
                              <span class="details1">
                                <!--<xsl:text>: </xsl:text>-->
                                <xsl:apply-templates select="c:FullName/text()"/>
                              </span>
                            </div>
                          </div>
                          <div class="row mt-2 ml-0">
                            <div class="col-lg-3 col-sm-3 col-md-3  pl-0">
                              <span class="details">Organisation</span>
                            </div>
                            <div class="col-lg-9 col-sm-6 col-md-6">
                              <span class="details1">
                                <!--<xsl:text>: </xsl:text>-->
                                <xsl:apply-templates select="c:OrganisationName/text()"/>
                              </span>
                            </div>
                          </div>
                          <div class="row mt-2 ml-0">
                            <div class="col-lg-3 col-sm-3 col-md-3  pl-0">
                              <span class="details">Address</span>
                            </div>
                            <div class="col-lg-9 col-sm-6 col-md-6">
                              <div class="details1">
                                <!--<xsl:text>: </xsl:text>-->
                                <xsl:choose>
                                  <xsl:when test="c:Address/d:Line">
                                    <xsl:for-each select="./c:Address/d:Line">
                                      <!--<xsl:choose>-->
                                      <xsl:if test="position() &lt; 4">
                                        <xsl:apply-templates select="text()"/>
                                        <xsl:text>, </xsl:text>
                                      </xsl:if>
                                    </xsl:for-each>
                                    <div class="details1" style="margin-left:10px;">
                                      <xsl:for-each select="./c:Address/d:Line">
                                        <!--<div class="ctest_content">-->
                                        <xsl:choose>
                                          <xsl:when test="position() = 4 ">
                                            <xsl:if test="text() != ''">
                                              <xsl:apply-templates select="text()"/>
                                            </xsl:if>
                                          </xsl:when>
                                          <xsl:otherwise>
                                            <xsl:if test="position() = 5">
                                              <xsl:if test="text() != ''">
                                                <xsl:text>, </xsl:text>
                                                <xsl:apply-templates select="text()"/>
                                                <xsl:text> </xsl:text>
                                              </xsl:if>
                                            </xsl:if>
                                          </xsl:otherwise>
                                        </xsl:choose>
                                      </xsl:for-each>
                                    </div>
                                  </xsl:when>
                                  <xsl:otherwise>
                                    <xsl:for-each select="./c:Address/f:Line">
                                      <xsl:if test="position() &lt; 4">
                                        <xsl:apply-templates select="text()"/>
                                        <xsl:text>, </xsl:text>
                                      </xsl:if>
                                    </xsl:for-each>
                                    <!--<div class="details1" style="margin-left:10px;">-->
                                    <xsl:for-each select="./c:Address/f:Line">
                                      <xsl:choose>
                                        <xsl:when test="position() = 4 ">
                                          <xsl:if test="text() != ''">
                                            <xsl:apply-templates select="text()"/>
                                          </xsl:if>
                                        </xsl:when>
                                        <xsl:otherwise>
                                          <xsl:if test="position() = 5">
                                            <xsl:if test="text() != ''">
                                              <xsl:text>, </xsl:text>
                                              <xsl:apply-templates select="text()"/>
                                              <xsl:text> </xsl:text>
                                            </xsl:if>
                                          </xsl:if>
                                        </xsl:otherwise>
                                      </xsl:choose>
                                    </xsl:for-each>
                                    <!--</div>-->
                                  </xsl:otherwise>
                                </xsl:choose>
                              </div>
                            </div>
                          </div>
                          <div class="row mt-2 ml-0">
                            <div class="col-lg-3 col-sm-3 col-md-3  pl-0">
                              <span class="details">Country</span>
                            </div>
                            <div class="col-lg-9 col-sm-6 col-md-6">
                              <span class="details1">
                                <xsl:apply-templates select="c:Address/d:Country/text()"/>
                              </span>
                            </div>
                          </div>
                          <div class="row mt-2 mb-2 ml-0">
                            <div class="col-lg-3 col-sm-3 col-md-3  pl-0">
                              <span class="details">Tel</span>
                            </div>
                            <div class="col-lg-9 col-sm-6 col-md-6">
                              <span class="details1">
                                <!--<xsl:text>: </xsl:text>-->
                                <xsl:choose>
                                  <xsl:when test="c:MobileNumber">
                                    <xsl:apply-templates select="c:MobileNumber/text()"/>
                                  </xsl:when>
                                  <xsl:otherwise>
                                    <xsl:apply-templates select="c:TelephoneNumber/text()"/>
                                  </xsl:otherwise>
                                </xsl:choose>
                              </span>
                            </div>
                          </div>
                          <div class="row mt-4 ml-0">
                            <div class="col-lg-3 col-sm-3 col-md-3  pl-0">
                              <span class="details">Email</span>
                            </div>
                            <div class="col-lg-9 col-sm-6 col-md-6">
                              <span class="details1">
                                <!--<xsl:text>: </xsl:text>-->
                                <xsl:apply-templates select="c:EmailAddress/text()"/>
                              </span>
                            </div>
                          </div>
                        </xsl:for-each>

                      </xsl:if>
                      <xsl:if test="./b:Conditions">
                        <div class="row mt-2 ml-0">
                          <div class="col-lg-3 col-sm-3 col-md-3  pl-0">
                            <span class="details">Affected roads</span>
                          </div>
                          <div class="col-lg-9 col-sm-6 col-md-6">
                            <span class="details1">
                              <!--<xsl:text>: </xsl:text>-->
                              <xsl:if test="./a:Road/d:Number">
                                <xsl:apply-templates select="a:Road/d:Number/text()"/>
                              </xsl:if>
                              <xsl:if test="./a:Road/d:Name">
                                <xsl:text>  </xsl:text>
                                <xsl:apply-templates select="a:Road/d:Name/text()"/>
                              </xsl:if>
                            </span>
                          </div>
                        </div>
                        <xsl:if test="./b:Conditions/b:MaxGrossWeight/text() &gt; 0">
                          <div class="row mt-2 ml-0">

                            <div class="col-lg-3 col-sm-3 col-md-3  pl-0">
                              <span class="details">Gross weight</span>
                            </div>
                            <div class="col-lg-9 col-sm-6 col-md-6">
                              <span class="details1">
                                <!--<xsl:text>: </xsl:text>-->
                                <xsl:apply-templates select="b:Conditions/b:MaxGrossWeight"/> kg
                              </span>
                            </div>
                          </div>
                        </xsl:if>
                        <xsl:if test="./b:Conditions/b:MaxAxleWeight/text() &gt; 0">
                          <div class="row mt-2 ml-0">
                            <div class="col-lg-3 col-sm-3 col-md-3  pl-0">
                              <span class="details">Axle weight</span>
                            </div>
                            <div class="col-lg-9 col-sm-6 col-md-6">
                              <span class="details1">
                                <!--<xsl:text>: </xsl:text>-->
                                <xsl:apply-templates select="b:Conditions/b:MaxAxleWeight"/> kg
                              </span>
                            </div>
                          </div>
                        </xsl:if>
                      </xsl:if>

                      <div class="row mt-2 ml-0">
                        <div class="col-lg-3 col-sm-3 col-md-3  pl-0">
                          <span class="details">Cautioned entity</span>
                        </div>
                        <div class="col-lg-9 col-sm-6 col-md-6">
                          <span class="details1">
                            <xsl:if test="./a:CautionedEntity/a:Constraint">
                              <div class="details1">
                                <!--<xsl:text>: </xsl:text>-->

                                Constraint : <xsl:apply-templates select="a:CautionedEntity/a:Constraint/a:Name/text()"/> (<xsl:apply-templates select="a:CautionedEntity/a:Constraint/a:Type/text()"/>)  (<xsl:apply-templates select="a:CautionedEntity/a:Constraint/a:ECRN/text()"/>)
                                <!--</span>-->
                              </div>
                            </xsl:if>

                            <xsl:if test="./a:CautionedEntity/a:Structure">
                              <div class="details1">
                                <!--<xsl:text>: </xsl:text>-->
                                <!--<span id="span_Structure">-->
                                Structure : <xsl:apply-templates select="a:CautionedEntity/a:Structure/a:Name/text()"/> (<xsl:apply-templates select="a:CautionedEntity/a:Structure/a:Type/text()"/>)  (<xsl:apply-templates select="a:CautionedEntity/a:Structure/a:ESRN/text()"/>)
                                <!--</span>-->
                              </div>
                            </xsl:if>

                          </span>
                        </div>
                      </div>

                      <div class="row mt-2 ml-0">
                        <div class="col-lg-3 col-sm-3 col-md-3  pl-0">
                          <span class="details">Affected vehicles</span>
                        </div>
                        <div class="col-lg-9 col-sm-6 col-md-6">
                          <span class="details1">
                            <!--<xsl:text>: </xsl:text>-->
                            <xsl:choose>
                              <xsl:when test="a:Vehicle/text() = 'Suitable'">
                                None
                              </xsl:when>
                              <xsl:when test="./a:Vehicle">
                                <xsl:apply-templates select="a:Vehicle"/>
                              </xsl:when>
                              <xsl:otherwise>
                                All Vehicles
                              </xsl:otherwise>
                            </xsl:choose>
                          </span>
                        </div>
                      </div>
                      <xsl:if test="./b:Action/b:SpecificAction/text()!=''">
                        <div class="row mt-2 ml-0">
                          <div class="col-lg-3 col-sm-3 col-md-3  pl-0">
                            <span class="details">Specific action</span>
                          </div>
                          <div class="col-lg-9 col-sm-6 col-md-6">
                            <span class="details1">
                              <!--<xsl:text>: </xsl:text>-->
                              <xsl:apply-templates select="b:Action/b:SpecificAction/text()"/>
                            </span>
                          </div>
                        </div>
                      </xsl:if>
                      <xsl:if test="./b:ConstrainingAttribute">
                        <div class="row mt-2 ml-0">
                          <div class="col-lg-3 col-sm-3 col-md-3  pl-0">
                            <span class="details">Constraining attribute</span>
                          </div>
                          <div class="col-lg-9 col-sm-6 col-md-6">
                            <span class="details1">
                              <!--<xsl:text>: </xsl:text>-->
                              <xsl:for-each select="./b:ConstrainingAttribute">
                                <xsl:if test="position() &gt; 1">
                                  <xsl:text>, </xsl:text>
                                </xsl:if>
                                <xsl:apply-templates select="text()"/>
                              </xsl:for-each>
                            </span>
                          </div>
                        </div>
                      </xsl:if>


                    </xsl:for-each>
                  </div>
                </xsl:when>
                <xsl:otherwise>
                  <div class="flexer radioSOA mt-4 ml-0">
                    <span class="text-highlight">
                      <b style="color:#414193;">
                        No Cautions
                      </b>
                    </span>
                  </div>
                </xsl:otherwise>
              </xsl:choose>
            </div>
          </xsl:for-each>
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


