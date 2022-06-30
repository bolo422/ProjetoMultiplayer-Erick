using System.Collections;
using UnityEngine;


public static class TutorialTexts
{
    private static string[] PT_tutorials = new string[] { 
        "O chão quadriculado à direita é o \"Local de Entrega\"" ,
        "As caixas que possuem marcadores (Tags) devem ser levadas até os Locais de Entrega",
        "O \"Local de Entrega\" correto pode ser identificado pela cor que estiver escrita no bloco flutuante",
        "As caixas entregues nos locais incorretos lhe concederão progresso, mas haverá penalidade na pontuação (feature não implementada ainda)",
        "Deve fazer tudo antes do tempo acabar! (feature não implementada ainda)"
    };

    private static string[] EN_tutorials = new string[] {
        "The checkered floor on the right is the \"Delivery Location\"",
        "Boxes that have tags (Tags) must be taken to the Delivery Locations",
        "The correct \"Delivery Location\" can be identified by the color that is written in the floating block",
        "Crates delivered to incorrect locations will grant you progress, but there will be a score penalty (feature not implemented yet)",
        "Must do everything before time runs out! (feature not implemented yet)"
    };

    private static Language lang = Language.portuguese;

    public static string GetTutorial(int index)
    {
        if(index < PT_tutorials.Length && index >= 0)
            return lang == Language.portuguese ? PT_tutorials[index] : EN_tutorials[index];
        return null;
    }

    public static void SwitchLanguage()
    {
        lang = lang == Language.portuguese ? Language.english : Language.portuguese;
    }

    public static int LastIndex()
    {
        return PT_tutorials.Length-1;
    }
}
