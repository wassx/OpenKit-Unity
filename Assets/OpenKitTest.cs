using System.Collections.Generic;
using System.Threading.Tasks;
using Dynatrace.OpenKit.API;
using UnityEngine;
using UnityEngine.UI;
using XRTK.Services;

public class OpenKitTest : MonoBehaviour {


    [SerializeField] private Button _startActionBtn;
    [SerializeField] private Button _startTaskBtn;
    [SerializeField] private Button _startAction2Btn;
    private IRootAction _rootAction;

    // Start is called before the first frame update
    void Start() {
        _rootAction = MixedRealityToolkit.GetService<OpenKitService>().EnsureRootAction("Test Root Action");
        _startActionBtn.onClick.AddListener(OnActionButtonClicked);
        _startAction2Btn.onClick.AddListener(OnActionButton2Clicked);
        _startTaskBtn.onClick.AddListener(OnStartTaskButtonClicked);
        _rootAction.ReportEvent("Added button listeners");
    }

    private async void OnActionButton2Clicked() {
        IAction enterAction = _rootAction.EnterAction("Action 2");
        await Task.Delay(500);
        enterAction.ReportEvent("Done some work.");
        enterAction.LeaveAction();
    }

    private void OnDestroy() {
        _rootAction.ReportEvent("OnDestroy").LeaveAction();
    }

    private async void OnStartTaskButtonClicked() {
        await Task.Run(async () => {
            IAction taskAction = _rootAction.EnterAction("Task action start.");
            await Task.Delay(3000);
            taskAction.ReportEvent("Task delayed 3s.").LeaveAction();
        });
    }

    private void OnActionButtonClicked() {
        List<string> list = new List<string>();
        string test = list[3]; //throw exception
    }

}