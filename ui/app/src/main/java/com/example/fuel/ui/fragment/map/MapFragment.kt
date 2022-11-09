package com.example.fuel.ui.fragment.map

import android.Manifest
import android.app.AlertDialog
import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent
import android.content.IntentFilter
import android.graphics.Color
import android.graphics.drawable.BitmapDrawable
import android.location.Address
import android.location.Geocoder
import android.location.LocationManager
import android.os.Bundle
import android.provider.Settings
import androidx.preference.PreferenceManager
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.Menu
import android.view.MenuInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.activity.result.contract.ActivityResultContracts
import androidx.appcompat.app.AppCompatActivity
import androidx.appcompat.widget.SearchView
import androidx.core.graphics.drawable.toBitmap
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
    private lateinit var geocoder: Geocoder

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
        setHasOptionsMenu(true)
        binding = FragmentMapBinding.inflate(inflater, container, false)
        binding.fabCenter.setOnClickListener {
            askToEnableGps()
            animateMapToUserLocation()
        }

        requestPermissionsResultLauncher.launch(optionalPermissions.plus(requiredPermissions))

        configureMapView()
        initLocationOverly()
        centerMapViewOnFixedPoint()
        addMarkers()

        geocoder = Geocoder(requireContext())

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

    override fun onAttach(context: Context) {
        super.onAttach(context)

        val appActivity = (activity as AppCompatActivity)
        if (!appActivity.supportActionBar?.isShowing!!) {
            appActivity.supportActionBar?.show()
        }
    }

    private fun centerMapViewOnFixedPoint() {
        val mapController: IMapController = binding.map.controller
        mapController.setCenter(centerOfPoland)
        mapController.setZoom(INITIAL_ZOOM)
    }

    private fun animateMapToLocation() =
        if (allPermissionsGranted(requireContext(), optionalPermissions) && isGpsEnabled()) animateMapToUserLocation()
        else animateMapToStartLocation(centerOfPoland)

    private fun animateMapToUserLocation() {
        if (!locationOverlay.isMyLocationEnabled) {
            locationOverlay.enableMyLocation()
            locationOverlay.enableFollowLocation()
        }

        val mapController: IMapController = binding.map.controller

        locationOverlay.runOnFirstFix { requireActivity().runOnUiThread {
            mapController.animateTo(locationOverlay.myLocation, USER_LOCATION_INITIAL_ZOOM, 1000)
        } }
    }

    private fun animateMapToStartLocation(geoPoint: GeoPoint) {
        val mapController: IMapController = binding.map.controller
        mapController.animateTo(geoPoint, INITIAL_ZOOM, 1000)
    }

    private fun animateMapToAddress(address: Address) {
        if (locationOverlay.isFollowLocationEnabled) locationOverlay.disableFollowLocation()

        val location = GeoPoint(address.latitude, address.longitude)

        val mapController: IMapController = binding.map.controller
        mapController.animateTo(location, USER_LOCATION_INITIAL_ZOOM, 1000)
    }

    private fun searchForLocation(searchPhrase: String) {
        val addresses = geocoder.getFromLocationName(searchPhrase, 1)

        if (addresses.size > 0) {
            animateMapToAddress(addresses.get(0))
        } else {
            val toast = Toast.makeText(requireContext(), "Nothing found", Toast.LENGTH_SHORT)
            toast.show()
        }
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

    override fun onCreateOptionsMenu(menu: Menu, inflater: MenuInflater) {
        inflater.inflate(R.menu.options_menu, menu)
        val searchItem = menu.findItem(R.id.search)
        val searchView = searchItem.actionView as SearchView

        searchView.apply {
            isIconified = false
            setOnQueryTextListener(object : SearchView.OnQueryTextListener{
                override fun onQueryTextChange(newText: String?): Boolean {
                    return false
                }

                override fun onQueryTextSubmit(query: String?): Boolean {
                    if (query != null) {
                        searchForLocation(query)
                    }

                    return false;
                }
            })
        }
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