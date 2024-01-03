using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.LoggingAndReporting.Models
{
    //Dependency - Vehicle Configuration in Vehicle and Fleet
    public class MovementActionIdentifiers
    {


        public long haulierID { get; set; }
        public string hauliername { get; set; }// The haulier name which is changed by admin.
        public string fail_to_send_user { get; set; }//The fail_to_send_user is used, if the document is failed to reach.
        public string esdalRef { get; set; }//This is for Movement version
        public string notifCode { get; set; }//This is for Notification code
        public string Org_name_sender { get; set; }//This for Sender's Org.
        public string Sender_Contact_Name { get; set; }//This is for Org Sender's contact name
        public string Org_name_receiver { get; set; }//This for Reveiver's Org.
        public string Reciver_Contact_Name { get; set; }//This is for Org Receiver's contact name
        public string fail_to_send_org { get; set; }//This is for showing error msg for delegation failure
        public long org_id { get; set; }
        public string Title { get; set; }
        public long CONTACT_ID { get; set; }//This is for delete contact users
        public long org_id_reciver { get; set; }//distribution status//to get to org_id
        public long contact_id_reciver { get; set; }//distribution status//to get to contact_id        
        public string contactName { get; set; }//This is for new contact user
        //public string adminName { get; set; }
        public long CollabStatus { get; set; }//This is for collaboration status showing,it will take long value.
        public long TransID { get; set; }//This is for transmission ID
        public string TransDocType { get; set; }//This is for showing transmission doc type
        public DateTime Date_Time { get; set; }//Date Time for showing Day Month Time.
        public string TransErrorMsg { get; set; }//This is for showing transmission delivery failure error msg
        public string FullName { get; set; }
        public string DocType { get; set; }//This is used for showing which order is having appliction.
        public int itemTypeno { get; set; }//This is for how many items are daily digest sent.
        public string manually_added_cont_name { get; set; }//manual party added for showing contact name
        public string manually_added_org_name { get; set; }//manual party added for showing org name
        public string applicant_name { get; set; }//
        public string allocateUser { get; set; }
        public string AllocateToName { get; set; }
        public string reallocateUser { get; set; }
        public int mov_ver { get; set; }
        public int rev_no { get; set; }
        public string proj_status { get; set; }
        public string Spec_Order_No { get; set; }
        public bool revise_by_sort { get; set; }
        public string job_file_ref { get; set; }
        public int vr1_gen_num { get; set; }
        public int cand_ver_no { get; set; }
        public string checkername { get; set; }
        public long App_ID { get; set; }
        public string Org_name { get; set; }
        public string UserName { get; set; }
        public int User_ID { get; set; }
        public int revision_id { get; set; }
        public int new_revision_id { get; set; }
        public int vehicle_id { get; set; }
        public int vehicle_id_no { get; set; }
        public int component_id { get; set; }
        public int component_id_no { get; set; }//The registration id no of so or vr1 component
        public long fleet_veh_id { get; set; }// The haulier fleet vehicle id.
        public long fleet_comp_id { get; set; }// The haulier fleet component id.
        public string fleet_veh_name { get; set; }// The haulier fleet vehicle name.
        public string fleet_comp_name { get; set; }// The haulier fleet component name.
        public int fleet_comp_id_no { get; set; }//The registration id no of fleet component
        public int fleet_vhcl_id_no { get; set; }//The registration id no
        public int route_id { get; set; }
        public int ret_route_id { get; set; }
        public int prev_mov_route_id { get; set; }
        public int lib_route_id { get; set; }
        public int new_lib_route_id { get; set; }
        public int Edit_Flag { get; set; }
        public contactPreference contactPreference { get; set; }//This is used for checking outbound document transaction through which service

        public TransmissionModelFilter TransmissionModelFilter { get; set; }//This is used for tracking transmission status.

        public TransmissionModel TransmissionModel { get; set; }//This will check is it INBOX or not

        public MovementnActionType movementActionType { get; set; }

        public SysEventType systemEventType { get; set; }

        public MovementActionIdentifiers()
        {
            contactPreference = new contactPreference();
            TransmissionModelFilter = new TransmissionModelFilter();
            TransmissionModel = new TransmissionModel();
            movementActionType = new MovementnActionType();
            systemEventType = new SysEventType();
            esdalRef = null;
            VR1App = "false";
        }

        public string hauliermnemonic { get; set; }

        public long ProjectId { get; set; }

        public string VR1App { get; set; }

        public long AnalysisId { get; set; }

        public string RouteType { get; set; }

        public string Routename { get; set; }

        public long CandRouteId { get; set; }

        public long CandVersionId { get; set; }

        public string Status { get; set; }

        //Notification
        public long NotificationID { get; set; }
        public long NewNotificationID { get; set; }
        public string ContentRefNo { get; set; }
        public int prev_mov_veh_id { get; set; }

        public int organisationId { get; set; }
    }   

}