﻿using Business_logic_layer.interfaces;
using Data_access_layer.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic_layer.Repository
{
    public class QuestionRepo :genericRepo<Questions>, IquestionRepo
    {
        public QuestionRepo(ApplicationDbContext context) : base(context)
        {
        }
    }
    
}
