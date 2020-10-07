/*Selim Gezer tarafından hazırlanmıştır.*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Karakter : MonoBehaviour
{

    public int toplananItem;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Finish"))
        {
            sonrakiLevelKontrolcusu();
            //SceneManager.LoadScene("Leveller"); //level seçim ekranı
           
        }

        if (other.gameObject.tag.Contains("Respawn"))
        {
            toplananItem += 1;
        }

    }

    

    public void sonrakiLevelKontrolcusu()
    {
        string currentLevel = PlayerPrefs.GetString("suankiSecilenLevel");
        int currentLevelID = int.Parse(currentLevel.Split('_')[1]); //Level_id biçiminde olduğundan sağtaraf yani (id) 
        yildizci(currentLevelID);
        int nextLevel = PlayerPrefs.GetInt("level") + 1;

        if (currentLevelID==PlayerPrefs.GetInt("seviyeSayisi"))
        {
            Debug.Log("Oyun Sonu");
        }
        else
        {
            if (nextLevel - currentLevelID == 1)
            PlayerPrefs.SetInt("level", nextLevel);
            else
            Debug.Log("Önceden Açılmış bir bölüme girdiniz.");
        }     
    }

    public string orjinal;
    public void yildizci(int level_ID)
    {
        orjinal = PlayerPrefs.GetString("yildizlar"); //"0,0,0"

        if (toplananItem >int.Parse( orjinal.Substring((level_ID - 1) * 2, 1)))   //3>0
        {
            orjinal = orjinal.Remove((level_ID - 1) * 2, 1);//",0,0,"
            orjinal = orjinal.Insert((level_ID - 1) * 2, toplananItem.ToString()); //3 "3"
        }
        PlayerPrefs.SetString("yildizlar",orjinal); //"3,0,0"
    }

}
