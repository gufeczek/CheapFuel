package com.example.fuel.utils

import android.Manifest
import android.annotation.SuppressLint
import android.content.Context
import android.location.Location
import android.location.LocationManager
import com.example.fuel.ui.utils.permission.allPermissionsGranted

private val locationPermissions: Array<String> = arrayOf(
    Manifest.permission.ACCESS_COARSE_LOCATION,
    Manifest.permission.ACCESS_FINE_LOCATION)

fun isGpsEnabled(context: Context): Boolean {
    val locationManager = context.getSystemService(Context.LOCATION_SERVICE) as LocationManager

    return locationManager.isProviderEnabled(LocationManager.GPS_PROVIDER)
            || locationManager.isProviderEnabled(LocationManager.NETWORK_PROVIDER)
}

@SuppressLint("MissingPermission")
fun getUserLocation(context: Context): Location? {
    val locationManager = context.getSystemService(Context.LOCATION_SERVICE) as LocationManager

    if (!allPermissionsGranted(context, locationPermissions)) return null

    val locationGPS = locationManager.getLastKnownLocation(LocationManager.GPS_PROVIDER)
    val locationNet = locationManager.getLastKnownLocation(LocationManager.NETWORK_PROVIDER)

    var gpsLocationTime: Long = 0

    if (locationGPS != null) {
        gpsLocationTime = locationGPS.time
    }

    var netLocationTime: Long = 0

    if (locationNet != null) {
        netLocationTime = locationNet.time
    }

    return if (0 < gpsLocationTime - netLocationTime) locationGPS else locationNet
}

fun calculateDistance(startLatitude: Double, startLongitude: Double, endLatitude: Double, endLongitude: Double): Float {
    val results = FloatArray(3)
    Location.distanceBetween(startLatitude, startLongitude, endLatitude, endLongitude, results)
    return results[0]
}