using notoito.vrcanimatorascode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyVars: Vars
{
    public readonly VarInt a = new VarInt(VarSyncMode.Sync);
    public readonly VarBool b = new VarBool(VarSyncMode.Local, value: true);
    public readonly VarFloat c = new VarFloat(VarSyncMode.SyncSaved);
}

