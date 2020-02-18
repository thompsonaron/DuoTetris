using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Shape[] allShapes;
    public Transform[] queeuedTransforms = new Transform[3];
    Shape[] queuedShapes = new Shape[3];
    public Transform pos;

    float queueScale = 0.5f;

    private void Awake()
    {
        InitQueue();
    }

    Shape GetRandomShape()
    {
        int i = Random.Range(0, allShapes.Length);
        if (allShapes[i])
        {
            return allShapes[i];
        }
        else
        {
            Debug.Log("WARNING! Invalid shape in spawner!");
            return null;
        }
    }

    public Shape SpawnShape()
    {
        Shape shape = null;
        // shape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;
        shape = GetQueuedShape();
        shape.transform.position = pos.position;
        shape.transform.localScale = Vector3.one; ;

        if (shape)
        {
            return shape;
        }
        else
        {
            Debug.Log("WARNING! Invalid shape in spawner!");
            return null;
        }
    }

    void InitQueue()
    {
        for (int i = 0; i < queuedShapes.Length; i++)
        {
            queuedShapes[i] = null;
        }
        FillQueue();
    }

    void FillQueue()
    {
        for (int i = 0; i < queuedShapes.Length; i++)
        {
            if (!queuedShapes[i])
            {
                queuedShapes[i] = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;
                queuedShapes[i].transform.position = queeuedTransforms[i].position;
                queuedShapes[i].transform.localScale = new Vector3(queueScale, queueScale, queueScale);

            }
        }
    }

    Shape GetQueuedShape()
    {
        Shape firstShape = null;
        if (queuedShapes[0])
        {
            firstShape = queuedShapes[0];
        }

        for (int i = 1; i < queuedShapes.Length; i++)
        {
            queuedShapes[i - 1] = queuedShapes[i];
            queuedShapes[i - 1].transform.position = queeuedTransforms[i - 1].position + queuedShapes[i].queueOffset;
        }
        queuedShapes[queuedShapes.Length - 1] = null;

        FillQueue();
        return firstShape;
    }
}
