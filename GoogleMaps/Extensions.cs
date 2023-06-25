using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMaps
{
    public static class Extensions
    {
        public static int? GetDistanceInMeters(this DistanceMatrixResponse distanceMatrixResponse)
        {
            return distanceMatrixResponse.Rows.FirstOrDefault()?.Elements?.FirstOrDefault()?.Distance.Value;
        }
    }
}
