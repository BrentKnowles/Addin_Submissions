namespace Submissions
{
    partial class SubmissionEditPanel
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.notes = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textDaysOut = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dateReply = new System.Windows.Forms.DateTimePicker();
            this.rights = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.earned = new System.Windows.Forms.NumericUpDown();
            this.feedback = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.replytype = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxDraft = new System.Windows.Forms.TextBox();
            this.textBoxSale = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.labelHiatus = new System.Windows.Forms.Label();
            this.prioritylabel = new System.Windows.Forms.Label();
            this.ePriority = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateSubmission = new System.Windows.Forms.DateTimePicker();
            this.postage = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.SubmissionType = new System.Windows.Forms.ComboBox();
            this.panelHeader = new System.Windows.Forms.Panel();
         //   this.editMarketName = new System.Windows.Forms.LinkLabel();
            this.labelMarket = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.earned)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.postage)).BeginInit();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.panelHeader);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(740, 501);
            this.panel1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.panel2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupBox2.Location = new System.Drawing.Point(5, 184);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(728, 310);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Reply Details";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.notes);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupBox3.Location = new System.Drawing.Point(313, 22);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox3.Size = new System.Drawing.Size(412, 285);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "  Notes";
            // 
            // notes
            // 
            this.notes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.notes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notes.Location = new System.Drawing.Point(5, 20);
            this.notes.Name = "notes";
            this.notes.Size = new System.Drawing.Size(402, 260);
            this.notes.TabIndex = 9;
        //    this.notes.Text = global::Worgan2006.Header.Blank;
            this.notes.TextChanged += new System.EventHandler(this.notes_TextChanged);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel2.Controls.Add(this.textDaysOut);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.dateReply);
            this.panel2.Controls.Add(this.rights);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.earned);
            this.panel2.Controls.Add(this.feedback);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.replytype);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(3, 22);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(310, 285);
            this.panel2.TabIndex = 4;
            // 
            // textDaysOut
            // 
            this.textDaysOut.Location = new System.Drawing.Point(138, 229);
            this.textDaysOut.Name = "textDaysOut";
            this.textDaysOut.ReadOnly = true;
            this.textDaysOut.Size = new System.Drawing.Size(162, 26);
            this.textDaysOut.TabIndex = 17;
            this.toolTip1.SetToolTip(this.textDaysOut, "If this submission has not been \"returned\" this field will \r\ndisplay the number o" +
                    "f days since submission until today.\r\nOtherwise, it will use the reply date as t" +
                    "he end date.\r\n");
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label9.Location = new System.Drawing.Point(3, 229);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 16);
            this.label9.TabIndex = 18;
            this.label9.Text = "Days Out";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Location = new System.Drawing.Point(3, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 16);
            this.label4.TabIndex = 8;
            this.label4.Text = "Amount of Sale";
            // 
            // dateReply
            // 
            this.dateReply.Enabled = false;
            this.dateReply.Location = new System.Drawing.Point(138, 47);
            this.dateReply.Name = "dateReply";
            this.dateReply.Size = new System.Drawing.Size(162, 26);
            this.dateReply.TabIndex = 5;
            this.dateReply.ValueChanged += new System.EventHandler(this.dateReply_ValueChanged);
            // 
            // rights
            // 
            this.rights.Location = new System.Drawing.Point(138, 149);
            this.rights.Multiline = true;
            this.rights.Name = "rights";
            this.rights.Size = new System.Drawing.Size(162, 74);
            this.rights.TabIndex = 8;
            this.rights.TextChanged += new System.EventHandler(this.rights_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label5.Location = new System.Drawing.Point(3, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 16);
            this.label5.TabIndex = 10;
            this.label5.Text = "Reply Date";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label8.Location = new System.Drawing.Point(3, 149);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 16);
            this.label8.TabIndex = 16;
            this.label8.Text = "Rights Sold";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label7.Location = new System.Drawing.Point(3, 84);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 16);
            this.label7.TabIndex = 14;
            this.label7.Text = "Reply Type";
            // 
            // earned
            // 
            this.earned.DecimalPlaces = 2;
            this.earned.Location = new System.Drawing.Point(138, 13);
            this.earned.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.earned.Name = "earned";
            this.earned.Size = new System.Drawing.Size(162, 26);
            this.earned.TabIndex = 4;
            this.earned.ValueChanged += new System.EventHandler(this.earned_ValueChanged);
            // 
            // feedback
            // 
            this.feedback.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.feedback.FormattingEnabled = true;
            this.feedback.Location = new System.Drawing.Point(138, 115);
            this.feedback.Name = "feedback";
            this.feedback.Size = new System.Drawing.Size(162, 28);
            this.feedback.TabIndex = 7;
            this.feedback.SelectedIndexChanged += new System.EventHandler(this.feedback_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.Location = new System.Drawing.Point(3, 115);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 16);
            this.label6.TabIndex = 12;
            this.label6.Text = "Reply Feedback";
            // 
            // replytype
            // 
            this.replytype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.replytype.FormattingEnabled = true;
            this.replytype.Location = new System.Drawing.Point(138, 79);
            this.replytype.Name = "replytype";
            this.replytype.Size = new System.Drawing.Size(162, 28);
            this.replytype.TabIndex = 6;
            this.replytype.SelectedIndexChanged += new System.EventHandler(this.replytype_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.textBoxDraft);
            this.groupBox1.Controls.Add(this.textBoxSale);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.labelHiatus);
            this.groupBox1.Controls.Add(this.prioritylabel);
            this.groupBox1.Controls.Add(this.ePriority);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dateSubmission);
            this.groupBox1.Controls.Add(this.postage);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.SubmissionType);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupBox1.Location = new System.Drawing.Point(5, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(728, 152);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Submission Details";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label11.Location = new System.Drawing.Point(315, 33);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(80, 16);
            this.label11.TabIndex = 12;
            this.label11.Text = "Draft Sent";
            this.toolTip1.SetToolTip(this.label11, "Enter the version (i.e., draft 1, draft 2) of this story that was submitted.");
            // 
            // textBoxDraft
            // 
            this.textBoxDraft.Location = new System.Drawing.Point(432, 27);
            this.textBoxDraft.Name = "textBoxDraft";
            this.textBoxDraft.Size = new System.Drawing.Size(200, 26);
            this.textBoxDraft.TabIndex = 11;
            this.textBoxDraft.TextChanged += new System.EventHandler(this.textBoxDraft_TextChanged);
            // 
            // textBoxSale
            // 
            this.textBoxSale.Location = new System.Drawing.Point(432, 97);
            this.textBoxSale.Name = "textBoxSale";
            this.textBoxSale.ReadOnly = true;
            this.textBoxSale.Size = new System.Drawing.Size(80, 26);
            this.textBoxSale.TabIndex = 10;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label10.Location = new System.Drawing.Point(315, 97);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(106, 16);
            this.label10.TabIndex = 9;
            this.label10.Text = "Potential Sale";
            this.toolTip1.SetToolTip(this.label10, "This is an estimate of how much this submission would earn, if accepted");
            // 
            // labelHiatus
            // 
            this.labelHiatus.AutoSize = true;
            this.labelHiatus.BackColor = System.Drawing.Color.Orange;
            this.labelHiatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelHiatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHiatus.ForeColor = System.Drawing.Color.SteelBlue;
            this.labelHiatus.Location = new System.Drawing.Point(171, 126);
            this.labelHiatus.Name = "labelHiatus";
            this.labelHiatus.Size = new System.Drawing.Size(309, 20);
            this.labelHiatus.TabIndex = 8;
            this.labelHiatus.Text = "Market on Hiatus or outside reading period";
            this.labelHiatus.Visible = false;
            // 
            // prioritylabel
            // 
            this.prioritylabel.AutoSize = true;
            this.prioritylabel.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prioritylabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.prioritylabel.Location = new System.Drawing.Point(6, 103);
            this.prioritylabel.Name = "prioritylabel";
            this.prioritylabel.Size = new System.Drawing.Size(65, 16);
            this.prioritylabel.TabIndex = 7;
            this.prioritylabel.Text = "Priority";
            this.toolTip1.SetToolTip(this.prioritylabel, "For destinations this number will be used to rank the markets\r\n ");
            // 
            // ePriority
            // 
            this.ePriority.Location = new System.Drawing.Point(141, 99);
            this.ePriority.Name = "ePriority";
            this.ePriority.Size = new System.Drawing.Size(80, 26);
            this.ePriority.TabIndex = 6;
            this.ePriority.TextChanged += new System.EventHandler(this.ePriority_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(6, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Submission Date";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(315, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Postage";
            // 
            // dateSubmission
            // 
            this.dateSubmission.Location = new System.Drawing.Point(141, 33);
            this.dateSubmission.Name = "dateSubmission";
            this.dateSubmission.Size = new System.Drawing.Size(162, 26);
            this.dateSubmission.TabIndex = 1;
            this.dateSubmission.ValueChanged += new System.EventHandler(this.dateSubmission_ValueChanged);
            // 
            // postage
            // 
            this.postage.DecimalPlaces = 2;
            this.postage.Location = new System.Drawing.Point(432, 65);
            this.postage.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.postage.Name = "postage";
            this.postage.Size = new System.Drawing.Size(162, 26);
            this.postage.TabIndex = 3;
            this.postage.ValueChanged += new System.EventHandler(this.postage_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(6, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Submission Type";
            // 
            // SubmissionType
            // 
            this.SubmissionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SubmissionType.FormattingEnabled = true;
            this.SubmissionType.Location = new System.Drawing.Point(141, 65);
            this.SubmissionType.Name = "SubmissionType";
            this.SubmissionType.Size = new System.Drawing.Size(162, 28);
            this.SubmissionType.TabIndex = 2;
            this.SubmissionType.SelectedIndexChanged += new System.EventHandler(this.SubmissionType_SelectedIndexChanged);
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panelHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         //   this.panelHeader.Controls.Add(this.editMarketName);
            this.panelHeader.Controls.Add(this.labelMarket);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(5, 5);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(728, 27);
            this.panelHeader.TabIndex = 0;
            // 
            // editMarketName
            // 
//            this.editMarketName.AutoSize = true;
//            this.editMarketName.Dock = System.Windows.Forms.DockStyle.Left;
//            this.editMarketName.Location = new System.Drawing.Point(219, 0);
//            this.editMarketName.Name = "editMarketName";
//            this.editMarketName.Size = new System.Drawing.Size(50, 13);
//            this.editMarketName.TabIndex = 0;
//            this.editMarketName.TabStop = true;
//            this.editMarketName.Text = "(Change)";
//            this.editMarketName.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.editMarketName_LinkClicked);
            // 
            // labelMarket
            // 
            this.labelMarket.AutoSize = true;
            this.labelMarket.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelMarket.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelMarket.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMarket.Location = new System.Drawing.Point(0, 0);
            this.labelMarket.Name = "labelMarket";
            this.labelMarket.Size = new System.Drawing.Size(219, 24);
            this.labelMarket.TabIndex = 0;
            this.labelMarket.Text = "Select or add a market";
            this.labelMarket.Click += new System.EventHandler(this.labelMarket_Click);
            // 
            // SubmissionEditPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panel1);
            this.Name = "SubmissionEditPanel";
            this.Size = new System.Drawing.Size(740, 501);
            this.Load += new System.EventHandler(this.SubmissionEditPanel_Load);
            this.Leave += new System.EventHandler(this.SubmissionEditPanel_Leave);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.earned)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.postage)).EndInit();
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelHeader;
     //   private System.Windows.Forms.LinkLabel editMarketName;
        private System.Windows.Forms.ComboBox SubmissionType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown postage;
        private System.Windows.Forms.DateTimePicker dateSubmission;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox feedback;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dateReply;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown earned;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox replytype;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox rights;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox textDaysOut;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox ePriority;
        private System.Windows.Forms.Label prioritylabel;
        public System.Windows.Forms.RichTextBox notes;
        public System.Windows.Forms.Label labelHiatus;
        private System.Windows.Forms.Label labelMarket;
        private System.Windows.Forms.TextBox textBoxSale;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxDraft;
    }
}
