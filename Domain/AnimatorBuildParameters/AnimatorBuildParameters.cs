using Optional;
using Optional.Linq;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;
namespace notoito.vrcanimatorascode
{
    [CreateAssetMenu(fileName = "AnimatorBuildParameters", menuName = "VRChat/Avatars/AnimatorBuildParameters")]
    public class AnimatorBuildParametersScriptable : ScriptableObject
    {

        protected VRCExpressionParameters baseVRCExpressionParametersNullable;
        protected VRCExpressionParameters destVRCExpressionParametersNullable;
        protected AnimationLinkParameter[] animationLinkParametersNullable;

        protected AnimatorBuildParametersScriptable(
            VRCExpressionParameters baseVRCExpressionParametersNullable,
            VRCExpressionParameters destVRCExpressionParametersNullable,
            AnimationLinkParameter[] animationLinkParametersNullable
            )
        {
            this.baseVRCExpressionParametersNullable = baseVRCExpressionParametersNullable;
            this.destVRCExpressionParametersNullable = destVRCExpressionParametersNullable;
            this.animationLinkParametersNullable = animationLinkParametersNullable;
        }

        protected AnimatorBuildParametersScriptable(
            AnimatorBuildParametersScriptable animatorBuildParametersScriptable
            )
        {
            this.baseVRCExpressionParametersNullable = animatorBuildParametersScriptable.baseVRCExpressionParametersNullable;
            this.destVRCExpressionParametersNullable = animatorBuildParametersScriptable.destVRCExpressionParametersNullable;
            this.animationLinkParametersNullable = animatorBuildParametersScriptable.animationLinkParametersNullable;
        }

        public void Rewrite(AnimatorBuildParameters animatorBuildParameters)
        {
            this.baseVRCExpressionParametersNullable = animatorBuildParameters.baseVRCExpressionParameters.Match(v => v, () => null);
            this.destVRCExpressionParametersNullable = animatorBuildParameters.destVRCExpressionParameters.Match(v => v, () => null);
            this.animationLinkParametersNullable = animatorBuildParameters.animationLinkParameters;
        }
    }

    public class AnimatorBuildParameters : AnimatorBuildParametersScriptable
    {
        public Option<VRCExpressionParameters> baseVRCExpressionParameters { get { return baseVRCExpressionParametersNullable.SomeNotNull(); } }
        public Option<VRCExpressionParameters> destVRCExpressionParameters { get { return destVRCExpressionParametersNullable.SomeNotNull(); } }
        public AnimationLinkParameter[] animationLinkParameters { get { return animationLinkParametersNullable.SomeNotNull().Match(v => v, () => new AnimationLinkParameter[] { }); } }

        private AnimatorBuildParameters(
            VRCExpressionParameters baseVRCExpressionParametersNullable,
            VRCExpressionParameters destVRCExpressionParametersNullable,
            AnimationLinkParameter[] animationLinkParametersNullable
            )
            : base(
                 baseVRCExpressionParametersNullable: baseVRCExpressionParametersNullable,
                 destVRCExpressionParametersNullable: destVRCExpressionParametersNullable,
                 animationLinkParametersNullable: animationLinkParametersNullable
                 )
        { }

        private AnimatorBuildParameters(
            AnimatorBuildParametersScriptable animatorBuildParametersScriptable
            )
            : base(animatorBuildParametersScriptable)
        { }

        public static Option<AnimatorBuildParameters, DomainDefinedError> Create(
            VRCExpressionParameters baseVRCExpressionParametersNullable,
            VRCExpressionParameters destVRCExpressionParametersNullable,
            AnimationLinkParameter[] animationLinkParametersNullable
            )
            =>
                Option.Some<AnimatorBuildParameters, DomainDefinedError>(
                    new AnimatorBuildParameters(
                        baseVRCExpressionParametersNullable: baseVRCExpressionParametersNullable,
                        destVRCExpressionParametersNullable: destVRCExpressionParametersNullable,
                        animationLinkParametersNullable: animationLinkParametersNullable
                    )
                );

        public static Option<AnimatorBuildParameters, DomainDefinedError> Create(AnimatorBuildParametersScriptable animatorBuildParametersScriptable)
            => (new AnimatorBuildParameters(animatorBuildParametersScriptable))
                .SomeNotNull()
                .Match(
                    some: v => Option.Some<AnimatorBuildParameters, DomainDefinedError>(v),
                    none: () => Option.None<AnimatorBuildParameters, DomainDefinedError>(new SystemError("unknown"))
                );
    }

    public static class AnimatorBuildParametersGUI
    {
        public static Option<AnimatorBuildParameters, DomainDefinedError> Draw(this AnimatorBuildParameters animatorBuildParameters)
        {
            var baseVRCExpressionParameters = EditorGUILayout.ObjectField(
                "Base VRC Expression Parameters",
                animatorBuildParameters.baseVRCExpressionParameters.Match(v => v, () => null),
                typeof(VRCExpressionParameters),
                false
            ) as VRCExpressionParameters;
            var destVRCExpressionParameters = EditorGUILayout.ObjectField(
                "Destination VRC Expression Parameters",
                animatorBuildParameters.destVRCExpressionParameters.Match(v => v, () => null),
                typeof(VRCExpressionParameters),
                false
            ) as VRCExpressionParameters;
            var animationLinkParameters = animatorBuildParameters
                .animationLinkParameters?
                .Select(v => v.Draw())
                .ToArray();
            return AnimatorBuildParameters.Create(
                baseVRCExpressionParametersNullable: baseVRCExpressionParameters,
                destVRCExpressionParametersNullable: destVRCExpressionParameters,
                animationLinkParametersNullable: animationLinkParameters
            );
        }
    }

    public class AnimatorBuildParametersValidated
    {
        public readonly Option<VRCExpressionParameters> baseVRCExpressionParameters;
        public readonly VRCExpressionParameters destVRCExpressionParameters;
        public readonly AnimationLinkParameter[] animationLinkParameters;

        private AnimatorBuildParametersValidated(
            Option<VRCExpressionParameters> baseVRCExpressionParameters,
            VRCExpressionParameters destVRCExpressionParameters,
            AnimationLinkParameter[] animationLinkParameters
            )
        {
            this.baseVRCExpressionParameters = baseVRCExpressionParameters;
            this.destVRCExpressionParameters = destVRCExpressionParameters;
            this.animationLinkParameters = animationLinkParameters;
        }

        public static Option<AnimatorBuildParametersValidated, DomainDefinedError> Create(
            AnimatorBuildParameters animatorBuildParameters
            )
            =>
                (from destVRCExpressionParametersValidated in animatorBuildParameters.destVRCExpressionParameters
                select new AnimatorBuildParametersValidated(
                    baseVRCExpressionParameters: animatorBuildParameters.baseVRCExpressionParameters,
                    destVRCExpressionParameters: destVRCExpressionParametersValidated,
                    animationLinkParameters: animatorBuildParameters.animationLinkParameters
                )).Match(
                    some: v => Option.Some<AnimatorBuildParametersValidated, DomainDefinedError>(v),
                    none: () => Option.None<AnimatorBuildParametersValidated, DomainDefinedError>(new ValidationError("on AnimatorBuildParametersValidated"))
                );
    }

    public class AnimationLinkParameter : ScriptableObject
    {
        public readonly AnimationClipWithTag[] animationClipWithTags;
        public readonly AnimatorController[] baseAnimatorControllers;
        public readonly AnimatorController destAnimatorController;
        public readonly AnimatorBuilder animatorBuilder;

        public AnimationLinkParameter(
            AnimationClipWithTag[] animationClipWithTags,
            AnimatorController[] baseAnimatorControllers,
            AnimatorController destAnimatorController,
            AnimatorBuilder animatorBuilder
            )
        {
            this.animationClipWithTags = animationClipWithTags;
            this.baseAnimatorControllers = baseAnimatorControllers;
            this.destAnimatorController = destAnimatorController;
            this.animatorBuilder = animatorBuilder;
        }
    }

    public static class AnimationLinkParameterGUI
    {
        public static AnimationLinkParameter Draw(this AnimationLinkParameter animationLinkParameter)
        {
            var animationClipWithTags = animationLinkParameter
                .animationClipWithTags?
                .Select(v => v.Draw())
                .ToArray();
            var baseAnimatorControllers = animationLinkParameter
                .baseAnimatorControllers?
                .Select((v, i) => EditorGUILayout.ObjectField(
                    $"Base Animator Controller {i}",
                    v,
                    typeof(AnimatorController),
                    false
                    ) as AnimatorController
                )
                .ToArray();
            var destAnimatorController = EditorGUILayout.ObjectField(
                "Destination Animator Controller",
                animationLinkParameter.destAnimatorController,
                typeof(AnimatorController),
                false
                ) as AnimatorController;
            var animatorBuilder = EditorGUILayout.ObjectField(
                "Animator Builder",
                animationLinkParameter.animatorBuilder,
                typeof(AnimatorBuilder),
                false
                ) as AnimatorBuilder;
            return new AnimationLinkParameter(
                animationClipWithTags: animationClipWithTags,
                baseAnimatorControllers: baseAnimatorControllers,
                destAnimatorController: destAnimatorController,
                animatorBuilder: animatorBuilder
            );
        }
    }

    public class AnimationClipWithTag : ScriptableObject
    {
        public readonly string tagName;
        public readonly AnimationClip animationClip;
        public AnimationClipWithTag(
            string tagName,
            AnimationClip animationClip
            )
        {
            this.tagName = tagName;
            this.animationClip = animationClip;
        }
    }

    public static class AnimationClipWithTagGUI
    {
        public static AnimationClipWithTag Draw(this AnimationClipWithTag animationClipWithTag)
        {
            var tagName = EditorGUILayout.TextField(animationClipWithTag.tagName);
            var animationClip = EditorGUILayout.ObjectField(
                "Animation Clip",
                animationClipWithTag.animationClip,
                typeof(AnimationClip),
                false
            ) as AnimationClip;
            return new AnimationClipWithTag(
                tagName: tagName,
                animationClip: animationClip
            );
        }
    }
}
