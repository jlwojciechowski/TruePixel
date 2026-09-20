namespace True_Pixel
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            lvFiles = new ListView();
            FileName = new ColumnHeader();
            FileSize = new ColumnHeader();
            AI = new ColumnHeader();
            AIModel = new ColumnHeader();
            MarkAsAI = new ColumnHeader();
            pbPreview = new PictureBox();
            btnAdd = new Button();
            btnRemove = new Button();
            btnCheck = new Button();
            openFileDialog1 = new OpenFileDialog();
            pbProgress = new ProgressBar();
            pbLogo = new PictureBox();
            btnInfo = new Button();
            ((System.ComponentModel.ISupportInitialize)pbPreview).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbLogo).BeginInit();
            SuspendLayout();
            // 
            // lvFiles
            // 
            lvFiles.AllowDrop = true;
            lvFiles.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvFiles.Columns.AddRange(new ColumnHeader[] { FileName, FileSize, AI, AIModel, MarkAsAI });
            lvFiles.FullRowSelect = true;
            lvFiles.GridLines = true;
            lvFiles.Location = new Point(-1, 79);
            lvFiles.Name = "lvFiles";
            lvFiles.Size = new Size(825, 671);
            lvFiles.TabIndex = 0;
            lvFiles.UseCompatibleStateImageBehavior = false;
            lvFiles.View = View.Details;
            lvFiles.SelectedIndexChanged += lvFiles_SelectedIndexChanged;
            lvFiles.DragDrop += lvFiles_DragDrop;
            lvFiles.DragEnter += lvFiles_DragEnter;
            // 
            // FileName
            // 
            FileName.Text = "Nazwa pliku";
            FileName.Width = 300;
            // 
            // FileSize
            // 
            FileSize.Text = "Rozmiar";
            FileSize.Width = 90;
            // 
            // AI
            // 
            AI.Text = "AI";
            AI.Width = 70;
            // 
            // AIModel
            // 
            AIModel.Text = "Model AI";
            AIModel.Width = 200;
            // 
            // MarkAsAI
            // 
            MarkAsAI.Text = "Oznaczyć jako AI";
            MarkAsAI.Width = 140;
            // 
            // pbPreview
            // 
            pbPreview.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pbPreview.BackColor = SystemColors.ButtonHighlight;
            pbPreview.BorderStyle = BorderStyle.FixedSingle;
            pbPreview.Location = new Point(844, 79);
            pbPreview.Name = "pbPreview";
            pbPreview.Size = new Size(513, 671);
            pbPreview.SizeMode = PictureBoxSizeMode.Zoom;
            pbPreview.TabIndex = 1;
            pbPreview.TabStop = false;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(12, 12);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(147, 48);
            btnAdd.TabIndex = 2;
            btnAdd.Text = "Dodaj pliki";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnRemove
            // 
            btnRemove.Location = new Point(165, 12);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(143, 48);
            btnRemove.TabIndex = 3;
            btnRemove.Text = "Usuń pliki";
            btnRemove.UseVisualStyleBackColor = true;
            btnRemove.Click += btnRemove_Click;
            // 
            // btnCheck
            // 
            btnCheck.Location = new Point(314, 12);
            btnCheck.Name = "btnCheck";
            btnCheck.Size = new Size(305, 48);
            btnCheck.TabIndex = 4;
            btnCheck.Text = "Sprawdź czy wygenerowane przez AI";
            btnCheck.UseVisualStyleBackColor = true;
            btnCheck.Click += btnCheck_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            openFileDialog1.Filter = "Obrazy|*.jpg;*.jpeg;*.png;*.bmp;*.webp;*.tif;*.tiff;*.gif;*.avif;*.heic";
            openFileDialog1.Multiselect = true;
            // 
            // pbProgress
            // 
            pbProgress.Location = new Point(640, 22);
            pbProgress.Name = "pbProgress";
            pbProgress.Size = new Size(247, 28);
            pbProgress.Style = ProgressBarStyle.Continuous;
            pbProgress.TabIndex = 5;
            pbProgress.Visible = false;
            // 
            // pbLogo
            // 
            pbLogo.Image = (Image)resources.GetObject("pbLogo.Image");
            pbLogo.Location = new Point(1016, 12);
            pbLogo.Name = "pbLogo";
            pbLogo.Size = new Size(341, 61);
            pbLogo.SizeMode = PictureBoxSizeMode.Zoom;
            pbLogo.TabIndex = 6;
            pbLogo.TabStop = false;
            // 
            // btnInfo
            // 
            btnInfo.Location = new Point(907, 12);
            btnInfo.Name = "btnInfo";
            btnInfo.Size = new Size(94, 48);
            btnInfo.TabIndex = 7;
            btnInfo.Text = "Informacje";
            btnInfo.UseVisualStyleBackColor = true;
            btnInfo.Click += btnInfo_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DarkGreen;
            ClientSize = new Size(1358, 753);
            Controls.Add(btnInfo);
            Controls.Add(pbLogo);
            Controls.Add(btnCheck);
            Controls.Add(btnRemove);
            Controls.Add(btnAdd);
            Controls.Add(pbPreview);
            Controls.Add(lvFiles);
            Controls.Add(pbProgress);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "True Pixel v.1.0";
            ((System.ComponentModel.ISupportInitialize)pbPreview).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbLogo).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private ListView lvFiles;
        private PictureBox pbPreview;
        private Button btnAdd;
        private Button btnRemove;
        private Button btnCheck;
        private ProgressBar pbProgress;
        private OpenFileDialog openFileDialog1;
        private ColumnHeader FileName;
        private ColumnHeader FileSize;
        private ColumnHeader AI;
        private ColumnHeader AIModel;
        private ColumnHeader MarkAsAI;
        private PictureBox pbLogo;
        private Button btnInfo;
    }
}