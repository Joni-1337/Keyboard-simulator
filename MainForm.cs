using System;
using System.Windows.Forms;
using System.IO;
using System.Media;

namespace Keyboard
{
    public partial class MainForm : Form
    {
        public double allTime=0;
        public int countError;
        public int countChars;
        public int level;
        public int currentString;
        public int currentChar;
        public string[] lines;
        public double inputSpeed;
        public string input;

        public MainForm()
        {
            InitializeComponent();
            startButton.Enabled = false;
            KeyPreview = true;
        }

        private void СправкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefForm refForm = new RefForm();
            refForm.Show();
        }

        private void LevelZeroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            level = 0;
            levelLabel.Text = "Уровень сложности: Ознакомительный";
            startButton.Enabled = true;
            helpLabel.Visible = false;
        }

        private void LevelFirstToolStripMenuItem_Click(object sender, EventArgs e)
        {
            level = 1;
            levelLabel.Text = "Уровень сложности: 1";
            startButton.Enabled = true;
            helpLabel.Visible = false;
        }

        private void LevelSecondToolStripMenuItem_Click(object sender, EventArgs e)
        {
            level = 2;
            levelLabel.Text = "Уровень сложности: 2";
            startButton.Enabled = true;
            helpLabel.Visible = false;
        }

        private void LevelThirdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            level = 3;
            levelLabel.Text = "Уровень сложности: 3";
            startButton.Enabled = true;
            helpLabel.Visible = false;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (startButton.Text == "Start")
            {
                startButton.Text = "Stop";
                currentString = 0;
                currentChar = 0;
                countChars = 0;
                countError = 0;
                allTime = 0;
                inputSpeed = 0;
                changeLevelToolStripMenuItem.Enabled = false;
                decisionLabel.Text = " ";
                decisionLabel.Focus();
            }
            else
            {
                startButton.Text = "Start";
                lines = null;
                changeLevelToolStripMenuItem.Enabled = true;
                input = "";
            }
            if (startButton.Text == "Stop")
            {
                timer1.Enabled = true;
                switch (level)
                {
                    case 0: lines = File.ReadAllLines(@".\zero.txt"); break;
                    case 1: lines = File.ReadAllLines(@".\first.txt"); break;
                    case 2: lines = File.ReadAllLines(@".\second.txt"); break;
                    case 3: lines = File.ReadAllLines(@".\third.txt"); break;
                }
                taskLabel.Text = lines[currentString].ToString();
                subLevelLabel.Text = "Подуровень: 1/" + lines.Length;
            }
            else timer1.Enabled = false;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            allTime++;
            timerLabel.Text = "Время: " + allTime.ToString() + "сек.";
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (startButton.Text == "Start") return;
            if (e.KeyChar.ToString() == lines[currentString][currentChar].ToString())
            {
                input += e.KeyChar.ToString();
                decisionLabel.Text = input;
                currentChar++;
            }
            else
            {
                SystemSounds.Beep.Play();
                countError++;
            }
            countChars++;
            inputSpeed = countChars / allTime * 60;
            speedLabel.Text = "Скорость набора: " + Math.Round(inputSpeed).ToString() + " сим/мин";
            if (input.Length == lines[currentString].Length)
            {
                currentChar = 0;
                currentString++;
                if (currentString+1<=lines.Length) subLevelLabel.Text = "Подуровень: " + (currentString + 1).ToString() + "/" + lines.Length;
                if (currentString == lines.Length)
                {
                    timer1.Enabled = false;
                    startButton.Text = "Start";
                    changeLevelToolStripMenuItem.Enabled = true;
                    MessageBox.Show("Уровень пройден", "Complete", MessageBoxButtons.OKCancel);
                    if (level < 3)
                    {
                        level++;
                        levelLabel.Text = "Уровень сложности: " + level.ToString();
                        taskLabel.Text = "";
                        decisionLabel.Text = " ";
                        input = "";
                        countError = 0;
                        allTime = 0;
                        speedLabel.Text = "Скорость набора: 0 сим/мин";
                        errorLabel.Text = "Количество ошибок: 0";
                        timerLabel.Text = "Время: 0 сек.";
                        subLevelLabel.Text = "Подуровень: ";
                    }
                    else MessageBox.Show("Все уровни пройдены!", "Complete", MessageBoxButtons.OK);
                }
                else
                {
                    taskLabel.Text = lines[currentString];
                    decisionLabel.Text = " ";
                    input = "";
                }
            }
            errorLabel.Text = "Количество ошибок: " + countError.ToString();
        }
    }
}
