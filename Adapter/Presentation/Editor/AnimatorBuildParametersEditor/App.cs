using UnityEditor;
using notoito.vrcanimatorascode.Adapter.Infrastructure.Unity;

namespace notoito.vrcanimatorascode.Adapter.Presentation.Editor.AnimatorBuildParameters
{
    [CustomEditor(typeof(AnimatorBuildParametersScriptable))]
    public sealed class App : Base
    {
        public void OnEnable()
        {
            this.animatorBuildParametersRepository = new AnimatorBuildParametersRepositoryOnUnity(
                serializedObject: serializedObject
            );
        }
    }
}
