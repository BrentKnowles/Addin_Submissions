using System;
using System.Windows.Forms;
using Layout;
using CoreUtilities;

namespace Submissions
{
	public class AddEditSubmissionsForm : Form
	{

		#region gui
		public SubmissionEditPanel SubEditPanel = null;
		#endregion

	

		public AddEditSubmissionsForm (bool ProvideDefaults)
		{
			this.StartPosition = FormStartPosition.CenterScreen;
			SubEditPanel = new SubmissionEditPanel(ProvideDefaults);
			SubEditPanel.Dock = DockStyle.Fill;
			this.Controls.Add (SubEditPanel);
			this.Width = 1075;
			this.Height = 850;
			this.Icon = LayoutDetails.Instance.MainFormIcon;
			FormUtils.SizeFormsForAccessibility(this, LayoutDetails.Instance.MainFormFontSize);

			Panel bottomPanel = new Panel();
			bottomPanel.Dock = DockStyle.Bottom;
			bottomPanel.Height = LayoutDetails.ButtonHeight;

			Button ok = new Button();
			ok.DialogResult = System.Windows.Forms.DialogResult.OK;
			ok.Height = LayoutDetails.ButtonHeight;
			ok.Dock = DockStyle.Left;
			ok.Text = Loc.Instance.GetString ("OK");

			Button cancel = new Button();
			cancel.DialogResult = DialogResult.Cancel;
			cancel.Height = LayoutDetails.ButtonHeight;
			cancel.Dock = DockStyle.Right;
			cancel.Text = Loc.Instance.GetString ("Cancel");

			bottomPanel.Controls.Add (ok);
			bottomPanel.Controls.Add (cancel);

			this.Controls.Add (bottomPanel);
			this.Controls.Add (SubEditPanel);
			SubEditPanel.BringToFront();


		}
	}
}

