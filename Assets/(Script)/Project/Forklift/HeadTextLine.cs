using UnityEngine;
using TMPro;
using UnityEngine.Assertions;


namespace edu.tnu.dgd.project.forklift
{
    public class HeadTextLine : MonoBehaviour
    {
        private TMP_Text line1;
        private TMP_Text line2;

        private void Awake()
        {
            Assert.IsNotNull(line1);
            Assert.IsNotNull(line2);
        }
        public void UpdateHeadTextLine1(string mission, int percentage)
        {
            line1.text = "任務：" + mission + "    完成：" + percentage + "%";
        }

        public void UpdateHeadTextLine2(int time, int failed)
        {
            int min = Mathf.FloorToInt(time / 60);
            int sec = time - min * 60;
            string str = "" + min + ":" + sec.ToString().PadLeft(2, '0');
            line2.text = "時間：" + str + "    失誤：" + failed;
        }
    }
}
