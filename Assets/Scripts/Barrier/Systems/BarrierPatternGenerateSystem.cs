using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Assertions;

namespace Odyssey {
    [EcsInject]
    sealed class BarrierPatternGenerateSystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<BarrierLinePatternGenerateEvent> _lineBarrierPatternFilter = null;
        private EcsFilter<BarrierRectPatternGenerateEvent> _rectBarrierPatternFilter = null;

        void IEcsRunSystem.Run ()
        {
            foreach (var i in _lineBarrierPatternFilter)
            {
                GenerateBarrierLinePattern(_lineBarrierPatternFilter.Components1[i]);
            }

            foreach (var i in _rectBarrierPatternFilter)
            {
                GenerateBarrierRectPattern(_rectBarrierPatternFilter.Components1[i]);
            }
        }

        private void GenerateBarrierLinePattern(BarrierLinePatternGenerateEvent pattern)
        {
            if (pattern.fillType == BarrierPatternFillType.Random)
                GenerateRandomBarrierLinePattern(pattern);
            else
                GeneratePeriodicBarrierLinePattern(pattern);
        }

        private void GenerateRandomBarrierLinePattern(BarrierLinePatternGenerateEvent pattern)
        {
            float maxArea = pattern.size;
            float targetArea = maxArea * pattern.density;
            float currentArea = 0.0f;

            while (currentArea < targetArea)
            {
                Vector3 position = new Vector3(0, 0, Random.Range(-pattern.size / 2, pattern.size / 2));
                Vector3 scale = Vector3.one * Random.Range(0.5f, 2.0f);

                var go = GameObject.Instantiate(pattern.barrierPrefab,
                                                pattern.patternGameObject.transform);

                go.name = pattern.barrierPrefab.name;

                go.transform.localPosition = position;
                go.transform.localScale = scale;

                var collider = go.GetComponent<Collider>();
                Assert.IsNotNull(collider, "Barrier collider not found");

                currentArea += collider.bounds.size.z;
            }
        }

        private void GeneratePeriodicBarrierLinePattern(BarrierLinePatternGenerateEvent pattern)
        {
            var collider = pattern.barrierPrefab.GetComponent<Collider>();
            Assert.IsNotNull(collider, "Barrier collider not found");

            float barrierLength = collider.bounds.size.z;
            int numberOfBarriers = (int) ((pattern.size / barrierLength) * pattern.density);

            for (int i = 0; i < numberOfBarriers; ++i)
            {
                Vector3 position = new Vector3(0, 0, (i + 1) * (pattern.size / (numberOfBarriers + 1)) - pattern.size / 2);

                var go = GameObject.Instantiate(pattern.barrierPrefab,
                                       pattern.patternGameObject.transform);
                go.name = pattern.barrierPrefab.name;

                go.transform.localPosition = position;
            }
        }

        private void GenerateBarrierRectPattern(BarrierRectPatternGenerateEvent pattern)
        {
            if (pattern.fillType == BarrierPatternFillType.Random)
                GenerateRandomBarrierRectPattern(pattern);
            else
                GeneratePeriodicBarrierRectPattern(pattern);
        }

        private void GenerateRandomBarrierRectPattern(BarrierRectPatternGenerateEvent pattern)
        {
            float maxArea = pattern.size.x * pattern.size.z;
            float targetArea = maxArea * pattern.density;
            float currentArea = 0.0f;

            while (currentArea < targetArea)
            {
                Vector3 position = new Vector3(Random.Range(-pattern.size.x / 2, pattern.size.x / 2), 0, Random.Range(-pattern.size.z / 2, pattern.size.z / 2));
                Vector3 scale = Vector3.one * Random.Range(0.5f, 2.0f);

                var go = GameObject.Instantiate(pattern.barrierPrefab,
                                                pattern.patternGameObject.transform);

                go.name = pattern.barrierPrefab.name;

                go.transform.localPosition = position;
                go.transform.localScale = scale;

                var collider = go.GetComponent<Collider>();
                Assert.IsNotNull(collider, "Barrier collider not found");

                currentArea += collider.bounds.size.x * collider.bounds.size.z;
            }
        }

        private void GeneratePeriodicBarrierRectPattern(BarrierRectPatternGenerateEvent pattern)
        {
            var collider = pattern.barrierPrefab.GetComponent<Collider>();
            Assert.IsNotNull(collider, "Barrier collider not found");

            int row = (int)((pattern.size.z / collider.bounds.size.z) * pattern.density);
            int column = (int)((pattern.size.x / collider.bounds.size.x) * pattern.density);

            for (int iRow = 0; iRow < row; ++iRow)
            {
                float zPosition = (iRow + 1) * (pattern.size.z / (row + 1)) - pattern.size.z / 2;
                for (int iColumn = 0; iColumn < column; ++iColumn)
                {
                    Vector3 position = new Vector3((iColumn + 1) * (pattern.size.x / (column + 1)) - pattern.size.x / 2, 0, zPosition);

                    var go = GameObject.Instantiate(pattern.barrierPrefab,
                                                    pattern.patternGameObject.transform);

                    go.name = pattern.barrierPrefab.name;

                    go.transform.localPosition = position;
                }
            }
        }
    }
}