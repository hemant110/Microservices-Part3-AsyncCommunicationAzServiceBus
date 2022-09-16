using Microsoft.EntityFrameworkCore;
using QCService.DBContexts;
using QCService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QCService.Repositories
{
    public class QualityCheckRepository : IQualityCheckRepository
    {
        DbContextOptions<QualityCheckDbContext> qualityCheckDBContext;
        public QualityCheckRepository(DbContextOptions<QualityCheckDbContext> qualityCheckDBContext)
        {
            this.qualityCheckDBContext = qualityCheckDBContext;
        }
        public async Task AddQualityCheck(QualityCheck qualityCheck)
        {

            await using (var _qualityCheckContext = new QualityCheckDbContext(qualityCheckDBContext))
            {
                await _qualityCheckContext.QualityCheck.AddAsync(qualityCheck);
                await _qualityCheckContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<QualityCheck>> GetAllQualityCheckTasks()
        {
            var _qualityCheckContext = new QualityCheckDbContext(qualityCheckDBContext);
            return await _qualityCheckContext.QualityCheck.OrderBy(x => x.QC_List).ThenBy(x => x.QC_Tag).ToListAsync();
        }

        public async Task<IEnumerable<QualityCheck>> GetQualityCheckTasksByOrder(string orderCode)
        {
            var _qualityCheckContext = new QualityCheckDbContext(qualityCheckDBContext);
            var qualityCheckList = await _qualityCheckContext.QualityCheck.Where(x => x.QC_List == orderCode).OrderBy(x => x.QC_List).ThenBy(x => x.QC_Tag).ToListAsync();
            return qualityCheckList;
        }

        public async Task<QualityCheck> GetQualityCheckTasksByOrderAndTag(string orderCode, string qcTag)
        {
            var _qualityCheckContext = new QualityCheckDbContext(qualityCheckDBContext);
            var qualityCheckList = await _qualityCheckContext.QualityCheck.Where(x => x.QC_List == orderCode && x.QC_Tag == qcTag).OrderBy(x => x.QC_List).ThenBy(x=> x.QC_Tag).FirstOrDefaultAsync();
            return qualityCheckList;
        }

        public async Task<bool> OrderExistsForQualityCheck(string orderCode, string qcTag)
        {
            var _qualityCheckContext = new QualityCheckDbContext(qualityCheckDBContext);

            if (qcTag == null || qcTag == String.Empty)
                return await _qualityCheckContext.QualityCheck.Where(x => x.QC_List == orderCode).AnyAsync();
            else
                return await _qualityCheckContext.QualityCheck.Where(x => x.QC_List == orderCode && x.QC_Tag == qcTag).AnyAsync();

        }

        public async Task<bool> SaveChanges()
        {
            var _qualityCheckContext = new QualityCheckDbContext(qualityCheckDBContext);
            return (await _qualityCheckContext.SaveChangesAsync() > 0);
        }

        public async Task<QualityCheck> UpdateQualityCheckStatus(string orderCode, string qcTag, QualityCheck qualityCheck)
        {
            try
            {
                using (var _qualityCheckContext = new QualityCheckDbContext(qualityCheckDBContext))
                {
                    var existingQC = await _qualityCheckContext.QualityCheck.Where(x => x.QC_List == orderCode && x.QC_Tag == qcTag).FirstOrDefaultAsync();

                    if (existingQC == null)
                    {
                        qualityCheck.Active = true;
                        qualityCheck.IsDeleted = false;
                        qualityCheck.Customer_Code = String.IsNullOrEmpty(qualityCheck.Customer_Code) == true ? "ABC" : qualityCheck.Customer_Code;
                        qualityCheck.Customer_Name = String.IsNullOrEmpty(qualityCheck.Customer_Name) == true ? "ABC" : qualityCheck.Customer_Name;
                        qualityCheck.Warehouse_Code = String.IsNullOrEmpty(qualityCheck.Warehouse_Code) == true ? "ABC" : qualityCheck.Warehouse_Code;
                        qualityCheck.Warehouse_Name = String.IsNullOrEmpty(qualityCheck.Warehouse_Name) == true ? "ABC" : qualityCheck.Warehouse_Name;
                        qualityCheck.Company_Code = String.IsNullOrEmpty(qualityCheck.Company_Code) == true ? "ABC" : qualityCheck.Company_Code;
                        qualityCheck.Company_Name = String.IsNullOrEmpty(qualityCheck.Company_Name) == true ? "ABC" : qualityCheck.Company_Name;
                        qualityCheck.CreatedBy = String.IsNullOrEmpty(qualityCheck.CreatedBy) == true ? "Sys" : qualityCheck.CreatedBy;
                        qualityCheck.CreatedDate = String.IsNullOrEmpty(qualityCheck.CreatedDate) == true ? DateTime.Now.Date.ToString("yyyy-MM-dd") : qualityCheck.CreatedDate;
                        qualityCheck.CreatedTime = String.IsNullOrEmpty(qualityCheck.CreatedTime) == true ? DateTime.Now.TimeOfDay.ToString().Substring(0, 8) : qualityCheck.CreatedTime;
                        _qualityCheckContext.QualityCheck.Add(qualityCheck);
                        await _qualityCheckContext.SaveChangesAsync();
                        return qualityCheck;
                    }
                    existingQC.Active = true;
                    existingQC.IsDeleted = false;
                    existingQC.UpdatedBy = "Sys";
                    existingQC.UpdatedDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    existingQC.UpdatedTime = DateTime.Now.TimeOfDay.ToString().Substring(0, 8);
                    existingQC.QC_Action = qualityCheck.QC_Action;
                    existingQC.QC_By = qualityCheck.QC_By;
                    existingQC.QC_Notes = qualityCheck.QC_Notes;
                    existingQC.QC_Status = qualityCheck.QC_Status;

                    existingQC.Customer_Code = String.IsNullOrEmpty(existingQC.Customer_Code) == true ? "ABC" : existingQC.Customer_Code;
                    existingQC.Customer_Name = String.IsNullOrEmpty(existingQC.Customer_Name) == true ? "ABC" : existingQC.Customer_Name;
                    existingQC.Warehouse_Code = String.IsNullOrEmpty(existingQC.Warehouse_Code) == true ? "ABC" : existingQC.Warehouse_Code;
                    existingQC.Warehouse_Name = String.IsNullOrEmpty(existingQC.Warehouse_Name) == true ? "ABC" : existingQC.Warehouse_Name;
                    existingQC.Company_Code = String.IsNullOrEmpty(existingQC.Company_Code) == true ? "ABC" : existingQC.Company_Code;
                    existingQC.Company_Name = String.IsNullOrEmpty(existingQC.Company_Name) == true ? "ABC" : existingQC.Company_Name;
                    existingQC.CreatedBy = String.IsNullOrEmpty(existingQC.CreatedBy) == true ? "Sys" : existingQC.CreatedBy;
                    existingQC.CreatedDate = String.IsNullOrEmpty(existingQC.CreatedDate) == true ? DateTime.Now.Date.ToString("yyyy-MM-dd") : existingQC.CreatedDate;
                    existingQC.CreatedTime = String.IsNullOrEmpty(existingQC.CreatedTime) == true ? DateTime.Now.TimeOfDay.ToString().Substring(0, 8) : existingQC.CreatedTime;

                    _qualityCheckContext.QualityCheck.Update(existingQC);
                    await _qualityCheckContext.SaveChangesAsync();
                    return existingQC;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new QualityCheck();
            }
        }
    }
}
