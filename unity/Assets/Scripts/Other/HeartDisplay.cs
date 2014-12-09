using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartDisplay : MonoBehaviour {
    #region Variables
    public List<Image> sprites;
    public Text floor;
    #endregion

    #region Monobehaviour Methods
    void Awake () {
    
    }
    
    void Update () {
        for (int i = 0; i < 8; i++) {
            sprites[i].enabled = i < PlayerController.Instance.health;
        }

        floor.text = Map.instance.bestFloor.ToString();
    }
    #endregion

    #region Methods
    #endregion
}
