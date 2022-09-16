using AutoMapper;
using QCService.Entities;
using QCService.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QCService.Profiles
{
    public class QCMessageProfile :Profile
    {
        public QCMessageProfile()
        {
            CreateMap<QCLineMessage, QualityCheck>();
        }
    }
}
