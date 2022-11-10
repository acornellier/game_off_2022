using UnityEditor;
using UnityEditor.Timeline;

namespace TimelineExtension
{
    [CustomEditor(typeof(EventClip), true)]
    public class EventClipInspector : Editor
    {
        string _lastKey;
        SerializedProperty _unityEvent;
        SerializedProperty _eventName;

        public void OnEnable()
        {
            _eventName = serializedObject.FindProperty("eventName");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.DelayedTextField(_eventName);

            var eventProperty = GetEventProperty(_eventName.stringValue);
            if (!string.IsNullOrEmpty(_eventName.stringValue) && eventProperty != null)
            {
                eventProperty.serializedObject.Update();
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(eventProperty);
                eventProperty.serializedObject.ApplyModifiedProperties();
            }

            serializedObject.ApplyModifiedProperties();
        }

        SerializedProperty GetEventProperty(string key)
        {
            if (TimelineEditor.inspectedDirector == null)
            {
                _unityEvent = null;
                return null;
            }

            if (_unityEvent == null || _lastKey != key)
            {
                var eventTable = TimelineEditor.inspectedDirector.GetComponent<EventTable>();
                if (eventTable == null)
                    eventTable =
                        TimelineEditor.inspectedDirector.gameObject.AddComponent<EventTable>();

                var o = new SerializedObject(eventTable);
                var evt = eventTable.GetEvent(key, true);
                o.Update();

                var table = o.FindProperty("m_Entries");
                var index = eventTable.IndexOf(evt);
                _unityEvent = table.GetArrayElementAtIndex(index).FindPropertyRelative("m_Event");
                _lastKey = key;
            }

            return _unityEvent;
        }
    }
}