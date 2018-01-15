﻿using System.Collections.Generic;
using TPGP.Models.Jobs;
using TPGP.ViewModels;

namespace TPGP.Models.ViewModels
{
    public class GeographicalZoneViewModel
    {
        public GeographicalZone Zone { get; set; }
        public ICollection<ContractViewModel> Contracts { get; set; }

        public GeographicalZoneViewModel()
        {
        }
    }
}