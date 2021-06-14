using Optional;
using Optional.Linq;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace notoito.vrcanimatorascode.Adapter.Presentation.Core
{
    public abstract class CustomEditorHandler : UnityEditor.Editor
    {
        private Option<DomainDefinedError> caughtError = Option.None<DomainDefinedError>();
        public override void OnInspectorGUI()
        {
            (from so in serializedObject
                .SomeNotNull()
                .Match(
                    some: v => Option.Some<SerializedObject, DomainDefinedError>(v),
                    none: () => Option.None<SerializedObject, DomainDefinedError>(new SystemError("Deserialize Error on CustomEditorHandler"))
                )
            from _ in caughtError
                .Match(
                    some: e => Option.Some<Unit, DomainDefinedError>(new Unit(() => ErrorGUI(e))),
                    none: () => GUIHandler()
                )
            select _)
                .Match(
                    some: _ => new Unit(),
                    none: e => new Unit(() => ErrorLogger(e))
                );
        }

        protected abstract Option<Unit, DomainDefinedError> GUIHandler();

        private void ErrorLogger(DomainDefinedError e)
        {
            Debug.LogException(e);
            caughtError = Option.Some(e);
        }

        private void ErrorGUI(DomainDefinedError e)
        {
            EditorGUILayout.LabelField("Oops! Caught ERROR!");
            EditorGUILayout.LabelField("Error message:");
            EditorGUILayout.SelectableLabel(e.Message, EditorStyles.textArea);
            EditorGUILayout.LabelField("Stack trace:");
            EditorGUILayout.SelectableLabel(e.StackTrace, EditorStyles.textArea);
            if(GUILayout.Button ("Restart"))
            {
                caughtError = Option.None<DomainDefinedError>();
            }
        }
    }
}
