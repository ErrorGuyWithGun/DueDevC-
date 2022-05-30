using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    //                    ┌──────────┐
    //                    │ Menu.cs  │
    // ┌───┬──────────────┼──────────┤
    // │П  │public string │ menuName │
    // │о  ├──────────────┴──────────┘
    // │л  │
    // │я  │
    // │   │
    // │К  │
    // │л  │
    // │а  │
    // │с  │
    // │с  │
    // │а  │
    // │   │
    // ├───┼────────────────┬───────┐
    // │М  │public void     │Open() │
    // │е  ├────────────────┼───────┤
    // │т  │public void     │Close()│
    // │о  ├────────────────┴───────┘
    // │д  │
    // │ы  │
    // │   │
    // │К  │
    // │л  │
    // │а  │
    // │с  │
    // │с  │
    // │а  │
    // │   │
    // └───┘


    public string  menuName;           //Название меню
                                            //Используется в MenuManager.cs для вызова нужного меню
    public void Open()                 //Функция открывающая меню
    {                                       //
        gameObject.SetActive(true);         //Показывает меню
    }                                       //
                                            //
                                            //
    public void Close()                //Функция закрывающая меню
    {                                       //
        gameObject.SetActive(false);        //Скрывает меню
    }                                       //
}
