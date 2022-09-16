using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OrderReceive.Entities
{
    public class OrderLines
    {
        public Guid Id { get; set; }
        [Required]
        public Guid OrderHeaderId { get; set; }
        [Required]
        public string Order_Code { get; set; }
        public OrderHeader OrderHeader { get; set; }
        public string Notes { get; set; }
        [Required]
        public string LineId { get; set; }
        [Required]
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public int QtyPerBox { get; set; }
        [Required]
        public string Unit { get; set; }
        [Required]
        public int QtyOrdered { get; set; }
        [Required]
        public int QtyPlanned { get; set; }
        public int QtyAllocated { get; set; }

        public int QtyPicked { get; set; }
        public string Status { get; set; }
        [Required]
        public bool Active { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        [Required]
        public string Customer_Code { get; set; }
        [Required]
        public string Customer_Name { get; set; }
        [Required]
        public string Warehouse_Code { get; set; }
        [Required]
        public string Warehouse_Name { get; set; }
        [Required]
        public string Company_Code { get; set; }
        [Required]
        public string Company_Name { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public string CreatedDate { get; set; }
        [Required]
        public string CreatedTime { get; set; }

        public string UpdatedBy { get; set; }

        public string UpdatedDate { get; set; }

        public string UpdatedTime { get; set; }

        public string DeletedBy { get; set; }

        public string DeletedDate { get; set; }

        public string DeletedTime { get; set; }
    }
}
