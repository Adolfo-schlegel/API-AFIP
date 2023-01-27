namespace Sockets
{
	partial class Main
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
			this.listView1 = new System.Windows.Forms.ListView();
			this.EndPoint = new System.Windows.Forms.ColumnHeader();
			this.UserID = new System.Windows.Forms.ColumnHeader();
			this.LastMsg = new System.Windows.Forms.ColumnHeader();
			this.LastRecTime = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// listView1
			// 
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.EndPoint,
            this.UserID,
            this.LastMsg,
            this.LastRecTime});
			this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView1.Location = new System.Drawing.Point(0, 0);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(800, 450);
			this.listView1.TabIndex = 0;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			// 
			// EndPoint
			// 
			this.EndPoint.Tag = "EndPoint";
			this.EndPoint.Text = "EndPoint";
			this.EndPoint.Width = 90;
			// 
			// UserID
			// 
			this.UserID.Text = "User ID";
			this.UserID.Width = 100;
			// 
			// LastMsg
			// 
			this.LastMsg.Text = "Last Message";
			this.LastMsg.Width = 300;
			// 
			// LastRecTime
			// 
			this.LastRecTime.Text = "Last Received Time";
			this.LastRecTime.Width = 100;
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.listView1);
			this.Name = "Main";
			this.Text = "Chat";
			this.ResumeLayout(false);

		}

		#endregion

		private ListView listView1;
		private ColumnHeader EndPoint;
		private ColumnHeader UserID;
		private ColumnHeader LastMsg;
		private ColumnHeader LastRecTime;
	}
}