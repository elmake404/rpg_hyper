using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField]
    private List<Image> _listAbilitiImage;
    void Start()
    {

    }

    void Update()
    {

    }
    public void ActivationAbiliti(int namberTegAbiliti,HeroControl heroMain , out Image image)
    {
        image = null;

        for (int i = 0; i < _listAbilitiImage.Count; i++)
        {
            if (_listAbilitiImage[i].tag == namberTegAbiliti.ToString())
            {
                _listAbilitiImage[i].gameObject.SetActive(true);
                Image img = _listAbilitiImage[i].transform.GetChild(0).GetComponent<Image>();
                IActiveAbility active = _listAbilitiImage[i].gameObject.GetComponent<IActiveAbility>();
                if (active!=null)
                {
                    active.initializationHero(heroMain, img);
                }
                else if(image == null)
                image = img;
            }
        }
    }
    public void DeactivationAbiliti(int namberTegAbiliti)
    {
        for (int i = 0; i < _listAbilitiImage.Count; i++)
        {
            if (_listAbilitiImage[i].tag == namberTegAbiliti.ToString())
            {
                _listAbilitiImage[i].gameObject.SetActive(false);
                //return;
            }
        }
    }
}
