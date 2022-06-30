using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingArguments : MonoBehaviour
{
    [SerializeField]
    private GameObject mainPanel, instructionsPanel, configPanel;

    [SerializeField]
    private GameObject tutorial;

    [SerializeField]
    private UnityEngine.UI.Text roomNameInputText;

    [SerializeField]
    private UnityEngine.UI.Text instructionsText;


    public static StartingArguments Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        StartingArguments.Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        mainPanel.SetActive(true);
        instructionsPanel.SetActive(false);
        configPanel.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            StartClient();
        }
    }



    private static string[] arguments = new[] { "-s", "false" };
    public static string[] GetArguments()
    {
        return arguments;
    }




    public void StartClient()
    {
        StartingArguments.arguments = new[] { "-s", "false" };
        SceneManager.LoadScene("Start");
    }

    private void SwitchPanel(GameObject g)
    {
        mainPanel.SetActive(false);
        instructionsPanel.SetActive(false);
        configPanel.SetActive(false);
        tutorial.SetActive(false);
        g.SetActive(true);
    }

    public void GoToInstructions()
    {
        SwitchPanel(instructionsPanel);
    }

    public void BackFromInstructions()
    {
        SwitchPanel(mainPanel);
    }

    public void GoToConfigServer()
    {
        SwitchPanel(configPanel);
    }

    public void BackFromConfigServer()
    {
        SwitchPanel(instructionsPanel);
    }

    public void StartServer()
    {
        StartingArguments.arguments = new[] { "-s", "true", "-r", roomNameInputText.text };
        SceneManager.LoadScene("Start");
    }

    public void English()
    {
        instructionsText.text =
            "INSTRUCTIONS\n\n"+
            "1.The name of the chosen room will be the name that the other players(Clients) must enter in their games.\n\n" +
            "2.If you want to play the game, in addition to being the server, you will need to start the game a second time and choose \"Client\" from the previous menu.\n\n" +
            "3.To terminate the server at any time, simply close this window.";
    }
    
    public void Portuguese()
    {
        instructionsText.text =
            "INSTRUÇÕES\n\n" +
            "1.O nome da sala escolhida será o nome que os demais jogadores(Clientes) deverão inserir em seus jogos.\n\n" +
            "2.Se você quiser jogar o jogo, além de ser o servidor, você precisará iniciar o jogo uma segunda vez e escolher \"Cliente\" no menu anterior.\n\n" +
            "3.Para encerrar o servidor a qualquer momento, basta fechar esta janela.";
    }

    public void StartTutorial()
    {
        SwitchPanel(tutorial);
    }
}
