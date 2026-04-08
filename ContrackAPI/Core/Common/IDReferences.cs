using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContrackAPI
{
    public static class IDReferences
    {
        public static string GetCustomerTypeName(int customertype)
        {
            switch (customertype)
            {
                case 1:
                    return "Shipper";
                case 2:
                    return "Consignee";
                case 3:
                    return "Forwarder";
                default:
                    return "Unknown";
            }
        }
        public static string GetPackageType(int packagetype)
        {
            switch (packagetype)
            {
                case 1:
                    return "Wooden Cases";
                case 2:
                    return "Carton Boxes";
                case 3:
                    return "Palletized";
                default:
                    return "";
            }
        }

        public static string GetOwnership(int customertype)
        {
            switch (customertype)
            {
                case 1:
                    return "Owned";
                case 2:
                    return "Leased";
                case 3:
                    return "SOC";
                case 4:
                    return "COC";
                default:
                    return "";
            }
        }
        public static string GetTransferType(int Transfertype)
        {
            switch (Transfertype)
            {
                case 1:
                    return "Door to Door";
                case 2:
                    return "Door to Port";
                case 3:
                    return "Port to Port";
                case 4:
                    return "Port to Door";

                default:
                    return "";
            }
        }
        public static string GetTransferTypeIcon(int Transfertype)
        {
            switch (Transfertype)
            {
                case 1:
                    return "<span class=\"material-symbols-outlined text-lg\">door_front</span>";
                case 2:
                    return "<span class=\"material-symbols-outlined text-lg\">anchor</span>";
                case 3:
                    return "<span class=\"material-symbols-outlined text-lg\">sailing</span>";
                case 4:
                    return "<span class=\"material-symbols-outlined text-lg\">apartment</span>";

                default:
                    return "";
            }
        }
        public static string GetFullOREmpty(string EmptyFull)
        {
            switch (EmptyFull)
            {
                case "E":
                    return "Empty";
                case "F":
                    return "Full";
                default:
                    return "";
            }
        }
        public static string GetFullOREmptyHTML(string EmptyFull)
        {
            switch (EmptyFull)
            {
                case "E":
                    return "<span class=\"badge-styling yellow flex gap-1\">Empty</span>";
                case "F":
                    return "<span class=\"badge-styling blue flex gap-1\">Full</span>";
                default:
                    return "";
            }
        }
    }
}