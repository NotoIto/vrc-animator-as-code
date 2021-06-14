using notoito.vrcanimatorascode.Adapter.Presentation.Core;
using Optional;
using Optional.Linq;
using System.Linq;

namespace notoito.vrcanimatorascode.Adapter.Presentation.Editor.AnimatorBuildParameters
{
    public class Base : CustomEditorHandler
    {
        protected IAnimatorBuildParametersRepository animatorBuildParametersRepository;

        protected override sealed Option<Unit, DomainDefinedError> GUIHandler()
            =>
                from animatorBuildParameters in animatorBuildParametersRepository.Read()
                from nextAnimatorBuildParameters in animatorBuildParameters.Draw()
                from _ in animatorBuildParametersRepository.Write(nextAnimatorBuildParameters)
                select _;
    }
}
