/*
 * Created by SharpDevelop.
 * User: USER
 * Date: 1/8/2023
 * Time: 16:57
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Skripsi_Muhammad_Fadli
{
	/// <summary>
	/// Description of DekompresiDekripsi.
	/// </summary>
	public partial class DekompresiDekripsi : Form
	{
		Stopwatch sw = new Stopwatch();
		public DekompresiDekripsi()
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
		
		string decrypt(byte[] ciphertext, byte[] key, byte[] iv) {
			string plaintext = null;
			using(AesManaged aes = new AesManaged()) {
				ICryptoTransform decryptor = aes.CreateDecryptor(key, iv);
				using(MemoryStream ms = new MemoryStream(ciphertext)) {
					using(CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read)) {
						using(StreamReader reader = new StreamReader(cs))
							plaintext = reader.ReadToEnd();
					}
				}
			}
			return plaintext;
		}
		
		byte[] stringToByte(string input)
		{
			byte[] output = new byte[input.Length];
			for(int i = 0; i < input.Length; i++) {
				if(input[i] == (char)990)
					output[i] = 0;
				else if(input[i] == (char)991)
					output[i] = 11;
				else if(input[i] == (char)992)
					output[i] = 13;
				else
					output[i] = (byte)input[i];
			}
			return output;
		}
		
		string byteToString(byte[] input)
		{
			string output = "";
			for(int i = 0; i < input.Length; i++) {
				if(input[i] == 0)
					output += (char)990;
				else if(input[i] == 11)
					output += (char)991;
				else if(input[i] == 13)
					output += (char)992;
				else
					output += (char)input[i];
			}
			return output;
		}
		
		byte[] stringHexToByte(string input)
		{
			string[] key_arr = input.Split('-');
			byte[] output = new byte[key_arr.Length];
			for(int i = 0; i < key_arr.Length; i++)
				output[i] = Convert.ToByte(key_arr[i], 16);
			
			return output;
		}
		
		// FADLI[Sp]ILKOM[Sp]USU[Sp] - 70, 65, 68, 76, 73, 32, 73, 76, 75, 79, 77, 32, 85, 83, 85, 32
		byte[] iv_byte = new byte[] {70, 65, 68, 76, 73, 32, 73, 76, 75, 79, 77, 32, 85, 83, 85, 32};
		void Btn_dekripsiClick(object sender, EventArgs e)
		{
			try
			{
				sw.Restart();
				using(AesManaged aes = new AesManaged()) {
					// key
					aes.KeySize = 128;
					string key = tb_key.Text;
					key = key.PadRight(16, ' ');
					byte[] key_byte = new byte[16];
					for(int i = 0; i < key.Length; i++)
						key_byte[i] = Convert.ToByte(key[i]);
					
					// input
					string ciphertext = rb_uncompress.Text;
					byte[] ciphertext_byte = stringHexToByte(ciphertext);
						
					// process
					string decrypted = decrypt(ciphertext_byte, key_byte, iv_byte);
					rb_plaintext.Text = decrypted;
				}
				sw.Stop();
				tb_rt_dekripsi.Text = sw.Elapsed.TotalMilliseconds.ToString() + " ms";
				tb_ukuran_dekripsi.Text = rb_ciphertext.Text.Length + " byte";
			}
			catch(Exception)
			{
				MessageBox.Show("Kunci AES Invalid");
			}
		}
		
		// DEKOMPRESI
		string stringToBiner(string text)
		{
			string text_biner = "";
			for(int i=0; i<text.Length; i++) {
				if(text[i] == (char)990)
					text_biner += Convert.ToString(0, 2).PadLeft(8, '0');
				else if(text[i] == (char)991)
					text_biner += Convert.ToString(11, 2).PadLeft(8, '0');
				else if(text[i] == (char)992)
					text_biner += Convert.ToString(13, 2).PadLeft(8, '0');
				else
					text_biner += Convert.ToString(text[i], 2).PadLeft(8, '0');
			}	
			
			// check flag and remove
			int flag = Convert.ToInt32(text_biner.Substring(text_biner.Length - 8, 8), 2);
			text_biner = text_biner.Remove(text_biner.Length - 8);
			
			// remove padding based on flag
			text_biner = text_biner.Remove(text_biner.Length - flag);
			
			return text_biner;
		}
		
		List<byte> decode(string biner) {
			List<byte> res = new List<byte>();
			while(biner.Length > 1) {
				int index = biner.IndexOf('0');
				int len = 1;
				while(index > 0) {
					int temp_len = Convert.ToInt32(biner.Substring(index, len), 2) + 1;
					biner = biner.Remove(index, len);
					len = temp_len;
					index--;
				}
				res.Add(Convert.ToByte(biner.Substring(0, len), 2));
				biner = biner.Remove(0, len);
			}
			return res;
		}
		
		void Btn_dekompresiClick(object sender, EventArgs e)
		{
			sw.Restart();
			// dictionary
			Dictionary<int, char> codeword = new Dictionary<int, char>();
			string get_dictionary = dictionary;
			for(int i=0; i<get_dictionary.Length; i++)
				codeword[i] = (char)get_dictionary[i];
			
			// proses decoding
			List<byte> lc = decode(stringToBiner(rb_ciphertext.Text));
//			string res = "";
			StringBuilder res = new StringBuilder();
			foreach(byte l in lc)
				res.Append(codeword[l]);
			
			// output
			rb_uncompress.Text = res.ToString();
			
			sw.Stop();
			tb_rt_dekompresi.Text = sw.Elapsed.TotalMilliseconds.ToString() + " ms";
			tb_ukuran_dekompresi.Text = rb_uncompress.Text.Length + " byte";
		}
		
		string dictionary = "";
		void Btn_browseClick(object sender, EventArgs e)
		{
			// file
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Levenshtein Code|*.lc";
			if(ofd.ShowDialog() == DialogResult.OK)
			{
				file_path.Text = Path.GetFileName(ofd.FileName);
				rb_ciphertext.Text = File.ReadAllText(ofd.FileName);
			}
			
			tb_ukuran.Text = rb_ciphertext.Text.Length + " byte";
			
			// dictionary
			ofd = new OpenFileDialog();
			ofd.Filter = "Dictionary|*.dict";
			if(ofd.ShowDialog() == DialogResult.OK)
			{
				file_path.Text = Path.GetFileName(ofd.FileName);
				dictionary = File.ReadAllText(ofd.FileName);
			}
		}
		
		void Btn_simpanClick(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "Corpus|*.txt";
			if(sfd.ShowDialog() == DialogResult.OK)
			{
				string fileExt = Path.GetExtension(sfd.FileName).Substring(1);
				if(fileExt == "txt")
					File.WriteAllText(sfd.FileName, rb_plaintext.Text);
			}
		}
	}
}
