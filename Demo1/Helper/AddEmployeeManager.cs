using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Demo1.Model
{
    public class AddEmployeeManager
    {
        public bool CheckIsEmployee(string str)
        {
            using (var context = new PBL3_demoEntities())
            {
                bool existsInTable1 = context.Receptionists.Any(t => t.receptionistID == str);
                bool existsInTable2 = context.WarehouseStaffs.Any(t => t.warehouseStaffID == str);

                bool res = existsInTable1 || existsInTable2;
                return res;
            }
            //true neu ton tai,false neu 0 ton tai
        }

        public Account GetAccountRecord(string roleID)
        {
            using (var context = new PBL3_demoEntities())
            {
                var thisAccount = context.Accounts.Where(x => x.accountID == roleID).FirstOrDefault();
                if (thisAccount != null)
                {
                    return thisAccount;
                }
                else
                {
                    return null;
                }
            }
        }

        public Receptionist GetReceptionistRecord(string receptionistID)
        {
            using (var context = new PBL3_demoEntities())
            {
                var res = context.Receptionists.Where(x => x.receptionistID == receptionistID).FirstOrDefault();
                if (res != null)
                {
                    return res;
                }
                else
                {
                    return null;
                }
            }
        }

        public WarehouseStaff GetWHStaffRecord(string WHStaffID)
        {
            using (var context = new PBL3_demoEntities())
            {
                var res = context.WarehouseStaffs.Where(x => x.warehouseStaffID == WHStaffID).FirstOrDefault();
                if (res != null)
                {
                    return res;
                }
                else
                {
                    return null;
                }
            }
        }
        public bool CreateNewEmployee(string roleText, string newAccountName, string newPassword, string newPhoneNumber, string newLocation, string newEmployeeName, string whid)
        {
            using (var context = new PBL3_demoEntities())
            {
                int mode = GetRole(roleText);
                var newAccount = new Account { accountID = roleText, accountName = newAccountName, accountPassword = newPassword, accessRightID = mode };
                context.Accounts.Add(newAccount);

                if (mode == 1)
                {
                    var newEmployee = new Receptionist { receptionistID = roleText, receptionistLocation = newLocation, receptionistPhoneNumber = newPhoneNumber, receptionistName = newEmployeeName, warehouseID = whid };
                    context.Receptionists.Add(newEmployee);
                }
                else
                {
                    var newEmployee = new WarehouseStaff { warehouseStaffID = roleText, warehouseStaffLocation = newLocation, warehouseStaffPhoneNumber = newPhoneNumber, warehouseStaffName = newEmployeeName, warehouseID = whid };
                    context.WarehouseStaffs.Add(newEmployee);
                }

                context.SaveChanges();
                return true;
            }
        }

        public bool IsAccountUserNameExist(string newAccountName)
        {
            using (var context = new PBL3_demoEntities())
            {
                var res = context.Accounts.FirstOrDefault(x => x.accountName == newAccountName);
                return res != null;
            }
        }

        public bool IsValid(string roleText, string inputStr, string newPhoneNumber, string newAccountName)
        {
            string pattern = @"^0\d{9}$"; // Định dạng số điện thoại gồm 10 chữ số và có số 0 ở trước

            if (inputStr?.Length > 0 && Regex.IsMatch(inputStr, pattern) && !string.IsNullOrEmpty(newPhoneNumber) && !IsAccountUserNameExist(newAccountName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateEmployee(string roleText, string newAccountName, string newPassword, string newPhoneNumber, string newLocation, string newEmployeeName, string whid)
        {
            using (var context = new PBL3_demoEntities())
            {
                Account accountToUpdate = context.Accounts.FirstOrDefault(a => a.accountID == roleText);
                if (accountToUpdate != null)
                {
                    accountToUpdate.accountName = newAccountName;
                    accountToUpdate.accountPassword = newPassword;
                }

                int mode = GetRole(roleText);
                if (mode == 1)
                {
                    Receptionist receptionistToUpdate = context.Receptionists.FirstOrDefault(a => a.receptionistID == roleText);
                    if (receptionistToUpdate != null)
                    {
                        receptionistToUpdate.receptionistName = newEmployeeName;
                        receptionistToUpdate.receptionistLocation = newLocation;
                        receptionistToUpdate.receptionistPhoneNumber = newPhoneNumber;
                        receptionistToUpdate.warehouseID = whid;
                    }
                }
                else
                {
                    WarehouseStaff whstaffToUpdate = context.WarehouseStaffs.FirstOrDefault(a => a.warehouseStaffID == roleText);
                    if (whstaffToUpdate != null)
                    {
                        whstaffToUpdate.warehouseStaffName = newEmployeeName;
                        whstaffToUpdate.warehouseStaffLocation = newLocation;
                        whstaffToUpdate.warehouseStaffPhoneNumber = newPhoneNumber;
                        whstaffToUpdate.warehouseID = whid;
                    }
                }

                context.SaveChanges();
            }
        }

        public int GetRole(string RoleText)
        {
            if (RoleText.Contains("R")) return 1;
            else if (RoleText.Contains("S")) return 2;
            else return 0;

        }
    }
}
