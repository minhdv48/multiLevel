using BaseRepo.Repositories;
using Dapper;
using Microsoft.Extensions.Configuration;
using Multi_LevelModels.Interfaces;
using Multi_LevelModels.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Multi_LevelModels.Implements
{
    public class IplInvoice : Repository<Invoice>, IInvoice
    {
        #region Default and Contructor
        public IConfiguration _Configuration { get; }
        internal string _cnnString;
        public MultiLevelContext _context;
        public IplInvoice(MultiLevelContext Context, IConfiguration configuration) : base(Context)
        {
            _Configuration = configuration;
            _context = Context;
            _cnnString = _Configuration.GetConnectionString("DefaultConnection");
        }
        #endregion
        public List<Invoice> GetInvoices(int proId, DateTime? fromDate, DateTime? toDate, int offset, int limit, ref int total)
        {
            List<Invoice> List = new List<Invoice>();
            var unitOfWork = new UnitOfWorkFactory(_cnnString);
            try
            {
                using (var u = unitOfWork.Create(false))
                {
                    var p = new DynamicParameters();
                    p.Add("@proId", proId);
                    p.Add("@fromDate", fromDate);
                    p.Add("@toDate", toDate);
                    p.Add("@offSet", offset);
                    p.Add("@limit", limit);
                    p.Add("@totalRow", total, DbType.Int32, ParameterDirection.Output);
                    List = u.GetIEnumerable<Invoice>("sp_Invoice_GetDataListAll", p).ToList();
                    total = p.Get<int>("@totalRow");
                }
            }
            catch (Exception ex)
            {
                return List;
            }
            return List;
        }
        public List<Invoice> GetListBenefit(int proId, DateTime? fromDate, DateTime? toDate, int offset, int limit, ref int total)
        {
            List<Invoice> List = new List<Invoice>();
            var unitOfWork = new UnitOfWorkFactory(_cnnString);
            try
            {
                using (var u = unitOfWork.Create(false))
                {
                    var p = new DynamicParameters();
                    p.Add("@proId", proId);
                    p.Add("@fromDate", fromDate);
                    p.Add("@toDate", toDate);
                    p.Add("@offSet", offset);
                    p.Add("@limit", limit);
                    List = u.GetIEnumerable<Invoice>("sp_Invoice_GetListBenefit", p).ToList();
                }
            }
            catch (Exception ex)
            {
                return List;
            }
            return List;
        }
        public bool UpdatePayment(int profileId, DateTime? fromDate, DateTime? toDate)
        {
            bool retval = false;
            var unitOfWork = new UnitOfWorkFactory(_cnnString);
            try
            {
                using (var u = unitOfWork.Create(true))
                {
                    var p = new DynamicParameters();
                    p.Add("@proId", profileId);
                    p.Add("@fromDate", fromDate);
                    p.Add("@toDate", toDate);
                    u.ProcedureExecute("sp_Invoice_UpdatePayment", p);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return retval;
            }
            return retval;
        }
    }
}
