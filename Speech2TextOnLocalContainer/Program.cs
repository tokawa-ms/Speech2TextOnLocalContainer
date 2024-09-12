using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

class Program
{
    async static Task Main(string[] args)
    {
        // 自前のコンテナで稼働する Speech Service に接続する場合は、SpeechConfig.FromHost を使用
        // 特に API Key とかは設定しなくても OK（コンテナ側に設定されている）
        var speechConfig = SpeechConfig.FromHost(new Uri("ws://localhost:5000"));

        // 認識対象言語を指定（コンテナの言語設定と合わせる）
        speechConfig.SpeechRecognitionLanguage = "ja-JP";

        // マイクから音声を入力するための設定
        using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
        using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

        // ここでフレーズリストを追加することも可能
        var phraseList = PhraseListGrammar.FromRecognizer(speechRecognizer);
        phraseList.AddPhrase("大川");

        // 音声認識の終了を待機するための TaskCompletionSource
        var stopRecognition = new TaskCompletionSource<int>();

        // イベントハンドラを設定
        speechRecognizer.Recognizing += (s, e) =>
        {
            Console.WriteLine($"RECOGNIZING: Text={e.Result.Text}");
        };

        speechRecognizer.Recognized += (s, e) =>
        {
            if (e.Result.Reason == ResultReason.RecognizedSpeech)
            {
                Console.WriteLine($"RECOGNIZED: Text={e.Result.Text}");
            }
            else if (e.Result.Reason == ResultReason.NoMatch)
            {
                Console.WriteLine($"NOMATCH: Speech could not be recognized.");
            }
        };

        speechRecognizer.Canceled += (s, e) =>
        {
            Console.WriteLine($"CANCELED: Reason={e.Reason}");

            if (e.Reason == CancellationReason.Error)
            {
                Console.WriteLine($"CANCELED: ErrorCode={e.ErrorCode}");
                Console.WriteLine($"CANCELED: ErrorDetails={e.ErrorDetails}");
                Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
            }

            stopRecognition.TrySetResult(0);
        };

        speechRecognizer.SessionStopped += (s, e) =>
        {
            Console.WriteLine("\n    Session stopped event.");
            stopRecognition.TrySetResult(0);
        };

        // 音声認識を開始
        Console.WriteLine("マイクに向かって話してください。");
        await speechRecognizer.StartContinuousRecognitionAsync();

        // 音声認識の終了を待機（このコードサンプルでは Ctrl+C で終了する以外に音声認識の終了にはならないはず。）
        Task.WaitAny(new[] { stopRecognition.Task });
    }
}