using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    [System.Serializable]
    public struct Puzzle
    {
        public string name;
        public MonoBehaviour script;
    }

    [SerializeField]
    public Puzzle[] _puzzles;

    // Puzzle Scripts
    GaugeIndicator _gaugeIndicator;
    //IceCubeSpawner _iceCubeSpawner; // New dispenser script, needs updating

    void Start()
    {
        foreach (Puzzle puzzle in _puzzles)
        {
            if (puzzle.script != null)
            {
                if (puzzle.script is GaugeIndicator)
                {
                    _gaugeIndicator = puzzle.script as GaugeIndicator;
                    _gaugeIndicator.OnComplete.AddListener(OnBoilerPuzzleComplete);
                }
                //else if (puzzle.script is IceCubeSpawner)
                //{
                //    _iceCubeSpawner = puzzle.script as IceCubeSpawner;
                //    _iceCubeSpawner.OnComplete.AddListener(OnIceDispenserComplete);
                //}
            }
        }
    }

    void OnBoilerPuzzleComplete(string destinationName)
    {
        int levelIndex = SceneManager.GetActiveScene().buildIndex;

        const int fireTutorial = 4;

        if (levelIndex == fireTutorial && destinationName == "On")
        {
            _gaugeIndicator.enabled = false; // script disabled.
            // Extra Code
        }
    }

    void OnIceDispenserComplete()
    {
        //_iceCubeSpawner.enabled = false;  // script disabled.
        // Extra Code
    }
}