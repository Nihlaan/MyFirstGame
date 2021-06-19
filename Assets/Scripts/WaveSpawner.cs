using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public Transform enemyPrefab;
    public Transform spawnPoint;

    public float timeBetweenWaves = 5.5f;
    private float countdown = 2f;

    public Text waveCountdownText;

    private int waveIndex = 0;

    // FPS
    public float timer, refresh, avgFramerate;
    public string display = "{0} FPS";

    private void Update()
    {
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }

        countdown -= Time.deltaTime;

        waveCountdownText.text = Mathf.Round(countdown).ToString();

        // DisplayFramerate();
    }

    private void DisplayFramerate()
	{
        /*deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        var fpsText = Mathf.Ceil(fps).ToString();*/

        float timeLapse = Time.smoothDeltaTime;
        timer = timer <= 0 ? refresh : timer - timeLapse;

        if (timer <= 0)
		{
            avgFramerate = (int)(1f / timeLapse);
		}

        Debug.Log($"SMOOTH DELTA TIME: {timeLapse}");
        waveCountdownText.text += " ///// " + string.Format(display, avgFramerate);
    }

    private IEnumerator SpawnWave()
    {
        waveIndex++;

        for (int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }   
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

}
