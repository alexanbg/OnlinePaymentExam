using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePayments.Services.Helpers
{
    public static class StatusHelper
    {
        public static string ToStatusString(byte status)
        {
            switch(status)
            {
                case 1:
                    return "Изчаква";
                case 2:
                    return "Обработен";
                default:
                    return "Отказан";
            }
        }
    }
}
