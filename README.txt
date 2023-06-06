# Coroutine Tracker

Coroutine Tracker는 Unity 에디터에서 실행 중인 코루틴을 실시간으로 감지하고 표시해주는 도구입니다.

## 사용법

1. 이 프로젝트를 다운로드하고 Unity 에디터를 열어 프로젝트를 로드합니다.
2. 에디터 상단 메뉴에서 "Window"를 선택하고 "Coroutine Viewer"를 클릭합니다.
3. Coroutine Viewer 창이 열리면 실행 중인 코루틴 목록을 확인할 수 있습니다.
4. 코루틴을 선택하면 해당 스크립트 파일이 열리고 코루틴이 위치한 라인으로 이동합니다.

## 작동 원리

Coroutine Tracker는 EditorWindow를 상속받은 CoroutineTrackerWindow 클래스를 사용하여 구현되었습니다. 다음은 주요 메서드들의 기능에 대한 설명입니다:

- `OnEnable()`: 에디터 윈도우가 활성화될 때 실행되는 메서드입니다. 실행 중인 코루틴을 업데이트하기 위해 `EditorApplication.update` 이벤트에 `UpdateRunningCoroutines` 메서드를 등록합니다.
- `OnDisable()`: 에디터 윈도우가 비활성화될 때 실행되는 메서드입니다. `EditorApplication.update` 이벤트에서 `UpdateRunningCoroutines` 메서드를 제거합니다.
- `OnGUI()`: 에디터 윈도우에서 실행되는 GUI를 처리하는 메서드입니다. 실행 중인 코루틴 목록을 스크롤 뷰로 표시하고, 각 코루틴을 선택하면 해당 스크립트를 열고 코루틴이 위치한 라인으로 이동합니다.
- `UpdateRunningCoroutines()`: 실행 중인 코루틴을 업데이트하여 `runningCoroutines` 리스트를 갱신하는 메서드입니다. 현재 Scene에 존재하는 모든 MonoBehaviour를 확인하고, 그 중에서 StartCoroutine 메서드를 사용하는 경우 해당 코루틴의 정보를 저장합니다.
- `FindCoroutineName(string line)`: 주어진 코드 라인에서 코루틴 이름을 찾는 메서드입니다. 코루틴 이름은 StartCorotine 메서드의 인자로 전달된 함수 이름입니다.
- `GetScriptFilePath(MonoBehaviour monoBehaviour)`: 주어진 MonoBehaviour의 스크립트 파일 경로를 가져오는 메서드입니다. 해당 MonoBehaviour에 연결된 MonoScript 인스턴스를 사용하여 스크립트 파일 경로를 찾습니다.

위의 설명은 Coroutine Tracker의 사용법과 작동 원리를 간단히 소개한 것입니다. 더 자세한 내용이 필요하다면 코드의 주석과 해당 스크립트 파일을 참고하실 수 있습니다.

### 기여

이 프로젝트는 오픈 소스로 개발되었으며, 기여는 언제든 환영합니다. 버그를 발견하거나 개선 아이디어가 있다면 이슈를 등록하거나 풀 리퀘스트를 남겨주세요.