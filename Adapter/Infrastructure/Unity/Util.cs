using Optional;
using System.Linq;
using UnityEditor;

namespace notoito.vrcanimatorascode.Adapter.Infrastructure.Unity
{
    public static class SerialzierUtil
    {
        public static Option<SerializedProperty[], DomainDefinedError> GetArrayAll(this SerializedProperty serializedProperty) =>
            serializedProperty.isArray ?
            Option.Some<SerializedProperty[], DomainDefinedError>((new object[serializedProperty.arraySize])
                .Select((_, i) => serializedProperty.GetArrayElementAtIndex(i)).ToArray()) :
            Option.None<SerializedProperty[], DomainDefinedError>(new SerializerError($"{serializedProperty.name} is not array", null));

        public static Option<Unit, DomainDefinedError> AddArray(this SerializedProperty serializedProperty) =>
            serializedProperty.isArray ?
            Option.Some<Unit, DomainDefinedError>(new Unit(() => serializedProperty.InsertArrayElementAtIndex(serializedProperty.arraySize))) :
            Option.None<Unit, DomainDefinedError>(new SerializerError($"{serializedProperty.name} is not array", null));
    }

}
