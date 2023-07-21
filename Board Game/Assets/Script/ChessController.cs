
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChessController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody[] rigidbody;
    public List<touchLocation> touches = new List<touchLocation>();
    public ParticleSystem goodParticle;
    public ParticleSystem awesomeParticle;
    public ParticleSystem exellentParticle;
    public ParticleSystem deathParticle;
    private List<ParticleSystem> listPS = new List<ParticleSystem>();
    
    private int player1 = 0;
    private int player2 = 0;

    private int count = 0;
    private int indexEff1;
    private int indexEff2;

    public GameObject gameOverPanel;
    public Button tryAgianBtn;
    public Button menuBtn;
    public Text panelText;

    private Vector3 _position = new Vector3(0f, 0f, 0f);
    void Awake(){
        count = 5;
        indexEff2 = 0;
        indexEff1 = 0;
        goodParticle.Stop();
        awesomeParticle.Stop();
        deathParticle.Stop();
        exellentParticle.Stop();
        listPS.Add(goodParticle);
        listPS.Add(awesomeParticle);
        listPS.Add(exellentParticle);
    }
    void Update(){
        resetRotation(listPS);
        int i = 0;
        while (i < Input.touchCount){
            Touch t = Input.GetTouch(i);
            if (t.phase == TouchPhase.Began){
                Ray ray = Camera.main.ScreenPointToRay(t.position);
                RaycastHit hit;
                bool a = true;
                if (Physics.Raycast(ray, out hit)){
                    for (int j = 0; j < rigidbody.Length; j++) {
                        if (hit.transform.name == rigidbody[j].name){
                            touches.Add(new touchLocation(t.fingerId, rigidbody[j]));
                            a = false;
                            break;
                        }
                    }
                    if (a) {
                        touches.Add(new touchLocation(t.fingerId, null));
                    }
                }
            } else if (t.phase == TouchPhase.Ended){
                touchLocation thisTouch = touches.Find(touchLocation => touchLocation.touchId == t.fingerId);
                touches.RemoveAt(touches.IndexOf(thisTouch));              
            } else if (t.phase == TouchPhase.Moved){
                touchLocation thisTouch = touches.Find(touchLocation => touchLocation.touchId == t.fingerId);
                if (thisTouch.rb != null){
                    float mZCoord = Camera.main.WorldToScreenPoint(thisTouch.rb.transform.position).z;
                    Vector3 mousePoint = t.position;
                    mousePoint.z = mZCoord;
                    Vector3 temp = Camera.main.ScreenToWorldPoint(mousePoint);
                    if (thisTouch.rb.transform.position.z > 0) {
                        if (temp.x >= -4.2f && temp.x <= 4f) {
                            if (temp.z >= 0.6f && temp.z <= 8f) {
                                thisTouch.rb.transform.position = temp;
                            } else {
                                thisTouch.rb.transform.position = new Vector3(temp.x, thisTouch.rb.transform.position.y, thisTouch.rb.transform.position.z);
                        }
                    }  else {
                        if (temp.z >= 0.6f && temp.z <= 8f) { 
                            thisTouch.rb.transform.position = new Vector3(thisTouch.rb.transform.position.x, thisTouch.rb.transform.position.y, temp.z);
                        }
                    }
                    } else {
                        if (temp.x >= -4.2f && temp.x <= 4f) {
                            if (temp.z <= -0.6f && temp.z > -7.8f) {
                                thisTouch.rb.transform.position = temp;
                            } else {
                                thisTouch.rb.transform.position = new Vector3(temp.x, thisTouch.rb.transform.position.y, thisTouch.rb.transform.position.z);
                            }
                        } else if (temp.z <= -0.6f && temp.z >= -7.8f) { 
                            thisTouch.rb.transform.position = new Vector3(thisTouch.rb.transform.position.x, thisTouch.rb.transform.position.y, temp.z);
                        }
                    }
                    
                }
            }
            i++;
        }
        player1 = 0;
        player2 = 0;
        for (int j = 0; j < rigidbody.Length; j++) {
            if (rigidbody[j].transform.position.z > 0){
                player1++;
            } else player2++;
        }
        if (count > player1) {
            indexEff2 = Random.Range(0 , 2);
            listPS[indexEff2].transform.position = new  Vector3(0f, 0f, 1f);
            findText(listPS[indexEff2], 3.14f);
            listPS[indexEff2].GetComponent<AudioSource>().Play();
            listPS[indexEff2].Play();
            count = player1;
        } else if (count < player1) {
            indexEff1 = Random.Range(0 , 2);
            listPS[indexEff1].transform.position = new  Vector3(0f, 0f, -1f);
            listPS[indexEff1].GetComponent<AudioSource>().Play();
            listPS[indexEff1].Play();
            count = player1;
        } 
        if (player1 == 0) {
            deathParticle.transform.position = new  Vector3(0f, 0f, -3f);
            deathParticle.GetComponent<AudioSource>().Play();
            deathParticle.Play();
            showGameOverPanel("Player 1 Win!!!");
        } else if (player2 == 0){
            deathParticle.transform.position = new  Vector3(0f, 0f, 3f);
            deathParticle.GetComponent<AudioSource>().Play();
            deathParticle.Play();
            showGameOverPanel("Player 2 Win!!!");
        }
    }

    IEnumerator WaitAndShowPanel(){
        yield return new WaitForSeconds(2f); // Đợi 1 giây
        gameOverPanel.SetActive(true); // Hiển thị panel game over
    }

    public void showGameOverPanel(string message){
        panelText.text = message;
        StartCoroutine(WaitAndShowPanel());
    }

    public void OnTryAgianButtonClick(){
        SceneManager.LoadScene("GamePlay");
    }

    public void OnMenuButtonClick(){
        SceneManager.LoadScene("Menu");
    }

    void resetRotation(List<ParticleSystem> particles) {
        foreach (ParticleSystem particle in particles) {
            ParticleSystem[] childParticleSystems = particle.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in childParticleSystems)
            {
                if (ps.gameObject.name == "PS_Text"){
                    ps.startRotation = 0f;
                    break;
                }
            }
        }
    }
    void findText(ParticleSystem particle , float rotation){
        ParticleSystem[] childParticleSystems = particle.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in childParticleSystems)
        {
            if (ps.gameObject.name == "PS_Text"){
                ps.startRotation = rotation;
                break;
            }
        }
    } 
}