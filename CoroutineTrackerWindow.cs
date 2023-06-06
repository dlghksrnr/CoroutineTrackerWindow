using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

public class CoroutineTrackerWindow : EditorWindow
{
    private Vector2 scrollPosition;
    private List<CoroutineInfo> runningCoroutines;

    private bool coroutinesFound = false;
    private bool refreshButtonClicked = false;
    private class CoroutineInfo
    {
        public string scriptName;
        public string methodName;
        public MonoBehaviour scriptInstance;
        public int lineNumber;
    }

    [MenuItem("Window/Coroutine Viewer")]
    public static void ShowWindow()
    {
        GetWindow<CoroutineTrackerWindow>("Coroutine Viewer");
    }

    private void OnEnable()
    {
        runningCoroutines = new List<CoroutineInfo>();
        EditorApplication.update += UpdateRunningCoroutines;

    }

    private void OnDisable()
    {
        EditorApplication.update -= UpdateRunningCoroutines;
    }
    /// <summary>
    /// ������ â���� ����Ǵ� GUI�� ó���ϴ� �޼����Դϴ�.
    /// ���� ���� �ڷ�ƾ ����� ��ũ�� ��� ǥ���ϰ�, �� �ڷ�ƾ�� �����ϸ� �ش� ��ũ��Ʈ�� ���� �ڷ�ƾ�� ��ġ�� �������� �̵��մϴ�.
    /// </summary>
    private void OnGUI()
    {
        GUILayout.Label("Running Coroutines", EditorStyles.boldLabel);

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        if (GUILayout.Button("Refresh"))
        {
            refreshButtonClicked = true;
            coroutinesFound = false;
        }

        foreach (CoroutineInfo coroutineInfo in runningCoroutines)
        {
            string coroutineName = $"{coroutineInfo.scriptName} - {coroutineInfo.methodName}";

            if (GUILayout.Button(coroutineName))
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = coroutineInfo.scriptInstance;

                string scriptPath = AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(coroutineInfo.scriptInstance));
                UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(scriptPath, coroutineInfo.lineNumber);
            }
        }

        GUILayout.EndScrollView();
    }
    /// <summary>
    /// ���� ���� �ڷ�ƾ�� ������Ʈ�Ͽ� runningCoroutines ����Ʈ�� �����մϴ�.
    /// </summary>
    private void UpdateRunningCoroutines()
    {
        if (!coroutinesFound || refreshButtonClicked)
        {
            runningCoroutines.Clear();

            MonoBehaviour[] monoBehaviours = FindObjectsOfType<MonoBehaviour>();

            foreach (MonoBehaviour monoBehaviour in monoBehaviours)
            {
                MethodInfo[] methods = monoBehaviour.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (MethodInfo method in methods)
                {
                    if (method.Name == "StartCoroutine")
                    {
                        ParameterInfo[] parameters = method.GetParameters();
                        if (parameters.Length == 1 && parameters[0].ParameterType == typeof(IEnumerator))
                        {
                            bool isObsolete = false;
                            object[] attributes = method.GetCustomAttributes(true);
                            foreach (object attribute in attributes)
                            {
                                if (attribute is System.ObsoleteAttribute)
                                {
                                    isObsolete = true;
                                    break;
                                }
                            }

                            if (!isObsolete)
                            {
                                string scriptName = monoBehaviour.GetType().Name;
                                string[] lines = File.ReadAllLines(GetScriptFilePath(monoBehaviour));
                                for (int i = 0; i < lines.Length; i++)
                                {
                                    if (lines[i].Contains(method.Name))
                                    {
                                        string coroutineName = FindCoroutineName(lines[i]);
                                        if (!string.IsNullOrEmpty(coroutineName))
                                        {
                                            CoroutineInfo coroutineInfo = new CoroutineInfo();
                                            coroutineInfo.scriptName = scriptName;
                                            coroutineInfo.methodName = coroutineName;
                                            coroutineInfo.scriptInstance = monoBehaviour;
                                            coroutineInfo.lineNumber = i + 1;
                                            runningCoroutines.Add(coroutineInfo);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            coroutinesFound = true;
            refreshButtonClicked = false;
        }
    }


    /// <summary>
    /// �־��� �ڵ� ���ο��� �ڷ�ƾ �̸��� ã���ϴ�.
    /// </summary>
    /// <param name="line">�м��� �ڵ� ����</param>
    /// <returns>ã�� �ڷ�ƾ �̸�</returns>
    private string FindCoroutineName(string line)
    {
        int startIndex = line.IndexOf("(");
        int endIndex = line.IndexOf(")");
        if (startIndex >= 0 && endIndex >= 0 && endIndex > startIndex)
        {
            string coroutineName = line.Substring(startIndex + 1, endIndex - startIndex).Trim();

            
            if (line.Contains("//")) //�ּ� ó���� �κ����� Ȯ��
            {
                //�ּ� ó���� �κ��� ó������ �ʰ� null ��ȯ
                return null;
            }

            return coroutineName; //�ڷ�ƾ �̸� ��ȯ
        }
        return null; //�̸� ��ã�´ٸ� null ��ȯ
    }


    /// <summary>
    /// �־��� MonoBehaviour�� ��ũ��Ʈ ���� ��θ� �����ɴϴ�.
    /// </summary>
    /// <param name="monoBehaviour">��� MonoBehaviour</param>
    /// <returns>��ũ��Ʈ ���� ���</returns>
    private string GetScriptFilePath(MonoBehaviour monoBehaviour)
    {
        MonoScript monoScript = MonoScript.FromMonoBehaviour(monoBehaviour); //MonoBehaviour�� ����� MonoScript �ν��Ͻ��� ��������
        string scriptFilePath = AssetDatabase.GetAssetPath(monoScript); //MonoScript�� ���� ��ũ��Ʈ ���� ��θ� ��������
        return scriptFilePath; //��ũ��Ʈ ���ϰ�� ��ȯ�ϱ�
    }
}