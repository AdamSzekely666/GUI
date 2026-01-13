namespace MatroxLDS
{
    /// <summary>
    /// Provides mapping to be able to write to Design Assistant nodes.
    /// </summary>
    public static class DANodeMappings
    {
        public const string DA_NAMESPACE = "ns=2;s=";

        public const string DA_EVENT_FILTER_TYPE_NODEID = "ns=2;i=1003";
        public const string DA_RUNTIME_EVENT_NODEID = "ns=2;i=3004";
        public const string DA_VARIABLE_TYPE_NODEID = "ns=2;i=3002";

        public const string ENUM_TYPE_NODE_ID = "i=7594";

        public const string IMAGEWRITER_RESIZE_NODEID = "ns=2;s=Bindings.ImageWriter_ResizeHeight";

        public const string RERUN_FLOWCHART_NAME = "OnRerunTrigger";

        public const string NEXT_IMAGE_FLOWCHART_NAME = "OnSendTrigger";
    }
}
