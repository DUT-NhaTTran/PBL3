using Demo1.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Demo1.ViewModel.SearchParcelModel;

namespace Demo1.Model
{
    public class SearchManager
    {
        public SearchManager() { }
        public string GetSisCOD(int iParcelID)
        {
            using (var context = new PBL3_demoEntities())
            {
                var parcel = context.Parcels.FirstOrDefault(x => x.parcelID == iParcelID);
                if (parcel != null)
                {
                    if (parcel.isCOD.HasValue)
                    {
                        bool isCOD = (bool)parcel.isCOD;
                        return isCOD ? "COD" : "Bình thường";
                    }
                    else
                    {
                        return "Không xác định";
                    }
                }
            }

            return string.Empty; // Trả về một giá trị mặc định nếu không tìm thấy parcel hoặc có lỗi xảy ra.
        }
        public ObservableCollection<ParcelInfoInSearch> GetAllParcels()
        {
            ObservableCollection<ParcelInfoInSearch> parcelInfoListInSearch = new ObservableCollection<ParcelInfoInSearch>();

            using (var context = new PBL3_demoEntities())
            {
                var parcels = context.Parcels.ToList();

                foreach (var parcel in parcels)
                {
                    var parcelInfo = new ParcelInfoInSearch
                    {
                        ID = parcel.parcelID,
                        ParcelName = parcel.parcelName,
                        ParcelType = ((bool)parcel.type) ? "Hàng dễ vỡ" : "Hàng bình thường",
                        ParcelValue = (double)parcel.parcelValue
                    };

                    parcelInfoListInSearch.Add(parcelInfo);
                }
            }

            return parcelInfoListInSearch;
        }
        public List<ParcelInfoInSearch> LoadAllParcelSearched(string searchParcelText)
        {
            List<ParcelInfoInSearch> parcelInfoListInSearch = new List<ParcelInfoInSearch>();

            using (var dbContext = new Model.PBL3_demoEntities())
            {
                var query = dbContext.Parcels
                    .Where(x => x.parcelID.ToString().Contains(searchParcelText) || x.parcelName.Contains(searchParcelText))
                    .Select(y => new ParcelInfoInSearch
                    {
                        ID = y.parcelID,
                        ParcelName = y.parcelName,
                        ParcelType = (bool)y.type ? "Hàng dễ vỡ/điện tử" : "Bình thường",
                        ParcelValue = (double)y.parcelValue
                    });

                parcelInfoListInSearch = query.ToList();
            }

            return parcelInfoListInSearch;
        }
    }
}
