﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trivia_Stage2.Services;

namespace Trivia_Stage2.ViewModels
{
    public class BestScoresPageViewModel : ViewModel
    {
        private Service service;
        public BestScoresPageViewModel(Service service_)
        {
            service = service_;
        }
    }
}