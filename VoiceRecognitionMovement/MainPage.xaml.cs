using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Media.Capture;
using Windows.Media.SpeechRecognition;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace VoiceRecognitionMovement
{
    public sealed partial class MainPage : Page
    {
        private const string SRGS_FILE = "Gramatica\\gramatica.xml";
        private const string TAG_CMD = "cmd";
        private const string TAG_DEVICE = "device";

        private const string CMD_PARAR = "PARAR";
        private const string CMD_ANDAR = "ANDAR";
        private const string CMD_ANDA = "ANDA";
        private const string CMD_VIRAR = "VIRAR";
        private const string CMD_VIRA = "VIRA";
        private const string DEVICE_FRENTE = "FRENTE";
        private const string DEVICE_TRAS = "TRAS";
        private const string DEVICE_ESQEUERDA = "ESQUERDA";
        private const string DEVICE_DIREITA = "DIREITA";

        private SpeechRecognizer recognizer;

        Thickness margin;

        public MainPage()
        {
            this.InitializeComponent();
           

            InicializaVoz();

        }

        /// <summary>
        /// Método que inicia o reconhecimento de voz
        /// </summary>
        public async void InicializaVoz()
        {
            bool hasPermission = await CheckMicrophonePermission();

            if (hasPermission)
            {
                var idioma = new Windows.Globalization.Language("en-US");
                recognizer = new SpeechRecognizer(idioma);
                //recognizer.StateChanged += RecognizerStateChanged;
                recognizer.ContinuousRecognitionSession.ResultGenerated += RecognizerResultGenerated;
                string fileName = String.Format(SRGS_FILE);
                StorageFile grammarContentFile = await Package.Current.InstalledLocation.GetFileAsync(fileName);
                SpeechRecognitionGrammarFileConstraint grammarConstraint = new SpeechRecognitionGrammarFileConstraint(grammarContentFile);
                recognizer.Constraints.Add(grammarConstraint);
                SpeechRecognitionCompilationResult compilationResult = await recognizer.CompileConstraintsAsync();

                if (compilationResult.Status == SpeechRecognitionResultStatus.Success)
                {
                    await recognizer.ContinuousRecognitionSession.StartAsync();
                }
                else
                {
                    Debug.WriteLine("Status: " + compilationResult.Status);
                }
            }
        }

        /// <summary>
        /// Método que a partir do resultado interpretado pelo app, toma as ações devidas
        /// </summary>
        /// <param name="session"></param>
        /// <param name="args"></param>
        private void RecognizerResultGenerated(SpeechContinuousRecognitionSession session, SpeechContinuousRecognitionResultGeneratedEventArgs args)
        {
            int count = args.Result.SemanticInterpretation.Properties.Count;
            String cmd = args.Result.SemanticInterpretation.Properties.ContainsKey(TAG_CMD) ? args.Result.SemanticInterpretation.Properties[TAG_CMD][0].ToString() : "";
            String device = args.Result.SemanticInterpretation.Properties.ContainsKey(TAG_DEVICE) ? args.Result.SemanticInterpretation.Properties[TAG_DEVICE][0].ToString() : "";

            switch (cmd)
            {
                case CMD_ANDAR:
                case CMD_ANDA:
                    {
                        switch (device)
                        {
                            case DEVICE_FRENTE:
                                Move(DEVICE_FRENTE, 1);
                                break;

                            case DEVICE_TRAS:
                                Move(DEVICE_TRAS, 1);
                                break;
                        }
                    }
                    break;

                case CMD_VIRAR:
                case CMD_VIRA:
                    {
                        switch (device)
                        {
                            case DEVICE_DIREITA:
                                Move(DEVICE_DIREITA, 1);
                                break;

                            case DEVICE_ESQEUERDA:
                                Move(DEVICE_ESQEUERDA, 1);
                                break;
                        }
                    }
                    break;

                case CMD_PARAR:
                    break;

            }


        }

        /// <summary>
        /// Método para verificar se o app possui a mermissão para escutar o microfone
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> CheckMicrophonePermission()
        {
            try
            {
                MediaCaptureInitializationSettings settings = new MediaCaptureInitializationSettings();
                settings.StreamingCaptureMode = StreamingCaptureMode.Audio;
                settings.MediaCategory = MediaCategory.Speech;
                MediaCapture capture = new MediaCapture();

                await capture.InitializeAsync(settings);
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Método para mover a cadeira
        /// </summary>
        /// <param name="direction"> Direção que a cadeira será movida</param>
        /// <param name="speed"> Velocidade em que a cadeira será movida</param>
        private void Move(string direction, decimal speed)
        {
            Thickness margin = imgCadeira.Margin;            
            

            switch (direction)
            {
                case DEVICE_FRENTE:
                    margin.Left += 10;
                    break;

                case DEVICE_TRAS:
                    margin.Right += 10;
                    break;

                case DEVICE_DIREITA:
                    margin.Top += 10;
                    break;

                case DEVICE_ESQEUERDA:
                    margin.Top -= 10;
                    break;
            }

            imgCadeira.Margin = margin;
        }

        private void btnFrente_Click(object sender, RoutedEventArgs e)
        {
            Move(DEVICE_FRENTE, 1);
        }

        private void btnTras_Click(object sender, RoutedEventArgs e)
        {
            Move(DEVICE_TRAS, 1);
        }

        private void btnEsquerda_Click(object sender, RoutedEventArgs e)
        {
            Move(DEVICE_ESQEUERDA, 1);
        }

        private void btnDireita_Click(object sender, RoutedEventArgs e)
        {
            Move(DEVICE_DIREITA, 1);
        }
    }

}