namespace FlexColParser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.openbutton = new System.Windows.Forms.ToolStripButton();
            this.mecagoendios = new System.Windows.Forms.ToolStripButton();
            this.openJSONToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.checkBoxExtendColHeap = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox69 = new System.Windows.Forms.TextBox();
            this.textBoxObjectName = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.openbutton, this.mecagoendios });
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(799, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // openbutton
            // 
            this.openbutton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.openbutton.Image = ((System.Drawing.Image)(resources.GetObject("openbutton.Image")));
            this.openbutton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openbutton.Name = "openbutton";
            this.openbutton.Size = new System.Drawing.Size(40, 22);
            this.openbutton.Text = "Open";
            this.openbutton.ToolTipText = "FileSelect";
            this.openbutton.Click += new System.EventHandler(this.openbutton_Click);
            // 
            // mecagoendios
            // 
            this.mecagoendios.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.mecagoendios.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mecagoendios.Name = "mecagoendios";
            this.mecagoendios.Size = new System.Drawing.Size(35, 22);
            this.mecagoendios.Text = "Save";
            this.mecagoendios.Click += new System.EventHandler(this.mecagoendios_Click);
            // 
            // openJSONToolStripMenuItem
            // 
            this.openJSONToolStripMenuItem.Name = "openJSONToolStripMenuItem";
            this.openJSONToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openJSONToolStripMenuItem.Text = "Open JSON";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 93);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(194, 342);
            this.listBox1.TabIndex = 4;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 37);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(194, 40);
            this.button1.TabIndex = 6;
            this.button1.Text = "Load .OBJ";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(222, 67);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(566, 368);
            this.propertyGrid1.TabIndex = 7;
            this.propertyGrid1.Click += new System.EventHandler(this.propertyGrid1_Click);
            // 
            // checkBoxExtendColHeap
            // 
            this.checkBoxExtendColHeap.Location = new System.Drawing.Point(668, 46);
            this.checkBoxExtendColHeap.Name = "checkBoxExtendColHeap";
            this.checkBoxExtendColHeap.Size = new System.Drawing.Size(109, 24);
            this.checkBoxExtendColHeap.TabIndex = 8;
            this.checkBoxExtendColHeap.Text = "extend_col_heap";
            this.checkBoxExtendColHeap.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox69);
            this.groupBox1.Controls.Add(this.textBoxObjectName);
            this.groupBox1.Location = new System.Drawing.Point(222, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(440, 39);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Stuff";
            // 
            // textBox69
            // 
            this.textBox69.Location = new System.Drawing.Point(189, 13);
            this.textBox69.Name = "textBox69";
            this.textBox69.Size = new System.Drawing.Size(100, 20);
            this.textBox69.TabIndex = 1;
            this.textBox69.Text = "Vss_Something";
            // 
            // textBoxObjectName
            // 
            this.textBoxObjectName.Location = new System.Drawing.Point(6, 13);
            this.textBoxObjectName.Name = "textBoxObjectName";
            this.textBoxObjectName.Size = new System.Drawing.Size(100, 20);
            this.textBoxObjectName.TabIndex = 0;
            this.textBoxObjectName.Text = "Fld_Something";
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(799, 448);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkBoxExtendColHeap);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.toolStrip1);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "Flexlion Collision Helper";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ToolStripButton toolStripButton1;

        private System.Windows.Forms.TextBox textBox69;

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxObjectName;

        private System.Windows.Forms.CheckBox checkBoxExtendColHeap;

        private System.Windows.Forms.PropertyGrid propertyGrid1;

        private System.Windows.Forms.Button button1;

        private System.Windows.Forms.ListBox listBox1;

        private System.Windows.Forms.ToolStripMenuItem openJSONToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton mecagoendios;

        private System.Windows.Forms.ToolStripButton openbutton;

        private System.Windows.Forms.ToolStrip toolStrip1;

        #endregion
    }
}