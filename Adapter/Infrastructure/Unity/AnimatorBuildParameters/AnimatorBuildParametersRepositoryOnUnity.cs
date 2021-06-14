using Optional;
using Optional.Linq;
using System.Linq;
using UnityEditor;

namespace notoito.vrcanimatorascode.Adapter.Infrastructure.Unity
{
    public class AnimatorBuildParametersRepositoryOnUnity : IAnimatorBuildParametersRepository
    {
        private SerializedObject serializedObject;

        public AnimatorBuildParametersRepositoryOnUnity(
            SerializedObject serializedObject
            )
        {
            this.serializedObject = serializedObject;
        }

        public Option<AnimatorBuildParameters, DomainDefinedError> Read()
            =>
                from targetNullable in (serializedObject.targetObject as AnimatorBuildParametersScriptable)
                    .SomeNotNull()
                    .Match(
                        some: v => Option.Some<AnimatorBuildParametersScriptable, DomainDefinedError>(v),
                        none: () => Option.None<AnimatorBuildParametersScriptable, DomainDefinedError>(new SerializerError("Read Error"))
                        )
                from target in AnimatorBuildParameters.Create(targetNullable)
                select target;

        public Option<Unit, DomainDefinedError> Write(AnimatorBuildParameters animatorBuildParameters)
            =>
                (from target in (serializedObject.targetObject as AnimatorBuildParametersScriptable)
                    .SomeNotNull()
                    .Match(
                        some: v => Option.Some<AnimatorBuildParametersScriptable, DomainDefinedError>(v),
                        none: () => Option.None<AnimatorBuildParametersScriptable, DomainDefinedError>(new SerializerError("Write Error"))
                    )
                 from _ in UnitOption.Some<DomainDefinedError>(() => target.Rewrite(animatorBuildParameters))
                 select serializedObject.ApplyModifiedProperties())
                .Match(
                    some: _ => UnitOption.Some<DomainDefinedError>(),
                    none: e => UnitOption.None(e)
                );
    }
}
