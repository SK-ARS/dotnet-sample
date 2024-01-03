<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://www.esdal.com/schemas/core/routeanalysis" xmlns:b="http://www.esdal.com/schemas/core/route" xmlns:c="http://www.esdal.com/schemas/core/esdalcommontypes">
  <xsl:template match="/">
    <html xsl:version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://www.esdal.com/schemas/core/routeanalysis" xmlns:b="http://www.esdal.com/schemas/core/route" xmlns:c="http://www.esdal.com/schemas/core/esdalcommontypes">
      <body>
        <div class="row">
          <div class="cardSOA1 mt-0">
            <h6 class="text-color3 stHeading1 m-0">Select route part</h6>
          </div>
          <div class="col-9 c9p40Helper c9H1 pl-0">
            <div id="dlfId" class="flexer radioSOA mt-4 ml-0">
              <xsl:for-each select="/a:AnalysedRoadsRoute/a:AnalysedRoadsPart">
                <xsl:variable name="roaddiv" select="concat('roaddiv',position())"/>
                <div class="form-check">
                  <input class="form-check-input road_select" myaffroad ="0" type="radio" name="flexRadioDefault" id="flexRadioDefault11" value="{$roaddiv}"/>
                  <label class="form-check-label">
                    <span class="text-highlight">
                      Route part  <countNo>
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

        <div class="pl-0 main-sort-content">
          <div class="main-title">
            <span class="text-normal"> Affected Roads</span>
          </div>
        </div>

        <div class="main-table" style="border-radius:25px 0px 0px 0px">
          <xsl:for-each select="/a:AnalysedRoadsRoute/a:AnalysedRoadsPart">
            <xsl:variable name="roaddiv" select="concat('roaddiv',position())"/>
            <div id="{$roaddiv}" class="roaddivclass" >
              <table>
                <thead>
                  <tr>
                    <th>Road</th>
                    <th style="background-color: rgba(39, 87, 149, 1) !important;">Distance</th>
                  </tr>
                </thead>
                <tbody>
                  <xsl:for-each select="a:SubPart/a:Roads/a:Path/a:RoadsPathSegment">
                    <xsl:if test="./a:Road/a:RoadIdentity/c:Number">
                      <tr>
                        <td class="text-color1">
                          <xsl:apply-templates select="a:Road/a:RoadIdentity/c:Number"/>
                        </td>
                        <td class="text-color1">
                          <xsl:apply-templates select="a:Road/a:Distance"/>
                        </td>
                      </tr>
                    </xsl:if>
                    <xsl:if test="./a:Road/a:RoadIdentity/c:Name">
                      <tr>
                        <td class="text-color1">
                          <xsl:apply-templates select="a:Road/a:RoadIdentity/c:Name"/>
                        </td>
                        <td class="text-color1">
                          <xsl:apply-templates select="a:Road/a:Distance"/>
                        </td>
                      </tr>
                    </xsl:if>
                  </xsl:for-each>
                </tbody>
              </table>
            </div>
          </xsl:for-each>
        </div>
      </body>
    </html>
  </xsl:template>

  <xsl:template match="a:Distance">
    <xsl:if test="./text() &lt; 1000">
      <xsl:value-of select="./text()"/> m
    </xsl:if>
    <xsl:if test="./text() &gt; 1000 or ./text() = 1000">
      <xsl:value-of select="format-number(./text() div 1000,'#.#')"/> km
    </xsl:if>
  </xsl:template>

</xsl:stylesheet>
