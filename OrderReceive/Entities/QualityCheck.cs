﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderReceive.Entities
{
    public class QualityCheck
    {
        public Guid QualityCheckId { get; set; }
        public string Product_Code { get; set; }
        public string QC_Tag { get; set; }
        public string QC_List { get; set; }
        public string QC_ListDate { get; set; }
        public string QC_ListTime { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
        public string Customer_Code { get; set; }
        public string Warehouse_Code { get; set; }
        public string Company_Code { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedTime { get; set; }
    }
}
