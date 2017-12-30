﻿using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using TPGP.ActionFilters;
using TPGP.DAL.Interfaces;
using TPGP.Models.Jobs;

namespace TPGP.Controllers
{
    [CustomAuthorizeOthers]
    public class ContractController : Controller
    {
        private readonly IContractRepository contractRepository;

        public ContractController(IContractRepository contractRepository)
        {
            this.contractRepository = contractRepository;
        }

        public ActionResult Index(string currentFilter, string searchString)
        {
            return View();
        }

        public ActionResult Details(long id)
        {
            Contract contract = contractRepository.GetById(id);

            return View(contract);
        }
    }
}