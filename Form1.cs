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
using System.Globalization;
using System.Net;

namespace HOI4_GFX_Generator
{
    public partial class Form1 : Form
    {
        private List<string> selectedFiles = new List<string>();
        public Form1()

        {
            InitializeComponent();

            openFileDialog1.Multiselect = true;
            openFileDialog2.Multiselect = true;
            openFileDialog3.Multiselect = true;

            button1.Click += new EventHandler(button1_Click);
            button2.Click += new EventHandler(button2_Click);
            button3.Click += new EventHandler(button3_Click);
            button4.Click += new EventHandler(button4_Click);
            button5.Click += new EventHandler(button5_Click);
            button6.Click += new EventHandler(button6_Click);
            button7.Click += new EventHandler(button7_Click);
            button8.Click += new EventHandler(button8_Click);
            button9.Click += new EventHandler(button9_Click);
            toolTip1.SetToolTip(this.numericUpDown1, "Number of frames");
            toolTip1.SetToolTip(this.numericUpDown2, "Defines the length of time to pause for after an animation loop");
            toolTip1.SetToolTip(this.checkBoxPlayOnShow, "Defines whether the animations starts playing when visible");
            label5.Text = "Version: " + Application.ProductVersion;
            label6.Text = "Version: " + Application.ProductVersion;
            this.Load += new EventHandler(Form1_Load);
        }
        ToolTip toolTip1 = new ToolTip();
        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForUpdates();
        }

        private void CheckForUpdates()
        {
            try
            {

                string versionInfoUrl = "https://hoi4gfxgenerator.neocities.org/version.txt";


                WebClient webClient = new WebClient();
                string latestVersion = webClient.DownloadString(versionInfoUrl).Trim();


                string currentVersion = Application.ProductVersion;


                if (latestVersion != currentVersion)
                {
                    DialogResult dialogResult = MessageBox.Show("An update is available. Latest version: " + latestVersion + ". Do you want to download it now?", "Update Available", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {

                        System.Diagnostics.Process.Start("https://github.com/osleek/HOI4-GFX-Generator/releases/latest/download/HOI4.GFX.Generator.exe");
                    }
                }
            }
            catch
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                if (!checkBox1.Checked)
                {
                    textBox1.Clear();
                    textBox2.Clear();
                }

                foreach (string filePath in openFileDialog1.FileNames)
                {
                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                    string fileDirectory = Path.GetDirectoryName(filePath);

                    int index = fileDirectory.IndexOf("gfx");
                    if (index < 0)
                    {
                        MessageBox.Show("Folder 'gfx' not found in file path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string relativePath = fileDirectory.Remove(0, index).Replace("\\", "/");

                    string prefix = fileName.StartsWith("idea_") || !checkBox3.Checked ? "" : "idea_";

                    string spriteType = $@"
spriteType = {{
    name = ""GFX_{prefix}{fileName}""
    texturefile = ""{relativePath}/{fileName}.dds""
}}";

                    textBox1.AppendText(spriteType);
                    textBox1.AppendText(Environment.NewLine);


                    string spriteType2 = $@"
    SpriteType = {{
        name = ""GFX_{fileName}_shine""
        texturefile = ""{relativePath}/{fileName}.dds""
        effectFile = ""gfx/FX/buttonstate.lua""
        animation = {{
            animationmaskfile = ""{relativePath}/{fileName}.dds""			
            animationtexturefile = ""gfx/interface/goals/shine_overlay.dds""
            animationrotation = -90.0
            animationlooping = no
            animationtime = 0.75
            animationdelay = 0
            animationblendmode = ""add""
            animationtype = ""scrolling""
            animationrotationoffset = {{ x = 0.0 y = 0.0 }}
            animationtexturescale = {{ x = 1.0 y = 1.0 }} 
        }}

        animation = {{
            animationmaskfile = ""{relativePath}/{fileName}.dds""			
            animationtexturefile = ""gfx/interface/goals/shine_overlay.dds""
            animationrotation = 90.0
            animationlooping = no
            animationtime = 0.75
            animationdelay = 0
            animationblendmode = ""add""
            animationtype = ""scrolling""
            animationrotationoffset = {{ x = 0.0 y = 0.0 }}
            animationtexturescale = {{ x = 1.0 y = 1.0 }} 
        }}
        legacy_lazy_load = no
    }}";
                    if (!checkBox3.Checked)
                    {
                        textBox2.AppendText(spriteType2);
                        textBox2.AppendText(Environment.NewLine);
                    }
                    else
                    {

                    }

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("No text to copy.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Clipboard.SetText(textBox1.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("No text to copy.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Clipboard.SetText(textBox2.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "GFX File|*.gfx";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                {
                    sw.WriteLine("spriteTypes = {");
                    sw.WriteLine(textBox1.Text);
                    sw.WriteLine("}");
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "GFX File|*.gfx";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog1.FileName))
                {
                    sw.WriteLine("spriteTypes = {");
                    sw.WriteLine(textBox2.Text);
                    sw.WriteLine("}");
                }
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Если чекбокс не отмечен, очистите список файлов.
                if (!checkBox2.Checked)
                {
                    selectedFiles.Clear();
                }

                // Добавьте выбранные файлы в список
                selectedFiles.AddRange(openFileDialog1.FileNames);
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            // Если чекбокс не отмечен, очистите текстовые поля.
            if (!checkBox2.Checked)
            {
                textBox3.Clear();
            }

            foreach (string filePath in selectedFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string fileDirectory = Path.GetDirectoryName(filePath);

                int index = fileDirectory.IndexOf("gfx");
                if (index < 0)
                {
                    MessageBox.Show("Folder 'gfx' not found in file path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string relativePath = fileDirectory.Remove(0, index).Replace("\\", "/");

                // Проверьте значения параметров из текстовых полей
                int noOfFrames;
                if (!int.TryParse(numericUpDown1.Text, out noOfFrames))
                {
                    MessageBox.Show("Invalid noOfFrames. Must be a number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string loadType = comboBox1.Text;
                if (!new List<string> { "INGAME", "FRONTEND" }.Contains(loadType))
                {
                    MessageBox.Show("Invalid loadType. Must be INGAME or FRONTEND.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool transparencecheck = checkBoxTransparenceCheck.Checked;

                int animation_rate_fps;
                if (!int.TryParse(numericUpDown3.Text, out animation_rate_fps))
                {
                    MessageBox.Show("Invalid animation_rate_fps. Must be a number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool looping = checkBoxLooping.Checked;
                bool play_on_show = checkBoxPlayOnShow.Checked;

                float pause_on_loop;
                if (!float.TryParse(numericUpDown2.Text, out pause_on_loop))
                {
                    MessageBox.Show("Invalid pause_on_loop. Must be a number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string frameAnimatedSpriteType = $@"
    frameAnimatedSpriteType = {{
        name = ""GFX_{fileName}""
        texturefile = ""{relativePath}/{fileName}.dds""
        noOfFrames = {noOfFrames}
        loadType = ""{loadType}""
        transparencecheck = {transparencecheck}
        animation_rate_fps = {animation_rate_fps}
        looping = {looping}
        pause_on_loop = {pause_on_loop.ToString(CultureInfo.InvariantCulture)}
    }}";


                textBox3.AppendText(frameAnimatedSpriteType);
                textBox3.AppendText(Environment.NewLine);

            }
            
        }
        private void button8_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "GFX File|*.gfx";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog1.FileName))
                {
                    sw.WriteLine("spriteTypes = {");
                    sw.WriteLine(textBox3.Text);
                    sw.WriteLine("}");
                }
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("No text to copy.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Clipboard.SetText(textBox3.Text);
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {

                string versionInfoUrl = "https://hoi4gfxgenerator.neocities.org/version.txt";


                WebClient webClient = new WebClient();
                string latestVersion = webClient.DownloadString(versionInfoUrl).Trim();


                string currentVersion = Application.ProductVersion;


                if (latestVersion != currentVersion)
                {
                    DialogResult dialogResult = MessageBox.Show("An update is available. Latest version: " + latestVersion + ". Do you want to download it now?", "Update Available", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("https://github.com/osleek/HOI4-GFX-Generator/releases/latest/download/HOI4.GFX.Generator.exe");
                    }
                }
                else
                {
                    MessageBox.Show("You have the latest version of the application.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to check for updates. Check your internet connection. Error: " + ex.Message);
            }
        }
    }
}

        