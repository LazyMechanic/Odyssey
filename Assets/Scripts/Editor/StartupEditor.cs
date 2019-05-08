using UnityEditor;
using UnityEngine;

namespace Odyssey
{
    [CustomEditor(typeof(Startup))]
    public class StartupEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Startup startup = (Startup)target;

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Set Menu level"))
            {
                startup.ChangeLevel(GameConfig.GameLevel.Menu);
            }

            if (GUILayout.Button("Set Game level"))
            {
                startup.ChangeLevel(GameConfig.GameLevel.Game);
            }

            GUILayout.EndHorizontal();

            if (GUILayout.Button("Reset map"))
            {
                EntityBuilder.Instance(startup.GetWorld())
                             .CreateEntity()
                             .AddComponent<BarrierAreaMapResetEvent>();
            }
        }
    }
}