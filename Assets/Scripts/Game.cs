using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

    [SerializeField]
    GameObject canvas;

    [SerializeField]
    Player player;
    [SerializeField]
    Conveyor conveyor;
    [SerializeField]
    BoxSpawner spawner;

    [SerializeField]
    AnimationCurve minimumBoxSpawnTimeCurve;
    [SerializeField]
    AnimationCurve maximumBoxSpawnTimeCurve;
    [SerializeField]
    AnimationCurve boxSpawnAmountCurve;

    [SerializeField]
    AnimationCurve conveyorSpeedCurve;
    [SerializeField]
    AnimationCurve conveyorKickCurve;

    [SerializeField]
    float secondsUntilMaxDifficulty = 300;

    private bool gameOver = false;

    private void Start() {
        
    }

    private void Update() {
        if (!player.Alive && !this.gameOver) {
            this.GameOver();
        }
        this.spawner.AdjustMinimumSpawnTime(this.minimumBoxSpawnTimeCurve.Evaluate(Time.timeSinceLevelLoad / this.secondsUntilMaxDifficulty));
        this.spawner.AdjustMaximumSpawnTime(this.maximumBoxSpawnTimeCurve.Evaluate(Time.timeSinceLevelLoad / this.secondsUntilMaxDifficulty));
        this.spawner.AdjustSpawnAmount(this.boxSpawnAmountCurve.Evaluate(Time.timeSinceLevelLoad / this.secondsUntilMaxDifficulty));
        this.conveyor.AdjustConveyorSpeed(this.conveyorSpeedCurve.Evaluate(Time.timeSinceLevelLoad / this.secondsUntilMaxDifficulty));
        this.conveyor.AdjustConveyorKick(this.conveyorKickCurve.Evaluate(Time.timeSinceLevelLoad / this.secondsUntilMaxDifficulty));
    }

    void GameOver() {
        Destroy(this.spawner.gameObject);
        //get what player was killed by and change gameover text.
        if (player.KilledBy == "burner")
        this.gameOver = true;
        this.canvas.SetActive(true);
    }

    public void loadMenu() {
        SceneManager.LoadScene(0);
    }

    public void loadGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
