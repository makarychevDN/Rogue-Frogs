﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAttackable : MonoBehaviour
{
    public abstract void Attack();
    public abstract void CheckAttackIsPossible();
}
