/*Selim Gezer tarafından hazırlanmıştır.*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Karakter : MonoBehaviour
{
    [Header("Bolum Sonu Panel")]
    public GameObject bolumSonu_P;
    [Space]
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
        string currentLevel = levelAdi(SceneManager.GetActiveScene().buildIndex);// (PlayerPrefs.GetString("suankiSecilenLevel");) Yenilendi çünkü level ekranından geçişte kaydettiğimiz leveli alıyorduk fakat sonraki levele bu sahneden geçince kayıtlı level eskisi kalıyor o yüzden direk aktif sahne build indexinden adını çağırıp işlem yaptırıyoruz.
        int currentLevelID = int.Parse(currentLevel.Split('_')[1]); //Level_id biçiminde olduğundan sağtaraf yani (id) 
        yildizci(currentLevelID);
        int nextLevel = PlayerPrefs.GetInt("level") + 1;

        if (currentLevelID==PlayerPrefs.GetInt("seviyeSayisi"))
        {
            Debug.Log("Oyun Sonu");

            bolumSonu_P.transform.GetChild(1).gameObject.SetActive(false); //sonraki level butonunu kapatıyoruz oyun sonuna gelindiği için
        }
        else
        {
            if (nextLevel - currentLevelID == 1) 
            PlayerPrefs.SetInt("level", nextLevel);
            else
            Debug.Log("Önceden Açılmış bir bölüme girdiniz.");

            bolumSonu_P.transform.GetChild(1).gameObject.SetActive(true); // sonraki level butonu aktif(eğer son bolüme kadar gidip tekrar onceki bolumlere girerse diye aktif hale getiriyoruz.)          
        }

        bolumSonuPanel(); //bolum sonu panel işlemlerini başlat
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

    SahneGecis sahneGecis;
    private void Start()
    {
        sahneGecis = GameObject.Find("GameManager").GetComponent<SahneGecis>();
    }

    private void bolumSonuPanel()
    {
        bolumSonu_P.SetActive(true);//Panel Aç
        for (int i = 0; i < toplananItem; i++)///Kazandığı kadar yıldızın alpha değerlerini 255 yap
        {
            bolumSonu_P.transform.GetChild(0).GetChild(i).GetComponent<Image>().color = new Color(255, 255, 255, 255); //Bu nokta önemli yıldızların panelin ilk çocuğunun çocukları(yildizlar_G->yildiz1/yildiz2/yildiz_3) olduğuna dikkat edin!
        }
    }

    string levelAdi(int id)//id den level'in ismini döndürüyor
    {
        string scenePath = SceneUtility.GetScenePathByBuildIndex(id);
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
        return sceneName;
    }

    public void SonrakiLevel()//Sonraki Level Butonu
    {
        sahneGecis.SahneDegistir(levelAdi(SceneManager.GetActiveScene().buildIndex+1));
    }

    public void TekrarOyna()//Tekrar Oyna Butonu
    {
        sahneGecis.SahneDegistir(levelAdi(SceneManager.GetActiveScene().buildIndex));
    }

}
