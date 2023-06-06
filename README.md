# Coroutine Tracker

Coroutine Tracker is a tool that detects and displays coroutines running in the Unity editor in real time.

## How to use

1. Download this project and open the Unity editor to load the project.
2. Select "Window" from the editor top menu and click "Coroutine Viewer".
3. When the Coroutine Viewer window opens, you can see a list of running coroutines.
4. Selecting a coroutine opens the corresponding script file and moves to the line where the coroutine is located.

## How it Works

Coroutine Tracker is implemented using the CoroutineTrackerWindow class, which inherits from EditorWindow. Here's a description of what the main methods do:

- `OnEnable()`: This method is executed when the editor window is activated. Register the `UpdateRunningCoroutines` method on the `EditorApplication.update` event to update running coroutines.
- `OnDisable()`: This method is executed when the editor window is deactivated. Remove the `UpdateRunningCoroutines` method from the `EditorApplication.update` event.
- `OnGUI()`: This method handles the GUI running in the editor window. Displays a list of running coroutines in a scroll view, and selecting each coroutine opens the corresponding script and jumps to the line where the coroutine is located.
- `UpdateRunningCoroutines()`: This method updates the running coroutines to update the `runningCoroutines` list. It checks all MonoBehaviours that exist in the current Scene, and if you use the StartCoroutine method among them, the corresponding coroutine information is saved.
- `FindCoroutineName(string line)`: This method finds the coroutine name in a given code line. The coroutine name is the function name passed as an argument to the StartCorotine method.
- `GetScriptFilePath(MonoBehaviour monoBehaviour)`: This method gets the script file path of the given MonoBehaviour. Find the script file path using the MonoScript instance attached to that MonoBehaviour.

The above description is a brief introduction to how to use Coroutine Tracker and how it works. If you need more details, you can refer to the comments in the code and the corresponding script file.

### contribute

This project is developed as open source, and contributions are welcome. If you find a bug or have an improvement idea, please file an issue or leave a pull request.
