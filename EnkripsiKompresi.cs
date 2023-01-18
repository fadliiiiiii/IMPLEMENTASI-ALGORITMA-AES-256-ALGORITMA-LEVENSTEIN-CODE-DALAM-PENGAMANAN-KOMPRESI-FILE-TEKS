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
	/// Description of EnkripsiKompresi.
	/// </summary>
	public partial class EnkripsiKompresi : Form
	{
		Stopwatch sw = new Stopwatch();
		public EnkripsiKompresi()
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
		
		void Btn_browseClick(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Corpus|*.txt;*html";
			if(ofd.ShowDialog() == DialogResult.OK)
			{
				file_path.Text = Path.GetDirectoryName(ofd.FileName);
				rb_plaintext.Text = File.ReadAllText(ofd.FileName);
			}
			tb_ukuran.Text = rb_plaintext.Text.Length + " byte";
		}
		
		// ENKRIPSI
		byte[] encrypt(string plaintext, byte[] key, byte[] iv) {
			byte[] encrypted;
			using(AesManaged aes = new AesManaged()) {
				ICryptoTransform encryptor = aes.CreateEncryptor(key, iv);
				
				using(MemoryStream ms = new MemoryStream()) {
					using(CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
						using(StreamWriter sw = new StreamWriter(cs)) {
							sw.Write(plaintext);
						}
						encrypted = ms.ToArray();
					}
				}
			}
			return encrypted;
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
		void Btn_enkripsiClick(object sender, EventArgs e)
		{
			sw.Restart();
			using(AesManaged aes = new AesManaged()) {
				// key
				aes.KeySize = 128;
				string key = tb_key.Text;
				key = key.PadRight(16, ' ');
				byte[] key_byte = stringToByte(key);
				
				// input
				string plaintext = rb_plaintext.Text;
				
				// process
				byte[] encrypted = encrypt(plaintext, key_byte, iv_byte);
				string output = byteToString(encrypted);
				output = BitConverter.ToString(encrypted);
				rb_ciphertext.Text = output;
			}
			sw.Stop();
			tb_rt_enkripsi.Text = sw.Elapsed.TotalMilliseconds.ToString() + " ms";
			tb_ukuran_enkripsi.Text = rb_ciphertext.Text.Length + " byte";
		}
		
		// KOMPRESI
		string binerToString(string biner) 
		{
			// add padding
			int add_padding = 8 - biner.Length%8;
			for(int i=0; i<add_padding; i++)
				biner += "0";
			
			// add flag
			string add_flag = Convert.ToString(add_padding, 2).PadLeft(8, '0');
			biner += add_flag;
			
			// er dlm karakter
			string res_char = "";
			for(int i=0; i<biner.Length; i+=8) {
				int dec = Convert.ToInt32(biner.Substring(i, 8), 2);
				if(dec == 0)
					res_char += (char)990;
				else if(dec == 11)
					res_char += (char)991;
				else if(dec == 13)
					res_char += (char)992;
				else
					res_char += (char)dec;
			}
			return res_char;
		}
		
		Dictionary<char, int> freq_table_func(string s, int max_value) {
			//table temp freq n char
			int[] temp_freq = new int[max_value];
			int[] temp_ch = new int[max_value];
			for(int i=0; i<s.Length; i++)
				temp_freq[s[i]]++;
			
			for(int i=0; i<max_value; i++)
				temp_ch[i] = i;
			
			//initialize from table temp (256) to table fix (cut of)
			int[] freq = new int[max_value]; 
			int[] ch = new int[max_value];
			int index = 0;
			for(int i=0; i<s.Length; i++) {
				if(temp_freq[s[i]] != 0) {
					freq[index] = temp_freq[s[i]];
					ch[index] = temp_ch[s[i]];
					index++;
					temp_freq[s[i]] = 0;
				}
			}
				
			//sorting - bubble sort
			for(int i=0; i<index-1; i++) {
				for(int j=0; j<index-i-1; j++) {
					if(freq[j] < freq[j+1]) {
						int temp = freq[j];
						freq[j] = freq[j+1];
						freq[j+1] = temp;
						
						int temp1 = ch[j];
						ch[j] = ch[j+1];
						ch[j+1] = temp1;
					}
				}
			}
			
			// FADLII
			// table[I] = 0
			// table[F] = 1
			// table[A] = 2
			// ...
			Dictionary<char, int> table = new Dictionary<char, int>();
			for(int i=0; i<index; i++)
				table[(char)ch[i]] = i;
			
			return table;
		}
		
		string encode(int n) {
			string biner = Convert.ToString(n, 2);
			string insert_biner = biner;
			int i = 1;
			while(insert_biner != "0") {
				insert_biner = Convert.ToString(insert_biner.Length - 1, 2);
				biner = biner.Insert(i, insert_biner);
				i++;
			}
			return biner;
		}
		
		string dictionary = "";
		void Btn_kompresiClick(object sender, EventArgs e)
		{
			sw.Restart();
			// input
			string plaintext = rb_ciphertext.Text;
			
			// proses tabel frekuensi
			Dictionary<char, int> freq_table = freq_table_func(plaintext, 256);
			string[] encode_list = new string[freq_table.Count];
			
			// proses encoding
			for(int i=0; i<freq_table.Count; i++)
				encode_list[i] = encode(i);
			
			// output
//			string res = "";
			StringBuilder res = new StringBuilder();
			for(int i = 0; i < plaintext.Length; i++)
				res.Append(encode_list[freq_table[plaintext[i]]]);
			
			rb_compress.Text = binerToString(res.ToString());
			
			// kamus
			foreach(KeyValuePair<char, int> ft in freq_table)
				dictionary += ft.Key;
			
			sw.Stop();
			tb_rt_kompresi.Text = sw.Elapsed.TotalMilliseconds.ToString() + " ms";
			tb_ukuran_kompresi.Text = rb_compress.Text.Length + " byte";
			
			double before = rb_ciphertext.Text.Length;
			double after = rb_compress.Text.Length;
			tb_rc.Text = Math.Round(after/before*100, 2) + "%";
			tb_cr.Text = Math.Round(before/after, 2).ToString();
			tb_ss.Text = Math.Round(100 - after/before*100, 2) + "%";
			tb_br.Text = Math.Round(after*8/before, 2).ToString();
		}
		
		void Btn_simpanClick(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "Levenshtein Code|*.lc";
			if(sfd.ShowDialog() == DialogResult.OK)
			{
				string fileExt = Path.GetExtension(sfd.FileName).Substring(1);
				if(fileExt == "lc")
					File.WriteAllText(sfd.FileName, rb_compress.Text);
			}
			
			sfd = new SaveFileDialog();
			sfd.Filter = "Dictionary|*.dict";
			if(sfd.ShowDialog() == DialogResult.OK)
			{
				string fileExt = Path.GetExtension(sfd.FileName).Substring(1);
				File.WriteAllText(sfd.FileName, dictionary);
			}
		}
	}
}
