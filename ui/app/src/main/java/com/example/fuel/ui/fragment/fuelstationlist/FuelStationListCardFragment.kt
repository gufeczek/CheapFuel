package com.example.fuel.ui.fragment.fuelstationlist

import android.os.Bundle
import android.view.LayoutInflater
import android.view.Menu
import android.view.View
import android.view.ViewGroup
import android.widget.PopupMenu
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import androidx.navigation.Navigation
import com.example.fuel.R
import com.example.fuel.databinding.FragmentFuelStationListCardBinding
import com.example.fuel.model.SimpleFuelStation
import com.example.fuel.ui.fragment.fuelstation.FuelStationDetailsFragment
import com.example.fuel.utils.calculateDistance
import com.example.fuel.utils.converters.UnitConverter
import com.example.fuel.utils.getUserLocation
import com.example.fuel.utils.isGpsEnabled
import com.example.fuel.viewmodel.FuelStationListViewModel
import com.example.fuel.viewmodel.ViewModelFactory
import com.example.fuel.viewmodel.mediator.SharedFuelStationFilter

class FuelStationListCardFragment(private val fuelStation: SimpleFuelStation) : Fragment() {

    private lateinit var viewModel: FuelStationListViewModel
    private lateinit var binding: FragmentFuelStationListCardBinding

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {

        viewModel = ViewModelProvider(requireActivity(), ViewModelFactory())[FuelStationListViewModel::class.java]
        binding = FragmentFuelStationListCardBinding.inflate(inflater, container, false)

        initWithData()
        initPopupMenu()
        initOnClickListener()

        return binding.root
    }

    private fun initWithData() {
        binding.tvFuelStationName.text = fuelStation.stationChainName
        binding.tvFuelPrice.text = viewModel.parsePrice(fuelStation.price, resources)
        binding.tvFuelPriceLastUpdateDate.text = viewModel.parseFuelPriceLastUpdate(fuelStation.lastFuelPriceUpdate, resources)
        setDistance(fuelStation.latitude, fuelStation.longitude)
    }

    private fun setDistance(latitude: Double, longitude: Double) {
        val textView = binding.tvDistanceBetweenUserAndStation

        if (!isGpsEnabled(requireContext()) || getUserLocation(requireContext()) == null) {
            textView.visibility = View.GONE
            return
        }

        val location = getUserLocation(requireContext()) ?: return
        val distance = calculateDistance(location.latitude, location.longitude, latitude, longitude)

        textView.text =  resources.getString(R.string.from_you, UnitConverter.fromMetersToTarget(distance.toDouble()))
        textView.visibility = View.VISIBLE
    }

    private fun initOnClickListener() {
        binding.clMainContainer.setOnClickListener {
            showFuelStationDetails()
        }
    }

    private fun initPopupMenu() {
        val actionButton = binding.acibFuelStationActionButton

        actionButton.setOnClickListener {
            val popupMenu = PopupMenu(requireActivity(), actionButton)
            initPopupMenuWithCommonActions(popupMenu)

            popupMenu.show()
        }
    }

    private fun initPopupMenuWithCommonActions(popupMenu: PopupMenu) {
        val detailsItem = popupMenu.menu.add(Menu.NONE, Menu.NONE, 0, getString(R.string.details))
        val showOnMapItem = popupMenu.menu.add(Menu.NONE, Menu.NONE, 1, getString(R.string.show_on_map))

        detailsItem.setOnMenuItemClickListener {
            showFuelStationDetails()
            true
        }

        showOnMapItem.setOnMenuItemClickListener {
            showOnMap()
            true
        }
    }

    private fun showFuelStationDetails() {
        val bundle = Bundle()
        bundle.putLong("fuelStationId", fuelStation.id)

        val fuelStationDetails = FuelStationDetailsFragment()
        fuelStationDetails.arguments = bundle
        fuelStationDetails.show(requireFragmentManager(), FuelStationDetailsFragment.TAG)
    }

    private fun showOnMap() {
        val bundle = Bundle()
        bundle.putDouble("lat", fuelStation.latitude)
        bundle.putDouble("lon", fuelStation.longitude)

        SharedFuelStationFilter.setFuelTypeId(viewModel.getFuelTypeId())

        Navigation.findNavController(binding.root).navigate(R.id.mapFragment, bundle)
    }
}