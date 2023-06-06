using Demo1.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo1.UserInfo
{
    public class ParcelInfo
    {
        private static ParcelInfo instance;

        private ParcelInfo() { }
        public static ParcelInfo Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ParcelInfo();
                }
                return instance;
            }
        }

        public string Details { get; private set; }

        public Parcel GetParcelRecord(string ParcelID)
        {
            int parcelID = Convert.ToInt32(ParcelID);
            using (var context = new PBL3_demoEntities())
            {
                var thisParcel = context.Parcels.Where(x => x.parcelID == parcelID).FirstOrDefault();
                if (thisParcel != null)
                {
                    return thisParcel;
                }
                else
                {
                    return null;
                }

            }
        }

        public Parcel GetParcelRecordInt(int parcelID)
        {
            using (var context = new PBL3_demoEntities())
            {
                var thisParcel = context.Parcels.Where(x => x.parcelID == parcelID).FirstOrDefault();
                if (thisParcel != null)
                {
                    return thisParcel;
                }
                else
                {
                    return null;
                }

            }
        }
        public bool IsParcelExist(int parcelID)
        {
            using (var context = new PBL3_demoEntities())
            {
                var thisParcel = context.Parcels.Where(x => x.parcelID == parcelID).FirstOrDefault();
                if (thisParcel != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
        public string GetParcelName(string ParcelID)
        {
            Parcel thisParcel = GetParcelRecord(ParcelID);
            if (thisParcel != null) return thisParcel.parcelName;
            else return "";
        }
        public double GetParcelMass(string ParcelID)
        {

            Parcel thisParcel = GetParcelRecord(ParcelID);
            if (thisParcel != null) return thisParcel.parcelMass;
            else return -1;
        }
        public string GetParcelCurrentWH(string ParcelID)
        {
            Parcel thisParcel = GetParcelRecord(ParcelID);
            if (thisParcel != null) return thisParcel.currentWarehouseID;
            else return "";
        }
        public double GetParcelValue(string ParcelID)
        {
            Parcel thisParcel = GetParcelRecord(ParcelID);
            if (thisParcel != null) return (double)thisParcel.parcelValue;
            else return -1;
        }
        public string GetSCustomerID(string ParcelID)
        {
            Parcel thisParcel = GetParcelRecord(ParcelID);
            if (thisParcel != null) return thisParcel.SCustomerID;
            else return "";
        }
        public string GetCustomerName(string ParcelID, int mode)
        {
            string customerid = "";
            if (mode == 1) customerid = GetSCustomerID(ParcelID);
            else if (mode == 2) customerid = GetRCustomerID(ParcelID);
            string res = "";
            if (customerid != null)
            {
                using (var context = new Model.PBL3_demoEntities())
                {
                    var customerName = context.Customers.FirstOrDefault(x => x.customerID == customerid)?.customerName;
                    res = customerName;

                }
            }
            return res;

        }
        public string GetCustomerPhoneNumber(string ParcelID, int mode)
        {
            string customerid = "";
            if (mode == 1) customerid = GetSCustomerID(ParcelID);
            else if (mode == 2) customerid = GetRCustomerID(ParcelID);
            string res = "";
            if (customerid != null)
            {
                using (var context = new Model.PBL3_demoEntities())
                {
                    var customerphonenumber = context.Customers.FirstOrDefault(x => x.customerID == customerid)?.customerPhoneNumber;
                    res = customerphonenumber;

                }
            }
            return res;

        }
        public string GetRCustomerID(string ParcelID)
        {
            Parcel thisParcel = GetParcelRecord(ParcelID);
            if (thisParcel != null) return thisParcel.RCustomerID;
            else return "";
        }

        public string GetCustomerAddress(string ParcelID, int mode)
        {

            string customerid = "";
            if (mode == 1) customerid = GetSCustomerID(ParcelID);
            else if (mode == 2) customerid = GetRCustomerID(ParcelID);
            string res = "";
            if (customerid != null)
            {

                using (var context = new Model.PBL3_demoEntities())
                {
                    var customeraddress = context.Customers.FirstOrDefault(x => x.customerID == customerid)?.customerLocation;
                    res = customeraddress;

                }
            }
            return res;

        }
        public double GetParcelTotalCost(string ParcelID)
        {
            double res;
            int iParcelID = Convert.ToInt32(ParcelID);
            using (var context = new Model.PBL3_demoEntities())
            {
                var totalcost = context.Invoices.FirstOrDefault(x => x.parcelID == iParcelID)?.cost;
                res = (double)totalcost;
            }
            return res;
        }
        public string GetDetails(string ParcelID)
        {
            string details = null;
            int iParcelID = Convert.ToInt32(ParcelID);
            using (var dbContext = new Model.PBL3_demoEntities())
            {
                var maxRouteID = dbContext.Routes
                    .Where(r => r.parcelID == iParcelID)
                    .Max(r => r.routeID);

                var query = dbContext.Routes
                    .Where(r => r.parcelID == iParcelID && r.routeID == maxRouteID)
                    .Select(r => r.details);

                details = query.FirstOrDefault();
            }

            return details;
        }
        public string GetSCustomerCity(string ParcelID)
        {
            int iParcelID = Convert.ToInt32(ParcelID);
            using (var context = new PBL3_demoEntities())
            {
                var result = context.Customers
                    .Join(context.Parcels,
                        customer => customer.customerID,
                        parcel => parcel.SCustomerID,
                        (customer, parcel) => new { Customer = customer, Parcel = parcel })
                    .Where(joinResult => joinResult.Parcel.SCustomerID == joinResult.Customer.customerID && joinResult.Parcel.parcelID == iParcelID)
                    .Select(joinResult => new
                    {
                        SCustomerLocation = joinResult.Customer.customerLocation,

                    })
                    .FirstOrDefault(); // hoặc SingleOrDefault()

                if (result != null)
                {
                    string scustomerlocation = result.SCustomerLocation;
                    string[] scustomercity = scustomerlocation.Split(',');
                    string res = scustomercity.LastOrDefault()?.Trim();
                    return res;
                }
                else
                {
                    return "";
                }
            }
        }
        public bool GetShippingMethod(string ParcelID)
        {
            Parcel thisParcel = GetParcelRecord(ParcelID);
            if (thisParcel != null) return thisParcel.shippingMethod;
            else return false;
        }
        //Nullable datetime
        public DateTime? GetCreateTime(string ParcelID)
        {
            Parcel thisParcel = GetParcelRecord(ParcelID);
            if (thisParcel != null) return thisParcel.createTime;
            else return null;

        }
        public string GetRCustomerCity(string ParcelID)
        {
            int iParcelID = Convert.ToInt32(ParcelID);
            using (var context = new PBL3_demoEntities())
            {
                var result = context.Customers
                    .Join(context.Parcels,
                        customer => customer.customerID,
                        parcel => parcel.RCustomerID,
                        (customer, parcel) => new { Customer = customer, Parcel = parcel })
                    .Where(joinResult => joinResult.Parcel.RCustomerID == joinResult.Customer.customerID && joinResult.Parcel.parcelID == iParcelID)
                    .Select(joinResult => new
                    {
                        RCustomerLocation = joinResult.Customer.customerLocation,

                    })
                    .FirstOrDefault(); // hoặc SingleOrDefault()

                if (result != null)
                {
                    string rcustomerlocation = result.RCustomerLocation;
                    string[] rcustomercity = rcustomerlocation.Split(',');

                    return rcustomercity.LastOrDefault()?.Trim();
                }
                else
                {
                    return "";
                }
            }
        }


    }
}
