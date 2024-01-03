using System.ComponentModel;

namespace STP.Common.Enums
{
    public enum MovementType
    {
        [Description("Special Order")]
        special_order = 207001,
        [Description("VR1")]
        vr_1 = 207002,
        [Description("Notification")]
        notification = 207003,
        [Description("No Movement")]
        no_movement = 207004
    }

    public enum VSOType
    {
        [Description("soa")]
        soa = 267001,
        [Description("police")]
        police = 267002,
        [Description("soa and police")]
        soapolice =267003
    }
    public enum DocumentDistribute
    {
        [Description("Document Delivery")] doc_delivery = 1,
        [Description("Document Delivered")] doc_delivered = 2,
        [Description("Mail Sending")] trans_delivery = 3,
        [Description("Mail Forwarded")] trans_forwarded = 4,
        [Description("Mail Delivered")] trans_delivered = 5,
        [Description("Mail Sending Failed")] trans_delivery_fail = 6,
        [Description("Document Delivery Failed")] doc_delivery_fail = 7,
        [Description("Inbox Items Start Online Inbox & Mail")] inbox_item_delivered_with_mail = 8,
        [Description("Inbox Items Failed Online Inbox & Mail")] inbox_item_failed_with_mail = 9,
        [Description("Inbox Items Start Online Inbox Only")] inbox_item_delivered_without_mail = 10,
        [Description("Inbox Items Failed Online Inbox Only")] inbox_item_failed_without_mail = 11
    }
    public enum DistibuteStat
    {
        [Description("310004")] initializing = 0,
        [Description("310001")] trans_delivered = 1,
        [Description("310002")] trans_failed = 2,
        [Description("310003")] pending = 3,
        [Description("310005")] sending = 5,
        [Description("310001")] with_mail_inbox_delivered = 6,
        [Description("310002")] with_mail_inbox_failed = 7,
        [Description("310001")] with_mail_retransmit_delivered = 8,
        [Description("310009")] inbox_opened = 9,
        [Description("310001")] without_mail_inbox_delivered = 10,
        [Description("310002")] without_mail_inbox_failed = 11,
        [Description("310002")] with_mail_retransmit_failed = 12
    }
}
