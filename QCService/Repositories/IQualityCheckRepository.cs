using QCService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QCService.Repositories
{
    public interface IQualityCheckRepository
    {
        Task<bool> OrderExistsForQualityCheck(string orderCode, string qcTag);

        Task<IEnumerable<QualityCheck>> GetAllQualityCheckTasks();

        Task<IEnumerable<QualityCheck>> GetQualityCheckTasksByOrder(string orderCode);
        Task<QualityCheck> GetQualityCheckTasksByOrderAndTag(string orderCode, string qcTag);

        Task AddQualityCheck(QualityCheck qualityCheck);

        Task<QualityCheck> UpdateQualityCheckStatus(string orderCode, string qcTag, QualityCheck qualityCheck);

        Task<bool> SaveChanges();

    }
}
