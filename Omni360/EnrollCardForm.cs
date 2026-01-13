using System;
using System.Windows.Forms;

namespace MatroxLDS
{
    public partial class EnrollCardForm : Form
    {
        private readonly UsersManager _usersManager;
        private readonly CardReaderService _readerService;
        private readonly string _username;

        public EnrollCardForm(UsersManager usersManager, CardReaderService readerService, string username)
        {
            InitializeComponent();

            if (usersManager == null) throw new ArgumentNullException(nameof(usersManager));
            if (readerService == null) throw new ArgumentNullException(nameof(readerService));
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException(nameof(username));

            _usersManager = usersManager;
            _readerService = readerService;
            _username = username;

            Text = $"Enroll card for {_username}";
            StartPosition = FormStartPosition.CenterParent;
            Width = 420;
            Height = 200;

            var lbl = new Label
            {
                Text = "Present card to reader to enroll...",
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            Controls.Add(lbl);

            var btnCancel = new Button { Text = "Cancel", Dock = DockStyle.Bottom, Height = 36 };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            Controls.Add(btnCancel);

            // Subscribe to the singleton service's CardPresented event
            _readerService.CardPresented += Reader_CardPresented;
        }

        private void Reader_CardPresented(string cardId)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => Reader_CardPresented(cardId)));
                return;
            }

            var confirm = MessageBox.Show(this, $"Enroll card '{cardId}' to user '{_username}'?", "Confirm Enroll", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
                return;

            try
            {
                var previous = _usersManager.EnrollCardToUser(_username, cardId);
                MessageBox.Show(this, previous == null ? "Enrolled successfully." : $"Card reassigned from {previous} to {_username}.", "Enroll", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Enroll failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
            }
            finally
            {
                Close();
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            try { _readerService.CardPresented -= Reader_CardPresented; } catch { }
            base.OnFormClosed(e);
        }
    }
}