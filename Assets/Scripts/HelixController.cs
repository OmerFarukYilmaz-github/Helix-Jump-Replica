using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixController : MonoBehaviour {

    private Vector3 startRotation;
    private Vector2 lastTapPos;
    private float helixDistance;

    public Transform topTransform;
    public Transform goalTransform;

    public GameObject helixLevelPrefab;

    public List<StageSO> allStages = new List<StageSO>();
    private List<GameObject> spawnedLevels = new List<GameObject>();

 
    void Awake () 
    {
        startRotation = transform.localEulerAngles;
        helixDistance = topTransform.localPosition.y - (goalTransform.localPosition.y + .1f);
        LoadStage(0);
    }
	
	// Update is called once per frame
	void Update ()
    {
        
        if (Input.GetMouseButton(0))
        {
            Vector2 currentTapPos = Input.mousePosition;

            if (lastTapPos == Vector2.zero)
                lastTapPos = currentTapPos;

            float delta = lastTapPos.x - currentTapPos.x;
            lastTapPos = currentTapPos;

            transform.Rotate(Vector3.up * delta);
        }

        if (Input.GetMouseButtonUp(0))
        {
            lastTapPos = Vector2.zero;
        }
    }

    public void LoadStage(int stageNumber)
    {

        StageSO stage = allStages[Mathf.Clamp(stageNumber, 0, allStages.Count - 1)];

        if (stage == null)
        {
            Debug.LogError("Undefined stage " + stageNumber);
            return;
        }

        ChangeColors(stageNumber);

        // Reset the helix rotation
        transform.localEulerAngles = startRotation;

        DestroyOldLevelIfExist();

        // Create the new levels
        float levelDistance = helixDistance / stage.levels.Count;
        float spawnPosY = topTransform.localPosition.y;

        for (int i = 0; i < stage.levels.Count; i++)
        {
            spawnPosY -= levelDistance;
            GameObject level = Instantiate(helixLevelPrefab, transform);
            //Debug.Log("Spawned Level");
            level.transform.localPosition = new Vector3(0, spawnPosY, 0);
            spawnedLevels.Add(level);


            int partsToDisable = 12 - stage.levels[i].partCount;
            List<GameObject> disabledParts = new List<GameObject>();

            //Debug.Log("Should disable " + partsToDisable);

            DisableRandomPartsInDisableObject(level, partsToDisable, disabledParts);

            List<GameObject> leftParts = MarkDeathParts(stageNumber, level);

            //Debug.Log(leftParts.Count + " parts left");

            List<GameObject> deathParts = new List<GameObject>();

            //Debug.Log("Should mark " + stage.levels[i].deathPartCount + " death parts");

            while (deathParts.Count < stage.levels[i].deathPartCount)
            {
                GameObject randomPart = leftParts[(Random.Range(0, leftParts.Count))];

                if (!deathParts.Contains(randomPart))
                {
                    randomPart.gameObject.AddComponent<DeadlyPart>();
                    deathParts.Add(randomPart);
                    //Debug.Log("Set death part");
                }
            }


        }
    }

    private List<GameObject> MarkDeathParts(int stageNumber, GameObject level)
    {
        List<GameObject> leftParts = new List<GameObject>();

        foreach (Transform t in level.transform)
        {
            t.GetComponent<Renderer>().material.color = allStages[stageNumber].stageLevelPartColor; // Set color of part

            if (t.gameObject.activeInHierarchy)
                leftParts.Add(t.gameObject);
        }

        return leftParts;
    }

    private static void DisableRandomPartsInDisableObject(GameObject level, int partsToDisable, List<GameObject> disabledParts)
    {
        while (disabledParts.Count < partsToDisable)
        {
            GameObject randomPart = level.transform.GetChild(Random.Range(0, level.transform.childCount)).gameObject;
            if (!disabledParts.Contains(randomPart))
            {
                randomPart.SetActive(false);
                disabledParts.Add(randomPart);
                // Debug.Log("Disabled Part");
            }
        }
    }

    private void DestroyOldLevelIfExist()
    {
        foreach (GameObject go in spawnedLevels)
            Destroy(go);
    }

    private void ChangeColors(int stageNumber)
    {
        Camera.main.backgroundColor = allStages[stageNumber].stageBackgroundColor;
        FindObjectOfType<BallController>().GetComponent<Renderer>().material.color = allStages[stageNumber].stageBallColor;
    }
}
