using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STP.Domain;

namespace STP.Common.SortHelper
{
    public class CheckingProcess
    {
        //New sort actions
        public static SortActions SortSOActions(SortActions model, long UserId, long PlannerId, long CheckerId, string SORTAllocateJob, int AppStatus, int WorkingStatus, decimal IsDistrubuted, int CandidateId, int MovVersionNo, int EnterdbySort)
        {
            //Owner actions
            if (PlannerId == UserId)
            {
                switch (AppStatus)
                {
                    case 307002: //Work In Progress
                    case 307012: //Agreement work in progress
                        if (EnterdbySort == 1)
                            model.CreateApplication = true;
                        model.EditApplication = true;
                        //model.CreateCandidateVersion = true;
                        if (CandidateId != 0)
                        {
                            model.CreateCandidateVersion = true;
                            if (WorkingStatus != 301002)
                            {
                                if (WorkingStatus != 301003)
                                {
                                    model.CreateCandidateVersion = true;
                                    model.EditCandidateVersion = true;
                                }
                                model.PerformRouteAssesment = true;
                                model.EditAffectedParties = true;
                            }
                            model.SendforChecking = true;
                            if (WorkingStatus == 301003)//Add one more condition
                            {
                                model.CreateMovementVersion = true;
                                if ((int)IsDistrubuted == 0 && MovVersionNo != 0)
                                {
                                    model.DistributeMovements = true;
                                }
                            }
                        }
                        break;
                    case 307003://Proposed
                    case 307004://Reproposed
                    case 307006: //Agreed revised
                        model.CreateApplication = true;
                        model.EditApplication = true;
                        model.CreateCandidateVersion = true;
                        model.SetCollabration = true;
                        model.Agree = true;
                        model.Retransmit = true;
                        break;
                    case 307005: //Agreed 
                    case 307007: //Agreed recleared
                        model.Retransmit = true;
                        model.CreateApplication = true;
                        model.EditApplication = true;
                        model.PerformRouteAssesment = true;
                        model.EditAffectedParties = true;
                        model.UnAgree = true;
                        model.SpecialOrder = true;
                        model.VR1 = true;
                        model.GenerateSODocument = true;
                        model.GenerateVR1Document = true;
                        model.EditNotestoHaulier = true;
                        model.SendForQAChecking = true;

                        if (WorkingStatus != 301006 && WorkingStatus != 301010 && WorkingStatus != 301007)
                        {
                            model.CreateCandidateVersion = true;
                            /*if (WorkingStatus != 301002 || WorkingStatus != 301005)
                            {
                                model.CreateCandidateVersion = true;
                                model.EditCandidateVersion = true;
                            }*/
                            model.SetCollabration = true;
                        }
                        else if (WorkingStatus == 301006)
                        {
                            //model.CreateMovementVersion = true;
                            model.DistributeAgreedRoute = true;
                        }
                        if (WorkingStatus == 301009 || WorkingStatus == 301005 || WorkingStatus == 301007)
                        {
                            model.SendForSignOff = true;
                        }

                        break;
                    case 307011: //Revised
                        model.CreateRevisedApplication = true;
                        if (EnterdbySort == 0)
                            model.Acknowledgement = true;
                        else
                        {
                            if (CandidateId != 0)
                            {
                                model.CreateCandidateVersion = true;
                            }
                            if(WorkingStatus == 301005 || WorkingStatus == 3010008)
                            {
                                model.UnAgree = true;
                            }
                        }
                        break;
                    case 307014: //Planned
                                 // model.AmendmentOrder = true;
                        model.Retransmit = true;
                        break;
                }
            }
            //Checker actions
            model = SortCheckerActions(model, AppStatus, WorkingStatus, UserId, CheckerId);
            //User rights actions
            model = SortUserRightsActions(model, AppStatus, WorkingStatus, SORTAllocateJob, IsDistrubuted, EnterdbySort);

            return model;
        }
        public static bool EditCandidate(long UserId, long PlannerId, long CheckerId, int AppStatus, int WorkingStatus)
        {
            bool status = false;
            if (PlannerId == UserId)
            {
                if (AppStatus == 307002 || AppStatus == 307012)
                {
                    if (WorkingStatus == 0 || WorkingStatus == 301001 || WorkingStatus == 301004)
                    {
                        status = true;
                    }
                }
            }
            if (UserId == CheckerId)
            {
                if (AppStatus == 307002 || AppStatus == 307012)
                {
                    if (WorkingStatus == 301002)
                    {
                        status = true;
                    }
                }
            }
            return status;
        }
        private static SortActions SortCheckerActions(SortActions model, int AppStatus, int WorkingStatus, long UserId, long CheckerId)
        {
            if (UserId == CheckerId)
            {
                if (AppStatus == 307002 || AppStatus == 307012)
                {
                    if (WorkingStatus == 301002)
                    {
                        //model.CreateCandidateVersion = true;
                        model.EditCandidateVersion = true;
                        model.CompleteChecking = true;
                    }
                }
                if (AppStatus == 307005 || AppStatus == 307006 || AppStatus == 307007)
                {
                    if (WorkingStatus == 301008 || WorkingStatus == 301010)
                    {
                        //model.CreateCandidateVersion = true;
                        //model.EditCandidateVersion = true;
                        model.CompleteQAChecking = true;
                    }
                    else if (WorkingStatus == 301005 || WorkingStatus == 301007)
                    {
                        model.SignOff = true;
                    }
                }
            }
            return model;
        }
        private static SortActions SortUserRightsActions(SortActions model, int AppStatus, int WorkingStatus, string SORTAllocateJob, decimal IsDistrubuted, int enteredBySORT = 0)
        {
            if (SORTAllocateJob == "1")
            {                
                switch (AppStatus)
                {
                    case 307001:
                    case 307002:
                    case 307003:
                    case 307004:
                    case 307005:
                    case 307006:
                    case 307007:
                    case 307012:
                    //case 307011: //Ref: HE-4497 & 4854
                    case 0:
                        model.Allocation = true;
                        model.Withdraw = true;                        
                        model.Decline = true;
                        break;
                    case 307011:
                        //if (enteredBySORT != 0)
                        //{
                        //    model.Decline = true;
                        //    model.Withdraw = true;
                        //}
                        model.Allocation = true;
                        break;
                    case 307014:                        
                        model.Allocation = true;
                        break;
                    case 307008:
                        model.Unwithdraw = true;
                        break;
                }
            }
            return model;
        }
        public static SortActions SortVR1Actions(SortActions model, int AppStatus, int WorkingStatus, long UserId, long PlannerId, long CheckerId, string SORTAllocateJob, string SORTCanApproveSignVR1, string VR1Number, int EnterdbySort)
        {
            if (SORTAllocateJob == "1")
            {
                model.Allocation = true;
                model.Withdraw = true;
                model.Decline = true;
                if (AppStatus == 307008)//This is a withdrawn application
                {
                    model.Withdraw = false;
                    model.Unwithdraw = true;
                }
            }
           
           
            if (PlannerId == UserId)
            {
                switch (AppStatus)
                {
                    case 307002:
                        model.SendforChecking = true;
                        if (WorkingStatus == 301003)
                            model.VR1Number = true;
                        break;
                    case 307016:
                        model.VR1Document = true;
                        break;
                    case 307011: //Revised                        
                        if (EnterdbySort == 0)
                            model.Acknowledgement = true;
                        else
                            model.SendforChecking = true;
                        break;
                }
            }
            if (UserId == CheckerId)
            {
                switch (AppStatus)
                {
                    case 307002:
                        if (WorkingStatus == 301002)
                            model.CompleteChecking = true;
                        break;
                }
            }
            if (SORTCanApproveSignVR1 == "1")
            {
                if (AppStatus == 307002 && WorkingStatus == 301003)
                {
                    if (VR1Number != "")
                        model.ApproveVr1 = true;
                }
            }
            return model;
        }
    }

    public class SortActions
    {
        public SortActions()
        {
            this.Allocation = false;
            this.Withdraw = false;
            this.Unwithdraw = false;
            this.Decline = false;
            this.SendforChecking = false;
            this.CompleteChecking = false;
            this.SendForQAChecking = false;
            this.CompleteQAChecking = false;
            this.SendForSignOff = false;
            this.SignOff = false;
            this.Agree = false;
            this.UnAgree = false;
            this.DistributeMovements = false;
            this.AmendmentOrder = false;
            this.ApproveVr1 = false;
            this.VR1Number = false;
            this.VR1Document = false;
            this.CreateCandidateVersion = false;
            this.EditCandidateVersion = false;
            this.PerformRouteAssesment = false;
            this.EditAffectedParties = false;
            this.EditNotestoHaulier = false;
            this.EditApplication = false;
            this.CreateApplication = false;
            this.SetCollabration = false;
            this.SpecialOrder = false;
            this.VR1 = false;
            this.GenerateSODocument = false;
            this.GenerateVR1Document = false;
            this.Retransmit = false;
            this.CreateMovementVersion = false;
            this.CreateRevisedApplication = false;
            this.DistributeAgreedRoute = false;
            this.Acknowledgement = false;
        }
        
        public bool Allocation { get; set; }
        public bool Withdraw { get; set; }
        public bool Unwithdraw { get; set; }
        public bool Decline { get; set; }
        public bool SendforChecking { get; set; }
        public bool SendForQAChecking { get; set; }
        public bool CompleteChecking { get; set; }
        public bool CompleteQAChecking { get; set; }
        public bool SendForSignOff { get; set; }
        public bool SignOff { get; set; }
        public bool Agree { get; set; }
        public bool UnAgree { get; set; }
        public bool DistributeMovements { get; set; }
        public bool AmendmentOrder { get; set; }
        public bool VR1Application { get; set; }
        public bool ApproveVr1 { get; set; }
        public bool VR1Number { get; set; }
        public bool VR1Document { get; set; }
        public bool CreateCandidateVersion { get; set; }
        public bool EditCandidateVersion { get; set; }
        public bool PerformRouteAssesment { get; set; }
        public bool EditAffectedParties { get; set; }
        public bool EditNotestoHaulier { get; set; }
        public bool EditApplication { get; set; }
        public bool CreateApplication { get; set; }
        public bool SetCollabration { get; set; }
        public bool SpecialOrder { get; set; }
        public bool VR1 { get; set; }
        public bool GenerateSODocument { get; set; }
        public bool GenerateVR1Document { get; set; }
        public bool Retransmit { get; set; }
        public bool CreateMovementVersion { get; set; }
        public bool CreateRevisedApplication { get; set; }
        public bool DistributeAgreedRoute { get; set; }
        public bool Acknowledgement { get; set; }
    }
}
