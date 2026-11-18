using System;
using System.Diagnostics;
using System.Globalization;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Windows.Forms;
     
namespace juliemarie
{
    public partial class Form1 : Form
    {
        private SpeechRecognitionEngine recognizer;
        private SpeechSynthesizer speaker;

        public Form1()
        {
            InitializeComponent();

            speaker = new SpeechSynthesizer();

            InitializeSpeechRecognition();

            lblRecognized.Text = "None";
            lblStatus.Text = "Status: Not Listening";

            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

        private void InitializeSpeechRecognition()
        {
            try
            {
                recognizer = new SpeechRecognitionEngine(
                    new CultureInfo("en-US")
                );

                Choices commands = new Choices();

                commands.Add(new string[]
                {
                    "Open Word",
                    "Open Notepad",
                    "Open Calculator",
                    "Open Paint",
                    "Open File Explorer"
                });

                GrammarBuilder grammarBuilder = new GrammarBuilder();

                grammarBuilder.Append(commands);

                Grammar grammar = new Grammar(grammarBuilder);

                recognizer.LoadGrammar(grammar);

                recognizer.SetInputToDefaultAudioDevice();

                recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error initializing speech recognition.\n\n" +
                    ex.Message,
                    "Speech Recognition Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void Recognizer_SpeechRecognized(
            object sender,
            SpeechRecognizedEventArgs e)
        {
            try
            {
                string command = e.Result.Text;

                lblRecognized.Text =
                    "Recognized Command: " + command;

                speaker.SpeakAsync(command);

                OpenApplication(command);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error occurred while processing the voice command.\n\n" +
                    ex.Message,
                    "Recognition Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void OpenApplication(string command)
        {
            try
            {
                switch (command)
                {
                    case "Open Word":
                        Process.Start("winword.exe");

                        break;

                    case "Open Notepad":
                        Process.Start("notepad.exe");

                        break;

                    case "Open Calculator":
                        Process.Start("calc.exe");

                        break;

                    case "Open Paint":
                        Process.Start("mspaint.exe");

                        break;

                    case "Open File Explorer":
                        Process.Start("explorer.exe");

                        break;
                }
            }
            catch (System.ComponentModel.Win32Exception)
            {
                MessageBox.Show(
                    "The application could not be opened.\n\n" +
                    "Please make sure the application is installed " +
                    "on this computer.",
                    "Application Unavailable",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void btnStart_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                recognizer.RecognizeAsync(
                    RecognizeMode.Multiple
                );

                lblStatus.Text = "Status: Listening...";

                btnStart.Enabled = false;
                btnStop.Enabled = true;
            }
            catch
            {
            }
        }

        private void btnStop_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                if (recognizer != null)
                {
                    recognizer.RecognizeAsyncStop();
                }

                lblStatus.Text = "Status: Not Listening";

                btnStart.Enabled = true;
                btnStop.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to stop speech recognition.\n\n" +
                    ex.Message,
                    "Recognition Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        protected override void OnFormClosing(
            FormClosingEventArgs e)
        {
            if (recognizer != null)
            {
                recognizer.RecognizeAsyncCancel();
                recognizer.Dispose();
            }

            if (speaker != null)
            {
                speaker.Dispose();
            }

            base.OnFormClosing(e);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

  