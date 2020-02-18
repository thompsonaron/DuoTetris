﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Transform emptySprite;
    public int height = 30;
    public int width = 10;

    public int m_header = 8;

    // regular math x,y grid
    Transform[,] m_grid;

    public int m_completedRows = 0;

    public ParticlePlayer[] m_rowGlowFX = new ParticlePlayer[4];

    void Awake()
    {
        // setting up x,y grid
        m_grid = new Transform[width, height];
    }

    void Start()
    {
        DrawEmptyCells();
    }

    bool IsWithinBoard(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0);
    }

    //check if space is occupied by previous shapes or our active shape
    bool IsOccupied(int x, int y, Shape shape)
    {
        return (m_grid[x, y] != null && m_grid[x, y].parent != shape.transform);
    }

    public bool IsValidPosition(Shape shape)
    {
        foreach (Transform child in shape.transform)
        {
            Vector2 pos = Vector2Int.RoundToInt(child.position);

            if (!IsWithinBoard((int)pos.x, (int)pos.y))
            {
                return false;
            }

            if (IsOccupied((int)pos.x, (int)pos.y, shape))
            {
                return false;
            }
        }
        return true;
    }

    // draws game grid
    void DrawEmptyCells()
    {
        if (emptySprite != null)
        {

            for (int y = 0; y < height - m_header; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Transform clone;
                    clone = Instantiate(emptySprite, new Vector3(x, y, 0), Quaternion.identity) as Transform;
                    clone.name = "Board space ( x = " + x.ToString() + " , y =" + y.ToString() + ")";
                    clone.transform.parent = transform;
                }
            }
        }
        else
        {
            Debug.Log("WARNING! Please assign the emptySprite object!");
        }
    }

    public void StoreShapeInGrid(Shape shape)
    {
        if (shape == null)
        {
            return;
        }

        foreach (Transform child in shape.transform)
        {
            Vector2 pos = Vector2Int.RoundToInt(child.position);
            m_grid[(int)pos.x, (int)pos.y] = child;
        }
    }

    bool IsRowComplete(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (m_grid[x, y] == null)
            {
                return false;
            }
        }

        return true;
    }

    void ClearRow(int y)
    {
        Debug.Log("Clearing");
        for (int x = 0; x < width; x++)
        {
            if (m_grid[x, y] != null)
            {
                Destroy(m_grid[x, y].gameObject);
            }
            m_grid[x, y] = null;
        }
    }

    void ShiftOneRowDown(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (m_grid[x, y] != null)
            {
                // adjusts grid
                m_grid[x, y - 1] = m_grid[x, y];
                m_grid[x, y] = null;
                // moves obj pos
                m_grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    void ShiftRowsDown(int startY)
    {
        for (int i = startY; i < height; i++)
        {
            ShiftOneRowDown(i);
        }
    }

    public IEnumerator ClearAllRows()
    {
        m_completedRows = 0;

        for (int y = 0; y < height; y++)
        {
            if (IsRowComplete(y))
            {
                ClearRowFX(m_completedRows, y);
                m_completedRows++;
            }
        }
        yield return new WaitForSeconds(0.5f);
        for (int y = 0; y < height; y++)
        {
            if (IsRowComplete(y))
            {
                ClearRow(y);
                ShiftRowsDown(y + 1);
                yield return new WaitForSeconds(0.3f);
                // if row was completed, will check same row again in case the one that fell is also completed row
                y--;
            }

        }
    }

    public bool IsOverLimit(Shape shape)
    {
        foreach (Transform child in shape.transform)
        {
            if (child.transform.position.y >= (height - m_header - 1))
            {
                return true;
            }
        }
        return false;
    }

    void ClearRowFX(int idx, int y)
    {
        if (m_rowGlowFX[idx])
        {
            m_rowGlowFX[idx].transform.position = new Vector3(0, y, -2);
            m_rowGlowFX[idx].Play();
        }
    }
}
