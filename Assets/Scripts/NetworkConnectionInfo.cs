using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NetworkConnectionInfo
{
    ///<summary>
    ///ȣ��Ʈ�� ���� ����
    /// </summary>
    public bool Host;
    ///<summary>
    ///Ŭ���̾�Ʈ�� ����� ������ ȣ��Ʈ�� IP �ּ�
    /// </summary>
    public string IPAddress;
    /// <summary>
    /// Ŭ���̾�Ʈ�� ����� ������ ȣ��Ʈ�� Port
    /// </summary>
    public int Port;
}
