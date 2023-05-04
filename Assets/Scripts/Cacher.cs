using Unity.VisualScripting;
using UnityEngine;

    //TODO : �ֿ� Ŭ�������� ȣ�� Ÿ�̹��� ��Ȯ�ϰ� �������� ���� ���� ����.
    //       �ʱ�ȭ ������ ��Ȯ�ϰ� �����ϰ�, �ʱ�ȭ�� �Ϸ�Ǿ����� Ȯ���ϴ� ����� ã�ƾ���.
    //       �� ���� Ŭ������ ���� Getter�� FindObjectOfType ȣ���� �ּ�ȭ �ϱ� ���� �ӽù���.
    public static class Cacher
    {
        static UIManager ui;
        static DataManager dm;

        static Cacher()
        {
            dm = GameObject.FindObjectOfType<DataManager>();
            ui = GameObject.FindObjectOfType<UIManager>();
        }

        public static UIManager uiManager
        {
            get
            {
                return ui;
            }
        }

        public static DataManager dataManager
        {
            get
            {
                return dm;
            }
        }
    }