namespace Reddot_Express_Inventory
{
    partial class InvoiceBill
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label9 = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.txtdiscount = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtInvoice = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtparty = new System.Windows.Forms.TextBox();
            this.txtpname = new System.Windows.Forms.TextBox();
            this.txtquant = new System.Windows.Forms.TextBox();
            this.txtperprodprice = new System.Windows.Forms.TextBox();
            this.txttotal = new System.Windows.Forms.TextBox();
            this.txtbamount = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btn_print = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnupdate = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.txt_date = new System.Windows.Forms.DateTimePicker();
            this.txtnetamount = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btn_delete = new System.Windows.Forms.Button();
            this.btn_add = new System.Windows.Forms.Button();
            this.txtsaletax = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(366, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(610, 568);
            this.dataGridView1.TabIndex = 98;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(12, 411);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 19);
            this.label9.TabIndex = 93;
            this.label9.Text = "Remarks";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Enabled = false;
            this.txtRemarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRemarks.Location = new System.Drawing.Point(169, 412);
            this.txtRemarks.MaxLength = 300;
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(172, 58);
            this.txtRemarks.TabIndex = 92;
            this.txtRemarks.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRemarks_KeyPress);
            // 
            // txtdiscount
            // 
            this.txtdiscount.BackColor = System.Drawing.SystemColors.Window;
            this.txtdiscount.Enabled = false;
            this.txtdiscount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtdiscount.Location = new System.Drawing.Point(169, 309);
            this.txtdiscount.MaxLength = 10;
            this.txtdiscount.Name = "txtdiscount";
            this.txtdiscount.Size = new System.Drawing.Size(172, 21);
            this.txtdiscount.TabIndex = 90;
            this.txtdiscount.TabStop = false;
            this.txtdiscount.TextChanged += new System.EventHandler(this.txtdiscount_TextChanged);
            this.txtdiscount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtdiscount_KeyPress);
            this.txtdiscount.Validated += new System.EventHandler(this.txtdiscount_Validated);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(13, 312);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 19);
            this.label8.TabIndex = 91;
            this.label8.Text = "Discount";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(12, 275);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 19);
            this.label7.TabIndex = 89;
            this.label7.Text = "Bill Amount";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(12, 236);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 19);
            this.label6.TabIndex = 87;
            this.label6.Text = "Total Amount";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(12, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 19);
            this.label1.TabIndex = 77;
            this.label1.Text = "Invoive ID";
            // 
            // txtInvoice
            // 
            this.txtInvoice.Enabled = false;
            this.txtInvoice.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInvoice.Location = new System.Drawing.Point(169, 24);
            this.txtInvoice.Name = "txtInvoice";
            this.txtInvoice.ReadOnly = true;
            this.txtInvoice.Size = new System.Drawing.Size(172, 21);
            this.txtInvoice.TabIndex = 76;
            this.txtInvoice.TextChanged += new System.EventHandler(this.txtInvoice_TextChanged);
            this.txtInvoice.Leave += new System.EventHandler(this.txtInvoice_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(12, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 19);
            this.label2.TabIndex = 100;
            this.label2.Text = "Party Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(11, 168);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 19);
            this.label3.TabIndex = 108;
            this.label3.Text = "Quantity";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(12, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 19);
            this.label4.TabIndex = 106;
            this.label4.Text = "Product_Name";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(11, 203);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(140, 19);
            this.label10.TabIndex = 110;
            this.label10.Text = "Price Per Product";
            // 
            // txtparty
            // 
            this.txtparty.Enabled = false;
            this.txtparty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtparty.Location = new System.Drawing.Point(169, 61);
            this.txtparty.MaxLength = 30;
            this.txtparty.Name = "txtparty";
            this.txtparty.Size = new System.Drawing.Size(172, 21);
            this.txtparty.TabIndex = 111;
            this.txtparty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtparty_KeyPress);
            // 
            // txtpname
            // 
            this.txtpname.Enabled = false;
            this.txtpname.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtpname.Location = new System.Drawing.Point(169, 130);
            this.txtpname.MaxLength = 50;
            this.txtpname.Name = "txtpname";
            this.txtpname.Size = new System.Drawing.Size(172, 21);
            this.txtpname.TabIndex = 112;
            this.txtpname.TextChanged += new System.EventHandler(this.txtpname_TextChanged);
            this.txtpname.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtbname_KeyPress);
            // 
            // txtquant
            // 
            this.txtquant.Enabled = false;
            this.txtquant.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtquant.Location = new System.Drawing.Point(169, 168);
            this.txtquant.Name = "txtquant";
            this.txtquant.Size = new System.Drawing.Size(172, 21);
            this.txtquant.TabIndex = 113;
            this.txtquant.TextChanged += new System.EventHandler(this.txtperprodprice_TextChanged);
            this.txtquant.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtdiscount_KeyPress);
            // 
            // txtperprodprice
            // 
            this.txtperprodprice.Enabled = false;
            this.txtperprodprice.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtperprodprice.Location = new System.Drawing.Point(169, 203);
            this.txtperprodprice.MaxLength = 8;
            this.txtperprodprice.Name = "txtperprodprice";
            this.txtperprodprice.Size = new System.Drawing.Size(172, 21);
            this.txtperprodprice.TabIndex = 114;
            this.txtperprodprice.TextChanged += new System.EventHandler(this.txtperprodprice_TextChanged);
            this.txtperprodprice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtdiscount_KeyPress);
            // 
            // txttotal
            // 
            this.txttotal.Enabled = false;
            this.txttotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txttotal.Location = new System.Drawing.Point(169, 236);
            this.txttotal.Name = "txttotal";
            this.txttotal.Size = new System.Drawing.Size(172, 21);
            this.txttotal.TabIndex = 115;
            // 
            // txtbamount
            // 
            this.txtbamount.Enabled = false;
            this.txtbamount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbamount.Location = new System.Drawing.Point(169, 275);
            this.txtbamount.Name = "txtbamount";
            this.txtbamount.Size = new System.Drawing.Size(172, 21);
            this.txtbamount.TabIndex = 116;
            this.txtbamount.TextChanged += new System.EventHandler(this.txtbamount_TextChanged);
            this.txtbamount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCashRelease_KeyPress);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 131F));
            this.tableLayoutPanel1.Controls.Add(this.btnNew, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btn_print, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnEdit, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnupdate, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(16, 522);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(339, 72);
            this.tableLayoutPanel1.TabIndex = 117;
            // 
            // btnNew
            // 
            this.btnNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNew.Font = new System.Drawing.Font("Cambria", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNew.Location = new System.Drawing.Point(3, 3);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(98, 30);
            this.btnNew.TabIndex = 96;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.Enabled = false;
            this.btnSave.Font = new System.Drawing.Font("Cambria", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(107, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(98, 30);
            this.btnSave.TabIndex = 97;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btn_print
            // 
            this.btn_print.Font = new System.Drawing.Font("Cambria", 14.25F, System.Drawing.FontStyle.Bold);
            this.btn_print.Location = new System.Drawing.Point(211, 3);
            this.btn_print.Name = "btn_print";
            this.tableLayoutPanel1.SetRowSpan(this.btn_print, 2);
            this.btn_print.Size = new System.Drawing.Size(125, 66);
            this.btn_print.TabIndex = 98;
            this.btn_print.Text = "Print";
            this.btn_print.UseVisualStyleBackColor = true;
            this.btn_print.Click += new System.EventHandler(this.btn_print_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.Location = new System.Drawing.Point(3, 39);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(98, 30);
            this.btnEdit.TabIndex = 100;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnupdate
            // 
            this.btnupdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnupdate.Enabled = false;
            this.btnupdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnupdate.Location = new System.Drawing.Point(107, 39);
            this.btnupdate.Name = "btnupdate";
            this.btnupdate.Size = new System.Drawing.Size(98, 30);
            this.btnupdate.TabIndex = 101;
            this.btnupdate.Text = "Update";
            this.btnupdate.UseVisualStyleBackColor = true;
            this.btnupdate.Click += new System.EventHandler(this.btnupdate_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(13, 99);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(43, 19);
            this.label11.TabIndex = 119;
            this.label11.Text = "Date";
            // 
            // txt_date
            // 
            this.txt_date.CustomFormat = "dd-MM-yyyy";
            this.txt_date.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_date.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.txt_date.Location = new System.Drawing.Point(170, 97);
            this.txt_date.Name = "txt_date";
            this.txt_date.Size = new System.Drawing.Size(171, 21);
            this.txt_date.TabIndex = 120;
            // 
            // txtnetamount
            // 
            this.txtnetamount.BackColor = System.Drawing.SystemColors.Window;
            this.txtnetamount.Enabled = false;
            this.txtnetamount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtnetamount.Location = new System.Drawing.Point(169, 373);
            this.txtnetamount.Name = "txtnetamount";
            this.txtnetamount.ReadOnly = true;
            this.txtnetamount.Size = new System.Drawing.Size(172, 21);
            this.txtnetamount.TabIndex = 121;
            this.txtnetamount.TabStop = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(12, 373);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(97, 19);
            this.label12.TabIndex = 122;
            this.label12.Text = "Net Amount";
            // 
            // btn_delete
            // 
            this.btn_delete.Enabled = false;
            this.btn_delete.Location = new System.Drawing.Point(183, 476);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(147, 35);
            this.btn_delete.TabIndex = 124;
            this.btn_delete.Text = "Delete";
            this.btn_delete.UseVisualStyleBackColor = true;
            // 
            // btn_add
            // 
            this.btn_add.Enabled = false;
            this.btn_add.Location = new System.Drawing.Point(26, 476);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(147, 35);
            this.btn_add.TabIndex = 125;
            this.btn_add.Text = "ADD";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // txtsaletax
            // 
            this.txtsaletax.BackColor = System.Drawing.SystemColors.Window;
            this.txtsaletax.Enabled = false;
            this.txtsaletax.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtsaletax.Location = new System.Drawing.Point(167, 340);
            this.txtsaletax.MaxLength = 3;
            this.txtsaletax.Name = "txtsaletax";
            this.txtsaletax.ReadOnly = true;
            this.txtsaletax.Size = new System.Drawing.Size(172, 21);
            this.txtsaletax.TabIndex = 126;
            this.txtsaletax.TabStop = false;
            this.txtsaletax.TextChanged += new System.EventHandler(this.txtsaletax_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(11, 343);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 19);
            this.label5.TabIndex = 127;
            this.label5.Text = "Sale Tax";
            // 
            // InvoiceBill
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(982, 606);
            this.Controls.Add(this.txtsaletax);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btn_add);
            this.Controls.Add(this.btn_delete);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtnetamount);
            this.Controls.Add(this.txt_date);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.txtbamount);
            this.Controls.Add(this.txttotal);
            this.Controls.Add(this.txtperprodprice);
            this.Controls.Add(this.txtquant);
            this.Controls.Add(this.txtpname);
            this.Controls.Add(this.txtparty);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtRemarks);
            this.Controls.Add(this.txtdiscount);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtInvoice);
            this.MinimizeBox = false;
            this.Name = "InvoiceBill";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Invoice";
            this.Load += new System.EventHandler(this.Courier_Transaction_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.TextBox txtdiscount;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox txtInvoice;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtparty;
        private System.Windows.Forms.TextBox txtpname;
        private System.Windows.Forms.TextBox txtquant;
        private System.Windows.Forms.TextBox txtperprodprice;
        private System.Windows.Forms.TextBox txttotal;
        private System.Windows.Forms.TextBox txtbamount;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnupdate;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btn_print;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker txt_date;
        private System.Windows.Forms.TextBox txtnetamount;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btn_delete;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.TextBox txtsaletax;
        private System.Windows.Forms.Label label5;

    }
}