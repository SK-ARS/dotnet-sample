namespace STP.MovementsAndNotifications.Persistance
{
    public static class NotificationDocumentDAO
    {
        #region Removed Unwanted code by Mahzeer on 04-12-2023

        //public static NotificationXSD.PredefinedCautionsDescriptionsStructure1 Pcds { get; set; }

        //public static decimal TotalDistanceMetric { get; set; }

        //public static decimal TotalDistanceImperial { get; set; }


        #region GetRecipientDetails
        /// <summary>
        /// get recipient detail for notification document
        /// </summary>
        /// <param name="NotificationID">notification id</param>
        /// <returns></returns>
        //public static ContactModel GetRecipientDetails(long NotificationID)
        //{
        //    ContactModel contactDetail = new ContactModel();

        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
        //        contactDetail,
        //        UserSchema.Portal + ".GET_OUTBOUND_DRIVINGINS_DETAIL",
        //        parameter =>
        //        {
        //            parameter.AddWithValue("P_NOTIFICATION_ID", NotificationID, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //        },
        //           (records, instance) =>
        //           {
        //               instance.AffectedParties = records.GetByteArrayOrNull("AFFECTED_PARTIES");
        //           }
        //    );
        //    return contactDetail;
        //}
        #endregion

        #region GetContactDetails
        //public static ContactModel GetContactDetails(int contactId)
        //{
        //    ContactModel contactDetail = new ContactModel();

        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
        //        contactDetail,
        //        UserSchema.Portal + ".GET_CONTACT_DETAILS",
        //        parameter =>
        //        {
        //            parameter.AddWithValue("p_ContactId", contactId, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //        },
        //           (records, instance) =>
        //           {
        //               instance.ContactId = contactId;
        //               instance.Email = records.GetStringOrDefault("Email");
        //               instance.Fax = records.GetStringOrDefault("Fax");
        //           }
        //    );
        //    return contactDetail;
        //}
        #endregion

        #region GetDrivingInstructionStructures
        /// <summary>
        /// get driving instruction xml detail from the database
        /// </summary>
        /// <param name="NotificationID"></param>
        /// <returns></returns>
        //public static DrivingInstructionModel GetDrivingInstructionStructures(long NotificationID)
        //{
        //    string messg = "OutboundDAO/GetDrivingInstructionStructures?NotificationID=" + NotificationID;
        //    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

        //    DrivingInstructionModel structuresDetail = new DrivingInstructionModel();

        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
        //        structuresDetail,
        //        UserSchema.Portal + ".GET_OUTBOUND_DRIVINGINS_DETAIL",
        //        parameter =>
        //        {
        //            parameter.AddWithValue("P_NOTIFICATION_ID", NotificationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //        },
        //           (records, instance) =>
        //           {
        //               instance.DrivingInstructions = records.GetByteArrayOrNull("DRIVING_INSTRUCTIONS");
        //           }
        //    );
        //    return structuresDetail;
        //}
        #endregion

        #region getDrivingDetails
        /// <summary>
        /// generate driving instruction detail by processing xml
        /// </summary>
        /// <param name="DrivingInstructionInfo">DrivingInstructionModel</param>
        /// <param name="incdoccaution">Include Dock Caution</param>
        /// <returns></returns>
        //public static NotificationXSD.DrivingInstructionsStructure getDrivingDetails(DrivingInstructionModel DrivingInstructionInfo, int incdoccaution)
        //{
        //    string messg = "OutboundDAO/getDrivingDetails?incdoccaution=" + incdoccaution;
        //    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));


        //    NotificationXSD.DrivingInstructionsStructure dis = new NotificationXSD.DrivingInstructionsStructure();
        //    string recipientXMLInformation = string.Empty;

        //    Byte[] DrivingInstructionArray = DrivingInstructionInfo.DrivingInstructions;

        //    if (DrivingInstructionArray != null)
        //    {
        //        XmlDocument xmlDoc = new XmlDocument();
        //        try
        //        {
        //            recipientXMLInformation = Encoding.UTF8.GetString(DrivingInstructionArray, 0, DrivingInstructionArray.Length);

        //            recipientXMLInformation = recipientXMLInformation.Replace("<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "#b#");
        //            recipientXMLInformation = recipientXMLInformation.Replace("</Bold>", "#be#");

        //            xmlDoc.LoadXml(recipientXMLInformation);
        //        }
        //        catch
        //        {
        //            //Some data is stored in gzip format, so we need to unzip then load it.
        //            byte[] outBoundDecryptString = STP.Common.General.XsltTransformer.Trafo(DrivingInstructionArray);

        //            recipientXMLInformation = Encoding.UTF8.GetString(outBoundDecryptString, 0, outBoundDecryptString.Length);

        //            recipientXMLInformation = recipientXMLInformation.Replace("<Bold xmlns=\"http://www.esdal.com/schemas/core/formattedtext\">", "#b#");
        //            recipientXMLInformation = recipientXMLInformation.Replace("</Bold>", "#be#");

        //            xmlDoc.LoadXml(recipientXMLInformation);
        //        }

        //        List<DrivingInstructionModel> drivingInsList = new List<DrivingInstructionModel>();

        //        string legDetailsMessg = string.Empty;
        //        string instruction = string.Empty;
        //        int MeasuredMetric = 0;
        //        int DisplayMetric = 0;
        //        int DisplayImperial = 0;
        //        string PointType = string.Empty;
        //        string Description = string.Empty;
        //        ulong X = 0;
        //        ulong Y = 0;
        //        int ComparisonId = 0;
        //        int Id = 0;
        //        string LegNumber = string.Empty;
        //        string Name = string.Empty;
        //        bool MoterwayCaution = false;

        //        XmlNodeList node = xmlDoc.GetElementsByTagName("DrivingInstructions");
        //        foreach (XmlElement xmlElement1 in node)//Instruction
        //        {
        //            if (xmlElement1.Name == "DrivingInstructions")
        //            {
        //                if ((xmlElement1 != null) && xmlElement1.HasAttribute("ComparisonId"))
        //                {
        //                    ComparisonId = Convert.ToInt32(xmlElement1.Attributes["ComparisonId"].InnerText);
        //                }
        //                if ((xmlElement1 != null) && xmlElement1.HasAttribute("Id"))
        //                {
        //                    Id = Convert.ToInt32(xmlElement1.Attributes["Id"].InnerText);
        //                }
        //                if (xmlElement1["LegNumber"] != null)
        //                {
        //                    legDetailsMessg = string.Empty;
        //                    LegNumber = xmlElement1["LegNumber"].InnerText;
        //                    legDetailsMessg = "OutboundDAO/getDrivingDetails?LegNumber=" + LegNumber;
        //                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(legDetailsMessg + "; In DrivingInstructions Element"));

        //                }
        //                if (xmlElement1["Name"] != null)
        //                {
        //                    legDetailsMessg = string.Empty;
        //                    Name = xmlElement1["Name"].InnerText;
        //                    legDetailsMessg = "OutboundDAO/getDrivingDetails?Name=" + Name;
        //                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(legDetailsMessg + "; In DrivingInstructions Element"));
        //                }
        //            }
        //        }


        //        XmlNodeList parentNode = xmlDoc.GetElementsByTagName("InstructionListPosition");

        //        foreach (XmlElement xmlElement in parentNode)  //InstructionListPosition
        //        {
        //            foreach (XmlElement xmlElement1 in xmlElement.ChildNodes)//Instruction
        //            {
        //                foreach (XmlElement xmlElement2 in xmlElement1.ChildNodes)//Instruction
        //                {
        //                    if (xmlElement2.Name == "Navigation")
        //                    {
        //                        foreach (XmlElement xmlElement3 in xmlElement2.ChildNodes)
        //                        {
        //                            if (xmlElement3.Name == "Instruction")//Instruction
        //                                instruction = xmlElement3.InnerText;

        //                            if (xmlElement3.Name == "Distance")//Distance
        //                            {
        //                                foreach (XmlElement xmlElement4 in xmlElement3.ChildNodes)
        //                                {
        //                                    if (xmlElement4.Name == "MeasuredMetric")
        //                                        MeasuredMetric = Convert.ToInt32(xmlElement4.InnerText);
        //                                    if (xmlElement4.Name == "DisplayMetric")
        //                                        DisplayMetric = Convert.ToInt32(xmlElement4.InnerText);
        //                                    if (xmlElement4.Name == "DisplayImperial")
        //                                        DisplayImperial = Convert.ToInt32(xmlElement4.InnerText);
        //                                }
        //                            }
        //                        }
        //                    }
        //                    if (xmlElement2.Name == "NoteListPosition")
        //                    {
        //                        foreach (XmlElement xmlElement3 in xmlElement2.ChildNodes) //Note
        //                        {
        //                            foreach (XmlElement xmlElement4 in xmlElement3.ChildNodes) //Content
        //                            {
        //                                if (xmlElement4.Name == "Content")
        //                                {
        //                                    foreach (XmlElement xmlElement5 in xmlElement4.ChildNodes)
        //                                    {
        //                                        if (xmlElement5.Name == "RoutePoint")
        //                                        {
        //                                            if ((xmlElement5 != null) && xmlElement5.HasAttribute("PointType"))
        //                                            {
        //                                                PointType = xmlElement5.Attributes["PointType"].InnerText;
        //                                            }
        //                                            if (xmlElement5["Description"] != null)
        //                                                Description = xmlElement5["Description"].InnerText;
        //                                        }
        //                                        if (xmlElement5.Name == "MotorwayCaution")
        //                                        {
        //                                            MoterwayCaution = true;
        //                                        }
        //                                    }
        //                                }
        //                                if (xmlElement4.Name == "GridReference")
        //                                {
        //                                    foreach (XmlElement xmlElement5 in xmlElement4.ChildNodes)
        //                                    {
        //                                        if (xmlElement5.Name == "X")
        //                                        {
        //                                            X = Convert.ToUInt64(xmlElement5.InnerText);
        //                                        }
        //                                        if (xmlElement5.Name == "Y")
        //                                        {
        //                                            Y = Convert.ToUInt64(xmlElement5.InnerText);
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //            string motorwayCautionDescription = string.Empty;
        //            foreach (XmlElement xmlElements in node)//Instruction
        //            {
        //                foreach (XmlElement xmlElement1 in xmlElements.ChildNodes)//Instruction
        //                {

        //                    if (xmlElement1.Name == "MotorwayCautionDescription")
        //                    {
        //                        motorwayCautionDescription = xmlElement1.InnerText;
        //                    }
        //                }
        //            }

        //            DrivingInstructionInfo = new DrivingInstructionModel
        //            {
        //                ComparisonId = ComparisonId,
        //                Id = Id,
        //                LegNumber = LegNumber,
        //                Name = Name,
        //                Instruction = instruction,
        //                MeasuredMetricDistance = MeasuredMetric,
        //                DisplayMetricDistance = DisplayMetric,
        //                DisplayImperialDistance = DisplayImperial,
        //                PointType = PointType,
        //                Description = Description,
        //                GridRefX = X,
        //                GridRefY = Y,
        //                MotorwayCaution = MoterwayCaution
        //            };

        //            drivingInsList.Add(DrivingInstructionInfo);

        //            Pcds = new NotificationXSD.PredefinedCautionsDescriptionsStructure1();
        //            Pcds.DockCautionDescription = incdoccaution;
        //            Pcds.MotorwayCautionDescription = motorwayCautionDescription;

        //            instruction = string.Empty;
        //            MeasuredMetric = 0;
        //            DisplayMetric = 0;
        //            DisplayImperial = 0;
        //            PointType = string.Empty;
        //            Description = string.Empty;
        //            X = 0;
        //            Y = 0;
        //            MoterwayCaution = false;
        //        }




        //        List<NotificationXSD.DrivingInstructionsStructureSubPartListPosition> disslpList = new List<NotificationXSD.DrivingInstructionsStructureSubPartListPosition>();

        //        foreach (DrivingInstructionModel drivinginsstru in drivingInsList)
        //        {

        //            NotificationXSD.DrivingInstructionsStructureSubPartListPosition disslp1 = new NotificationXSD.DrivingInstructionsStructureSubPartListPosition();

        //            List<NotificationXSD.DrivingInstructionSubPartStructureAlternativeListPosition> dispsalpList = new List<NotificationXSD.DrivingInstructionSubPartStructureAlternativeListPosition>();
        //            NotificationXSD.DrivingInstructionSubPartStructureAlternativeListPosition dispsalp1 = new NotificationXSD.DrivingInstructionSubPartStructureAlternativeListPosition();


        //            NotificationXSD.DrivingInstructionListStructure dils = new NotificationXSD.DrivingInstructionListStructure();

        //            List<NotificationXSD.DrivingInstructionListStructureInstructionListPosition> dilsilpList = new List<NotificationXSD.DrivingInstructionListStructureInstructionListPosition>();
        //            NotificationXSD.DrivingInstructionListStructureInstructionListPosition dilsilp1 = new NotificationXSD.DrivingInstructionListStructureInstructionListPosition();
        //            //----------------------------------------------------
        //            NotificationXSD.DrivingInstructionStructure drvIns1 = new NotificationXSD.DrivingInstructionStructure();

        //            NotificationXSD.NavigationInstructionStructure nis1 = new NotificationXSD.NavigationInstructionStructure();
        //            NotificationXSD.SimpleTextStructure sts1 = new NotificationXSD.SimpleTextStructure();
        //            string[] str1 = new string[1];
        //            str1[0] = drivinginsstru.Instruction;
        //            sts1.Text = str1;
        //            nis1.Instruction = sts1;

        //            NotificationXSD.DrivingInstructionDistanceStructure dids = new NotificationXSD.DrivingInstructionDistanceStructure();
        //            if (drivinginsstru.MeasuredMetricDistance != 0)
        //            {
        //                dids.MeasuredMetric = Convert.ToDecimal(drivinginsstru.MeasuredMetricDistance);
        //            }
        //            if (drivinginsstru.DisplayMetricDistance != 0)
        //            {
        //                dids.DisplayMetric = Convert.ToDecimal(drivinginsstru.DisplayMetricDistance);
        //            }
        //            if (drivinginsstru.DisplayImperialDistance != 0)
        //            {
        //                dids.DisplayImperial = Convert.ToDecimal(drivinginsstru.DisplayImperialDistance);
        //            }
        //            if (dids.MeasuredMetric != 0 || dids.DisplayMetric != 0 || dids.DisplayImperial != 0)
        //            {
        //                nis1.Distance = dids;
        //            }
        //            drvIns1.Navigation = nis1;

        //            List<NotificationXSD.DrivingInstructionStructureNoteListPosition> disnlpList = new List<NotificationXSD.DrivingInstructionStructureNoteListPosition>();
        //            NotificationXSD.DrivingInstructionStructureNoteListPosition disnlp1 = new NotificationXSD.DrivingInstructionStructureNoteListPosition();

        //            NotificationXSD.DrivingInstructionNoteStructure dins1 = new NotificationXSD.DrivingInstructionNoteStructure();
        //            NotificationXSD.NoteChoiceStructure ncs1 = new NotificationXSD.NoteChoiceStructure();

        //            if (drivinginsstru.MotorwayCaution)
        //            {

        //                object motorWayCaution;

        //                motorWayCaution = "Apply motorway caution";

        //                ncs1.Item = motorWayCaution;

        //                dins1.Content = ncs1;
        //            }
        //            else
        //            {
        //                NotificationXSD.RoutePointDescriptionStructure rps1 = new NotificationXSD.RoutePointDescriptionStructure();
        //                if (drivinginsstru.PointType == "start")
        //                {
        //                    rps1.PointType = NotificationXSD.RoutePointType.start;
        //                }
        //                if (drivinginsstru.PointType == "end")
        //                {
        //                    rps1.PointType = NotificationXSD.RoutePointType.end;
        //                }
        //                if (drivinginsstru.PointType == "way")
        //                {
        //                    rps1.PointType = NotificationXSD.RoutePointType.way;
        //                }
        //                if (drivinginsstru.PointType == "intermediate")
        //                {
        //                    rps1.PointType = NotificationXSD.RoutePointType.intermediate;
        //                }
        //                rps1.Description = drivinginsstru.Description;
        //                ncs1.Item = rps1;

        //                if (rps1.Description != string.Empty)
        //                {
        //                    dins1.Content = ncs1;
        //                }
        //            }



        //            NotificationXSD.GridReferenceStructure grs1 = new NotificationXSD.GridReferenceStructure();
        //            grs1.X = drivinginsstru.GridRefX;
        //            grs1.Y = drivinginsstru.GridRefY;
        //            if (grs1.X != 0 || grs1.Y != 0)
        //            {
        //                dins1.GridReference = grs1;
        //            }

        //            if (dins1.Content != null || dins1.GridReference != null)
        //            {
        //                disnlp1.Note = dins1;
        //            }

        //            disnlpList.Add(disnlp1);
        //            drvIns1.NoteListPosition = disnlpList.ToArray();
        //            dilsilp1.Instruction = drvIns1;
        //            dilsilpList.Add(dilsilp1);
        //            //---------------------------------------------------------
        //            dils.InstructionListPosition = dilsilpList.ToArray();

        //            dispsalp1.Alternative = dils;
        //            dispsalpList.Add(dispsalp1);
        //            disslp1.SubPart = dispsalpList.ToArray();

        //            disslpList.Add(disslp1);

        //            dis.ComparisonId = drivinginsstru.ComparisonId;
        //            dis.Id = drivinginsstru.Id;
        //            dis.LegNumber = drivinginsstru.LegNumber;
        //            dis.Name = drivinginsstru.Name;
        //        }

        //        dis.SubPartListPosition = disslpList.ToArray();
        //    }

        //    return dis;
        //}
        #endregion

        #region GetOrgTypeDetails
        /// <summary>
        /// get Organisation type detail based on Organisation id
        /// </summary>
        /// <param name="OrganisationID">organistion id</param>
        /// <returns></returns>
        //public static bool GetOrgTypeDetails(int OrganisationID)
        //{
        //    ContactModel contactDetail = new ContactModel();

        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
        //        contactDetail,
        //        UserSchema.Portal + ".GET_ORGTYPE_DETAIL",
        //        parameter =>
        //        {
        //            parameter.AddWithValue("p_OrganisationID", OrganisationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //        },
        //         (records, instance) =>
        //         {
        //             if (records.GetStringOrDefault("orgtype").ToLower() == "haulier")
        //                 instance.IsHaulier = true;
        //             else
        //                 instance.IsHaulier = false;
        //         }
        //        );

        //    return contactDetail.IsHaulier;
        //}
        #endregion

        #region getRoadDetails
        /// <summary>
        /// generate roads node detail based on processing of xml file
        /// </summary>
        /// <param name="NotificationID"></param>
        /// <returns></returns>
        //public static AffectedRoadsStructure getRoadDetails(long NotificationID)
        //{
        //    string messg = "OutboundDAO/getRoadDetails?NotificationID=" + NotificationID;
        //    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

        //    AffectedRoadsStructure affRoadStruct = new AffectedRoadsStructure();
        //    DrivingInstructionModel DrivingInstructionInfo = GetDrivingInstructionStructures(NotificationID);

        //    string recipientXMLInformation = string.Empty;

        //    Byte[] DrivingInstructionArray = DrivingInstructionInfo.DrivingInstructions;

        //    if (DrivingInstructionArray != null)
        //    {
        //        XmlDocument xmlDoc = new XmlDocument();
        //        try
        //        {
        //            recipientXMLInformation = Encoding.UTF8.GetString(DrivingInstructionArray, 0, DrivingInstructionArray.Length);

        //            xmlDoc.LoadXml(recipientXMLInformation);
        //        }
        //        catch
        //        {
        //            //Some data is stored in gzip format, so we need to unzip then load it.
        //            byte[] outBoundDecryptString = STP.Common.General.XsltTransformer.Trafo(DrivingInstructionArray);

        //            recipientXMLInformation = Encoding.UTF8.GetString(outBoundDecryptString, 0, outBoundDecryptString.Length);

        //            xmlDoc.LoadXml(recipientXMLInformation);
        //        }

        //        XmlNodeList parentNode = xmlDoc.GetElementsByTagName("Navigation");
        //        NavigationXML navigationXML;
        //        List<NavigationXML> navigationXMLs = new List<NavigationXML>();
        //        foreach (XmlNode childrenNode in parentNode)
        //        {
        //            if (childrenNode.InnerText.ToLower().Contains("continue "))
        //            {
        //                navigationXML = new NavigationXML();
        //                navigationXML.Instruction = childrenNode["Instruction"].InnerText;
        //                navigationXML.MeasuredMetric = childrenNode.LastChild["MeasuredMetric"] == null ? 0 : Convert.ToDecimal(childrenNode.LastChild["MeasuredMetric"].InnerText);
        //                navigationXML.DisplayMetric = childrenNode.LastChild["DisplayMetric"] == null ? 0 : Convert.ToDecimal(childrenNode.LastChild["DisplayMetric"].InnerText);
        //                navigationXML.DisplayImperial = childrenNode.LastChild["DisplayImperial"] == null ? 0 : Convert.ToDecimal(childrenNode.LastChild["DisplayImperial"].InnerText);
        //                navigationXMLs.Add(navigationXML);
        //            }
        //        }
        //        List<NavigationXML> finalNavigationXMLs = new List<NavigationXML>();

        //        if (navigationXMLs.Count > 0)
        //        {
        //            finalNavigationXMLs = navigationXMLs.GroupBy(x => x.Instruction).Select(g => new NavigationXML
        //            {
        //                Instruction = g.Key,
        //                MeasuredMetric = g.Sum(x => x.MeasuredMetric),
        //                DisplayMetric = g.Sum(x => x.DisplayMetric),
        //                DisplayImperial = g.Sum(x => x.DisplayImperial)
        //            }).ToList();
        //        }
        //        decimal totalsmeric = 0;
        //        decimal totalsimperial = 0;
        //        foreach (NavigationXML xml in finalNavigationXMLs)
        //        {
        //            if (xml.DisplayImperial > 700)
        //            {
        //                xml.YardMiles = Math.Round(xml.DisplayImperial / 1760, 1) + " miles ";
        //            }
        //            else
        //            {
        //                xml.YardMiles = xml.DisplayImperial + " yards ";
        //            }
        //            if (xml.DisplayImperial > 0)
        //            {
        //                totalsimperial = totalsimperial + xml.DisplayImperial;
        //            }

        //            if (xml.DisplayMetric > 0)
        //            {
        //                totalsmeric = totalsmeric + xml.DisplayMetric;
        //            }
        //        }

        //        TotalDistanceMetric = totalsmeric;

        //        TotalDistanceImperial = totalsimperial;


        //        affRoadStruct.IsBroken = false;

        //        List<AffectedRoadsStructureRouteSubPartListPosition> arsrslpList = new List<AffectedRoadsStructureRouteSubPartListPosition>();
        //        AffectedRoadsStructureRouteSubPartListPosition arsrslp = new AffectedRoadsStructureRouteSubPartListPosition();
        //        AffectedRoadsSubPartStructure arsps = new AffectedRoadsSubPartStructure();
        //        List<AffectedRoadsSubPartStructurePathListPosition> arspslpList = new List<AffectedRoadsSubPartStructurePathListPosition>();
        //        AffectedRoadsSubPartStructurePathListPosition arspslp = new AffectedRoadsSubPartStructurePathListPosition();
        //        List<AffectedRoadsPathStructureRoadTraversalListPosition> arpsrtlpList = new List<AffectedRoadsPathStructureRoadTraversalListPosition>();

        //        foreach (NavigationXML xml in finalNavigationXMLs)
        //        {
        //            if (xml.Instruction != string.Empty || (xml.DisplayMetric != 0 && xml.YardMiles != string.Empty))
        //            {
        //                AffectedRoadsPathStructureRoadTraversalListPosition arpsrtlp1 = new AffectedRoadsPathStructureRoadTraversalListPosition();

        //                AffectedRoadStructure ars1 = new AffectedRoadStructure();
        //                ars1.IsMyResponsibility = false;
        //                ars1.IsStartOfMyResponsibility = false;

        //                List<AffectedRoadStructure> affrdconsStruList = new List<AffectedRoadStructure>();
        //                NotificationXSD.RoadIdentificationStructure ris = new NotificationXSD.RoadIdentificationStructure();
        //                if (xml.Instruction != string.Empty)
        //                {
        //                    ris.Name = xml.Instruction;
        //                    ars1.RoadIdentity = ris;
        //                }

        //                MetricImperialDistancePairStructure metimpstru = new MetricImperialDistancePairStructure();
        //                metimpstru.Metric = Convert.ToString(xml.DisplayMetric);
        //                metimpstru.Imperial = xml.YardMiles;
        //                if (xml.DisplayMetric != 0 && xml.YardMiles != string.Empty)
        //                {
        //                    ars1.Distance = metimpstru;
        //                }

        //                arpsrtlp1.RoadTraversal = ars1;

        //                arpsrtlpList.Add(arpsrtlp1);
        //            }
        //        }
        //        arspslp.Path = arpsrtlpList.ToArray();
        //        arspslpList.Add(arspslp);
        //        arsps.PathListPosition = arspslpList.ToArray();
        //        arsrslp.RouteSubPart = arsps;
        //        arsrslpList.Add(arsrslp);
        //        affRoadStruct.RouteSubPartListPosition = arsrslpList.ToArray();
        //    }
        //    return affRoadStruct;
        //}
        #endregion

        #region getRouteDetails
        /// <summary>
        /// generate routedescription node detail by processing routedescription xml
        /// </summary>
        /// <param name="NotificationID">notification id</param>
        /// <returns></returns>
        //public static string getRouteDetails(long NotificationID)
        //{
        //    string messg = "OutboundDAO/getRouteDetails?NotificationID=" + NotificationID;
        //    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

        //    string errormsg;
        //    string result = string.Empty;
        //    Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);
        //    //For RM#4311 Change
        //    string[] separators = { "Split" };
        //    string resultPart = string.Empty;
        //    string FinalResult = string.Empty;
        //    string[] versionSplit = new string[] { };
        //    StringBuilder sb = new StringBuilder();
        //    //End

        //    DrivingInstructionModel DrivingInstructionInfo = GetRouteDescription(NotificationID);

        //    XSLTPath xSLTPath = new XSLTPath();
        //    string path = Path.Combine(xSLTPath.xsltRouteDetail);

        //    result = STP.Common.General.XsltTransformer.Trafo(DrivingInstructionInfo.DrivingInstructions, path, out errormsg);

        //    //For RM#4311
        //    if (result != null && result.Length > 0)
        //    {
        //        versionSplit = result.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        //    }


        //    foreach (string part in versionSplit)
        //    {
        //        resultPart = string.Empty;

        //        if (part != null && part.Length > 0 && part.IndexOf("Alternative end # 1") != -1 && part.IndexOf("Alternative end # 2") == -1)
        //        {
        //            resultPart = part.Replace("arrive at destination", "<u><b>Alternative end # 2:</b></u> arrive at destination");
        //            sb.Append(resultPart);
        //        }
        //        else
        //        {
        //            resultPart = part;
        //            sb.Append(resultPart);
        //        }
        //    }

        //    if (sb.ToString() != null || sb.ToString() != "")
        //    {
        //        FinalResult = sb.ToString();
        //    }

        //    FinalResult = FinalResult.Replace("<u>", "##us##");
        //    FinalResult = FinalResult.Replace("</u>", "##ue##");

        //    FinalResult = FinalResult.Replace("<b>", "#bst#");
        //    FinalResult = FinalResult.Replace("</b>", "#be#");

        //    FinalResult = _htmlRegex.Replace(FinalResult, string.Empty);

        //    FinalResult = FinalResult.Replace("Start of new part", "");

        //    return FinalResult;
        //}
        #endregion

        #region GetRouteDescription

        /// <summary>
        /// get route description xml detail from the database
        /// </summary>
        /// <param name="NotificationID">notification id</param>
        /// <returns></returns>
        //public static DrivingInstructionModel GetRouteDescription(long NotificationID)
        //{
        //    DrivingInstructionModel structuresDetail = new DrivingInstructionModel();

        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
        //        structuresDetail,
        //        UserSchema.Portal + ".GET_OUTBOUND_DRIVINGINS_DETAIL",
        //        parameter =>
        //        {
        //            parameter.AddWithValue("P_NOTIFICATION_ID", NotificationID, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //        },
        //           (records, instance) =>
        //           {
        //               instance.DrivingInstructions = records.GetByteArrayOrNull("ROUTE_DESCRIPTION");
        //           }
        //    );
        //    return structuresDetail;
        //}
        #endregion

        #region getRouteDetailsImperial
        //public static string getRouteDetailsImperial(long NotificationID)
        //{
        //    string messg = "OutboundDAO/getRouteDetailsImperial?NotificationID=" + NotificationID;
        //    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

        //    string errormsg;
        //    string result = string.Empty;
        //    Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);
        //    //For RM#4311 Change
        //    string[] separators = { "Split" };
        //    string resultPart = string.Empty;
        //    string FinalResult = string.Empty;
        //    string[] versionSplit = new string[] { };
        //    StringBuilder sb = new StringBuilder();
        //    //End

        //    DrivingInstructionModel DrivingInstructionInfo = GetRouteDescription(NotificationID);

        //    XSLTPath xSLTPath = new XSLTPath();
        //    string path = Path.Combine(xSLTPath.xsltRouteDetailsImperial);

        //    result = STP.Common.General.XsltTransformer.Trafo(DrivingInstructionInfo.DrivingInstructions, path, out errormsg);

        //    //For RM#4311
        //    if (result != null && result.Length > 0)
        //    {
        //        versionSplit = result.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        //    }


        //    foreach (string part in versionSplit)
        //    {
        //        resultPart = string.Empty;

        //        if (part != null && part.Length > 0 && part.IndexOf("Alternative end # 1") != -1 && part.IndexOf("Alternative end # 2") == -1)
        //        {
        //            resultPart = part.Replace("arrive at destination", "<u><b>Alternative end # 2:</b></u> arrive at destination");
        //            sb.Append(resultPart);
        //        }
        //        else
        //        {
        //            resultPart = part;
        //            sb.Append(resultPart);
        //        }
        //    }

        //    if (sb.ToString() != null || sb.ToString() != "")
        //    {
        //        FinalResult = sb.ToString();
        //    }

        //    FinalResult = FinalResult.Replace("<u>", "##us##");
        //    FinalResult = FinalResult.Replace("</u>", "##ue##");

        //    FinalResult = FinalResult.Replace("<b>", "#bst#");
        //    FinalResult = FinalResult.Replace("</b>", "#be#");

        //    FinalResult = _htmlRegex.Replace(FinalResult, string.Empty);

        //    FinalResult = FinalResult.Replace("Start of new part", "");

        //    return FinalResult;
        //}
        #endregion

        #region GetSimplifiedRoutePointEnd
        /// <summary>
        /// get journey end date for notification document
        /// </summary>
        /// <param name="NotificationID">notification id</param>
        /// <param name="routepartid">route part id</param>
        /// <returns></returns>
        //public static NotificationXSD.SimplifiedRoutePointStructure GetSimplifiedRoutePointEnd(long NotificationID, long routepartid)
        //{
        //    string messg = "OutboundDAO/GetSimplifiedRoutePointEnd?NotificationID=" + NotificationID + ", routepartid=" + routepartid;
        //    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

        //    NotificationXSD.SimplifiedRoutePointStructure srps = new NotificationXSD.SimplifiedRoutePointStructure();

        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
        //        srps,
        //        UserSchema.Portal + ".GET_OUTBOUND_ENDPOINT",
        //        parameter =>
        //        {
        //            parameter.AddWithValue("P_NOTIFICATION_ID", NotificationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("P_ROUTEPART_ID", routepartid, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //        },
        //         (records, instance) =>
        //         {
        //             instance.Description = records.GetStringOrDefault("DESCR");
        //             instance.GridRef = Convert.ToString(records.GetDecimalOrDefault("road_point_geometry.SDO_POINT.X")) + ',' + Convert.ToString(records.GetDecimalOrDefault("road_point_geometry.SDO_POINT.Y"));
        //         }
        //        );

        //    return srps;
        //}
        #endregion

        #region GetSimplifiedRoutePointStart
        /// <summary>
        /// get journey start point for notification document
        /// </summary>
        /// <param name="NotificationID">notification id</param>
        /// <param name="routepartid">route part id</param>
        /// <returns></returns>
        //public static NotificationXSD.SimplifiedRoutePointStructure GetSimplifiedRoutePointStart(long NotificationID, long routepartid)
        //{
        //    string messg = "OutboundDAO/GetSimplifiedRoutePointStart?NotificationID=" + NotificationID + ", routepartid=" + routepartid;
        //    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

        //    NotificationXSD.SimplifiedRoutePointStructure srps = new NotificationXSD.SimplifiedRoutePointStructure();

        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
        //        srps,
        //        UserSchema.Portal + ".GET_OUTBOUND_STARTPOINT",
        //        parameter =>
        //        {
        //            parameter.AddWithValue("P_NOTIFICATION_ID", NotificationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("P_ROUTEPART_ID", routepartid, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //        },
        //         (records, instance) =>
        //         {
        //             instance.Description = records.GetStringOrDefault("DESCR");
        //             instance.GridRef = Convert.ToString(records.GetDecimalOrDefault("road_point_geometry.SDO_POINT.X")) + ',' + Convert.ToString(records.GetDecimalOrDefault("road_point_geometry.SDO_POINT.Y"));
        //         }
        //        );

        //    return srps;
        //}
        #endregion

        #region GetStructureDataDetails
        /// <summary>
        /// process structure node detail and generate structure node detail based on notification document format
        /// </summary>
        /// <param name="NotificationID">notification id</param>
        /// <returns></returns>
        //public static AffectedStructuresStructure GetStructureDataDetails(long NotificationID, string RoutePartName)
        //{
        //    string messg = "OutboundDAO/GetStructureDataDetails?NotificationID=" + NotificationID + ", RoutePartName=" + RoutePartName;
        //    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

        //    AffectedStructuresStructure affStructures = new AffectedStructuresStructure();


        //    try
        //    {

        //        StructuresModel struInfo = GetStructuresXML(NotificationID);

        //        string recipientXMLInformation = string.Empty;

        //        Byte[] affectedPartiesArray = struInfo.AffectedStructures;

        //        if (affectedPartiesArray != null)
        //        {
        //            XmlDocument xmlDoc = new XmlDocument();
        //            try
        //            {
        //                recipientXMLInformation = Encoding.UTF8.GetString(affectedPartiesArray, 0, affectedPartiesArray.Length);

        //                xmlDoc.LoadXml(recipientXMLInformation);
        //            }
        //            catch
        //            {
        //                //Some data is stored in gzip format, so we need to unzip then load it.
        //                byte[] outBoundDecryptString = STP.Common.General.XsltTransformer.Trafo(affectedPartiesArray);

        //                recipientXMLInformation = Encoding.UTF8.GetString(outBoundDecryptString, 0, outBoundDecryptString.Length);

        //                xmlDoc.LoadXml(recipientXMLInformation);
        //            }

        //            List<StructuresModel> struList = new List<StructuresModel>();
        //            XNamespace StructNS = "http://www.esdal.com/schemas/core/routeanalysis";

        //            NotificationXSD.StructureSuitabilityStructure appraisalRecord = new NotificationXSD.StructureSuitabilityStructure();

        //            XDocument StructXDocument = XDocument.Parse(xmlDoc.OuterXml); // Converting from XMLDocument to XDocument

        //            XDocument SelectedStructures = new XDocument(new XElement("AddedElements", from p in StructXDocument.Root.Elements(StructNS + "AnalysedStructuresPart")
        //                                                                                       where p.Element(StructNS + "Name").Value == RoutePartName
        //                                                                                       select p.Elements(StructNS + "Structure"))); // Fetching structures in XDocument object

        //            XmlDocument StructXMLDocument = new XmlDocument();
        //            string StructModelString = string.Empty;
        //            StructModelString = SelectedStructures.ToString();
        //            StructXMLDocument.LoadXml(StructModelString); // Converting XDocument to XMLDocument

        //            XmlNodeList parentNode = StructXMLDocument.GetElementsByTagName("Structure");

        //            List<NotificationXSD.StructureSectionSuitabilityStructure> individaulSection = new List<NotificationXSD.StructureSectionSuitabilityStructure>();
        //            NotificationXSD.StructureSectionSuitabilityStructure structureSection = new NotificationXSD.StructureSectionSuitabilityStructure();

        //            List<NotificationXSD.SuitabilityResultStructure> resultStructureList = new List<NotificationXSD.SuitabilityResultStructure>();
        //            NotificationXSD.SuitabilityResultStructure resultStructure = new NotificationXSD.SuitabilityResultStructure();

        //            AffectedStructureStructure structure = new AffectedStructureStructure();

        //            List<AffectedStructureStructure> affectedstructList = new List<AffectedStructureStructure>();

        //            foreach (XmlElement childrenNode in parentNode)
        //            {

        //                bool isConstraint = false;
        //                bool isInFailedDelegation = false;

        //                bool isMyResponsibility = false;
        //                bool isRetainNotification = false;

        //                int structureSectionId = 0;
        //                int orgId = 0;
        //                string traversalType = string.Empty;

        //                string structureCode = string.Empty;
        //                string structureName = string.Empty;

        //                if ((childrenNode != null) && childrenNode.HasAttribute("StructureSectionId"))
        //                {
        //                    structureSectionId = Convert.ToInt32(childrenNode.Attributes["StructureSectionId"].InnerText);
        //                }

        //                if ((childrenNode != null) && childrenNode.HasAttribute("IsConstrained"))
        //                {
        //                    isConstraint = Convert.ToBoolean(childrenNode.Attributes["IsConstrained"].InnerText);
        //                }

        //                if ((childrenNode != null) && childrenNode.HasAttribute("IsInFailedDelegation"))
        //                {
        //                    isInFailedDelegation = Convert.ToBoolean(childrenNode.Attributes["IsInFailedDelegation"].InnerText);
        //                }

        //                if ((childrenNode != null) && childrenNode.HasAttribute("IsMyResponsibility"))
        //                {
        //                    isMyResponsibility = Convert.ToBoolean(childrenNode.Attributes["IsMyResponsibility"].InnerText);
        //                }

        //                if ((childrenNode != null) && childrenNode.HasAttribute("IsRetainNotificationOnly"))
        //                {
        //                    isRetainNotification = Convert.ToBoolean(childrenNode.Attributes["IsRetainNotificationOnly"].InnerText);
        //                }

        //                if ((childrenNode != null) && childrenNode.HasAttribute("TraversalType"))
        //                {
        //                    traversalType = Convert.ToString(childrenNode.Attributes["TraversalType"].InnerText);
        //                }
        //                if (childrenNode != null)
        //                {
        //                    foreach (XmlElement xmlElement in childrenNode)
        //                    {

        //                        if (xmlElement.Name == "ESRN")
        //                        {
        //                            structureCode = xmlElement.InnerText;
        //                        }

        //                        if (xmlElement.Name == "Name")
        //                        {
        //                            structureName = xmlElement.InnerText;
        //                        }

        //                        if (xmlElement.Name == "StructureResponsibility")
        //                        {
        //                            foreach (XmlElement xmlElement1 in xmlElement.ChildNodes)
        //                            {
        //                                if ((xmlElement1 != null) && xmlElement1.HasAttribute("OrganisationId"))
        //                                {
        //                                    orgId = Convert.ToInt32(xmlElement1.Attributes["OrganisationId"].InnerText);
        //                                }

        //                            }
        //                        }

        //                        if (xmlElement.Name == "Appraisal")
        //                        {

        //                            string suitability = string.Empty;
        //                            string OrganisationName = string.Empty;

        //                            string sectionSuitability = string.Empty;
        //                            string sectionDescription = string.Empty;

        //                            string childSectionSuitability = string.Empty;
        //                            string childSectionTestClass = string.Empty;
        //                            string childSectionTestIdentity = string.Empty;
        //                            string childSectionResultDetails = string.Empty;

        //                            foreach (XmlElement xmlElement1 in xmlElement.ChildNodes)
        //                            {
        //                                if (xmlElement1.Name == "Suitability")
        //                                {
        //                                    suitability = xmlElement1.InnerText;
        //                                }

        //                                if (xmlElement1.Name == "Organisation")
        //                                {
        //                                    OrganisationName = xmlElement1.InnerText;
        //                                }

        //                                if (xmlElement1.Name == "IndividualSectionSuitability")
        //                                {
        //                                    foreach (XmlElement individualSectionElement in xmlElement1.ChildNodes)
        //                                    {
        //                                        if (individualSectionElement.Name == "Suitability")
        //                                        {
        //                                            sectionSuitability = individualSectionElement.InnerText;
        //                                        }

        //                                        if (individualSectionElement.Name == "SectionDescription")
        //                                        {
        //                                            sectionDescription = individualSectionElement.InnerText;
        //                                        }

        //                                        foreach (XmlElement childIndividualSection in individualSectionElement.ChildNodes)
        //                                        {
        //                                            if (childIndividualSection.Name == "Suitability")
        //                                            {
        //                                                childSectionSuitability = childIndividualSection.InnerText;
        //                                            }

        //                                            if (childIndividualSection.InnerText == "TestClass")
        //                                            {
        //                                                childSectionTestClass = childIndividualSection.InnerText;
        //                                            }

        //                                            if (childIndividualSection.InnerText == "TestIdentity")
        //                                            {
        //                                                childSectionTestIdentity = childIndividualSection.InnerText;
        //                                            }

        //                                            if (childIndividualSection.InnerText == "ResultDetails")
        //                                            {
        //                                                childSectionResultDetails = childIndividualSection.InnerText;
        //                                            }

        //                                            resultStructure = new NotificationXSD.SuitabilityResultStructure
        //                                            {
        //                                                Suitability = childSectionSuitability == "suitable" ? NotificationXSD.SuitabilityType.suitable : childSectionSuitability == "marginal" ? NotificationXSD.SuitabilityType.marginal : childSectionSuitability == "unsuitable" ? NotificationXSD.SuitabilityType.unsuitable : NotificationXSD.SuitabilityType.unknown,
        //                                                ResultDetails = childSectionResultDetails,
        //                                                TestClass = childSectionTestClass == "dimensional constraint" ? NotificationXSD.SuitabilityTestClassType.dimensionalconstraint : NotificationXSD.SuitabilityTestClassType.ICA,
        //                                                TestIdentity = childSectionTestIdentity
        //                                            };

        //                                            resultStructureList.Add(resultStructure);
        //                                        }

        //                                        structureSection = new NotificationXSD.StructureSectionSuitabilityStructure
        //                                        {
        //                                            Suitability = sectionSuitability == "suitable" ? NotificationXSD.SuitabilityType.suitable : sectionSuitability == "marginal" ? NotificationXSD.SuitabilityType.marginal : sectionSuitability == "unsuitable" ? NotificationXSD.SuitabilityType.unsuitable : NotificationXSD.SuitabilityType.unknown,
        //                                            SectionDescription = sectionDescription,
        //                                            IndividualResult = resultStructureList.ToArray()
        //                                        };

        //                                        individaulSection.Add(structureSection);
        //                                    }
        //                                }
        //                            }

        //                            appraisalRecord = new NotificationXSD.StructureSuitabilityStructure()
        //                            {
        //                                Suitability = suitability == "suitable" ? NotificationXSD.SuitabilityType.suitable : suitability == "marginal" ? NotificationXSD.SuitabilityType.marginal : suitability == "unsuitable" ? NotificationXSD.SuitabilityType.unsuitable : NotificationXSD.SuitabilityType.unknown,
        //                                Organisation = OrganisationName,
        //                                OrganisationId = orgId,
        //                                IndividualSectionSuitability = individaulSection.ToArray()
        //                            };
        //                        }
        //                    }
        //                }


        //                structure = new AffectedStructureStructure
        //                {
        //                    StructureSectionId = structureSectionId,
        //                    IsConstrained = isConstraint,
        //                    IsInFailedDelegation = isInFailedDelegation,
        //                    IsRetainNotificationOnly = isRetainNotification,
        //                    IsMyResponsibility = isMyResponsibility,

        //                    TraversalType = traversalType.ToLower() == "overbridge" ? NotificationXSD.StructureTraversalType.overbridge : traversalType.ToLower() == "underbridge" ? NotificationXSD.StructureTraversalType.underbridge : traversalType.ToLower() == "levelcrossing" ? NotificationXSD.StructureTraversalType.levelcrossing : NotificationXSD.StructureTraversalType.archedoverbridge,

        //                    ESRN = structureCode,
        //                    Name = structureName,
        //                    Appraisal = appraisalRecord,

        //                };

        //                affectedstructList.Add(structure);
        //            }

        //            affStructures.Structure = affectedstructList.ToArray();
        //        }
        //        else
        //        {
        //            affStructures.Structure = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, string.Format("OutBoundDAO/GetStructureDataDetails, Exception: {0}", ex));
        //    }
        //    return affStructures;
        //}
        #endregion

        #region GetStructuresXML

        /// <summary>
        /// get structure xml detail from the database
        /// </summary>
        /// <param name="NotificationID"></param>
        /// <returns></returns>
        //public static StructuresModel GetStructuresXML(long NotificationID)
        //{
        //    StructuresModel structuresDetail = new StructuresModel();

        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
        //        structuresDetail,
        //        UserSchema.Portal + ".GET_OUTBOUND_DRIVINGINS_DETAIL",
        //        parameter =>
        //        {
        //            parameter.AddWithValue("P_NOTIFICATION_ID", NotificationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //        },
        //           (records, instance) =>
        //           {
        //               instance.AffectedStructures = records.GetByteArrayOrNull("AFFECTED_STRUCTURES");
        //           }
        //    );
        //    return structuresDetail;
        //}
        #endregion

        #region GetVehicleComponentAxles

        /// <summary>
        /// get vehicle component detail for notification document
        /// </summary>
        /// <param name="NotificationID">notification id</param>
        /// <returns></returns>
        //public static List<VehComponentAxles> GetVehicleComponentAxles(long NotificationID, long VehicleID)
        //{
        //    string messg = "OutboundDAO/GetVehicleComponentAxles?NotificationID=" + NotificationID + ", VehicleID=" + VehicleID;
        //    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

        //    List<VehComponentAxles> componentAxleList = new List<VehComponentAxles>();

        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
        //        componentAxleList,
        //          UserSchema.Portal + ".GET_OUTBOUND_AXLE_CONFIG",
        //        parameter =>
        //        {
        //            parameter.AddWithValue("P_NOTIFICATION_ID", NotificationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("P_VehicleId", VehicleID, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //        },
        //            (records, instance) =>
        //            {
        //                instance.AxleCount = records.GetInt16OrDefault("axle_count");
        //                instance.ComponentId = records.GetLongOrDefault("Component_id");
        //                instance.NextAxleDistNoti = records.GetDecimalOrDefault("NEXT_AXLE_DIST");
        //                instance.TyreSize = records.GetStringOrDefault("tyre_size");

        //                instance.Weight = records.GetFloatOrDefault("weight");
        //                instance.WheelCount = records.GetInt16OrDefault("wheel_count");
        //                instance.AxleNumber = records.GetInt16OrDefault("AXLE_NO");
        //                instance.WheelSpacingList = records.GetStringOrDefault("wheel_spacing_list");
        //                instance.AxleSpacingToFollowing = Convert.ToDouble(records.GetDecimalOrDefault("AXLE_SPACE_TO_FOLLOW"));
        //            }
        //    );
        //    return componentAxleList;
        //}
        #endregion

        #region GetVehicleComponentAxlesByComponent
        //public static List<VehComponentAxles> GetVehicleComponentAxlesByComponent(long componentID, long VehicleID, string userSchema)
        //{
        //    List<VehComponentAxles> componentAxleList = new List<VehComponentAxles>();

        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
        //        componentAxleList,
        //          userSchema + ".GET_AXLE_CONFIG_NONSEMI",
        //        parameter =>
        //        {
        //            parameter.AddWithValue("P_ComponentId", componentID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("P_VehicleId", VehicleID, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //        },
        //            (records, instance) =>
        //            {
        //                instance.AxleCount = records.GetInt16OrDefault("axle_count");
        //                instance.ComponentId = records.GetLongOrDefault("Component_Id");
        //                instance.NextAxleDist = records.GetFloatOrDefault("NEXT_AXLE_DIST");
        //                instance.TyreSize = records.GetStringOrDefault("tyre_size");
        //                instance.Weight = records.GetFloatOrDefault("weight");
        //                instance.WheelCount = records.GetInt16OrDefault("wheel_count");
        //                instance.AxleNumber = records.GetInt16OrDefault("AXLE_NO");
        //                instance.WheelSpacingList = records.GetStringOrDefault("wheel_spacing_list");
        //            }
        //    );
        //    return componentAxleList;
        //}
        #endregion

        #region GetVehicleConfigurationDetails
        //public static List<VehicleConfigList> GetVehicleConfigurationDetails(int vhclID, string userSchema)
        //{
        //    string messg = "OutboundDAO/GetVehicleConfigurationDetails?vhclID=" + vhclID + ", userSchema=" + userSchema;
        //    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

        //    List<VehicleConfigList> listVehclRegObj = new List<VehicleConfigList>();
        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
        //        listVehclRegObj,
        //        userSchema + ".GET_VEHICLE_CONFIG_POSN",
        //        parameter =>
        //        {
        //            parameter.AddWithValue("p_VHCL_ID", vhclID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //        },
        //        (records, result) =>
        //        {
        //            result.VehicleId = records.GetLongOrDefault("VEHICLE_ID");
        //            result.ComponentId = records.GetLongOrDefault("COMPONENT_ID");
        //            result.ComponentSubType = records.GetStringOrDefault("SUB_TYPE");
        //            result.ComponentType = records.GetStringOrDefault("TYPE");
        //            result.LatPosn = records.GetInt16OrDefault("LONG_POSN");
        //            result.LongPosn = records.GetInt16OrDefault("Lat_Posn");

        //            result.Length = (records["len"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("len");
        //            result.Width = (records["width"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("width");

        //            result.RedGroundClearance = (records["RED_Ground_Clearance"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("RED_Ground_Clearance");
        //            result.GroundClearance = (records["Ground_Clearance"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("Ground_Clearance");

        //            result.RedHeight = (records["Red_Height"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("Red_Height");

        //            try
        //            {
        //                result.WheelBase = (records["wheelbase"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("wheelbase");
        //            }
        //            catch
        //            {

        //                try
        //                {
        //                    result.WheelBase = (records["wheelbase"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("wheelbase");
        //                }
        //                catch (Exception ex)
        //                {
        //                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
        //                }
        //            }
        //            result.OutsideTrack = (records["Outside_Track"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("Outside_Track");

        //            result.RightOverhang = (records["Right_Overhang"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("Right_Overhang");
        //            result.LeftOverhang = (records["Left_Overhang"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("Left_Overhang");

        //            result.FrontOverhang = (records["Front_Overhang"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("Front_Overhang");
        //            result.RearOverhang = (records["Rear_Overhang"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("Rear_Overhang");

        //            result.IsSteerableAtRear = (records["Is_Steerable_At_Rear"]).ToString() == string.Empty ? 0 : records.GetDecimalOrDefault("Is_Steerable_At_Rear");
        //            result.SpaceToFollowing = (records["Space_To_Following"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("Space_To_Following");
        //            result.VehicleDescription = records.GetStringOrDefault("Vehicle_Desc");

        //            result.GrossWeight = (records["Gross_Weight"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("Gross_Weight");

        //            result.RigidLength = (records["RIGID_LEN"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("RIGID_LEN");
        //        }
        //    );
        //    return listVehclRegObj;
        //}
        #endregion

        #region  GetVehicleMaxHeight
        //public static List<OutBoundDoc> GetVehicleMaxHeight(long RoutePart_Id)
        //{
        //    string messg = "OutboundDAO/GetVehicleMaxHeight?RoutePart_Id=" + RoutePart_Id;
        //    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

        //    List<OutBoundDoc> outBoundDoc = new List<OutBoundDoc>();
        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
        //        outBoundDoc,
        //        UserSchema.Portal + ".GET_VEHICLE_MAX_HEIGHT",
        //        parameter =>
        //        {
        //            parameter.AddWithValue("p_ROUTEPART_ID", RoutePart_Id, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //        },
        //        (records, result) =>
        //        {
        //            try
        //            {
        //                result.MaximumHeight = (records["max_height"]).ToString() == string.Empty ? 0 : Convert.ToDouble(records.GetDecimalOrDefault("max_height"));
        //            }
        //            catch
        //            {

        //                try
        //                {
        //                    result.MaximumHeight = (records["max_height"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("max_height");
        //                }
        //                catch (Exception ex)
        //                {
        //                    Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured ,Exception:" + ex);
        //                }
        //            }
        //        }
        //    );
        //    return outBoundDoc;

        //}
        #endregion

        #region GetVehicleSummaryConfigurationIdentity
        /// <summary>
        /// get vehicle configuration identity detail for notification document
        /// </summary>
        /// <param name="NotificationID">notification id</param>
        /// <returns></returns>
        //public static NotificationXSD.VehicleSummaryStructureConfigurationIdentityListPosition[] GetVehicleSummaryConfigurationIdentity(long NotificationID)
        //{
        //    string messg = "OutboundDAO/GetVehicleSummaryConfigurationIdentity?NotificationID=" + NotificationID;
        //    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));
        //    List<NotificationXSD.VehicleSummaryStructureConfigurationIdentityListPosition> vsscilpList = new List<NotificationXSD.VehicleSummaryStructureConfigurationIdentityListPosition>();


        //    try
        //    {
        //        NotificationXSD.VehicleSummaryStructure vssList = new NotificationXSD.VehicleSummaryStructure();

        //        SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
        //            vsscilpList,
        //            UserSchema.Portal + ".GET_CONFIGURATION_IDENTITY",
        //            parameter =>
        //            {
        //                parameter.AddWithValue("P_NOTIFICATION_ID", NotificationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //                parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //            },
        //             (records, instance) =>
        //             {
        //                 NotificationXSD.SummaryVehicleIdentificationStructure svis = new NotificationXSD.SummaryVehicleIdentificationStructure();
        //                 svis.PlateNo = records.GetStringOrDefault("LICENSE_PLATE");
        //                 svis.FleetNumber = records.GetStringOrDefault("FLEET_NO");
        //                 instance.ConfigurationIdentity = svis;
        //             }
        //            );


        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - GetVehicleSummaryConfigurationIdentity,Exception:" + ex);
        //    }
        //    return vsscilpList.ToArray();
        //}
        #endregion

        #region NullableOrNotForDecimal
        /// Get Nullable data for decimal
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        //public static decimal? NullableOrNotForDecimal(double? data)
        //{
        //    if (data == null || data == 0)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return Convert.ToDecimal(data);
        //    }
        //}
        #endregion

        #region GetAxleWeightListPosition
        /// <summary>
        /// get vehicle axle weight list  detail for notification document
        /// </summary>
        /// <param name="vehicleComponentAxlesList">vehicle ComponentAxles List</param>
        /// <returns></returns>
        //public static NotificationXSD.SummaryAxleStructureAxleWeightListPosition[] GetAxleWeightListPosition(List<VehComponentAxles> vehicleComponentAxlesList)
        //{
        //    List<NotificationXSD.SummaryAxleStructureAxleWeightListPosition> sasawlpList = new List<NotificationXSD.SummaryAxleStructureAxleWeightListPosition>();

        //    List<long> componentList = vehicleComponentAxlesList.Select(x => x.ComponentId).Distinct().ToList();

        //    foreach (long component in componentList)
        //    {
        //        List<VehComponentAxles> componentWiseAxleList = (from apca in vehicleComponentAxlesList
        //                                                         where apca.ComponentId == component
        //                                                         orderby apca.AxleNumber ascending
        //                                                         select apca).ToList();

        //        List<float> weightList = componentWiseAxleList.Select(x => x.Weight).ToList();

        //        int count = weightList.Count;
        //        int localCount = 0;

        //        float oldDummyweight = 0;
        //        int oldcountaAxles = 0;

        //        foreach (float weight in weightList)
        //        {
        //            localCount = localCount + 1;

        //            if (oldcountaAxles == 0)
        //            {
        //                oldDummyweight = weight;

        //                oldcountaAxles = oldcountaAxles + 1;
        //            }
        //            else
        //            {
        //                if (weight == oldDummyweight)
        //                {
        //                    oldcountaAxles = oldcountaAxles + 1;
        //                }
        //                else
        //                {
        //                    NotificationXSD.SummaryAxleStructureAxleWeightListPosition sasawlp = new NotificationXSD.SummaryAxleStructureAxleWeightListPosition();
        //                    NotificationXSD.AxleWeightSummaryStructure awss = new NotificationXSD.AxleWeightSummaryStructure();

        //                    awss.Value = Convert.ToString(oldDummyweight);
        //                    awss.AxleCount = Convert.ToString(oldcountaAxles);

        //                    sasawlp.AxleWeight = awss;

        //                    sasawlpList.Add(sasawlp);

        //                    oldcountaAxles = 1;
        //                    oldDummyweight = weight;
        //                }
        //            }

        //            if (localCount == count)
        //            {
        //                NotificationXSD.SummaryAxleStructureAxleWeightListPosition sasawlp = new NotificationXSD.SummaryAxleStructureAxleWeightListPosition();
        //                NotificationXSD.AxleWeightSummaryStructure awss = new NotificationXSD.AxleWeightSummaryStructure();

        //                awss.Value = Convert.ToString(oldDummyweight);
        //                awss.AxleCount = Convert.ToString(oldcountaAxles);

        //                sasawlp.AxleWeight = awss;

        //                sasawlpList.Add(sasawlp);
        //            }
        //        }


        //    }

        //    if (sasawlpList.ToArray().Length == 0)
        //    {
        //        NotificationXSD.SummaryAxleStructureAxleWeightListPosition sasawlp = new NotificationXSD.SummaryAxleStructureAxleWeightListPosition();
        //        NotificationXSD.AxleWeightSummaryStructure awss = new NotificationXSD.AxleWeightSummaryStructure();
        //        awss.Value = "0";
        //        sasawlp.AxleWeight = awss;
        //        sasawlpList.Add(sasawlp);
        //    }

        //    return sasawlpList.ToArray();
        //}
        #endregion

        #region GetWheelsPerAxleListPosition
        /// <summary>
        /// get vehicle wheel per axle detail for notification document
        /// </summary>
        /// <param name="vehicleComponentAxlesList">vehicle ComponentAxles List</param>
        /// <returns></returns>
        //public static NotificationXSD.SummaryAxleStructureWheelsPerAxleListPosition[] GetWheelsPerAxleListPosition(List<VehComponentAxles> vehicleComponentAxlesList)
        //{
        //    List<NotificationXSD.SummaryAxleStructureWheelsPerAxleListPosition> saswpalpList = new List<NotificationXSD.SummaryAxleStructureWheelsPerAxleListPosition>();

        //    List<long> componentList = vehicleComponentAxlesList.Select(x => x.ComponentId).Distinct().ToList();

        //    foreach (long component in componentList)
        //    {
        //        List<VehComponentAxles> componentWiseAxleList = (from apca in vehicleComponentAxlesList
        //                                                         where apca.ComponentId == component
        //                                                         orderby apca.AxleNumber ascending
        //                                                         select apca).ToList();

        //        List<short> weightList = componentWiseAxleList.Select(x => x.WheelCount).ToList();

        //        int count = weightList.Count;
        //        int localCount = 0;

        //        short oldDummyweight = 0;
        //        int oldcountaAxles = 0;

        //        foreach (short weight in weightList)
        //        {
        //            localCount = localCount + 1;

        //            if (oldcountaAxles == 0)
        //            {
        //                oldDummyweight = weight;

        //                oldcountaAxles = oldcountaAxles + 1;
        //            }
        //            else
        //            {
        //                if (weight == oldDummyweight)
        //                {
        //                    oldcountaAxles = oldcountaAxles + 1;
        //                }
        //                else
        //                {
        //                    NotificationXSD.SummaryAxleStructureWheelsPerAxleListPosition saswpalp = new NotificationXSD.SummaryAxleStructureWheelsPerAxleListPosition();
        //                    NotificationXSD.WheelsPerAxleSummaryStructure wpass = new NotificationXSD.WheelsPerAxleSummaryStructure();

        //                    wpass.Value = Convert.ToString(oldDummyweight);
        //                    wpass.AxleCount = Convert.ToString(oldcountaAxles);

        //                    saswpalp.WheelsPerAxle = wpass;

        //                    saswpalpList.Add(saswpalp);

        //                    oldcountaAxles = 1;
        //                    oldDummyweight = weight;
        //                }
        //            }

        //            if (localCount == count)
        //            {
        //                NotificationXSD.SummaryAxleStructureWheelsPerAxleListPosition saswpalp = new NotificationXSD.SummaryAxleStructureWheelsPerAxleListPosition();
        //                NotificationXSD.WheelsPerAxleSummaryStructure wpass = new NotificationXSD.WheelsPerAxleSummaryStructure();

        //                wpass.Value = Convert.ToString(oldDummyweight);
        //                wpass.AxleCount = Convert.ToString(oldcountaAxles);

        //                saswpalp.WheelsPerAxle = wpass;

        //                saswpalpList.Add(saswpalp);
        //            }
        //        }
        //    }

        //    if (saswpalpList.ToArray().Length == 0)
        //    {
        //        NotificationXSD.SummaryAxleStructureWheelsPerAxleListPosition saswpalp = new NotificationXSD.SummaryAxleStructureWheelsPerAxleListPosition();
        //        NotificationXSD.WheelsPerAxleSummaryStructure wpass = new NotificationXSD.WheelsPerAxleSummaryStructure();
        //        wpass.Value = "0";
        //        saswpalp.WheelsPerAxle = wpass;
        //        saswpalpList.Add(saswpalp);
        //    }

        //    return saswpalpList.ToArray();
        //}
        #endregion

        #region GetAxleSpacingListPositionAxleSpacing
        //public static NotificationXSD.SummaryAxleStructureAxleSpacingListPosition[] GetAxleSpacingListPositionAxleSpacing(List<VehComponentAxles> vehicleComponentAxlesList)
        //{
        //    List<NotificationXSD.SummaryAxleStructureAxleSpacingListPosition> sasaslpList = new List<NotificationXSD.SummaryAxleStructureAxleSpacingListPosition>();

        //    List<long> componentList = vehicleComponentAxlesList.Select(x => x.ComponentId).Distinct().ToList();

        //    foreach (long component in componentList)
        //    {
        //        List<VehComponentAxles> componentWiseAxleList = (from apca in vehicleComponentAxlesList
        //                                                         where apca.ComponentId == component
        //                                                         orderby apca.AxleNumber ascending
        //                                                         select apca).ToList();

        //        List<decimal> axleSpacingList = componentWiseAxleList.Select(x => x.NextAxleDistNoti).ToList();

        //        int count = axleSpacingList.Count;
        //        int localCount = 0;

        //        decimal oldDummyweight = 0;
        //        int oldcountaAxles = 0;

        //        foreach (decimal axleSpacing in axleSpacingList)
        //        {
        //            localCount = localCount + 1;

        //            if (axleSpacing != 0)
        //            {
        //                if (oldcountaAxles == 0)
        //                {
        //                    oldDummyweight = axleSpacing;

        //                    oldcountaAxles = oldcountaAxles + 1;
        //                }
        //                else
        //                {
        //                    if (axleSpacing == oldDummyweight)
        //                    {
        //                        oldcountaAxles = oldcountaAxles + 1;
        //                    }
        //                    else
        //                    {
        //                        NotificationXSD.SummaryAxleStructureAxleSpacingListPosition sasaslp = new NotificationXSD.SummaryAxleStructureAxleSpacingListPosition();
        //                        NotificationXSD.AxleSpacingSummaryStructure ass = new NotificationXSD.AxleSpacingSummaryStructure();

        //                        ass.Value = oldDummyweight;
        //                        ass.AxleCount = Convert.ToString(oldcountaAxles);
        //                        sasaslp.AxleSpacing = ass;
        //                        sasaslpList.Add(sasaslp);

        //                        oldcountaAxles = 1;
        //                        oldDummyweight = axleSpacing;
        //                    }
        //                }
        //            }

        //            if (localCount == count)
        //            {
        //                NotificationXSD.SummaryAxleStructureAxleSpacingListPosition sasaslp = new NotificationXSD.SummaryAxleStructureAxleSpacingListPosition();
        //                NotificationXSD.AxleSpacingSummaryStructure ass = new NotificationXSD.AxleSpacingSummaryStructure();

        //                ass.Value = oldDummyweight;
        //                ass.AxleCount = Convert.ToString(oldcountaAxles);
        //                sasaslp.AxleSpacing = ass;
        //                sasaslpList.Add(sasaslp);
        //            }
        //        }
        //    }

        //    if (sasaslpList.ToArray().Length == 0)
        //    {
        //        NotificationXSD.SummaryAxleStructureAxleSpacingListPosition sasaslp = new NotificationXSD.SummaryAxleStructureAxleSpacingListPosition();
        //        NotificationXSD.AxleSpacingSummaryStructure ass = new NotificationXSD.AxleSpacingSummaryStructure();
        //        ass.Value = 0;
        //        sasaslp.AxleSpacing = ass;
        //        sasaslpList.Add(sasaslp);
        //    }

        //    return sasaslpList.ToArray();
        //}
        #endregion

        #region GetAxleSpacingToFollowListPositionAxleSpacing
        // Added RM#4386
        //public static SummaryAxleStructureAxleSpacingToFollowListPosition[] GetAxleSpacingToFollowListPositionAxleSpacing(List<VehComponentAxles> vehicleComponentAxlesList, double FirstComponentAxleSpaceToFollow)
        //{
        //    List<SummaryAxleStructureAxleSpacingToFollowListPosition> sasaslpList = new List<SummaryAxleStructureAxleSpacingToFollowListPosition>();

        //    if (FirstComponentAxleSpaceToFollow == 0)// RM#4386 Condition to show max axle value when the vehicle is manually added
        //    {
        //        SummaryAxleStructureAxleSpacingToFollowListPosition sasaslp = new SummaryAxleStructureAxleSpacingToFollowListPosition();
        //        AxleSpacingToFollowSummaryStructure ass = new AxleSpacingToFollowSummaryStructure();

        //        if (vehicleComponentAxlesList.ToArray().Length == 0)
        //        {
        //            ass.Value = 0;
        //            sasaslp.AxleSpacingToFollow = ass;
        //            sasaslpList.Add(sasaslp);
        //        }
        //        else
        //        {
        //            ass.Value = vehicleComponentAxlesList.Max(x => x.NextAxleDistNoti);
        //            sasaslp.AxleSpacingToFollow = ass;
        //            sasaslpList.Add(sasaslp);
        //        }

        //    }
        //    else
        //    {

        //        List<long> componentList = vehicleComponentAxlesList.Select(x => x.ComponentId).Distinct().ToList();

        //        foreach (long component in componentList)
        //        {
        //            List<VehComponentAxles> componentWiseAxleList = (from apca in vehicleComponentAxlesList
        //                                                             where apca.ComponentId == component
        //                                                             orderby apca.AxleNumber ascending
        //                                                             select apca).ToList();

        //            List<decimal> axleSpacingList = componentWiseAxleList.Select(x => Convert.ToDecimal(x.AxleSpacingToFollowing)).ToList();

        //            int count = axleSpacingList.Count;
        //            int localCount = 0;

        //            decimal oldDummyweight = 0;
        //            int oldcountaAxles = 0;

        //            foreach (decimal axleSpacing in axleSpacingList)
        //            {
        //                localCount = localCount + 1;

        //                if (axleSpacing != 0)
        //                {
        //                    if (oldcountaAxles == 0)
        //                    {
        //                        oldDummyweight = axleSpacing;

        //                        oldcountaAxles = oldcountaAxles + 1;
        //                    }
        //                    else
        //                    {
        //                        if (axleSpacing == oldDummyweight)
        //                        {
        //                            oldcountaAxles = oldcountaAxles + 1;
        //                        }
        //                        else
        //                        {
        //                            SummaryAxleStructureAxleSpacingToFollowListPosition sasaslp = new SummaryAxleStructureAxleSpacingToFollowListPosition();
        //                            AxleSpacingToFollowSummaryStructure ass = new AxleSpacingToFollowSummaryStructure();

        //                            ass.Value = oldDummyweight;
        //                            ass.AxleCount = Convert.ToString(oldcountaAxles);
        //                            sasaslp.AxleSpacingToFollow = ass;
        //                            sasaslpList.Add(sasaslp);

        //                            oldcountaAxles = 1;
        //                            oldDummyweight = axleSpacing;
        //                        }
        //                    }
        //                }

        //                if (localCount == count)
        //                {
        //                    SummaryAxleStructureAxleSpacingToFollowListPosition sasaslp = new SummaryAxleStructureAxleSpacingToFollowListPosition();
        //                    AxleSpacingToFollowSummaryStructure ass = new AxleSpacingToFollowSummaryStructure();

        //                    if (oldDummyweight > 0)
        //                    {
        //                        ass.Value = oldDummyweight;
        //                        ass.AxleCount = Convert.ToString(oldcountaAxles);
        //                        sasaslp.AxleSpacingToFollow = ass;
        //                        sasaslpList.Add(sasaslp);
        //                    }
        //                }
        //            }
        //        }

        //        if (sasaslpList.ToArray().Length == 0)
        //        {
        //            SummaryAxleStructureAxleSpacingToFollowListPosition sasaslp = new SummaryAxleStructureAxleSpacingToFollowListPosition();
        //            AxleSpacingToFollowSummaryStructure ass = new AxleSpacingToFollowSummaryStructure();
        //            ass.Value = 0;
        //            sasaslp.AxleSpacingToFollow = ass;
        //            sasaslpList.Add(sasaslp);
        //        }
        //    }

        //    return sasaslpList.ToArray();
        //}
        #endregion

        #region GetAxleSpacingListPosition
        /// <summary>
        /// get vehicle axle spacing detail for notification document
        /// </summary>
        /// <param name="vehicleComponentAxlesList">vehicle ComponentAxles List</param>
        /// <returns></returns>
        //public static NotificationXSD.SummaryAxleStructureAxleSpacingListPosition[] GetAxleSpacingListPosition(List<VehComponentAxles> vehicleComponentAxlesList)
        //{
        //    List<NotificationXSD.SummaryAxleStructureAxleSpacingListPosition> sasaslpList = new List<NotificationXSD.SummaryAxleStructureAxleSpacingListPosition>();

        //    List<long> componentList = vehicleComponentAxlesList.Select(x => x.ComponentId).Distinct().ToList();

        //    foreach (long component in componentList)
        //    {
        //        List<VehComponentAxles> componentWiseAxleList = (from apca in vehicleComponentAxlesList
        //                                                         where apca.ComponentId == component
        //                                                         select apca).ToList();


        //        List<float> axleSpacingList = componentWiseAxleList.Select(x => x.NextAxleDist).ToList();

        //        int count = axleSpacingList.Count;
        //        int localCount = 0;

        //        float oldDummyweight = 0;
        //        int oldcountaAxles = 0;

        //        foreach (float axleSpacing in axleSpacingList)
        //        {
        //            localCount = localCount + 1;

        //            if (axleSpacing != 0)
        //            {
        //                if (oldcountaAxles == 0)
        //                {
        //                    oldDummyweight = axleSpacing;

        //                    oldcountaAxles = oldcountaAxles + 1;
        //                }
        //                else
        //                {
        //                    if (axleSpacing == oldDummyweight)
        //                    {
        //                        oldcountaAxles = oldcountaAxles + 1;
        //                    }
        //                    else
        //                    {
        //                        NotificationXSD.SummaryAxleStructureAxleSpacingListPosition sasaslp = new NotificationXSD.SummaryAxleStructureAxleSpacingListPosition();
        //                        NotificationXSD.AxleSpacingSummaryStructure ass = new NotificationXSD.AxleSpacingSummaryStructure();

        //                        ass.Value = Convert.ToDecimal(oldDummyweight);
        //                        ass.AxleCount = Convert.ToString(oldcountaAxles);
        //                        sasaslp.AxleSpacing = ass;
        //                        sasaslpList.Add(sasaslp);

        //                        oldcountaAxles = 1;
        //                        oldDummyweight = axleSpacing;
        //                    }
        //                }
        //            }

        //            if (localCount == count)
        //            {
        //                NotificationXSD.SummaryAxleStructureAxleSpacingListPosition sasaslp = new NotificationXSD.SummaryAxleStructureAxleSpacingListPosition();
        //                NotificationXSD.AxleSpacingSummaryStructure ass = new NotificationXSD.AxleSpacingSummaryStructure();

        //                ass.Value = Convert.ToDecimal(oldDummyweight);
        //                ass.AxleCount = Convert.ToString(oldcountaAxles);
        //                sasaslp.AxleSpacing = ass;
        //                sasaslpList.Add(sasaslp);
        //            }
        //        }
        //    }

        //    if (sasaslpList.ToArray().Length == 0)
        //    {
        //        NotificationXSD.SummaryAxleStructureAxleSpacingListPosition sasaslp = new NotificationXSD.SummaryAxleStructureAxleSpacingListPosition();
        //        NotificationXSD.AxleSpacingSummaryStructure ass = new NotificationXSD.AxleSpacingSummaryStructure();
        //        ass.Value = 0;
        //        sasaslp.AxleSpacing = ass;
        //        sasaslpList.Add(sasaslp);
        //    }

        //    return sasaslpList.ToArray();
        //}
        #endregion

        #region GetTyreSizeListPosition
        /// <summary>
        /// get vehicle tyre size detail for notification document
        /// </summary>
        /// <param name="vehicleComponentAxlesList">vehicle ComponentAxles List</param>
        /// <returns></returns>
        //public static NotificationXSD.SummaryAxleStructureTyreSizeListPosition[] GetTyreSizeListPosition(List<VehComponentAxles> vehicleComponentAxlesList)
        //{
        //    List<NotificationXSD.SummaryAxleStructureTyreSizeListPosition> sastslpList = new List<NotificationXSD.SummaryAxleStructureTyreSizeListPosition>();

        //    List<long> componentList = vehicleComponentAxlesList.Select(x => x.ComponentId).Distinct().ToList();

        //    foreach (long component in componentList)
        //    {
        //        List<VehComponentAxles> componentWiseAxleList = (from apca in vehicleComponentAxlesList
        //                                                         where apca.ComponentId == component
        //                                                         orderby apca.AxleNumber ascending
        //                                                         select apca).ToList();

        //        List<string> tyreSizeList = componentWiseAxleList.Select(x => x.TyreSize).ToList();

        //        int count = tyreSizeList.Count;
        //        int localCount = 0;

        //        string oldDummyweight = "";
        //        int oldcountaAxles = 0;

        //        foreach (string tyreSize in tyreSizeList)
        //        {
        //            localCount = localCount + 1;

        //            if (oldcountaAxles == 0)
        //            {
        //                oldDummyweight = tyreSize;

        //                oldcountaAxles = oldcountaAxles + 1;
        //            }
        //            else
        //            {
        //                if (tyreSize == oldDummyweight)
        //                {
        //                    oldcountaAxles = oldcountaAxles + 1;
        //                }
        //                else
        //                {
        //                    NotificationXSD.SummaryAxleStructureTyreSizeListPosition sastslp = new NotificationXSD.SummaryAxleStructureTyreSizeListPosition();
        //                    NotificationXSD.TyreSizeSummaryStructure tzss = new NotificationXSD.TyreSizeSummaryStructure();

        //                    tzss.Value = oldDummyweight;
        //                    tzss.AxleCount = Convert.ToString(oldcountaAxles);
        //                    sastslp.TyreSize = tzss;
        //                    sastslpList.Add(sastslp);

        //                    oldcountaAxles = 1;
        //                    oldDummyweight = tyreSize;
        //                }
        //            }

        //            if (localCount == count)
        //            {
        //                NotificationXSD.SummaryAxleStructureTyreSizeListPosition sastslp = new NotificationXSD.SummaryAxleStructureTyreSizeListPosition();
        //                NotificationXSD.TyreSizeSummaryStructure tzss = new NotificationXSD.TyreSizeSummaryStructure();

        //                tzss.Value = oldDummyweight;
        //                tzss.AxleCount = Convert.ToString(oldcountaAxles);
        //                sastslp.TyreSize = tzss;
        //                sastslpList.Add(sastslp);
        //            }
        //        }
        //    }

        //    return sastslpList.ToArray();
        //}
        #endregion

        #region GetWheelSpacingListPosition
        /// <summary>
        /// get wheel spacing detail for for notification document
        /// </summary>
        /// <param name="vehicleComponentAxlesList">vehicle ComponentAxles List</param>
        /// <returns></returns>
        //public static NotificationXSD.SummaryAxleStructureWheelSpacingListPosition[] GetWheelSpacingListPosition(List<VehComponentAxles> vehicleComponentAxlesList)
        //{
        //    List<NotificationXSD.SummaryAxleStructureWheelSpacingListPosition> saswslp = new List<NotificationXSD.SummaryAxleStructureWheelSpacingListPosition>();

        //    List<long> componentList = vehicleComponentAxlesList.Select(x => x.ComponentId).Distinct().ToList();

        //    foreach (long component in componentList)
        //    {
        //        List<VehComponentAxles> componentWiseAxleList = (from apca in vehicleComponentAxlesList
        //                                                         where apca.ComponentId == component
        //                                                         orderby apca.AxleNumber ascending
        //                                                         select apca).ToList();

        //        List<string> wheelSpacingList = componentWiseAxleList.Select(x => x.WheelSpacingList).ToList();

        //        int count = wheelSpacingList.Count;
        //        int localCount = 0;

        //        string oldDummyweight = "";
        //        int oldcountaAxles = 0;

        //        foreach (string tyreSize in wheelSpacingList)
        //        {
        //            localCount = localCount + 1;

        //            if (oldcountaAxles == 0)
        //            {
        //                oldDummyweight = tyreSize;

        //                oldcountaAxles = oldcountaAxles + 1;
        //            }
        //            else
        //            {
        //                if (tyreSize == oldDummyweight)
        //                {
        //                    oldcountaAxles = oldcountaAxles + 1;
        //                }
        //                else
        //                {
        //                    NotificationXSD.SummaryAxleStructureWheelSpacingListPosition saswsplp = new NotificationXSD.SummaryAxleStructureWheelSpacingListPosition();
        //                    NotificationXSD.WheelSpacingSummaryStructure wsss = new NotificationXSD.WheelSpacingSummaryStructure();

        //                    wsss.Value = oldDummyweight;
        //                    wsss.AxleCount = Convert.ToString(oldcountaAxles);
        //                    saswsplp.WheelSpacing = wsss;
        //                    saswslp.Add(saswsplp);

        //                    oldcountaAxles = 1;
        //                    oldDummyweight = tyreSize;
        //                }
        //            }

        //            if (localCount == count)
        //            {
        //                NotificationXSD.SummaryAxleStructureWheelSpacingListPosition saswsplp = new NotificationXSD.SummaryAxleStructureWheelSpacingListPosition();
        //                NotificationXSD.WheelSpacingSummaryStructure wsss = new NotificationXSD.WheelSpacingSummaryStructure();

        //                wsss.Value = oldDummyweight;
        //                wsss.AxleCount = Convert.ToString(oldcountaAxles);
        //                saswsplp.WheelSpacing = wsss;
        //                saswslp.Add(saswsplp);
        //            }
        //        }
        //    }

        //    return saswslp.ToArray();
        //}
        #endregion

        #region GetVehicleComponentSubTypes
        /// <summary>
        /// get actual vehicle component sub type value from the enum value for notification document
        /// </summary>
        /// <param name="type">vehicle component type</param>
        /// <returns></returns>
        //public static NotificationXSD.VehicleComponentSubType GetVehicleComponentSubTypes(String type)
        //{
        //    NotificationXSD.VehicleComponentSubType vehtype = new NotificationXSD.VehicleComponentSubType();
        //    if (type == "ballast tractor")
        //    {
        //        vehtype = NotificationXSD.VehicleComponentSubType.ballasttractor;
        //    }
        //    else if (type == "conventional tractor")
        //    {
        //        vehtype = NotificationXSD.VehicleComponentSubType.conventionaltractor;
        //    }
        //    else if (type == "other tractor")
        //    {
        //        vehtype = NotificationXSD.VehicleComponentSubType.othertractor;
        //    }
        //    else if (type == "semi trailer")
        //    {
        //        vehtype = NotificationXSD.VehicleComponentSubType.semitrailer;
        //    }
        //    else if (type == "semi low loader")
        //    {
        //        vehtype = NotificationXSD.VehicleComponentSubType.semilowloader;
        //    }
        //    else if (type == "trombone trailer")
        //    {
        //        vehtype = NotificationXSD.VehicleComponentSubType.trombonetrailer;
        //    }
        //    else if (type == "other semi trailer")
        //    {
        //        vehtype = NotificationXSD.VehicleComponentSubType.othersemitrailer;
        //    }
        //    else if (type == "drawbar trailer")
        //    {
        //        vehtype = NotificationXSD.VehicleComponentSubType.drawbartrailer;
        //    }
        //    else if (type == "other drawbar trailer")
        //    {
        //        vehtype = NotificationXSD.VehicleComponentSubType.otherdrawbartrailer;
        //    }
        //    else if (type == "twin bogies")
        //    {
        //        vehtype = NotificationXSD.VehicleComponentSubType.twinbogies;
        //    }
        //    else if (type == "tracked vehicle")
        //    {
        //        vehtype = NotificationXSD.VehicleComponentSubType.trackedvehicle;
        //    }
        //    else if (type == "rigid vehicle")
        //    {
        //        vehtype = NotificationXSD.VehicleComponentSubType.rigidvehicle;
        //    }
        //    else if (type == "girder set")
        //    {
        //        vehtype = NotificationXSD.VehicleComponentSubType.girderset;
        //    }
        //    else if (type == "wheeled load")
        //    {
        //        vehtype = NotificationXSD.VehicleComponentSubType.wheeledload;
        //    }
        //    else if (type == "recovery vehicle")
        //    {
        //        vehtype = NotificationXSD.VehicleComponentSubType.recoveryvehicle;
        //    }
        //    else if (type == "recovered vehicle")
        //    {
        //        vehtype = NotificationXSD.VehicleComponentSubType.recoveredvehicle;
        //    }
        //    else if (type == "mobile crane")
        //    {
        //        vehtype = NotificationXSD.VehicleComponentSubType.mobilecrane;
        //    }
        //    else if (type == "engineering plant")
        //    {
        //        vehtype = NotificationXSD.VehicleComponentSubType.engineeringplant;
        //    }
        //    return vehtype;
        //}
        #endregion

        #region GetVehicleComponentSubTypesNonSemiVehicles
        /// <summary>
        /// Get Vehicle component subtype for Non Semi Vehicles
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        //public static NotificationXSD.VehicleComponentType GetVehicleComponentSubTypesNonSemiVehicles(String type)
        //{
        //    NotificationXSD.VehicleComponentType vehtype = new NotificationXSD.VehicleComponentType();

        //    if (type == "ballast tractor")
        //    {
        //        vehtype = NotificationXSD.VehicleComponentType.ballasttractor;
        //    }
        //    else if (type == "conventional tractor")
        //    {
        //        vehtype = NotificationXSD.VehicleComponentType.conventionaltractor;
        //    }
        //    else if (type == "semi trailer")
        //    {
        //        vehtype = NotificationXSD.VehicleComponentType.semitrailer;
        //    }
        //    else if (type == "drawbar trailer")
        //    {
        //        vehtype = NotificationXSD.VehicleComponentType.drawbartrailer;
        //    }
        //    else if (type == "rigid vehicle")
        //    {
        //        vehtype = NotificationXSD.VehicleComponentType.rigidvehicle;
        //    }
        //    else if (type == "spmt")
        //    {
        //        vehtype = NotificationXSD.VehicleComponentType.spmt;
        //    }

        //    else if (type == "other drawbar trailer")
        //    {
        //        vehtype = NotificationXSD.VehicleComponentType.trackedvehicle;
        //    }

        //    return vehtype;
        //}
        #endregion

        
        #region GetSpecialOrderNo
        /// <summary>
        /// get special order no based on esdal reference
        /// </summary>
        /// <param name="esDAlRefNo">esdal ref no</param>
        /// <returns></returns>
        //public static SignedOrderSummaryStructure GetSpecialOrderNo(string esDAlRefNo)
        //{
        //    string messg = "OutboundDAO/GetSpecialOrderNo?esDAlRefNo=" + esDAlRefNo;
        //    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));
        //    SignedOrderSummaryStructure srps = new SignedOrderSummaryStructure();
        //    try
        //    {

        //        SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
        //            srps,
        //            UserSchema.Portal + ".GET_SPECIAL_ORDERS",
        //            parameter =>
        //            {
        //                parameter.AddWithValue("p_ESDAL_REF_NUMBER", esDAlRefNo, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
        //                parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //            },
        //             (records, instance) =>
        //             {
        //                 instance.OrderNumber = records.GetStringOrDefault("order_no");
        //                 instance.SignedOn = records.GetDateTimeOrDefault("SIGNED_DATE");
        //                 instance.ExpiresOn = records.GetDateTimeOrDefault("EXPIRY_DATE");
        //             }
        //            );


        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - GetSpecialOrderNo,Exception:" + ex);

        //    }
        //    return srps;
        //}
        #endregion

        #region GetSODetail
        /// <summary>
        /// get special order detail
        /// </summary>
        /// <param name="notificationId">notification id</param>
        /// <returns></returns>
        //public static NotificationSOInformationStructure GetSODetail(string esdalRefNumber)
        //{
        //    string messg = "OutboundDAO/GetSODetail?esdalRefNumber=" + esdalRefNumber;
        //    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

        //    NotificationSOInformationStructure soInformation = new NotificationSOInformationStructure();
        //    PossiblyReplacingSignedOrderSummaryStructure[] prsosList = new PossiblyReplacingSignedOrderSummaryStructure[1];
        //    PossiblyReplacingSignedOrderSummaryStructure prsos = new PossiblyReplacingSignedOrderSummaryStructure();


        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
        //        soInformation,
        //        UserSchema.Portal + ".GET_SO_DETAILS",
        //        parameter =>
        //        {
        //            parameter.AddWithValue("P_ESDAL_REF_NUMBER", esdalRefNumber, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //        },
        //           (records, instance) =>
        //           {

        //               SignedOrderSummaryStructure soss = new SignedOrderSummaryStructure();

        //               soss.IsAmendmentOrder = records.GetDecimalOrDefault("IS_AMENDMENT_ORDER") == 1 ? true : false;
        //               soss.OrderNumber = records.GetStringOrDefault("ORDER_NO");
        //               soss.SignedOn = records.GetDateTimeOrDefault("SIGNED_DATE");
        //               soss.ExpiresOn = records.GetDateTimeOrDefault("EXPIRY_DATE");
        //               soss.SignedBy = records.GetStringOrDefault("SIGNATORY");
        //               soss.HAJobRefNumber = records.GetStringOrDefault("HA_JOB_FILE_REF");
        //               prsos.CurrentOrder = soss;
        //               prsosList[0] = prsos;
        //               instance.Summary = prsosList;

        //               instance.HAJobRefNumber = records.GetStringOrDefault("HA_JOB_FILE_REF");

        //               decimal status = records.GetDecimalOrDefault("STATE");
        //               instance.Status = SOStatusType.expired;
        //               if (status == 230001)
        //               {
        //                   instance.Status = SOStatusType.proposedrouteonly;
        //               }
        //               else if (status == 230002)
        //               {
        //                   instance.Status = SOStatusType.expired;
        //               }
        //               else if (status == 230003)
        //               {
        //                   instance.Status = SOStatusType.granted;
        //               }
        //               else if (status == 230004)
        //               {
        //                   instance.Status = SOStatusType.externaltoesdal;
        //               }
        //               else if (status == 230005)
        //               {
        //                   instance.Status = SOStatusType.reclearance;
        //               }
        //           }
        //          );

        //    if (soInformation.Summary == null)
        //    {
        //        return null;
        //    }
        //    else if (soInformation.Summary != null && soInformation.Summary[0].CurrentOrder.OrderNumber == string.Empty)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return soInformation;
        //    }
        //}
        #endregion

        #region GetRecipientContactStructure
        /// <summary>
        /// generate recipient detail based on processing of xml file
        /// </summary>
        /// <param name="NotificationID"></param>
        /// <returns></returns>
        //public static List<NotificationXSD.RecipientContactStructure> GetRecipientContactStructure(long NotificationID)
        //{
        //    string messg = "OutboundDAO/Get NotificationXSD.RecipientContactStructure?NotificationID=" + NotificationID;
        //    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(messg + "; Method started successfully"));

        //    List<NotificationXSD.RecipientContactStructure> rcsclplist = new List<NotificationXSD.RecipientContactStructure>();
        //    string contactInformationLog = string.Empty;
        //    try
        //    {


        //        ContactModel contactInfo = GetRecipientDetails(NotificationID);

        //        string recipientXMLInformation = string.Empty;

        //        Byte[] affectedPartiesArray = contactInfo.AffectedParties;

        //        XmlDocument xmlDoc = new XmlDocument();
        //        if (affectedPartiesArray != null)
        //        {
        //            try
        //            {
        //                recipientXMLInformation = Encoding.UTF8.GetString(affectedPartiesArray, 0, affectedPartiesArray.Length);

        //                xmlDoc.LoadXml(recipientXMLInformation);
        //            }
        //            catch
        //            {
        //                //Some data is stored in gzip format, so we need to unzip then load it.
        //                byte[] outBoundDecryptString = STP.Common.General.XsltTransformer.Trafo(affectedPartiesArray);

        //                recipientXMLInformation = Encoding.UTF8.GetString(outBoundDecryptString, 0, outBoundDecryptString.Length);

        //                xmlDoc.LoadXml(recipientXMLInformation);
        //            }
        //        }
        //        List<ContactModel> contactList = new List<ContactModel>();

        //        XmlNodeList parentNode = null;
        //        if (xmlDoc.GetElementsByTagName("AffectedParty").Item(0) == null)
        //        {
        //            parentNode = xmlDoc.GetElementsByTagName("movement:AffectedParty");
        //        }
        //        else
        //        {
        //            parentNode = xmlDoc.GetElementsByTagName("AffectedParty");
        //        }
        //        foreach (XmlElement childrenNode in parentNode)                                   //AffectedParty
        //        {
        //            bool isPolice = false;
        //            bool isRetainNotificationOnly = false;
        //            bool isHaulier = false;
        //            bool isRecepient = false;

        //            string reason = string.Empty;

        //            string exclusionOutCome = string.Empty;
        //            string exclude = string.Empty;

        //            if ((childrenNode != null) && childrenNode.HasAttribute("IsPolice"))
        //            {
        //                isPolice = Convert.ToBoolean(childrenNode.Attributes["IsPolice"].InnerText);
        //            }

        //            if ((childrenNode != null) && childrenNode.HasAttribute("Exclude"))
        //            {
        //                exclude = Convert.ToString(childrenNode.Attributes["Exclude"].InnerText);
        //            }
        //            else if ((childrenNode != null) && childrenNode.HasAttribute("contact:Exclude"))
        //            {
        //                exclude = Convert.ToString(childrenNode.Attributes["contact:Exclude"].InnerText);
        //            }

        //            if ((childrenNode != null) && childrenNode.HasAttribute("IsRetainedNotificationOnly"))
        //            {
        //                isRetainNotificationOnly = Convert.ToBoolean(childrenNode.Attributes["IsRetainedNotificationOnly"].InnerText);
        //            }

        //            if ((childrenNode != null) && childrenNode.HasAttribute("Reason"))
        //            {
        //                reason = Convert.ToString(childrenNode.Attributes["Reason"].InnerText);
        //            }
        //            else if ((childrenNode != null) && childrenNode.HasAttribute("contact:Reason"))
        //            {
        //                reason = Convert.ToString(childrenNode.Attributes["contact:Reason"].InnerText);
        //            }

        //            if ((childrenNode != null) && childrenNode.HasAttribute("ExclusionOutcome"))
        //            {
        //                exclusionOutCome = Convert.ToString(childrenNode.Attributes["ExclusionOutcome"].InnerText);
        //            }
        //            else if ((childrenNode != null) && childrenNode.HasAttribute("contact:ExclusionOutcome"))
        //            {
        //                exclusionOutCome = Convert.ToString(childrenNode.Attributes["contact:ExclusionOutcome"].InnerText);
        //            }

        //            if (childrenNode != null && childrenNode.HasAttribute("IsHaulier"))
        //            {
        //                isHaulier = Convert.ToBoolean(childrenNode.Attributes["IsHaulier"].InnerText);
        //            }

        //            if (childrenNode != null && childrenNode.HasAttribute("IsRecipient"))
        //            {
        //                isRecepient = Convert.ToBoolean(childrenNode.Attributes["IsRecipient"].InnerText);
        //            }
        //            if (childrenNode != null)
        //            {
        //                foreach (XmlElement xmlElement in childrenNode)                             //Contact
        //                {

        //                    int DelegationId = 0;
        //                    int DelegatorsContactId = 0;
        //                    int DelegatorsOrganisationId = 0;

        //                    bool RetainNotification = false;
        //                    bool WantsFailureAlert = false;
        //                    string DelegatorsOrganisationName = string.Empty;

        //                    if (xmlElement.Name == "OnBehalfOf")
        //                    {
        //                        if ((xmlElement != null) && xmlElement.HasAttribute("DelegationId"))
        //                        {
        //                            DelegationId = Convert.ToInt32(xmlElement.Attributes["DelegationId"].InnerText);
        //                        }
        //                        if ((xmlElement != null) && xmlElement.HasAttribute("DelegatorsContactId"))
        //                        {
        //                            DelegatorsContactId = Convert.ToInt32(xmlElement.Attributes["DelegatorsContactId"].InnerText);
        //                        }
        //                        if ((xmlElement != null) && xmlElement.HasAttribute("DelegatorsOrganisationId"))
        //                        {
        //                            DelegatorsOrganisationId = Convert.ToInt32(xmlElement.Attributes["DelegatorsOrganisationId"].InnerText);
        //                        }
        //                        if ((xmlElement != null) && xmlElement.HasAttribute("RetainNotification"))
        //                        {
        //                            RetainNotification = Convert.ToBoolean(xmlElement.Attributes["RetainNotification"].InnerText);
        //                        }
        //                        if ((xmlElement != null) && xmlElement.HasAttribute("WantsFailureAlert"))
        //                        {
        //                            WantsFailureAlert = Convert.ToBoolean(xmlElement.Attributes["WantsFailureAlert"].InnerText);
        //                        }
        //                        if (xmlElement["DelegatorsOrganisationName"] != null)
        //                        {
        //                            DelegatorsOrganisationName = xmlElement["DelegatorsOrganisationName"].InnerText;
        //                        }

        //                        if ((!string.IsNullOrEmpty(reason) && reason.ToLower() != "no longer affected") && (string.IsNullOrEmpty(exclusionOutCome) || exclusionOutCome.ToLower() != "no longer affected")
        //                                               && (string.IsNullOrEmpty(exclude) || exclude.ToLower() == "false"))
        //                        {
        //                            if (contactList.Count > 0)
        //                            {
        //                                contactList[contactList.Count - 1].DelegationId = DelegationId;
        //                                contactList[contactList.Count - 1].DelegatorsContactId = DelegatorsContactId;
        //                                contactList[contactList.Count - 1].DelegatorsOrganisationId = DelegatorsOrganisationId;
        //                                contactList[contactList.Count - 1].RetainNotification = RetainNotification;
        //                                contactList[contactList.Count - 1].WantsFailureAlert = WantsFailureAlert;
        //                                contactList[contactList.Count - 1].DelegatorsOrganisationName = DelegatorsOrganisationName;
        //                            }
        //                        }
        //                    }

        //                    foreach (XmlElement xmlElement1 in xmlElement.ChildNodes)               //Contact
        //                    {
        //                        foreach (XmlNode childNode in xmlElement1)                          //SimpleReference
        //                        {
        //                            if (childNode.Name == "SimpleReference")
        //                            {
        //                                int contactId = 0;
        //                                int orgId = 0;
        //                                string contactname = string.Empty;
        //                                string orgname = string.Empty;
        //                                XmlElement simpleReference = childNode as XmlElement;
        //                                if ((simpleReference != null) && simpleReference.HasAttribute("ContactId"))
        //                                {
        //                                    contactId = Convert.ToInt32(childNode.Attributes["ContactId"].InnerText);
        //                                }
        //                                if ((simpleReference != null) && simpleReference.HasAttribute("OrganisationId"))
        //                                {
        //                                    orgId = Convert.ToInt32(childNode.Attributes["OrganisationId"].InnerText);
        //                                }

        //                                if (childNode["FullName"] != null)
        //                                {
        //                                    contactname = childNode["FullName"].InnerText;
        //                                }
        //                                if (childNode["OrganisationName"] != null)
        //                                {
        //                                    orgname = childNode["OrganisationName"].InnerText;
        //                                }

        //                                ContactModel cntDetails = GetContactDetails(contactId);

        //                                if ((!string.IsNullOrEmpty(reason) && reason.ToLower() != "no longer affected") && (string.IsNullOrEmpty(exclusionOutCome) || exclusionOutCome.ToLower() != "no longer affected")
        //                                               && (string.IsNullOrEmpty(exclude) || exclude.ToLower() == "false"))
        //                                {
        //                                    contactInfo = new ContactModel()
        //                                    {
        //                                        IsRecipient = isRecepient,
        //                                        IsHaulier = isHaulier,
        //                                        ContactId = contactId,
        //                                        OrganisationId = orgId,
        //                                        Fax = cntDetails.Fax,
        //                                        Email = cntDetails.Email,
        //                                        ISPolice = isPolice,
        //                                        IsRetainedNotificationOnly = isRetainNotificationOnly,
        //                                        Reason = reason,
        //                                        FullName = contactname,
        //                                        Organisation = orgname,
        //                                    };

        //                                    contactList.Add(contactInfo);

        //                                    contactInformationLog = string.Empty;

        //                                    contactInformationLog = "OutboundDAO/Get NotificationXSD.RecipientContactStructure?IsRecipient=" + contactInfo.IsRecipient + ", IsHaulier=" + contactInfo.IsHaulier + ", ContactId=" + contactInfo.ContactId + ", OrganisationID=" + contactInfo.OrganisationId + ", Fax=" + contactInfo.Fax + ", Email=" + contactInfo.Email + ", ISPolice=" + contactInfo.ISPolice + ", IsRetainedNotificationOnly=" + contactInfo.IsRetainedNotificationOnly + ", Reason=" + contactInfo.Reason + ", FullName=" + contactInfo.FullName + ", Organisation=" + contactInfo.Organisation;
        //                                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(contactInformationLog + "; SimpleReference contact Info details"));
        //                                }
        //                            }
        //                            else if (childNode.Name.Contains("AdhocReference"))
        //                            {

        //                                string contactname = string.Empty;
        //                                string orgname = string.Empty;
        //                                string Email = string.Empty;
        //                                string Fax = string.Empty;

        //                                if (childNode["contact:FullName"] != null)
        //                                {
        //                                    contactname = childNode["contact:FullName"].InnerText;
        //                                }
        //                                else
        //                                {
        //                                    contactname = childNode["FullName"] != null ? childNode["FullName"].InnerText : null;
        //                                }

        //                                if (childNode["contact:EmailAddress"] != null)
        //                                {
        //                                    Email = childNode["contact:EmailAddress"].InnerText;
        //                                }
        //                                else
        //                                {
        //                                    Email = childNode["EmailAddress"] != null ? childNode["EmailAddress"].InnerText : null;
        //                                }

        //                                if (childNode["contact:OrganisationName"] != null)
        //                                {
        //                                    orgname = childNode["contact:OrganisationName"].InnerText;
        //                                }
        //                                else
        //                                {
        //                                    orgname = childNode["OrganisationName"] != null ? childNode["OrganisationName"].InnerText : null;
        //                                }
        //                                //here Fax is also fetched
        //                                if (childNode["contact:FaxNumber"] != null)
        //                                {
        //                                    Fax = childNode["contact:FaxNumber"].InnerText;
        //                                }
        //                                else
        //                                {
        //                                    Fax = childNode["FaxNumber"] != null ? childNode["FaxNumber"].InnerText : null;
        //                                }
        //                                if ((!string.IsNullOrEmpty(reason) && reason.ToLower() != "no longer affected") && (string.IsNullOrEmpty(exclusionOutCome) || exclusionOutCome.ToLower() != "no longer affected")
        //                                                   && (string.IsNullOrEmpty(exclude) || exclude.ToLower() == "false"))
        //                                {
        //                                    contactInfo = new ContactModel
        //                                    {

        //                                        IsRecipient = isRecepient,
        //                                        IsHaulier = isHaulier,
        //                                        Fax = Fax,
        //                                        Email = Email,
        //                                        ISPolice = isPolice,
        //                                        IsRetainedNotificationOnly = isRetainNotificationOnly,
        //                                        Reason = reason,
        //                                        FullName = contactname,
        //                                        Organisation = orgname,
        //                                    };

        //                                    contactList.Add(contactInfo);

        //                                    contactInformationLog = string.Empty;

        //                                    contactInformationLog = "ProposalDAO/Get NotificationXSD.RecipientContactStructure?IsRecipient=" + contactInfo.IsRecipient + ", IsHaulier=" + contactInfo.IsHaulier + ", Fax=" + contactInfo.Fax + ", Email=" + contactInfo.Email + ", ISPolice=" + contactInfo.ISPolice + ", IsRetainedNotificationOnly=" + contactInfo.IsRetainedNotificationOnly + ", Reason=" + contactInfo.Reason + ", FullName=" + contactInfo.FullName + ", Organisation=" + contactInfo.Organisation;
        //                                    Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format(contactInformationLog + "; Adhoc Reference contact Info details"));
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //        }

        //        NotificationXSD.RecipientContactStructure rcs1;
        //        foreach (ContactModel cont in contactList)
        //        {
        //            rcs1 = new NotificationXSD.RecipientContactStructure();
        //            rcs1.Reason = cont.Reason;
        //            rcs1.ContactId = cont.ContactId;
        //            rcs1.ContactIdSpecified = true;
        //            rcs1.IsHaulier = GetOrgTypeDetails(cont.OrganisationId);
        //            rcs1.IsPolice = cont.ISPolice;
        //            rcs1.IsRecipient = cont.IsRecipient;
        //            rcs1.IsRetainedNotificationOnly = cont.IsRetainedNotificationOnly;
        //            rcs1.OrganisationId = cont.OrganisationId;
        //            rcs1.OrganisationIdSpecified = true;

        //            rcs1.ContactName = cont.FullName;
        //            rcs1.OrganisationName = cont.Organisation;
        //            rcs1.Fax = cont.Fax;
        //            rcs1.Email = cont.Email;

        //            NotificationXSD.OnBehalfOfStructure onbehalfofstru = new NotificationXSD.OnBehalfOfStructure();

        //            onbehalfofstru.DelegationId = cont.DelegationId;
        //            onbehalfofstru.DelegationIdSpecified = true;
        //            onbehalfofstru.DelegatorsContactId = cont.DelegatorsContactId;
        //            onbehalfofstru.DelegatorsContactIdSpecified = true;
        //            onbehalfofstru.DelegatorsOrganisationId = cont.DelegatorsOrganisationId;
        //            onbehalfofstru.DelegatorsOrganisationIdSpecified = true;
        //            onbehalfofstru.DelegatorsOrganisationName = cont.DelegatorsOrganisationName;
        //            onbehalfofstru.RetainNotification = cont.RetainNotification;
        //            onbehalfofstru.WantsFailureAlert = cont.WantsFailureAlert;

        //            if (onbehalfofstru.DelegationId > 0 && onbehalfofstru.DelegatorsContactId > 0)
        //            {
        //                rcs1.OnbehalfOf = onbehalfofstru;
        //            }

        //            rcsclplist.Add(rcs1);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
        //    }
        //    return rcsclplist;
        //}
        #endregion

        #region GetRouteParts
        /// <summary>
        /// get route part node detail for notification document
        /// </summary>
        /// <param name="NotificationID">notification id</param>
        /// <param name="isHaulier">is haulier</param>
        /// <returns></returns>
        //public static List<RoutePartsStructureRoutePartListPosition> GetRouteParts(long NotificationID)
        //{
        //    bool isHaulier = false;
        //    long routepartid = 0;
        //    int legNumber = 1;
        //    bool ReturnFlag = false; //RM#3659
        //    int counter = 0; //RM#3659
        //    string EsdalRefNumber = "1#";

        //    List<OutBoundDoc> outBoundDoc = new List<OutBoundDoc>();
        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
        //       outBoundDoc,
        //       UserSchema.Portal + ".GET_MULTI_ROUTEVEHICLE_DETAILS",
        //       parameter =>
        //       {
        //           parameter.AddWithValue("P_NOTIFICATION_ID", NotificationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //           parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //       },
        //          (records, instance) =>
        //          {
        //              try
        //              {
        //                  instance.RoutePartId = records.GetLongOrDefault("route_part_id");
        //              }
        //              catch (Exception ex)
        //              {
        //                  Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
        //              }

        //              try
        //              {
        //                  instance.VehicleId = records.GetLongOrDefault("vehicle_id");
        //              }
        //              catch (Exception ex)
        //              {
        //                  Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
        //              }

        //              instance.VehicleDesc = records.GetStringOrDefault("vehicle_desc");

        //              try
        //              {
        //                  instance.Length = (records["len"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("len");
        //              }
        //              catch (Exception ex)
        //              {
        //                  Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
        //              }

        //              try
        //              {
        //                  instance.RearOverhang = (records["REAR_OVERHANG"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("REAR_OVERHANG");
        //              }
        //              catch (Exception ex)
        //              {
        //                  Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
        //              }

        //              try
        //              {
        //                  instance.RigidLength = (records["rigid_len"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("rigid_len");
        //              }
        //              catch (Exception ex)
        //              {
        //                  Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
        //              }

        //              try
        //              {
        //                  instance.FrontOverhang = (records["front_overhang"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("front_overhang");
        //              }
        //              catch (Exception ex)
        //              {
        //                  Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
        //              }

        //              try
        //              {
        //                  instance.Width = (records["width"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("width");
        //              }
        //              catch (Exception ex)
        //              {
        //                  Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
        //              }

        //              try
        //              {
        //                  instance.MaximumHeight = (records["max_height"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("max_height");
        //              }
        //              catch (Exception ex)
        //              {
        //                  Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
        //              }

        //              try
        //              {
        //                  instance.RedHeight = (records["Red_Height"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("Red_Height");
        //              }
        //              catch (Exception ex)
        //              {
        //                  Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
        //              }

        //              try
        //              {
        //                  instance.GrossWeight = (records["gross_weight"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("gross_weight");
        //              }
        //              catch (Exception ex)
        //              {
        //                  Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
        //              }

        //              try
        //              {
        //                  instance.MaximumAxleWeight = (records["MAX_AXLE_WEIGHT"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("MAX_AXLE_WEIGHT");
        //              }
        //              catch (Exception ex)
        //              {
        //                  Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
        //              }

        //              instance.PlannedContentRefNo = records.GetStringOrDefault("planned_content_ref_no");

        //              try
        //              {
        //                  instance.IsSteerableAtRear = (records["is_steerable_at_rear"]).ToString() == string.Empty ? 0 : records.GetDecimalOrDefault("is_steerable_at_rear");
        //              }
        //              catch (Exception ex)
        //              {
        //                  Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
        //              }

        //              try
        //              {
        //                  instance.WheelBase = (records["wheelbase"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("wheelbase");
        //              }
        //              catch (Exception ex)
        //              {
        //                  Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
        //              }

        //              try
        //              {
        //                  instance.GroundClearance = (records["ground_clearance"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("ground_clearance");
        //              }
        //              catch (Exception ex)
        //              {
        //                  Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
        //              }

        //              try
        //              {
        //                  instance.OutsideTrack = (records["outside_track"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("outside_track");
        //              }
        //              catch (Exception ex)
        //              {
        //                  Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
        //              }

        //              //=====================================================
        //              try
        //              {
        //                  instance.ComparisonId = (records["comparison_id"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("comparison_id");
        //              }
        //              catch
        //              {
        //                  try
        //                  {
        //                      instance.ComparisonId = (records["comparison_id"]).ToString() == string.Empty ? 0 : records.GetLongOrDefault("comparison_id");
        //                  }
        //                  catch (Exception ex)
        //                  {
        //                      Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
        //                  }
        //              }
        //              //======================================================

        //              try
        //              {
        //                  instance.RoutePartNo = (records["route_part_no"]).ToString() == string.Empty ? 0 : records.GetDecimalOrDefault("route_part_no");
        //              }
        //              catch (Exception ex)
        //              {
        //                  Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
        //              }


        //              instance.PartDescr = records.GetStringOrDefault("part_descr");

        //              try
        //              {
        //                  instance.TransportMode = (records["transport_mode"]).ToString() == string.Empty ? 0 : records.GetInt32OrDefault("transport_mode");
        //              }
        //              catch (Exception ex)
        //              {
        //                  Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
        //              }

        //              instance.PartName = records.GetStringOrDefault("PART_NAME");
        //              instance.Name = records.GetStringOrDefault("name");
        //              instance.ComponentType = records.GetStringOrDefault("component_type");
        //              instance.VehicleType = records.GetStringOrDefault("vehicle_type");
        //              instance.ComponentSubtype = records.GetStringOrDefault("COMPONENT_SUBTYPE");

        //              try
        //              {
        //                  instance.SpaceToFollowing = (records["SPACE_TO_FOLLOWING"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("SPACE_TO_FOLLOWING");
        //              }
        //              catch (Exception ex)
        //              {
        //                  Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
        //              }

        //              try
        //              {
        //                  instance.LeftOverhang = (records["LEFT_OVERHANG"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("LEFT_OVERHANG");
        //              }
        //              catch (Exception ex)
        //              {
        //                  Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
        //              }

        //              try
        //              {
        //                  instance.RightOverhang = (records["RIGHT_OVERHANG"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("RIGHT_OVERHANG");
        //              }
        //              catch (Exception ex)
        //              {
        //                  Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
        //              }

        //              try
        //              {
        //                  instance.RedGroundClearance = (records["RED_GROUND_CLEARANCE"]).ToString() == string.Empty ? 0 : records.GetDoubleOrDefault("RED_GROUND_CLEARANCE");
        //              }
        //              catch (Exception ex)
        //              {
        //                  Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Exception occured,Exception:" + ex);
        //              }
        //          }
        //   );

        //    List<RoutePartsStructureRoutePartListPosition> rpsrplp = new List<RoutePartsStructureRoutePartListPosition>();

        //    SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstanceList(
        //        rpsrplp,
        //        UserSchema.Portal + ".GET_VEHICLE_DETAILS",
        //        parameter =>
        //        {
        //            parameter.AddWithValue("P_NOTIFICATION_ID", NotificationID, OracleDbType.Int64, ParameterDirectionWrap.Input, 32767);
        //            parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //        },
        //           (records, instance) =>
        //           {
        //               PlannedRoutePartStructure2 prps = new PlannedRoutePartStructure2();
        //               string RoutePartName = string.Empty;
        //               prps.Id = Convert.ToInt32(records.GetLongOrDefault("route_part_id"));

        //               prps.LegNumber = Convert.ToString(legNumber);

        //               routepartid = records.GetLongOrDefault("route_part_id");

        //               List<OutBoundDoc> outBoundDocVehicle = new List<OutBoundDoc>();
        //               outBoundDocVehicle = GetVehicleMaxHeight(routepartid);

        //               prps.Name = records.GetStringOrDefault("PART_NAME");

        //               if (records.GetStringOrDefault("name") == "road")
        //               {
        //                   prps.Mode = NotificationXSD.ModeOfTransportType.road;
        //               }
        //               else if (records.GetStringOrDefault("name") == "air")
        //               {
        //                   prps.Mode = NotificationXSD.ModeOfTransportType.air;
        //               }
        //               else if (records.GetStringOrDefault("name") == "rail")
        //               {
        //                   prps.Mode = NotificationXSD.ModeOfTransportType.rail;
        //               }
        //               else if (records.GetStringOrDefault("name") == "sea")
        //               {
        //                   prps.Mode = NotificationXSD.ModeOfTransportType.sea;
        //               }

        //               PlannedRoadRoutePartStructure prp = new PlannedRoadRoutePartStructure();

        //               #region StartPointListPosition
        //               PlannedRoadRoutePartStructureStartPointListPosition[] prrpssplpList = new PlannedRoadRoutePartStructureStartPointListPosition[1];
        //               PlannedRoadRoutePartStructureStartPointListPosition prrpssplp = new PlannedRoadRoutePartStructureStartPointListPosition();

        //               prrpssplp.StartPoint = GetSimplifiedRoutePointStart(NotificationID, routepartid);
        //               prrpssplpList[0] = prrpssplp;
        //               prp.StartPointListPosition = prrpssplpList;
        //               #endregion
        //               #region EndPointListPosition
        //               PlannedRoadRoutePartStructureEndPointListPosition[] prrpseplpList = new PlannedRoadRoutePartStructureEndPointListPosition[1];
        //               PlannedRoadRoutePartStructureEndPointListPosition prrpseplp = new PlannedRoadRoutePartStructureEndPointListPosition();

        //               prrpseplp.EndPoint = GetSimplifiedRoutePointEnd(NotificationID, routepartid);
        //               prrpseplpList[0] = prrpseplp;
        //               prp.EndPointListPosition = prrpseplpList;
        //               #endregion

        //               int recordcount = 0;
        //               var outBoundDocVar = outBoundDoc.Where(r => r.RoutePartId.Equals(routepartid)).ToList();
        //               NotificationXSD.VehiclesSummaryStructure vss = new NotificationXSD.VehiclesSummaryStructure();
        //               outBoundDoc.RemoveAll(r => r.RoutePartId.Equals(routepartid));

        //               if ((EsdalRefNumber.Contains("#") && ReturnFlag) || (!ReturnFlag && counter == 0)) //RM#3659 If condition added to show 1 vehicle details for simplified noti when return route is selected
        //               {
        //                   if (ReturnFlag)
        //                   {
        //                       counter++;
        //                       ReturnFlag = false;
        //                   }

        //                   if (outBoundDocVar.Count > 0)
        //                   {
        //                       #region ConfigurationSummaryListPosition
        //                       NotificationXSD.VehiclesSummaryStructureConfigurationSummaryListPosition[] vscslpList = new NotificationXSD.VehiclesSummaryStructureConfigurationSummaryListPosition[outBoundDocVar.Count];
        //                       recordcount = 0;
        //                       foreach (var outbound in outBoundDocVar)
        //                       {
        //                           NotificationXSD.VehiclesSummaryStructureConfigurationSummaryListPosition vscslp = new NotificationXSD.VehiclesSummaryStructureConfigurationSummaryListPosition();
        //                           vscslp.ConfigurationSummary = outbound.VehicleDesc;
        //                           vscslpList[recordcount] = vscslp;
        //                           recordcount++;
        //                       }
        //                       vss.ConfigurationSummaryListPosition = vscslpList;
        //                       #endregion

        //                       #region OverallLengthListPosition
        //                       NotificationXSD.VehiclesSummaryStructureOverallLengthListPosition[] vssollpList = new NotificationXSD.VehiclesSummaryStructureOverallLengthListPosition[outBoundDocVar.Count];
        //                       recordcount = 0;
        //                       foreach (var outbound in outBoundDocVar)
        //                       {
        //                           NotificationXSD.VehiclesSummaryStructureOverallLengthListPosition vssollp = new NotificationXSD.VehiclesSummaryStructureOverallLengthListPosition();
        //                           NotificationXSD.SummaryLengthStructure sls = new NotificationXSD.SummaryLengthStructure();
        //                           sls.IncludingProjections = NullableOrNotForDecimal(outbound.Length);

        //                           sls.ExcludingProjections = sls.IncludingProjections - (NullableOrNotForDecimal(outbound.RearOverhang) + NullableOrNotForDecimal(outbound.FrontOverhang));

        //                           if (sls.ExcludingProjections != null)
        //                           {
        //                               sls.ExcludingProjectionsSpecified = true;
        //                           }
        //                           else
        //                           {
        //                               sls.ExcludingProjectionsSpecified = false;
        //                           }

        //                           vssollp.OverallLength = sls;
        //                           vssollpList[recordcount] = vssollp;
        //                           recordcount++;
        //                       }
        //                       vss.OverallLengthListPosition = vssollpList;
        //                       #endregion

        //                       #region RigidLengthListPosition
        //                       NotificationXSD.VehiclesSummaryStructureRigidLengthListPosition[] vssrllpList = new NotificationXSD.VehiclesSummaryStructureRigidLengthListPosition[outBoundDocVar.Count];
        //                       recordcount = 0;
        //                       foreach (var outbound in outBoundDocVar)
        //                       {

        //                           NotificationXSD.VehiclesSummaryStructureRigidLengthListPosition vssrllp = new NotificationXSD.VehiclesSummaryStructureRigidLengthListPosition();
        //                           vssrllp.RigidLength = NullableOrNotForDecimal(outbound.RigidLength);

        //                           if (vssrllp.RigidLength != null)
        //                           {
        //                               vssrllp.RigidLengthSpecified = true;
        //                           }
        //                           else
        //                           {
        //                               vssrllp.RigidLengthSpecified = false;
        //                           }

        //                           vssrllpList[recordcount] = vssrllp;
        //                           recordcount++;
        //                       }
        //                       vss.RigidLengthListPosition = vssrllpList;
        //                       #endregion

        //                       #region RearOverhangListPosition
        //                       NotificationXSD.VehiclesSummaryStructureRearOverhangListPosition[] vssrolpList = new NotificationXSD.VehiclesSummaryStructureRearOverhangListPosition[outBoundDocVar.Count];
        //                       recordcount = 0;
        //                       foreach (var outbound in outBoundDocVar)
        //                       {
        //                           NotificationXSD.VehiclesSummaryStructureRearOverhangListPosition vssrolp = new NotificationXSD.VehiclesSummaryStructureRearOverhangListPosition();
        //                           vssrolp.RearOverhang = NullableOrNotForDecimal(outbound.RearOverhang);
        //                           if (vssrolp.RearOverhang != null)
        //                           {
        //                               vssrolp.RearOverhangSpecified = true;
        //                           }
        //                           else
        //                           {
        //                               vssrolp.RearOverhangSpecified = false;
        //                           }

        //                           vssrolpList[recordcount] = vssrolp;
        //                           recordcount++;
        //                       }
        //                       vss.RearOverhangListPosition = vssrolpList;
        //                       #endregion

        //                       #region FrontOverhangListPosition
        //                       NotificationXSD.VehiclesSummaryStructureFrontOverhangListPosition[] vssfolpList = new NotificationXSD.VehiclesSummaryStructureFrontOverhangListPosition[outBoundDocVar.Count];
        //                       recordcount = 0;
        //                       foreach (var outbound in outBoundDocVar)
        //                       {
        //                           NotificationXSD.VehiclesSummaryStructureFrontOverhangListPosition vssfolp = new NotificationXSD.VehiclesSummaryStructureFrontOverhangListPosition();
        //                           NotificationXSD.FrontOverhangStructure frontoverhangstru = new NotificationXSD.FrontOverhangStructure();
        //                           frontoverhangstru.InfrontOfCab = false;
        //                           frontoverhangstru.Value = NullableOrNotForDecimal(outbound.FrontOverhang);
        //                           vssfolp.FrontOverhang = frontoverhangstru;
        //                           if (frontoverhangstru.Value != null)
        //                           {
        //                               vssfolp.FrontOverhangSpecified = true;
        //                           }
        //                           else
        //                           {
        //                               vssfolp.FrontOverhangSpecified = false;
        //                           }
        //                           vssfolpList[recordcount] = vssfolp;
        //                           recordcount++;
        //                       }
        //                       vss.FrontOverhangListPosition = vssfolpList;
        //                       #endregion

        //                       #region LeftOverhangListPosition
        //                       NotificationXSD.VehiclesSummaryStructureLeftOverhangListPosition[] vsslolpList = new NotificationXSD.VehiclesSummaryStructureLeftOverhangListPosition[outBoundDocVar.Count];
        //                       recordcount = 0;
        //                       foreach (var outbound in outBoundDocVar)
        //                       {
        //                           NotificationXSD.VehiclesSummaryStructureLeftOverhangListPosition vsslolp = new NotificationXSD.VehiclesSummaryStructureLeftOverhangListPosition();
        //                           NotificationXSD.LeftOverhangStructure leftoverhangstru = new NotificationXSD.LeftOverhangStructure();
        //                           leftoverhangstru.InLeftOfCab = false;
        //                           leftoverhangstru.Value = NullableOrNotForDecimal(outbound.LeftOverhang);
        //                           vsslolp.LeftOverhang = leftoverhangstru;
        //                           if (leftoverhangstru.Value != null)
        //                           {
        //                               vsslolp.LeftOverhangSpecified = true;
        //                           }
        //                           else
        //                           {
        //                               vsslolp.LeftOverhangSpecified = false;
        //                           }
        //                           vsslolpList[recordcount] = vsslolp;
        //                           recordcount++;
        //                       }
        //                       vss.LeftOverhangListPosition = vsslolpList;
        //                       #endregion

        //                       #region RightOverhangListPosition
        //                       NotificationXSD.VehiclesSummaryStructureRightOverhangListPosition[] vssriolpList = new NotificationXSD.VehiclesSummaryStructureRightOverhangListPosition[outBoundDocVar.Count];
        //                       recordcount = 0;
        //                       foreach (var outbound in outBoundDocVar)
        //                       {
        //                           NotificationXSD.VehiclesSummaryStructureRightOverhangListPosition vssrolp = new NotificationXSD.VehiclesSummaryStructureRightOverhangListPosition();
        //                           NotificationXSD.RightOverhangStructure rightoverhangstru = new NotificationXSD.RightOverhangStructure();
        //                           rightoverhangstru.InRightOfCab = false;
        //                           rightoverhangstru.Value = NullableOrNotForDecimal(outbound.RightOverhang);
        //                           vssrolp.RightOverhang = rightoverhangstru;
        //                           if (rightoverhangstru.Value != null)
        //                           {
        //                               vssrolp.RightOverhangSpecified = true;
        //                           }
        //                           else
        //                           {
        //                               vssrolp.RightOverhangSpecified = false;
        //                           }
        //                           vssriolpList[recordcount] = vssrolp;
        //                           recordcount++;
        //                       }
        //                       vss.RightOverhangListPosition = vssriolpList;
        //                       #endregion

        //                       #region OverallWidthListPosition
        //                       NotificationXSD.VehiclesSummaryStructureOverallWidthListPosition[] vssowlpList = new NotificationXSD.VehiclesSummaryStructureOverallWidthListPosition[outBoundDocVar.Count];
        //                       recordcount = 0;
        //                       foreach (var outbound in outBoundDocVar)
        //                       {

        //                           NotificationXSD.VehiclesSummaryStructureOverallWidthListPosition vssowlp = new NotificationXSD.VehiclesSummaryStructureOverallWidthListPosition();
        //                           vssowlp.OverallWidth = NullableOrNotForDecimal(outbound.Width);
        //                           if (vssowlp.OverallWidth != null)
        //                           {
        //                               vssowlp.OverallWidthSpecified = true;
        //                           }
        //                           else
        //                           {
        //                               vssowlp.OverallWidthSpecified = false;
        //                           }
        //                           vssowlpList[recordcount] = vssowlp;
        //                           recordcount++;
        //                       }
        //                       vss.OverallWidthListPosition = vssowlpList;
        //                       #endregion

        //                       #region OverallHeightListPosition
        //                       NotificationXSD.VehiclesSummaryStructureOverallHeightListPosition[] vssohlpList = new NotificationXSD.VehiclesSummaryStructureOverallHeightListPosition[outBoundDocVar.Count];
        //                       recordcount = 0;
        //                       foreach (var outbound in outBoundDocVar)
        //                       {
        //                           NotificationXSD.VehiclesSummaryStructureOverallHeightListPosition vssohlp = new NotificationXSD.VehiclesSummaryStructureOverallHeightListPosition();
        //                           NotificationXSD.SummaryHeightStructure shs = new NotificationXSD.SummaryHeightStructure();
        //                           shs.MaxHeight = Convert.ToDecimal(outbound.MaximumHeight);
        //                           // RM#4581 - ReducibleHeight should be compared with trailer/tractor max height and take max value from that. 
        //                           if (outbound.VehicleType != "rigid vehicle" && outbound.RedHeight != 0 && outBoundDocVehicle != null && outBoundDocVehicle.Any() && outBoundDocVehicle.FirstOrDefault().MaximumHeight > outbound.RedHeight)
        //                           {
        //                               shs.ReducibleHeight = Convert.ToDecimal(outBoundDocVehicle.FirstOrDefault().MaximumHeight);
        //                           }
        //                           else
        //                           {
        //                               shs.ReducibleHeight = NullableOrNotForDecimal(outbound.RedHeight);
        //                           }

        //                           if (shs.ReducibleHeight != null)
        //                           {
        //                               shs.ReducibleHeightSpecified = true;
        //                           }
        //                           else
        //                           {
        //                               shs.ReducibleHeightSpecified = false;
        //                           }
        //                           vssohlp.OverallHeight = shs;
        //                           vssohlpList[recordcount] = vssohlp;
        //                           recordcount++;
        //                       }
        //                       vss.OverallHeightListPosition = vssohlpList;
        //                       #endregion

        //                       #region GrossWeightListPosition
        //                       NotificationXSD.VehiclesSummaryStructureGrossWeightListPosition[] vssgwlpList = new NotificationXSD.VehiclesSummaryStructureGrossWeightListPosition[outBoundDocVar.Count];
        //                       recordcount = 0;
        //                       foreach (var outbound in outBoundDocVar)
        //                       {
        //                           NotificationXSD.VehiclesSummaryStructureGrossWeightListPosition vssgwlp = new NotificationXSD.VehiclesSummaryStructureGrossWeightListPosition();
        //                           NotificationXSD.GrossWeightStructure gws = new NotificationXSD.GrossWeightStructure();
        //                           gws.ExcludesTractors = false;
        //                           gws.Item = Convert.ToString(outbound.GrossWeight);

        //                           vssgwlp.GrossWeight = gws;
        //                           vssgwlpList[recordcount] = vssgwlp;
        //                           recordcount++;
        //                       }
        //                       #endregion

        //                       #region MaxAxleWeightListPosition
        //                       vss.GrossWeightListPosition = vssgwlpList;
        //                       NotificationXSD.VehiclesSummaryStructureMaxAxleWeightListPosition[] vssmawlpList = new NotificationXSD.VehiclesSummaryStructureMaxAxleWeightListPosition[outBoundDocVar.Count];
        //                       recordcount = 0;
        //                       foreach (var outbound in outBoundDocVar)
        //                       {
        //                           NotificationXSD.VehiclesSummaryStructureMaxAxleWeightListPosition vssmawlp = new NotificationXSD.VehiclesSummaryStructureMaxAxleWeightListPosition();
        //                           NotificationXSD.SummaryMaxAxleWeightStructure smaws = new NotificationXSD.SummaryMaxAxleWeightStructure();
        //                           smaws.ItemElementName = NotificationXSD.ItemChoiceType2.Weight;
        //                           smaws.Item = Convert.ToString(outbound.MaximumAxleWeight);
        //                           vssmawlp.MaximumAxleWeight = smaws;
        //                           vssmawlpList[recordcount] = vssmawlp;
        //                           recordcount++;
        //                       }
        //                       vss.MaximumAxleWeightListPosition = vssmawlpList;
        //                       #endregion

        //                       #region ConfigurationIdentityListPosition

        //                       NotificationXSD.VehiclesSummaryStructureVehicleSummaryListPosition[] vssvslpList = new NotificationXSD.VehiclesSummaryStructureVehicleSummaryListPosition[outBoundDocVar.Count];

        //                       recordcount = 0;
        //                       int totaAlternativeId = 1;

        //                       foreach (var outbound in outBoundDocVar)
        //                       {

        //                           List<VehicleConfigList> vehicleConfigurationList = GetVehicleConfigurationDetails(Convert.ToInt32(outbound.VehicleId), UserSchema.Portal);

        //                           // Business logic is written for semi vehicle
        //                           if (outbound.VehicleType.ToLower() == "semi trailer(3-8) vehicle" ||
        //                           outbound.VehicleType.ToLower() == "boat mast exception" || outbound.VehicleType.ToLower() == "semi vehicle" || outbound.VehicleType.ToLower() == "conventional tractor"
        //                               || outbound.VehicleType.ToLower() == "crane" || outbound.VehicleType.ToLower() == "mobile crane")
        //                           {
        //                               NotificationXSD.VehiclesSummaryStructureVehicleSummaryListPosition vssvslp = new NotificationXSD.VehiclesSummaryStructureVehicleSummaryListPosition();
        //                               NotificationXSD.VehicleSummaryStructure vssList = new NotificationXSD.VehicleSummaryStructure();

        //                               NotificationXSD.VehicleSummaryTypeChoiceStructure vstcs = new NotificationXSD.VehicleSummaryTypeChoiceStructure();
        //                               NotificationXSD.SemiTrailerSummaryStructure stss = new NotificationXSD.SemiTrailerSummaryStructure();

        //                               if (outbound.ComponentType != string.Empty)
        //                               {
        //                                   stss.TractorSubType = GetVehicleComponentSubTypes(outbound.ComponentType);
        //                               }

        //                               if (outbound.ComponentSubtype != string.Empty)
        //                               {
        //                                   stss.TrailerSubType = GetVehicleComponentSubTypes(outbound.ComponentSubtype);
        //                               }

        //                               stss.Summary = outbound.VehicleDesc;

        //                               NotificationXSD.SummaryWeightStructure sws = new NotificationXSD.SummaryWeightStructure();
        //                               sws.Item = Convert.ToString(outbound.GrossWeight);
        //                               stss.GrossWeight = sws;

        //                               stss.IsSteerableAtRear = outbound.IsSteerableAtRear == 1 ? true : false;
        //                               stss.IsSteerableAtRearSpecified = true;

        //                               NotificationXSD.SummaryMaxAxleWeightStructure smawsConfig = new NotificationXSD.SummaryMaxAxleWeightStructure();
        //                               smawsConfig.ItemElementName = NotificationXSD.ItemChoiceType2.Weight;
        //                               smawsConfig.Item = Convert.ToString(outbound.MaximumAxleWeight);
        //                               stss.MaximumAxleWeight = smawsConfig;

        //                               NotificationXSD.SummaryAxleStructure sas = new NotificationXSD.SummaryAxleStructure();

        //                               if (isHaulier == false)
        //                               {
        //                                   List<VehComponentAxles> vehicleComponentAxlesList = GetVehicleComponentAxles(NotificationID, outbound.VehicleId);

        //                                   sas.NumberOfAxles = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.AxleCount));

        //                                   sas.NumberOfWheel = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.WheelCount));

        //                                   #region AxleWeightListPosition
        //                                   sas.AxleWeightListPosition = GetAxleWeightListPosition(vehicleComponentAxlesList);
        //                                   #endregion

        //                                   #region WheelsPerAxleListPosition
        //                                   sas.WheelsPerAxleListPosition = GetWheelsPerAxleListPosition(vehicleComponentAxlesList);
        //                                   #endregion

        //                                   #region AxleSpacingListPosition
        //                                   sas.AxleSpacingListPosition = GetAxleSpacingListPositionAxleSpacing(vehicleComponentAxlesList);
        //                                   #endregion

        //                                   //Added RM#4386
        //                                   #region AxleSpacingToFollow
        //                                   sas.AxleSpacingToFollowing = GetAxleSpacingToFollowListPositionAxleSpacing(vehicleComponentAxlesList, vehicleConfigurationList[0].SpaceToFollowing);
        //                                   #endregion

        //                                   #region TyreSizeListPosition
        //                                   sas.TyreSizeListPosition = GetTyreSizeListPosition(vehicleComponentAxlesList);
        //                                   #endregion

        //                                   #region WheelSpacingListPosition
        //                                   sas.WheelSpacingListPosition = GetWheelSpacingListPosition(vehicleComponentAxlesList);
        //                                   #endregion

        //                                   stss.AxleConfiguration = sas;
        //                               }

        //                               stss.RigidLength = (decimal)outbound.RigidLength;
        //                               stss.Width = (decimal)outbound.Width;
        //                               NotificationXSD.SummaryHeightStructure shsVehi = new NotificationXSD.SummaryHeightStructure();
        //                               shsVehi.MaxHeight = (decimal)outbound.MaximumHeight;

        //                               shsVehi.ReducibleHeight = (decimal?)outbound.RedHeight;
        //                               if (shsVehi.ReducibleHeight != null && shsVehi.ReducibleHeight > 0)
        //                               {
        //                                   shsVehi.ReducibleHeightSpecified = true;
        //                               }
        //                               else
        //                               {
        //                                   shsVehi.ReducibleHeightSpecified = false;
        //                               }

        //                               stss.Height = shsVehi;
        //                               stss.Wheelbase = (decimal?)outbound.WheelBase;
        //                               if (stss.Wheelbase != null && stss.Wheelbase > 0)
        //                               {
        //                                   stss.WheelbaseSpecified = true;
        //                               }
        //                               else
        //                               {
        //                                   stss.WheelbaseSpecified = false;
        //                               }

        //                               stss.GroundClearance = (decimal?)outbound.GroundClearance;
        //                               if (stss.GroundClearance != null && stss.GroundClearance > 0)
        //                               {
        //                                   stss.GroundClearanceSpecified = true;
        //                               }
        //                               else
        //                               {
        //                                   stss.GroundClearanceSpecified = false;
        //                               }

        //                               stss.OutsideTrack = (decimal?)outbound.OutsideTrack;
        //                               if (stss.OutsideTrack != null && stss.OutsideTrack > 0)
        //                               {
        //                                   stss.OutsideTrackSpecified = true;
        //                               }
        //                               else
        //                               {
        //                                   stss.OutsideTrackSpecified = false;
        //                               }

        //                               // new code added
        //                               stss.RearOverhang = (decimal?)outbound.RearOverhang;

        //                               stss.LeftOverhang = (decimal?)outbound.LeftOverhang;
        //                               if (stss.LeftOverhang != null && stss.LeftOverhang > 0)
        //                               {
        //                                   stss.LeftOverhangSpecified = true;
        //                               }
        //                               else
        //                               {
        //                                   stss.LeftOverhangSpecified = false;
        //                               }

        //                               stss.RightOverhang = (decimal?)outbound.RightOverhang;
        //                               if (stss.RightOverhang != null && stss.RightOverhang > 0)
        //                               {
        //                                   stss.RightOverhangSpecified = true;
        //                               }
        //                               else
        //                               {
        //                                   stss.RightOverhangSpecified = false;
        //                               }

        //                               stss.FrontOverhang = (decimal?)outbound.FrontOverhang;
        //                               if (stss.RightOverhang != null && stss.RightOverhang > 0)
        //                               {
        //                                   stss.FrontOverhangSpecified = true;
        //                               }
        //                               else
        //                               {
        //                                   stss.FrontOverhangSpecified = false;
        //                               }
        //                               //new code ends here

        //                               vstcs.Item = stss;

        //                               vssList.Configuration = vstcs;
        //                               if (outbound.VehicleType == "drawbar vehicle")
        //                               {
        //                                   vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.drawbarvehicle;
        //                               }
        //                               if (outbound.VehicleType == "semi vehicle")
        //                               {
        //                                   vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.semivehicle;
        //                               }
        //                               if (outbound.VehicleType == "rigid vehicle")
        //                               {
        //                                   vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.rigidvehicle;
        //                               }
        //                               if (outbound.VehicleType == "tracked vehicle")
        //                               {
        //                                   vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.trackedvehicle;
        //                               }
        //                               if (outbound.VehicleType == "other in line")
        //                               {
        //                                   vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.otherinline;
        //                               }
        //                               if (outbound.VehicleType == "other side by side")
        //                               {
        //                                   vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.othersidebyside;
        //                               }
        //                               //Added - Jan272014 
        //                               if (outbound.VehicleType == "spmt")
        //                               {
        //                                   vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.spmt;
        //                               }
        //                               if (outbound.VehicleType == "crane" || outbound.VehicleType == "mobile crane")
        //                               {
        //                                   vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.crane;
        //                               }
        //                               if (outbound.VehicleType == "Recovery Vehicle")
        //                               {
        //                                   vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.recoveryvehicle;
        //                               }
        //                               //Added - Jan272014 - end
        //                               vssList.ConfigurationIdentityListPosition = GetVehicleSummaryConfigurationIdentity(NotificationID);

        //                               vssList.AlternativeId = Convert.ToString(totaAlternativeId);

        //                               double grossWeight = Convert.ToDouble(outbound.GrossWeight);

        //                               if (grossWeight >= 80000 && grossWeight <= 150000)
        //                               {
        //                                   vssList.WeightConformance = NotificationXSD.SummaryWeightConformanceType.heavystgo;
        //                               }
        //                               else
        //                               {
        //                                   vssList.WeightConformance = NotificationXSD.SummaryWeightConformanceType.other;
        //                               }

        //                               vssvslp.VehicleSummary = vssList;

        //                               vssvslpList[recordcount] = vssvslp;

        //                               recordcount++;
        //                               totaAlternativeId++;
        //                           }
        //                           else if (outbound.VehicleType.ToLower() == "drawbar vehicle" ||
        //                            outbound.VehicleType.ToLower() == "drawbar trailer(3-8) vehicle" ||
        //                               outbound.VehicleType.ToLower() == "ballast tractor" ||
        //                               outbound.VehicleType.ToLower() == "rigid vehicle" ||
        //                               outbound.VehicleType.ToLower() == "spmt"
        //                               || outbound.VehicleType.ToLower() == "other in line" || outbound.VehicleType.ToLower() == "recovery vehicle" || outbound.VehicleType.ToLower() == "rigid and drag")
        //                           // Facing issues with "other in line" as this contains both Semi and Non Semi Vehicles
        //                           {
        //                               #region For Other Inline or other vehicle
        //                               // Business logic is written for Non Semi Vehicle
        //                               NotificationXSD.VehiclesSummaryStructureVehicleSummaryListPosition vssvslp = new NotificationXSD.VehiclesSummaryStructureVehicleSummaryListPosition();
        //                               NotificationXSD.VehicleSummaryStructure vssList = new NotificationXSD.VehicleSummaryStructure();

        //                               NotificationXSD.VehicleSummaryTypeChoiceStructure vstcs = new NotificationXSD.VehicleSummaryTypeChoiceStructure();
        //                               NotificationXSD.NonSemiTrailerSummaryStructure stss = new NotificationXSD.NonSemiTrailerSummaryStructure();

        //                               NotificationXSD.NonSemiTrailerSummaryStructureComponentListPosition[] nsclp = new NotificationXSD.NonSemiTrailerSummaryStructureComponentListPosition[vehicleConfigurationList.Count];

        //                               int newRecordcount = 0;
        //                               int longitudeIncrement = 1;
        //                               int SpecialCaseIncrement = 0;
        //                               int SpecialCaseFlag = 0;

        //                               vssList.ConfigurationIdentityListPosition = GetVehicleSummaryConfigurationIdentity(NotificationID);

        //                               foreach (var vehicleList in vehicleConfigurationList)
        //                               {
        //                                   NotificationXSD.NonSemiTrailerSummaryStructureComponentListPosition nsclpObject = new NotificationXSD.NonSemiTrailerSummaryStructureComponentListPosition();

        //                                   NotificationXSD.ComponentSummaryStructure css = new NotificationXSD.ComponentSummaryStructure();

        //                                   css.Longitude = Convert.ToString(longitudeIncrement);

        //                                   if (vehicleList.ComponentType != string.Empty)
        //                                   {
        //                                       css.ComponentType = GetVehicleComponentSubTypesNonSemiVehicles(vehicleList.ComponentType);
        //                                   }

        //                                   if (vehicleList.ComponentSubType != string.Empty)
        //                                   {
        //                                       css.ComponentSubType = GetVehicleComponentSubTypes(vehicleList.ComponentSubType);
        //                                   }

        //                                   nsclpObject.Component = css;

        //                                   #region Non Semi Vehicle configuration

        //                                   List<string> allowedComponentTypeList = new List<string>();
        //                                   allowedComponentTypeList.Add("rigid vehicle");
        //                                   allowedComponentTypeList.Add("ballast tractor");
        //                                   allowedComponentTypeList.Add("drawbar trailer");
        //                                   allowedComponentTypeList.Add("spmt");

        //                                   if (outbound.VehicleType.ToLower() == "other in line" && vehicleConfigurationList.Count >= 2 && !vehicleConfigurationList.All(x => allowedComponentTypeList.Contains(x.ComponentType.ToLower())) && (vehicleConfigurationList[0].ComponentType.Trim().ToLower() == "rigid vehicle" || vehicleConfigurationList[0].ComponentType.Trim().ToLower() == "ballast tractor" || vehicleConfigurationList[0].ComponentType.Trim().ToLower() == "drawbar trailer" || vehicleConfigurationList[0].ComponentType.Trim().ToLower() == "spmt")) // Special condition when rigidvehicle + semitrailer
        //                                   {
        //                                       if (SpecialCaseIncrement >= 1)
        //                                       {
        //                                           break;
        //                                       }
        //                                       #region For Special case when nonsemi is at top and semi-trailer is at bottom

        //                                       NotificationXSD.VehiclesSummaryStructureVehicleSummaryListPosition vssvslp2 = new NotificationXSD.VehiclesSummaryStructureVehicleSummaryListPosition();

        //                                       NotificationXSD.SemiTrailerSummaryStructure dtss = new NotificationXSD.SemiTrailerSummaryStructure();

        //                                       dtss.Summary = outbound.VehicleDesc;

        //                                       NotificationXSD.SummaryWeightStructure sws = new NotificationXSD.SummaryWeightStructure();
        //                                       sws.Item = Convert.ToString(outbound.GrossWeight);
        //                                       dtss.GrossWeight = sws;

        //                                       dtss.IsSteerableAtRear = outbound.IsSteerableAtRear == 1 ? true : false;
        //                                       dtss.IsSteerableAtRearSpecified = true;

        //                                       NotificationXSD.SummaryMaxAxleWeightStructure smawsConfig = new NotificationXSD.SummaryMaxAxleWeightStructure();
        //                                       smawsConfig.ItemElementName = NotificationXSD.ItemChoiceType2.Weight;
        //                                       smawsConfig.Item = Convert.ToString(outbound.MaximumAxleWeight);
        //                                       dtss.MaximumAxleWeight = smawsConfig;

        //                                       NotificationXSD.SummaryAxleStructure sas = new NotificationXSD.SummaryAxleStructure();

        //                                       if (!isHaulier)
        //                                       {
        //                                           List<VehComponentAxles> vehicleComponentAxlesList = GetVehicleComponentAxles(NotificationID, outbound.VehicleId);

        //                                           sas.NumberOfAxles = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.AxleCount));

        //                                           sas.NumberOfWheel = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.WheelCount));

        //                                           #region AxleWeightListPosition
        //                                           sas.AxleWeightListPosition = GetAxleWeightListPosition(vehicleComponentAxlesList);
        //                                           #endregion

        //                                           #region WheelsPerAxleListPosition
        //                                           sas.WheelsPerAxleListPosition = GetWheelsPerAxleListPosition(vehicleComponentAxlesList);
        //                                           #endregion

        //                                           #region AxleSpacingListPosition
        //                                           sas.AxleSpacingListPosition = GetAxleSpacingListPositionAxleSpacing(vehicleComponentAxlesList);
        //                                           #endregion

        //                                           //Added RM#4386
        //                                           #region AxleSpacingToFollow
        //                                           sas.AxleSpacingToFollowing = GetAxleSpacingToFollowListPositionAxleSpacing(vehicleComponentAxlesList, vehicleConfigurationList[0].SpaceToFollowing);
        //                                           #endregion

        //                                           #region TyreSizeListPosition
        //                                           sas.TyreSizeListPosition = GetTyreSizeListPosition(vehicleComponentAxlesList);
        //                                           #endregion

        //                                           #region WheelSpacingListPosition
        //                                           sas.WheelSpacingListPosition = GetWheelSpacingListPosition(vehicleComponentAxlesList);
        //                                           #endregion

        //                                           dtss.AxleConfiguration = sas;
        //                                       }

        //                                       dtss.RigidLength = (decimal)outbound.RigidLength;
        //                                       dtss.Width = (decimal)outbound.Width;

        //                                       NotificationXSD.SummaryHeightStructure shsVehi = new NotificationXSD.SummaryHeightStructure();
        //                                       shsVehi.MaxHeight = (decimal)outbound.MaximumHeight;

        //                                       shsVehi.ReducibleHeight = (decimal)outbound.RedHeight;
        //                                       if (shsVehi.ReducibleHeight != null && shsVehi.ReducibleHeight > 0)
        //                                       {
        //                                           shsVehi.ReducibleHeightSpecified = true;
        //                                       }
        //                                       else
        //                                       {
        //                                           shsVehi.ReducibleHeightSpecified = false;
        //                                       }

        //                                       dtss.Height = shsVehi;
        //                                       dtss.Wheelbase = (decimal?)outbound.WheelBase;
        //                                       if (dtss.Wheelbase != null && dtss.Wheelbase > 0)
        //                                       {
        //                                           dtss.WheelbaseSpecified = true;
        //                                       }
        //                                       else
        //                                       {
        //                                           dtss.WheelbaseSpecified = false;
        //                                       }

        //                                       dtss.GroundClearance = (decimal?)outbound.GroundClearance;
        //                                       if (dtss.GroundClearance != null && dtss.GroundClearance > 0)
        //                                       {
        //                                           dtss.GroundClearanceSpecified = true;
        //                                       }
        //                                       else
        //                                       {
        //                                           dtss.GroundClearanceSpecified = false;
        //                                       }

        //                                       dtss.OutsideTrack = (decimal?)outbound.OutsideTrack;
        //                                       if (dtss.OutsideTrack != null && dtss.OutsideTrack > 0)
        //                                       {
        //                                           dtss.OutsideTrackSpecified = true;
        //                                       }
        //                                       else
        //                                       {
        //                                           dtss.OutsideTrackSpecified = false;
        //                                       }

        //                                       // new code added

        //                                       dtss.RearOverhang = (decimal?)outbound.RearOverhang;

        //                                       dtss.LeftOverhang = (decimal?)outbound.LeftOverhang;
        //                                       if (dtss.LeftOverhang != null && dtss.LeftOverhang > 0)
        //                                       {
        //                                           dtss.LeftOverhangSpecified = true;
        //                                       }
        //                                       else
        //                                       {
        //                                           dtss.LeftOverhangSpecified = false;
        //                                       }

        //                                       dtss.RightOverhang = (decimal?)outbound.RightOverhang;
        //                                       if (dtss.RightOverhang != null && dtss.RightOverhang > 0)
        //                                       {
        //                                           dtss.RightOverhangSpecified = true;
        //                                       }
        //                                       else
        //                                       {
        //                                           dtss.RightOverhangSpecified = false;
        //                                       }

        //                                       dtss.FrontOverhang = (decimal?)outbound.FrontOverhang;
        //                                       if (dtss.FrontOverhang != null && dtss.FrontOverhang > 0)
        //                                       {
        //                                           dtss.FrontOverhangSpecified = true;
        //                                       }
        //                                       else
        //                                       {
        //                                           dtss.FrontOverhangSpecified = false;
        //                                       }

        //                                       //new code ends here

        //                                       if (outbound.VehicleType == "drawbar vehicle")
        //                                       {
        //                                           vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.drawbarvehicle;
        //                                       }
        //                                       if (outbound.VehicleType == "semi vehicle")
        //                                       {
        //                                           vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.semivehicle;
        //                                       }
        //                                       if (outbound.VehicleType == "rigid vehicle")
        //                                       {
        //                                           vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.rigidvehicle;
        //                                       }
        //                                       if (outbound.VehicleType == "tracked vehicle")
        //                                       {
        //                                           vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.trackedvehicle;
        //                                       }
        //                                       if (outbound.VehicleType == "other in line")
        //                                       {
        //                                           vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.otherinline;
        //                                       }
        //                                       if (outbound.VehicleType == "other side by side")
        //                                       {
        //                                           vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.othersidebyside;
        //                                       }
        //                                       //Added - Jan272014 
        //                                       if (outbound.VehicleType == "spmt")
        //                                       {
        //                                           vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.spmt;
        //                                       }

        //                                       vstcs.Item = dtss;
        //                                       vssList.Configuration = vstcs;
        //                                       vssvslp.VehicleSummary = vssList;

        //                                       if (outBoundDocVar[0].VehicleType.ToLower() == "semi vehicle" || outBoundDocVar[0].VehicleType.ToLower() == "conventional tractor")
        //                                       {
        //                                           vssvslpList[recordcount] = vssvslp;
        //                                           recordcount++;
        //                                       }
        //                                       else
        //                                       {
        //                                           vssvslpList[newRecordcount] = vssvslp;
        //                                           newRecordcount++;
        //                                       }

        //                                       SpecialCaseFlag++;
        //                                       SpecialCaseIncrement++;
        //                                       #endregion
        //                                   }
        //                                   else
        //                                   {

        //                                       if (nsclpObject.Component.ComponentType == NotificationXSD.VehicleComponentType.ballasttractor)
        //                                       {
        //                                           #region For ballasttractor
        //                                           NotificationXSD.DrawbarTractorSummaryStructure dtss = new NotificationXSD.DrawbarTractorSummaryStructure();

        //                                           dtss.Summary = vehicleList.VehicleDescription;

        //                                           dtss.Weight = Convert.ToString(vehicleList.GrossWeight);

        //                                           NotificationXSD.SummaryMaxAxleWeightStructure smawsConfig = new NotificationXSD.SummaryMaxAxleWeightStructure();
        //                                           smawsConfig.ItemElementName = NotificationXSD.ItemChoiceType2.Weight;
        //                                           smawsConfig.Item = Convert.ToString(outbound.MaximumAxleWeight);

        //                                           dtss.MaxAxleWeight = smawsConfig;

        //                                           NotificationXSD.SummaryAxleStructure sas = new NotificationXSD.SummaryAxleStructure();

        //                                           if (!isHaulier)
        //                                           {
        //                                               List<VehComponentAxles> vehicleComponentAxlesList = GetVehicleComponentAxlesByComponent(vehicleList.ComponentId, outbound.VehicleId, UserSchema.Portal);

        //                                               sas.NumberOfAxles = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.AxleCount));

        //                                               sas.NumberOfWheel = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.WheelCount));

        //                                               #region AxleWeightListPosition
        //                                               sas.AxleWeightListPosition = GetAxleWeightListPosition(vehicleComponentAxlesList);
        //                                               #endregion

        //                                               #region WheelsPerAxleListPosition
        //                                               sas.WheelsPerAxleListPosition = GetWheelsPerAxleListPosition(vehicleComponentAxlesList);
        //                                               #endregion

        //                                               #region AxleSpacingListPosition
        //                                               sas.AxleSpacingListPosition = GetAxleSpacingListPosition(vehicleComponentAxlesList);
        //                                               #endregion

        //                                               //Added RM#4386
        //                                               #region AxleSpacingToFollow
        //                                               sas.AxleSpacingToFollowing = GetAxleSpacingToFollowListPositionAxleSpacing(vehicleComponentAxlesList, vehicleConfigurationList[0].SpaceToFollowing);
        //                                               #endregion

        //                                               #region TyreSizeListPosition
        //                                               sas.TyreSizeListPosition = GetTyreSizeListPosition(vehicleComponentAxlesList);
        //                                               #endregion

        //                                               #region WheelSpacingListPosition
        //                                               sas.WheelSpacingListPosition = GetWheelSpacingListPosition(vehicleComponentAxlesList);
        //                                               #endregion

        //                                               dtss.AxleConfiguration = sas;
        //                                           }

        //                                           dtss.Length = (decimal?)outbound.RigidLength;
        //                                           if (dtss.Length != null && dtss.Length > 0)
        //                                           {
        //                                               dtss.LengthSpecified = true;
        //                                           }
        //                                           else
        //                                           {
        //                                               dtss.LengthSpecified = false;
        //                                           }

        //                                           dtss.AxleSpacingToFollowing = (decimal)vehicleList.SpaceToFollowing;
        //                                           dtss.AxleSpacingToFollowingSpecified = true;

        //                                           NotificationXSD.SummaryHeightStructure shsVehi = new NotificationXSD.SummaryHeightStructure();
        //                                           shsVehi.MaxHeight = (decimal)outbound.MaximumHeight;

        //                                           shsVehi.ReducibleHeight = (decimal?)vehicleList.RedHeight;
        //                                           if (shsVehi.ReducibleHeight != null && shsVehi.ReducibleHeight > 0)
        //                                           {
        //                                               shsVehi.ReducibleHeightSpecified = true;
        //                                           }
        //                                           else
        //                                           {
        //                                               shsVehi.ReducibleHeightSpecified = false;
        //                                           }

        //                                           nsclpObject.Component.Item = dtss;

        //                                           nsclp[newRecordcount] = nsclpObject;

        //                                           newRecordcount++;
        //                                           #endregion
        //                                       }
        //                                       if ((outbound.VehicleType.ToLower() == "recovery vehicle" && (nsclpObject.Component.ComponentType == NotificationXSD.VehicleComponentType.semitrailer || nsclpObject.Component.ComponentType == NotificationXSD.VehicleComponentType.conventionaltractor)))
        //                                       {
        //                                           #region For ballasttractor
        //                                           NotificationXSD.DrawbarTractorSummaryStructure dtss = new NotificationXSD.DrawbarTractorSummaryStructure();

        //                                           dtss.Summary = vehicleList.VehicleDescription;

        //                                           dtss.Weight = Convert.ToString(vehicleList.GrossWeight);

        //                                           NotificationXSD.SummaryMaxAxleWeightStructure smawsConfig = new NotificationXSD.SummaryMaxAxleWeightStructure();
        //                                           smawsConfig.ItemElementName = NotificationXSD.ItemChoiceType2.Weight;
        //                                           smawsConfig.Item = Convert.ToString(outbound.MaximumAxleWeight);

        //                                           dtss.MaxAxleWeight = smawsConfig;

        //                                           NotificationXSD.SummaryAxleStructure sas = new NotificationXSD.SummaryAxleStructure();

        //                                           List<VehComponentAxles> vehicleComponentAxlesList = GetVehicleComponentAxlesByComponent(vehicleList.ComponentId, outbound.VehicleId, UserSchema.Portal);

        //                                           sas.NumberOfAxles = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.AxleCount));

        //                                           sas.NumberOfWheel = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.WheelCount));

        //                                           #region AxleWeightListPosition
        //                                           sas.AxleWeightListPosition = GetAxleWeightListPosition(vehicleComponentAxlesList);
        //                                           #endregion

        //                                           #region WheelsPerAxleListPosition
        //                                           sas.WheelsPerAxleListPosition = GetWheelsPerAxleListPosition(vehicleComponentAxlesList);
        //                                           #endregion

        //                                           #region AxleSpacingListPosition
        //                                           sas.AxleSpacingListPosition = GetAxleSpacingListPosition(vehicleComponentAxlesList);
        //                                           #endregion

        //                                           //Added RM#4386
        //                                           #region AxleSpacingToFollow
        //                                           sas.AxleSpacingToFollowing = GetAxleSpacingToFollowListPositionAxleSpacing(vehicleComponentAxlesList, vehicleConfigurationList[0].SpaceToFollowing);
        //                                           #endregion

        //                                           #region TyreSizeListPosition
        //                                           sas.TyreSizeListPosition = GetTyreSizeListPosition(vehicleComponentAxlesList);
        //                                           #endregion

        //                                           #region WheelSpacingListPosition
        //                                           sas.WheelSpacingListPosition = GetWheelSpacingListPosition(vehicleComponentAxlesList);
        //                                           #endregion

        //                                           dtss.AxleConfiguration = sas;

        //                                           dtss.Length = (decimal?)outbound.RigidLength;
        //                                           if (dtss.Length != null && dtss.Length > 0)
        //                                           {
        //                                               dtss.LengthSpecified = true;
        //                                           }
        //                                           else
        //                                           {
        //                                               dtss.LengthSpecified = false;
        //                                           }

        //                                           dtss.AxleSpacingToFollowing = (decimal)vehicleList.SpaceToFollowing;
        //                                           dtss.AxleSpacingToFollowingSpecified = true;

        //                                           NotificationXSD.SummaryHeightStructure shsVehi = new NotificationXSD.SummaryHeightStructure();
        //                                           shsVehi.MaxHeight = (decimal)outbound.MaximumHeight;

        //                                           shsVehi.ReducibleHeight = (decimal?)vehicleList.RedHeight;
        //                                           if (shsVehi.ReducibleHeight != null && shsVehi.ReducibleHeight > 0)
        //                                           {
        //                                               shsVehi.ReducibleHeightSpecified = true;
        //                                           }
        //                                           else
        //                                           {
        //                                               shsVehi.ReducibleHeightSpecified = false;
        //                                           }

        //                                           nsclpObject.Component.Item = dtss;

        //                                           nsclp[newRecordcount] = nsclpObject;

        //                                           newRecordcount++;
        //                                           #endregion
        //                                       }

        //                                       else if (nsclpObject.Component.ComponentType == NotificationXSD.VehicleComponentType.drawbartrailer
        //                                           || nsclpObject.Component.ComponentType == NotificationXSD.VehicleComponentType.spmt
        //                                           || nsclpObject.Component.ComponentType == NotificationXSD.VehicleComponentType.rigidvehicle)
        //                                       {
        //                                           #region For rigidvehicle or spmt or drawbartrailer
        //                                           NotificationXSD.LoadBearingSummaryStructure dtss = new NotificationXSD.LoadBearingSummaryStructure();

        //                                           dtss.Summary = vehicleList.VehicleDescription;

        //                                           dtss.Weight = Convert.ToString(vehicleList.GrossWeight);

        //                                           dtss.IsSteerableAtRear = vehicleList.IsSteerableAtRear == 1;
        //                                           dtss.IsSteerableAtRearSpecified = true;

        //                                           NotificationXSD.SummaryMaxAxleWeightStructure smawsConfig = new NotificationXSD.SummaryMaxAxleWeightStructure();
        //                                           smawsConfig.ItemElementName = NotificationXSD.ItemChoiceType2.Weight;
        //                                           smawsConfig.Item = Convert.ToString(outbound.MaximumAxleWeight);
        //                                           dtss.MaxAxleWeight = smawsConfig;

        //                                           NotificationXSD.SummaryAxleStructure sas = new NotificationXSD.SummaryAxleStructure();

        //                                           if (!isHaulier)
        //                                           {
        //                                               List<VehComponentAxles> vehicleComponentAxlesList = GetVehicleComponentAxlesByComponent(vehicleList.ComponentId, outbound.VehicleId, UserSchema.Portal);

        //                                               sas.NumberOfAxles = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.AxleCount));

        //                                               sas.NumberOfWheel = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.WheelCount));

        //                                               #region AxleWeightListPosition
        //                                               sas.AxleWeightListPosition = GetAxleWeightListPosition(vehicleComponentAxlesList);
        //                                               #endregion

        //                                               #region WheelsPerAxleListPosition
        //                                               sas.WheelsPerAxleListPosition = GetWheelsPerAxleListPosition(vehicleComponentAxlesList);
        //                                               #endregion

        //                                               #region AxleSpacingListPosition
        //                                               sas.AxleSpacingListPosition = GetAxleSpacingListPosition(vehicleComponentAxlesList);
        //                                               #endregion

        //                                               //Added RM#4386
        //                                               #region AxleSpacingToFollow
        //                                               sas.AxleSpacingToFollowing = GetAxleSpacingToFollowListPositionAxleSpacing(vehicleComponentAxlesList, vehicleConfigurationList[0].SpaceToFollowing);
        //                                               #endregion

        //                                               #region TyreSizeListPosition
        //                                               sas.TyreSizeListPosition = GetTyreSizeListPosition(vehicleComponentAxlesList);
        //                                               #endregion

        //                                               #region WheelSpacingListPosition
        //                                               sas.WheelSpacingListPosition = GetWheelSpacingListPosition(vehicleComponentAxlesList);
        //                                               #endregion

        //                                               dtss.AxleConfiguration = sas;
        //                                           }

        //                                           dtss.RigidLength = (decimal)outbound.RigidLength;
        //                                           dtss.Width = (decimal)outbound.Width;

        //                                           NotificationXSD.SummaryHeightStructure shsVehi = new NotificationXSD.SummaryHeightStructure();
        //                                           shsVehi.MaxHeight = (decimal)outbound.MaximumHeight;

        //                                           shsVehi.ReducibleHeight = (decimal?)vehicleList.RedHeight;
        //                                           if (shsVehi.ReducibleHeight != null && shsVehi.ReducibleHeight > 0)
        //                                           {
        //                                               shsVehi.ReducibleHeightSpecified = true;
        //                                           }
        //                                           else
        //                                           {
        //                                               shsVehi.ReducibleHeightSpecified = false;
        //                                           }


        //                                           dtss.Height = shsVehi;
        //                                           dtss.Wheelbase = (decimal?)vehicleList.WheelBase;
        //                                           if (dtss.Wheelbase != null && dtss.Wheelbase > 0)
        //                                           {
        //                                               dtss.WheelbaseSpecified = true;
        //                                           }
        //                                           else
        //                                           {
        //                                               dtss.WheelbaseSpecified = false;
        //                                           }

        //                                           dtss.GroundClearance = (decimal?)vehicleList.GroundClearance;
        //                                           if (dtss.GroundClearance != null && dtss.GroundClearance > 0)
        //                                           {
        //                                               dtss.GroundClearanceSpecified = true;
        //                                           }
        //                                           else
        //                                           {
        //                                               dtss.GroundClearanceSpecified = false;
        //                                           }

        //                                           dtss.OutsideTrack = (decimal?)vehicleList.OutsideTrack;
        //                                           if (dtss.OutsideTrack != null && dtss.OutsideTrack > 0)
        //                                           {
        //                                               dtss.OutsideTrackSpecified = true;
        //                                           }
        //                                           else
        //                                           {
        //                                               dtss.OutsideTrackSpecified = false;
        //                                           }

        //                                           // new code added

        //                                           dtss.RearOverhang = (decimal?)vehicleList.RearOverhang;

        //                                           dtss.LeftOverhang = (decimal?)vehicleList.LeftOverhang;
        //                                           if (dtss.LeftOverhang != null && dtss.LeftOverhang > 0)
        //                                           {
        //                                               dtss.LeftOverhangSpecified = true;
        //                                           }
        //                                           else
        //                                           {
        //                                               dtss.LeftOverhangSpecified = false;
        //                                           }

        //                                           dtss.RightOverhang = (decimal?)vehicleList.RightOverhang;
        //                                           if (dtss.RightOverhang != null && dtss.RightOverhang > 0)
        //                                           {
        //                                               dtss.RightOverhangSpecified = true;
        //                                           }
        //                                           else
        //                                           {
        //                                               dtss.RightOverhangSpecified = false;
        //                                           }

        //                                           dtss.FrontOverhang = (decimal?)vehicleList.FrontOverhang;
        //                                           if (dtss.FrontOverhang != null && dtss.FrontOverhang > 0)
        //                                           {
        //                                               dtss.FrontOverhangSpecified = true;
        //                                           }
        //                                           else
        //                                           {
        //                                               dtss.FrontOverhangSpecified = false;
        //                                           }

        //                                           dtss.AxleSpacingToFollowing = Convert.ToDecimal(vehicleList.SpaceToFollowing);
        //                                           if (dtss.AxleSpacingToFollowing != null && dtss.AxleSpacingToFollowing > 0)
        //                                           {
        //                                               dtss.AxleSpacingToFollowingSpecified = true;
        //                                           }
        //                                           else
        //                                           {
        //                                               dtss.AxleSpacingToFollowingSpecified = false;
        //                                           }

        //                                           dtss.ReducedGroundClearance = (decimal?)vehicleList.RedGroundClearance;
        //                                           if (dtss.ReducedGroundClearance != null && dtss.ReducedGroundClearance > 0)
        //                                           {
        //                                               dtss.ReducedGroundClearanceSpecified = true;
        //                                           }
        //                                           else
        //                                           {
        //                                               dtss.ReducedGroundClearanceSpecified = false;
        //                                           }
        //                                           //new code ends here

        //                                           nsclpObject.Component.Item = dtss;

        //                                           nsclp[newRecordcount] = nsclpObject;

        //                                           newRecordcount++;
        //                                           #endregion
        //                                       }

        //                                       else if (nsclpObject.Component.ComponentType == NotificationXSD.VehicleComponentType.semitrailer || nsclpObject.Component.ComponentType == NotificationXSD.VehicleComponentType.conventionaltractor)
        //                                       {
        //                                           #region For semitrailer or conventionaltractor

        //                                           NotificationXSD.VehiclesSummaryStructureVehicleSummaryListPosition vssvslp2 = new NotificationXSD.VehiclesSummaryStructureVehicleSummaryListPosition();

        //                                           NotificationXSD.SemiTrailerSummaryStructure dtss = new NotificationXSD.SemiTrailerSummaryStructure();

        //                                           dtss.Summary = outbound.VehicleDesc;

        //                                           NotificationXSD.SummaryWeightStructure sws = new NotificationXSD.SummaryWeightStructure();
        //                                           sws.Item = Convert.ToString(outbound.GrossWeight);
        //                                           dtss.GrossWeight = sws;

        //                                           dtss.IsSteerableAtRear = outbound.IsSteerableAtRear == 1;
        //                                           dtss.IsSteerableAtRearSpecified = true;

        //                                           NotificationXSD.SummaryMaxAxleWeightStructure smawsConfig = new NotificationXSD.SummaryMaxAxleWeightStructure();
        //                                           smawsConfig.ItemElementName = NotificationXSD.ItemChoiceType2.Weight;
        //                                           smawsConfig.Item = Convert.ToString(outbound.MaximumAxleWeight);
        //                                           dtss.MaximumAxleWeight = smawsConfig;

        //                                           NotificationXSD.SummaryAxleStructure sas = new NotificationXSD.SummaryAxleStructure();

        //                                           if (!isHaulier)
        //                                           {
        //                                               List<VehComponentAxles> vehicleComponentAxlesList = GetVehicleComponentAxles(NotificationID, outbound.VehicleId);

        //                                               sas.NumberOfAxles = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.AxleCount));

        //                                               sas.NumberOfWheel = Convert.ToString(vehicleComponentAxlesList.Sum(x => x.WheelCount));

        //                                               #region AxleWeightListPosition
        //                                               sas.AxleWeightListPosition = GetAxleWeightListPosition(vehicleComponentAxlesList);
        //                                               #endregion

        //                                               #region WheelsPerAxleListPosition
        //                                               sas.WheelsPerAxleListPosition = GetWheelsPerAxleListPosition(vehicleComponentAxlesList);
        //                                               #endregion

        //                                               #region AxleSpacingListPosition
        //                                               sas.AxleSpacingListPosition = GetAxleSpacingListPositionAxleSpacing(vehicleComponentAxlesList);
        //                                               #endregion

        //                                               //Added RM#4386
        //                                               #region AxleSpacingToFollow
        //                                               sas.AxleSpacingToFollowing = GetAxleSpacingToFollowListPositionAxleSpacing(vehicleComponentAxlesList, vehicleConfigurationList[0].SpaceToFollowing);
        //                                               #endregion

        //                                               #region TyreSizeListPosition
        //                                               sas.TyreSizeListPosition = GetTyreSizeListPosition(vehicleComponentAxlesList);
        //                                               #endregion

        //                                               #region WheelSpacingListPosition
        //                                               sas.WheelSpacingListPosition = GetWheelSpacingListPosition(vehicleComponentAxlesList);
        //                                               #endregion

        //                                               dtss.AxleConfiguration = sas;
        //                                           }

        //                                           dtss.RigidLength = (decimal)outbound.RigidLength;
        //                                           dtss.Width = (decimal)outbound.Width;

        //                                           NotificationXSD.SummaryHeightStructure shsVehi = new NotificationXSD.SummaryHeightStructure();
        //                                           shsVehi.MaxHeight = (decimal)outbound.MaximumHeight;

        //                                           shsVehi.ReducibleHeight = (decimal?)outbound.RedHeight;
        //                                           if (shsVehi.ReducibleHeight != null && shsVehi.ReducibleHeight > 0)
        //                                           {
        //                                               shsVehi.ReducibleHeightSpecified = true;
        //                                           }
        //                                           else
        //                                           {
        //                                               shsVehi.ReducibleHeightSpecified = false;
        //                                           }

        //                                           dtss.Height = shsVehi;
        //                                           dtss.Wheelbase = (decimal)outbound.WheelBase;
        //                                           if (dtss.Wheelbase != null && dtss.Wheelbase > 0)
        //                                           {
        //                                               dtss.WheelbaseSpecified = true;
        //                                           }
        //                                           else
        //                                           {
        //                                               dtss.WheelbaseSpecified = false;
        //                                           }

        //                                           dtss.GroundClearance = (decimal)outbound.GroundClearance;
        //                                           if (dtss.GroundClearance != null && dtss.GroundClearance > 0)
        //                                           {
        //                                               dtss.GroundClearanceSpecified = true;
        //                                           }
        //                                           else
        //                                           {
        //                                               dtss.GroundClearanceSpecified = false;
        //                                           }

        //                                           dtss.OutsideTrack = (decimal)outbound.OutsideTrack;
        //                                           if (dtss.OutsideTrack != null && dtss.OutsideTrack > 0)
        //                                           {
        //                                               dtss.OutsideTrackSpecified = true;
        //                                           }
        //                                           else
        //                                           {
        //                                               dtss.OutsideTrackSpecified = false;
        //                                           }

        //                                           // new code added

        //                                           dtss.RearOverhang = (decimal?)outbound.RearOverhang;

        //                                           dtss.LeftOverhang = (decimal?)outbound.LeftOverhang;
        //                                           if (dtss.LeftOverhang != null && dtss.LeftOverhang > 0)
        //                                           {
        //                                               dtss.LeftOverhangSpecified = true;
        //                                           }
        //                                           else
        //                                           {
        //                                               dtss.LeftOverhangSpecified = false;
        //                                           }

        //                                           dtss.RightOverhang = (decimal?)outbound.RightOverhang;
        //                                           if (dtss.RightOverhang != null && dtss.RightOverhang > 0)
        //                                           {
        //                                               dtss.RightOverhangSpecified = true;
        //                                           }
        //                                           else
        //                                           {
        //                                               dtss.RightOverhangSpecified = false;
        //                                           }

        //                                           dtss.FrontOverhang = (decimal?)outbound.FrontOverhang;
        //                                           if (dtss.FrontOverhang != null && dtss.FrontOverhang > 0)
        //                                           {
        //                                               dtss.FrontOverhangSpecified = true;
        //                                           }
        //                                           else
        //                                           {
        //                                               dtss.FrontOverhangSpecified = false;
        //                                           }

        //                                           //new code ends here

        //                                           vstcs.Item = dtss;
        //                                           vssList.Configuration = vstcs;
        //                                           vssvslp.VehicleSummary = vssList;

        //                                           newRecordcount++;
        //                                           #endregion
        //                                       }
        //                                   }

        //                                   #endregion

        //                                   longitudeIncrement++;
        //                               }

        //                               if (SpecialCaseFlag == 0)
        //                               {

        //                                   if (vehicleConfigurationList.Count > 0 && (vehicleConfigurationList[0].ComponentType != "semi trailer" && vehicleConfigurationList[0].ComponentType != "conventional tractor"))
        //                                   {
        //                                       stss.ComponentListPosition = nsclp;
        //                                   }

        //                                   if (outbound.VehicleType == "drawbar vehicle")
        //                                   {
        //                                       vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.drawbarvehicle;
        //                                   }
        //                                   if (outbound.VehicleType == "semi vehicle")
        //                                   {
        //                                       vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.semivehicle;
        //                                   }
        //                                   if (outbound.VehicleType == "rigid vehicle")
        //                                   {
        //                                       vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.rigidvehicle;
        //                                   }
        //                                   if (outbound.VehicleType == "tracked vehicle")
        //                                   {
        //                                       vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.trackedvehicle;
        //                                   }
        //                                   if (outbound.VehicleType == "other in line")
        //                                   {
        //                                       vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.otherinline;
        //                                   }
        //                                   if (outbound.VehicleType == "other side by side")
        //                                   {
        //                                       vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.othersidebyside;
        //                                   }
        //                                   //Added - Jan272014 
        //                                   if (outbound.VehicleType == "spmt")
        //                                   {
        //                                       vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.spmt;
        //                                   }
        //                                   if (outbound.VehicleType == "crane" || outbound.VehicleType == "mobile crane")
        //                                   {
        //                                       vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.crane;
        //                                   }
        //                                   if (outbound.VehicleType == "Recovery Vehicle")
        //                                   {
        //                                       vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.recoveryvehicle;
        //                                   }
        //                                   //Added - Jan272014 - end

        //                                   if (vehicleConfigurationList.Count > 0 && (vehicleConfigurationList[0].ComponentType != "semi trailer" && vehicleConfigurationList[0].ComponentType != "conventional tractor"))
        //                                   {
        //                                       vstcs.Item = stss;
        //                                   }

        //                                   vssList.Configuration = vstcs;

        //                                   vssvslp.VehicleSummary = vssList;

        //                                   vssvslpList[recordcount] = vssvslp;
        //                               }
        //                               recordcount++;
        //                               #endregion
        //                           }
        //                           else
        //                           {
        //                               NotificationXSD.VehiclesSummaryStructureVehicleSummaryListPosition vssvslp = new NotificationXSD.VehiclesSummaryStructureVehicleSummaryListPosition();
        //                               NotificationXSD.VehicleSummaryStructure vssList = new NotificationXSD.VehicleSummaryStructure();

        //                               NotificationXSD.VehicleSummaryTypeChoiceStructure vstcs = new NotificationXSD.VehicleSummaryTypeChoiceStructure();

        //                               NotificationXSD.TrackedVehicleSummaryStructure dtss = new NotificationXSD.TrackedVehicleSummaryStructure();

        //                               NotificationXSD.ComponentSummaryStructure css = new NotificationXSD.ComponentSummaryStructure();

        //                               vssList.ConfigurationIdentityListPosition = GetVehicleSummaryConfigurationIdentity(NotificationID);

        //                               dtss.Summary = outbound.VehicleDesc;

        //                               NotificationXSD.SummaryWeightStructure sws = new NotificationXSD.SummaryWeightStructure();
        //                               sws.Item = Convert.ToString(outbound.GrossWeight);
        //                               dtss.GrossWeight = sws;

        //                               dtss.RigidLength = (decimal)outbound.RigidLength;

        //                               dtss.Width = (decimal)outbound.Width;

        //                               if (outbound.VehicleType == "drawbar vehicle")
        //                               {
        //                                   vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.drawbarvehicle;
        //                               }
        //                               if (outbound.VehicleType == "semi vehicle")
        //                               {
        //                                   vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.semivehicle;
        //                               }
        //                               if (outbound.VehicleType == "rigid vehicle")
        //                               {
        //                                   vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.rigidvehicle;
        //                               }
        //                               if (outbound.VehicleType == "tracked vehicle")
        //                               {
        //                                   vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.trackedvehicle;
        //                               }
        //                               if (outbound.VehicleType == "other in line")
        //                               {
        //                                   vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.otherinline;
        //                               }
        //                               if (outbound.VehicleType == "other side by side")
        //                               {
        //                                   vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.othersidebyside;
        //                               }
        //                               //Added - Jan272014 
        //                               if (outbound.VehicleType == "spmt")
        //                               {
        //                                   vssList.ConfigurationType = NotificationXSD.VehicleConfigurationType.spmt;
        //                               }
        //                               //Added - Jan272014 - end
        //                               vstcs.Item = dtss;

        //                               vssList.Configuration = vstcs;

        //                               vssvslp.VehicleSummary = vssList;

        //                               vssvslpList[recordcount] = vssvslp;

        //                               recordcount++;
        //                               totaAlternativeId++;
        //                           }
        //                       }

        //                       vss.VehicleSummaryListPosition = vssvslpList;
        //                       #endregion
        //                   }

        //                   prp.Vehicles = vss;

        //               }

        //               //=============================================================================================================================
        //               // Date 11 Feb 2015 Ticket No 3537
        //               DrivingInstructionModel DrivingInstructionInfo = GetDrivingInstructionStructures(NotificationID);

        //               #region "Driving Instructions"
        //               int incdoccaution = 0;
        //               incdoccaution = records.GetShortOrDefault("include_dock_caution");
        //               prp.DrivingInstructions = getDrivingDetails(DrivingInstructionInfo, incdoccaution);
        //               #endregion

        //               //=============================================================================================================================

        //               #region Roads
        //               prp.Roads = getRoadDetails(NotificationID);
        //               #endregion

        //               #region "Structures"
        //               AffectedStructuresStructure affStructure = new AffectedStructuresStructure();
        //               RoutePartName = records.GetStringOrDefault("PART_NAME");
        //               affStructure = GetStructureDataDetails(NotificationID, RoutePartName);
        //               prp.Structures = affStructure;
        //               #endregion

        //               #region Distance
        //               VariableMetricImperialDistancePairStructure vmdps = new VariableMetricImperialDistancePairStructure();

        //               VariableMetricDistanceStructure vmds = new VariableMetricDistanceStructure();
        //               vmds.Item = Convert.ToString(TotalDistanceMetric);
        //               vmdps.Metric = vmds;

        //               VariableImperialDistanceStructure vids = new VariableImperialDistanceStructure();
        //               vids.Item = Convert.ToString(TotalDistanceImperial);
        //               vmdps.Imperial = vids;

        //               prp.Distance = vmdps;
        //               #endregion

        //               prps.RoadPart = prp;
        //               instance.RoutePart = prps;

        //               string routedescImperial = getRouteDetailsImperial(NotificationID);

        //               if (routedescImperial == string.Empty)
        //               {
        //                   routedescImperial = "\u2002";
        //               }

        //               instance.RouteImperial = routedescImperial;

        //               string routedesc = getRouteDetails(NotificationID);

        //               if (routedesc == string.Empty)
        //               {
        //                   routedesc = "\u2002";
        //               }

        //               instance.Route = routedesc;

        //               legNumber++;
        //           }
        //    );
        //    return rpsrplp;
        //}
        #endregion

        #region GetOutboundNotificationDetailsForNotification
        /// <summary>
        /// get notification detail based on notification id
        /// </summary>
        /// <param name="NotificationID">notification id</param>
        /// <param name="isHaulier">is haulier</param>
        /// <returns></returns>
        /*public static OutboundNotificationStructure GetOutboundNotificationDetailsForNotification(long NotificationID)
        {
            OutboundNotificationStructure odns = new OutboundNotificationStructure();
            try
            {
                SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
                    odns,
                    UserSchema.Portal + ".GET_OUTBOUND_DETAILS",
                    parameter =>
                    {
                        parameter.AddWithValue("P_NOTIFICATION_ID", NotificationID, OracleDbType.Varchar2, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("P_CONTACT_ID", 0, OracleDbType.Long, ParameterDirectionWrap.Input, 32767);
                        parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
                    },
                       (records, instance) =>
                       {
                           NotificationXSD.ESDALReferenceNumberStructure esdalrefnostru = new NotificationXSD.ESDALReferenceNumberStructure();
                           NotificationVR1InformationStructure vr1Info = new NotificationVR1InformationStructure();
                           VR1NumbersStructure vr1Str = new VR1NumbersStructure();

                           esdalrefnostru.Mnemonic = records.GetStringOrDefault("HAULIER_MNEMONIC");
                           string[] hauMnemonicArr = records.GetStringOrDefault("NOTIFICATION_CODE").Split("/".ToCharArray());
                           if (hauMnemonicArr.Length > 0)
                           {
                               esdalrefnostru.MovementProjectNumber = hauMnemonicArr[1];
                           }
                           instance.NotificationOnEscort = records.GetStringOrDefault("NOTESONESCORT");

                           NotificationXSD.MovementVersionNumberStructure movvernostru = new NotificationXSD.MovementVersionNumberStructure();
                           movvernostru.Value = records.GetShortOrDefault("VERSION_NO");
                           esdalrefnostru.MovementVersion = movvernostru;

                           esdalrefnostru.NotificationNumber = records.GetShortOrDefault("NOTIFICATION_NO");
                           esdalrefnostru.NotificationNumberSpecified = true;

                           instance.ESDALReferenceNumber = esdalrefnostru;

                           // Start for #3846
                           vr1Str.Scottish = records.GetStringOrDefault("VR1_NUMBER");
                           vr1Info.Numbers = vr1Str;
                           instance.VR1Information = vr1Info;
                           // End for #3846

                           #region Vehicle Classification
                           if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.VehicleSpecialOrder)
                           {
                               instance.Classification = MovementClassificationType.specialorder;
                           }
                           else if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.SpecialOrder)
                           {
                               instance.Classification = MovementClassificationType.specialorder;
                           }
                           else if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.StgoailCat1)
                           {
                               instance.Classification = MovementClassificationType.stgoailcat1;
                           }
                           else if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.StgoailCat2)
                           {
                               instance.Classification = MovementClassificationType.stgoailcat2;
                           }
                           else if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.StgoailCat3)
                           {
                               instance.Classification = MovementClassificationType.stgoailcat3;
                           }
                           else if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.StgoMobileCraneCata)
                           {
                               instance.Classification = MovementClassificationType.stgomobilecranecata;
                           }
                           else if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.StgoMobileCraneCatb)
                           {
                               instance.Classification = MovementClassificationType.stgomobilecranecatb;
                           }
                           else if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.StgoMobileCraneCatc)
                           {
                               instance.Classification = MovementClassificationType.stgomobilecranecatc;
                           }
                           else if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.StgoRoadRecoveryVehicle)
                           {
                               instance.Classification = MovementClassificationType.stgoroadrecoveryvehicle;
                           }
                           else if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.WheeledConstructionAndUse)
                           {
                               instance.Classification = MovementClassificationType.wheeledconstructionanduse;
                           }
                           else if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.Tracked)
                           {
                               instance.Classification = MovementClassificationType.tracked;
                           }
                           else if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.StgoEngineeringPlantWheeled)
                           {
                               instance.Classification = MovementClassificationType.stgoengineeringplantwheeled;
                           }
                           else if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.StgoEngineeringPlantTracked)
                           {
                               instance.Classification = MovementClassificationType.stgoengineeringplanttracked;
                           }
                           else if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.StgoCat1EngineeringPlantWheeled)
                           {
                               instance.Classification = MovementClassificationType.StgoCat1EngineeringPlantWheeled;
                           }
                           else if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.StgoCat2EngineeringPlantWheeled)
                           {
                               instance.Classification = MovementClassificationType.StgoCat2EngineeringPlantWheeled;
                           }
                           else if (records.GetInt32OrDefault("vehicle_classification") == (int)VehicleEnums.VehicleClassificationType.StgoCat3EngineeringPlantWheeled)
                           {
                               instance.Classification = MovementClassificationType.StgoCat3EngineeringPlantWheeled;
                           }
                           #endregion

                           string specialOrderESDALRefNo = esdalrefnostru.Mnemonic + "/" + esdalrefnostru.MovementProjectNumber + "/S" + movvernostru.Value;
                           SignedOrderSummaryStructure soss;
                           soss = GetSpecialOrderNo(specialOrderESDALRefNo);

                           instance.DftReference = soss.OrderNumber == null ? records.GetStringOrDefault("SO_NUMBERS") : soss.OrderNumber;

                           instance.ClientName = records.GetStringOrDefault("Client_Descr");

                           instance.JobFileReference = records.GetStringOrDefault("HA_JOB_FILE_REF");

                           #region Haulier Details
                           NotificationXSD.HaulierDetailsStructure hds = new NotificationXSD.HaulierDetailsStructure();
                           instance.HauliersReference = records.GetStringOrDefault("hauliers_ref");
                           hds.HaulierContact = records.GetStringOrDefault("HAULIER_CONTACT");
                           hds.HaulierName = records.GetStringOrDefault("HAULIER_NAME");
                           hds.TelephoneNumber = records.GetStringOrDefault("HAULIER_TEL_NO");
                           hds.FaxNumber = records.GetStringOrDefault("HAULIER_Fax_NO");
                           hds.EmailAddress = records.GetStringOrDefault("HAULIER_Email");
                           hds.OrganisationId = records.GetLongOrDefault("organisation_id");
                           hds.OrganisationIdSpecified = records.GetLongOrDefault("organisation_id") > 0;
                           hds.Licence = records.GetStringOrDefault("HAULIER_LICENCE_NO");

                           NotificationXSD.AddressStructure has = new NotificationXSD.AddressStructure();

                           string[] Addstru = new string[5];
                           Addstru[0] = records.GetStringOrDefault("HAULIER_ADDRESS_1");
                           Addstru[1] = records.GetStringOrDefault("HAULIER_ADDRESS_2");
                           Addstru[2] = records.GetStringOrDefault("HAULIER_ADDRESS_3");
                           Addstru[3] = records.GetStringOrDefault("HAULIER_ADDRESS_4");
                           Addstru[4] = records.GetStringOrDefault("HAULIER_ADDRESS_5");
                           has.Line = Addstru;
                           has.PostCode = records.GetStringOrDefault("haulier_post_code");
                           int country = records.GetInt32OrDefault("COUNTRY");
                           if (country == (int)Country.england)
                           {
                               has.Country = NotificationXSD.CountryType.england;
                               has.CountrySpecified = true;
                           }
                           else if (country == (int)Country.northernireland)
                           {
                               has.Country = NotificationXSD.CountryType.northernireland;
                               has.CountrySpecified = true;
                           }
                           else if (country == (int)Country.scotland)
                           {
                               has.Country = NotificationXSD.CountryType.scotland;
                               has.CountrySpecified = true;
                           }
                           else if (country == (int)Country.wales)
                           {
                               has.Country = NotificationXSD.CountryType.wales;
                               has.CountrySpecified = true;
                           }
                           hds.HaulierAddress = has;
                           instance.HaulierDetails = hds;
                           #endregion

                           #region JourneyFromToSummary
                           JourneyFromToSummaryStructure jftss = new JourneyFromToSummaryStructure();

                           jftss.From = records.GetStringOrDefault("FROM_DESCR");

                           jftss.To = records.GetStringOrDefault("TO_DESCR");

                           instance.JourneyFromToSummary = jftss;
                           #endregion

                           #region JourneyFromTo
                           JourneyFromToStructure jfts = new JourneyFromToStructure();
                           jfts.From = records.GetStringOrDefault("FROM_DESCR");

                           jfts.To = records.GetStringOrDefault("TO_DESCR");

                           instance.JourneyFromTo = jfts;
                           #endregion

                           #region JourneyTiming
                           JourneyTimingStructure jts = new JourneyTimingStructure();
                           jts.FirstMoveDate = records.GetDateTimeOrDefault("movement_start_date");
                           jts.LastMoveDate = records.GetDateTimeOrDefault("movement_end_date");
                           jts.LastMoveDateSpecified = true;
                           jts.StartTime = Convert.ToString(jts.FirstMoveDate.TimeOfDay);
                           jts.EndTime = Convert.ToString(jts.LastMoveDate.TimeOfDay);
                           instance.JourneyTiming = jts;
                           #endregion

                           #region LoadDetails
                           LoadDetailsStructure lds = new LoadDetailsStructure();
                           lds.Description = records.GetStringOrDefault("load_descr");
                           lds.TotalMoves = Convert.ToString(records.GetInt16OrDefault("NO_OF_MOVES"));
                           lds.MaxPiecesPerMove = Convert.ToString(records.GetInt32OrDefault("max_pieces_per_move"));
                           lds.MaxPiecesPerMoveSpecified = true;
                           instance.LoadDetails = lds;
                           instance.NotificationNotesFromHaulier = records.GetStringOrDefault("HAUL_NOTES");
                           InboundNotificationStructure inBoundStructure = new InboundNotificationStructure();
                           inBoundStructure.OnBehalfOf = records.GetStringOrDefault("ON_BEHALF_OF");
                           instance.OnBehalfOf = inBoundStructure.OnBehalfOf;
                           #endregion

                           #region Indemnity Information

                           IndemnityStructure indemnityDetails = new IndemnityStructure();

                           bool isIndemnityPresent = records.GetInt16OrDefault("INDEMNITY_CONFIRMATION") > 0;

                           short totalMoves = 0;

                           if (lds.TotalMoves != string.Empty)
                           {
                               totalMoves = Convert.ToInt16(lds.TotalMoves);
                           }
                           indemnityDetails.MultipleMoves = totalMoves > 1;

                           indemnityDetails.Confirmed = isIndemnityPresent;

                           indemnityDetails.OnBehalfOf = instance.OnBehalfOf;

                           indemnityDetails.Haulier = records.GetStringOrDefault("HAULIER_CONTACT");

                           indemnityDetails.SignedDate = DateTime.Today;

                           indemnityDetails.Signatory = records.GetStringOrDefault("HAULIER_CONTACT");

                           MovementTimingStructure movementStructure = new MovementTimingStructure();
                           MovementTimingStructureMovementDateRange movementTiming = new MovementTimingStructureMovementDateRange();

                           movementTiming.FromDate = jts.FirstMoveDate;
                           movementTiming.ToDate = jts.LastMoveDate;

                           movementStructure.Item = movementTiming;

                           indemnityDetails.Timing = movementStructure;

                           if (!isIndemnityPresent)
                               instance.IndemnityConfirmation = null;
                           else
                               instance.IndemnityConfirmation = indemnityDetails;

                           #endregion

                           instance.SentDateTime = records.GetDateTimeOrEmpty("notification_date");

                           #region SOInformation
                           try
                           {
                               instance.SOInformation = GetSODetail(specialOrderESDALRefNo);
                           }
                           catch { }

                           if (instance.SOInformation != null && instance.SOInformation.Summary != null && instance.SOInformation.Summary[0].CurrentOrder != null && instance.SOInformation.Summary[0].CurrentOrder.OrderNumber != null)
                           {
                               instance.JobFileReference = instance.SOInformation.Summary[0].CurrentOrder.HAJobRefNumber;
                           }

                           #endregion

                           #region Recipients
                           instance.Recipients = GetRecipientContactStructure(NotificationID).ToArray();
                           #endregion

                           #region RouteParts
                           List<RoutePartsStructureRoutePartListPosition> rpsrplpList = new List<RoutePartsStructureRoutePartListPosition>();
                           rpsrplpList = GetRouteParts(NotificationID);
                           instance.RouteParts = rpsrplpList.ToArray();
                           #endregion

                           instance.OldNotificationID = Convert.ToInt64(records.GetDecimalOrDefault("old_noti"));
                           instance.OrganisationName = records.GetStringOrDefault("orgname");
                       });
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - GetOutboundNotificationDetailsForNotification,Exception:" + ex);
            }
            return odns;
        }*/
        #endregion

        #region GetNotificationDetails
        /// <summary>
        /// get notification details
        /// </summary>
        /// <param name="notificationID">notification id</param>
        /// <returns></returns>
        //public static OutboundDocuments GetNotificationDetails(int notificationID)
        //{
        //    OutboundDocuments outbounddocs = new OutboundDocuments();
        //    try
        //    {

        //        outbounddocs.ContactID = 0;
        //        outbounddocs.OrganisationID = 0;

        //        SafeProcedure.DBProvider.Oracle.ExecuteAndHydrateInstance(
        //            outbounddocs,
        //            UserSchema.Portal + ".GET_NOTIFICATION_DETAILS",
        //            parameter =>
        //            {
        //                parameter.AddWithValue("P_NOTIFICATION_ID", notificationID, OracleDbType.Int32, ParameterDirectionWrap.Input, 32767);
        //                parameter.AddWithValue("p_ResultSet", null, OracleDbType.RefCursor, ParameterDirectionWrap.Output);
        //            },
        //             (records, instance) =>
        //             {
        //                 instance.OrganisationID = records.GetLongOrDefault("organisation_id");
        //                 instance.EsdalReference = records.GetStringOrDefault("notification_code");
        //             }
        //            );
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"{ConfigurationManager.AppSettings["Instance"]} - NotificationDocument/GetNotificationDetails, Exception: {ex}");

        //    }
        //    return outbounddocs;
        //}
        #endregion

        #endregion
    }
}