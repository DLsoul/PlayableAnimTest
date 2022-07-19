using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Slate
{
	public static class MyCommands
	{
        [MenuItem("Tools/ParadoxNotion/SLATE/Create/New SkillSequence", false, 500)]
        public static ActSequence CreateCutscene()
        {
            var cutscene = ActSequence.Create();
            ActEditor.ShowWindow(cutscene);
            Selection.activeObject = cutscene;
            return cutscene;
        }
    }
}
