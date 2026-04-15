namespace ContrackAPI
{

    public static class LoginType
    {
        public const string Hub = "1";
        public const string Agency = "2";
    }
    public static class PageKeys
    {
        public const string Container = "Container";
        public const string Voyage = "Voyage";
        public const string Booking = "Booking";
        public const string Quotation = "Quotation";
        public const string Dashboard = "Dashboard";
    }
    public static class ChatType
    {
        public const string Quotation = "CONT-QT";
    }

    public static class QuoteStatus
    {
        public const int Draft = 1;
        public const int Awaiting_HOD_approval = 2;
        public const int Approved = 3;
        public const int HOD_Rejected = 4;
    }
    public static class UserRols
    {
        public const int Admin = 1;
        public const int Purchaser = 2;
        public const int Approver = 3;
        public const int Approver_Tech = 4;
        public const int Approver_HOD = 5;
        public const int Approver_Finance = 6;
        public const int Approver_Advanced = 7;
    }
    public static class ApproveStatus
    {
        public const int SendForApproval = 0;
        public const int Approved = 1;
        public const int Rejected = 2;
        public const int AutoApproved = 3;
    }

    public static class StatusEnum
    {

        public const int Booking = 105;
        public const int Quotation = 106;
    }
    public static class AttachmentFileTypes
    {
        public const int PurchaseIntent = 1;
        public const int PurchaseIntentDetail = 2;
        public const int RFQ = 3;
        public const int RFQDetail = 4;
        public const int Comparison = 5;
        public const int Quotation = 6;
        public const int QuotationDetail = 7;
        public const int PurchaseOrder = 8;
        public const int PurchaseOrderDetail = 9;
        public const int VendorInvoiceMain = 10;
        public const int VendorInvoice = 11;
        public const int Batch = 12;
        public const int Vendor = 13;

        public static readonly Dictionary<int, string> TypeNames = new Dictionary<int, string>
        {
            { PurchaseIntent, nameof(PurchaseIntent) },
            { PurchaseIntentDetail, nameof(PurchaseIntentDetail) },
            { RFQ, nameof(RFQ) },
            { RFQDetail, nameof(RFQDetail) },
            { Comparison, nameof(Comparison) },
            { Quotation, nameof(Quotation) },
            { QuotationDetail, nameof(QuotationDetail) },
            { PurchaseOrder, nameof(PurchaseOrder) },
            { PurchaseOrderDetail, nameof(PurchaseOrderDetail) },
            { VendorInvoiceMain, nameof(VendorInvoiceMain) },
            { VendorInvoice, nameof(VendorInvoice) },
            { Batch, nameof(Batch) },
            { Vendor, nameof(Vendor) },
        };

    }
}