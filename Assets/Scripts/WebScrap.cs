using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using HtmlAgilityPack;
using TMPro;
using System.Net;
using System;

public class WebScrap : MonoBehaviour
{
    
    //Para seleção dos objetos de texto na Unity
    [Header ("TMP Objects")]
    public TMP_Text m_TotalCasos;
    public TMP_Text m_TotalRec;
    public TMP_Text m_Atendimentos;
    public TMP_Text m_TotalObitos;
    public TMP_Text m_Letalidade;
    public TMP_Text m_Ocupação;
    public TMP_Text m_Reproducao;

    //Caso queira mudar o texto na Unity
    [Header("Textos")]
    public string TotalCasos;
    public string TotalRec;
    public string Atendimentos;
    public string TotalObitos;
    public string Letalidade;
    public string Ocupação;
    public string Reprodução;
    
    //Armazena os dados  
    public int numAtendimentos = 0;
    public int numCasos = 0;
    public int numCasosRec = 0;
    public int numMortes = 0;
    public int numReproducao = 0;
    public int numOcupacao = 0;

    //Armazena os dados em String
    public string strOcupacao = null;
    public string strReproducao = null;
    public string letalidade = null;

    //Armazena o progresso da url
    private float progress;


    //Inicia com o app
    void Start()
    {
        StartCoroutine(WWWRoutine());
    }

    public IEnumerator WWWRoutine()
    {
        //Uso de data do sistema
        DateTime localDate = DateTime.Now;
        int day = localDate.Day;
        int month = localDate.Month;
        int year = localDate.Year;
        int hour = localDate.Hour;
        string monthStr;

        string url;

        //Define que a url muda de acordo com o dia e horário
        //Se for antes de meio-dia, ele utiliza a url do dia anterior
        //Geralmente fazem um novo post até as 10h todos os dias
        if (month < 10)
        {
            monthStr = "0" + month.ToString(); ;
        }
        else
        {
            monthStr = month.ToString(); ;
        }

        if (hour >= 18)
        {
            url = "http://www.macae.rj.gov.br/noticias/leitura/noticia/coronavirus-informe-" + day + monthStr + year.ToString();
            Debug.Log(url);
        }
        else
        {
            url = "http://www.macae.rj.gov.br/noticias/leitura/noticia/coronavirus-informe-" + (day - 1) + monthStr + year.ToString();
            Debug.Log(url);
        }

        //Inicio do WebRequest
        UnityWebRequest www = UnityWebRequest.Get(url);
        var asyncOperation = www.SendWebRequest();

        while (!www.isDone)
        {
            progress = asyncOperation.progress;
            yield return null;
        }

        progress = 1f;
        if (!string.IsNullOrEmpty(www.error))
        {
            //Algum erro add dps corno
        }
        
        //Faz o download do código fonte
        var text = www.downloadHandler.text;

        //Cria um documento HTML e passa o codigo fonte para ele
        var htmlDocument = new HtmlAgilityPack.HtmlDocument();
        htmlDocument.LoadHtml(text);

        //String inicial onde o codigo fonte é armazenado
        string content = string.Empty;

        //Encontra os dados
        foreach (HtmlNode node in htmlDocument.DocumentNode.SelectNodes("//span[@class='" + "noticia-conteudo" + "']"))
        {
            content = node.InnerText;
            content = WebUtility.HtmlDecode(content);
        }

        //Vetor onde o codigo-fonte é separado
        string[] contentSplit = content.Replace(".", "").Split(' ');
        
        //Loop para extração de dados
        for (int i = 0; i <= 150 ; i++)
        {
            bool resultAtendimentos = contentSplit[i].Equals("atendimentos");
            if (resultAtendimentos)
            {
                int.TryParse(contentSplit[i - 1], out numAtendimentos);
                //Debug.Log("Atendimentos: " + numAtendimentos);
            }

            bool resultCasos = contentSplit[i].Equals("casos");
            if (resultCasos)
            {
                int.TryParse(contentSplit[i - 1], out numCasos);
                //Debug.Log("Total de Casos: " + numCasos);
            }

            bool resultCasosRec = contentSplit[i].Equals("recuperados");
            if (resultCasosRec)
            {
                int.TryParse(contentSplit[i - 2], out numCasosRec);
                //Debug.Log("Pacientes Recuperados: " + numCasosRec);
            }

            bool resultLetalidade = contentSplit[i].Equals("letalidade");
            if (resultLetalidade)
            {
                letalidade = contentSplit[i + 1];
                letalidade = letalidade.Replace("O", "");
                //Debug.Log("Letalidade: " + letalidade);
            }

            bool resultObitos = contentSplit[i].Equals("óbitos");
            if (resultObitos)
            {
                int.TryParse(contentSplit[i - 1], out numMortes);
               // Debug.Log("Obitos: " + numMortes);
            }

            bool resultOcupacao = contentSplit[i].Equals("Covid-19,");
            if (resultOcupacao)
            {
                strOcupacao = contentSplit[i + 1];
                strOcupacao = strOcupacao.Replace(";", "").Replace("%", "");
                int.TryParse(strOcupacao, out numOcupacao);
                //Debug.Log("Ocupação : " + numOcupacao);
            }

            bool resultReproducao = contentSplit[i].Equals("vírus");
            if (resultReproducao)
            {
                strReproducao = contentSplit[i + 1];
                strReproducao = strReproducao.Replace(",", "").Replace(";", "");
                int.TryParse(strReproducao, out numReproducao);
                //Debug.Log("Reproducao: " + numReproducao);
            }
        }
        
            //Define os dados em texto
            m_Atendimentos.text = Atendimentos + numAtendimentos.ToString();
            m_TotalCasos.text = TotalCasos + numCasos.ToString();
            m_TotalRec.text = TotalRec + numCasosRec.ToString();
            m_TotalObitos.text = TotalObitos + numMortes.ToString();
            m_Letalidade.text = Letalidade + letalidade;
            m_Ocupação.text = Ocupação + numOcupacao + "%";

    }
}
