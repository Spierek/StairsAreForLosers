using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartDisplay : MonoBehaviour {
    #region Variables
    public List<Image> sprites;
    public Text floor;
    public Text gameOver;
    public Text restart;
    #endregion

    #region Monobehaviour Methods
    void Awake () {
            gameOver.enabled = false;
            restart.enabled = false;
    }
    
    void Update () {
        for (int i = 0; i < 8; i++) {
            sprites[i].enabled = i < PlayerController.Instance.health;
        }

        floor.text = Map.instance.bestFloor.ToString();

        if (PlayerController.Instance.health == 0) {
            gameOver.enabled = true;
            restart.enabled = true;
        }
    }
    #endregion

    #region Methods
    #endregion
}
