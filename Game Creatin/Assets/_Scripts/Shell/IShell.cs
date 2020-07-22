using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShell
{
    void Initialization(EnemyControl enemy, float damag, bool isIgnorArmor);
    void ShellAbility();
}
