# SC-KRM
Simsimhan Chobo Kernel Manager

## 주의
이 프로젝트는 처음 프로젝트를 만들때 사용해야 나중에 안 귀찮아집니다.

Newtonsoft.Json이 내장 되어있습니다.

만약 중복되었다는 오류가 발생한다면 Vesion Control 패키지를 지우거나 SC KRM/Json/Newtonsoft.Json.dll을 지워주세요.

## 기능
- Minecraft에 있는 리소스팩 기능이 포함되어있습니다 (리소스팩을 바꿀수 있는 GUI 포함됨)
즉 유니티에 있는 기본 리소스 관리 시스템이 사용되지 않습니다
사용 할 수 있기는 하지만, 웬만하면 추천하지 않습니다

- 조작키를 쉽게 변경 할 수 있고, GUI가 마련되어있습니다 Kernel 프리팹에 Input Manager 오브젝트의 스크립트를 보시면 사용 방법을 웬만하면 쉽게 알 수 있습니다.

- 언어 기능이 있습니다 (언어를 바꿀수 있는 GUI 포함)

- Kernel 스크립트에 확장 함수들과 최적화를 위한 여러 변수들이 있습니다.
예를들어 Time.deltaTime 같은경우 값을 얻을때마다 함수가 호출되서 여러번 사용하면 상당한 개적화가 되지만
Kernel.deltaTime을 사용 할 경우, 일반 변수처럼 비슷하게 작동하게 됩니다.
직접 for로 여러번 실행시켜보시면 알게될겁니다.

- 커스텀 렌더러들이 있습니다
  기본적으로 유니티 내장 렌더러를 사용하게 되지만, 리소스팩과 호환되게 만들어주는 렌더러입니다.

  정확하겐 커스텀 렌더러 스크립트는 기존에 있던 렌더러의 스프라이트 같은거를 바꿔주는거입니다.

  단, 스트리밍 에셋에 파일이 있어야 하며

  이미지 같은 UI 컴포넌트들은 `assets/%NameSpace%/textures/gui`에 위치해있어야합니다.

  스프라이트 렌더러 같은것들은 `assets/%NameSpace%/textures`에 위치해있어야합니다.

  하지만 커스텀 경로가 켜져있다면 모든 렌더러가 전부 `assets/%NameSpace%` 경로까지 파일이 위치해 있을 수 있습니다.

  텍스트 렌더러는 예외로, 언어 파일에서 가져오는것이기 때문에 경로를 바꿀순 없습니다.
  
- 세이브 로드 기능이 있습니다, 자동화가 아니기 때문에 SaveLoadManager 스크립트에서 직접 수정해줘야하지만 만들어진것들이 있기때문에 의미만 파악하면 쉽게 할수 있습니다.
- 오브젝트 풀링 시스템이 만들어져 있습니다. Kernel 프리팹에 Object Polling System 오브젝트의 스크립트를 보시면 사용 방법을 웬만하면 쉽게 알 수 있습니다.
- 마인크래프트의 PlaySound랑 비슷한 기능이 만들어져있습니다 관련 함수는 `SoundManager.PlayBGM() SoundManager.PlaySound() SoundManager.StopBGM() SoundManager.StopSound() SoundManager.StopAll()` 입니다
- 윈도우에서는 무려 창 위치와 크기를 마음대로 지지고 볶을 수 있습니다! 리듬게임에 들어간다면 금상첨화죠.
- 등등...

## 사용 방법
- 스크립트에서 수동으로 리소스팩에 있는 에셋을 가져올려면 `ResourcesManager.Search<T>()` 함수를 사용하면 됩니다. (네임스페이스는 경로에 'namespace:path' 이렇게 하시면 됩니다.)
- 조작키 설정은 Kernel 오브젝트에 Input Manager 오브젝트로 가서 수정하거나, 스트리밍 에셋에 있는 projectSettings 폴더에가서 inputManager.controlSettings.json을 수정하면 됩니다.
- 오브젝트 풀링에서 가져올 오브젝트를 설정하는건 Kernel 오브젝트에 있는 Object Polling System 오브젝트로 가서 수정하거나, 스트리밍 에셋에 있는 projectSettings 폴더에 가서 objectPollingSystem.json을 수정하면 됩니다.
- 오브젝트 풀링 관련 함수는 `ObjectPollingSystem.ObjectCreate() ObjectPollingSystem.ObjectRemove()` 입니다.
- 스크립트에서 언어 파일을 수동으로 불러올려면 `LanguageManager.LanguageLoad()` 함수를 사용하면 됩니다.
- 윈도우에서 창 위치와 크기를 마음대로 지지고 볶을려면 `WindowManager.public static void SetWindowRect(Rect rect, Vector2 windowDatumPoint, Vector2 screenDatumPoint, bool clientDatum = true, bool Lerp = false, float time = 0.05f)` 함수를 사용하시면 됩니다.
  
  들어가기에 앞서, 윈도우는 유니티랑 y좌표가 반대입니다. 즉 맨 위가 y의 0이 됩니다.
  
  rect의 x와 y는 창의 위치이고 width랑 height는 창의 크기를 담당합니다.
  
  windowDatumPoint는 창의 중심점을 설정합니다, 즉 x1 y1으로 설정하면 창의 오른쪽 아래가 중심점이 됩니다.
  
  screenDatumPoint는 화면의 중심점을 설정합니다, 즉 x1 y1으로 설정하면 화면의 오른쪽 아래가 중심점이 됩니다.
  
  clientDatum은 켜져있으면 크기를 설정할때 윈도우의 보더를 포함하지 않고 크기를 설정하지만 (`Screen.SetResolution`랑 동일) 꺼져있으면 윈도우의 보더를 포함해서 크기를 설정합니다.
  
  나머지는 아실거라고 믿습니다.
