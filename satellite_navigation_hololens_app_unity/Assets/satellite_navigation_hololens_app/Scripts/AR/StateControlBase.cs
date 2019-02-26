using System.Collections;
using Helpers;

namespace Controllers.ARScene
{
	public abstract class StateControlBase : MonoBehaviourWrapper
	{
		protected void Init()
		{
			base.Disable();
		}

		public virtual IEnumerator EnterState()
		{
			base.Enable();
			yield return null;
		}

		public virtual IEnumerator ExitState()
		{
			base.Disable();
			yield return null;
		}
		
		public void StartProcessing()
		{
			OnStartProcessing();
		}

		public sealed override void Enable() { }

		public sealed override void Disable() { }

		protected abstract void OnStartProcessing();

		protected void ActiveState<T>() where T : StateControlBase
		{
            //TODO 
			//GetSceneControler<StateMachineSceneControlBase>().ActiveState<T>();
		}
	}
}
