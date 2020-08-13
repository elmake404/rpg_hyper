using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IAbilities 
{
    void Initialization(Image image);
    void AtackСorrection(IShell shell, EnemyControl enemy, float damag, bool isIgnorArmor);
    void StartAbility();
    void DethAbility();
    void Atack(float AtackPower, out float Atack, out bool ignoreArmor,Vector3 posTarget);
    float Armor(float DamagPower);
    float AtackSpeed(float Speed);
}
