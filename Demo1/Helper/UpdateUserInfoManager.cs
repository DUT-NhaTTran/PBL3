using Demo1.Model;
using Demo1.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Demo1.Model
{
    public class UpdateUserInfoManager
    {
        public UpdateUserInfoManager() { }
        public void UpdateInfo(string RoleID, int RolePermission, string UpdateName, string UpdateLocation, string UpdatePhoneNumber)
        {
            using (var context = new PBL3_demoEntities())
            {
                try
                {
                    if (RolePermission == 1)
                    {
                        var res = context.Receptionists.FirstOrDefault(x => x.receptionistID == RoleID);


                        res.receptionistName = UpdateName;
                        res.receptionistLocation = UpdateLocation;
                        res.receptionistPhoneNumber = UpdatePhoneNumber;

                        context.SaveChanges();
                    }
                    if (RolePermission == 2)
                    {
                        var res = context.WarehouseStaffs.FirstOrDefault(x => x.warehouseStaffID == RoleID);


                        res.warehouseStaffName = UpdateName;
                        res.warehouseStaffLocation = UpdateLocation;
                        res.warehouseStaffPhoneNumber = UpdatePhoneNumber;

                        context.SaveChanges();
                    }
                    if (RolePermission == 3)
                    {
                        var res = context.WarehouseManagers.FirstOrDefault(x => x.warehouseManagerID == RoleID);


                        res.warehouseManagerName = UpdateName;
                        res.warehouseManagerLocation = UpdateLocation;
                        res.warehouseManagerPhoneNumber = UpdatePhoneNumber;

                        context.SaveChanges();
                    }
                }
                catch
                {
                    MessageBoxWindow.Show("Thiếu thông tin");
                }
            }
        }
     
    }
}
