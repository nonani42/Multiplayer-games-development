using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;

    [SerializeField] private AccountSignIn signInPanel;
    [SerializeField] private AccountSignUp signUpPanel;

    [SerializeField] private Button signInBtn;
    [SerializeField] private Button signUpBtn;

    public void Start()
    {
        GoBack();

        signInBtn.onClick.AddListener(SignIn);
        signInBtn.onClick.AddListener(HideStart);

        signUpBtn.onClick.AddListener(SignUp);
        signUpBtn.onClick.AddListener(HideStart);

        signInPanel.OnSuccess += NextScene;
        signInPanel.OnBack += GoBack;
        signUpPanel.OnBack += GoBack;
    }

    private void OnDestroy()
    {
        signInBtn.onClick.RemoveAllListeners();
        signUpBtn.onClick.RemoveAllListeners();
        signInPanel.OnSuccess -= NextScene;
        signInPanel.OnBack -= GoBack;
        signUpPanel.OnBack -= GoBack;
    }

    private void SignIn() => signInPanel.Show();

    private void SignUp() => signUpPanel.Show();

    private void HideStart() => startPanel.SetActive(false);

    private void GoBack()
    {
        startPanel.SetActive(true);
        signUpPanel.Hide();
        signInPanel.Hide();
    }

    private void NextScene() => SceneManager.LoadScene("LobbyScene");
}