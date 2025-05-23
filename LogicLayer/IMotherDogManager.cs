﻿using DataDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface IMotherDogManager
    {
        public List<MotherDog> GetAllMotherDogs();
        public MotherDog SelectMotherDogByMotherDogID(string motherDogID);
    }
}
