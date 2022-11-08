package com.example.fuel.ui.fragment.map

import android.Manifest
import android.app.AlertDialog
import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent
import android.content.IntentFilter
import android.content.res.ColorStateList
import android.graphics.Canvas
import android.graphics.Color
import android.graphics.Paint
import android.graphics.drawable.BitmapDrawable
import android.graphics.drawable.GradientDrawable
import android.graphics.drawable.ShapeDrawable
import android.graphics.drawable.shapes.Shape
import android.location.LocationManager
import android.os.Bundle
import android.provider.Settings
import androidx.preference.PreferenceManager
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.activity.result.contract.ActivityResultContracts
import androidx.core.graphics.drawable.toBitmap
import androidx.navigation.fragment.findNavController
import com.example.fuel.R
import com.example.fuel.databinding.FragmentMapBinding
import com.example.fuel.ui.utils.drawable.FuelStationMarker
import com.example.fuel.ui.utils.permission.allPermissionsGranted
import com.google.android.material.internal.ViewUtils.dpToPx
import org.osmdroid.api.IMapController
import org.osmdroid.bonuspack.location.NominatimPOIProvider
import org.osmdroid.config.Configuration.getInstance
import org.osmdroid.config.IConfigurationProvider
import org.osmdroid.tileprovider.tilesource.TileSourceFactory
import org.osmdroid.util.GeoPoint
import org.osmdroid.views.CustomZoomButtonsController
import org.osmdroid.views.MapView
import org.osmdroid.views.overlay.FolderOverlay
import org.osmdroid.views.overlay.Marker
import org.osmdroid.views.overlay.SpeechBalloonOverlay
import org.osmdroid.views.overlay.mylocation.GpsMyLocationProvider
import org.osmdroid.views.overlay.mylocation.MyLocationNewOverlay

private const val INITIAL_ZOOM = 7.0;
private const val USER_LOCATION_INITIAL_ZOOM = 11.0
private const val MIN_ZOOM = 2.9
private const val MAX_ZOOM = 18.0

class MapFragment : Fragment(R.layout.fragment_map) {
    private val centerOfPoland: GeoPoint = GeoPoint(52.44819702037008, 19.418026355263613);

    private lateinit var binding: FragmentMapBinding
    private lateinit var locationOverlay: MyLocationNewOverlay

    private val requiredPermissions: Array<String> = arrayOf(
        Manifest.permission.WRITE_EXTERNAL_STORAGE)

    private val optionalPermissions: Array<String> = arrayOf(
        Manifest.permission.ACCESS_COARSE_LOCATION,
        Manifest.permission.ACCESS_FINE_LOCATION)

    private val requestPermissionsResultLauncher = registerForActivityResult(
        ActivityResultContracts.RequestMultiplePermissions()) { animateMapToLocation() }

    private val gpsSwitchStateReceiver = object : BroadcastReceiver() {
        override fun onReceive(context: Context?, intent: Intent?) {
            if (LocationManager.PROVIDERS_CHANGED_ACTION == intent!!.action && !isGpsEnabled()) {
                locationOverlay.disableMyLocation()
            }
        }
    }

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        binding = FragmentMapBinding.inflate(inflater, container, false);

        binding.fabCenter.setOnClickListener {
            askToEnableGps()
            animateMapToUserLocation()
        }

        requestPermissionsResultLauncher.launch(optionalPermissions.plus(requiredPermissions))

        configureToolbar()
        configureMapView()

        initLocationOverly()
        centerMapViewOnFixedPoint()
        addMarkers()
        return binding.root
    }

    private fun addMarkers() {
        val marker = Marker(binding.map)
//        marker.title = "Some text here"
//        marker.icon = null
//        marker.position = centerOfPoland
//        marker.setAnchor(Marker.ANCHOR_CENTER, Marker.ANCHOR_BOTTOM)
//        marker.setSnippet("The White House is the official residence and principal workplace of the President of the United States.");
//        marker.setSubDescription("1600 Pennsylvania Ave NW, Washington, DC 20500");
        //marker.icon = Test.getTextIcon(binding.map.context.resources, "Text")
        val fuelStationMarker = FuelStationMarker(binding.map.context.resources, "Orlen")
        marker.icon = BitmapDrawable(binding.map.context.resources, fuelStationMarker.toBitmap(200, 200))
        marker.position = centerOfPoland
        marker.setAnchor(Marker.ANCHOR_CENTER, Marker.ANCHOR_BOTTOM)
        binding.map.overlays.add(marker)
        binding.map.invalidate()
    }

    private fun getTextDrawable(): ShapeDrawable {
        var shape: Shape = object : Shape() {
            override fun draw(canvas: Canvas?, paint: Paint?) {
                paint?.color = Color.BLUE
                paint?.textSize = 100F
                val radii = 50
                if (paint != null) {
                    canvas?.drawCircle((canvas.width - radii * 2).toFloat(), (canvas.height / 2 - radii).toFloat(), radii.toFloat(), paint)
                    paint.color = Color.WHITE
                    canvas?.drawText("Hello Canvas", (canvas.width - 150).toFloat(), (canvas.height / 2).toFloat(), paint)
                }
            }
        }
        return ShapeDrawable(shape)
    }



    private fun configureToolbar() {
        binding.toolbar.setNavigationIcon(androidx.appcompat.R.drawable.abc_ic_ab_back_material)
        binding.toolbar.setOnClickListener { findNavController().popBackStack() }
    }

    private fun configureMapView() {
        val configuration: IConfigurationProvider = getInstance()
        configuration.load(requireContext(), PreferenceManager.getDefaultSharedPreferences(requireContext()))
        configuration.userAgentValue = requireContext().packageName;

        binding.map.setTileSource(TileSourceFactory.DEFAULT_TILE_SOURCE)
        binding.map.setMultiTouchControls(true)
        binding.map.setScrollableAreaLimitLatitude(
            MapView.getTileSystem().maxLatitude, MapView.getTileSystem().minLatitude, 0)
        binding.map.isVerticalMapRepetitionEnabled = false
        binding.map.minZoomLevel = MIN_ZOOM
        binding.map.maxZoomLevel = MAX_ZOOM
        binding.map.zoomController.setVisibility(CustomZoomButtonsController.Visibility.NEVER)
    }

    private fun initLocationOverly() {
        val provider = GpsMyLocationProvider(requireContext())
        provider.addLocationSource(LocationManager.NETWORK_PROVIDER)

        locationOverlay = MyLocationNewOverlay(provider, binding.map)
        locationOverlay.enableMyLocation()
        locationOverlay.enableFollowLocation()
        locationOverlay.isDrawAccuracyEnabled = true
        binding.map.overlays.add(locationOverlay)
    }

    private fun centerMapViewOnFixedPoint() {
        val mapController: IMapController = binding.map.controller
        mapController.setCenter(centerOfPoland)
        mapController.setZoom(INITIAL_ZOOM)
    }

    private fun animateMapToLocation() =
        if (allPermissionsGranted(requireContext(), optionalPermissions) && isGpsEnabled()) animateMapToUserLocation()
        else animateMapToFixedLocation()

    private fun animateMapToUserLocation() {
        if (!locationOverlay.isMyLocationEnabled) locationOverlay.enableMyLocation()

        val mapController: IMapController = binding.map.controller

        locationOverlay.runOnFirstFix { requireActivity().runOnUiThread {
            mapController.animateTo(locationOverlay.myLocation, USER_LOCATION_INITIAL_ZOOM, 1000)
        } }
    }

    private fun animateMapToFixedLocation() {
        val mapController: IMapController = binding.map.controller
        mapController.animateTo(centerOfPoland, INITIAL_ZOOM, 1000)
    }

    private fun askToEnableGps() {
        if (isGpsEnabled()) {
            return
        }


        AlertDialog.Builder(requireContext())
            .setMessage(R.string.ask_to_enable_gps)
            .setNegativeButton(R.string.no_thanks, null)
            .setPositiveButton(R.string.ok) { _, _ ->
                startActivity(Intent(Settings.ACTION_LOCATION_SOURCE_SETTINGS))
            }
            .show()
    }

    private fun isGpsEnabled(): Boolean {
        val locationManager = requireContext().getSystemService(Context.LOCATION_SERVICE) as LocationManager
        return locationManager.isProviderEnabled(LocationManager.GPS_PROVIDER)
                || locationManager.isProviderEnabled(LocationManager.NETWORK_PROVIDER)
    }

    override fun onResume() {
        super.onResume()
        binding.map.onResume()
        requireActivity().registerReceiver(gpsSwitchStateReceiver, IntentFilter(LocationManager.PROVIDERS_CHANGED_ACTION))
    }

    override fun onPause() {
        super.onPause()
        binding.map.onPause()
    }

    override fun onDestroy() {
        super.onDestroy()
        requireActivity().unregisterReceiver(gpsSwitchStateReceiver)
    }
}