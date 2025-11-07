using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingTrails.Shared.Features.ManageTrails.Requests
{
    public class GetTrailsRequestFlyoutMenuItem
    {
        public GetTrailsRequestFlyoutMenuItem()
        {
            TargetType = typeof(GetTrailsRequestFlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}