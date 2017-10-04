//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Windows.ApplicationModel;
//using Windows.Media.Capture;
//using Windows.Media.SpeechRecognition;
//using Windows.Storage;
//using Windows.UI.Xaml;
//using Windows.UI.Xaml.Controls;

//namespace VoiceRecognitionMovement
//{
//    public class Recognizer
//    {
        
        

//        public Recognizer(Page mPage)
//        {
//            this.mPage = mPage;

//            var t = mPage.FindName("pagePrincipal");
//            Page r = (Page)t;
//            r.
//        }

//        public async void InicializaVoz()
//        {
//            bool hasPermission = await CheckMicrophonePermission();

//            if (hasPermission)
//            {
//                var idioma = new Windows.Globalization.Language("en-US");
//                recognizer = new SpeechRecognizer(idioma);
//                //recognizer.StateChanged += RecognizerStateChanged;
//                recognizer.ContinuousRecognitionSession.ResultGenerated += RecognizerResultGenerated;
//                string fileName = String.Format(SRGS_FILE);
//                StorageFile grammarContentFile = await Package.Current.InstalledLocation.GetFileAsync(fileName);
//                SpeechRecognitionGrammarFileConstraint grammarConstraint = new SpeechRecognitionGrammarFileConstraint(grammarContentFile);
//                recognizer.Constraints.Add(grammarConstraint);
//                SpeechRecognitionCompilationResult compilationResult = await recognizer.CompileConstraintsAsync();

//                if (compilationResult.Status == SpeechRecognitionResultStatus.Success)
//                {
//                    await recognizer.ContinuousRecognitionSession.StartAsync();
//                }
//                else
//                {
//                    Debug.WriteLine("Status: " + compilationResult.Status);
//                } 
//            }
//        }

//        private void RecognizerResultGenerated(SpeechContinuousRecognitionSession session, SpeechContinuousRecognitionResultGeneratedEventArgs args)
//        {
//            int count = args.Result.SemanticInterpretation.Properties.Count;
//            String cmd = args.Result.SemanticInterpretation.Properties.ContainsKey(TAG_CMD) ? args.Result.SemanticInterpretation.Properties[TAG_CMD][0].ToString() : "";
//            String device = args.Result.SemanticInterpretation.Properties.ContainsKey(TAG_DEVICE) ? args.Result.SemanticInterpretation.Properties[TAG_DEVICE][0].ToString() : "";
            
//            switch(cmd)
//            {
//                case CMD_ANDAR:
//                    {
//                        switch(device)
//                        {
//                            case DEVICE_FRENTE:

//                                break;

//                            case DEVICE_TRAS:
//                                break;
//                        }
//                    }
//                    break;

//                case CMD_VIRAR:
//                    {
//                        switch(device)
//                        {
//                            case DEVICE_DIREITA:
//                                break;

//                            case DEVICE_ESQEUERDA:
//                                break;
//                        }
//                    }
//                    break;

//                case CMD_PARAR:
//                    break;

//            }
//            //if(cmd.Equals(CMD_ANDAR))
//            //{
//            //    if (device.Equals(DEVICE_FRENTE))
//            //    {

//            //    }
//            //    else if(device.Equals(DEVICE_TRAS))
//            //    {

//            //    }                
//            //}
//            //else if(cmd.Equals())

//            //if (device.Equals(DEVICE_DIREITA))
//            //{

//            //}
//            //else if (device.Equals(DEVICE_ESQEUERDA))
//            //{

//            //}
//            //bool isOn = cmd.Equals(STATE_ON);
//            //if (device.Equals(DEVICE_LUZ))
//            //{
//            //    //EscreveGPIOPin(pinoLuz, isOn ? GpioPinValue.High : GpioPinValue.Low);
//            //    if (isOn)
//            //    {
//            //      //  mudaImagem(imgLuz, "luzOn");
//            //    }
//            //    else
//            //    {
//            //        //mudaImagem(imgLuz, "luzOff");
//            //    }
//            //}
//            //else if (device.Equals(DEVICE_VENT))
//            //{
//            //    //EscreveGPIOPin(pinoVent, isOn ? GpioPinValue.High : GpioPinValue.Low) 
//            //    if (isOn)
//            //    {
//            //      //  mudaImagem(imgVent, "venton");
//            //    }
//            //    else
//            //    {
//            //        //mudaImagem(imgVent, "ventoff");
//            //    }
//            //}
//            //else if (device.Equals(DEVICE_TUDO))
//            //{
//            //    //EscreveGPIOPin(pinoVent, isOn ? GpioPinValue.High : GpioPinValue.Low);
//            //    //EscreveGPIOPin(pinoLuz, isOn ? GpioPinValue.High : GpioPinValue.Low);
//            //    if (isOn)
//            //    {
//            //        //mudaImagem(imgLuz, "luzOn");
//            //        //mudaImagem(imgVent, "venton");
//            //    }
//            //    else
//            //    {
//            //        //mudaImagem(imgLuz, "luzOff");
//            //        //mudaImagem(imgVent, "ventoff");
//            //    }
//            //}
//            //else if (device.Equals(DEVICE_ENFERMEIRA))
//            //{
//            //    if (cmd.Equals(DEVICE_CHAMAR))
//            //    {
//            //        //mudaEnfermeira();
//            //    }
//            //}
//            //else
//            //{
//            //    Debug.WriteLine("Dispositivo Desconhecido");
//            //}
//        }

//        public static async Task<bool> CheckMicrophonePermission()
//        {
//            try
//            {
//                MediaCaptureInitializationSettings settings = new MediaCaptureInitializationSettings();
//                settings.StreamingCaptureMode = StreamingCaptureMode.Audio;
//                settings.MediaCategory = MediaCategory.Speech;
//                MediaCapture capture = new MediaCapture();

//                await capture.InitializeAsync(settings);
//            }
//            catch(UnauthorizedAccessException)
//            {
//                return false;
//            }

//            return true;
//        }

//        private void Move(string direction, int speed)
//        {

//        }
//    }


//}
