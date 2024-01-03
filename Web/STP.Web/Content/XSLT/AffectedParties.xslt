<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://www.esdal.com/schemas/core/movement" xmlns:b="http://www.esdal.com/schemas/core/contact" xmlns:c="http://www.esdal.com/schemas/core/esdalcommontypes" xmlns:movement="http://www.esdal.com/schemas/core/movement" xmlns:contact="http://www.esdal.com/schemas/core/contact">
  <xsl:param name="SORTType"></xsl:param>
  <xsl:param name="ProjectStatus"></xsl:param>
  <xsl:template match="/">
    <html xsl:version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:a="http://www.esdal.com/schemas/core/movement" xmlns:b="http://www.esdal.com/schemas/core/contact" xmlns:c="http://www.esdal.com/schemas/core/esdalcommontypes" xmlns:movement="http://www.esdal.com/schemas/core/movement" xmlns:contact="http://www.esdal.com/schemas/core/contact">
      <body>

        <div class="pl-0 main-sort-content">
          <div class="main-title">
            <span class="text-normal"> Affected Parties </span>
            <span class="details ml-2" style="float: right;">
              <button class="btn outline-btn-primary" type="button" id="addaddressbook"><i class="fa fa-book" aria-hidden="true"></i>
                  ADDRESS BOOK 
              </button>
            </span>
            <span class="details" style="float: right;">
              <button class="btn outline-btn-primary add-contact-popup" type="button" id="addcontact">
               <i class="fa fa-plus" aria-hidden="true"></i>  ADD
                 CONTACT
              </button>
            </span>

          </div>
        </div>
        <div class="main-table mt-0 pt-0" style="border-radius:25px 0px 0px 0px" id="table-first2">
          <xsl:for-each select="/a:AffectedParties/a:GeneratedAffectedParties">
            <table>
              <thead>
                <tr>
                  <th style="width:25%">Organisation</th>
                  <th style="width:20%">Contact</th>
                  <xsl:if test="$SORTType!=''">
                  <th style="width:25%">Status</th>
                  </xsl:if>
                  <xsl:if test="$SORTType=''">
                    <th style="width:20%">Dispensation Status</th>
                    <th style="width:10%;background-color: rgba(39, 87, 149, 1) !important;"></th>
                  </xsl:if>
                  <xsl:if test="$SORTType!=''">
                    <th style="width:18%">
                    </th>
                  </xsl:if>
                </tr>
              </thead>
              <tbody>
                <xsl:choose>
                  <xsl:when test="a:AffectedParty">
                    <xsl:for-each select="./a:AffectedParty">
                      <tr>
                        <td class="text-color1">
                          <span id="{./a:Contact/b:Contact/b:SimpleReference/b:OrganisationName}">
                            <xsl:apply-templates select="a:Contact/b:Contact/b:SimpleReference/b:OrganisationName"/>
                          </span>
                        </td>
                        <td class="text-color2">
                          <a class="text-decoration-link" >
                            <span class="display-contact" contactid="{./a:Contact/b:Contact/b:SimpleReference/@ContactId}" id="{./a:Contact/b:Contact/b:SimpleReference/b:FullName}">
                              <xsl:apply-templates select="a:Contact/b:Contact/b:SimpleReference/b:FullName"/>
                              <xsl:if test="@Reason='no longer affected'">
                                <b style="color:red">*</b>
                              </xsl:if>
                            </span>
                          </a>
                        </td>
                        <xsl:if test="$SORTType!=''">
                        <td class="text-color1" style="position: relative;">
                          <div style="float: left; width: 104px;">

                            <xsl:choose>
                              <xsl:when test="@Reason='no longer affected'">
                                <!--<b style="color:#C81117">-->
                                <xsl:apply-templates select="@Reason"/>
                                <!--</b>-->
                              </xsl:when>
                              <xsl:when test="@Reason='affected by change of route'">
                                <!--<b style="color:#00A0F0">-->
                                Affected by the change
                                <!--</b>-->
                              </xsl:when>
                              <xsl:when test="@Reason='newly affected'">
                                <!--<b style="color:green">-->
                                <xsl:apply-templates select="@Reason"/>
                                <!--</b>-->
                              </xsl:when>
                              <xsl:otherwise>
                                <!--<b style="color:#414193">-->
                                Still affected but not affected by the change
                                <!--</b>-->
                              </xsl:otherwise>
                            </xsl:choose>

                          </div>

                          <div style="float:right;position: absolute;top: 50%;-ms-transform: translateY(-50%);transform: translateY(-50%);left: 66%;">
                            <!-- fix applied for HE-3334 -->
                            <xsl:choose>
                              <xsl:when test="$SORTType=''">
                              </xsl:when>

                              <xsl:otherwise>
                                <xsl:if test="@Exclude='true'">
									<a href="#">
								<img id="YesIcon{./a:Contact/b:Contact/b:SimpleReference/@ContactId}" src="../Content/assets/images/no-icon.svg" contactid="{./a:Contact/b:Contact/b:SimpleReference/@ContactId}" orgid="{./a:Contact/b:Contact/b:SimpleReference/@OrganisationId}" orgname="{./a:Contact/b:Contact/b:SimpleReference/b:OrganisationName}" alt="checked" width="20" class="ml-2 .yes-icon include" title="Excluded"/>
									</a>
										</xsl:if>
                                <xsl:if test="@Exclude='false'">
									<a href="#">
                                  <img id="YesIcon{./a:Contact/b:Contact/b:SimpleReference/@ContactId}" src="../Content/assets/images/yes-icon.svg" contactid="{./a:Contact/b:Contact/b:SimpleReference/@ContactId}" orgid="{./a:Contact/b:Contact/b:SimpleReference/@OrganisationId}" orgname="{./a:Contact/b:Contact/b:SimpleReference/b:OrganisationName}" alt="checked" width="20" class="ml-2 .yes-icon exclude" title="Included"/>
									</a>
										</xsl:if>
                              </xsl:otherwise>
                            </xsl:choose>
                          </div>

                        </td>
                        </xsl:if>
                        <xsl:if test="$SORTType=''">
                          <td class="text-color1">
                            <xsl:choose>
                              <xsl:when test="@DispensationStatus='some matching'">
                                <b style="color:blue;">
                                  <xsl:apply-templates select="@DispensationStatus" />
                                </b>
                              </xsl:when>
                              <xsl:when test="@DispensationStatus='in use'">
                                <b style="color:blue;">
                                  <xsl:apply-templates select="@DispensationStatus" />
                                </b>
                              </xsl:when>
                              <xsl:when test="@DispensationStatus">
                                <xsl:apply-templates select="@DispensationStatus" />
                              </xsl:when>
                              <xsl:otherwise>
                                none matching
                              </xsl:otherwise>
                            </xsl:choose>
                          </td>
                        </xsl:if>
                        <!---->
                        <td class="text-color1">
                          <!---->
                          <xsl:choose>
                            <xsl:when test="$SORTType=''">
                              <div class="dispensation_btn">
                                <a href="#" class="view-disp-aff-party" orgid="{./a:Contact/b:Contact/b:SimpleReference/@OrganisationId}" orgname="{./a:Contact/b:Contact/b:SimpleReference/b:OrganisationName}">
                                  <img src="../Content/assets/images/edit-icon.svg" width="20" data-toggle="tooltip" data-placement="right" title="Edit"/>
                                </a>
                              </div>
                              <!---->
                            </xsl:when>
                            <xsl:otherwise>
                              <button orgid="{./a:Contact/b:Contact/b:SimpleReference/@OrganisationId}" orgname="{./a:Contact/b:Contact/b:SimpleReference/b:OrganisationName}" type="button" class="btn btn-outline-primary btn-normal import-button affect-party-show-detail" aria-hidden="true" >SHOW DETAILS</button>
                            </xsl:otherwise>
                          </xsl:choose>
                        </td>

                      </tr>
                    </xsl:for-each>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:if test="$SORTType =''">
                      <tr>
                        <td class="text-color1" colspan="5" style="text-align:center;">
                          No affected parties
                        </td>
                      </tr>
                    </xsl:if>
                    <xsl:if test="$SORTType !=''">
                      <tr>
                        <td class="text-color1" colspan="4" style="text-align:center;">
                          No affected parties
                        </td>
                      </tr>
                    </xsl:if>
                  </xsl:otherwise>
                </xsl:choose>
              </tbody>
            </table>
          </xsl:for-each>
        </div>
        <xsl:if test="./a:AffectedParties/a:ManualAffectedParties">
          <div class="pl-0 main-sort-content">
            <div class="main-title pt-8">
              <span class="text-normal"> Additional Parties </span>
            </div>
          </div>
          <div class="main-table" style="border-radius:25px 0px 0px 0px">
            <xsl:for-each select="/a:AffectedParties/a:ManualAffectedParties">
              <table>
                <thead>
                  <tr>
                    <th style="width:25%">Organisation</th>
                    <th style="width:20%">Contacts</th>
                    <th style="width:43%;background-color: rgba(39, 87, 149, 1) !important;"></th>
                  </tr>
                </thead>
                <tbody>
                  <xsl:choose>
                    <xsl:when test="./movement:AffectedParty">
                      <xsl:for-each select="./movement:AffectedParty">
                        <xsl:if test="./movement:Contact/contact:Contact/contact:SimpleReference">
                          <tr>
                            <td class="text-color1">
                              <span id="{./movement:Contact/contact:Contact/contact:SimpleReference/contact:OrganisationName}">
                                <xsl:apply-templates select="movement:Contact/contact:Contact/contact:SimpleReference/contact:OrganisationName"/>
                              </span>
                            </td>
                            <td>
                              <div id="mid_row">
                                <span class="text-color2">
                                  <span id="{./movement:Contact/contact:Contact/contact:SimpleReference/contact:FullName}">
                                    <xsl:apply-templates select="movement:Contact/contact:Contact/contact:SimpleReference/contact:FullName"/>
                                    <xsl:if test="@Reason='no longer affected'">
                                      *
                                    </xsl:if>
                                  </span>
                                </span>
                                <span id="mid_row_data">
                                  <img style="cursor: pointer;" src="assets/images/delete-icon.svg" width="20" class="delete-affect-contact" orgname="{./movement:Contact/contact:Contact/contact:SimpleReference/contact:OrganisationName}" fullname="{./movement:Contact/contact:Contact/contact:SimpleReference/contact:FullName}" />
                                </span>
                              </div>
                            </td>


                          </tr>
                        </xsl:if>
                        <xsl:if test="./movement:Contact/contact:Contact/contact:AdhocReference">
                          <tr>
                            <td class="text-color1">
                              <span id="{./movement:Contact/contact:Contact/contact:AdhocReference/contact:OrganisationName}">
                                <xsl:apply-templates select="movement:Contact/contact:Contact/contact:AdhocReference/contact:OrganisationName"/>
                              </span>
                            </td>
                            <td class="text-color2">
                              <span id="{./movement:Contact/contact:Contact/contact:AdhocReference/contact:FullName}" style="font-family: lato_bold, Arial !important;">
                                <xsl:apply-templates select="movement:Contact/contact:Contact/contact:AdhocReference/contact:FullName"/>
                                <xsl:if test="@Reason='no longer affected'">
                                  *
                                </xsl:if>
                              </span>
                            </td>
                            <td>
                              <div id="mid_row">
                                <span class="text-color2">
                                  <span id="{./movement:Contact/contact:Contact/contact:SimpleReference/contact:FullName}">
                                    <xsl:apply-templates select="movement:Contact/contact:Contact/contact:SimpleReference/contact:FullName"/>
                                    <xsl:if test="@Reason='no longer affected'">
                                      *
                                    </xsl:if>
                                  </span>
                                </span>
                                <div class ="delete-manually-affected">
                                  <span id="mid_row_data">
                                    <img style="cursor: pointer;" src="../Content/assets/images/delete-icon.svg" width="20" class="delete-affect-contact" orgname="{./movement:Contact/contact:Contact/contact:AdhocReference/contact:OrganisationName}" fullname="{./movement:Contact/contact:Contact/contact:AdhocReference/contact:FullName}" title='Delete'/>
                                  </span>
                                </div>

                              </div>
                            </td>

                          </tr>
                        </xsl:if>
                      </xsl:for-each>

                    </xsl:when>
                    <xsl:otherwise>
                      <tr>
                        <td class="text-color1" colspan="4" style="text-align:center;">
                          No additional parties
                        </td>
                      </tr>
                    </xsl:otherwise>
                  </xsl:choose>

                </tbody>
              </table>
            </xsl:for-each>
          </div>
        </xsl:if>
      </body>
    </html>
  </xsl:template>

</xsl:stylesheet>
