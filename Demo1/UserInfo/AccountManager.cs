using Demo1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo1.UserInfo
{
    public class AccountManager
    {
        private static AccountManager instance;

        // Thông tin tài khoản đăng nhập và phân quyền
        private string accountID;
        private string accountname;
        private string password;
        private int accessRight;
        private string userWHID;
        private string username;
        private string rolename;
       


        // Các role của hệ thống.
       
        // Các thông tin của một người dùng.
        
        private AccountManager() 
        {
            
       
        }
        public static AccountManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccountManager();
                }
                return instance;
            }
        }
        public void SetLoginInfo(string accountID,string loginname,string loginpassword,int accessRight)
        {
            this.accountID= accountID;
            this.accountname= loginname;
            this.password= loginpassword;
            this.accessRight = accessRight;
        }
        public int GetAccessRight()
        {
            return accessRight;
        }
        public string GetAccountID()
        {
            return accountID;
        }
        public string GetUserName(string accountID)
        {
            using (var context = new PBL3_demoEntities())
            {
                if (accessRight == 1)
                {
                    var check = context.Receptionists.FirstOrDefault(x => x.receptionistID == accountID);
                    if (check != null) 
                    {
                        username = check.receptionistName;
                    }
                    
                }
                if (accessRight == 2)
                {
                    var check = context.WarehouseStaffs.FirstOrDefault(x => x.warehouseStaffID == accountID);
                    if (check != null)
                    {
                        username = check.warehouseStaffName;
                    }
                    
                }
                if (accessRight == 3)
                {
                    var check = context.WarehouseManagers.FirstOrDefault(x => x.warehouseManagerID == accountID);
                    if (check != null)
                    {
                        username = check.warehouseManagerName;
                    }
                    
                }
                return username;
            }
            
        }
        public string GetUserLocation(string accountID)
        {
            using (var context = new PBL3_demoEntities())
            {
                if (accessRight == 1)
                {
                    var check = context.Receptionists.FirstOrDefault(x => x.receptionistID == accountID);
                    if (check != null)
                    {
                        username = check.receptionistLocation;
                    }

                }
                if (accessRight == 2)
                {
                    var check = context.WarehouseStaffs.FirstOrDefault(x => x.warehouseStaffID == accountID);
                    if (check != null)
                    {
                        username = check.warehouseStaffLocation;
                    }

                }
                if (accessRight == 3)
                {
                    var check = context.WarehouseManagers.FirstOrDefault(x => x.warehouseManagerID == accountID);
                    if (check != null)
                    {
                        username = check.warehouseManagerLocation;
                    }

                }
                return username;
            }

        }
        public string GetUserPhoneNumber(string accountID)
        {
            using (var context = new PBL3_demoEntities())
            {
                if (accessRight == 1)
                {
                    var check = context.Receptionists.FirstOrDefault(x => x.receptionistID == accountID);
                    if (check != null)
                    {
                        username = check.receptionistPhoneNumber;
                    }

                }
                if (accessRight == 2)
                {
                    var check = context.WarehouseStaffs.FirstOrDefault(x => x.warehouseStaffID == accountID);
                    if (check != null)
                    {
                        username = check.warehouseStaffPhoneNumber;
                    }

                }
                if (accessRight == 3)
                {
                    var check = context.WarehouseManagers.FirstOrDefault(x => x.warehouseManagerID == accountID);
                    if (check != null)
                    {
                        username = check.warehouseManagerPhoneNumber;
                    }

                }
                return username;
            }

        }
        public string GetUserWarehouseID(string accountID)
        {
            using (var context = new PBL3_demoEntities())
            {
                if (accessRight == 1)
                {
                    var check = context.Receptionists.FirstOrDefault(x => x.receptionistID == accountID);
                    if (check != null)
                    {
                        userWHID = check.warehouseID;
                    }
                }
                if (accessRight == 2)
                {
                    var check = context.WarehouseStaffs.FirstOrDefault(x => x.warehouseStaffID == accountID);
                    if (check != null)
                    {
                        userWHID = check.warehouseID;
                    }
                }
                if (accessRight == 3)
                {
                    var check = context.WarehouseManagers.FirstOrDefault(x => x.warehouseManagerID == accountID);
                    if (check != null)
                    {
                        userWHID = check.warehouseID;
                    }
                }
            }
            return userWHID;
        }
        public string GetRoleName(int accessRight)
        {
            if (accessRight == 1) rolename= "Lễ tân kho";
            else if (accessRight == 2) rolename = "Nhân viên kho";
            else if (accessRight == 3) rolename = "Quản lý kho";
            return rolename;
        }
      
        public Receptionist GetReceptionist(string receptionistId)
        {
            using (var context = new PBL3_demoEntities())
            {
                Receptionist reception = context.Receptionists.FirstOrDefault(r => r.receptionistID == receptionistId);

                return reception;
            }
        }
        public WarehouseStaff GetWHS(string id)
        {
            using (var context = new PBL3_demoEntities())
            {
                WarehouseStaff whs = context.WarehouseStaffs.FirstOrDefault(r => r.warehouseStaffID == id);

                return whs;
            }
        }
        public WarehouseManager GetWHM(string id)
        {
            using (var context = new PBL3_demoEntities())
            {
                WarehouseManager whm = context.WarehouseManagers.FirstOrDefault(r => r.warehouseManagerID == id);

                return whm;
            }
        }


    }
}
