using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;
    [SerializeField] private CharacterView characterView;

    [SerializeField] private AccountSignIn signInPanel;
    [SerializeField] private AccountSignUp signUpPanel;

    [SerializeField] private Button signInBtn;
    [SerializeField] private Button signUpBtn;

    private PlayFabCharacter _character;

    public void Start()
    {
        _character = new PlayFabCharacter(characterView);

        GoBack();

        signInBtn.onClick.AddListener(SignIn);
        signInBtn.onClick.AddListener(HideStart);

        signUpBtn.onClick.AddListener(SignUp);
        signUpBtn.onClick.AddListener(HideStart);

        signInPanel.OnSuccess += ChooseCharacter;
        signInPanel.OnBack += GoBack;
        signUpPanel.OnBack += GoBack;

        _character.OnCharSelected += NextScene;
    }

    private void OnDestroy()
    {
        signInBtn.onClick.RemoveAllListeners();
        signUpBtn.onClick.RemoveAllListeners();
        signInPanel.OnSuccess -= ChooseCharacter;
        signInPanel.OnBack -= GoBack;
        signUpPanel.OnBack -= GoBack;
        _character.OnCharSelected -= NextScene;
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

    private void ChooseCharacter() => _character.SelectCharacter();

    private void NextScene() => SceneManager.LoadScene("LobbyScene");
}