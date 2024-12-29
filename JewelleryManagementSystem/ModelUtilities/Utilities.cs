using JewelleryManagementSystem.OrnamentManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelleryManagementSystem.ModelUtilities
{
    public static class Utilities
    {
        public static float GetRatePerGram(float rate, WeightType weightType)
        {
            switch (weightType)
            {
                case WeightType.InMiliGram:
                    return rate * 1000;
                case WeightType.InGram:
                    return rate;
                case WeightType.InTenGram:
                    return rate / 10;
                case WeightType.InKiloGram:
                    return rate / 1000;
                default:
                    return 0;
            }
        }
    }
}
