using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;          //Меню менеджер - singleton
                                                    //Гарантирует, что у класса есть только один экземпляр, 
                                                    // и предоставляет к нему глобальную точку доступа
    [SerializeField] private List<Menu> _menus;  //Лист менюшек
                                                    // в который мы влаживаем все менюшки для дальшейшего использования
    public void OpenMenu(string menuName)        //Функция по открывания менюшек
    {                                               //Цикл foreach во время работы с массивами и коллекциями выполняет итерацию по их элементам
        foreach (Menu menu in _menus)               //Цикл по всем менюшкам
        {                                           //
            if (menu.menuName == menuName)          //Проверяем, есть ли совпадения с названием менюшки (что задаётся в Menu.cs) в массиве менюшек
            {                                       //
                menu.Open();                        //Если есть совпадение - то открывем
            }                                       //
            else                                    //
            {                                       //
                menu.Close();                       //Если название не совападает - то не открывем
            }                                       //
        }                                           //
    }

    private void Awake()                        //Функция для использования MenuManager из любого скрипта
    {                                               //Назначаем до старта игры для использования "loading"
        instance = this;                            //С помощью instance открывает любую менюшку что нам нужна 
    }                                               //

}
