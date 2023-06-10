using System;
using System.Linq;

namespace Demo1.Model
{
    public class AddManager
    {
        

        public AddManager()
        {
            
        }

        public void AddOrUpdateCustomer(string customerID, string customerName, string customerLocation, string customerPhoneNumber)
        {
            using(var context=new PBL3_demoEntities())
            {
                var customer = context.Customers.FirstOrDefault(x => x.customerID == customerID);
                if (customer == null)
                {
                    var newCustomer = new Customer { customerID = customerID, customerName = customerName, customerLocation = customerLocation, customerPhoneNumber = customerPhoneNumber, joinTime = DateTime.Now };
                    context.Customers.Add(newCustomer);
                }
                else
                {
                    if (customer.customerName != customerName || customer.customerLocation != customerLocation || customer.customerPhoneNumber != customerPhoneNumber)
                    {
                        customer.customerName = customerName;
                        customer.customerLocation = customerLocation;
                        customer.customerPhoneNumber = customerPhoneNumber;
                    }
                }

                context.SaveChanges();
            }
           
        }

        public void AddNewParcel(string parcelName, double parcelMass, string parcelSize, double parcelValue, bool isSpec, string RCustomerID, string SCustomerID, bool isFast, bool isCOD, string WarehouseID)
        {
            using (var context = new PBL3_demoEntities())
            {
                
                var lastRow = context.Parcels.OrderByDescending(x => x.parcelID).FirstOrDefault();
                
                var newParcel = new Parcel { parcelID = lastRow != null ? lastRow.parcelID + 1 : 1000000, parcelName = parcelName, parcelMass = parcelMass, parcelSize = parcelSize, parcelValue = parcelValue, type = isSpec, RCustomerID = RCustomerID, SCustomerID = SCustomerID, shippingMethod = isFast, isCOD = isCOD, createTime = DateTime.Now, currentWarehouseID = WarehouseID };
                context.Parcels.Add(newParcel);
                context.SaveChanges();
            }
        }
        public string GetParcelID()
        {
            string ParcelID = "";
            using (var context1 = new Model.PBL3_demoEntities())
            {
                var lastRow = context1.Parcels.OrderByDescending(x => x.parcelID).FirstOrDefault();
                ParcelID = Convert.ToString(Convert.ToInt32(lastRow.parcelID) + 1);
            }
            return ParcelID;
        }
        public string GetNameFromWarehouseID(string warehouseID)
        {
            using (var context = new PBL3_demoEntities())
            {
                var warehouseName = context.Warehouses.Where(x => x.warehouseID == warehouseID).Select(x=> x.warehouseName).FirstOrDefault(); 
                return warehouseName;
            }

        }
        public void AddNewRoute(int parcelID, string relatedWarehouseID)
        {
            using (var context = new PBL3_demoEntities())
            {
                var newRoute = new Route { parcelID = parcelID, relatedWarehouseID = relatedWarehouseID, time = DateTime.Now, details = "Đơn hàng đã được khởi tạo tại " + GetNameFromWarehouseID(relatedWarehouseID) };
                context.Routes.Add(newRoute);
                context.SaveChanges();
            }
        }
        public void CreateInvoice(string parcelID, string SCustomerID, double TotalCost, string startWHID, double shippingFee)
        {
           
            using (var context = new PBL3_demoEntities())
            {
                var lastRow = context.Invoices.OrderByDescending(x => x.invoiceID).FirstOrDefault();
                var newInvoice = new Invoice
                {
                    invoiceID =lastRow!=null?lastRow.invoiceID+1:1,
                    parcelID = Convert.ToInt32(parcelID),
                    customerID = SCustomerID,
                    cost = TotalCost,
                    outputTime = DateTime.Now,
                    shippingFee = shippingFee,
                    startWarehouseID = startWHID
                };

                context.Invoices.Add(newInvoice);
                context.SaveChanges();
            }
          
           
        }
    }
}