using System.Collections.Generic;

namespace MatroxLDS
{
    public interface IEventResult
    {
        void UpdateModel(string variableName, bool isAvailable, object currentValue, List<string> availableValues);
    }
}
