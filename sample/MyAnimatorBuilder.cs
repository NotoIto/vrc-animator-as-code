using notoito.vrcanimatorascode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAnimatorBuilder : AnimatorBuilder<MyVars>
{
    protected override VRCAnimator AnimatorDefinition(MyVars vars) =>
        new VRCAnimator(
            "TestAnimator",
            new VRCAnimationLayer(
                "Layer1",
                XXXXTask(vars.a, vars.a),
                MyAnimatorLib.HogeTask(vars.a, vars.a)
            ),
            new MyAnimatorLib(
                "UseMyLib",
                vars.a,
                vars.a,
                vars.c
            )
        );

    static VRCAnimationTaskStack XXXXTask(VarInt a, VarInt b) =>
        new VRCAnimationTaskStack(
            "tasks",
            new VRCAnimationTask[]{
        });
}
