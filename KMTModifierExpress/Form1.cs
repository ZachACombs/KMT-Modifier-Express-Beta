using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace KMTModifierExpress
{
    public partial class Form1 : Form
    {

		byte IntToAByte(int nnn)
		{
			if (nnn > 255) { nnn = 255; }
			if (nnn < 0) { nnn = 0; }
			byte[] bbbb = BitConverter.GetBytes(nnn);
			return bbbb[0];
		}

		bool SaveKMT(string Ofile)
		{
			string MissionMode = "00000000000000000000000000000000";

			for (int nn = 0; nn < 64; nn = nn + 1)
			{
				byte[] BbbCourse = { DataCourse[nn], 0, 0, 0, 0, 0, 0, 0 };
				byte[] BbbChar = { DataChar[nn], 0, 0, 0, 0, 0, 0, 0 };
				byte[] BbbVehicle = { DataVehicle[nn], 0, 0, 0, 0, 0, 0, 0 };
				byte[] BbbClass = { DataClass[nn], 0, 0, 0, 0, 0, 0, 0 };

				string VMRFN = DectoHex_Uint(Convert.ToInt64(DataMRFN[nn]));
				string VGM = DectoHex_Uint(Convert.ToInt64(DataGM[nn]));
				string VCourse = DectoHex_Uint(BitConverter.ToInt64(BbbCourse,0));
				string VChar = DectoHex_Uint(BitConverter.ToInt64(BbbChar,0));
				string VVehicle = DectoHex_Uint(BitConverter.ToInt64(BbbVehicle,0));
				string VClass = DectoHex_Uint(BitConverter.ToInt64(BbbClass,0));
				string VTime = DectoHex_Uint(Convert.ToInt64(DataTime[nn]));
				string VScore = DectoHex_Uint(DataScore[nn]);
				VMRFN = "00000000" + VMRFN;
				VGM = "00000000" + VGM;
				VCourse = "00000000" + VCourse;
				VChar = "00000000" + VChar;
				VVehicle = "00000000" + VVehicle;
				VClass = "00000000" + VClass;
				VTime = "00000000" + VTime;
				VScore = "00000000" + VScore;
				VMRFN = VMRFN.Substring(VMRFN.Length - 4, 4);
				VGM = VGM.Substring(VGM.Length - 4, 4);
				VCourse = VCourse.Substring(VCourse.Length - 2, 2);
				VChar = VChar.Substring(VChar.Length - 2, 2);
				VVehicle = VVehicle.Substring(VVehicle.Length - 2, 2);
				VClass = VClass.Substring(VClass.Length - 2, 2);
				VTime = VTime.Substring(VTime.Length - 4, 4);
				VScore = VScore.Substring(VScore.Length - 8, 8);
				MissionMode = MissionMode + VMRFN + VGM + " " + VCourse + VChar + VVehicle + VClass +
					"00000000 00000000" +
					"00000000 00000000" +
					"00000000 00000000" +
					"00000000 00000000" +
					"00000000 " + VTime + "0000" +
					VScore + " 00000000" +
					"00000000 00000000" +
					"00000000 00000000" +
					"00000000 00000000" +
					"00000000 00000000" +
					"00000000 00000000" +
					"00000000 00000000" +
					"00000000 00000000"
					;
			}
			
			byte[] fff = HextoBytes(MissionMode);
			File.WriteAllBytes(Ofile, fff);
			return true;
		}

		public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
			Text = OpenF + " - " + Pname;

			byte[] BbbCourse = { DataCourse[MLevel * 8 + MMission], 0, 0, 0 };
			byte[] BbbChar = { DataChar[MLevel * 8 + MMission], 0, 0, 0 };
			byte[] BbbVehicle = { DataVehicle[MLevel * 8 + MMission], 0, 0, 0 };
			byte[] BbbClass = { DataClass[MLevel * 8 + MMission], 0, 0, 0 };

			TextboxMRFN.Text = DataMRFN[MLevel * 8 + MMission].ToString();
			ComboGM.SelectedIndex = DataGM[MLevel * 8 + MMission];
			ComboCourse.SelectedIndex = BitConverter.ToInt32(BbbCourse, 0);
			ComboChar.SelectedIndex = BitConverter.ToInt32(BbbChar, 0);
			ComboVehicle.SelectedIndex = BitConverter.ToInt32(BbbVehicle, 0);
			ComboClass.SelectedIndex = BitConverter.ToInt32(BbbClass, 0);
			TextboxTime.Text = DataTime[MLevel * 8 + MMission].ToString();
			TextboxScore.Text = DataScore[MLevel * 8 + MMission].ToString();
		}

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = MessageBox.Show("Any unsaved changes will be lost. Is this OK?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (DialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void TextboxMRFN_TextChanged(object sender, EventArgs e)
        {
            string ttt = TextboxMRFN.Text;
            TextboxMRFN.Text = OnlyDigits(ttt);
            int.TryParse(TextboxMRFN.Text, out int nnn);
            if (nnn < 0)
            {
                TextboxMRFN.Text = "0";
				nnn = 0;
            }
            if (nnn > 65535)
            {
                TextboxMRFN.Text = "65535";
				nnn = 65535;
            }
			DataMRFN[MLevel * 8 + MMission] = nnn;
		}

        private void TextboxTime_TextChanged(object sender, EventArgs e)
        {
            string ttt = TextboxTime.Text;
            TextboxTime.Text = OnlyDigits(ttt);
            int.TryParse(TextboxTime.Text, out int nnn);
            if (nnn < 0)
            {
                TextboxTime.Text = "0";
				nnn = 0;
            }
            if (nnn > 65535)
            {
                TextboxTime.Text = "65535";
				nnn = 65535;
            }
			DataTime[MLevel * 8 + MMission] = nnn;
		}

        private void TextboxScore_TextChanged(object sender, EventArgs e)
        {
            string ttt = TextboxScore.Text;
            TextboxScore.Text = OnlyDigits(ttt);
            long.TryParse(TextboxScore.Text, out long nnn);
            if (nnn < 0)
            {
                TextboxScore.Text = "0";
				nnn = 0;
            }
            if (nnn > 4294967295)
            {
                TextboxScore.Text = "4294967295";
				nnn = 4294967295;
			}
			DataScore[MLevel * 8 + MMission] = nnn;
		}

        private void copyAsCheatCodeToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            long.TryParse(TextboxMRFN.Text, out long nnn);
			string VMRFN=DectoHex_Uint(nnn);
            string VGM = DectoHex_Uint(Convert.ToInt64(ComboGM.SelectedIndex));
            string VCourse = DectoHex_Uint(Convert.ToInt64(ComboCourse.SelectedIndex));
            string VChar = DectoHex_Uint(Convert.ToInt64(ComboChar.SelectedIndex));
            string VVehicle = DectoHex_Uint(Convert.ToInt64(ComboVehicle.SelectedIndex));
            string VClass = DectoHex_Uint(Convert.ToInt64(ComboClass.SelectedIndex));
            long.TryParse(TextboxTime.Text, out nnn);
            string VTime = DectoHex_Uint(nnn);
            long.TryParse(TextboxScore.Text, out nnn);
            string VScore = DectoHex_Uint(nnn);
            VMRFN = "00000000" + VMRFN;
            VGM = "00000000" + VGM;
            VCourse = "00000000" + VCourse;
            VChar = "00000000" + VChar;
            VVehicle = "00000000" + VVehicle;
            VClass = "00000000" + VClass;
            VTime = "00000000" + VTime;
            VScore = "00000000" + VScore;
            VMRFN = VMRFN.Substring(VMRFN.Length - 4, 4);
            VGM = VGM.Substring(VGM.Length - 4, 4);
            VCourse = VCourse.Substring(VCourse.Length - 2, 2);
            VChar = VChar.Substring(VChar.Length - 2, 2);
            VVehicle = VVehicle.Substring(VVehicle.Length - 2, 2);
            VClass = VClass.Substring(VClass.Length - 2, 2);
            VTime = VTime.Substring(VTime.Length - 4, 4);
            VScore = VScore.Substring(VScore.Length - 8, 8);
            Clipboard.SetText(
                VMRFN + VGM + " " + VCourse + VChar + VVehicle + VClass + System.Environment.NewLine +
                "00000000 00000000" + System.Environment.NewLine +
                "00000000 00000000" + System.Environment.NewLine +
                "00000000 00000000" + System.Environment.NewLine +
                "00000000 00000000" + System.Environment.NewLine +
                "00000000 " + VTime + "0000" + System.Environment.NewLine +
                VScore + " 00000000" + System.Environment.NewLine +
                "00000000 00000000" + System.Environment.NewLine +
                "00000000 00000000" + System.Environment.NewLine +
                "00000000 00000000" + System.Environment.NewLine +
                "00000000 00000000" + System.Environment.NewLine +
                "00000000 00000000" + System.Environment.NewLine +
                "00000000 00000000" + System.Environment.NewLine +
                "00000000 00000000"
                );
            //System.Environment.NewLine
        }

        private void pALToolStripMenuItem_Click(object sender, EventArgs e)
        {
            long.TryParse(TextboxMRFN.Text, out long nnn);
            string VMRFN = DectoHex_Uint(nnn);
            string VGM = DectoHex_Uint(Convert.ToInt64(ComboGM.SelectedIndex));
            string VCourse = DectoHex_Uint(Convert.ToInt64(ComboCourse.SelectedIndex));
            string VChar = DectoHex_Uint(Convert.ToInt64(ComboChar.SelectedIndex));
            string VVehicle = DectoHex_Uint(Convert.ToInt64(ComboVehicle.SelectedIndex));
            string VClass = DectoHex_Uint(Convert.ToInt64(ComboClass.SelectedIndex));
            long.TryParse(TextboxTime.Text, out nnn);
            string VTime = DectoHex_Uint(nnn);
            long.TryParse(TextboxScore.Text, out nnn);
            string VScore = DectoHex_Uint(nnn);
            VMRFN = "00000000" + VMRFN;
            VGM = "00000000" + VGM;
            VCourse = "00000000" + VCourse;
            VChar = "00000000" + VChar;
            VVehicle = "00000000" + VVehicle;
            VClass = "00000000" + VClass;
            VTime = "00000000" + VTime;
            VScore = "00000000" + VScore;
            VMRFN = VMRFN.Substring(VMRFN.Length - 4, 4);
            VGM = VGM.Substring(VGM.Length - 4, 4);
            VCourse = VCourse.Substring(VCourse.Length - 2, 2);
            VChar = VChar.Substring(VChar.Length - 2, 2);
            VVehicle = VVehicle.Substring(VVehicle.Length - 2, 2);
            VClass = VClass.Substring(VClass.Length - 2, 2);
            VTime = VTime.Substring(VTime.Length - 4, 4);
            VScore = VScore.Substring(VScore.Length - 8, 8);
            string Patch = Patchcode.Replace("GGGG", VMRFN);
            Patch = Patch.Replace("HHHH", VGM);
            Patch = Patch.Replace("II", VCourse);
            Patch = Patch.Replace("JJ", VChar);
            Patch = Patch.Replace("KK", VVehicle);
            Patch = Patch.Replace("LL", VClass);
            Patch = Patch.Replace("MMMM", VTime);
            Patch = Patch.Replace("NNNNNNNN", VScore);
            string MissionMode = GCTHeader + Begin_PAL + Patch + End_PAL + GCTFooter;
            SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
            SaveFileDialog1.InitialDirectory = "c:\\";
            SaveFileDialog1.Filter = "Gecko Cheat Code File (*.gct)|*.gct";
            SaveFileDialog1.FilterIndex = 1;
            SaveFileDialog1.RestoreDirectory = true;
            if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    byte[] fff = HextoBytes(MissionMode);
                    string Ofile = SaveFileDialog1.FileName;
                    File.WriteAllBytes(Ofile, fff);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void pALToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            long.TryParse(TextboxMRFN.Text, out long nnn);
            string VMRFN = DectoHex_Uint(nnn);
            string VGM = DectoHex_Uint(Convert.ToInt64(ComboGM.SelectedIndex));
            string VCourse = DectoHex_Uint(Convert.ToInt64(ComboCourse.SelectedIndex));
            string VChar = DectoHex_Uint(Convert.ToInt64(ComboChar.SelectedIndex));
            string VVehicle = DectoHex_Uint(Convert.ToInt64(ComboVehicle.SelectedIndex));
            string VClass = DectoHex_Uint(Convert.ToInt64(ComboClass.SelectedIndex));
            long.TryParse(TextboxTime.Text, out nnn);
            string VTime = DectoHex_Uint(nnn);
            long.TryParse(TextboxScore.Text, out nnn);
            string VScore = DectoHex_Uint(nnn);
            VMRFN = "00000000" + VMRFN;
            VGM = "00000000" + VGM;
            VCourse = "00000000" + VCourse;
            VChar = "00000000" + VChar;
            VVehicle = "00000000" + VVehicle;
            VClass = "00000000" + VClass;
            VTime = "00000000" + VTime;
            VScore = "00000000" + VScore;
            VMRFN = VMRFN.Substring(VMRFN.Length - 4, 4);
            VGM = VGM.Substring(VGM.Length - 4, 4);
            VCourse = VCourse.Substring(VCourse.Length - 2, 2);
            VChar = VChar.Substring(VChar.Length - 2, 2);
            VVehicle = VVehicle.Substring(VVehicle.Length - 2, 2);
            VClass = VClass.Substring(VClass.Length - 2, 2);
            VTime = VTime.Substring(VTime.Length - 4, 4);
            VScore = VScore.Substring(VScore.Length - 8, 8);
            string Patch = Patchcode.Replace("GGGG", VMRFN);
            Patch = Patch.Replace("HHHH", VGM);
            Patch = Patch.Replace("II", VCourse);
            Patch = Patch.Replace("JJ", VChar);
            Patch = Patch.Replace("KK", VVehicle);
            Patch = Patch.Replace("LL", VClass);
            Patch = Patch.Replace("MMMM", VTime);
            Patch = Patch.Replace("NNNNNNNN", VScore);
            Clipboard.SetText(
                Begin_PAL + System.Environment.NewLine +
                Patch + System.Environment.NewLine +
                End_PAL
                );
        }

		private void NTSCUToolStripMenuItem_Click(object sender, EventArgs e)
		{
			long.TryParse(TextboxMRFN.Text, out long nnn);
			string VMRFN = DectoHex_Uint(nnn);
			string VGM = DectoHex_Uint(Convert.ToInt64(ComboGM.SelectedIndex));
			string VCourse = DectoHex_Uint(Convert.ToInt64(ComboCourse.SelectedIndex));
			string VChar = DectoHex_Uint(Convert.ToInt64(ComboChar.SelectedIndex));
			string VVehicle = DectoHex_Uint(Convert.ToInt64(ComboVehicle.SelectedIndex));
			string VClass = DectoHex_Uint(Convert.ToInt64(ComboClass.SelectedIndex));
			long.TryParse(TextboxTime.Text, out nnn);
			string VTime = DectoHex_Uint(nnn);
			long.TryParse(TextboxScore.Text, out nnn);
			string VScore = DectoHex_Uint(nnn);
			VMRFN = "00000000" + VMRFN;
			VGM = "00000000" + VGM;
			VCourse = "00000000" + VCourse;
			VChar = "00000000" + VChar;
			VVehicle = "00000000" + VVehicle;
			VClass = "00000000" + VClass;
			VTime = "00000000" + VTime;
			VScore = "00000000" + VScore;
			VMRFN = VMRFN.Substring(VMRFN.Length - 4, 4);
			VGM = VGM.Substring(VGM.Length - 4, 4);
			VCourse = VCourse.Substring(VCourse.Length - 2, 2);
			VChar = VChar.Substring(VChar.Length - 2, 2);
			VVehicle = VVehicle.Substring(VVehicle.Length - 2, 2);
			VClass = VClass.Substring(VClass.Length - 2, 2);
			VTime = VTime.Substring(VTime.Length - 4, 4);
			VScore = VScore.Substring(VScore.Length - 8, 8);
			string Patch = Patchcode.Replace("GGGG", VMRFN);
			Patch = Patch.Replace("HHHH", VGM);
			Patch = Patch.Replace("II", VCourse);
			Patch = Patch.Replace("JJ", VChar);
			Patch = Patch.Replace("KK", VVehicle);
			Patch = Patch.Replace("LL", VClass);
			Patch = Patch.Replace("MMMM", VTime);
			Patch = Patch.Replace("NNNNNNNN", VScore);
			string MissionMode = GCTHeader + Begin_NTSCU + Patch + End_NTSCU + GCTFooter;
			SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
			SaveFileDialog1.InitialDirectory = "c:\\";
			SaveFileDialog1.Filter = "Gecko Cheat Code File (*.gct)|*.gct";
			SaveFileDialog1.FilterIndex = 1;
			SaveFileDialog1.RestoreDirectory = true;
			if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				try
				{
					byte[] fff = HextoBytes(MissionMode);
					string Ofile = SaveFileDialog1.FileName;
					File.WriteAllBytes(Ofile, fff);
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
				}
			}
		}

		private void NTSCUToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			long.TryParse(TextboxMRFN.Text, out long nnn);
			string VMRFN = DectoHex_Uint(nnn);
			string VGM = DectoHex_Uint(Convert.ToInt64(ComboGM.SelectedIndex));
			string VCourse = DectoHex_Uint(Convert.ToInt64(ComboCourse.SelectedIndex));
			string VChar = DectoHex_Uint(Convert.ToInt64(ComboChar.SelectedIndex));
			string VVehicle = DectoHex_Uint(Convert.ToInt64(ComboVehicle.SelectedIndex));
			string VClass = DectoHex_Uint(Convert.ToInt64(ComboClass.SelectedIndex));
			long.TryParse(TextboxTime.Text, out nnn);
			string VTime = DectoHex_Uint(nnn);
			long.TryParse(TextboxScore.Text, out nnn);
			string VScore = DectoHex_Uint(nnn);
			VMRFN = "00000000" + VMRFN;
			VGM = "00000000" + VGM;
			VCourse = "00000000" + VCourse;
			VChar = "00000000" + VChar;
			VVehicle = "00000000" + VVehicle;
			VClass = "00000000" + VClass;
			VTime = "00000000" + VTime;
			VScore = "00000000" + VScore;
			VMRFN = VMRFN.Substring(VMRFN.Length - 4, 4);
			VGM = VGM.Substring(VGM.Length - 4, 4);
			VCourse = VCourse.Substring(VCourse.Length - 2, 2);
			VChar = VChar.Substring(VChar.Length - 2, 2);
			VVehicle = VVehicle.Substring(VVehicle.Length - 2, 2);
			VClass = VClass.Substring(VClass.Length - 2, 2);
			VTime = VTime.Substring(VTime.Length - 4, 4);
			VScore = VScore.Substring(VScore.Length - 8, 8);
			string Patch = Patchcode.Replace("GGGG", VMRFN);
			Patch = Patch.Replace("HHHH", VGM);
			Patch = Patch.Replace("II", VCourse);
			Patch = Patch.Replace("JJ", VChar);
			Patch = Patch.Replace("KK", VVehicle);
			Patch = Patch.Replace("LL", VClass);
			Patch = Patch.Replace("MMMM", VTime);
			Patch = Patch.Replace("NNNNNNNN", VScore);
			Clipboard.SetText(
				Begin_NTSCU + System.Environment.NewLine +
				Patch + System.Environment.NewLine +
				End_NTSCU
				);
		}

		private void NTSCJToolStripMenuItem_Click(object sender, EventArgs e)
		{
			long.TryParse(TextboxMRFN.Text, out long nnn);
			string VMRFN = DectoHex_Uint(nnn);
			string VGM = DectoHex_Uint(Convert.ToInt64(ComboGM.SelectedIndex));
			string VCourse = DectoHex_Uint(Convert.ToInt64(ComboCourse.SelectedIndex));
			string VChar = DectoHex_Uint(Convert.ToInt64(ComboChar.SelectedIndex));
			string VVehicle = DectoHex_Uint(Convert.ToInt64(ComboVehicle.SelectedIndex));
			string VClass = DectoHex_Uint(Convert.ToInt64(ComboClass.SelectedIndex));
			long.TryParse(TextboxTime.Text, out nnn);
			string VTime = DectoHex_Uint(nnn);
			long.TryParse(TextboxScore.Text, out nnn);
			string VScore = DectoHex_Uint(nnn);
			VMRFN = "00000000" + VMRFN;
			VGM = "00000000" + VGM;
			VCourse = "00000000" + VCourse;
			VChar = "00000000" + VChar;
			VVehicle = "00000000" + VVehicle;
			VClass = "00000000" + VClass;
			VTime = "00000000" + VTime;
			VScore = "00000000" + VScore;
			VMRFN = VMRFN.Substring(VMRFN.Length - 4, 4);
			VGM = VGM.Substring(VGM.Length - 4, 4);
			VCourse = VCourse.Substring(VCourse.Length - 2, 2);
			VChar = VChar.Substring(VChar.Length - 2, 2);
			VVehicle = VVehicle.Substring(VVehicle.Length - 2, 2);
			VClass = VClass.Substring(VClass.Length - 2, 2);
			VTime = VTime.Substring(VTime.Length - 4, 4);
			VScore = VScore.Substring(VScore.Length - 8, 8);
			string Patch = Patchcode.Replace("GGGG", VMRFN);
			Patch = Patch.Replace("HHHH", VGM);
			Patch = Patch.Replace("II", VCourse);
			Patch = Patch.Replace("JJ", VChar);
			Patch = Patch.Replace("KK", VVehicle);
			Patch = Patch.Replace("LL", VClass);
			Patch = Patch.Replace("MMMM", VTime);
			Patch = Patch.Replace("NNNNNNNN", VScore);
			string MissionMode = GCTHeader + Begin_NTSCJ + Patch + End_NTSCJ + GCTFooter;
			SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
			SaveFileDialog1.InitialDirectory = "c:\\";
			SaveFileDialog1.Filter = "Gecko Cheat Code File (*.gct)|*.gct";
			SaveFileDialog1.FilterIndex = 1;
			SaveFileDialog1.RestoreDirectory = true;
			if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				try
				{
					byte[] fff = HextoBytes(MissionMode);
					string Ofile = SaveFileDialog1.FileName;
					File.WriteAllBytes(Ofile, fff);
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
				}
			}
		}

		private void NTSCJToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			long.TryParse(TextboxMRFN.Text, out long nnn);
			string VMRFN = DectoHex_Uint(nnn);
			string VGM = DectoHex_Uint(Convert.ToInt64(ComboGM.SelectedIndex));
			string VCourse = DectoHex_Uint(Convert.ToInt64(ComboCourse.SelectedIndex));
			string VChar = DectoHex_Uint(Convert.ToInt64(ComboChar.SelectedIndex));
			string VVehicle = DectoHex_Uint(Convert.ToInt64(ComboVehicle.SelectedIndex));
			string VClass = DectoHex_Uint(Convert.ToInt64(ComboClass.SelectedIndex));
			long.TryParse(TextboxTime.Text, out nnn);
			string VTime = DectoHex_Uint(nnn);
			long.TryParse(TextboxScore.Text, out nnn);
			string VScore = DectoHex_Uint(nnn);
			VMRFN = "00000000" + VMRFN;
			VGM = "00000000" + VGM;
			VCourse = "00000000" + VCourse;
			VChar = "00000000" + VChar;
			VVehicle = "00000000" + VVehicle;
			VClass = "00000000" + VClass;
			VTime = "00000000" + VTime;
			VScore = "00000000" + VScore;
			VMRFN = VMRFN.Substring(VMRFN.Length - 4, 4);
			VGM = VGM.Substring(VGM.Length - 4, 4);
			VCourse = VCourse.Substring(VCourse.Length - 2, 2);
			VChar = VChar.Substring(VChar.Length - 2, 2);
			VVehicle = VVehicle.Substring(VVehicle.Length - 2, 2);
			VClass = VClass.Substring(VClass.Length - 2, 2);
			VTime = VTime.Substring(VTime.Length - 4, 4);
			VScore = VScore.Substring(VScore.Length - 8, 8);
			string Patch = Patchcode.Replace("GGGG", VMRFN);
			Patch = Patch.Replace("HHHH", VGM);
			Patch = Patch.Replace("II", VCourse);
			Patch = Patch.Replace("JJ", VChar);
			Patch = Patch.Replace("KK", VVehicle);
			Patch = Patch.Replace("LL", VClass);
			Patch = Patch.Replace("MMMM", VTime);
			Patch = Patch.Replace("NNNNNNNN", VScore);
			Clipboard.SetText(
				Begin_NTSCJ + System.Environment.NewLine +
				Patch + System.Environment.NewLine +
				End_NTSCJ
				);
		}


		private void Radio1(object sender, EventArgs e)
		{
			if (radioButton1.Checked == true) { MLevel = 0; }
			if (radioButton2.Checked == true) { MLevel = 1; }
			if (radioButton3.Checked == true) { MLevel = 2; }
			if (radioButton4.Checked == true) { MLevel = 3; }
			if (radioButton5.Checked == true) { MLevel = 4; }
			if (radioButton6.Checked == true) { MLevel = 5; }
			if (radioButton7.Checked == true) { MLevel = 6; }
			if (radioButton8.Checked == true) { MLevel = 7; }
			if (radioButton16.Checked == true) { MMission = 0; }
			if (radioButton15.Checked == true) { MMission = 1; }
			if (radioButton14.Checked == true) { MMission = 2; }
			if (radioButton13.Checked == true) { MMission = 3; }
			if (radioButton12.Checked == true) { MMission = 4; }
			if (radioButton11.Checked == true) { MMission = 5; }
			if (radioButton10.Checked == true) { MMission = 6; }
			if (radioButton9.Checked == true) { MMission = 7; }

			byte[] BbbCourse = { DataCourse[MLevel * 8 + MMission], 0, 0, 0 };
			byte[] BbbChar = { DataChar[MLevel * 8 + MMission], 0, 0, 0 };
			byte[] BbbVehicle = { DataVehicle[MLevel * 8 + MMission], 0, 0, 0 };
			byte[] BbbClass = { DataClass[MLevel * 8 + MMission], 0, 0, 0 };

			TextboxMRFN.Text = DataMRFN[MLevel * 8 + MMission].ToString();
			ComboGM.SelectedIndex = DataGM[MLevel * 8 + MMission];
			ComboCourse.SelectedIndex = BitConverter.ToInt32(BbbCourse, 0);
			ComboChar.SelectedIndex = BitConverter.ToInt32(BbbChar, 0);
			ComboVehicle.SelectedIndex = BitConverter.ToInt32(BbbVehicle, 0);
			ComboClass.SelectedIndex = BitConverter.ToInt32(BbbClass, 0);
			TextboxTime.Text = DataTime[MLevel * 8 + MMission].ToString();
			TextboxScore.Text = DataScore[MLevel * 8 + MMission].ToString();
		}

		private void ComboCourse_SelectedIndexChanged(object sender, EventArgs e)
		{
			DataCourse[MLevel * 8 + MMission] = IntToAByte(ComboCourse.SelectedIndex);
		}

		private void ComboGM_SelectedValueChanged(object sender, EventArgs e)
		{
			DataGM[MLevel * 8 + MMission] = ComboGM.SelectedIndex;
		}

		private void ComboChar_SelectedIndexChanged(object sender, EventArgs e)
		{
			DataChar[MLevel * 8 + MMission] = IntToAByte(ComboChar.SelectedIndex);
		}

		private void ComboVehicle_SelectedIndexChanged(object sender, EventArgs e)
		{
			DataVehicle[MLevel * 8 + MMission] = IntToAByte(ComboVehicle.SelectedIndex);
		}

		private void ComboClass_SelectedIndexChanged(object sender, EventArgs e)
		{
			DataClass[MLevel * 8 + MMission] = IntToAByte(ComboClass.SelectedIndex);
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
			SaveFileDialog1.InitialDirectory = "c:\\";
			SaveFileDialog1.Filter = "KMT File (*.kmt)|*.kmt";
			SaveFileDialog1.FilterIndex = 1;
			SaveFileDialog1.RestoreDirectory = true;
			if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				try
				{
					SaveKMT(SaveFileDialog1.FileName);
					OpenF = SaveFileDialog1.FileName; Text = OpenF + " - " + Pname;
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
				}
			}
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DialogResult = MessageBox.Show("Any unsaved changes will be lost. Is this OK?", "Warning",
 MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
			if (DialogResult == DialogResult.Yes)
			{
				OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
				OpenFileDialog1.InitialDirectory = "c:\\";
				OpenFileDialog1.Filter = "KMT File (*.kmt)|*.kmt";
				OpenFileDialog1.FilterIndex = 1;
				OpenFileDialog1.RestoreDirectory = true;
				if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
				{
					try
					{
						//MessageBox.Show(OpenFileDialog1.FileName);
						string Ofile = OpenFileDialog1.FileName;
						OpenF = Ofile; Text = OpenF + " - " + Pname;
						using (BinaryReader b = new BinaryReader(
						File.Open(OpenFileDialog1.FileName, FileMode.Open)))
						{

							int length = (int)b.BaseStream.Length;

							if (length == 7184)
							{
								byte[] Filebyte = null;
								Filebyte = new byte[7184 - 16];

								for (int nn = 0; nn < 7184; nn = nn + 1)
								{
									byte stt = b.ReadByte();
									if (nn>=16) { Filebyte[nn - 16] = stt;}
									//MessageBox.Show(Filebyte[nn - 16].ToString());
								}

								for (int nn = 0; nn < 64; nn = nn + 1)
								{
									//MessageBox.Show(Filebyte[(nn * 112) + 1].ToString());
									byte[] BitMRFN = { Filebyte[(nn*112) + 1], Filebyte[(nn*112)], 0, 0 };
									byte[] BitGM = { Filebyte[(nn*112) + 3], Filebyte[(nn*112) + 2], 0, 0 };
									byte[] BitTime = { Filebyte[(nn*112) + 45], Filebyte[(nn*112) + 44], 0, 0 };
									byte[] BitScore = { Filebyte[(nn*112) + 51], Filebyte[(nn*112) + 50], Filebyte[(nn*112) + 49], Filebyte[(nn*112) + 48], 0, 0, 0, 0 };
									DataMRFN[nn] = BitConverter.ToInt32(BitMRFN, 0);
									DataGM[nn] = BitConverter.ToInt32(BitGM, 0);
									DataCourse[nn] = Filebyte[(nn*112) + 4];
									DataChar[nn] = Filebyte[(nn*112) + 5];
									DataVehicle[nn] = Filebyte[(nn*112) + 6];
									DataClass[nn] = Filebyte[(nn*112) + 7];
									DataTime[nn] = BitConverter.ToInt32(BitTime, 0);
									DataScore[nn] = BitConverter.ToInt64(BitScore, 0);
								}
							}
							else
							{
								if (length < 7184)
									MessageBox.Show("Files must be 7184 bytes in size.", "File too small", MessageBoxButtons.OK, MessageBoxIcon.Error);
								if (length > 7184)
									MessageBox.Show("Files must be 7184 bytes in size.", "File too large", MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
					}
				}
			}

			byte[] BbbCourse = { DataCourse[MLevel * 8 + MMission], 0, 0, 0 };
			byte[] BbbChar = { DataChar[MLevel * 8 + MMission], 0, 0, 0 };
			byte[] BbbVehicle = { DataVehicle[MLevel * 8 + MMission], 0, 0, 0 };
			byte[] BbbClass = { DataClass[MLevel * 8 + MMission], 0, 0, 0 };

			TextboxMRFN.Text = DataMRFN[MLevel * 8 + MMission].ToString();
			ComboGM.SelectedIndex = DataGM[MLevel * 8 + MMission];
			ComboCourse.SelectedIndex = BitConverter.ToInt32(BbbCourse, 0);
			ComboChar.SelectedIndex = BitConverter.ToInt32(BbbChar, 0);
			ComboVehicle.SelectedIndex = BitConverter.ToInt32(BbbVehicle, 0);
			ComboClass.SelectedIndex = BitConverter.ToInt32(BbbClass, 0);
			TextboxTime.Text = DataTime[MLevel * 8 + MMission].ToString();
			TextboxScore.Text = DataScore[MLevel * 8 + MMission].ToString();
		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DialogResult = MessageBox.Show("Any unsaved changes will be lost. Is this OK?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
			if (DialogResult == DialogResult.Yes)
			{
				OpenF = "Untitled"; Text = OpenF + " - " + Pname;

				int[] DataMRFN = {
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
				};

				int[] DataGM = {
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
				};

				byte[] DataCourse = {
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
				};

				byte[] DataChar = {
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
				};

				byte[] DataVehicle = {
					1, 1, 1, 1, 1, 1, 1, 1,
					1, 1, 1, 1, 1, 1, 1, 1,
					1, 1, 1, 1, 1, 1, 1, 1,
					1, 1, 1, 1, 1, 1, 1, 1,
					1, 1, 1, 1, 1, 1, 1, 1,
					1, 1, 1, 1, 1, 1, 1, 1,
					1, 1, 1, 1, 1, 1, 1, 1,
					1, 1, 1, 1, 1, 1, 1, 1,
				};

				byte[] DataClass = {
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
				};

				int[] DataTime = {
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
				};

				long[] DataScore = {
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0,
				};

				byte[] BbbCourse = { DataCourse[MLevel * 8 + MMission], 0, 0, 0 };
				byte[] BbbChar = { DataChar[MLevel * 8 + MMission], 0, 0, 0 };
				byte[] BbbVehicle = { DataVehicle[MLevel * 8 + MMission], 0, 0, 0 };
				byte[] BbbClass = { DataClass[MLevel * 8 + MMission], 0, 0, 0 };

				TextboxMRFN.Text = DataMRFN[MLevel * 8 + MMission].ToString();
				ComboGM.SelectedIndex = DataGM[MLevel * 8 + MMission];
				ComboCourse.SelectedIndex = BitConverter.ToInt32(BbbCourse, 0);
				ComboChar.SelectedIndex = BitConverter.ToInt32(BbbChar, 0);
				ComboVehicle.SelectedIndex = BitConverter.ToInt32(BbbVehicle, 0);
				ComboClass.SelectedIndex = BitConverter.ToInt32(BbbClass, 0);
				TextboxTime.Text = DataTime[MLevel * 8 + MMission].ToString();
				TextboxScore.Text = DataScore[MLevel * 8 + MMission].ToString();
			}
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (File.Exists(OpenF))
			{
				SaveKMT(OpenF);
				Text = OpenF + " - " + Pname;
			}
			else
			{
				SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
				SaveFileDialog1.InitialDirectory = "c:\\";
				SaveFileDialog1.Filter = "KMT File (*.kmt)|*.kmt";
				SaveFileDialog1.FilterIndex = 1;
				SaveFileDialog1.RestoreDirectory = true;
				if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
				{
					try
					{
						SaveKMT(SaveFileDialog1.FileName);
						OpenF = SaveFileDialog1.FileName; Text = OpenF + " - " + Pname;
					}
					catch (Exception ex)
					{
						MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
					}
				}
			}
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Creator: TheZACAtac\nVersion: Beta\nWiki: wiki.tockdom.com/wiki/KMT_Modifier Express", "About KMT Modifier Express", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	}
}
