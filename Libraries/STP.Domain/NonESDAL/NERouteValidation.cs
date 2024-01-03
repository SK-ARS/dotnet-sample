using Newtonsoft.Json;
using STP.Common.Logger;
using STP.Domain.ExternalAPI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace STP.Domain.NonESDAL
{
    public class NERouteValidation
    {
        public class NERouteImport
        {
            public Route Route { get; set; }
            public ValidationError RouteError { get; set; }
        }
        public Dictionary<string, string> ValidationFailure { get; set; }
        public class NERouteImportModel
        {
            public Route Route { get; set; }
            public List<string> RouteNames { get; set; }
            public bool IsDuplicate { get; set; }
        }

        #region Validate NE Route
        public NERouteImport ValidateNeRoute(NERouteImportModel routeImportModel, int routeNum)
        {
            int errorCount = 0;
            StringBuilder validationErrorBuilder = new StringBuilder();
            string errorMsg = ValidationFailure["RouteGeneralError"] + routeNum + "~";
            validationErrorBuilder.Append(errorMsg);
            NERouteImport nERouteImport = new NERouteImport
            {
                Route = new Route()
            };

            #region RouteName
            if (string.IsNullOrWhiteSpace(routeImportModel.Route.RouteName))
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["RouteNameRequired"]);
            }
            else if(routeImportModel.Route.RouteName.Trim().Length > 100)
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["RouteNameSize"]);
            }
            else
                nERouteImport.Route.RouteName = routeImportModel.Route.RouteName.Trim();
            #endregion

            #region Route Description
            if (string.IsNullOrWhiteSpace(routeImportModel.Route.RouteDescription))
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["RouteDescriptionRequired"]);
            }
            else if (routeImportModel.Route.RouteDescription.Trim().Length > 4000)
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["RouteDescrSize"]);
            }
            else
                nERouteImport.Route.RouteDescription = routeImportModel.Route.RouteDescription.Trim();
            #endregion

            #region GPX
            if (string.IsNullOrWhiteSpace(routeImportModel.Route.GPX))
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["RouteGpxRequired"]);
            }
            else if (!CheckGPXValid(routeImportModel.Route.GPX))
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["InvalidRouteGPX"]);
            }
            else
                nERouteImport.Route.GPX = routeImportModel.Route.GPX;
            #endregion

            #region Duplicate RouteName
            if (routeImportModel.IsDuplicate)
            {
                errorCount++;
                validationErrorBuilder.Append(errorCount + ValidationFailure["DuplicateRouteName"]);
            }
            #endregion

            #region Error Count
            if (errorCount > 0)
            {
                string ErrorMessage = validationErrorBuilder.ToString().Trim();
                List<string> routeDetailsErrorList = ErrorMessage.Split('~').ToList();
                routeDetailsErrorList.RemoveAt(routeDetailsErrorList.Count - 1);
                ValidationError validationError = new ValidationError
                {
                    ErrorCount = errorCount,
                    ErrorMessage = string.Join("~", routeDetailsErrorList),
                    ErrorList = routeDetailsErrorList
                };
                nERouteImport.RouteError = validationError;
            }
            #endregion

            return nERouteImport;
        }

        #endregion

        #region Route GPX Validation Private function
        private bool CheckGPXValid(string gpxInput)
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(gpxType));
                using (XmlReader reader = XmlReader.Create(new StringReader(gpxInput)))
                {
                    gpxType route = (gpxType)deserializer.Deserialize(reader);
                    return route != null;
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, $"CheckGPXValid : Invalid GPX Exception: { ex.Message } StackTrace { ex.StackTrace}");
                return false;
            }
        }
        #endregion
    }
}
