using System.Threading.Tasks;
using UnityEngine;

public delegate void TimeActionFinish();

public class TimeAction : MonoBehaviour
{
    protected float _time;
    protected float _actionTimer;
    protected TimeActionFinish _action;

    public virtual void Delay(
        float time,
        TimeActionFinish action = null
    )
    {
        _actionTimer = 0;
        _time = time;
        _action = action;
    }

    private void Update()
    {
        _actionTimer += Time.deltaTime;
        float way = _actionTimer / _time;
        if (way < 1)
            UpdateWay(way);
        else
        {
            UpdateWay(1);
            _action?.Invoke();
            Destroy(this);
        }
    }

    protected virtual void UpdateWay(float way) { }
}

public class MoveAction : TimeAction
{
    protected Vector3 _startPoint;
    protected Vector3 _startEnd;
    private TaskCompletionSource<bool> _task;

    public Task MoveWithTimeAsyn(float time, Vector3 targetPosition)
    {
        _task = new TaskCompletionSource<bool>();
        MoveWithTime(time, targetPosition, () => _task.TrySetResult(true));
        return _task.Task;
    }

    public void MoveWithTime(
        float time,
        Vector3 targetPosition,
        TimeActionFinish action = null
    )
    {
        // Обнуление так как не корректно берётся позиция
        targetPosition.z = 0;

        _startPoint = gameObject.transform.localPosition;
        _startEnd = targetPosition;
        base.Delay(time, action);
    }

    protected override void UpdateWay(float way)
    {
        transform.localPosition = Vector3.Lerp(
            _startPoint,
            _startEnd,
            way
        );
    }
}