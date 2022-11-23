package com.example.fuel.ui.utils.permission

import android.content.Context
import android.content.pm.PackageManager
import androidx.core.content.ContextCompat

fun allPermissionsGranted(context: Context, permissions: Array<String>): Boolean {
    val allGranted = permissions.all {
        ContextCompat.checkSelfPermission(context, it) == PackageManager.PERMISSION_GRANTED
    }

    return allGranted
}