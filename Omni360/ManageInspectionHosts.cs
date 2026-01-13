using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zebra.ADA.OperatorAPI;
using Zebra.ADA.OperatorAPI.Communication;
using Zebra.ADA.OperatorAPI.ControlsPackage;
using Zebra.ADA.OperatorAPI.ExecutionMessagesPackage;
using Zebra.ADA.OperatorAPI.HostServices;
using Zebra.ADA.OperatorAPI.InputsPackage;
using Zebra.ADA.OperatorAPI.OutputsPackage;

namespace MatroxLDS
{

    public delegate void InspectionReadyDelegate();

    public class ManageInspectionHosts
    {
        public MainForm mainForm;

        public Host Host { get; private set; }
        private Project _startProject ;




        public ManageInspectionHosts(Host host,Project project)
        {
            Host = host;
            _startProject = project;

            Host.ConnectionStateChanged += OnHostConnectionStateChange;
        }


        private void OnHostConnectionStateChange(object sender, ConnectionStateChangedArgs<HostConnectionState> args)
        {
            switch (args.CurrentState)
            {
                case HostConnectionState.Connected:
                        //MessageBox.Show(MainForm.Instance, $"Connected to host project state {_startProject.State} ");
                        if (_startProject.State == ProjectState.Stopped)
                        {
                            try
                            {
                                Host.StartProjectAsync(_startProject.Name);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(MainForm.Instance,$"Error while starting project: {ex.Message}");
                            }
                        }

                   break;
            }
        }

    }
}
