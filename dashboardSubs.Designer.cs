// dashboardSubs.Designer.cs
//
// Copyright (c) 2013 Brent Knowles (http://www.brentknowles.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// Review documentation at http://www.yourothermind.com for updated implementation notes, license updates
// or other general information/
// 
// Author information available at http://www.brentknowles.com or http://www.amazon.com/Brent-Knowles/e/B0035WW7OW
// Full source code: https://github.com/BrentKnowles/YourOtherMind
//###
using System.Windows.Forms;
namespace MefAddIns
{
	partial class dashboardSubs
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
           // this.checkBoxStatus = new System.Windows.Forms.CheckBox();
            this.checkBoxSent = new System.Windows.Forms.CheckBox();
            this.checkBoxReadyToSend = new System.Windows.Forms.CheckBox();
            this.buttonRefresh = new System.Windows.Forms.Button();
           // this.comboBoxStatus = new System.Windows.Forms.ComboBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
          //  this.panel1.Controls.Add(this.checkBoxStatus);
            this.panel1.Controls.Add(this.checkBoxSent);
            this.panel1.Controls.Add(this.checkBoxReadyToSend);
            this.panel1.Controls.Add(this.buttonRefresh);
         //   this.panel1.Controls.Add(this.comboBoxStatus);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 119);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(648, 31);
            this.panel1.TabIndex = 1;
            // 
            // checkBoxStatus
            // 
//            this.checkBoxStatus.AutoSize = true;
//            this.checkBoxStatus.Dock = System.Windows.Forms.DockStyle.Right;
//            this.checkBoxStatus.Location = new System.Drawing.Point(456, 0);
//            this.checkBoxStatus.Name = "checkBoxStatus";
//            this.checkBoxStatus.Size = new System.Drawing.Size(71, 31);
//            this.checkBoxStatus.TabIndex = 3;
//            this.checkBoxStatus.Text = "By Status";
//            this.checkBoxStatus.UseVisualStyleBackColor = true;
//            this.checkBoxStatus.CheckedChanged += new System.EventHandler(this.checkBoxStatus_CheckedChanged);
            // 
            // checkBoxSent
            // 
            this.checkBoxSent.AutoSize = true;
            this.checkBoxSent.Checked = false;
            this.checkBoxSent.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSent.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBoxSent.Location = new System.Drawing.Point(176, 0);
            this.checkBoxSent.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.checkBoxSent.Name = "checkBoxSent";
            this.checkBoxSent.Size = new System.Drawing.Size(48, 31);
            this.checkBoxSent.TabIndex = 2;
            this.checkBoxSent.Text = "Sent";
            this.checkBoxSent.UseVisualStyleBackColor = true;
            this.checkBoxSent.CheckedChanged += new System.EventHandler(this.checkBoxSent_CheckedChanged);
            // 
            // checkBoxReadyToSend
            // 
            this.checkBoxReadyToSend.AutoSize = true;
            this.checkBoxReadyToSend.Checked = false;
            this.checkBoxReadyToSend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxReadyToSend.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBoxReadyToSend.Location = new System.Drawing.Point(75, 0);
            this.checkBoxReadyToSend.Name = "checkBoxReadyToSend";
            this.checkBoxReadyToSend.Size = new System.Drawing.Size(101, 31);
            this.checkBoxReadyToSend.TabIndex = 1;
            this.checkBoxReadyToSend.Text = "Ready To Send";
            this.checkBoxReadyToSend.UseVisualStyleBackColor = true;
            this.checkBoxReadyToSend.CheckedChanged += new System.EventHandler(this.checkBoxReadyToSend_CheckedChanged);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonRefresh.Location = new System.Drawing.Point(0, 0);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(75, 31);
            this.buttonRefresh.TabIndex = 0;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // comboBoxStatus
            // 
//            this.comboBoxStatus.Dock = System.Windows.Forms.DockStyle.Right;
//            this.comboBoxStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
//            this.comboBoxStatus.Enabled = false;
//            this.comboBoxStatus.FormattingEnabled = true;
//            this.comboBoxStatus.Location = new System.Drawing.Point(527, 0);
//            this.comboBoxStatus.Name = "comboBoxStatus";
//            this.comboBoxStatus.Size = new System.Drawing.Size(121, 21);
//            this.comboBoxStatus.TabIndex = 5;
//            this.comboBoxStatus.SelectedIndexChanged += new System.EventHandler(this.comboBoxStatus_SelectedIndexChanged);
//            this.comboBoxStatus.DropDown += new System.EventHandler(this.comboBoxStatus_DropDown);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader4,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.ShowItemToolTips = true;
            this.listView1.Size = new System.Drawing.Size(648, 119);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            this.listView1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseMove);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Story";
            this.columnHeader1.Width = 200;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Words";
            this.columnHeader4.Width = 80;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Submission Status";
            this.columnHeader2.Width = 200;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Next Market";
            this.columnHeader3.Width = 200;
            // 
            // dashboardSubs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.panel1);
            this.Name = "dashboardSubs";
            this.Size = new System.Drawing.Size(648, 150);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.CheckBox checkBoxSent;
        private System.Windows.Forms.CheckBox checkBoxReadyToSend;
//        private System.Windows.Forms.CheckBox checkBoxStatus;
//        private System.Windows.Forms.ComboBox comboBoxStatus;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ColumnHeader columnHeader4;
    }
}
