<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ns1="http://www.esdal.com/schemas/core/proposedroute"
    xmlns:ns2="http://www.esdal.com/schemas/core/movement"
    xmlns:ns3="http://www.esdal.com/schemas/core/vehicle"
    xmlns:ns4="http://www.esdal.com/schemas/core/esdalcommontypes"
    xmlns:ns5="http://www.esdal.com/schemas/core/route"
    xmlns:ns6="http://www.esdal.com/schemas/core/drivinginstruction"
    xmlns:ns7="http://www.esdal.com/schemas/core/formattedtext"
    xmlns:ns8="http://www.govtalk.gov.uk/people/bs7666"
    xmlns:ns9="http://www.esdal.com/schemas/core/annotation"
    xmlns:ns10="http://www.esdal.com/schemas/core/caution">

  <xsl:param name="Contact_ID"></xsl:param>
  <xsl:param name="DocType"></xsl:param>

  <xsl:template match="/ns1:Proposal">

    <html>
      <body>
        <!--<img src="~/Content/Images/documentHeader.png" />
        <p>
          FACSIMILE MESSAGE - Provisional Route
        </p>
        <p>
          Classification: SPECIAL ORDER
        </p>-->
        <div>
          <div style="text-align: end;padding-top: 1cm;padding-right: 2cm;">
            <xsl:choose>
              <xsl:when test="$DocType = 'EMAIL'">
                <img align="right" width="540" height="80" src="https://esdal.dft.gov.uk/Content/Images/ESDAL 2 Logo_org.png"/>
              </xsl:when>
              <xsl:otherwise>
                <img align="right" width="540"  id="hdr_img"/>
              </xsl:otherwise>
            </xsl:choose>
          </div>
        </div>
        <div>
          <!--FACSIMILE MESSAGE-->
        </div>
        <br />
        <table  bgcolor="#e9eef4">
          <tr>
            <td style="padding-left:3em;">
              <h2>Provisional route</h2>
            </td>
          </tr>
          <tr>
            <td style="padding-left:3em;font-style: normal">

              <span >
                Classification: <span style="color:#275795">special order</span>
              </span>
              <br />
              <br />
            </td>
          </tr>
          <br>

          </br>
        </table>
        <br />
        <br />
        <br />
        <table  width  ="400" cellspacing ="0" cellpadding ="0">
          <tr>
            <td align ="left" width ="100">
              Date Sent:
            </td>
            <td>
              <xsl:value-of select="ns2:SentDateTime" />
            </td>
          </tr>
          <xsl:if test="@DocType = 'PDF'">
            <tr>
              <td align ="left" width ="200">
                No of Pages:
              </td>
              <td>
                ###Noofpages###
              </td>
            </tr>
          </xsl:if>
          <tr>
            <td align ="left" width ="200">
              AIL CONTACT:
            </td>
            <td colspan="2">
              <xsl:value-of select="ns2:HAContact/ns2:Contact" />
              <br/>
              <xsl:value-of select="ns2:HAContact/ns2:Address" />
              <br/>
              <xsl:value-of select="ns2:HAContact/ns2:Address/ns4:PostCode" />
              <br/>
              <xsl:value-of select="ns2:HAContact/ns2:Address/ns4:Country" />
              <br/>
              <xsl:value-of select="ns2:HAContact/ns2:FaxNumber" />
            </td>
          </tr>

        </table>

        <p>
          To :
        </p>
        <xsl:for-each select="ns2:Recipients/ns2:Contact">
          <p>
            <xsl:if test="@ContactId = $Contact_ID">
              <b>
                <xsl:value-of select="ns2:ContactName" />,<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:value-of select="ns2:OrganisationName" /><br/>
              </b>
            </xsl:if>
            <xsl:if test="@ContactId != $Contact_ID">
              <xsl:value-of select="ns2:ContactName" />,<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:value-of select="ns2:OrganisationName" /><br/>
            </xsl:if>
          </p>
        </xsl:for-each>

        <p>
          Route Summary for
        </p>
        <br/>
        <p>

          Provisional Route

        </p>
        <table  width  ="400" cellspacing ="0" cellpadding ="0">


          <tr>
            <td align ="left" width ="100">
              ESDAL reference:
            </td>
            <td>

            </td>
          </tr>
          <!--<tr>
            <td align ="left" width ="200">
              HA Reference:
            </td>
            <td>

            </td>
          </tr>-->
          <tr>
            <td align ="left" width ="200">
              Quick Reference:
            </td>
            <td>

            </td>
          </tr>
          <tr>
            <td align ="left" width ="200">
              From:
            </td>
            <td>

            </td>
          </tr>
          <tr>
            <td align ="left" width ="200">
              To:
            </td>
            <td>

            </td>
          </tr>
          <tr>
            <td align ="left" width ="200">
              Haulier:
            </td>
            <td  width ="300" cellspacing ="0" >
              <xsl:value-of select="ns2:HaulierDetails/ns2:HaulierName" />
            </td>
          </tr>
          <tr>
            <td align ="left" width ="200">
              Haulier Contact Details:
            </td>
            <td  width ="300" cellspacing ="0" height="100" >
              <xsl:value-of select="ns2:HaulierDetails/ns2:HaulierContact" />

              <xsl:value-of select="ns2:HaulierDetails/ns2:HaulierAddress" />
            </td>
          </tr>


          <tr>
            <td align ="left" width ="200">
              Telephone Number:
            </td>
            <td  width ="300" cellspacing ="0" >
              <xsl:value-of select="ns2:HaulierDetails/ns2:TelephoneNumber" />
            </td>
          </tr>
          <tr>
            <td align ="left" width ="100" valign="top">
              Email address:
            </td>
            <td align ="left" width ="60"  >
              <xsl:if test="contains(ns2:HaulierDetails/ns2:EmailAddress, '##**##')">
                <xsl:value-of select="substring-after(ns2:HaulierDetails/ns2:EmailAddress, '##**##')"/>
              </xsl:if>
              <xsl:if test="contains(ns2:HaulierDetails/ns2:EmailAddress, '##**##')=false()">
                <xsl:value-of select="ns2:HaulierDetails/ns2:EmailAddress"/>
              </xsl:if>
            </td>
            <td border ="0">
            </td>
          </tr>
          <!--<tr>
            <td align ="left" width ="200">
              Haulier Licence:
            </td>
            <td  width ="300" cellspacing ="0" >

            </td>
          </tr>-->
        </table>

        <br/>
        <p>
          Approximate date of first movement: <xsl:value-of select="ns2:JourneyTiming/ns2:FirstMoveDate" />

        </p>
        <p style="margin-left:20px;">
          <!--1. The National Highways has been requested to supply a route for the above mentioned movement(s).<br/>-->
          1. All highway, bridge and agent authorities should assess the adequacy of the route for the vehicle or the <br/>
          vehicle combination at the scheduled date of movement and confirm their acceptance / rejection via the <br/>
          Esdal on-line collaboration facility or by email. <br/>
          2. The assessment should take account of the capacity of the bridge structures, the strength of the<br/>
          carriageway, and headroom and ground clearances. The assessment should also take into account the<br/>
          possibility of damage to statutory undertakers' apparatus, which may be present under the road surface<br/>
          with shallow cover.
          3. Would Network Rail also please provide the following information regarding level crossings and overhead
          wires.
          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>i. confirmation of the location of automatic half-barrier crossings indicated on the route and any
          additional locations.
          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text> ii. the location of any other type of public or private road level crossings where special safety
          precautions and arrangements would be required.
          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>iii. locations of those level crossings where the track level could affect the road profile.
          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>iv. the safe clearance under any overhead Network Rail electrified wires crossing the route and details
          of any special arrangements that should be made.
          <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text> v. where circumstances require, the designation and telephone number of the local Railway Officer
          whom the haulier should contact before the passage of the vehicle over the crossing.
          4. In those cases where the manoeuvrability is in doubt, arrangements will be made for a joint route
          survey by the haulier with the authority concerned.
          5. Copies of all relevant observations from the authorities consulted for the route (and of any assessments
          involved) and any other comments you may have should be forwarded without delay.
          6. (In cases where the date of movement is well in advance, your early observations are required to enable
          the manufacturer to know whether or not a route is available before the actual manufacture commences).

        </p>

        <p>
          ROUTE OVERVIEW <br/>
          Number of Movements: <xsl:value-of select="ns2:LoadDetails/ns2:TotalMoves" /> Number of Pieces moved at one time : <xsl:value-of select="ns2:LoadDetails/ns2:MaxPiecesPerMove" />

        </p>

        <p >
          Leg <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>Mode <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>Route<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text><xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>Distance
          <br/>

          <xsl:value-of select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:LegNumber" />
          Road
          <br/>
          <xsl:value-of select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:Name" />
          <br/>
          <xsl:value-of select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Distance/ns2:Metric/ns2:Distance" />

        </p>
        <br/>

        <table border = "1" width = "400">

          <tr>
            <th>Road</th>
            <th>Distance</th>
          </tr>
          <xsl:for-each select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Roads
            /ns2:RouteSubPartListPosition/ns2:RouteSubPart/ns2:PathListPosition/ns2:Path/ns2:RoadTraversalListPosition/ns2:RoadTraversal">
            <xsl:if test="ns2:RoadIdentity != '' and ns2:Distance/ns2:Metric!=''">
              <tr>
                <td>
                  <xsl:value-of select="ns2:RoadIdentity"/>
                </td>
                <td>
                  <xsl:value-of select="ns2:Distance/ns2:Metric"/>miles
                </td>
              </tr>
            </xsl:if>
          </xsl:for-each >
        </table>

        <p>
          Route Details Leg 1: <xsl:value-of select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:Name" /> <xsl:value-of select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Distance/ns2:Metric/ns2:Distance" />miles

        </p>

        <br/>

        <table border = "1" width = "500">

          <tr>
            <th>Summary:</th>
            <th>SemiConfig:</th>
          </tr>
          <tr>
            <td>Overall Length: </td>
            <td>
              <xsl:value-of select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/
          ns3:OverallLengthListPosition/ns3:OverallLength/ns3:IncludingProjections"/>m(Including Projections),
              <xsl:value-of select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/
          ns3:OverallLengthListPosition/ns3:OverallLength/ns3:ExcludingProjections"/>m(Excluding projections)
            </td>
          </tr>
          <tr>
            <td>Rigid Length:</td>
            <td>
              <xsl:value-of select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/
          ns3:RigidLengthListPosition/ns3:RigidLength"/>m
            </td>
          </tr>
          <tr>
            <td>Overall Width:</td>
            <td>
              <xsl:value-of select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/
          ns3:OverallWidthListPosition/ns3:OverallWidth"/>m
            </td>
          </tr>
          <tr>
            <td>Overall Height:</td>
            <td>
              <xsl:value-of select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/
          ns3:OverallHeightListPosition/ns3:OverallHeight/ns3:MaxHeight"/>m
            </td>
          </tr>
          <tr>
            <td>Gross Weight:</td>
            <td>
              <xsl:value-of select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/
          ns3:GrossWeightListPosition/ns3:GrossWeight/ns3:Weight"/>Kg
            </td>
          </tr>
          <tr>
            <td>Max Axle Weight:</td>
            <td>
              <xsl:value-of select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/
          ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:MaxAxleWeight/ns3:Weight"/>Kg
            </td>
          </tr>

        </table>
        <br/>

        <table border = "1" width = "500">
          <tr>
            <td>Gross Weight:</td>
            <td>
              <xsl:value-of select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/
          ns3:GrossWeightListPosition/ns3:GrossWeight/ns3:Weight"/>Kg
            </td>
          </tr>
          <tr>
            <td>Axle Weight:</td>
            <td>
              <xsl:for-each select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/
          ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:AxleConfiguration/ns3:AxleWeightListPosition">

                <xsl:value-of select="ns3:AxleWeight"/>Kg * <xsl:value-of select="ns3:AxleWeight/@AxleCount"/> ,


              </xsl:for-each>
            </td>

          </tr>
          <tr>
            <td>Wheel Per Axle:</td>
            <td>
              <xsl:for-each select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/
          ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:AxleConfiguration/ns3:WheelsPerAxleListPosition">

                <xsl:value-of select="ns3:WheelsPerAxle"/> * <xsl:value-of select="ns3:WheelsPerAxle/@AxleCount"/> ,
              </xsl:for-each>
            </td>


          </tr>

          <tr>
            <td>Axle Spacing:</td>

            <td>
              <xsl:for-each select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/
          ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:AxleConfiguration/ns3:AxleSpacingListPosition">

                <xsl:value-of select="ns3:AxleSpacing"/>m*<xsl:value-of select="ns3:AxleSpacing/@AxleCount"/>,
              </xsl:for-each>
            </td>

          </tr>

          <tr>
            <td>Tyre Size:</td>
            <td>
              <xsl:for-each select="ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:AxleConfiguration/ns3:TyreSizeListPosition">

                <xsl:value-of select="ns3:TyreSize"/>*<xsl:value-of select="ns3:TyreSize/@AxleCount"/>,
              </xsl:for-each>
            </td>
          </tr>

          <tr>
            <td>Tyre Centre:</td>
            <td> Kg </td>
          </tr>

          <tr>
            <td>Length:</td>
            <td>
              <xsl:value-of select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/
          ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:RigidLength"/>m
            </td>
          </tr>

          <tr>
            <td>Width:</td>
            <td>
              <xsl:value-of select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/
          ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:Width"/>m
            </td>
          </tr>

          <tr>
            <td>Max. Height:</td>
            <td>
              <xsl:value-of select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/
          ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:Height/ns3:MaxHeight"/>m
            </td>
          </tr>
          <tr>
            <td>Reducible Height:</td>
            <td>
              <xsl:value-of select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/
          ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:Height/ns3:ReducibleHeight"/>m
            </td>
          </tr>
          <tr>
            <td>Wheelbase:</td>
            <td>
              <xsl:value-of select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/
          ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:Wheelbase"/>m
            </td>
          </tr>

          <tr>
            <td>Rear Overhang:</td>
            <td>
              <xsl:value-of select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/
          ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:RearOverhang"/>m
            </td>
          </tr>

          <tr>
            <td>Outside Track:</td>
            <td>
              <xsl:value-of select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns2:Vehicles/
          ns3:VehicleSummaryListPosition/ns3:VehicleSummary/ns3:Configuration/ns3:SemiVehicle/ns3:OutsideTrack"/>m
            </td>
          </tr>

        </table>

        <br/>

        <table border = "1" width = "500" >
          <tr>
            <th>Route Directions</th>
            <th>Cautions</th>
          </tr>

          <xsl:for-each select="ns2:RouteParts/ns2:RoutePartListPosition/ns2:RoutePart/ns2:RoadPart/ns6:DrivingInstructions
            /ns6:SubPartListPosition/ns6:SubPart/ns6:AlternativeListPosition/ns6:Alternative/ns6:InstructionListPosition/ns6:Instruction/ns6:Navigation">
            <tr>
              <td>
                <xsl:value-of select="ns6:Instruction"/>
                <xsl:value-of select="ns7:Bold"/>
              </td>
              <td>
                <xsl:value-of select="ns6:NoteListPosition/ns6:Note/ns6:GridReference/ns8:X"/>
                <xsl:value-of select="ns6:NoteListPosition/ns6:Note/ns6:GridReference/ns8:Y"/>
                <br/>
                <xsl:value-of select="ns6:NoteListPosition/ns6:Note/ns6:Content/ns6:Annotation/ns9:Text"/>
                <xsl:value-of select="ns6:NoteListPosition/ns6:Note/ns6:Content/ns6:Caution/ns10:Action/ns10:SpecificAction"/>
                <xsl:value-of select="ns6:NoteListPosition/ns6:Note/ns6:Content/ns6:RoutePoint/@PointType"/>
                <xsl:value-of select="ns6:NoteListPosition/ns6:Note/ns6:Content/ns6:RoutePoint/ns6:Description"/>

              </td>
            </tr>
          </xsl:for-each >
        </table>


        <p>
          NOTES FOR HAULIER

        </p>
        <p>
          DOCK CAUTION
          <br/>
          The manufacturer and/or hauliers are reminded that it is their responsibility to negotiate with the appropriate dock
          manager for the movements within the dock area.

        </p>
        <p>
          MOTORWAY CAUTIONS
          <br/>
          (a) When travelling on the motorway, vehicles must not travel on the hard shoulder or in the right hand lane.<br/>
          (b) When crossing motorway bridges, vehicles must travel in the left hand lane.<br/>
          (c) If over 4.877 m in height, height of load must be reduced to a minimum when travelling under motorway<br/>
          bridges.<br/>
          (d) If under but near 4.877 m in height, extreme caution must be exercised when travelling under motorway<br/>
          bridges.<br/>

        </p>
      </body>

    </html>

  </xsl:template>
</xsl:stylesheet>
