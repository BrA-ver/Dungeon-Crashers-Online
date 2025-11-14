using System.Collections;
using UnityEngine;

public class BossFight : Wave
{
    protected override IEnumerator StartWaveRoutine()
    {
        AudioManager.instance.PlayMusic("Boss");
        return base.StartWaveRoutine();

        
    }
}
