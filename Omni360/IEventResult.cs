using System.Collections.Generic;

namespace OPCUADemoClient.Models
{
    public interface IEventResult
    {
        void UpdateModel(string variableName, bool isAvailable, object currentValue, List<string> availableValues);
    }
}
