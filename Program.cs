namespace Speech
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using Microsoft.Kinect;
    using Microsoft.Speech.AudioFormat;
    using Microsoft.Speech.Recognition;

    public class Program
    {
        public static void Main(string[] args)
        {
            // Obtain a KinectSensor if any are available
            KinectSensor sensor = (from sensorToCheck in KinectSensor.KinectSensors where sensorToCheck.Status == KinectStatus.Connected select sensorToCheck).FirstOrDefault();
            if (sensor == null)
            {
                Console.WriteLine(
                        "No Kinect sensors are attached to this computer or none of the ones that are\n" +
                        "attached are \"Connected\".\n" +
                        //"Attach the KinectSensor and restart this application.\n" +
                        //"If that doesn't work run SkeletonViewer-WPF to better understand the Status of\n" +
                        //"the Kinect sensors.\n\n" +
                        "Press any key to continue.\n");

                // Give a chance for user to see console output before it is dismissed
                Console.ReadKey(true);
                return;
            }

            sensor.Start();

            // Obtain the KinectAudioSource to do audio capture
            KinectAudioSource source = sensor.AudioSource;
            source.EchoCancellationMode = EchoCancellationMode.None; // No AEC for this sample
            source.AutomaticGainControlEnabled = false; // Important to turn this off for speech recognition

            //RecognizerInfo ri = GetKinectRecognizer();

            if (GetKinectRecognizer() == null)
            {
                Console.WriteLine("Could not find Kinect speech recognizer! You should probably install the Audio SDK for Kinect (released by Microsoft"); //Put a download link here to get the audio sdk from microsoft for kinect
                return;
            }

            Console.WriteLine("Using: {0}", GetKinectRecognizer().Name); //TAKE A LOOK AT PRINTED CODE

            // NOTE: Need to wait 4 seconds for device to be ready right after initialization
            int wait = 4;
            while (wait > 0)
            {
                Console.Write("Device will be ready for speech recognition in {0} second(s).\r", wait--);
                Thread.Sleep(1000);
            }
            
            using (var sre = new SpeechRecognitionEngine(GetKinectRecognizer().Id))
            {                
                var colors = new Choices();
                colors.Add("red");
                colors.Add("green");
                colors.Add("blue");
                colors.Add("purple");
                colors.Add("orange");

                colors.Add("Hi");
                colors.Add("Nope");
                colors.Add("wake up");
                colors.Add("egg salad");
                colors.Add("derp");

                var gb = new GrammarBuilder { Culture = GetKinectRecognizer().Culture };

                // Specify the culture to match the recognizer in case we are running in a different culture.                                 
                gb.Append(colors);
                                    
                // Create the actual Grammar instance, and then load it into the speech recognizer.
                var g = new Grammar(gb);                    

                sre.LoadGrammar(g);
                sre.SpeechRecognized += SreSpeechRecognized;
                sre.SpeechHypothesized += SreSpeechHypothesized;
                sre.SpeechRecognitionRejected += SreSpeechRecognitionRejected;

                using (Stream s = source.Start())
                {
                    sre.SetInputToAudioStream(
                        s, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));

                    Console.WriteLine("Recognizing speech. Say: 'red', 'green', purple, orange or 'blue'. Press ENTER to quit");

                    sre.RecognizeAsync(RecognizeMode.Multiple);
                    Console.ReadLine();
                    Console.WriteLine("Stopping recognizer...");
                    sre.RecognizeAsyncStop();                       
                }
            }

            sensor.Stop();
        }

        private static RecognizerInfo GetKinectRecognizer()
        {
            Func<RecognizerInfo, bool> matchingFunc = r =>
            {
                string value;
                r.AdditionalInfo.TryGetValue("Kinect", out value);
                return "True".Equals(value, StringComparison.InvariantCultureIgnoreCase) && "en-US".Equals(r.Culture.Name, StringComparison.InvariantCultureIgnoreCase);
            };
            return SpeechRecognitionEngine.InstalledRecognizers().Where(matchingFunc).FirstOrDefault();
        }

        private static void SreSpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            Console.WriteLine("\nSpeech not recognized");
            if (e.Result != null)
            {
                DumpRecordedAudio(e.Result.Audio);
            }
        }

        private static void SreSpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            Console.Write("\rSpeech Hypothesized: \t{0}", e.Result.Text);
        }

        private static void SreSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence >= 0.7)
            {
                Console.WriteLine("\nSpeech Recognized: \t{0}\tConfidence:\t{1}", e.Result.Text, e.Result.Confidence);
            }
            else
            {
                Console.WriteLine("\nSpeech Recognized but confidence was too low: \t{0}", e.Result.Confidence);
                DumpRecordedAudio(e.Result.Audio); //deletes extra audio after being analyzed
            }
        }

        private static void DumpRecordedAudio(RecognizedAudio audio)
        {
            if (audio == null)
            {
                return;
            }

            int fileId = 0;
            string filename;
            while (File.Exists((filename = "RetainedAudio_" + fileId + ".wav")))
            {
                fileId++;
            }

            Console.WriteLine("\nWriting file: {0}", filename);
            using (var file = new FileStream(filename, System.IO.FileMode.CreateNew))
            {
                audio.WriteToWaveStream(file);
            }
        }
    }
}
