using BaseRepo.Interfaces;
using Multi_LevelModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multi_LevelModels.Interfaces
{
    public interface IInvoice:IRepository<Invoice>
    {
        List<Invoice> GetInvoices(int proId, DateTime? fromDate, DateTime? toDate, int offset, int limit, ref int total);
        List<Invoice> GetListBenefit(int proId, DateTime? fromDate, DateTime? toDate, int offset, int limit, ref int total);
        bool UpdatePayment(int profileId, DateTime? fromDate, DateTime? toDate);
    }
}
