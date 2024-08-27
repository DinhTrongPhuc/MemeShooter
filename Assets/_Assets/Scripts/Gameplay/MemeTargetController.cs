using MemeShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemeTargetController : MonoBehaviour
{
    public MemeDamageHandler MemeDamageHandler;

    public Vector2 activeDelay;
    float curActiveDelay = 0f;

    public Vector2 stayDelay;
    float curStayDelay = 0f;

    bool show = false;
    bool stay = false;
    bool hide = false;

    public Transform fromPoint;
    public Transform toPoint;

    //[SerializeField] Transform[] waypoints;

    [SerializeField] float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        MemeDamageHandler.OnDeadEvent.AddListener(Setup);

        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        ToShow();
        ToHide();
    }

    public void Setup()
    {
        curActiveDelay = Random.Range(activeDelay.x, activeDelay.y);

        curStayDelay = Random.Range(stayDelay.x, stayDelay.y);

        show = false;
        stay = false;
        hide = false;

        MemeDamageHandler.Setup();

        MemeDamageHandler.gameObject.SetActive(false);

        MemeDamageHandler.transform.position = fromPoint.position;
    }

    void ToShow()
    {
        if (stay) return;

        if (show == false && curActiveDelay > 0f)
        {
            curActiveDelay -= Time.deltaTime;

            if (curActiveDelay <= 0f)
            {
                show = true;

                MemeDamageHandler.gameObject.SetActive(true);
            }
        }

        if (show && stay == false)
        {
            MemeDamageHandler.transform.position = Vector3.MoveTowards(MemeDamageHandler.transform.position, toPoint.position, speed * Time.deltaTime);

            //foreach (Transform point in waypoints)
            //{
            //    if (point != null)
            //    {
            //        MemeDamageHandler.transform.position = Vector3.MoveTowards(MemeDamageHandler.transform.position, point.position, speed * Time.deltaTime);

            //    }


            //}

            // nếu vị trí của meme = vị trí của điểm đến cuối cùng của waypoints trong mảng 
            if (MemeDamageHandler.transform.position == toPoint.position)
            {
                stay = true;
            }
        }
    }

    void ToHide()
    {
        if (stay == false) return;

        if (hide == false && curStayDelay > 0f)
        {
            curStayDelay -= Time.deltaTime;

            if (curStayDelay <= 0f)
            {
                hide = true;
            }
        }

        if (hide)
        {
            MemeDamageHandler.transform.position = Vector3.MoveTowards(MemeDamageHandler.transform.position, fromPoint.position, speed * Time.deltaTime);

            if (MemeDamageHandler.transform.position == fromPoint.position)
            {
                Setup();
            }
        }
    }
}
