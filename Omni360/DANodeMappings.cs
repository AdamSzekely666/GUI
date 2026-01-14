namespace MatroxLDS
{
    public static class DANodeMappings
    {
        public const string DA_NAMESPACE = "http://zebra.com/DesignAssistant/";
        public const string DA_VARIABLE_TYPE_NODEID = "ns=2;i=3002";
        public const string DA_RUNTIME_EVENT_NODEID = "ns=2;i=5001";
        public const string DA_EVENT_FILTER_TYPE_NODEID = "ns=2;i=5001";
        public const string ENUM_TYPE_NODE_ID = "i=11737";

        // Default event name (fallback if config. json doesn't specify)
        public const string DEFAULT_EVENT_OBJECT_NAME = "Inspection End";

        // Flowchart names
        public const string NEXT_IMAGE_FLOWCHART_NAME = "NextImage";
        public const string RERUN_FLOWCHART_NAME = "Rerun";

        // Node IDs for specific bindings
        public const string IMAGEWRITER_RESIZE_NODEID = "ns=2;s=ImageWriter.ResizeHeight";
    }
}