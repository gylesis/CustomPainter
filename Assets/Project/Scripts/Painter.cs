using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class Painter : MonoBehaviour
    {
        [SerializeField] private TextureWrapMode _textureWrapMode;
        [SerializeField] private FilterMode _filterMode;
        [SerializeField] private Material _material;

        [SerializeField] private Texture2D _texture;

        private PaintableTexture _paintableTexture;
        private ToolsController _toolsController;
        private ColorPalette _colorPalette;
        private Camera _camera;

        public void Init(ToolsController toolsController, ColorPalette colorPalette, Camera camera)
        {
            _camera = camera;
            _paintableTexture = new PaintableTexture(_texture, _material, _textureWrapMode, _filterMode);

            _colorPalette = colorPalette;
            _toolsController = toolsController;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

                var raycast = Physics.Raycast(ray, out var hit, 999f);
    
                if (raycast)
                {
                    int x = (int) (hit.textureCoord.x * _paintableTexture.TextureSize);
                    int y = (int) (hit.textureCoord.y * _paintableTexture.TextureSize);

                    ToolType toolType = _toolsController.ToolType;

                    switch (toolType)
                    {
                        case ToolType.Brush:
                            DrawCircle(x, y, _colorPalette.Color);
                            break;
                        case ToolType.Bucket:
                            Bucket(x, y, _colorPalette.Color);
                            break;
                        case ToolType.Pencil:
                            _paintableTexture.SetPixel(x, y, _colorPalette.Color);
                            _paintableTexture.Apply();
                            break;
                        case ToolType.Eraser:
                            DrawCircle(x, y, Color.white);
                            break;
                    }
                }
            }
        }

        private void Bucket(int x, int y, Color bucketColor)
        {
            TextureCellData start = _paintableTexture.GetData(x, y);

            Color oldColor = _paintableTexture.GetPixel(x,y);
            Color newColor = bucketColor;
            
            if(AreColorsSame(oldColor,newColor))
                return;

            Queue<Vector2Int> openSet = new Queue<Vector2Int>();
            openSet.Enqueue(start.Position);

            while (openSet.Count > 0)
            {
                Vector2Int currentPosition = openSet.Dequeue();

                var positionX = currentPosition.x;
                var positionY = currentPosition.y;

                if (positionX < 0 || positionY < 0)
                    continue;

                if (positionX > _paintableTexture.Width - 1 ||
                    positionY > _paintableTexture.Height - 1)
                    continue;

                if (AreColorsSame(_paintableTexture.GetPixel(positionX, positionY), oldColor) == false)
                    continue;

                _paintableTexture.SetPixel(positionX,positionY, newColor);
                
                var left = new Vector2Int(positionX + 1, positionY);
                var right = new Vector2Int(positionX - 1, positionY);
                var up = new Vector2Int(positionX, positionY + 1);
                var down = new Vector2Int(positionX, positionY - 1);

                openSet.Enqueue(left);
                openSet.Enqueue(right);
                openSet.Enqueue(up);
                openSet.Enqueue(down);
            }

            _paintableTexture.Apply();
        }

        private bool AreColorsSame(Color originColor, Color color)
        {
            float precision = 0.07f;

            var first = (originColor.r + originColor.g + originColor.b) / 3;
            var second = (color.r + color.g + color.b) / 3;

            var value = Mathf.Abs(first - second);

            return value < precision;
        }

        private void DrawCircle(int x, int y, Color color)
        {
            for (int i = 0; i < _toolsController.BrushSize; i++)
            {
                for (int j = 0; j < _toolsController.BrushSize; j++)
                {
                    var brushRadius = _toolsController.BrushSize / 2;

                    float x2 = (i - brushRadius) * (i - brushRadius);
                    float y2 = (j - brushRadius) * (j - brushRadius);

                    float r2 = brushRadius * brushRadius;

                    if (x2 + y2 < r2)
                    {
                        var drawPos = new Vector2Int(x + i - brushRadius, y + j - brushRadius);

                        _paintableTexture.SetPixel(drawPos.x, drawPos.y, color);
                    }
                }
            }

            _paintableTexture.Apply();
        }
    }
}