using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

namespace notoito.vrcanimatorascode
{

    public interface IVar
    {

    }
    public class Vars
    {
        public IVar[] ToVars()
        {
            //リフレクションで
            return new IVar[] { };
        }
    }
    public enum VarSyncMode
    {
        SyncSaved,
        Sync,
        Local
    }

    public class VarDefine
    {

    }

    public class VarInt : VarDefine
    {
        public VarInt(VarSyncMode syncMode, int value = 0)
        {

        }
    }

    public class VarBool : VarDefine
    {
        public VarBool(VarSyncMode syncMode, bool value = false)
        {

        }
    }

    public class VarFloat : VarDefine
    {
        public VarFloat(VarSyncMode syncMode, float value = 0.0f)
        {

        }
    }

    public class VRCAnimator
    {
        private readonly string name;
        private readonly IVar[] vars;
        public VRCAnimator(string name, params VRCAnimationLayerStack[] layers)
        {

        }
    }

    public class VRCAnimationLayerStack
    {
        private readonly string name;
        private readonly VRCAnimationLayer[] layers;
        public VRCAnimationLayerStack(string name, params VRCAnimationLayer[] layers)
        {
            this.name = name;
            this.layers = layers;
        }
    }

    public class VRCAnimationLayer : VRCAnimationLayerStack
    {
        public VRCAnimationLayer(string name, params VRCAnimationTaskStack[] tasks)
            : base(name, new VRCAnimationLayer[] { new VRCAnimationLayer(name, tasks) })
        {

        }
    }

    public class VRCAnimationTaskStack
    {
        private readonly string name;
        private readonly VRCAnimationTaskStack[] tasks;
        private readonly IVar[] vars;

        public VRCAnimationTaskStack(string name, params VRCAnimationTaskStack[] tasks)
        {
            this.name = name;
            this.tasks = tasks;
        }

        public VRCAnimationTaskStack(params VRCAnimationTaskStack[] tasks)
        {
            this.name = this.GetType().Name;
            this.tasks = tasks;
        }

        public AnimatorController GenerateTask(AnimatorController animatorController) =>
            tasks.Aggregate(animatorController, (acc, v) => v.GenerateTask(acc));
    }

    public abstract class VRCAnimationTask : VRCAnimationTaskStack
    {
        public VRCAnimationTask(string name)
            : base(name)
        {

        }

        public VRCAnimationTask()
        {

        }

        public new abstract AnimatorController GenerateTask(AnimatorController animatorController);
    }

    public class If : VRCAnimationTask
    {
        public override AnimatorController GenerateTask(AnimatorController animatorController)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class AnimatorBuilder<T> : AnimatorBuilder
        where T : Vars, new()
    {
        public AnimatorBuilder()
        {
        }

        protected abstract VRCAnimator AnimatorDefinition(T vars);

        public override void Build()
        {
            var animator = AnimatorDefinition(new T());
        }

        public override IVar[] GetVars()
        {
            return null;
        }
    }

    public abstract class AnimatorBuilder : ScriptableObject
    {
        public abstract void Build();
        public abstract IVar[] GetVars();
    }
}
