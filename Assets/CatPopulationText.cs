using Code;
using UnityEngine;
using UnityEngine.UI;

public class CatPopulationText : MonoBehaviour
{
    private GameView _gameView;
    private Text _text;

    private void Awake()
    {
        _gameView = GameView.Instance;
        _text = GetComponent<Text>();
    }

    void Update()
    {
        _text.text = "Cat population: " + _gameView.GetCatPopulation();
    }
}
