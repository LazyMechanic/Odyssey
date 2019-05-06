using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey
{

    [EcsOneFrame]
    sealed class BarrierAreaSpawnEvent : IEcsAutoResetComponent
    {
        public enum InsertPosition
        {
            First,
            Last
        }

        public InsertPosition insertPositionInRow = InsertPosition.Last;
        public GameObject barrierAreaPrefab;
        public LinkedList<EcsEntity> row;
        public Transform parent;
        public Vector3 position;

        public void Reset()
        {
            barrierAreaPrefab = null;
            row = null; 
            parent = null;
        }
    }
}