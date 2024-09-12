# Speech to Text on Local Container
This repository contains the code to run a Azure Speech to Text service on a local container.
The code is based on the official [Azure Speech SDK](https://learn.microsoft.com/ja-jp/azure/ai-services/speech-service/speech-sdk)

## Requirements
- Docker
- Visual Studio 2022

## How to run
1. Run Azure Speech to Text service on a local container with the following command

```bash
docker run --rm -it -p 5000:5000 --memory 8g --cpus 4 mcr.microsoft.com/azure-cognitive-services/speechservices/speech-to-text:4.9.0-amd64-ja-jp Eula=accept Billing=https://<SpeechServiceName>.cognitiveservices.azure.com/ ApiKey=<APIKEY>
```

1. Clone this repository
1. Open the solution file `Speech2TextOnLocalContainer.sln` with Visual Studio 2022
1. Build the solution
1. Run the project `Speech2TextOnLocalContainer`
1. enjoy!
