using Shapes2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicBase : MonoBehaviour
{
    [SerializeField]
    RectTransform GameZoneCanvas;
    
    //Me --- Filled Path ------------
    [SerializeField]
    Shape Me_Shape;
    [SerializeField]
    GameObject Me;
    PathSegment[] Me_Segs 
    { 
        get { return Me_Shape.settings.pathSegments; } 
        set { Me_Shape.settings.pathSegments = value; }
    }
    //Me --- Line Path -------------
    [SerializeField]
    Shape Me_LineShape;
    //Me ---------------------------

    [SerializeField]
    int Segs_Count;
    float Seg_X_Size;

    [SerializeField]
    GameObject Pointer;

    [SerializeField]
    float Speed;
    [SerializeField]
    float Debug_Point_Speed;

    int select_prev = -1;

    Vector2 Highest_Point;

    PathSegment[] Generate_Vertexes(int Parts_Count)
    {
        PathSegment[] Segs = new PathSegment[Parts_Count + 3];
        float X_interval = 1f / Parts_Count;
        Segs[0] = new PathSegment(BoundsToWorldPoint(new Vector2(0f, 0f), GameZoneCanvas), BoundsToWorldPoint(new Vector2(0f, 0.5f), GameZoneCanvas));
        for (int i = 0; i < Parts_Count; i++)
        {
            Segs[i + 1] = new PathSegment(BoundsToWorldPoint(new Vector2(X_interval * i, 0.5f), GameZoneCanvas), BoundsToWorldPoint(new Vector2(X_interval * (i + 1), 0.5f), GameZoneCanvas));
        }
        Segs[Parts_Count + 1] = new PathSegment(BoundsToWorldPoint(new Vector2(1f, 0.5f), GameZoneCanvas), BoundsToWorldPoint(new Vector2(1f, 0f), GameZoneCanvas));
        Segs[Parts_Count + 2] = new PathSegment(BoundsToWorldPoint(new Vector2(1f, 0f), GameZoneCanvas), BoundsToWorldPoint(new Vector2(0f, 0f), GameZoneCanvas));

        return Segs;
    }
    //Конвертация из RectTransform в мировую координату
    Vector2 BoundsToWorldPoint(Vector2 InBounds, RectTransform BoundsSource)
    {
        Vector3[] V = new Vector3[4];
        BoundsSource.GetWorldCorners(V);
        return new Vector2(V[0].x + (V[2].x - V[0].x) * InBounds.x, V[0].y + (V[2].y - V[0].y) * InBounds.y);
    }
    Vector2 BoundsToWorldPoint(Vector2 InBounds, RectTransform BoundsSource, Vector2 Pivot)
    {
        Vector3[] V = new Vector3[4];
        BoundsSource.GetWorldCorners(V);
        return new Vector2(V[0].x + (V[2].x - V[0].x) * (InBounds.x + Pivot.x), V[0].y + (V[2].y - V[0].y) * (InBounds.y + Pivot.y));
    }
    //Мировая координата по точке в пределах сегмента
    Vector2 WorldPointFromSegment(Vector2 Point, int select)
    {
        return BoundsToWorldPoint(new Vector2((1f / Segs_Count) * (select - 1 + Point.x), Point.y), GameZoneCanvas, new Vector2(0f, 0.5f));
    }
    Vector2 WorldToBounds(Vector2 WorldPoint, RectTransform BoundsSource)
    {
        Vector3[] V = new Vector3[4];
        BoundsSource.GetWorldCorners(V);
        Vector2 size = V[2] - BoundsSource.position;
        return new Vector2((WorldPoint.x - BoundsSource.position.x) / size.x, (WorldPoint.y - BoundsSource.position.y) / size.y);
    }

    void Start()
    {
        Vector3[] V = new Vector3[4];
        GameZoneCanvas.GetWorldCorners(V);

        Me.transform.position = (V[3] + V[1]) / 2;
        Me.transform.localScale = V[2] - V[0];

        Me_LineShape.gameObject.transform.position = Me.transform.position;
        Me_LineShape.gameObject.transform.localScale = Me.transform.localScale;

        Me_Shape.SetPathWorldSegments(Generate_Vertexes(Segs_Count));

        Pointer.transform.position = BoundsToWorldPoint(new Vector2(0.5f, 0.5f), GameZoneCanvas);

        Seg_X_Size = 1f / Segs_Count;

        Me_LineShape.settings.pathSegments = new PathSegment[Segs_Count];
    }

    void MoveVertexes(int select)
    {
        if (select == Segs_Count + 1 || select == 0)
            return;

        Me_Shape.settings.pathSegments[select].p1.y += Speed * Time.deltaTime;
        if (select == Segs_Count)
        {
            Me_Shape.settings.pathSegments[select].p2.y += Speed * Time.deltaTime;
            Me_Shape.settings.pathSegments[select + 1].p0.y += Speed * Time.deltaTime;
        }
        else
        {
            Me_Shape.settings.pathSegments[select].p2.y += Speed * Time.deltaTime / 2;
            Me_Shape.settings.pathSegments[select + 1].p0.y += Speed * Time.deltaTime / 2;
        }
        if (select == 1)
        {
            Me_Shape.settings.pathSegments[select].p0.y += Speed * Time.deltaTime;
            Me_Shape.settings.pathSegments[select - 1].p2.y += Speed * Time.deltaTime;
        }
        else
        {
            Me_Shape.settings.pathSegments[select].p0.y += Speed * Time.deltaTime / 2;
            Me_Shape.settings.pathSegments[select - 1].p2.y += Speed * Time.deltaTime / 2;
        }

    }


    //Getting critical point on bezier curve
    float Get_X_Of_Highest_Point(PathSegment Segment)
    {
        if (Segment.p0.y > Segment.p1.y)
            return 0;
        else if (Segment.p2.y > Segment.p1.y)
            return 1;
        return (Segment.p0.y - Segment.p1.y) / (Segment.p0.y + Segment.p2.y - 2 * Segment.p1.y);
    }
    
    
    //Value from 0 to 1 on the X-line of the segment
    float Get_Y_By_X_On_Segment(float X, PathSegment Segment)
    {
        return Segment.p0.y * (1 - X) * (1 - X) + Segment.p1.y * 2 * X * (1 - X) + Segment.p2.y * X * X;
    }
    float Get_Y_By_X_On_GameZone(float X)
    {
        return Get_Y_By_X_On_Segment(X % Seg_X_Size * Segs_Count, Me_Segs[Get_Select(X)]);
    }

    Vector2 Get_Highest_Point(PathSegment[] Segs)
    {
        Vector2 Max = new Vector2(Get_X_Of_Highest_Point(Segs[1]), Get_Y_By_X_On_Segment(Get_X_Of_Highest_Point(Segs[1]), Segs[1]));
        int select = 1;
        for (int i = 2; i <= Segs_Count; i++)
        {
            Vector2 New_Max = new Vector2(Get_X_Of_Highest_Point(Segs[i]), Get_Y_By_X_On_Segment(Get_X_Of_Highest_Point(Segs[i]), Segs[i]));
            if (Max.y < New_Max.y)
            {
                Max = New_Max;
                select = i;
            }
        }
        return WorldPointFromSegment(Max, select);
    }

    int Get_Select(float X)
    {
        return (int)(X * Segs_Count) + 1;
    }

    void Move_Pointer(Vector2 Pos)
    {
        float X = Camera.main.WorldToViewportPoint(Pointer.transform.position).x + (Pos.x - Camera.main.WorldToViewportPoint(Pointer.transform.position).x) * Time.deltaTime * Debug_Point_Speed;
        Pointer.transform.position = BoundsToWorldPoint(new Vector2(X, Get_Y_By_X_On_GameZone(X)), GameZoneCanvas, new Vector2(0,0.5f));
    }

    void Update()
    {
        Vector2 MousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        if (Input.GetMouseButton(0))
        {
            Move_Pointer(MousePos);
            MoveVertexes(Get_Select(Camera.main.WorldToViewportPoint(Pointer.transform.position).x));
            Me_Shape.ComputeAndApply();

            
            for (int i = 0; i < Segs_Count; i++)
            {
                Me_LineShape.settings.pathSegments[i] = Me_Segs[i + 1];
            }
            Me_LineShape.ComputeAndApply();
        }
    }
}
