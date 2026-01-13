using System;
using System.Windows.Forms;
using System.Drawing;

namespace MatroxLDS
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
            // Keep the splash on top during startup
            this.TopMost = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
        }

        // Defensive UpdateProgress: returns quietly if the form is disposed or closing,
        // and catches ObjectDisposed/InvalidOperation exceptions when marshaling to the UI thread.
        public void UpdateProgress(string message)
        {
            try
            {
                if (this.IsDisposed || this.Disposing) return;

                if (this.InvokeRequired)
                {
                    try
                    {
                        // Use BeginInvoke and swallow if the control gets disposed before the delegate runs
                        this.BeginInvoke(new Action(() =>
                        {
                            try
                            {
                                if (this.IsDisposed || this.Disposing) return;
                                progressTextBox.AppendText(message + Environment.NewLine);
                                Application.DoEvents(); // Keep UI responsive
                            }
                            catch
                            {
                                // Ignore any exceptions caused by the control being disposed during invoke
                            }
                        }));
                    }
                    catch (ObjectDisposedException) { /* control already disposed */ }
                    catch (InvalidOperationException) { /* control handle invalid */ }
                    catch { /* swallow other marshal exceptions */ }
                }
                else
                {
                    try
                    {
                        progressTextBox.AppendText(message + Environment.NewLine);
                        Application.DoEvents();
                    }
                    catch (ObjectDisposedException) { /* ignore */ }
                    catch (InvalidOperationException) { /* ignore */ }
                    catch { /* swallow */ }
                }
            }
            catch
            {
                // Best-effort: never throw from UpdateProgress
            }
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            // Initialize tasks if any
        }
    }
}