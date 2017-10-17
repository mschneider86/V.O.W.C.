using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Media.Capture;
using Windows.Media.SpeechRecognition;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;


namespace VoiceRecognitionMovement
{
    public sealed partial class MainPage : Page
    {
        private const string SRGS_FILE = "Gramatica\\gramatica.xml";
        private const string TAG_CMD = "cmd";
        private const string TAG_DEVICE = "device";

        private const string CMD_PARAR = "PARAR";
        private const string CMD_PARA = "PARA";
        private const string CMD_ANDAR = "ANDAR";
        private const string CMD_ANDA = "ANDA";
        private const string CMD_VIRAR = "VIRAR";
        private const string CMD_VIRA = "VIRA";
        private const string DEVICE_FRENTE = "FRENTE";
        private const string DEVICE_TRAS = "TRAS";
        private const string DEVICE_ESQEUERDA = "ESQUERDA";
        private const string DEVICE_DIREITA = "DIREITA";

        private const string PATH_RIGHT_ARROW_GREEN = "Assets/arrow-right-green.png";
        private const string PATH_RIGHT_ARROW_RED = "Assets/arrow-right-red.png";
        private const string PATH_LEFT_ARROW_GREEN = "Assets/arrow-left-green.png";
        private const string PATH_LEFT_ARROW_RED = "Assets/arrow-left-red.png";
        private const string PATH_UP_ARROW_GREEN = "Assets/arrow-up-green.png";
        private const string PATH_UP_ARROW_RED = "Assets/arrow-up-red.png";
        private const string PATH_DOWN_ARROW_GREEN = "Assets/arrow-down-green.png";
        private const string PATH_DOWN_ARROW_RED = "Assets/arrow-down-red.png";


        private SpeechRecognizer recognizer;

        Thickness margin;

        public MainPage()
        {
            this.InitializeComponent();

            var appView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
            appView.Title = "V.O.W.C.";

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
                    Debug.WriteLine("reconhecendo");
                }
                else
                {
                    Debug.WriteLine("Status: " + compilationResult.Status);
                }
            }
            else
            {
                Debug.WriteLine("Sem permissão");
            }
        }

        /// <summary>
        /// Método que a partir do resultado interpretado pelo app, toma as ações devidas
        /// </summary>
        /// <param name="session"></param>
        /// <param name="args"></param>
        private async void RecognizerResultGenerated(SpeechContinuousRecognitionSession session, SpeechContinuousRecognitionResultGeneratedEventArgs args)
        {
            int count = args.Result.SemanticInterpretation.Properties.Count;
            String cmd = args.Result.SemanticInterpretation.Properties.ContainsKey(TAG_CMD) ? args.Result.SemanticInterpretation.Properties[TAG_CMD][0].ToString() : "";
            String device = args.Result.SemanticInterpretation.Properties.ContainsKey(TAG_DEVICE) ? args.Result.SemanticInterpretation.Properties[TAG_DEVICE][0].ToString() : "";


            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                Thickness margin = imgCadeira.Margin;
                if (cmd != string.Empty && device != string.Empty)
                    txbCommandRecognized.Text = cmd + " " + device;
                else
                    txbCommandRecognized.Text = "Comando não reconhecido!";
            });

            switch (cmd)
            {
                case CMD_ANDAR:
                case CMD_ANDA:
                    {
                        switch (device)
                        {
                            case DEVICE_FRENTE:
                                Move(DEVICE_FRENTE);
                                break;

                            case DEVICE_TRAS:
                                Move(DEVICE_TRAS);
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
                                Move(DEVICE_DIREITA);
                                break;

                            case DEVICE_ESQEUERDA:
                                Move(DEVICE_ESQEUERDA);
                                break;
                        }
                    }
                    break;


                default:
                    txbCommandRecognized.Text = "Comando não reconhecido!";
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
        private async void Move(string direction)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {                
                Thickness margin = imgCadeira.Margin;

                
                switch (direction)
                {
                    case DEVICE_FRENTE:
                        margin.Left += 10;
                        ChangeArrowColor(DEVICE_FRENTE);                        
                        break;

                    case DEVICE_TRAS:
                        margin.Right += 10;
                        ChangeArrowColor(DEVICE_TRAS);
                        break;

                    case DEVICE_DIREITA:
                        margin.Top += 10;
                        ChangeArrowColor(DEVICE_DIREITA);
                        break;

                    case DEVICE_ESQEUERDA:
                        margin.Top -= 10;
                        ChangeArrowColor(DEVICE_ESQEUERDA);
                        break;
                }

                txbDirectionMoved.Text = direction;
                imgCadeira.Margin = margin;
            }
            );
            
        }

        private void btnFrente_Click(object sender, RoutedEventArgs e)
        {            
            Move(DEVICE_FRENTE);
            txbCommandRecognized.Text = "TOQUE SETA DIREITA";
        }

        private void btnTras_Click(object sender, RoutedEventArgs e)
        {
            Move(DEVICE_TRAS);
            txbCommandRecognized.Text = "TOQUE SETA ESQUERDA";
        }

        private void btnEsquerda_Click(object sender, RoutedEventArgs e)
        {
            Move(DEVICE_ESQEUERDA);
            txbCommandRecognized.Text = "TOQUE SETA CIMA";
        }

        private void btnDireita_Click(object sender, RoutedEventArgs e)
        {
            Move(DEVICE_DIREITA);
            txbCommandRecognized.Text = "TOQUE SETA BAIXO";
        }

        /// <summary>
        /// Método para alterar a cor das setas de acordo com a direção informada
        /// </summary>
        /// <param name="direction">Direção em que a cadeira deve se mover</param>
        private void ChangeArrowColor(string direction)
        {            
            List<UIElement> elementsRight = FindChildren(btnFrente);
            Image imageRight = ((Image)elementsRight[0]);            

            List<UIElement> elementsLeft = FindChildren(btnTras);
            Image imageLeft = ((Image)elementsLeft[0]);            

            List<UIElement> elementsUp = FindChildren(btnEsquerda);
            Image imageUp = ((Image)elementsUp[0]);            

            List<UIElement> elementsDown = FindChildren(btnDireita);
            Image imageDown = ((Image)elementsDown[0]);            

            //Verifica qual a direção foi informada e muda a seta daquela diração para VERDE e as demais para VERMELHO
            switch (direction)
            {
                case DEVICE_FRENTE:
                    imageRight.Source = new BitmapImage(new Uri(this.BaseUri, PATH_RIGHT_ARROW_GREEN));
                    imageLeft.Source = new BitmapImage(new Uri(this.BaseUri, PATH_LEFT_ARROW_RED));
                    imageUp.Source = new BitmapImage(new Uri(this.BaseUri, PATH_UP_ARROW_RED));
                    imageDown.Source = new BitmapImage(new Uri(this.BaseUri, PATH_DOWN_ARROW_RED));
                    break;

                case DEVICE_TRAS:
                    imageRight.Source = new BitmapImage(new Uri(this.BaseUri, PATH_RIGHT_ARROW_RED));
                    imageLeft.Source = new BitmapImage(new Uri(this.BaseUri, PATH_LEFT_ARROW_GREEN));
                    imageUp.Source = new BitmapImage(new Uri(this.BaseUri, PATH_UP_ARROW_RED));
                    imageDown.Source = new BitmapImage(new Uri(this.BaseUri, PATH_DOWN_ARROW_RED));
                    break;

                case DEVICE_DIREITA:
                    imageRight.Source = new BitmapImage(new Uri(this.BaseUri, PATH_RIGHT_ARROW_RED));
                    imageLeft.Source = new BitmapImage(new Uri(this.BaseUri, PATH_LEFT_ARROW_RED));
                    imageUp.Source = new BitmapImage(new Uri(this.BaseUri, PATH_UP_ARROW_RED));
                    imageDown.Source = new BitmapImage(new Uri(this.BaseUri, PATH_DOWN_ARROW_GREEN));
                    break;

                case DEVICE_ESQEUERDA:
                    imageRight.Source = new BitmapImage(new Uri(this.BaseUri, PATH_RIGHT_ARROW_RED));
                    imageLeft.Source = new BitmapImage(new Uri(this.BaseUri, PATH_LEFT_ARROW_RED));
                    imageUp.Source = new BitmapImage(new Uri(this.BaseUri, PATH_UP_ARROW_GREEN));
                    imageDown.Source = new BitmapImage(new Uri(this.BaseUri, PATH_DOWN_ARROW_RED));
                    break;
                //caso a direção informada não corresponda a nenhuma das padrões, muda todas as setas de volta para VERMELHO
                default:
                    imageRight.Source = new BitmapImage(new Uri(this.BaseUri, PATH_RIGHT_ARROW_RED));
                    imageLeft.Source = new BitmapImage(new Uri(this.BaseUri, PATH_LEFT_ARROW_RED));
                    imageUp.Source = new BitmapImage(new Uri(this.BaseUri, PATH_UP_ARROW_RED));
                    imageDown.Source = new BitmapImage(new Uri(this.BaseUri, PATH_DOWN_ARROW_RED));
                    break;
            }            

        }

        /// <summary>
        /// Método para percorrer um objeto e retornar todos os seus filhos
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public List<UIElement> FindChildren(DependencyObject parent)
        {
            var _List = new List<UIElement>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var _Child = VisualTreeHelper.GetChild(parent, i);
                if (_Child is UIElement)
                    _List.Add(_Child as UIElement);
                _List.AddRange(FindChildren(_Child));
            }
            return _List;
        }


    }

}