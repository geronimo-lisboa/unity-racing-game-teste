using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// A função desse script é controlar qual interface de usuário mostrar de acordo com a plataforma
/// em que o jogo está rodando.
/// </summary>
public class GuiChooserByPlaform : MonoBehaviour {
    public GameObject AndroidGUI;
    public GameObject WebGLGUI;
	// Use this for initialization
	void Start () {
        AndroidGUI.SetActive(false);
        WebGLGUI.SetActive(false);
		if (Application.platform==RuntimePlatform.WebGLPlayer || Application.platform ==RuntimePlatform.WindowsEditor)
        {
            //TODO: Se webgl mostrar a interface de webgl
            WebGLGUI.SetActive(true);
            Debug.Log("VERSÃO WEBGL");
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            //TODO: Se android, mostrar a interface de android
            AndroidGUI.SetActive(true);
            Debug.Log("VERSÃO ANDROID");
        }
        else
        {
            throw new Exception("NÃO É UMA PLATAFORMA SUPORTADA");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
