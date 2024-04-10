using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatUpdater.Models.Response
{
    public class DemoListResponse
    {
        public List<DemoResponse> responseList { get; set; } = new List<DemoResponse>();
    }
}
