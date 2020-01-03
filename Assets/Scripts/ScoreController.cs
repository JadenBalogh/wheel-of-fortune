using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreController : MonoBehaviour {

    public AudioClip introSound;
    public GameObject redText;
    public GameObject yellowText;
    public GameObject blueText;
    public GameObject redTotalText;
    public GameObject yellowTotalText;
    public GameObject blueTotalText;
    public GameObject inputBox;
    public GameObject inputTotalBox;

    private int redScore;
    private int yellowScore;
    private int blueScore;
    private int redTotalScore;
    private int yellowTotalScore;
    private int blueTotalScore;
    private int scoreToAdd;
    private int scoreTotalToAdd;

    void Start()
    {
        redText.GetComponent<Text>().text = "$" + redScore;
        yellowText.GetComponent<Text>().text = "$" + yellowScore;
        blueText.GetComponent<Text>().text = "$" + blueScore;
        redTotalText.GetComponent<Text>().text = "$" + redTotalScore;
        yellowTotalText.GetComponent<Text>().text = "$" + yellowTotalScore;
        blueTotalText.GetComponent<Text>().text = "$" + blueTotalScore;
    }

    public void PlayIntro()
    {
        PlaySound(introSound);
    }

    public void UpdateRed()
    {
        redScore += scoreToAdd;
        redTotalScore += scoreTotalToAdd;
        scoreToAdd = 0;
        scoreTotalToAdd = 0;
        redText.GetComponent<Text>().text = "$" + redScore;
        redTotalText.GetComponent<Text>().text = "$" + redTotalScore;
        ResetBoxes();
    }

    public void UpdateYellow()
    {
        yellowScore += scoreToAdd;
        yellowTotalScore += scoreTotalToAdd;
        scoreToAdd = 0;
        scoreTotalToAdd = 0;
        yellowText.GetComponent<Text>().text = "$" + yellowScore;
        yellowTotalText.GetComponent<Text>().text = "$" + yellowTotalScore;
        ResetBoxes();
    }

    public void UpdateBlue()
    {
        blueScore += scoreToAdd;
        blueTotalScore += scoreTotalToAdd;
        scoreToAdd = 0;
        scoreTotalToAdd = 0;
        blueText.GetComponent<Text>().text = "$" + blueScore;
        blueTotalText.GetComponent<Text>().text = "$" + blueTotalScore;
        ResetBoxes();
    }

    public void UpdateScoreToAdd()
    {
        scoreToAdd = int.Parse(inputBox.GetComponent<InputField>().text);
    }

    public void UpdateTotalScoreToAdd()
    {
        scoreTotalToAdd = int.Parse(inputTotalBox.GetComponent<InputField>().text);
    }

    void ResetBoxes()
    {
        inputBox.GetComponent<InputField>().text = "";
        inputTotalBox.GetComponent<InputField>().text = "";
    }

    void PlaySound(AudioClip clip)
    {
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().Play();
    }
}
