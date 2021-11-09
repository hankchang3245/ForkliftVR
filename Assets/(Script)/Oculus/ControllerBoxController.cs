/************************************************************************************

Copyright (c) Facebook Technologies, LLC and its affiliates. All rights reserved.  

See SampleFramework license.txt for license terms.  Unless required by applicable law 
or agreed to in writing, the sample code is provided “AS IS” WITHOUT WARRANTIES OR 
CONDITIONS OF ANY KIND, either express or implied.  See the license for specific 
language governing permissions and limitations under the license.

************************************************************************************/

using UnityEngine;
using UnityEngine.Assertions;
using OculusSampleFramework;
using edu.tnu.dgd.debug;

namespace Oculus
{
	public class ControllerBoxController : MonoBehaviour
	{
		//[SerializeField] private TrainLocomotive _locomotive = null;
		//[SerializeField] private CowController _cowController = null;

		private void Awake()
		{
			//Assert.IsNotNull(_locomotive);
			//Assert.IsNotNull(_cowController);

		}
		
		public void ShowMainMenu()
        {
			//ShowDebugLog.instance.Log("ShowMainMenu..........", true);
        }
	}
}
