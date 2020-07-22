using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbilities 
{
    void Initialization(CanvasManager canvasManager);
    void AtackСorrection(IShell shell, EnemyControl enemy, float damag, bool isIgnorArmor);
    void StartAbility();
    void DethAbility();
    void Atack(float AtackPower, out float Atack, out bool ignoreArmor);
    float Armor(float DamagPower);
    float AtackSpeed(float Speed);
}
