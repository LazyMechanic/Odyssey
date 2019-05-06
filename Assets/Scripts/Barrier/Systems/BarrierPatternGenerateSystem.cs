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

        private void GenerateBarrierLinePattern(BarrierLinePatternGenerateEvent generateEvent)
        {
            if (generateEvent.fillType == BarrierPatternFillType.Random)
                GenerateRandomBarrierLinePattern(generateEvent);
            else
                GeneratePeriodicBarrierLinePattern(generateEvent);
        }

        private void GenerateRandomBarrierLinePattern(BarrierLinePatternGenerateEvent generateEvent)
        {
            float maxArea = generateEvent.size;
            float targetArea = maxArea * generateEvent.density;
            float currentArea = 0.0f;

            while (currentArea < targetArea)
            {
                Vector3 position = new Vector3(0, 0, Random.Range(-generateEvent.size / 2, generateEvent.size / 2));
                Vector3 scale = Vector3.one * Random.Range(0.5f, 2.0f);

                var go = GameObject.Instantiate(generateEvent.barrierPrefab,
                                                generateEvent.patternGameObject.transform);

                go.name = generateEvent.barrierPrefab.name;

                go.transform.localPosition = position;
                go.transform.localScale = new Vector3(go.transform.localScale.x * scale.x,
                                                      go.transform.localScale.y * scale.y,
                                                      go.transform.localScale.z * scale.z);

                var collider = go.GetComponentInChildren<Collider>();
                Assert.IsNotNull(collider, "Barrier collider not found");

                currentArea += collider.bounds.size.z;
            }
        }

        private void GeneratePeriodicBarrierLinePattern(BarrierLinePatternGenerateEvent generateEvent)
        {
            var collider = generateEvent.barrierPrefab.GetComponent<Collider>();
            Assert.IsNotNull(collider, "Barrier collider not found");

            float barrierLength = collider.bounds.size.z;
            int numberOfBarriers = (int) ((generateEvent.size / barrierLength) * generateEvent.density);

            for (int i = 0; i < numberOfBarriers; ++i)
            {
                Vector3 position = new Vector3(0, 0, (i + 1) * (generateEvent.size / (numberOfBarriers + 1)) - generateEvent.size / 2);

                var go = GameObject.Instantiate(generateEvent.barrierPrefab,
                                                generateEvent.patternGameObject.transform);
                go.name = generateEvent.barrierPrefab.name;

                go.transform.localPosition = position;
            }
        }

        private void GenerateBarrierRectPattern(BarrierRectPatternGenerateEvent generateEvent)
        {
            if (generateEvent.fillType == BarrierPatternFillType.Random)
                GenerateRandomBarrierRectPattern(generateEvent);
            else
                GeneratePeriodicBarrierRectPattern(generateEvent);
        }

        private void GenerateRandomBarrierRectPattern(BarrierRectPatternGenerateEvent generateEvent)
        {
            float maxArea = generateEvent.size.x * generateEvent.size.z;
            float targetArea = maxArea * generateEvent.density;
            float currentArea = 0.0f;

            while (currentArea < targetArea)
            {
                Vector3 position = new Vector3(Random.Range(-generateEvent.size.x / 2, generateEvent.size.x / 2), 0, Random.Range(-generateEvent.size.z / 2, generateEvent.size.z / 2));
                Vector3 scale = Vector3.one * Random.Range(0.5f, 2.0f);

                var go = GameObject.Instantiate(generateEvent.barrierPrefab,
                                                generateEvent.patternGameObject.transform);

                go.name = generateEvent.barrierPrefab.name;

                go.transform.localPosition = position;
                go.transform.localScale = new Vector3(go.transform.localScale.x * scale.x,
                                                      go.transform.localScale.y * scale.y,
                                                      go.transform.localScale.z * scale.z);

                var collider = go.GetComponentInChildren<Collider>();
                Assert.IsNotNull(collider, "Barrier collider not found");

                currentArea += collider.bounds.size.x * collider.bounds.size.z;
            }
        }

        private void GeneratePeriodicBarrierRectPattern(BarrierRectPatternGenerateEvent generateEvent)
        {
            var collider = generateEvent.barrierPrefab.GetComponent<Collider>();
            Assert.IsNotNull(collider, "Barrier collider not found");

            int row = (int)((generateEvent.size.z / collider.bounds.size.z) * generateEvent.density);
            int column = (int)((generateEvent.size.x / collider.bounds.size.x) * generateEvent.density);

            for (int iRow = 0; iRow < row; ++iRow)
            {
                float zPosition = (iRow + 1) * (generateEvent.size.z / (row + 1)) - generateEvent.size.z / 2;
                for (int iColumn = 0; iColumn < column; ++iColumn)
                {
                    Vector3 position = new Vector3((iColumn + 1) * (generateEvent.size.x / (column + 1)) - generateEvent.size.x / 2, 0, zPosition);

                    var go = GameObject.Instantiate(generateEvent.barrierPrefab,
                                                    generateEvent.patternGameObject.transform);

                    go.name = generateEvent.barrierPrefab.name;

                    go.transform.localPosition = position;
                }
            }
        }
    }
}