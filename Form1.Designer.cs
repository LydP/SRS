
namespace SRS
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cardFront = new System.Windows.Forms.Label();
            this.cardBack = new System.Windows.Forms.Label();
            this.rightWrongButtonPanel = new System.Windows.Forms.Panel();
            this.showCardButton = new System.Windows.Forms.Button();
            this.rightWrongButtonTable = new System.Windows.Forms.TableLayoutPanel();
            this.wrong = new System.Windows.Forms.Button();
            this.right = new System.Windows.Forms.Button();
            this.viewingTimerLabel = new System.Windows.Forms.Label();
            this.viewingTimer = new System.Windows.Forms.Timer(this.components);
            this.startButton = new System.Windows.Forms.Button();
            this.doneLabel = new System.Windows.Forms.Label();
            this.sessionCheckPanel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.rightWrongButtonPanel.SuspendLayout();
            this.rightWrongButtonTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.cardFront, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cardBack, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.rightWrongButtonPanel, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 151F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1504, 1286);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Visible = false;
            // 
            // cardFront
            // 
            this.cardFront.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.cardFront, 2);
            this.cardFront.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardFront.Location = new System.Drawing.Point(4, 1);
            this.cardFront.Name = "cardFront";
            this.cardFront.Size = new System.Drawing.Size(1496, 565);
            this.cardFront.TabIndex = 3;
            // 
            // cardBack
            // 
            this.cardBack.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.cardBack, 2);
            this.cardBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cardBack.Location = new System.Drawing.Point(4, 567);
            this.cardBack.Name = "cardBack";
            this.cardBack.Size = new System.Drawing.Size(1496, 565);
            this.cardBack.TabIndex = 4;
            this.cardBack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rightWrongButtonPanel
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.rightWrongButtonPanel, 2);
            this.rightWrongButtonPanel.Controls.Add(this.showCardButton);
            this.rightWrongButtonPanel.Controls.Add(this.rightWrongButtonTable);
            this.rightWrongButtonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightWrongButtonPanel.Location = new System.Drawing.Point(4, 1136);
            this.rightWrongButtonPanel.Name = "rightWrongButtonPanel";
            this.rightWrongButtonPanel.Size = new System.Drawing.Size(1496, 146);
            this.rightWrongButtonPanel.TabIndex = 5;
            // 
            // showCardButton
            // 
            this.showCardButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.showCardButton.Location = new System.Drawing.Point(0, 0);
            this.showCardButton.Name = "showCardButton";
            this.showCardButton.Size = new System.Drawing.Size(1496, 146);
            this.showCardButton.TabIndex = 1;
            this.showCardButton.Text = "Show Answer";
            this.showCardButton.UseVisualStyleBackColor = true;
            this.showCardButton.Click += new System.EventHandler(this.showCardButton_Click);
            // 
            // rightWrongButtonTable
            // 
            this.rightWrongButtonTable.ColumnCount = 2;
            this.rightWrongButtonTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.rightWrongButtonTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.rightWrongButtonTable.Controls.Add(this.wrong, 0, 0);
            this.rightWrongButtonTable.Controls.Add(this.right, 1, 0);
            this.rightWrongButtonTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightWrongButtonTable.Location = new System.Drawing.Point(0, 0);
            this.rightWrongButtonTable.Name = "rightWrongButtonTable";
            this.rightWrongButtonTable.RowCount = 2;
            this.rightWrongButtonTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.79279F));
            this.rightWrongButtonTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.207207F));
            this.rightWrongButtonTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.rightWrongButtonTable.Size = new System.Drawing.Size(1496, 146);
            this.rightWrongButtonTable.TabIndex = 0;
            // 
            // wrong
            // 
            this.wrong.BackColor = System.Drawing.Color.Red;
            this.wrong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wrong.Location = new System.Drawing.Point(3, 3);
            this.wrong.Name = "wrong";
            this.wrong.Size = new System.Drawing.Size(742, 129);
            this.wrong.TabIndex = 0;
            this.wrong.UseVisualStyleBackColor = false;
            this.wrong.Visible = false;
            this.wrong.Click += new System.EventHandler(this.wrong_Click_1);
            // 
            // right
            // 
            this.right.BackColor = System.Drawing.Color.Green;
            this.right.Dock = System.Windows.Forms.DockStyle.Fill;
            this.right.Location = new System.Drawing.Point(751, 3);
            this.right.Name = "right";
            this.right.Size = new System.Drawing.Size(742, 129);
            this.right.TabIndex = 1;
            this.right.UseVisualStyleBackColor = false;
            this.right.Visible = false;
            this.right.Click += new System.EventHandler(this.right_Click_1);
            // 
            // viewingTimerLabel
            // 
            this.viewingTimerLabel.AutoSize = true;
            this.viewingTimerLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.viewingTimerLabel.Location = new System.Drawing.Point(1446, 0);
            this.viewingTimerLabel.Name = "viewingTimerLabel";
            this.viewingTimerLabel.Size = new System.Drawing.Size(58, 32);
            this.viewingTimerLabel.TabIndex = 1;
            this.viewingTimerLabel.Text = "0:00";
            // 
            // viewingTimer
            // 
            this.viewingTimer.Interval = 1000;
            this.viewingTimer.Tick += new System.EventHandler(this.viewingTimer_Tick);
            // 
            // startButton
            // 
            this.startButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.startButton.AutoSize = true;
            this.startButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.startButton.Location = new System.Drawing.Point(473, 539);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(558, 209);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "START";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // doneLabel
            // 
            this.doneLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.doneLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 49.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.doneLabel.Location = new System.Drawing.Point(0, 0);
            this.doneLabel.Name = "doneLabel";
            this.doneLabel.Size = new System.Drawing.Size(1446, 1286);
            this.doneLabel.TabIndex = 4;
            this.doneLabel.Text = "Done!";
            this.doneLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.doneLabel.Visible = false;
            // 
            // sessionCheckPanel
            // 
            this.sessionCheckPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sessionCheckPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 49.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.sessionCheckPanel.Location = new System.Drawing.Point(0, 0);
            this.sessionCheckPanel.Name = "sessionCheckPanel";
            this.sessionCheckPanel.Size = new System.Drawing.Size(1446, 1286);
            this.sessionCheckPanel.TabIndex = 5;
            this.sessionCheckPanel.Text = "You\'ve completed today\'s lesson.";
            this.sessionCheckPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.sessionCheckPanel.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1504, 1286);
            this.Controls.Add(this.sessionCheckPanel);
            this.Controls.Add(this.doneLabel);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.viewingTimerLabel);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.rightWrongButtonPanel.ResumeLayout(false);
            this.rightWrongButtonTable.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label cardFront;
        private System.Windows.Forms.Label cardBack;
        private System.Windows.Forms.Panel rightWrongButtonPanel;
        private System.Windows.Forms.TableLayoutPanel rightWrongButtonTable;
        private System.Windows.Forms.Button wrong;
        private System.Windows.Forms.Button right;
        private System.Windows.Forms.Button showCardButton;
        private System.Windows.Forms.Timer viewingTimer;
        private System.Windows.Forms.Label viewingTimerLabel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label doneLabel;
        private System.Windows.Forms.Label sessionCheckPanel;
    }
}

