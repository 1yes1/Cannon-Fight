using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

namespace CannonFightBase
{
    public class JoystickUICreator : Graphic
    {
        [Header("UI Creator")]
        [SerializeField] private float _frequency = 0.05f;

        [SerializeField] private float _amplitude = 15;

        [SerializeField] private float _maxAmplitude = 100;

        [SerializeField] private float _topAmplitudePercent = 15;

        [SerializeField] private float _bottomAmplitudePercent = 15;

        private const int TotalVertexCount = 32;

        private const int PartCount = 1;

        public float Amplitude => _amplitude;

        public float MaxAmplitude => _maxAmplitude;

        protected override void OnPopulateMesh(VertexHelper helper)
        {
            int totalVertexCount = TotalVertexCount;

            if (TotalVertexCount % 4 != 0)
                totalVertexCount -= totalVertexCount % 4;

            int partCount = PartCount * 2;

            float minX = (0f - rectTransform.pivot.x) * rectTransform.rect.width;
            float minY = (0f - rectTransform.pivot.y) * rectTransform.rect.height;
            float maxX = (1f - rectTransform.pivot.x) * rectTransform.rect.width;
            float maxY = (1f - rectTransform.pivot.y) * rectTransform.rect.height;

            Color32 color32 = color;

            helper.Clear();
            int partVertexCount = totalVertexCount / partCount;

            float spaceX = rectTransform.rect.width / partVertexCount;
            float spaceY = rectTransform.rect.height / PartCount;

            for (int i = 0; i < partCount; i++)
            {
                for (int j = 0; j <= partVertexCount; j++)
                {
                    Vector3 pos = new Vector3(minX + spaceX * j, Mathf.Sin(j * _frequency) * _amplitude + maxY - spaceY * i);

                    //TopLeft Vertex
                    if (i == 0)
                        pos.x = Mathf.Cos(j * _frequency) * -_topAmplitudePercent * _amplitude / 100 + minX + spaceX * j;
                    else
                        pos.x = Mathf.Cos(j * _frequency) * -_bottomAmplitudePercent * _amplitude / 100 + minX + spaceX * j;

                    helper.AddVert(pos, color32, Vector2.one);
                }
            }

            int start = 0;

            for (int i = start; i < totalVertexCount / 2; i++)
            {
                helper.AddTriangle(i, i + 1, i + partVertexCount + 1);
                helper.AddTriangle(i + 1, i + partVertexCount + 2, i + partVertexCount + 1);
            }

            //helper.AddTriangle(0, 1, 10);
            //helper.AddTriangle(1, 11, 10);
            //helper.AddTriangle(0, 1, 11);
            //helper.AddTriangle(1, 12, 11);
            //helper.AddTriangle(2, 12, 11);
        }

        public float ChangeAmplitude(float verticalPercent)
        {
            _amplitude = _maxAmplitude * verticalPercent / 100;
            SetAllDirty();
            return _amplitude;
        }

    }
}
