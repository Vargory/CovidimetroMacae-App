using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Transition;
using Lean.Transition.Method;
using Lean.Transition.Examples;
using System;
using UnityEditor;

public class FillMeter : MonoBehaviour
{
    public WebScrap script;
    

    [Header("Range Converter [to 0 - 1]")]
    public float _repInicial;
    public float _max;
    public float _min;

    [Header("Resultado")]
    public float _repFinal;

    [Header("Animação")]
    public float increase = 0.01f;
    public float duration = 0.001f;

    public void Start()
    {
        StartCoroutine(fillUpdate());
    }

    [Header("Image")]
    public Image _fillAmount;
    public Image _fill;

    [Header("Transitions")]
    public LeanPlayer _riscoBaixo;
    public LeanPlayer _riscoModerado;
    public LeanPlayer _riscoAlto;
    public LeanPlayer _riscoMuitoAlto;  

    // Update is called once per frame
    public void Update()
    {
        _repInicial = Convert.ToSingle(script.numReproducao);

        if (_repInicial < 100)
        {
            _repInicial = _repInicial / 10;
        }else if(_repInicial >= 100)
        {
            _repInicial = _repInicial / 100;
        }

        _repFinal = (_repInicial - _min) / (_max - _min);
        

        //t += Time.deltaTime / duration;
        //_fillAmount.fillAmount = Mathf.Lerp(minimum, _repFinal, t);

        if ( _fillAmount.fillAmount < .25f)
        {
            _riscoBaixo.Begin();
        }
        else if (_fillAmount.fillAmount > .25f && _fillAmount.fillAmount < .50f)
        {
             _riscoModerado.Begin();
            
        }else if (_fillAmount.fillAmount > .50f && _fillAmount.fillAmount< .75f)
        {
            _riscoAlto.Begin();
        }
        else if (_fillAmount.fillAmount > .75f)
        {
            _riscoMuitoAlto.Begin();
        }
    }

    public IEnumerator fillUpdate()
    {
        while (true)
        {
            if (_fillAmount.fillAmount <= _repFinal)
            {
                _fillAmount.fillAmount = _fillAmount.fillAmount + increase; //Increment the display score by 1I
            }
            yield return new WaitForSeconds(duration);
        }
    }
}
