using Optional;

namespace notoito.vrcanimatorascode
{
    public interface IAnimatorBuildParametersRepository
    {
        Option<AnimatorBuildParameters, DomainDefinedError> Read();
        Option<Unit, DomainDefinedError> Write(AnimatorBuildParameters animatorBuildParameters);
    }
}
