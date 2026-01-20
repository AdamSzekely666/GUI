using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace Omnicheck360
{
    public class Configuration
    {

        #region Constants

        /// <summary>
        /// Root configuration node name.
        /// </summary>
        private const string _configurationNodeName = "Configuration";

        /// <summary>
        /// Node name for language.
        /// </summary>
        private const string _languageNodeName = "Language";

        /// <summary>
        /// Node name for application.
        /// </summary>
        private const string _applicationNodeName = "Application";

        /// <summary>
        /// Node name for useraccess.
        /// </summary>
        private const string _userAccessNodeName = "UserAccess";

        /// <summary>
        /// Node name for user DataBase.
        /// </summary>
        private const string _userDatabaseNodeName = "UserDataBase";

        /// <summary>
        /// Node name for Log DataBase.
        /// </summary>
        private const string _logDatabaseNodeName = "LogDataBase";

        /// <summary>
        /// Node name for user.
        /// </summary>
        private const string _userNodeName = "User";

        /// <summary>
        /// Node name for Documents.
        /// </summary>
        private const string _documentsNodeName = "Documents";

        /// <summary>
        /// Node name for Doc.
        /// </summary>
        private const string _docNodeName = "Doc";

        /// <summary>
        /// Node name for communication protocol.
        /// </summary>
        private const string _communicationProtocolNodeName = "CommunicationProtocol";

        /// <summary>
        /// Node name for communication cameras.
        /// </summary>
        private const string _communicationCamerasNodeName = "CommunicationCameras";

        /// <summary>
        /// Node name for communication motor drives.
        /// </summary>
        private const string _communicationMotorDrivesNodeName = "CommunicationMotorDrives";

        /// <summary>
        /// Node name for Bullnose Settings.
        /// </summary>
        private const string _bullnoseSettingsNodeName = "BullnoseSettings";

        /// <summary>
        /// Node name for protocol.
        /// </summary>
        private const string _protocolNodeName = "Protocol";

        /// <summary>
        /// Node name for Cam.
        /// </summary>
        private const string _camNodeName = "Cam";

        /// <summary>
        /// Attribute name for type.
        /// </summary>
        private const string _typeAttributeName = "Type";

        /// <summary>
        /// Attribute name for port.
        /// </summary>
        private const string _portAttributeName = "Port";

        /// <summary>
        /// Attribute name for IP.
        /// </summary>
        private const string _ipAttributeName = "IPAddress";

        /// <summary>
        /// Attribute name for application Key.
        /// </summary>
        private const string _keyAttributeName = "Key";

        /// <summary>
        /// Attribute name for application Title.
        /// </summary>
        private const string _TitleAttributeName = "Title";

        /// <summary>
        /// Attribute name for vision project Title.
        /// </summary>
        private const string _visonProjectAttributeName = "VisionProject";

        /// <summary>
        /// Attribute name for CurrentUserTxt.
        /// </summary>
        private const string _CurrentUserTxtAttributeName = "CurrentUserTxt";

        // <summary>
        /// Attribute name for Password.
        /// </summary>
        private const string _passwordAttributeName = "Password";


        /// <summary>
        /// Attribute name for Name.
        /// </summary>
        private const string _nameAttributeName = "Name";

        /// <summary>
        /// Attribute name for culture.
        /// </summary>
        private const string _cultureAttributeName = "Culture";

        /// <summary>
        /// Attribute name for numberOfSensors.
        /// </summary>
        private const string _nbOfSensorsAttributeName = "NumberOfSensors";

        /// <summary>
        /// Attribute name for SamplesPerSensor.
        /// </summary>
        private const string _samplesPerSensorAttributeName = "SamplesPerSensor";

        /// <summary>
        /// Attribute name for AdditionalData.
        /// </summary>
        private const string _additionalDataAttributeName = "AdditionalData";

        #endregion

        #region Variables

        /// <summary>
        /// Application key.
        /// </summary>
        private int _applicationKey = 0;

        /// <summary>
        /// Application Name.
        /// </summary>
        private String _applicationTitle;

        /// <summary>
        /// Vision Project Name.
        /// </summary>
        private String _visionProjectTitle;

        /// <summary>
        /// Application specific guid for multiple instances.
        /// </summary>
        private Guid _guidApplication = Guid.Empty;

        /// <summary>
        /// Culture information (language).
        /// </summary>
        private CultureInfo _culture = null;

        /// <summary>
        /// Modbus communication socket port number.
        /// </summary>
        private int _ModbusPort = 8111;

        /// <summary>
        /// Modbus communication IP address.
        /// </summary>
        private IPAddress _ModbusIP = null;

        /// <summary>
        /// TCP/IP communication socket port number.
        /// </summary>
        private int _TCPIPport = 8111;

        /// <summary>
        /// TCP/IP communication IP address.
        /// </summary>
        private IPAddress _TCPIPAddress = null;

        /// <summary>
        /// List of Matrox camera IP addresses.
        /// </summary>
        private List<IPAddress> _CameraIDs;

        /// <summary>
        /// List of Motor Drives IP addresses.
        /// </summary>
        private List<IPAddress> _motorDriveModAddr;

        /// <summary>
        /// Number of Bullnose Sensors.
        /// </summary>
        private int _nbOfSensors;

        /// <summary>
        /// Number Of Samples per Sensor.
        /// </summary>
        private int _samplesPerSensor;

        /// <summary>
        /// Additional sensor data.
        /// </summary>
        private int _sensorsAdditionalData;

        ///<summary>
        /// Database name to connect to
        //</summary>
        private string _databaseName;

        ///<summary>
        /// Database name to connect to
        //</summary>
        private string _LogdatabaseName;

        #endregion

        #region Properties

        /// <summary>
        /// Property. Gets or sets the Modbus communication socket port.
        /// </summary>
        public int PlcModbusPort
        {
            get
            {
                return _ModbusPort;
            }
            set
            {
                _ModbusPort = value;
            }
        }

        /// <summary>
        /// Property. Gets or sets the IP address used for Modbus communication.
        /// </summary>
        public IPAddress PlcIpAddress
        {
            get
            {
                return _ModbusIP;
            }
            set
            {
                _ModbusIP = value;
            }
        }


        /// <summary>
        /// Property. Gets or sets the communication socket port used for TCP/IP communication.
        /// </summary>
        public int TCPModbusPort
        {
            get
            {
                return _TCPIPport;
            }
            set
            {
                _TCPIPport = value;
            }
        }

        /// <summary>
        /// Property. Gets or sets the IP address used for TCP/IP communication.
        /// </summary>
        public IPAddress PlcTCPAddress
        {
            get
            {
                return _TCPIPAddress;
            }
            set
            {
                _TCPIPAddress = value;
            }
        }

        /// <summary>
        /// Property. Gets or sets the IP address used for camera communication.
        /// </summary>
        public List<IPAddress> CameraIDS
        {
            get
            {
                return _CameraIDs;
            }
            set
            {
                _CameraIDs = value;
            }
        }

        /// <summary>
        /// Property. Gets or sets the IP address used for TCP/IP communication.
        /// </summary>
        public List<IPAddress> MotorDriveModAddr
        {
            get
            {
                return _motorDriveModAddr;
            }
            set
            {
                _motorDriveModAddr = value;
            }
        }

        /// <summary>
        /// Property. Gets the application specific guid for multiple instances.
        /// </summary>
        public Guid GuidApplication
        {
            get
            {
                return _guidApplication;
            }
            protected set
            {
                _guidApplication = value;
            }
        }

        /// <summary>
        /// Property. Gets or Sets the application key.
        /// </summary>
        public int ApplicationKey
        {
            get
            {
                return _applicationKey;
            }
            set
            {
                _applicationKey = value;
            }
        }

        /// <summary>
        /// Property. Gets or Sets the Application Tile.
        /// </summary>
        public String ApplicationTitle
        {
            get
            {
                return _applicationTitle;
            }
            set
            {
                _applicationTitle = value;
            }
        }

        /// <summary>
        /// Property. Gets or Sets the Matrox Project Tile.
        /// </summary>
        public String VisionProjectTitle
        {
            get
            {
                return _visionProjectTitle;
            }
            set
            {
                _visionProjectTitle = value;
            }
        }

        /// <summary>
        /// Property. Gets or Sets the number of Bullnose Sensors.
        /// </summary>
        public int NbOfSensors
        {
            get
            {
                return _nbOfSensors;
            }
            set
            {
                _nbOfSensors = value;
            }
        }

        /// <summary>
        /// Property. Gets or Sets the number of samples per sensor.
        /// </summary>
        public int SamplesPerSensor
        {
            get
            {
                return _samplesPerSensor;
            }
            set
            {
                _samplesPerSensor = value;
            }
        }

        /// <summary>
        /// Property. Gets or Sets the sensor additional data.
        /// </summary>
        public int SensorsAdditionalData
        {
            get
            {
                return _sensorsAdditionalData;
            }
            set
            {
                _sensorsAdditionalData = value;
            }
        }

        ///<summary>
        ///Property. sets the databaseName
        /// </summary>
        public string LogDatabaseName
        {
            get
            {
                return _LogdatabaseName;
            }
        }

        /// <summary>
        /// Property. Gets or sets the culture information (language).
        /// </summary>
        public string  DatabaseName
        {
            get
            {
                return _databaseName;
            }
        }

        /// <summary>
        /// Property. Gets or sets the User Access Dictionary.
        /// </summary>
        public Dictionary<string,uint> UserAccess;

        /// <summary>
        /// Property. Gets or sets the Documents Dictionary.
        /// </summary>
        public Dictionary<string, string> DocumentsDict;


        #endregion

        #region Initializers

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Configuration()
        {
            //
            // Initialize our unique application specific GUID to prevent multiple instances.
            //
            this.GuidApplication = new Guid("02636BC3-811B-46B3-8AEA-C7FD7024F32B");

            //
            // Initialize with local pingback IP by default.
            //
            _ModbusIP = IPAddress.Parse("127.0.0.1");

            //
            // Use engligh (USA) as a default culture.
            //
            _culture = new CultureInfo("en-US", false);
        }

        #endregion

        /// <summary>
        /// Loads and parses the specified configuration .XML file.
        /// </summary>
        /// <param name="fileName">Full .XML configuration file path and name.</param>
        public void Load(string fileName)
        {
            XmlDocument document = null;
            XmlNodeList nodeList = null;
            XmlNode node = null;
            XPathNodeIterator iterator = null;
            string attribute;
            string CurrentUserTxt;
            int password;
            string docName;
            string docType;

            //
            // Create an XmlDocument object to help us parse the XML configuration file from disk.
            //
            document = new XmlDocument();

            document.Load(fileName);
            
            //
            // Retrieve all the configuration values from the file and store them internally.
            //

            //
            // Application configuration.
            //
            nodeList = document.GetElementsByTagName(_applicationNodeName);

            if (nodeList.Count > 0)
            {
                node = nodeList.Item(0);

                _applicationKey = Convert.ToInt32(node.Attributes[_keyAttributeName].Value);

                _applicationTitle = Convert.ToString(node.Attributes[_TitleAttributeName].Value);

                _visionProjectTitle = Convert.ToString(node.Attributes[_visonProjectAttributeName].Value);
                //
                // GuidApplication.
                //
                //this.GuidApplication = new Guid(node.Attributes[_guidApplicationAttributeName].Value);
            }

            //
            // Language configuration.
            //
            nodeList = document.GetElementsByTagName(_languageNodeName);

            if (nodeList.Count > 0)
            {
                node = nodeList.Item(0);

                _culture = new CultureInfo(node.Attributes[_cultureAttributeName].Value, false);
            }

            //
            // Bullnose Configuration.
            //
            nodeList = document.GetElementsByTagName(_bullnoseSettingsNodeName);

            if (nodeList.Count > 0)
            {
                node = nodeList.Item(0);
                _nbOfSensors = Convert.ToInt32(node.Attributes[_nbOfSensorsAttributeName].Value);
                _samplesPerSensor = Convert.ToInt32(node.Attributes[_samplesPerSensorAttributeName].Value);
                _sensorsAdditionalData = Convert.ToInt32(node.Attributes[_additionalDataAttributeName].Value);
                //
                // GuidApplication.
                //
                //this.GuidApplication = new Guid(node.Attributes[_guidApplicationAttributeName].Value);
            }

            //
            // User Access Configuration
            //
            node = document.GetElementsByTagName(_userAccessNodeName)[0];

            if (node != null)
            {
                iterator = node.CreateNavigator().Select("child::" + _userNodeName);
                UserAccess = new Dictionary<string, uint>();

                while (iterator.MoveNext())
                {                   
                    CurrentUserTxt = iterator.Current.GetAttribute(_CurrentUserTxtAttributeName, iterator.Current.NamespaceURI);
                    password = Convert.ToInt32(iterator.Current.GetAttribute(_passwordAttributeName,
                        iterator.Current.NamespaceURI));
                    if (!String.IsNullOrEmpty(CurrentUserTxt) && password != 0)
                    {
                        UserAccess.Add(CurrentUserTxt,Convert.ToUInt32(password));
                    }
                }
            }

            //
            //User Database Configuration
            //
            nodeList = document.GetElementsByTagName(_userDatabaseNodeName);

            if (nodeList.Count > 0)
            {
                node = nodeList.Item(0);
                _databaseName = Convert.ToString(node.Attributes[_nameAttributeName].Value);
            }


            //
            //Log Database Configuration
            //
            nodeList = document.GetElementsByTagName(_logDatabaseNodeName);

            if (nodeList.Count > 0)
            {
                node = nodeList.Item(0);
                _LogdatabaseName = Convert.ToString(node.Attributes[_nameAttributeName].Value);
            }

            //
            // Documents Configuration
            //
            node = document.GetElementsByTagName(_documentsNodeName)[0];

            if (node != null)
            {
                iterator = node.CreateNavigator().Select("child::" + _docNodeName);
                DocumentsDict = new Dictionary<string, string>();

                while (iterator.MoveNext())
                {
                    docName = iterator.Current.GetAttribute(_nameAttributeName, iterator.Current.NamespaceURI);
                    docType = iterator.Current.InnerXml;
                    if (!String.IsNullOrEmpty(docName) && !String.IsNullOrEmpty(docType))
                    {
                        DocumentsDict.Add(docType,docName);
                    }
                }
            }

            //
            // communication protocol configuration
            //
            node = document.GetElementsByTagName(_communicationProtocolNodeName)[0];
            if (node != null)
            {
                iterator = node.CreateNavigator().Select("child::" + _protocolNodeName);
                while (iterator.MoveNext())
                {
                    attribute = iterator.Current.GetAttribute(_typeAttributeName, iterator.Current.NamespaceURI);

                    if ( !String.IsNullOrEmpty(attribute)  &&  attribute.Equals("Modbus") ) 
                    {
                        PlcIpAddress = IPAddress.Parse(iterator.Current.GetAttribute(_ipAttributeName, iterator.Current.NamespaceURI));
                        PlcModbusPort = Convert.ToInt32(iterator.Current.GetAttribute(_portAttributeName,
                                iterator.Current.NamespaceURI));
                    }

                    if (!String.IsNullOrEmpty(attribute) && attribute.Equals("TCP/IP"))
                    {
                        PlcTCPAddress = IPAddress.Parse(iterator.Current.GetAttribute(_ipAttributeName, iterator.Current.NamespaceURI));
                        TCPModbusPort = Convert.ToInt32(iterator.Current.GetAttribute(_portAttributeName,
                                iterator.Current.NamespaceURI));
                    }
                }
            }

            //
            // GigeVision Cameras configuration
            //
            node = document.GetElementsByTagName(_communicationCamerasNodeName)[0];
            if (node != null)
            {
                iterator = node.CreateNavigator().Select("child::" + _camNodeName);
                CameraIDS = new List<IPAddress>();
                while (iterator.MoveNext())
                {
                   CameraIDS.Add(IPAddress.Parse(iterator.Current.GetAttribute(_ipAttributeName, iterator.Current.NamespaceURI)));
                }
            }

            //
            // communication protocol motor drives
            //
            node = document.GetElementsByTagName(_communicationMotorDrivesNodeName)[0];
            if (node != null)
            {
                iterator = node.CreateNavigator().Select("child::" + _protocolNodeName);
                MotorDriveModAddr = new List<IPAddress>();
                while (iterator.MoveNext())
                {
                    attribute = iterator.Current.GetAttribute(_typeAttributeName, iterator.Current.NamespaceURI);

                    if (!String.IsNullOrEmpty(attribute) && attribute.Equals("Modbus"))
                    {
                        MotorDriveModAddr.Add(IPAddress.Parse(iterator.Current.GetAttribute(_ipAttributeName, iterator.Current.NamespaceURI)));
                    }
                }
            }

        }

        /// <summary>
        /// Loads and writes the specified configuration .XML file.
        /// </summary>
        /// <param name="fileName">Full .XML configuration file path and name.</param>
        /// <param name="nodeName">.</param>
        /// <param name="userAccess"></param>
        public bool WriteNodeUserAccess(string fileName )
        {

            XmlDocument document = null;
            XmlNode node = null;
            XPathNodeIterator iterator = null;
            string CurrentUserTxt;
            uint password;

            try
            {
                //
                // Create an XmlDocument object to help us parse the XML configuration file from disk.
                //
                document = new XmlDocument();

                document.Load(fileName);

                //writeNavigator = document.CreateNavigator();

                //
                // User Access Configuration
                //
                node = document.GetElementsByTagName(_userAccessNodeName)[0];

                if (node != null)
                {
                    iterator = node.CreateNavigator().Select("child::" + _userNodeName);

                    while (iterator.MoveNext())
                    {
                        CurrentUserTxt = iterator.Current.GetAttribute(_CurrentUserTxtAttributeName, iterator.Current.NamespaceURI);
                        if (UserAccess.TryGetValue(CurrentUserTxt, out password))
                        {
                            iterator.Current.MoveToAttribute(_passwordAttributeName, iterator.Current.NamespaceURI);
                            iterator.Current.SetValue(password.ToString());
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Failed to write into XML configuration file");
                return false;
            }
           
            document.Save(fileName);
            return true;
        }

    }
}
