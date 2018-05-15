using UnityEngine;
using System.Collections;

public class TrajectoryPreview : MonoBehaviour {

    public Transform from;
    public Transform to;
    public GameObject go;

    public string data;
    private TrajectoryObject trajectoryObj;

    void Start()
    {
        ConfigManager.CreateInstance();
        TrajectoryManager.CreateInstance();
    }

    public void Play()
    {
        if (trajectoryObj == null && !string.IsNullOrEmpty(data) && from != null && to != null && go != null)
        {
            var cfgLoader = ConfigManager.Get<TrajectoryCfgLoader>();
            cfgLoader.SetTempConfig(-1, new TrajectoryCfg() { ID = -1, Data = data });

            trajectoryObj = TrajectoryManager.Play(-1, from.position, to.position);
        }
    }

    void Update()
    {
        if (trajectoryObj != null)
        {
            trajectoryObj.AlignTransform(go.transform);

            if (trajectoryObj.Finished)
            {
                trajectoryObj.Stop();
                trajectoryObj = null;
                return;
            }
        }

        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    Play();
        //}
    }


}
