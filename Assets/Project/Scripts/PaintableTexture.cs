using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class PaintableTexture
    {
        private readonly TextureCellData[,] _textureMap;
        private readonly Texture2D _tempTexture;

        public int TextureSize { get; private set; }

        public int Width => _tempTexture.width;
        public int Height => _tempTexture.height;
        
        public PaintableTexture(Texture2D texture, Material material, TextureWrapMode textureWrapMode,
            FilterMode filterMode)
        {
            Texture2D mainTexture = texture;

            var mainTextureHeight = mainTexture.height;
            var mainTextureWidth = mainTexture.width;

            TextureSize = (mainTextureHeight + mainTextureWidth) / 2;

            var tempTexture = new Texture2D(mainTextureWidth, mainTextureHeight);

            var pixels = mainTexture.GetPixels();

            tempTexture.SetPixels(pixels);

            _tempTexture = tempTexture;

            material.mainTexture = _tempTexture;

            _textureMap = new TextureCellData[mainTexture.width, mainTexture.height];

            for (int y = 0; y < _tempTexture.height; y++)
            {
                for (int x = 0; x < _tempTexture.width; x++)
                {
                    var textureCell = new TextureCellData();

                    textureCell.Position = new Vector2Int(x, y);
                    textureCell.Color = _tempTexture.GetPixel(x, y);

                    _textureMap[x, y] = textureCell;
                }
            }

            _tempTexture.wrapMode = textureWrapMode;
            _tempTexture.filterMode = filterMode;

            _tempTexture.Apply();
        }

        ~PaintableTexture()
        {
            Object.Destroy(_tempTexture);
        }

        public TextureCellData GetData(int x, int y) =>
            _textureMap[x, y];

        public Color GetPixel(int x, int y) =>
            _tempTexture.GetPixel(x, y);

        public Color[] GetPixels()
        {
            return _tempTexture.GetPixels();
        }

        public Color32[] GetPixels32()
        {
            return _tempTexture.GetPixels32();
        }
        
        public void SetPixels32(Color32[] pixels)
        {
            _tempTexture.SetPixels32(pixels);
        }

        public void SetPixel(int x, int y, Color color)
        {
            _tempTexture.SetPixel(x, y, color);

            x = Mathf.Clamp(x, 0, _tempTexture.width - 1);
            y = Mathf.Clamp(y, 0, _tempTexture.height - 1);

            _textureMap[x, y].Color = color;
        }
        public void Apply()
        {
            _tempTexture.Apply();
        }

        public List<Vector2Int> GetNeighbours(int x, int y)
        {
            var cellDatas = new List<Vector2Int>();

            TextureCellData right;
            TextureCellData left;
            TextureCellData up;
            TextureCellData down;

            if (x + 1 < _tempTexture.width)
            {
                right = _textureMap[x + 1, y];
                cellDatas.Add(right.Position);
            }

            if (x - 1 > 0)
            {
                left = _textureMap[x - 1, y];
                cellDatas.Add(left.Position);
            }

            if (y + 1 < _tempTexture.height)
            {
                up = _textureMap[x, y + 1];
                cellDatas.Add(up.Position);
            }

            if (y - 1 > 0)
            {
                down = _textureMap[x, y - 1];
                cellDatas.Add(down.Position);
            }

            return cellDatas;
        }
    }
}