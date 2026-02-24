using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPausePanel : MonoBehaviour, IDependency<Pauser>
{
    [SerializeField] private GameObject panel;

    private Pauser pauser;
    public void Construct(Pauser obj) => pauser = obj;

    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
        pauser.PauseStateChange += OnPauseStateChanged;
    }
    private void OnDestroy()
    {

        pauser.PauseStateChange -= OnPauseStateChanged;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauser.ChangePauseState();
        }
    }

    private void OnPauseStateChanged(bool isPause)
    {
        panel.SetActive(isPause);
    }

    public void Unpause()
    {
        pauser.UnPause();
    }
}
