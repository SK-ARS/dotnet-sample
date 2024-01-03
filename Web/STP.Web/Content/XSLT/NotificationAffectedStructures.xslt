<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"  xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="urn:my-scripts" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://www.esdal.com/schemas/core/routeanalysis" xmlns:b="http://www.esdal.com/schemas/core/structure" xmlns:c="http://www.esdal.com/schemas/core/formattedtext">
  <xsl:param name="OrgId"></xsl:param>
  <xsl:template match="/">
    <div id='AffectedStructureXslt'>
      <xsl:for-each select="/a:AnalysedStructures/a:AnalysedStructuresPart">
        <xsl:variable name="routediv" select="concat('routediv',position())"/>
        <xsl:variable name="routeId" select="@Id"/>
        <table id="{$routediv}" class="routedivclass">
          <tbody>
            <tr>
              <th class="headgrad">
                AFFECTED STRUCTURE (<xsl:apply-templates select="a:Name/text()"/>)
              </th>
            </tr>
            <xsl:for-each select="./a:Structure">
              <xsl:variable name="sectionId" select="@StructureSectionId"/>
              <xsl:if test="./a:StructureResponsibility/a:StructureResponsibleParty/@OrganisationId=$OrgId">
                <tr>
                  <td>
                    <xsl:choose>
                      <xsl:when test="./@TraversalType='overbridge'">
                        <span>The route traverses under </span>
                      </xsl:when>
                      <xsl:otherwise>
                        <span>The route traverses over </span>
                      </xsl:otherwise>
                    </xsl:choose>
                    <xsl:value-of select=" a:ESRN/text()"/>-
                    <xsl:apply-templates select=" a:Name/text()"/>
                  </td>
                </tr>
              </xsl:if>
            </xsl:for-each>
          </tbody>
        </table>
      </xsl:for-each>
    </div>
  </xsl:template>

  <xsl:template match="text()">
    <xsl:value-of select="."/>
  </xsl:template>

</xsl:stylesheet>