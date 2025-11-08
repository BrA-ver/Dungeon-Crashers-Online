using System.Collections;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] float lerpTime = 1f;

    [SerializeField] Transform gate;
    [SerializeField] Vector3 closePosition;
    [SerializeField] Vector3 openPosition;

    private void Start()
    {
        WaveSpawner.instance.onBattleStarted += Close;
        WaveSpawner.instance.onBattleEnded += Open;

        closePosition = gate.localPosition;

        Open();
    }

    private void OnDisable()
    {
        WaveSpawner.instance.onBattleStarted -= Close;
        WaveSpawner.instance.onBattleEnded -= Open;
    }

    void Open()
    {
        StartCoroutine(MoveGateRoutine(closePosition, openPosition));
    }

    void Close()
    {
        StartCoroutine(MoveGateRoutine(openPosition, closePosition));
    }

    IEnumerator MoveGateRoutine(Vector3 startPos, Vector3 targetPos)
    {
        float timePassed = 0f;

        while (timePassed < lerpTime)
        {
            timePassed += Time.deltaTime;

            float moveAmount = timePassed / lerpTime;

            gate.localPosition = Vector3.Lerp(startPos, targetPos, moveAmount);

            yield return null;
        }
    }
}
