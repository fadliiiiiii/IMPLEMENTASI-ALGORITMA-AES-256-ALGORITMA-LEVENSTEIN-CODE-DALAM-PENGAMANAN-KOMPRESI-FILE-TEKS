/*
 * Created by SharpDevelop.
 * User: USER
 * Date: 1/8/2023
 * Time: 16:56
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Skripsi_Muhammad_Fadli
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void BerandaToolStripMenuItemClick(object sender, EventArgs e)
		{
			MainForm mf = new MainForm();
			mf.Show();
			this.Hide();
		}
		
		void EnkripsiKompresiToolStripMenuItemClick(object sender, EventArgs e)
		{
			EnkripsiKompresi mf = new EnkripsiKompresi();
			mf.Show();
			this.Hide();
		}
		
		void DekompresiDekripsiToolStripMenuItemClick(object sender, EventArgs e)
		{
			DekompresiDekripsi mf = new DekompresiDekripsi();
			mf.Show();
			this.Hide();
		}
	}
}
