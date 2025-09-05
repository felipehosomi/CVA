using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPB1;
using App.Repository.Interfaces;
using App.Repository.Repositories;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using App.Repository.Exception;
using App.Repository.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ApplicationWpf.Enuns;
using System.Windows.Forms;

namespace App.ApplicationServices.ServiceLayer
{

    public class SD_OPService
    {
        
        private readonly IServiceLayerRepository<SD_OP> _repository;

        public SD_OPService()
        {
            _repository = new ServiceLayerRepositories<SD_OP>("SD_OP");
        }

        

    }
}
