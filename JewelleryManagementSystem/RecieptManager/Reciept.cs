﻿using JewelleryManagementSystem.CustomerManagement.Model;
using JewelleryManagementSystem.ModelUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace JewelleryManagementSystem.RecieptManager
{
    public interface IReciept
    {
        string ReceiptID { get; }
        string CustomerName { get; }
        List<IJewellery> PurchasedJewelleries { get; }
        float TotalAmount { get; }
        float OldJewelleryAmount { get; }
        float RemainingAmount { get; }
        float DiscountGiven { get; }
        string OrderStatus { get; }
        string OwnerSignature { get; }
    }
    public interface IGSTReciept : IReciept
   {
       float GstAmount { get; }
    }
    [DataContract]
    public class Reciept : CommonComponent, IReciept
    {
        private string _receiptID;
        private string _customerName;
        private List<IJewellery> _purchasedJewelleries;
        private float _totalAmount;
        private float _remainingAmount;
        private float _discountGiven = 0;
        private string _orderStatus;
        private float _oldJewelleriesAmount = 0f;
        public Reciept(string customerName, string orderID, string customerID)
        {
            _receiptID = string.Concat(orderID, customerID);
            _customerName = customerName;
        }
        [DataMember]
        public string ReceiptID
        {
            get => _receiptID;
            private set => _receiptID = value;
        }
        [DataMember]
        public string CustomerName
        {
            get => _customerName;
            private set => _customerName = value;
        }
        [DataMember]
        public List<IJewellery> PurchasedJewelleries
        {
            get => _purchasedJewelleries;
            set => _purchasedJewelleries = value;
        }
        [DataMember]
        public float TotalAmount
        {
            get => _totalAmount;
            set => _totalAmount = value;
        }      
        [DataMember]
        public float OldJewelleryAmount
        {
            get => _oldJewelleriesAmount;
            set => _oldJewelleriesAmount = value;
        }
        [DataMember]
        public float DiscountGiven
        {
            get => _discountGiven;
            set => _discountGiven = value;
        }
        [DataMember]
        public float RemainingAmount
        {
            get => _remainingAmount;
            set => _remainingAmount = value;
        }        
        [DataMember]
        public string OrderStatus
        {
            get => _orderStatus;
            set => _orderStatus = value;
        }
        [IgnoreDataMember]
        public string OwnerSignature => ProductInformation.Instance.OwenrSignature;
    }
    [DataContract]
    public class GSTReciept : Reciept, IGSTReciept
    {
        private float _gstAmount;
        public GSTReciept(string customerName, string orderID, string customerID) : base(customerName, orderID, customerID)
        {
        }
        [DataMember]
        public float GstAmount
        {
            get => _gstAmount;
            set => _gstAmount = value;
        }
    }
    public class RecieptGenerator
    {
        public static string CreateReciept(IReciept receipt)
        {
            StringBuilder html = new StringBuilder();

            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine("<style>");
            //  html.AppendLine("@page { size: A4; margin: 20mm; }"); // Sets the page size to A4 with margins
            html.AppendLine("body { font-family: Arial, sans-serif; margin: 0; padding: 0; }");
            html.AppendLine(".receipt { width: 100%; box-sizing: border-box; margin: auto; border: 2px solid darkorange; padding: 20px; }");
            html.AppendLine(".header { text-align: center; font-size: 30px; font-weight: bold; margin-bottom: 10px;  color: darkorange; padding: 10px; }");
            html.AppendLine(".details, .summary { margin: 20px 0; }");
            html.AppendLine("table { width: 100%; border-collapse: collapse; margin-bottom: 20px; }");
            html.AppendLine("th, td { border: 1px solid orange; padding: 8px; text-align: left; }");
            html.AppendLine("th { background-color: #f2f2f2; }");
            html.AppendLine(".footer { text-align: center; font-size: 14px; margin-top: 20px; }");
            html.AppendLine("@media print {");
            html.AppendLine("  body { margin: 0; box-shadow: none; }"); // Removes margin when printing
            html.AppendLine("  .receipt { margin: 0; page-break-inside: avoid; }"); // Avoids breaking the receipt content
            html.AppendLine("  table, th, td { font-size: 12px; }"); // Adjusts table font size for print
            html.AppendLine("}");
            html.AppendLine("</style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");

            // Receipt Container
            html.AppendLine("<div class='receipt'>");

            // Header Section
            html.AppendLine($"<div class='header'>{ProductInformation.Instance.ShopName}</div>");
            html.AppendLine($"<div font-size: 12px; >Shop Address<br>{ProductInformation.Instance.Address}<br>{ProductInformation.Instance.PhoneNumber}</div>");
            html.AppendLine($"<div style='text-align:right;  font-size: 14px; margin-top:10px;'>DATE: {DateTime.Now.ToShortDateString()}<br>RECEIPT NO.: {receipt.ReceiptID}</div>");

            // Bill To and Ship To
            html.AppendLine("<div class='details'>");
            html.AppendLine("<table>");
            html.AppendLine($"<tr><td><b>Bill To : </b>{receipt.CustomerName}</td><td><b>Status : </b>{receipt.OrderStatus}</td></tr>");
            html.AppendLine("</table>");
            html.AppendLine("</div>");

            // Table for Purchased Items
            html.AppendLine("<table>");
            html.AppendLine("<tr>");
            html.AppendLine("<th>Name</th><th>Gross Weight(Gram)</th><th>Net Weight(Gram)</th><th>Making/Gram</th>");
            html.AppendLine("</tr>");
            foreach (var jewellery in receipt.PurchasedJewelleries)
            {
                if (jewellery is INewJewellery)
                {
                    html.AppendLine("<tr>");
                    html.AppendLine($"<td>{(jewellery as INewJewellery).Ornament.Name}</td>");
                    html.AppendLine($"<td>{(jewellery as INewJewellery).GrossWeight}</td>");
                    html.AppendLine($"<td>{jewellery.NetWeight}</td>");
                    html.AppendLine($"<td>{(jewellery as INewJewellery).MakingChargesPerGram}</td>");
                    html.AppendLine("</tr>");
                }
            }
            html.AppendLine("</table>");

            // Summary Section
            html.AppendLine("<div class='summary'>");
            html.AppendLine("<table>");
            html.AppendLine("<tr><td>TOTAL :</td><td style='text-align:right;'>" + $"Rs. {receipt.TotalAmount}</td></tr>");
            html.AppendLine("<tr><td>Paid Amount :</td><td style='text-align:right;'>" + $"Rs. {receipt.TotalAmount - receipt.RemainingAmount - receipt.DiscountGiven - receipt.OldJewelleryAmount}</td></tr>");
            if (receipt.OldJewelleryAmount != 0)
                html.AppendLine("<tr><td>Old Jewelleries Amount :</td><td style='text-align:right;'>" + $"Rs. {receipt.OldJewelleryAmount}</td></tr>");
            if (receipt.DiscountGiven != 0)
                html.AppendLine("<tr><td>Discount of :</td><td style='text-align:right;'>" + $"Rs. {receipt.DiscountGiven}</td></tr>");
            if (receipt.RemainingAmount != 0)
                html.AppendLine("<tr><td>Remaining Amount :</td><td style='text-align:right;'>" + $"Rs. {receipt.RemainingAmount}</td></tr>");

            html.AppendLine("</table>");
            html.AppendLine("</div>");

            // Footer and Notes
            html.AppendLine($"<div text-align:right;>{ProductInformation.Instance.OwenrSignature}<br><div style='text-align:left; font-size: 12px;'>(This is an auto-generated signature)</div></div>");
            html.AppendLine("<div class='footer'>THANK YOU FOR YOUR VISIT..!</div>");

            // Closing tags
            html.AppendLine("</div>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");

            return html.ToString();

        }
        public static string CreateGSTReciept(IReciept receipt)
        {
            StringBuilder html = new StringBuilder();

            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine("<style>");
            //  html.AppendLine("@page { size: A4; margin: 20mm; }"); // Sets the page size to A4 with margins
            html.AppendLine("body { font-family: Arial, sans-serif; margin: 0; padding: 0; }");
            html.AppendLine(".receipt { width: 100%; box-sizing: border-box; margin: auto; border: 2px solid darkorange; padding: 20px; }");
            html.AppendLine(".header { text-align: center; font-size: 30px; font-weight: bold; margin-bottom: 10px;  color: darkorange; padding: 10px; }");
            html.AppendLine(".details, .summary { margin: 20px 0; }");
            html.AppendLine("table { width: 100%; border-collapse: collapse; margin-bottom: 20px; }");
            html.AppendLine("th, td { border: 1px solid orange; padding: 8px; text-align: left; }");
            html.AppendLine("th { background-color: #f2f2f2; }");
            html.AppendLine(".footer { text-align: center; font-size: 14px; margin-top: 20px; }");
            html.AppendLine("@media print {");
            html.AppendLine("  body { margin: 0; box-shadow: none; }"); // Removes margin when printing
            html.AppendLine("  .receipt { margin: 0; page-break-inside: avoid; }"); // Avoids breaking the receipt content
            html.AppendLine("  table, th, td { font-size: 12px; }"); // Adjusts table font size for print
            html.AppendLine("}");
            html.AppendLine("</style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");

            // Receipt Container
            html.AppendLine("<div class='receipt'>");

            // Header Section
            html.AppendLine($"<div class='header'>{ProductInformation.Instance.ShopName}</div>");
            html.AppendLine($"<div font-size: 12px; >Shop Address<br>{ProductInformation.Instance.Address}<br>{ProductInformation.Instance.PhoneNumber}</div>");
            html.AppendLine($"<div style='text-align:right;  font-size: 14px; margin-top:10px;'>DATE: {DateTime.Now.ToShortDateString()}<br>RECEIPT NO.: {receipt.ReceiptID}</div>");

            // Bill To and Ship To
            html.AppendLine("<div class='details'>");
            html.AppendLine("<table>");
            html.AppendLine($"<tr><td><b>Bill To : </b>{receipt.CustomerName}</td><td><b>Status : </b>{receipt.OrderStatus}</td></tr>");
            html.AppendLine("</table>");
            html.AppendLine("</div>");

            // Table for Purchased Items
            html.AppendLine("<table>");
            html.AppendLine("<tr>");
            html.AppendLine("<th>Name</th><th>Gross Weight(Gram)</th><th>Net Weight(Gram)</th><th>Making/Gram</th>");
            html.AppendLine("</tr>");
            foreach (var jewellery in receipt.PurchasedJewelleries)
            {
                if (jewellery is INewJewellery)
                {
                    html.AppendLine("<tr>");
                    html.AppendLine($"<td>{(jewellery as INewJewellery).Ornament.Name}</td>");
                    html.AppendLine($"<td>{(jewellery as INewJewellery).GrossWeight}</td>");
                    html.AppendLine($"<td>{jewellery.NetWeight}</td>");
                    html.AppendLine($"<td>{(jewellery as INewJewellery).MakingChargesPerGram}</td>");
                    html.AppendLine("</tr>");
                }
            }
            html.AppendLine("</table>");

            // Summary Section
            html.AppendLine("<div class='summary'>");
            html.AppendLine("<table>");
            html.AppendLine("<tr><td>TOTAL(GST Inc.) :</td><td style='text-align:right;'>" + $"Rs. {receipt.TotalAmount + (receipt as IGSTReciept).GstAmount}</td></tr>");
            html.AppendLine("<tr><td>GST :</td><td style='text-align:right;'>" + $"Rs. {(receipt as IGSTReciept).GstAmount}</td></tr>");
            html.AppendLine("<tr><td>Paid Amount :</td><td style='text-align:right;'>" + $"Rs. {receipt.TotalAmount - receipt.RemainingAmount - receipt.DiscountGiven - receipt.OldJewelleryAmount + (receipt as IGSTReciept).GstAmount}</td></tr>");
            if (receipt.OldJewelleryAmount != 0)
                html.AppendLine("<tr><td>Old Jewelleries Amount :</td><td style='text-align:right;'>" + $"Rs. {receipt.OldJewelleryAmount}</td></tr>");
            if (receipt.DiscountGiven != 0)
                html.AppendLine("<tr><td>Discount of :</td><td style='text-align:right;'>" + $"Rs. {receipt.DiscountGiven}</td></tr>");
            if (receipt.RemainingAmount != 0)
                html.AppendLine("<tr><td>Remaining Amount :</td><td style='text-align:right;'>" + $"Rs. {receipt.RemainingAmount}</td></tr>");

            html.AppendLine("</table>");
            html.AppendLine("</div>");

            // Footer and Notes
            html.AppendLine($"<div text-align:right;>{ProductInformation.Instance.OwenrSignature}<br><div style='text-align:left; font-size: 12px;'>(This is an auto-generated signature)</div></div>");
            html.AppendLine("<div class='footer'>THANK YOU FOR YOUR VISIT..!</div>");

            // Closing tags
            html.AppendLine("</div>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");

            return html.ToString();
        }
    }
}
