using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Civ5PrivateSave
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        byte[] myPattern = new byte[] { 0x01, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x40 }; //the byte just this pattern before contains the public/private info
        byte[] myByteArray = new byte[] { };
        int indexByteStatus = -1;
        bool isPrivate = true;
        bool detected = false;
        string lastFile = "";

        private void button1_Click(object sender, EventArgs e)
        {
            detected = false;
            button2.Enabled = false;
            openFileDialog1.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\my games\\Sid Meier's Civilization 5\\Saves\\multi";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                lastFile = openFileDialog1.FileName;
                myByteArray = File.ReadAllBytes(lastFile);
                indexByteStatus = IndexOf(myByteArray, myPattern) - 1;
                if (indexByteStatus > -1)
                {
                    if (myByteArray[indexByteStatus] == 0) { isPrivate = false; } else { isPrivate = true; }
                    detected = true;
                }
                else
                {
                    MessageBox.Show("Value not found, I can't help you, sorry.");
                }
                UpdateButton();
            }
            

        }

        

        public static int IndexOf(byte[] arrayToSearchThrough, byte[] patternToFind)
        {
            if (patternToFind.Length > arrayToSearchThrough.Length)
                return -1;
            for (int i = 0; i < arrayToSearchThrough.Length - patternToFind.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < patternToFind.Length; j++)
                {
                    if (arrayToSearchThrough[i + j] != patternToFind[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    return i;
                }
            }
            return -1;
        }

        private void UpdateButton()
        {
            if (detected)
            {
                button2.Enabled = true;
                button3.Enabled = true;
                if (isPrivate)
                {
                    button2.Text = "Make it public";
                }
                else
                {
                    button2.Text = "Make it private";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(isPrivate) { myByteArray[indexByteStatus] = 0; isPrivate = !isPrivate; } else { myByteArray[indexByteStatus] = 1; isPrivate = !isPrivate; }
            UpdateButton();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = lastFile;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(saveFileDialog1.FileName, myByteArray);
                MessageBox.Show("File saved.");
            }
        }
    }
}
