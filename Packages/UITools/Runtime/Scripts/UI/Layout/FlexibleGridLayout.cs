using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{

    public enum FitType
    {
        Uniform,
        Width,
        Height,
        FixedRows,
        FixedColums
    }
    public FitType fitType;
    
    public int rows;
    public int colums;

    public Vector2 cellSize;
    public Vector2 spacing;

    public bool fitX;
    public bool fitY;
    
    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        if (fitType == FitType.Uniform || fitType == FitType.Width || fitType == FitType.Height)
        {
            float sqrRt = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(sqrRt);
            colums = Mathf.CeilToInt(sqrRt);
            fitX = fitY = true;
        }

        if (fitType == FitType.Width || fitType == FitType.FixedColums)
        {
            rows = Mathf.CeilToInt(transform.childCount / (float)colums);
        }
        else if(fitType == FitType.Height || fitType == FitType.FixedRows)
        {
            colums = Mathf.CeilToInt(transform.childCount / (float)rows);
        }

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = parentWidth / colums - (spacing.x / colums) * (colums - 1)  - padding.left / (float) colums - padding.right / (float) colums;
        float cellHeight = parentHeight / rows - (spacing.y / rows) * (rows - 1) - padding.top / (float) rows - padding.bottom / (float) rows;

        cellSize = new Vector2(fitX ? cellWidth : cellSize.x, fitY ? cellWidth : cellSize.y);

        int columnCount = 0;
        int rowCount = 0;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / colums;
            columnCount = i % colums;

            var item = rectChildren[i];

            var xPos = cellSize.x * columnCount + spacing.x * columnCount + padding.left;
            var yPos = cellSize.y * rowCount + spacing.y * rowCount + padding.top;
            
            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
            
        }
    }

    public override void CalculateLayoutInputVertical()
    {
    }

    public override void SetLayoutHorizontal()
    {
    }

    public override void SetLayoutVertical()
    {
        
    }
}
