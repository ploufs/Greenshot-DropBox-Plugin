/*
 * Created by SharpDevelop.
 * User: Robin
 * Date: 05.06.2011
 * Time: 21:13
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace GreenshotDropBoxPlugin.Forms
{
	partial class DropBoxHistory
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DropBoxHistory));
            this.listview_DropBox_uploads = new System.Windows.Forms.ListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.copyLinksToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox_DropBox = new System.Windows.Forms.PictureBox();
            this.deleteButton = new System.Windows.Forms.Button();
            this.openButton = new System.Windows.Forms.Button();
            this.finishedButton = new System.Windows.Forms.Button();
            this.clipboardButton = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_DropBox)).BeginInit();
            this.SuspendLayout();
            // 
            // listview_DropBox_uploads
            // 
            this.listview_DropBox_uploads.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listview_DropBox_uploads.ContextMenuStrip = this.contextMenuStrip1;
            this.listview_DropBox_uploads.FullRowSelect = true;
            this.listview_DropBox_uploads.Location = new System.Drawing.Point(13, 5);
            this.listview_DropBox_uploads.Name = "listview_DropBox_uploads";
            this.listview_DropBox_uploads.Size = new System.Drawing.Size(557, 300);
            this.listview_DropBox_uploads.TabIndex = 0;
            this.listview_DropBox_uploads.UseCompatibleStateImageBehavior = false;
            this.listview_DropBox_uploads.View = System.Windows.Forms.View.Details;
            this.listview_DropBox_uploads.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listview_imgur_uploads_ColumnClick);
            this.listview_DropBox_uploads.SelectedIndexChanged += new System.EventHandler(this.Listview_DropBox_uploadsSelectedIndexChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_Open,
            this.copyLinksToClipboardToolStripMenuItem,
            this.ToolStripMenuItem_Delete});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(205, 92);
            // 
            // ToolStripMenuItem_Open
            // 
            this.ToolStripMenuItem_Open.Name = "ToolStripMenuItem_Open";
            this.ToolStripMenuItem_Open.Size = new System.Drawing.Size(204, 22);
            this.ToolStripMenuItem_Open.Text = "Open";
            this.ToolStripMenuItem_Open.Click += new System.EventHandler(this.ToolStripMenuItem_WebUrl_Click);
            // 
            // copyLinksToClipboardToolStripMenuItem
            // 
            this.copyLinksToClipboardToolStripMenuItem.Name = "copyLinksToClipboardToolStripMenuItem";
            this.copyLinksToClipboardToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.copyLinksToClipboardToolStripMenuItem.Text = "Copy link(s) to clipboard";
            this.copyLinksToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyLinksToClipboardToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem_Delete
            // 
            this.ToolStripMenuItem_Delete.Name = "ToolStripMenuItem_Delete";
            this.ToolStripMenuItem_Delete.Size = new System.Drawing.Size(204, 22);
            this.ToolStripMenuItem_Delete.Text = "&Delete";
            this.ToolStripMenuItem_Delete.Click += new System.EventHandler(this.ToolStripMenuItem_Delete_Click);
            // 
            // pictureBox_DropBox
            // 
            this.pictureBox_DropBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox_DropBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox_DropBox.Location = new System.Drawing.Point(13, 316);
            this.pictureBox_DropBox.Name = "pictureBox_DropBox";
            this.pictureBox_DropBox.Size = new System.Drawing.Size(90, 90);
            this.pictureBox_DropBox.TabIndex = 1;
            this.pictureBox_DropBox.TabStop = false;
            this.pictureBox_DropBox.Click += new System.EventHandler(this.pictureBox_DropBox_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteButton.AutoSize = true;
            this.deleteButton.Location = new System.Drawing.Point(109, 316);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 2;
            this.deleteButton.Text = "&Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButtonClick);
            // 
            // openButton
            // 
            this.openButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.openButton.AutoSize = true;
            this.openButton.Location = new System.Drawing.Point(109, 349);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(75, 23);
            this.openButton.TabIndex = 3;
            this.openButton.Text = "&Open";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.OpenButtonClick);
            // 
            // finishedButton
            // 
            this.finishedButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.finishedButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.finishedButton.Location = new System.Drawing.Point(495, 381);
            this.finishedButton.Name = "finishedButton";
            this.finishedButton.Size = new System.Drawing.Size(75, 23);
            this.finishedButton.TabIndex = 4;
            this.finishedButton.Text = "Finished";
            this.finishedButton.UseVisualStyleBackColor = true;
            this.finishedButton.Click += new System.EventHandler(this.FinishedButtonClick);
            // 
            // clipboardButton
            // 
            this.clipboardButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.clipboardButton.AutoSize = true;
            this.clipboardButton.Location = new System.Drawing.Point(109, 381);
            this.clipboardButton.Name = "clipboardButton";
            this.clipboardButton.Size = new System.Drawing.Size(129, 23);
            this.clipboardButton.TabIndex = 5;
            this.clipboardButton.Text = "&Copy link(s) to clipboard";
            this.clipboardButton.UseVisualStyleBackColor = true;
            this.clipboardButton.Click += new System.EventHandler(this.ClipboardButtonClick);
            // 
            // DropBoxHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 416);
            this.Controls.Add(this.clipboardButton);
            this.Controls.Add(this.finishedButton);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.pictureBox_DropBox);
            this.Controls.Add(this.listview_DropBox_uploads);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DropBoxHistory";
            this.Text = "DropBox history";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImgurHistoryFormClosing);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_DropBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.Button clipboardButton;
		private System.Windows.Forms.Button finishedButton;
		private System.Windows.Forms.Button deleteButton;
		private System.Windows.Forms.Button openButton;
		private System.Windows.Forms.PictureBox pictureBox_DropBox;
        private System.Windows.Forms.ListView listview_DropBox_uploads;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyLinksToClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Open;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Delete;
	}
}
