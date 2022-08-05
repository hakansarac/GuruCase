using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private AudioClip m_Clip;
    private int m_Samplerate = 44100;
    private float m_Frequency = 300;

    void Start()
    {
        m_Clip = AudioClip.Create("ComboClip", m_Samplerate*2, 1, m_Samplerate, false);
        AudioSource audSource = GetComponent<AudioSource>();
        CreateClip(m_Clip);
    }

    void CreateClip(AudioClip _clip)
    {
        var size = _clip.frequency * (int)Mathf.Ceil(_clip.length);
        float[] data = new float[size];

        int count = 0;
        while (count < data.Length)
        {
            data[count] = Mathf.Sin(2 * Mathf.PI * m_Frequency * count / m_Samplerate);
            count++;
        }

        m_Clip.SetData(data, 0);
    }

    private void OnEnable()
    {
        StackManager.PlayAudioClip += PlayClip;
    }

    private void OnDisable()
    {
        StackManager.PlayAudioClip -= PlayClip;
    }

    private void PlayClip(int combo)
    {
        StartCoroutine(PlayDuration(combo));
    }

    private IEnumerator PlayDuration(int combo)
    {
        AudioSource audSource = GetComponent<AudioSource>();
        audSource.pitch = 1 + combo*0.1f;
        audSource.PlayOneShot(m_Clip);
        yield return new WaitForSeconds(3f*Time.deltaTime);
        audSource.Stop();
    }
}
