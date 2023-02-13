using System;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace OceanFSM.Editor
{
    internal class StateGenerator : EditorWindow
    {
        private const string MenuItemPath = "Tools/OceanFSM/State Generator";
        private const string WindowTitle = "State Generator";
        
        private const string ReferenceTypeKey = "ReferenceType";
        private const string NamespaceKey = "NameSpace";
        private const string IsSerializableKey = "IsSerializable";
        private const string StateNameKey = "StateName";
        private const string FolderPathKey = "FolderPath";
        private const string ShouldHighlightFile = "ShouldHighlightFile";
        
        private string _mReferenceType;
        private string _mNameSpace;
        private bool _mIsSerializable;
        private bool _mShouldHighlightFile;
        private string _mStateName;
        private string _mFolderPath;
        
        [MenuItem(MenuItemPath)]
        public static void ShowWindow()
        {
            var window = GetWindow<StateGenerator>();
            window.minSize = window.maxSize = new Vector2(400, 220);
            
            window.position = new Rect(Screen.currentResolution.width / 2f - window.minSize.x / 2f,
                Screen.currentResolution.height / 2f - window.minSize.y / 2f, window.minSize.x, window.minSize.y);
            
            window.titleContent = new GUIContent(WindowTitle)
            {
                image = EditorGUIUtility.IconContent("d_Project").image
            };
        }

        private void OnEnable()
        {
            GetPrefs();
        }
        
        private void OnDisable()
        {
            SetPrefs();
        }
        
        private void OnGUI()
        {
            GUILayout.BeginVertical("Box");
            GUILayout.Label("Settings", EditorStyles.boldLabel);
            GUILayout.Space(10f);
            
            var tooltip = new GUIContent("Highlight File?", "If enabled, the file will be highlighted in the project window after generation.");
            _mShouldHighlightFile = EditorGUILayout.Toggle(tooltip, _mShouldHighlightFile);
            
            _mIsSerializable = EditorGUILayout.Toggle("Is Serializable?", _mIsSerializable);
            _mReferenceType = EditorGUILayout.TextField("Generic Reference Type", _mReferenceType);
            _mNameSpace = EditorGUILayout.TextField("Namespace", _mNameSpace);
            _mStateName = EditorGUILayout.TextField("State Name", _mStateName);
            _mFolderPath = EditorGUILayout.TextField("Folder Path", _mFolderPath);
            
            GUILayout.EndVertical();
            GUILayout.Space(10f);
            
            if (GUILayout.Button("Select Folder"))
            {
                _mFolderPath = EditorUtility.OpenFolderPanel("Select Folder", _mFolderPath, Application.dataPath);
            }

            GUI.enabled = IsValidPath(_mFolderPath) &&
                          !string.IsNullOrEmpty(_mReferenceType) &&
                          !string.IsNullOrEmpty(_mNameSpace) &&
                          !string.IsNullOrEmpty(_mStateName);
            
            if (GUILayout.Button("Generate State Boilerplate"))
            {
                if (GenerateBoilerplate(out string path))
                {
                    if (!EditorUtility.DisplayDialog("Success", "State boilerplate generated successfully!", "OK"))
                        return;
                    
                    if (_mShouldHighlightFile)
                    {
                        HighlightFileInProjectWindow(path);
                    }
                        
                    Close();
                }
            }
        }
        
        private static bool IsValidPath(string path)
        {
            return !string.IsNullOrEmpty(path) && Directory.Exists(path);
        }
        
        private bool GenerateBoilerplate(out string newPath)
        {
            string serializableString = _mIsSerializable ? "[Serializable]" : "";
            string boilerplate = $@"using System;
using UnityEngine;
using OceanFSM;

namespace {_mNameSpace}
{{
    {serializableString}
    public class {_mStateName} : State<{_mReferenceType}>
    {{
        public override void OnInitialize()
        {{
            base.OnInitialize();
        }}

        public override void OnEnter()
        {{
            base.OnEnter();
        }}

        public override void OnUpdate(float deltaTime)
        {{
            base.OnUpdate(deltaTime);
        }}

        public override void OnExit()
        {{
            base.OnExit();
        }}
    }}
}}";
            string path = Path.Combine(_mFolderPath, $"{_mStateName}.cs");

            if (File.Exists(path) && !EditorUtility.DisplayDialog("Warning", "File already exists. Do you want to overwrite it?", "Yes", "No"))
            {
                newPath = string.Empty;
                return false;
            }
                
            File.WriteAllText(path, boilerplate);
            newPath = path.Replace(Application.dataPath, "Assets");
            AssetDatabase.Refresh();
            return true;
        }

        private void SetPrefs()
        {
            EditorPrefs.SetString(ReferenceTypeKey, _mReferenceType);
            EditorPrefs.SetString(NamespaceKey, _mNameSpace);
            EditorPrefs.SetBool(IsSerializableKey, _mIsSerializable);
            EditorPrefs.SetString(StateNameKey, _mStateName);
            EditorPrefs.SetString(FolderPathKey, _mFolderPath);
            EditorPrefs.SetBool(ShouldHighlightFile, _mShouldHighlightFile);
        }
        
        private void GetPrefs()
        {
            _mReferenceType = EditorPrefs.GetString(ReferenceTypeKey, "");
            _mNameSpace = EditorPrefs.GetString(NamespaceKey, "");
            _mIsSerializable = EditorPrefs.GetBool(IsSerializableKey, false);
            _mStateName = EditorPrefs.GetString(StateNameKey, "");
            _mFolderPath = EditorPrefs.GetString(FolderPathKey, "");
            _mIsSerializable = EditorPrefs.GetBool(IsSerializableKey, false);
            _mShouldHighlightFile = EditorPrefs.GetBool(ShouldHighlightFile, true);
        }

        private static void HighlightFileInProjectWindow(string path)
        {
            var obj = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));
                
            EditorUtility.FocusProjectWindow();
                
            var pt = Type.GetType("UnityEditor.ProjectBrowser,UnityEditor");
                    
            var ins = pt?.GetField("s_LastInteractedProjectBrowser", 
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)?.GetValue(null);
                    
            var showDirectory = pt?.GetMethod("ShowFolderContents", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    
            showDirectory?.Invoke(ins, new object[]
            {
                obj.GetInstanceID(), true
            });
                
            Selection.activeObject = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));
        }
    }
}

