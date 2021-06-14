using notoito.vrcanimatorascode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAnimatorLib : VRCAnimationLayerStack
{
    public MyAnimatorLib(string name, VarInt a, VarInt b, VarFloat c) : base($"MyAnimatorLib.{name}", Layers(a, b, c))
    {
    }

    static VRCAnimationLayer[] Layers(VarInt a, VarInt b, VarFloat c) =>
        new VRCAnimationLayer[]
        {
            new VRCAnimationLayer(
                "Layer1",
                FugaTask(a, b),
                PiyoTask(c)
                )
        };

    public static VRCAnimationTaskStack HogeTask(VarInt a, VarInt b) =>
        new VRCAnimationTaskStack(
            "tasks",
            new VRCAnimationTaskStack[]{
        });

    static VRCAnimationTaskStack FugaTask(VarInt a, VarInt b) =>
        new VRCAnimationTaskStack(
            "tasks",
            new VRCAnimationTaskStack[]{
                HogeTask(a, b),
        });

    static VRCAnimationTaskStack PiyoTask(VarFloat a) =>
        new VRCAnimationTaskStack(
            "tasks",
            new VRCAnimationTaskStack[]{
        });

}
