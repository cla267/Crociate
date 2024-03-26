using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Button leftArrow;
    public Button rightArrow;
    public GameObject ImageHolder;
    public GameObject SlidesHolder;
    public Button leftSlide;
    public Button rightSlide;
    public float speed = 0.2f;
    public TextMeshProUGUI Title;
    
    int currentIndex = 0;
    int currentSlide = 0;
    GameObject[] images;
    GameObject[] slides;
    GameObject Elements;

    void Start()
    {
        images = new GameObject[ImageHolder.transform.childCount];
        slides = new GameObject[SlidesHolder.transform.childCount];
        for(int i = 0; i < images.Length; i++) images[i] = ImageHolder.transform.GetChild(i).gameObject;
        for(int i = 0; i < slides.Length-1; i++) {slides[i] = SlidesHolder.transform.GetChild(i).gameObject; slides[i].SetActive(false);}
        Elements = SlidesHolder.transform.GetChild(SlidesHolder.transform.childCount-1).gameObject;
        Elements.SetActive(false);
        StartCoroutine(Reload(-1));
    }

    void Update()
    {
        Title.text = images[currentIndex].name;
    }

    public void Open()
    {
        Elements.SetActive(true);
        slides[currentIndex].SetActive(true);
        currentSlide = 0;
        slides[currentIndex].transform.GetChild(currentSlide).gameObject.SetActive(true);
        leftSlide.interactable = false;
    }
    public void Close()
    {
        Elements.SetActive(false);
        slides[currentIndex].SetActive(false);
        rightSlide.interactable = true;
    }
    public void Left()
    {
        currentSlide--;
        slides[currentIndex].transform.GetChild(currentSlide+1).gameObject.SetActive(false);
        slides[currentIndex].transform.GetChild(currentSlide).gameObject.SetActive(true);
        if(currentSlide == 0) leftArrow.interactable = false;
        rightSlide.interactable = true;
    }
    public void Right()
    {
        currentSlide++;
        slides[currentIndex].transform.GetChild(currentSlide-1).gameObject.SetActive(false);
        slides[currentIndex].transform.GetChild(currentSlide).gameObject.SetActive(true);
        if(currentSlide == slides[currentIndex].transform.childCount-1) rightSlide.interactable = false;
        leftSlide.interactable = true;
    }

    public void Next() {currentIndex++; StartCoroutine(Reload(1));}
    public void Previous() {currentIndex--; StartCoroutine(Reload(-1));}

    IEnumerator Reload(int dir)
    {
        leftArrow.interactable = true;
        rightArrow.interactable = true;
        if(currentIndex == 0) leftArrow.interactable = false;
        if(currentIndex == images.Length-1) rightArrow.interactable = false;
        var tempColor = images[0].GetComponent<Image>().color;
        Image image;
        for(int t = 0; t < images.Length; t++)
        {
            int i = (dir == 1)? t: images.Length-1-t;
            int weight = i-currentIndex;
            RectTransform trans = images[i].GetComponent<RectTransform>();
            image = images[i].gameObject.GetComponent<Image>();
            Vector3 prevPos = trans.localPosition;
            Vector3 newPos = Vector3.one;
            Vector3 prevScal = trans.localScale;
            Vector3 newScal = Vector3.one;
            float prevAlpha = image.color.a;
            float newAlpha;
            switch(Mathf.Abs(weight))
            {
                case 0:
                    images[i].gameObject.SetActive(true);
                    newPos = Vector3.zero;
                    newScal = Vector3.one * 0.3f;
                    newAlpha = 1f;
                    break;
                case 1:
                    images[i].gameObject.SetActive(true);
                    newPos = new Vector3(250 * weight/Mathf.Abs(weight), 0, 0);
                    newScal = new Vector3(0.75f, 0.75f, 0.75f) * 0.3f;
                    newAlpha = 0.4f;
                    break;
                case 2:
                    images[i].gameObject.SetActive(true);
                    newPos = new Vector3(425 * weight/Mathf.Abs(weight), 0, 0);
                    newScal = Vector3.one/2 * 0.3f;
                    newAlpha = 0.1f;
                    break;
                default:
                    images[i].gameObject.SetActive(false);
                    trans.localPosition = Vector3.one * 1000 * dir;
                    newScal = Vector3.one * 0.3f;
                    newAlpha = 0f;
                    break;
            }
            for(float j = 1/speed; j <= 1; j+=1/speed)
            {
                trans.localPosition = new Vector3(
                                Mathf.Lerp(prevPos.x, newPos.x, j),
                                Mathf.Lerp(prevPos.y, newPos.y, j),
                                Mathf.Lerp(prevPos.z, newPos.z, j)
                                );
                trans.localScale = new Vector3(
                                Mathf.Lerp(prevScal.x, newScal.x, j),
                                Mathf.Lerp(prevScal.y, newScal.y, j),
                                Mathf.Lerp(prevScal.z, newScal.z, j)
                                );
                tempColor = image.color;
                tempColor.a = Mathf.Lerp(prevAlpha, newAlpha, j);
                image.color = tempColor;
                yield return null;
            }
        }
    }
}