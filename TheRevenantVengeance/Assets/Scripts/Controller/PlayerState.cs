using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public static class PlayerState
    {
        public static bool acquiredSwordSpin = false;

        public static bool acquiredFireball = false;
        public static bool acquiredAura = false;

        public static GameObject savedFireballPrefab = null;
        public static GameObject savedAuraPrefab = null;
    }
}
