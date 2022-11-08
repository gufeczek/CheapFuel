package com.example.fuel.ui.fragment.map

import android.Manifest
import android.app.AlertDialog
import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent
import android.content.IntentFilter
import android.graphics.Color
import android.graphics.drawable.BitmapDrawable
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
import com.example.fuel.mock.getFuelStations
import com.example.fuel.model.SimpleMapFuelStation
import com.example.fuel.ui.utils.drawable.FuelStationMarker
import com.example.fuel.ui.utils.permission.allPermissionsGranted
import org.osmdroid.api.IMapController
import org.osmdroid.bonuspack.clustering.RadiusMarkerClusterer
import org.osmdroid.config.Configuration.getInstance
import org.osmdroid.config.IConfigurationProvider
import org.osmdroid.tileprovider.tilesource.TileSourceFactory
import org.osmdroid.util.GeoPoint
import org.osmdroid.views.CustomZoomButtonsController
import org.osmdroid.views.MapView
import org.osmdroid.views.overlay.Marker
import org.osmdroid.views.overlay.mylocation.GpsMyLocationProvider
import org.osmdroid.views.overlay.mylocation.MyLocationNewOverlay

private const val INITIAL_ZOOM = 7.0;
private const val USER_LOCATION_INITIAL_ZOOM = 15.0
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
        binding.toolbar.setOnClickListener { findNavController().popBackStack() }

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
        val cluster = RadiusMarkerClusterer(requireContext())
        cluster.setRadius(85)
        cluster.mTextAnchorU = 0.70F
        cluster.mTextAnchorV = 0.27F
        cluster.textPaint.textSize = 14F

        val fuelStations: Array<SimpleMapFuelStation> = getFuelStations()
        fuelStations.forEach { fuelStation -> cluster.add(buildMarker(fuelStation)) }

        binding.map.overlays.add(cluster)
        binding.map.invalidate()
    }

    private fun buildMarker(fuelStation: SimpleMapFuelStation): Marker {
        val marker = Marker(binding.map)
        val fuelStationMarker = FuelStationMarker(binding.map.context.resources, fuelStation.parsePrice(), Color.GRAY, false)
        marker.icon = BitmapDrawable(binding.map.context.resources, fuelStationMarker.toBitmap(600, 200))
        marker.position = GeoPoint(fuelStation.latitude, fuelStation.longitude)
        marker.setInfoWindow(null)
        marker.setAnchor(Marker.ANCHOR_CENTER, Marker.ANCHOR_BOTTOM)
        return marker
    }

    private fun configureToolbar() {
        binding.toolbar.setNavigationIcon(androidx.appcompat.R.drawable.abc_ic_ab_back_material)
        binding.toolbar.setOnClickListener { findNavController().popBackStack() }
    }

    private fun configureMapView() {
        val configuration: IConfigurationProvider = getInstance()
        configuration.load(requireContext(), PreferenceManager.getDefaultSharedPreferences(requireContext()))
        configuration.userAgentValue = requireContext().packageName;

        binding.map.setTileSource(TileSourceFactory.WIKIMEDIA)
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