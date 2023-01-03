# KrTTSBot

Amazon Polly와 Lavalink를 이용한 Discord TTS 봇입니다.

## 사용 패키지(Packages)
- [Discord.Net](https://www.nuget.org/packages/Discord.Net) 3.7.2
- [Victoria](https://www.nuget.org/packages/Victoria) 5.2.8
- [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/7.0.0-preview.7.22375.6) 6.0.0
- [AWSSDK.Core](https://www.nuget.org/packages/AWSSDK.Core) 3.7.12.21
- [AWSSDK.Polly](https://www.nuget.org/packages/AWSSDK.Polly) 3.7.7.7
 
## 프레임워크(Framework)
`.NET 6.0` 사용 
- Visual Studio 2019는 .NET 6.0을 지원하지 않아 Visual Studio 2022 필요 

## Commands
Prefix, KrPrefix로 사용할 수 있는 명령어(Command)목록입니다.  
Prefix는 기본적인 명령어 사용을 위한 문자입니다. KrPrefix는 TTS 명령어를 한/영 변환없이 그리고 Prefix없이 사용하기 위한 Prefix입니다.  
둘 다 `Config.json` 에서 원하는 문자로 지정할 수 있습니다. 참고로 제가 지정한 Prefix는 `~`, KrPrefix는 `말ㄹ`입니다.  
    
`help`, `도움`: 봇 사용법과 명령어에 대한 설명을 보여줍니다.  
`tts`: TTS 명령어입니다.  
`말ㄹ(KrPrefix)`: 기본 Prefix 없이 사용가능한 TTS 명령어입니다. Config에서 KrPrefix를 변경해 원하는 문자로 지정할 수 있습니다.  
`join`,`드루와`: 음성채널 참가 명령어입니다. 굳이 사용하지 않고 tts 명령어를 사용하면 봇이 알아서 참가합니다.  
`leave`,`나가`: 음성채널 연결끊기 명령어입니다. 봇을 사용하지 않는 경우에 음성채널에서 나가게 하기 위한 명령어입니다.  
`stop`,`멈춰`: TTS 재생 정지 명령어입니다. 너무 길거나 듣기 싫은 TTS가 재생될 때 사용하기 위한 명령어입니다.  

추가적으로 5분간 사용하지 않으면 음성채널에서 자동으로 나가는 기능을 추가했습니다.
  

## Amazon Polly 사용법
Amazon Polly를 사용하려면 AccessKeyID와 SecretAccessKey가 필요합니다.  
이를 위해 Amazon Web Service 계정이 필요합니다. 계정이 없다면 [AWS Amazon](https://aws.amazon.com/ko/) 에서 계정을 만들 수 있습니다.  
이 후 IAM 계정과 그룹을 만들어야 합니다. 해당 부분은 [IAM 계정 관련 문서](https://docs.aws.amazon.com/ko_kr/IAM/latest/UserGuide/getting-started_create-admin-group.html)
를 참고하여 진행할 수 있습니다.  
AccessKeyID와 SecretAccessKey에 대한 부분은 [AccessKey 관련 문서](https://docs.aws.amazon.com/ko_kr/IAM/latest/UserGuide/id_credentials_access-keys.html)
를 참고하면 됩니다.

위 내용이 정리된 사이트(https://kaki104.tistory.com/591)

AWS CLI
https://awscli.amazonaws.com/AWSCLIV2.msi
credentials 파일 생성
https://docs.aws.amazon.com/ko_kr/cli/latest/userguide/cli-configure-files.html

## Lavalink 사용법
몇 가지 사전 준비를 거쳐 Lavalink를 통해 음성을 재생할 수 있습니다.  
우선 `lavalink.jar` 파일이 필요하고, jar파일을 실행하기 위해 `Java 13`버전 이상이 필요합니다.  
`lavalink.jar`파일은 [releases](https://github.com/freyacodes/Lavalink/releases) 에서 다운로드할 수 있습니다. 
 
Java 환경 변수 설정 
https://coding-factory.tistory.com/838

또 Lavalink server에 연결하기 위해 `application.yml`이 필요합니다. 
jar 파일과 마찬가지로 Lavalink github에서 [예시 파일](https://github.com/freyacodes/Lavalink/blob/master/LavalinkServer/application.yml.example)을 확인할 수 있습니다.  
이 봇은 tts mp3 파일을 다운받아 로컬 폴더에서 재생하므로 `sources:` 부분에서 `local: true`로 변경하여 사용합니다.  
이렇게 준비한 `application.yml`은 `lavalink.jar`파일과 같은 경로에 놓아주고 명령 프롬프트창에서 `java -jar lavalink.jar`를 입력해 사용할 수 있습니다.
[Lavalink](https://github.com/freyacodes/Lavalink) 와 [Dsharpplus](https://dsharpplus.github.io/articles/audio/lavalink/setup.html) 에서 자세한 내용을 확인할 수 있습니다. 

## Bot 실행방법
우선 프로젝트를 처음 실행하면 봇이 가동되지 않고 Resources 플더에 `Config.json` 파일이 생성됩니다.
```
{
  "token": null,
  "prefix": null,
  "krPrefix": null,
  "AWSAccessKeyId": null,
  "AWSSecretKey": null
}
```
token은 Discord Bot Token, prefix와 krPrefix 각각 사용하고자하는 prefix 문자와 한글 prefix를, 
AWSAccessKeyId와 AWSSecretKey는 Amazon Web Service에서 받은 Id와 Key를 입력합니다.  
이제 프로그램을 실행하면 봇이 실행됩니다. lavalink를 켜두지 않으면 음성관련 기능이 작동하지 않으니 봇 실행전에 반드시 켜줘야 합니다.  


## Update 할 내용
- 봇 서버에 올리기
- 봇 켜질 때 알림 기능
- 봇 변경사항 알림 기능
- 개발자 전용 명령어(봇 logout/login 등)

## Bot 설계, 작동 방식  
봇 제작의 토대가 된 영상: https://www.youtube.com/watch?v=K0UMmoyOqZI&t=1s&ab_channel=Koreanpanda345  
위 영상을 튜토리얼로 제작하였습니다.  

간략한 작동 방식 추가 예정
