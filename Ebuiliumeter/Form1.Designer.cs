namespace Ebuiliumeter
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.uc41 = new Ebuiliumeter.uc4();
            this.uc31 = new Ebuiliumeter.uc3();
            this.uc21 = new Ebuiliumeter.uc2();
            this.uc11 = new Ebuiliumeter.uc1();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.uc41);
            this.panel1.Controls.Add(this.uc31);
            this.panel1.Controls.Add(this.uc21);
            this.panel1.Controls.Add(this.uc11);
            this.panel1.Location = new System.Drawing.Point(248, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(790, 537);
            this.panel1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(1, 143);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(244, 58);
            this.button1.TabIndex = 1;
            this.button1.Text = "Lab Specific Testing";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button1.MouseEnter += new System.EventHandler(this.button1_MouseEnter);
            this.button1.MouseLeave += new System.EventHandler(this.button1_MouseLeave);
            this.button1.MouseHover += new System.EventHandler(this.button1_MouseHover);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(1, 207);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(244, 58);
            this.button2.TabIndex = 2;
            this.button2.Text = "Public Testing";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            this.button2.MouseEnter += new System.EventHandler(this.button2_MouseEnter);
            this.button2.MouseLeave += new System.EventHandler(this.button2_MouseLeave);
            this.button2.MouseHover += new System.EventHandler(this.button2_MouseHover_1);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(1, 271);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(244, 58);
            this.button3.TabIndex = 3;
            this.button3.Text = "Settings";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            this.button3.MouseEnter += new System.EventHandler(this.button3_MouseEnter);
            this.button3.MouseLeave += new System.EventHandler(this.button3_MouseLeave);
            this.button3.MouseHover += new System.EventHandler(this.button3_MouseHover);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(1, 335);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(244, 58);
            this.button4.TabIndex = 4;
            this.button4.Text = "Register/Log In";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            this.button4.MouseEnter += new System.EventHandler(this.button4_MouseEnter);
            this.button4.MouseLeave += new System.EventHandler(this.button4_MouseLeave);
            this.button4.MouseHover += new System.EventHandler(this.button4_MouseHover);
            // 
            // uc41
            // 
            this.uc41.BackColor = System.Drawing.Color.White;
            this.uc41.Location = new System.Drawing.Point(3, 0);
            this.uc41.Name = "uc41";
            this.uc41.Size = new System.Drawing.Size(793, 529);
            this.uc41.TabIndex = 3;
            // 
            // uc31
            // 
            this.uc31.BackColor = System.Drawing.Color.White;
            this.uc31.Location = new System.Drawing.Point(3, 0);
            this.uc31.Name = "uc31";
            this.uc31.Size = new System.Drawing.Size(793, 529);
            this.uc31.TabIndex = 2;
            // 
            // uc21
            // 
            this.uc21.BackColor = System.Drawing.Color.White;
            this.uc21.Location = new System.Drawing.Point(0, 0);
            this.uc21.Name = "uc21";
            this.uc21.Size = new System.Drawing.Size(793, 529);
            this.uc21.TabIndex = 1;
            // 
            // uc11
            // 
            this.uc11.BackColor = System.Drawing.Color.White;
            this.uc11.Location = new System.Drawing.Point(3, 0);
            this.uc11.Name = "uc11";
            this.uc11.Size = new System.Drawing.Size(793, 529);
            this.uc11.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(1041, 533);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private uc4 uc41;
        private uc3 uc31;
        private uc2 uc21;
        private uc1 uc11;
        private System.IO.Ports.SerialPort serialPort1;
    }
}

