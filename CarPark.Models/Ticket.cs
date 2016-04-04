using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPark.Models
{
    public class Ticket
    {
        public DateTime DateIn { get; set; }
        public DateTime? DateOut { get; set; }

        public decimal? ParkingTime
        {
            get
            {
                if (!DateOut.HasValue)
                    return null;
                var minTimespan = DateOut.Value - DateIn;
                var min = decimal.Ceiling(Convert.ToDecimal(minTimespan.TotalSeconds) / 60m);

                return Convert.ToDecimal(min);
            }
        }

        public decimal? ParkingFee
        {
            get
            {
                var parkingTime = ParkingTime;
                if (!parkingTime.HasValue)
                    return null;
                if (DateOut < DateIn)
                    throw new Exception("Invalid Date");

                int bufferTime = 15;
                decimal fee = decimal.Zero;
                
                // 15 Min = Free
                if (parkingTime.Value <= bufferTime)
                    fee = decimal.Zero;
                // 3 Hours = 50b
                else if (parkingTime.Value <= (180 + bufferTime))
                    fee = 50;
                // Next = 30b/Hour (over 15 mins = 1 hours)
                else if (parkingTime.Value > (180 + bufferTime))
                {
                    var baseFee = 50;
                    var totalMin = parkingTime.Value - (180 + bufferTime);
                    fee = baseFee + (decimal.Ceiling(totalMin / 60) * 30);
                }

                return fee;
            }
        }

        public string PlateNo { get; set; }
    }
}
