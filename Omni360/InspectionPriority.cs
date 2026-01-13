using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniCheck_360
{
    //internal class InspectionPriority
    //{
    //}
    public class InspectionPriorityConfig
    {
        public int maxCameras { get; set; }
        public List<CameraConfig> cameras { get; set; }
    }

    public class CameraConfig
    {
        public string name { get; set; }
        public List<InspectionConfig> inspections { get; set; }
    }

    public class InspectionConfig
    {
        public string name { get; set; }
        public int priority { get; set; }
    }
}
