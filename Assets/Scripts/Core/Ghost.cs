using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    Shape ghostShape = null;
    bool hitBottom = false;
    public Color color = new Color(1f, 1f, 1f, 0.2f);

    public void DrawGhost(Shape originalShape, Board gameBoard)
    {
        if (!ghostShape)
        {
            ghostShape = Instantiate(originalShape, originalShape.transform.position, originalShape.transform.rotation) as Shape;
            ghostShape.gameObject.name = "GhostShape";
            SpriteRenderer[] allRenderers = ghostShape.GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer renderer in allRenderers)
            {
                renderer.color = color;
            }
        }
        else
        {
            ghostShape.transform.position = originalShape.transform.position;
            ghostShape.transform.rotation = originalShape.transform.rotation;
        }
        hitBottom = false;
        while (!hitBottom)
        {
            ghostShape.MoveDown();
            if (!gameBoard.IsValidPosition(ghostShape))
            {
                ghostShape.MoveUp();
                hitBottom = true;
            }
        }
    }

    public void Reset()
    {
        Destroy(ghostShape.gameObject);
    }
    public Vector3 GhostShapePosition()
    {
        return ghostShape.transform.position;
    }

}
