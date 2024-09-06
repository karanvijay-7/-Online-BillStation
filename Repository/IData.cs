using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBilling.Models;
using System.Data.SqlClient;

namespace EBilling.Repository
{
    internal interface IData
    {
        void saveBillingDetails(BillDetail details);
        void saveBillItems(List<Items> items, SqlConnection con, int id);
        List<BillDetail> GetAllDetail();

        BillDetail GetDetail(int Id);
        
        
    }
}
