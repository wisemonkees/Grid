using UnityEngine;

namespace WiseMonkeES.Grid
{
    public class Grid <TGridObject>
{
    private int width;
    private int height;
    private Vector3 originPosition;
    private float cellSize;
    private TGridObject[,] gridArray;
    private TextMesh[,] debugTextArray;
    public Grid(int width, int height,float cellSize, Vector2 originPosition, bool showDebug = false, TGridObject defaultGridObject = default(TGridObject))
    {
        this.height = height;
        this.width = width;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                gridArray[i, j] = defaultGridObject;
            }
        }
        
        
        if(!showDebug) return;
        debugTextArray= new TextMesh[width, height];
        for (int x=0; x<gridArray.GetLength(0); x++ )
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                debugTextArray[x, y] = CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 20, Color.white
                    , TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(x,y),GetWorldPosition(x,y+1),Color.white,100f);
                Debug.DrawLine(GetWorldPosition(x,y),GetWorldPosition(x+1,y),Color.white,100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0,height),GetWorldPosition(width,height),Color.white,100f);
        Debug.DrawLine(GetWorldPosition(width,0),GetWorldPosition(width,height),Color.white,100f);
    }

    public static TextMesh CreateWorldText(string text, Transform parent = null,
        Vector3 localPosition = default(Vector3),
        int fontSize = 40, Color color = default, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment
            = TextAlignment.Left, int sortingOrder = 5000){
        if(color == null)
            color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color) color, textAnchor, textAlignment,
            sortingOrder);
    }

    private static TextMesh CreateWorldText(Transform parent, string text, Vector2 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text",typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent,false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        gameObject.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
        return textMesh;
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }
    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition-originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition-originPosition).y / cellSize);
    }

    private void SetValue(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
    }
    
    public void SetValue(Vector2 worldPosition, TGridObject value)
    {
        GetXY(worldPosition, out int x, out int y);
        SetValue(x, y, value);
    }
    public TGridObject GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(TGridObject);
        }
    }
    public TGridObject GetValue(Vector2 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }
}
}
